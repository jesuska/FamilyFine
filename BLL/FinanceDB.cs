using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace BLL
{
    public class FinanceDB : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BudgetLimit> BudgetLimits { get; set; }
        public DbSet<TotalCash> TotalCashes { get; set; }
    }
}
