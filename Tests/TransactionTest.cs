using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Transactions;

namespace Tests
{
    public class TransactionTest
    {
        private TransactionScope transactionScope;

        [TestInitialize]
        public void BaseTestInitialize()
        {
            // Begin a new transaction scope before each test
            transactionScope = new TransactionScope();
        }

        [TestCleanup]
        public void BaseTestCleanup()
        {
            // Dispose of the transaction scope after each test, rolling back any changes
            transactionScope.Dispose();
        }
    }
}