using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Net;
using InventionDiscovery.Models;
using PagedList;

namespace InventionDiscovery.Controllers
{
    public class HomeController : Controller
    {
        private InventionDbEntities db = new InventionDbEntities();
        // GET: Home
        public ActionResult Index()
        {
            return View(db.Inventions.ToList());
        }

        public ViewResult List(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.InventionSortParm = String.IsNullOrEmpty(sortOrder) ? "invdisc_desc" : "";
            var inv = from i in db.Inventions select i;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            
            if (!String.IsNullOrEmpty(searchString))
            {
                inv = inv.Where(i => i.InventionDiscovery.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "invdesc":
                    inv = inv.OrderByDescending(i => i.InventionDiscovery);
                    break;

                default:
                    inv = inv.OrderBy(i => i.InventionDiscovery);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(inv.ToPagedList(pageNumber, pageSize));
        }
        
        // GET: Home/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invention invention = db.Inventions.Find(id);
            if (invention == null)
            {
                return HttpNotFound();
            }
            return View(invention);
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,InventorName,ImagePath,Year,Country,Biography,InventionDiscovery,Description")]Invention invention)
        {
            if (ModelState.IsValid)
            {
                db.Inventions.Add(invention);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(invention);
        }

        // GET: Home/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invention invention = db.Inventions.Find(id);
            if (invention == null)
            {
                return HttpNotFound();
            }
            return View(invention);
        }

        // POST: Home/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,InventorName,ImagePath,Year,Country,Biography,InventionDiscovery,Description")]Invention invention)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invention).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(invention);
        }

        // GET: Home/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invention invention = db.Inventions.Find(id);
            if (invention == null)
            {
                return HttpNotFound();
            }
            return View(invention);
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invention invention = db.Inventions.Find(id);
            db.Inventions.Remove(invention);
            db.SaveChanges();
            return RedirectToAction("Index");
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
