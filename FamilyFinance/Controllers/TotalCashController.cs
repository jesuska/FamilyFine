using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;

namespace FamilyFinance.Controllers
{
    public class TotalCashController : Controller
    {
        private FinanceDB db = new FinanceDB();

        //
        // GET: /TotalCash/

        public ActionResult Index()
        {
            return View(db.TotalCashes.OrderByDescending(t => t.Date).ToList());
        }

        //
        // GET: /TotalCash/Details/5

        public ActionResult Details(int id = 0)
        {
            TotalCash totalcash = db.TotalCashes.Find(id);
            if (totalcash == null)
            {
                return HttpNotFound();
            }
            return View(totalcash);
        }

        //
        // GET: /TotalCash/Create

        public ActionResult Create()
        {
            object o = HttpContext.Session["lastCreateDate"];
            DateTime date = (o == null ? DateTime.Now : (DateTime)o);
            ViewBag.LastCreateDate = date.ToString("yyyy-MM-dd");

            return View();
        }

        //
        // POST: /TotalCash/Create

        [HttpPost]
        public ActionResult Create(TotalCash totalcash)
        {
            if (ModelState.IsValid)
            {
                db.TotalCashes.Add(totalcash);
                db.SaveChanges();

                HttpContext.Session["lastCreateDate"] = totalcash.Date;

                return RedirectToAction("Index");
            }

            object o = HttpContext.Session["lastCreateDate"];
            DateTime date = (o == null ? DateTime.Now : (DateTime)o);
            ViewBag.LastCreateDate = date.ToString("yyyy-MM-dd");

            return View(totalcash);
        }

        //
        // GET: /TotalCash/Edit/5

        public ActionResult Edit(int id = 0)
        {
            TotalCash totalcash = db.TotalCashes.Find(id);
            if (totalcash == null)
            {
                return HttpNotFound();
            }
            return View(totalcash);
        }

        //
        // POST: /TotalCash/Edit/5

        [HttpPost]
        public ActionResult Edit(TotalCash totalcash)
        {
            if (ModelState.IsValid)
            {
                db.Entry(totalcash).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(totalcash);
        }

        //
        // GET: /TotalCash/Delete/5

        public ActionResult Delete(int id = 0)
        {
            TotalCash totalcash = db.TotalCashes.Find(id);
            if (totalcash == null)
            {
                return HttpNotFound();
            }
            return View(totalcash);
        }

        //
        // POST: /TotalCash/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            TotalCash totalcash = db.TotalCashes.Find(id);
            db.TotalCashes.Remove(totalcash);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}