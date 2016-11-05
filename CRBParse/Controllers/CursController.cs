using CRBParse.Models;
using CRBParse.ViewModels;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CRBParse.Controllers
{
    public class CursController : Controller
    {
        private CBRParseContext db = new CBRParseContext();

        // GET: Curs
        public ActionResult Index()
        {
            List<Curs> curses = db.Database.SqlQuery<Curs>("GetCurses").ToList();

            List<Curs> currentCurses = curses.Take(2).ToList();

            Curs currentCurse = currentCurses[0].IsApply? currentCurses[0] : currentCurses[1];

            CursViewModel cursVm = new CursViewModel
            {
                CurrentCursDate = currentCurses[0].CursDate,
                CurrentDollarValue = currentCurse.DollarValue,
                CurrentEuroValue = currentCurse.EuroValue,
                Curses = curses
            };

            return View(cursVm);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
