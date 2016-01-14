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
    public class TransactionController : Controller
    {
        private FinanceDB db = new FinanceDB();

        //
        // GET: /Transaction/

        public ActionResult Index()
        {
            CategoryManager categoryManager = new CategoryManager();
            ViewData["categories"] = categoryManager.GetAllCategories();

            return View(db.Transactions.OrderByDescending(t => t.CreateDate).ThenByDescending(t => t.Id).ToList());
        }

        //
        // GET: /Transaction/Create

        public ActionResult Create()
        {
            CategoryManager categoryManager = new CategoryManager();
            ViewData["categories"] = new SelectList(categoryManager.GetAllCategories(), "Id", "Name", 0);

            object o = HttpContext.Session["lastCreateDate"];
            DateTime date = (o == null ? DateTime.Now : (DateTime)o);
            ViewBag.LastCreateDate = date.ToString("yyyy-MM-dd");

            Transaction trans = new Transaction();
            trans.AffectsMonthlyLimit = true;

            return View(trans);
        }

        //
        // POST: /Transaction/Create

        [HttpPost]
        public ActionResult Create(Transaction transaction)
        {
            CategoryManager categoryManager = new CategoryManager();
            ViewData["categories"] = new SelectList(categoryManager.GetAllCategories(), "Id", "Name", transaction.CategoryId);

            if (ModelState.IsValid)
            {
                if (transaction.CategoryId == 0)
                    transaction.CategoryId = null;
                db.Transactions.Add(transaction);
                db.SaveChanges();

                HttpContext.Session["lastCreateDate"] = transaction.CreateDate;

                return RedirectToAction("Index");
            }

            object o = HttpContext.Session["lastCreateDate"];
            DateTime date = (o == null ? DateTime.Now : (DateTime)o);
            ViewBag.LastCreateDate = date.ToString("yyyy-MM-dd");

            return View(transaction);
        }

        //
        // GET: /Transaction/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            CategoryManager categoryManager = new CategoryManager();
            ViewData["categories"] = new SelectList(categoryManager.GetAllCategories(), "Id", "Name", transaction.CategoryId);
            return View(transaction);
        }

        //
        // POST: /Transaction/Edit/5

        [HttpPost]
        public ActionResult Edit(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                if (transaction.CategoryId == 0)
                    transaction.CategoryId = null;
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(transaction);
        }

        //
        // GET: /Transaction/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }

            CategoryManager categoryManager = new CategoryManager();
            transaction.Category = categoryManager.GetAllCategories().Find(t => t.Id == transaction.CategoryId);
            return View(transaction);
        }

        //
        // POST: /Transaction/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            db.Transactions.Remove(transaction);
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