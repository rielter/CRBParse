using Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CRBParse.ViewModels
{
    public class CursViewModel
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CurrentCursDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.####}")]
        public decimal CurrentEuroValue { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.####}")]
        public decimal CurrentDollarValue { get; set; }

        public List<Curs> Curses { get; set; }
    }
}