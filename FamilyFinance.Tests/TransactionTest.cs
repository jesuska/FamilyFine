using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using BLL;

namespace FamilyFinance.Tests
{
    [TestClass]
    public class TransactionTest
    {
        DateTime createDate;
        Category auchanCategory;
        TransactionManager transactionManager;

        [TestInitialize]
        public void TestStart()
        {
            createDate = new DateTime(2013, 1, 1);
            auchanCategory = new Category { Id = 1, Name = "Auchan" };
            transactionManager = new TransactionManager();
        }

        [TestMethod]
        public void AddIncome()
        {
            Transaction transaction = new Transaction
            {
                Amount = 1000,
                CreateDate = createDate,
                Category = auchanCategory,
                AffectsMonthlyLimit = true,
                AffectsSpecialLimit = false
            };
            
            List<Transaction> transactions = transactionManager.GetAllTransactions();
            Assert.AreEqual(0, transactions.Count);

            transactionManager.SaveTransaction(transaction);

            transactions = transactionManager.GetAllTransactions();
            Assert.AreEqual(1, transactions.Count);
            Transaction trans = transactions.First();
            Assert.AreNotEqual(0, trans.Id);
            Assert.AreEqual(1000, trans.Amount);
            Assert.AreEqual(createDate, trans.CreateDate);
            Assert.AreEqual(auchanCategory.Id, trans.Category.Id);
            Assert.AreEqual(auchanCategory.Name, trans.Category.Name);
            Assert.AreEqual(true, trans.AffectsMonthlyLimit);
            Assert.AreEqual(false, trans.AffectsSpecialLimit);
        }

        [TestMethod]
        public void AddExpenditure()
        {
            Transaction transaction = new Transaction
            {
                Amount = -1000,
                CreateDate = createDate,
                Category = auchanCategory,
                AffectsMonthlyLimit = true,
                AffectsSpecialLimit = false
            };

            List<Transaction> transactions = transactionManager.GetAllTransactions();
            Assert.AreEqual(0, transactions.Count);

            transactionManager.SaveTransaction(transaction);

            transactions = transactionManager.GetAllTransactions();
            Assert.AreEqual(1, transactions.Count);
            Transaction trans = transactions.First();
            Assert.AreNotEqual(0, trans.Id);
            Assert.AreEqual(-1000, trans.Amount);
            Assert.AreEqual(createDate, trans.CreateDate);
            Assert.AreEqual(auchanCategory.Id, trans.Category.Id);
            Assert.AreEqual(auchanCategory.Name, trans.Category.Name);
            Assert.AreEqual(true, trans.AffectsMonthlyLimit);
            Assert.AreEqual(false, trans.AffectsSpecialLimit);
        }

        [TestMethod]
        public void UpdateTransaction()
        {
            Transaction transaction = new Transaction 
            { 
                Amount = 1000,
                CreateDate = createDate,
                Category = auchanCategory,
                AffectsMonthlyLimit = true,
                AffectsSpecialLimit = false
            };

            transactionManager.SaveTransaction(transaction);

            List<Transaction> transactions = transactionManager.GetAllTransactions();
            Assert.AreEqual(1, transactions.Count);
            Transaction oldtrans = transactions.First();

            Transaction modifiedTransaction = new Transaction
            {
                Id = oldtrans.Id,
                Amount = 2000,
                CreateDate = createDate.AddDays(1),
                Category = new Category { Id = 111, Name = "ads" },
                AffectsMonthlyLimit = false,
                AffectsSpecialLimit = true
            };

            transactionManager.SaveTransaction(modifiedTransaction);

            transactions = transactionManager.GetAllTransactions();
            Assert.AreEqual(1, transactions.Count);
            Transaction trans = transactions.First();
            Assert.AreEqual(oldtrans.Id, trans.Id);
            Assert.AreNotEqual(oldtrans.Amount, trans.Amount);
            Assert.AreNotEqual(oldtrans.CreateDate, trans.CreateDate);
            Assert.AreNotEqual(oldtrans.Category.Id, trans.Category.Id);
            Assert.AreNotEqual(oldtrans.AffectsMonthlyLimit, trans.AffectsMonthlyLimit);
            Assert.AreNotEqual(oldtrans.AffectsSpecialLimit, trans.AffectsSpecialLimit);
        }
    }
}
