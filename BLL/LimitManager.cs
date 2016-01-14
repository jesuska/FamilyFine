using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class LimitManager
    {
        public List<BudgetLimit> MonthlyLimits { get; set; }
        public List<BudgetLimit> SpecialLimits { get; set; }

        FinanceDB db;

        public LimitManager()
        {
            db = new FinanceDB();
        }

        public List<BudgetLimit> GetMonthlyLimitsInInterval(DateTime StartDate, DateTime EndDate)
        {
            List<BudgetLimit> limits = db.BudgetLimits.Where(t => t.IsMonthly && t.StartDate > StartDate && t.StartDate < EndDate).ToList();
            BudgetLimit previousLimit = db.BudgetLimits.Where(t => t.IsMonthly && t.StartDate < StartDate).OrderBy(t => t.StartDate).LastOrDefault();
            if (previousLimit != null)
                limits.Add(previousLimit);
            else
                limits.Add(new BudgetLimit { IsMonthly = true, Limit = 0, StartDate = DateTime.MinValue });

            return limits.OrderBy(t => t.StartDate).ToList();
        }

        public List<BudgetLimit> GetSpecialLimitsInInterval(DateTime StartDate, DateTime EndDate)
        {
            List<BudgetLimit> limits = db.BudgetLimits.Where(t => !t.IsMonthly && t.StartDate > StartDate && t.StartDate < EndDate).ToList();
            BudgetLimit previousLimit = db.BudgetLimits.Where(t => !t.IsMonthly && t.StartDate < StartDate).OrderBy(t => t.StartDate).LastOrDefault();
            if (previousLimit != null)
                limits.Add(previousLimit);
            else
                limits.Add(new BudgetLimit { IsMonthly = false, Limit = 0, StartDate = DateTime.MinValue });

            return limits.OrderBy(t => t.StartDate).ToList();
        }
    }
}
