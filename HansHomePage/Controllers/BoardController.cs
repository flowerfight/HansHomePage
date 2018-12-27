using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using HansHomePage.Models;
using System.Data.Entity;

namespace HansHomePage.Controllers
{
    public class BoardController : Controller
    {
        HansHomePageEntities db = new HansHomePageEntities();

        [HttpGet]
        public ActionResult Create()
        {
            Articles article = new Articles();
            return View(article);
        }

        [HttpPost]
        public ActionResult Create(Articles article)
        {
            try
            {
                article.ViewCnt = 0;
                article.IPAddress = Request.ServerVariables["REMOTE_ADDR"].ToString();
                article.RegistDate = DateTime.Now;
                article.RegistUserName = Session["ID"].ToString();
                article.ModifyDate = DateTime.Now;
                article.ModifyUserName = Session["ID"].ToString();

                db.Articles.Add(article);
                db.SaveChanges();

                if (Request.Files.Count > 0)
                {
                    var attachFile = Request.Files[0];

                    if (attachFile != null && attachFile.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(attachFile.FileName);
                        var path = Path.Combine(Server.MapPath("~/Upload/"), fileName);
                        attachFile.SaveAs(path);

                        ArticleFiles file = new ArticleFiles();
                        file.ArticleIDX = article.ArticleIDX;
                        file.FilePath = "/Upload/";
                        file.FileName = fileName;
                        file.FileFormat = Path.GetExtension(attachFile.FileName);
                        file.FileSize = attachFile.ContentLength;
                        file.UploadDate = DateTime.Now;
                        db.ArticleFiles.Add(file);
                        db.SaveChanges();
                    }
                }
                ViewBag.Result = "OK";
            }
            catch (Exception ex)
            {
                ViewBag.Result = "FAIL";
            }
            //return View(article);
            return RedirectToAction("ArticleList");
        }

        [HttpGet]
        public ActionResult ArticleList()
        {
            ArticleEditViewModel vm = new ArticleEditViewModel
            {
                ArticlesList = db.Articles.OrderByDescending(o => o.RegistDate).ToList(),
                Comments = db.ArticleComments.OrderByDescending(o => o.RegistDate).ToList()
            };
            return View(vm);
        }

        [HttpGet]
        public ActionResult Edit(int aidx)
        {
            ArticleEditViewModel vm = new ArticleEditViewModel();

            Articles article = db.Articles.Where(c => c.ArticleIDX == aidx).FirstOrDefault();

            List<ArticleFiles> files = db.ArticleFiles.Where(c => c.ArticleIDX == aidx).OrderBy(o => o.UploadDate).ToList();
            vm.Articles = article;
            vm.Files = files;

            return View(vm);
        }

        [HttpPost]
        public ActionResult Edit(ArticleEditViewModel vm)
        {
            ArticleEditViewModel dbVM = new ArticleEditViewModel();
            try
            {
                Articles dbArticle = db.Articles.Find(vm.Articles.ArticleIDX);

                dbArticle.Title = vm.Articles.Title;
                dbArticle.ArticleType = vm.Articles.ArticleType;
                dbArticle.Contents = vm.Articles.Contents;
                dbArticle.IPAddress = Request.ServerVariables["REMOTE_ADDR"].ToString();
                dbArticle.ModifyDate = DateTime.Now ;
                dbArticle.ModifyUserName = Session["ID"].ToString();

                db.Entry(dbArticle).State = EntityState.Modified;
                db.SaveChanges();

                if (Request.Files.Count > 0)
                {
                    var attachFile = Request.Files[0];

                    if(attachFile != null && attachFile.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(attachFile.FileName);
                        var path = Path.Combine(Server.MapPath("~/Upload/"), fileName);
                        attachFile.SaveAs(path);

                        ArticleFiles file = new ArticleFiles();
                        file.ArticleIDX = vm.Articles.ArticleIDX;
                        file.FilePath = "/Upload/";
                        file.FileName = fileName;
                        file.FileFormat = Path.GetExtension(attachFile.FileName);
                        file.FileSize = attachFile.ContentLength;
                        file.UploadDate = DateTime.Now;
                        db.ArticleFiles.Add(file);
                        db.SaveChanges();
                    }
                }

                Articles article = db.Articles.Where(c => c.ArticleIDX == vm.Articles.ArticleIDX).FirstOrDefault();
                List<ArticleFiles> files = db.ArticleFiles.Where(c => c.ArticleIDX == vm.Articles.ArticleIDX)
                    .OrderBy(o => o.UploadDate).ToList();

                dbVM.Articles = article;
                dbVM.Files = files;
                ViewBag.Result = "OK";
            }
            catch (Exception ex)
            {
                dbVM = vm;
                ViewBag.Result = "FAIL";
            }
            return RedirectToAction("ArticleList");
            //return View(dbVM);
        }

        [HttpGet]
        public ActionResult ArticleDelete(int aidx)
        {
            Articles dbArticle = db.Articles.Where(c => c.ArticleIDX == aidx).FirstOrDefault();
            db.Articles.Remove(dbArticle);
            db.SaveChanges();

            return RedirectToAction("ArticleList");
        }

        [HttpGet]
        public ActionResult FileRemove(int fidx)
        {
            ArticleFiles file = db.ArticleFiles.Where(c => c.FileIDX == fidx).FirstOrDefault();
            int articleIDX = Convert.ToInt32(file.ArticleIDX);

            System.IO.File.Delete(Server.MapPath(file.FilePath + file.FileName));

            db.ArticleFiles.Remove(file);
            db.SaveChanges();

            return RedirectToAction("Edit", new { aidx = articleIDX.ToString() });
        }

        [HttpGet]
        public ActionResult Read(int aidx)
        {
            ArticleEditViewModel vm = new ArticleEditViewModel();

            Articles article = db.Articles.Where(c => c.ArticleIDX == aidx).FirstOrDefault();
            List<ArticleFiles> files = db.ArticleFiles.Where(c => c.ArticleIDX == aidx).OrderBy(o => o.UploadDate).ToList();
            vm.Articles = article;
            vm.Files = files;
            article.ViewCnt = vm.Articles.ViewCnt + 1;
            vm.Comments = db.ArticleComments.Where(c => c.ArticleIDX == aidx).ToList();
            db.Entry(article).State = EntityState.Modified;
            db.SaveChanges();

            return View(vm);
            //return RedirectToAction("Read");
        }

        [HttpPost]
        public JsonResult Comments(ArticleComments articleComments)
        {
            string result = string.Empty;

            articleComments.MemberID = Session["ID"].ToString();
            articleComments.IPAddress = Request.ServerVariables["REMOTE_ADDR"].ToString();
            articleComments.RegistDate = DateTime.Now;

            if (articleComments.MemberID != null && articleComments.Comments != null)
            {
                result = "OK";
                db.ArticleComments.Add(articleComments);
                db.SaveChanges();
            }
            else
            {
                result = "FAIL";
            }            

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CommentsDelete(int cidx)
        {
            ArticleComments articleComments = db.ArticleComments.Where(c 
                => c.CommentsIDX == cidx).FirstOrDefault();
            int? aidx = articleComments.ArticleIDX;
            db.ArticleComments.Remove(articleComments);
            db.SaveChanges();

            return RedirectToAction("Read", "Board", new { aidx });
        }

        // GET: Board
        public ActionResult Index()
        {
            return View();
        }
    }
}