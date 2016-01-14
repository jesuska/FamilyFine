using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using BLL;

namespace FamilyFinance.Controllers
{
    public class HomeController : Controller
    {
        const string format = "{0:### ### ##0}";

        public ActionResult Index(DateTime? start = null, DateTime? end = null)
        {
            if (!start.HasValue || !end.HasValue)
            { 
                start = DateTime.Now.AddDays(-1 * DateTime.Now.Day + 1).Date;
                end = start.Value.AddMonths(1).AddDays(-1);
            }

            ViewBag.StartDate = start.Value.ToString("yyyy-MM-dd");
            ViewBag.EndDate = end.Value.ToString("yyyy-MM-dd");

            TransactionManager transactionManager = new TransactionManager();
            List<Transaction> transactions = transactionManager.GetTransactionsInInterval(start.Value, end.Value);

            BudgetLimitManager limitManager = new BudgetLimitManager();
            List<BudgetLimit> limits = limitManager.GetBudgetLimitsForInterval(start.Value, end.Value);

            TotalCashManager cashManager = new TotalCashManager();
            List<TotalCash> totalCashes = cashManager.GetAllTotalCashes();

            CategoryManager categoryManager = new CategoryManager();
            List<Category> categories = categoryManager.GetAllCategories();

            BLL.Reporter reporter = new BLL.Reporter(start.Value, end.Value, transactions, limits, totalCashes, categories);
            ViewData["AverageLimitPerDay"] = String.Format(format, reporter.GetAverageLimitPerDay());
            ViewData["TotalLimit"] = String.Format(format, reporter.GetTotalLimit());
            ViewData["AverageExpensePerDay"] = String.Format(format, reporter.GetAverageExpensePerDay());
            ViewData["TotalExpense"] = String.Format(format, reporter.GetTotalExpense());
            ViewData["TotalAvailableAmount"] = reporter.GetAvailableAmount();
            ViewData["TotalAvailableAmountFormatted"] = String.Format(format, reporter.GetAvailableAmount());
            ViewData["AvailableAmountPerDay"] = String.Format(format, Math.Max(0, reporter.GetAvailableAmountPerDay()));
            ViewData["NumberOfDays"] = reporter.NumberOfDays;
            ViewData["NumberOfDaysUntilToday"] = reporter.NumberOfDaysUntilToday;

            ViewData["CurrentTotalCash"] = String.Format(format, reporter.GetCurrentTotalCash());
            ViewData["StartingTotalCash"] = String.Format(format, reporter.GetStartingTotalCash());
            ViewData["TotalCashDifferenceFormatted"] = String.Format(format, reporter.GetCurrentTotalCash() - reporter.GetStartingTotalCash());
            ViewData["TotalCashDifference"] = reporter.GetCurrentTotalCash() - reporter.GetStartingTotalCash();

            List<KeyValuePair<Category, int>> amountsByCategoryName = reporter.GetAmountsByCategoryName();
            ViewData["AmountsByCategoryName"] = amountsByCategoryName;
            ViewData["TotalAmountsForCategories"] = amountsByCategoryName.Count == 0 ? 0 : (double)amountsByCategoryName.Select(t => t.Value).Sum();

            HttpContext.Session["Reporter"] = reporter;

            return View();
        }

        public ActionResult CashFlowChart()
        {
            if (HttpContext.Session["Reporter"] != null)
            {
                BLL.Reporter reporter = (BLL.Reporter)HttpContext.Session["Reporter"];
                Chart c = reporter.GetCashFlowReport();
                if (c != null)
                    c.Write("png");
            }

            return null;
        } 

        public ActionResult BalanceChart()
        {
            if (HttpContext.Session["Reporter"] != null)
            {
                BLL.Reporter reporter = (BLL.Reporter)HttpContext.Session["Reporter"];
                Chart c = reporter.GetBalanceReport();
                if (c != null)
                    c.Write("png");
            }

            return null;
        }

        public ActionResult CategoryChart()
        {
            if (HttpContext.Session["Reporter"] != null)
            {
                BLL.Reporter reporter = (BLL.Reporter)HttpContext.Session["Reporter"];
                Chart c = reporter.GetCategoryReport();
                if (c != null)
                    c.Write("png");
            }

            return null;
        }
    }
}
