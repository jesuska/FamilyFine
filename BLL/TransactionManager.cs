using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class TransactionManager
    {
        FinanceDB db;

        public TransactionManager()
        {
            db = new FinanceDB();
        }

        public List<Transaction> GetAllTransactions()
        {
            return db.Transactions.ToList();
        }

        public List<Transaction> GetTransactionsInInterval(DateTime startDate, DateTime endDate)
        {
            return db.Transactions.Where(t => t.CreateDate >= startDate && t.CreateDate <= endDate).OrderBy(t => t.CreateDate).ToList();
        }

        public void SaveTransaction(Transaction transaction)
        {
            Transaction foundTrans = db.Transactions.Find(transaction.Id);
            if (foundTrans != null)
                db.Transactions.Remove(foundTrans);

            transaction.Id = 1;
            db.Transactions.Add(transaction);
            db.SaveChanges();
            return;
        }
    }
}
