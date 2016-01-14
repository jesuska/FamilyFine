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
    public class BudgetLimitController : Controller
    {
        private FinanceDB db = new FinanceDB();

        //
        // GET: /BudgetLimit/

        public ActionResult Index()
        {
            List<BudgetLimit> limitlist = db.BudgetLimits.ToList();
            List<BudgetLimit> monthlylist = limitlist.Where(t => t.IsMonthly).ToList();
            List<BudgetLimit> speciallist = limitlist.Where(t => !t.IsMonthly).ToList();

            // insert fake limit + set enddates
            if (monthlylist.Count > 0)
            {
                monthlylist.Insert(0, new BudgetLimit { StartDate = DateTime.MinValue, IsMonthly = true, Id = 0, Limit = 0 });
                monthlylist = monthlylist.OrderBy(t => t.StartDate).ToList();
                for (int i = 1; i < monthlylist.Count; i++ )
                    monthlylist[i - 1].EndDate = monthlylist[i].StartDate;
            }

            if (speciallist.Count > 0)
            {
                speciallist.Insert(0, new BudgetLimit { StartDate = DateTime.MinValue, IsMonthly = false, Id = 0, Limit = 0 });
                speciallist = speciallist.OrderBy(t => t.StartDate).ToList();
                for (int i = 1; i < speciallist.Count; i++)
                    speciallist[i - 1].EndDate = speciallist[i].StartDate;
            }

            limitlist = monthlylist.Union(speciallist).ToList();

            return View(limitlist.OrderBy(t => t.StartDate));
        }

        //
        // GET: /BudgetLimit/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /BudgetLimit/Create

        [HttpPost]
        public ActionResult Create(BudgetLimit budgetlimit)
        {
            if (ModelState.IsValid)
            {
                db.BudgetLimits.Add(budgetlimit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(budgetlimit);
        }

        //
        // GET: /BudgetLimit/Edit/5

        public ActionResult Edit(int id = 0)
        {
            BudgetLimit budgetlimit = db.BudgetLimits.Find(id);
            if (budgetlimit == null)
            {
                return HttpNotFound();
            }
            return View(budgetlimit);
        }

        //
        // POST: /BudgetLimit/Edit/5

        [HttpPost]
        public ActionResult Edit(BudgetLimit budgetlimit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(budgetlimit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(budgetlimit);
        }

        //
        // GET: /BudgetLimit/Delete/5

        public ActionResult Delete(int id = 0)
        {
            BudgetLimit budgetlimit = db.BudgetLimits.Find(id);
            if (budgetlimit == null)
            {
                return HttpNotFound();
            }
            return View(budgetlimit);
        }

        //
        // POST: /BudgetLimit/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            BudgetLimit budgetlimit = db.BudgetLimits.Find(id);
            db.BudgetLimits.Remove(budgetlimit);
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