using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services.AccountUtils
{
    public static class AccountValidator
    {
        /// <summary>
        /// Check Account Is Valid
        /// </summary>
        /// <param name="account">Account</param>
        /// <param name="request">Request</param>
        /// <returns>True/False</returns>
        public static bool CheckAccountIsValid(Account account, MakePaymentRequest request)
        {
            bool retVal = true;

            if (account == null)
            {
                return false;
            }

            switch (request.PaymentScheme)
            {
                case PaymentScheme.Bacs:
                    if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs))
                    {
                        retVal = false;
                    }
                    break;

                case PaymentScheme.FasterPayments:
                    if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments))
                    {
                        retVal = false;
                    }
                    else if (account.Balance < request.Amount)
                    {
                        retVal = false;
                    }
                    break;

                case PaymentScheme.Chaps:
                    if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps))
                    {
                        retVal = false;
                    }
                    else if (account.Status != AccountStatus.Live)
                    {
                        retVal = false;
                    }
                    break;
            }

            return retVal;
        }
    }
}