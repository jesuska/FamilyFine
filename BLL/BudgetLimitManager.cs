using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BudgetLimitManager
    {
        private FinanceDB db = new FinanceDB();

        public List<BudgetLimit> GetBudgetLimitsForInterval(DateTime start, DateTime end)
        {
            List<BudgetLimit> limitlist = db.BudgetLimits.ToList();
            List<BudgetLimit> monthlylist = limitlist.Where(t => t.IsMonthly).ToList();
            List<BudgetLimit> speciallist = limitlist.Where(t => !t.IsMonthly).ToList();

            // insert fake limit + set enddates
            if (monthlylist.Count > 0)
            {
                monthlylist.Insert(0, new BudgetLimit { StartDate = DateTime.MinValue, IsMonthly = true, Id = 0, Limit = 0 });
                monthlylist = monthlylist.OrderBy(t => t.StartDate).ToList();
                for (int i = 1; i < monthlylist.Count; i++)
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

            return limitlist.OrderBy(t => t.StartDate).ToList();
        }

    }
}
