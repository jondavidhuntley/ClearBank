using ClearBank.DeveloperTest.Services.AccountUtils;
using ClearBank.DeveloperTest.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ClearBank.DeveloperTest.Tests.Tests
{
    [TestClass]
    public class AccountValidatorTests
    {
        [TestMethod]
        public void TestNullAccountAndPaymentSchemeIsBacsIsValid()
        {
            // Get Null Account
            Account account = null;

            // Get Bacs Request
            MakePaymentRequest request = new MakePaymentRequest
            {
                Amount = Convert.ToDecimal(30.50),
                PaymentScheme = PaymentScheme.Bacs
            };

            var result = AccountValidator.CheckAccountIsValid(account, request);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestBacsAccountAndPaymentSchemeIsBacsIsValid()
        {
            // Get Live Bacs Account
            Account account = GetDummyAccount(AccountStatus.Live, AllowedPaymentSchemes.Bacs, Convert.ToDecimal(200.43));

            // Get Bacs Request
            MakePaymentRequest request = new MakePaymentRequest
            {
                Amount = Convert.ToDecimal(30.50),
                PaymentScheme = PaymentScheme.Bacs
            };

            var result = AccountValidator.CheckAccountIsValid(account, request);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestBacsAccountAndPaymentSchemeIsNotBacsIsValid()
        {
            // Get Live Bacs Account
            Account account = GetDummyAccount(AccountStatus.Live, AllowedPaymentSchemes.Bacs, Convert.ToDecimal(200.43));

            // Get Bacs Request
            MakePaymentRequest request = new MakePaymentRequest
            {
                Amount = Convert.ToDecimal(30.50),
                PaymentScheme = PaymentScheme.Chaps
            };

            var result = AccountValidator.CheckAccountIsValid(account, request);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestFasterPaymentsAccountAndPaymentSchemeIsFasterPaymentsIsValid()
        {
            // Get Live Bacs Account
            Account account = GetDummyAccount(AccountStatus.Live, AllowedPaymentSchemes.FasterPayments, Convert.ToDecimal(200.43));

            // Get Bacs Request
            MakePaymentRequest request = new MakePaymentRequest
            {
                Amount = Convert.ToDecimal(30.50),
                PaymentScheme = PaymentScheme.FasterPayments
            };

            var result = AccountValidator.CheckAccountIsValid(account, request);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestFasterPaymentsAccountAndPaymentSchemeIsFasterPaymentsInsufficientFundsIsValid()
        {
            // Get Live Bacs Account
            Account account = GetDummyAccount(AccountStatus.Live, AllowedPaymentSchemes.FasterPayments, Convert.ToDecimal(25.42));

            // Get Bacs Request
            MakePaymentRequest request = new MakePaymentRequest
            {
                Amount = Convert.ToDecimal(30.50),
                PaymentScheme = PaymentScheme.FasterPayments
            };

            var result = AccountValidator.CheckAccountIsValid(account, request);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestFasterPaymentsAccountAndPaymentSchemeIsFasterPaymentsSufficientFundsIsValid()
        {
            // Get Live Bacs Account
            Account account = GetDummyAccount(AccountStatus.Live, AllowedPaymentSchemes.FasterPayments, Convert.ToDecimal(250.42));

            // Get Bacs Request
            MakePaymentRequest request = new MakePaymentRequest
            {
                Amount = Convert.ToDecimal(30.50),
                PaymentScheme = PaymentScheme.FasterPayments
            };

            var result = AccountValidator.CheckAccountIsValid(account, request);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestLiveChapsAccountAndPaymentSchemeIsChapsIsValid()
        {
            // Get Live Bacs Account
            Account account = GetDummyAccount(AccountStatus.Live, AllowedPaymentSchemes.Chaps, Convert.ToDecimal(200.43));

            // Get Bacs Request
            MakePaymentRequest request = new MakePaymentRequest
            {
                Amount = Convert.ToDecimal(30.50),
                PaymentScheme = PaymentScheme.Chaps
            };

            var result = AccountValidator.CheckAccountIsValid(account, request);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestNotLiveChapsAccountAndPaymentSchemeIsChapsIsValid()
        {
            // Get Live Bacs Account
            Account account = GetDummyAccount(AccountStatus.Disabled, AllowedPaymentSchemes.Chaps, Convert.ToDecimal(200.43));

            // Get Bacs Request
            MakePaymentRequest request = new MakePaymentRequest
            {
                Amount = Convert.ToDecimal(30.50),
                PaymentScheme = PaymentScheme.Chaps
            };

            var result = AccountValidator.CheckAccountIsValid(account, request);

            Assert.IsFalse(result);
        }

        /// <summary>
        /// Get Dummy Test Account
        /// </summary>
        /// <param name="status"></param>
        /// <param name="schemes"></param>
        /// <param name="balance"></param>
        /// <returns></returns>
        private Account GetDummyAccount(AccountStatus status, AllowedPaymentSchemes schemes, decimal balance)
        {
            Account account = new Account
            {
                AccountNumber = "1233456",
                Balance = balance,
                Status = status,
                AllowedPaymentSchemes = schemes
            };

            return account;
        }
    }
}