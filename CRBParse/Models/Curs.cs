using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Model
{
    public class Curs
    {
        public int ID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CursDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.####}")]
        public decimal EuroValue { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.####}")]
        public decimal DollarValue { get; set; }
        public bool IsApply { get; set; }
    }
}
