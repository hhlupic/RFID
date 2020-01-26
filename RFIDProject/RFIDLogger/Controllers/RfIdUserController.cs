using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RFIDLogger.Controllers
{
    public class RfIdUserController : Controller
    {
        RFID_DBEntities db = new RFID_DBEntities();

        public ActionResult Index()
        {
            return View(db.RfIdUser);
        }

        public ActionResult Details(int id)
        {
            List<RfIdCard> allCards = db.RfIdCard.ToList();
            List<RfIdUser> userList = db.RfIdUser.ToList();
            List<RfIdCard> unusedCards = allCards.Where(x => !userList.Any(y => y.RfIdCard.IDCard == x.IDCard)).ToList();
            ViewBag.UnusedCards = unusedCards;

            return View("UserDetails", db.RfIdUser.Where(x => x.IDRfIdUser == id).FirstOrDefault());
        }

        public ActionResult Create()
        {
            List<RfIdCard> allCards = db.RfIdCard.ToList();
            List<RfIdUser> userList = db.RfIdUser.ToList();
            List<RfIdCard> unusedCards = allCards.Where(x => !userList.Any(y => y.RfIdCard.IDCard == x.IDCard)).ToList();
            ViewBag.UnusedCards = unusedCards;

            return View("CreateUser");
        }

        [HttpPost]
        public ActionResult Create(RfIdUser rfIdUser)
        {
            try
            {
                User newUser = new User
                {
                    Name = rfIdUser.User.Name,
                    Surname = rfIdUser.User.Surname,
                    Address = rfIdUser.User.Address,
                    OIB = rfIdUser.User.OIB
                };
                db.User.Add(newUser);
                db.SaveChanges();

                db.RfIdUser.Add(new RfIdUser
                {
                    CardId = db.RfIdCard.Where(x => x.SerialNumber == rfIdUser.RfIdCard.SerialNumber).FirstOrDefault().IDCard,
                    UserId = newUser.IDUser
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
            List<RfIdCard> allCards = db.RfIdCard.ToList();
            List<RfIdUser> userList = db.RfIdUser.ToList();
            List<RfIdCard> unusedCards = allCards.Where(x => !userList.Any(y => y.RfIdCard.IDCard == x.IDCard)).ToList();
            ViewBag.UnusedCards = unusedCards;

            return View("EditUser", db.RfIdUser.Where(x => x.IDRfIdUser == id).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult Edit(int id, RfIdUser rfIdUser)
        {
            try
            {
                User userToUpdate = db.User.Where(x => x.IDUser == id).FirstOrDefault();
                userToUpdate.Name = rfIdUser.User.Name;
                userToUpdate.Surname = rfIdUser.User.Surname;
                userToUpdate.OIB = rfIdUser.User.OIB;
                userToUpdate.Address = rfIdUser.User.Address;

                RfIdCard rfIdCard = db.RfIdCard.Where(x => x.SerialNumber == rfIdUser.RfIdCard.SerialNumber).FirstOrDefault();
                RfIdUser rfIdUserToUpdate = db.RfIdUser.Where(x => x.IDRfIdUser == rfIdUser.IDRfIdUser).FirstOrDefault();
                rfIdUserToUpdate.CardId = rfIdCard.IDCard;

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
