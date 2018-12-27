using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HansHomePage.Models;

namespace HansHomePage.Controllers
{
    public class MemberController : Controller
    {
        HansHomePageEntities db = new HansHomePageEntities();

        [HttpGet]
        public ActionResult Entry()
        {
            Members members = new Members();
            return View(members);
        }

        [HttpPost]
        public ActionResult Entry(Members members)
        {
            members.EntryDate = DateTime.Now;
            try
            {
                db.Members.Add(members);
                db.SaveChanges();
                ViewBag.Result = "OK";
            } catch(Exception ex)
            {
                ViewBag.Result = "FAIL";
            }
            return RedirectToAction("Index", "Test");
            //return View(members);
        }

        public JsonResult IDCheck(Members members)
        {
            string result = string.Empty;
            Members dbMembers = db.Members.Find(members.MemberID);

            if (dbMembers == null)
            {
                result = "OK";
            } else
            {
                result = "FAIL";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PWDCheck(Members members)
        {
            string result = string.Empty;
            Members dbMembers = db.Members.Find(members.MemberID);

            if (dbMembers != null && dbMembers.MemberPWD == members.MemberPWD)
            {
                result = "OK";
            }
            else
            {
                result = "FAIL";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult List()
        {
            List<Members> list = db.Members.OrderByDescending(x => x.EntryDate).ToList();
            return View(list);
        }

        [HttpGet]
        public ActionResult Edit(string memberId)
        {
            Members members = db.Members.Where(x => x.MemberID == memberId).FirstOrDefault();
            return View(members);
        }

        [HttpPost]
        public ActionResult Edit(Members members)
        {
            Members dbMembers = db.Members.Find(members.MemberID);
            try
            {
                dbMembers.MemberName = members.MemberName;
                dbMembers.MemberPWD = members.MemberPWD;
                dbMembers.Email = members.Email;
                dbMembers.Telephone = members.Telephone;

                db.Entry(dbMembers).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Result = "OK";
            }
            catch (Exception ex)
            {
                ViewBag.Result = "FAIL";
            }
            return RedirectToAction("List");
            //return View(dbMembers);
        }

        [HttpGet]
        public ActionResult Delete(string memberId)
        {
            Members dbMembers = db.Members.Find(memberId);
            db.Members.Remove(dbMembers);
            db.SaveChanges();
            return RedirectToAction("List");
            // return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult Login(string memberId)
        {
            Members members = db.Members.Where(x => x.MemberID == memberId).FirstOrDefault();
            return View(members);
        }

        [HttpPost]
        public ActionResult Login(Members members)
        {
            Members dbMembers = db.Members.Find(members.MemberID);
            
            if (members.MemberID == dbMembers.MemberID && 
                members.MemberPWD == dbMembers.MemberPWD)
            {
                Session["ID"] = members.MemberID;                
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Test");
        }

        public ActionResult Logout()
        {
            try
            {
                Session.Remove("ID");
                return RedirectToAction("Index", "Test");
            }
            catch
            {
                throw;
            }
        }

        public ActionResult Index()
        {
            if (Session["ID"] != null)
            {
                return RedirectToAction("Index", "Test");             
            }
            else
            {
                return RedirectToAction("Login", "Member");
            }
            //return View();
        }
    }
}