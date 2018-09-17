using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services.AccountUtils;
using ClearBank.DeveloperTest.Types;
using System.Configuration;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        /// <summary>
        /// Account Backing Member
        /// </summary>
        private readonly IAccountDataStore _accountDataStore;

        //
        // Default Constructor - Sets DataStore by reading Config
        //
        public PaymentService()
        {
            var dataStoreType = ConfigurationManager.AppSettings["DataStoreType"];

            if (dataStoreType == "Backup")
            {
                _accountDataStore = new BackupAccountDataStore();
            }
            else
            {
                _accountDataStore = new AccountDataStore();
            }
        }

        //
        // NOTE Normally would Inject In DataStore - Configured at Startup with relevant Store Type
        //
        public PaymentService(IAccountDataStore dataStore)
        {
            _accountDataStore = dataStore;
        }

        /// <summary>
        /// Make Payment
        /// </summary>
        /// <param name="request">Payment Request</param>
        /// <returns>Payment Result</returns>
        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var result = new MakePaymentResult();

            // Recover Account
            Account account = _accountDataStore.GetAccount(request.DebtorAccountNumber);

            // Check and Set Account Validity
            result.Success = AccountValidator.CheckAccountIsValid(account, request);

            if (result.Success)
            {
                // Calc & Set New Balance
                account.Balance = CalculateNewAccountBalance(account.Balance, request.Amount);

                // Update Account 
                _accountDataStore.UpdateAccount(account);
            }

            return result;
        }
                       
        /// <summary>
        /// Calculate New Balance
        /// </summary>
        /// <param name="accountBalance"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public decimal CalculateNewAccountBalance(decimal accountBalance, decimal amount)
        {            
            return accountBalance - amount;            
        }               
    }
}
