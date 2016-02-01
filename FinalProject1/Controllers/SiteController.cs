using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using FinalProject1.Models;

namespace FinalProject1.Controllers
{
    public class SiteController : Controller
    {
        private FinalProject1Context db = new FinalProject1Context();

        // GET: /Site/
        public ActionResult Index()
        {
            return View(db.Sites.ToList());
        }

        public ActionResult Cal()
        {
            XDocument doc = XDocument.Load(@"C:\Users\Apple\Desktop\upload，delete，googlemap,halfcalendar 2 - 副本\FinalProject1\Content\image\Events.xml");
            var q = from x in doc.Elements("CATALOG").Elements("basketball").Elements("Day") select x;
            ViewBag.Date=q.First().Value;
            q = from x in doc.Elements("CATALOG").Elements("football").Elements("Day") select x;
            ViewBag.Date1 = q.First().Value;           
            q = from x in doc.Elements("CATALOG").Elements("lecture").Elements("Day") select x;
            ViewBag.Date2 = q.First().Value;            
            q = from x in doc.Elements("CATALOG").Elements("volunteer").Elements("Day") select x;
            ViewBag.Date3 = q.First().Value;
            q = from x in doc.Elements("CATALOG").Elements("concert").Elements("Day") select x;
            ViewBag.Date4 = q.First().Value;
            q = from x in doc.Elements("CATALOG").Elements("graduation").Elements("Day") select x;
            ViewBag.Date5 = q.First().Value;
            q = from x in doc.Elements("CATALOG").Elements("basketball").Elements("image") select x;
            ViewBag.image = q.First().Value;
            q = from x in doc.Elements("CATALOG").Elements("football").Elements("image") select x;
            ViewBag.image1 = q.First().Value;
            q = from x in doc.Elements("CATALOG").Elements("lecture").Elements("image") select x;
            ViewBag.image2 = q.First().Value;
            q = from x in doc.Elements("CATALOG").Elements("volunteer").Elements("image") select x;
            ViewBag.image3 = q.First().Value;
            q = from x in doc.Elements("CATALOG").Elements("concert").Elements("image") select x;
            ViewBag.image4 = q.First().Value;
            q = from x in doc.Elements("CATALOG").Elements("graduation").Elements("image") select x;
            ViewBag.image5 = q.First().Value;
            q = from x in doc.Elements("CATALOG").Elements("basketball").Elements("description") select x;
            ViewBag.des = q.First().Value;
            q = from x in doc.Elements("CATALOG").Elements("football").Elements("description") select x;
            ViewBag.des1 = q.First().Value;
            q = from x in doc.Elements("CATALOG").Elements("lecture").Elements("description") select x;
            ViewBag.des2 = q.First().Value;
            q = from x in doc.Elements("CATALOG").Elements("volunteer").Elements("description") select x;
            ViewBag.des3 = q.First().Value;
            q = from x in doc.Elements("CATALOG").Elements("concert").Elements("description") select x;
            ViewBag.des4 = q.First().Value;
            q = from x in doc.Elements("CATALOG").Elements("graduation").Elements("description") select x;
            ViewBag.des5 = q.First().Value;
            return View(db.Sites.ToList());
        }

        public ActionResult AJAX()
        {
            return View();
        }
        // GET: /Site/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Site site = db.Sites.Find(id);
            if (site == null)
            {
                return HttpNotFound();
            }
            return View(site);
        }

        public ActionResult HomePage()
        {
            return View(db.Sites.ToList());
        }
        // GET: /Site/Create


        public ActionResult Syracuse()
        {
            return View();
        }
        public ActionResult Catalog()
        {
 
            ((List<Site>)Session["SiteMap"]).Clear();
            return View(db.Sites.ToList());
        }
      
        public ActionResult AddToCart(string sitename="")
        {
            Site site = db.Sites.First(Site => Site.SiteName == sitename);
            if (site == null)
            {
                return HttpNotFound();
            }
            List<Site> test = (List<Site>)(HttpContext.Session["SiteMap"]);
            int a=0;
            foreach(var item in test)
            {
                if (item.SiteName == sitename)
                    a = 1;
            }
            if(a==0)
            ((List<Site>)(HttpContext.Session["SiteMap"])).Add(site);
            return View("HomePage", db.Sites.ToList());
        }
        public ActionResult StartTour()
        {

            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        
        // POST: /Site/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="SiteId,SiteName,Description,Image")] Site site)
        {
            if (ModelState.IsValid)
            {
                db.Sites.Add(site);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(site);
        }

        // GET: /Site/Edit/5
        [Authorize(Roles="employee")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Site site = db.Sites.Find(id);
            if (site == null)
            {
                return HttpNotFound();
            }
            return View(site);
        }

        // POST: /Site/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="SiteId,SiteName,Description,Image")] Site site)
        {
            if (ModelState.IsValid)
            {
                db.Entry(site).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(site);
        }

        // GET: /Site/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Site site = db.Sites.Find(id);
            if (site == null)
            {
                return HttpNotFound();
            }
            return View(site);
        }

        // POST: /Site/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Site site = db.Sites.Find(id);
            db.Sites.Remove(site);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult googleMap()
        {
            return View();
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
