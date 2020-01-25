using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RFIDLogger.Controllers
{
    public class HomeController : Controller
    {
        RFID_DBEntities db = new RFID_DBEntities();

        public ActionResult Index()
        {
            List<EntryLog> entryLogs = db.EntryLog.OrderByDescending(x => x.IDEntry).ToList();

            if (entryLogs.Count < 100)
                return View(entryLogs);
            else
                return View(entryLogs.GetRange(0, 100));
        }

        public ActionResult PVLive()
        {
            IOrderedQueryable<EntryLog> entryLogs = db.EntryLog.OrderByDescending(x => x.IDEntry);

            if (entryLogs.Count() < 100)
                return PartialView(entryLogs.ToList());
            else
                return PartialView(entryLogs.ToList().GetRange(0, 100));
        }

        public ActionResult FilteredView()
        {
            List<RfIdReader> rfIdReaders = db.RfIdReader.ToList();
            ViewBag.Message = "Filterrrrr";

            return View(rfIdReaders);
        }

        public ActionResult PVFiltered(string startDate, string endDate, string name, string surname, string location)
        {
            DateTime dtStart = GetDateFromString(startDate, true);
            DateTime dtEnd = GetDateFromString(endDate, false);

            IQueryable<EntryLog> entryLogs = db.EntryLog.Where(x => x.EntryTime >= dtStart && x.EntryTime <= dtEnd);

            if (name != null && name != "")
                entryLogs = entryLogs.Where(x => x.RfIdUser.User.Name.ToLower().Contains(name.Trim().ToLower()));

            if (surname != null && surname != "")
                entryLogs = entryLogs.Where(x => x.RfIdUser.User.Surname.ToLower().Contains(surname.Trim().ToLower()));

            if (location != null && location != "")
                entryLogs = entryLogs.Where(x => x.RfIdReader.Location.ToLower().Contains(location.Trim().ToLower()));

            return PartialView(entryLogs.OrderByDescending(x => x.IDEntry).ToList());
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private DateTime GetDateFromString(string date, bool isStartDate)
        {
            try
            {
                string[] tokens = date.Split('.');
                return new DateTime(int.Parse(tokens[2]), int.Parse(tokens[1]), int.Parse(tokens[0]));
            }
            catch (Exception)
            {
                return isStartDate ? DateTime.MinValue : DateTime.Now;
            }
        }
    }
}