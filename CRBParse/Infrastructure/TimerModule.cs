using CRBParse.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Xml.Serialization;

namespace CRBParse.Infrastructure
{
    public class TimerModule : IHttpModule
    {
        static Timer timer;
        const long interval = 30000; //30 секунд
        static object synclock = new object();
        static CBR.DailyInfoSoap dailyInfo;
        static CBRParseContext db;
        static bool isGet = false;

        public void Init(HttpApplication app)
        {
            db = new CBRParseContext();
            dailyInfo = new CBR.DailyInfoSoapClient();
            timer = new Timer(new TimerCallback(GetCurs), null, 0, interval);
        }

        private void GetCurs(object obj)
        {
            lock (synclock)
            {
                DateTime now = DateTime.Now;
                // если сейчас 14:00
                if (now.Hour == 14 && now.Minute == 0 && isGet == false)
                {
                    // попытка получения и добавления нового курса в БД
                    while (!AddCurs())
                    {
                        // если попытка неудачна -
                        // ожидаем 5 минут
                        Thread.Sleep(5 * 60 * 1000);
                    }

                    isGet = true;
                }
                else if (now.Hour == 0 && now.Minute == 0)
                {
                    // если сейчас 00:00
                    // изменяем признак применения последнего курса на "true"
                    db.Database.ExecuteSqlCommand("[dbo].[SetCursApply]");
                    isGet = true;
                }
                else
                {
                    isGet = false;
                }
            }
        }

        /// <summary>
        /// Добавление нового курса в БД
        /// </summary>
        /// <returns>'true' при успешном получении и добавлении, 'false' - иначе </returns>
        private bool AddCurs()
        {
            try
            {
                // Последняя дата публикации курсов валют
                DateTime cursDate = dailyInfo.GetLatestDateTime();
                // Получение ежедневных курсов валют
                System.Xml.XmlNode ret_xml = dailyInfo.GetCursOnDateXML(cursDate);

                // десериализация XMLDocument в ValuteData
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(ValuteData));
                ValuteData valuteData = xmlSerializer.Deserialize(new StringReader(ret_xml.OuterXml)) as ValuteData;

                // выборка курса доллара и евро
                List<ValuteCursOnDate> currentValuteData = valuteData.ValuteCursOnDate.Where(v => v.VchCode == "USD" || v.VchCode == "EUR").ToList();

                // добавление нового курса в базу
                db.Database.ExecuteSqlCommand("[dbo].[AddCurs] @cursDate, @euroValue, @dollarValue",
                    new SqlParameter("@cursDate", cursDate),
                    new SqlParameter("@euroValue", currentValuteData.Where(v => v.VchCode == "EUR").Select(v => v.Vcurs).FirstOrDefault()),
                    new SqlParameter("@dollarValue", currentValuteData.Where(v => v.VchCode == "USD").Select(v => v.Vcurs).FirstOrDefault())
                    );
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Dispose()
        { }
    }
}