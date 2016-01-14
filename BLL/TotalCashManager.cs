using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class TotalCashManager
    {
        FinanceDB db;

        public TotalCashManager()
        {
            db = new FinanceDB();
        }

        public List<TotalCash> GetAllTotalCashes()
        {
            return db.TotalCashes.OrderBy(t => t.Date).ToList();
        }

        public List<TotalCash> GetTotalCashesInInterval(DateTime startDate, DateTime endDate)
        {
            return db.TotalCashes.Where(t => t.Date >= startDate && t.Date <= endDate).OrderBy(t => t.Date).ToList();
        }
    }
}
