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
            List<EntryLog> entryLogs = db.EntryLog.OrderByDescending(x => x.IDEntry).ToList();

            if (entryLogs.Count < 100)
                return PartialView(entryLogs);
            else
                return PartialView(entryLogs.GetRange(0, 100));
        }

        public ActionResult FilteredView()
        {
            List<RfIdReader> rfIdReaders = db.RfIdReader.ToList();
            ViewBag.Message = "Filterrrrr";

            return View(rfIdReaders);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}