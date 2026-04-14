using System;

namespace BankApp.Exceptions
{
    public class FailedWithdrawalException : Exception
    {
        public FailedWithdrawalException(int accountId, string type, decimal amount, decimal limit, bool staff, string details)
            : base("Failed in " + type + " " + accountId +
                  ". Amount: " + amount.ToString("0.00") +
                  ", Available: " + limit.ToString("0.00") +
                  ", Staff: " + staff.ToString() +
                  ". " + details)
        {
        }
    }
}