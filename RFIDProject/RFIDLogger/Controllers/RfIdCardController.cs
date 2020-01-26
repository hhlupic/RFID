using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RFIDLogger.Controllers
{
    public class RfIdCardController : Controller
    {
        RFID_DBEntities db = new RFID_DBEntities();

        public ActionResult Index()
        {
            ViewBag.Users = db.RfIdUser.ToList();

            List<RfIdUser> users = ViewBag.Users;
            foreach (var user in users)
            {
                String name = user.User.DisplayedName;
                Console.WriteLine(name);
            }

                return View(db.RfIdCard);
        }

        public ActionResult Create()
        {
            return View("CreateCard");
        }

        [HttpPost]
        public ActionResult Create(RfIdCard rfIdCard)
        {
            try
            {
                db.RfIdCard.Add(new RfIdCard
                {
                    SerialNumber = rfIdCard.SerialNumber
                });
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            return View("EditCard", db.RfIdCard.Where(x => x.IDCard == id).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult Edit(int id, RfIdCard rfIdCard)
        {
            try
            {
                RfIdCard updatedrfIdCard = db.RfIdCard.Where(x => x.IDCard == id).FirstOrDefault();
                updatedrfIdCard.SerialNumber = rfIdCard.SerialNumber;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
