using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace BLL
{
    public class Reporter
    {
        FinanceDB db;
        private List<Transaction> Transactions { get; set; }
        private List<Transaction> AllTransactions { get; set; }
        private List<BudgetLimit> Limits { get; set; }
        private List<TotalCash> TotalCashes { get; set; }
        private List<Category> Categories { get; set; }
        private DateTime StartDate { get; set; }
        private DateTime EndDate { get; set; }
        public int NumberOfDays { get { return (int)((EndDate - StartDate).TotalDays) + 1; } }
        public int NumberOfDaysUntilToday 
        { 
            get 
            {
                if (StartDate < DateTime.Now.Date && EndDate > DateTime.Now.Date)
                    return (int)((DateTime.Now.Date - StartDate).TotalDays) + 1;

                return NumberOfDays; 
            } 
        }

        public Reporter(DateTime startDate, DateTime endDate, List<Transaction> transactions, List<BudgetLimit> limits, List<TotalCash> totalCashes, List<Category> categories)
        {
            db = new FinanceDB();
            this.AllTransactions = transactions;
            this.Transactions = transactions.Where(t => t.AffectsMonthlyLimit).ToList();
            this.Limits = limits;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.TotalCashes = totalCashes;
            this.Categories = categories;
        }

        #region GetCashFlowReport
        public Chart GetCashFlowReport()
        {
            if (Limits.Count == 0)
                return null;

            // Transactions
            List<DateTime> xValues = Transactions.Select(t => t.CreateDate).ToList();
            xValues.Insert(0, StartDate);
            xValues.Add(EndDate);
            List<int> yValues = new List<int>();
            int total = 0;
            yValues.Insert(0, total);
            foreach (int amount in Transactions.Select(t => t.Amount))
            {
                total += amount;
                yValues.Add(total);
            }
            yValues.Add(total);

            // Monthly limit
            List<DateTime> x2values = new List<DateTime>();
            List<int> y2Values = new List<int>();

            int limit = GetTotalLimit();
            x2values.Add(StartDate);
            y2Values.Add(limit);

            x2values.Add(EndDate);
            y2Values.Add(limit);
            
            //foreach (BudgetLimit limit in Limits.Where(t => t.IsMonthly && t.StartDate < EndDate && (!t.EndDate.HasValue || t.EndDate > StartDate)))
            //{
            //    x2values.Add(xValues.Min() > limit.StartDate ? xValues.Min() : limit.StartDate);
            //    y2Values.Add(limit.Limit);

            //    x2values.Add(!limit.EndDate.HasValue || xValues.Max() < limit.EndDate.Value ? xValues.Max() : limit.EndDate.Value);
            //    y2Values.Add(limit.Limit);
            //}

            // Average expense
            List<DateTime> x3values = new List<DateTime>();
            x3values.Add(StartDate);
            x3values.Add(EndDate);

            List<int> y3Values = new List<int>();
            y3Values.Add(0);
            y3Values.Add(y2Values.Last());

            Chart c = new Chart(width: 600, height: 400, theme: ChartTheme.Green);
            c.AddSeries(chartType: "line",
                        xValue: xValues,
                        yValues: yValues,
                        name: "Tényleges költés");
            c.AddSeries(chartType: "line",
                        xValue: x2values,
                        yValues: y2Values,
                        name: "Költés felső határa");
            c.AddSeries(chartType: "line",
                        xValue: x3values,
                        yValues: y3Values,
                        name: "Egyenletes költés");
            c.AddTitle("Költés és limit");
            c.AddLegend();
            return c;
        }
        #endregion

        #region GetBalanceReport
        public Chart GetBalanceReport()
        {
            List<TotalCash> totalCashes = new List<TotalCash>();
            List<TotalCash> orderedTotalCashes = TotalCashes.OrderBy(t => t.Date).ToList();
            for (int i = 0; i < orderedTotalCashes.Count; i++)
            {
                if ((orderedTotalCashes[i].Date <= EndDate && orderedTotalCashes[i].Date >= StartDate) ||
                    (orderedTotalCashes[i].Date < StartDate && i + 1 == orderedTotalCashes.Count) ||
                    (orderedTotalCashes[i].Date < StartDate && i + 1 < orderedTotalCashes.Count && orderedTotalCashes[i + 1].Date > StartDate))
                {
                    totalCashes.Add(orderedTotalCashes[i]);
                }
            }

            if (totalCashes.Count == 0)
                return null;

            totalCashes = totalCashes.OrderBy(t => t.Date).ToList();

            // TotalCashes
            //List<DateTime> xValues = totalCashes.Where(t => t.Date <= EndDate && t.Date >= StartDate).Select(t => t.Date).ToList();
            List<DateTime> xValues = new List<DateTime>();
            List<int> yValues = new List<int>();
            //xValues.Insert(0, StartDate);
            //yValues.Insert(0, GetStartingTotalCash());
            for (int i = 0; i < totalCashes.Count; i++)
            {
                List<Transaction> innerTransactions = AllTransactions.Where(t => t.CreateDate > totalCashes[i].Date 
                                                        && t.CreateDate <= (i + 1 < totalCashes.Count ? totalCashes[i + 1].Date : EndDate)).ToList();
                int total = totalCashes[i].Amount;
                if (innerTransactions.Count == 0)
                {
                    xValues.Add(totalCashes[i].Date);
                    yValues.Add(total);
                }
                foreach(var trans in innerTransactions.GroupBy(t => t.CreateDate))
                {
                    DateTime date = trans.Key;
                    foreach (Transaction tr in trans.ToList())
                    {
                        total -= tr.Amount;
                    }

                    xValues.Add(date);
                    yValues.Add(total);
                }
            }
            
            xValues.Add(EndDate);
            yValues.Add(GetCurrentTotalCash());
            //List<int> yValues = totalCashes.Where(t => t.Date <= EndDate && t.Date >= StartDate).Select(t => t.Amount).ToList();

            Chart c = new Chart(width: 600, height: 400, theme: ChartTheme.Green);
            c.AddSeries(chartType: "line",
                        xValue: xValues,
                        yValues: yValues);
            c.AddTitle("Aktuális pénzmennyiség alakulása");
            return c;
        }
        #endregion

        #region GetCategoryReport
        public Chart GetCategoryReport()
        {
            List<KeyValuePair<string, int>> amountByName = GetAmountsByCategoryName().Select(t => new KeyValuePair<string, int>(t.Key.Name, t.Value)).ToList();

            Chart c = new Chart(width: 600, height: 400, theme: ChartTheme.Green);
            c.AddSeries(chartType: "doughnut",
                        xValue: amountByName, xField: "Key",
                        yValues: amountByName, yFields: "Value");
            c.AddTitle("Kiadások kategóriánként");
            return c;
        }
        #endregion

        #region Calculations
        public int GetAverageLimitPerDay()
        {
            if (Limits.Count == 0 || NumberOfDays <= 0)
                return 0;

            return GetTotalLimit() / NumberOfDays;
        }

        public int GetAverageExpensePerDay()
        {
            if (Transactions.Count == 0 || NumberOfDaysUntilToday <= 0)
                return 0;

            return Transactions.Select(t => t.Amount).Sum() / NumberOfDaysUntilToday;
        }

        public int GetTotalLimit()
        {
            BudgetLimit lastLimit = Limits.Where(t => t.IsMonthly && t.StartDate < EndDate && (!t.EndDate.HasValue || t.EndDate > StartDate)).LastOrDefault();
            if (lastLimit == null)
                return 0;

            return lastLimit.Limit * (MonthDifference(EndDate, StartDate) + 1);
        }

        public int GetTotalExpense()
        {
            if (Transactions.Count == 0)
                return 0;

            return Transactions.Select(t => t.Amount).Sum();
        }

        public int GetAvailableAmount()
        {
            return GetTotalLimit() - GetTotalExpense();
        }

        public int GetAvailableAmountPerDay()
        {
            if (NumberOfDays - NumberOfDaysUntilToday <= 0)
                return 0;

            return GetAvailableAmount() / (NumberOfDays - NumberOfDaysUntilToday);
        }

        public int GetCurrentTotalCash()
        {
            if (TotalCashes.Count == 0)
                return 0;

            TotalCash lastTotalCash = TotalCashes.Last();
            List<Transaction> transactions = AllTransactions.Where(t => t.CreateDate > lastTotalCash.Date).ToList();

            return lastTotalCash.Amount - transactions.Select(t => t.Amount).Sum();
        }

        public int GetStartingTotalCash()
        {
            List<TotalCash> totalCashes = new List<TotalCash>();
            List<TotalCash> orderedTotalCashes = TotalCashes.OrderBy(t => t.Date).ToList();
            for (int i = 0; i < orderedTotalCashes.Count; i++)
            {
                if ((orderedTotalCashes[i].Date <= EndDate && orderedTotalCashes[i].Date >= StartDate) ||
                    (orderedTotalCashes[i].Date < StartDate && i + 1 == orderedTotalCashes.Count) ||
                    (orderedTotalCashes[i].Date < StartDate && i + 1 < orderedTotalCashes.Count && orderedTotalCashes[i + 1].Date > StartDate))
                {
                    totalCashes.Add(orderedTotalCashes[i]);
                }
            }

            if (totalCashes.Count == 0)
                return 0;

            TotalCash firstTotalCash = totalCashes.First();
            if (firstTotalCash.Date >= StartDate)
                return firstTotalCash.Amount;

            // get transactions until startdate
            List<Transaction> transactions = AllTransactions.Where(t => t.CreateDate < StartDate && t.CreateDate > firstTotalCash.Date).ToList();
            return firstTotalCash.Amount - transactions.Select(t => t.Amount).Sum();
        }
        #endregion

        #region Helpers
        public static int MonthDifference(DateTime lValue, DateTime rValue)
        {
            return Math.Abs((lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year));
        }

        public List<KeyValuePair<Category, int>> GetAmountsByCategoryName()
        {
            List<KeyValuePair<Category, int>> amountByName = new List<KeyValuePair<Category, int>>();
            ILookup<Category, Transaction> transactionsByCategoryId = AllTransactions.Where(t => t.CreateDate >= StartDate && t.CreateDate <= EndDate && t.Amount > 0).ToLookup(t => Categories.Find(n => n.Id == t.CategoryId.GetValueOrDefault()));

            if (transactionsByCategoryId.Count == 0)
                return amountByName;

            foreach (var item in transactionsByCategoryId)
            {
                KeyValuePair<Category, int> pair = new KeyValuePair<Category, int>(item.Key, item.ToList().Select(t => t.Amount).Sum());
                amountByName.Add(pair);
            }

            return amountByName.OrderByDescending(t => t.Value).ToList();
        }
        #endregion
    }
}
