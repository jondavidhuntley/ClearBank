using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Data;
using Moq;

namespace ClearBank.DeveloperTest.Tests.Tests
{
    [TestClass]
    public class PaymentServiceTests
    {        
        [TestInitialize()]
        public void MyTestInitialize()
        {            
        }

        [TestMethod]
        public void TestMakeSuccessfulPayment()
        {            
            Account account = new Account
            {
                AccountNumber = "1233456",
                Balance = Convert.ToDecimal(323.34),
                Status = AccountStatus.Live,
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments
            };

            var mockDataStore = new Mock<IAccountDataStore>();          

            mockDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);

            PaymentService service = new PaymentService(mockDataStore.Object);

            MakePaymentRequest request = new MakePaymentRequest
            {
                DebtorAccountNumber = account.AccountNumber,
                Amount = Convert.ToDecimal(30.50),
                PaymentScheme = PaymentScheme.FasterPayments
            };

            // Call Payment Service
            var result = service.MakePayment(request);

            // Verfiy Data Store Calls for Get Account & Update are Called           
            mockDataStore.Verify(s => s.GetAccount(account.AccountNumber), Times.Once);
            mockDataStore.Verify(s => s.UpdateAccount(account), Times.Once);

            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void TestMakeUnSuccessfulPaymentInSufficientFunds()
        {
            Account account = new Account
            {
                AccountNumber = "1233456",
                Balance = Convert.ToDecimal(23.34),
                Status = AccountStatus.Live,
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments
            };

            var mockDataStore = new Mock<IAccountDataStore>();

            mockDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);

            PaymentService service = new PaymentService(mockDataStore.Object);

            MakePaymentRequest request = new MakePaymentRequest
            {
                DebtorAccountNumber = account.AccountNumber,
                Amount = Convert.ToDecimal(30.50),
                PaymentScheme = PaymentScheme.FasterPayments
            };

            // Call Payment Service
            var result = service.MakePayment(request);

            // Verfiy Data Store Calls for Get Account Called but NOT Update
            mockDataStore.Verify(s => s.GetAccount(account.AccountNumber), Times.Once);
            mockDataStore.Verify(s => s.UpdateAccount(account), Times.Never);

            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void TestNewBalanceCalculation()
        {
            PaymentService service = new PaymentService();
              
            decimal balance = Convert.ToDecimal(240.45);
            decimal amount = Convert.ToDecimal(32.54);
            decimal result;
            decimal expected = balance - amount;

            result = service.CalculateNewAccountBalance(balance, amount);

            Assert.AreEqual(result, expected);
        }      
    }
}