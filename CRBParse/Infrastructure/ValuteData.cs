using System.Collections.Generic;
using System.Xml.Serialization;

namespace CRBParse.Infrastructure
{
    [XmlRoot("ValuteData")]
    public class ValuteData
    {
        [XmlElement("ValuteCursOnDate")]
        public List<ValuteCursOnDate> ValuteCursOnDate { get; set; }
    }

    public class ValuteCursOnDate
    {
        public string Vname { get; set; }
        public int Vnom { get; set; }
        public decimal Vcurs { get; set; }
        public string Vcode { get; set; }
        public string VchCode { get; set; }
    }
}