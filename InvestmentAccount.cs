using BankApp.Exceptions;

namespace BankApp
{
    /// <summary>
    /// Represents an investment account.
    /// This account earns interest and does not allow overdraft.
    /// </summary>
    public class InvestmentAccount : Account
    {
        /// <summary>
        /// Creates a new investment account.
        /// </summary>
        /// <param name="id">Account id.</param>
        /// <param name="opening">Opening balance.</param>
        /// <param name="rate">Interest rate.</param>
        /// <param name="fee">Failed transaction fee.</param>
        public InvestmentAccount(int id, decimal opening, decimal rate, decimal fee)
            : base(id, opening, rate, 0m, fee)
        {
        }

        /// <summary>
        /// Calculates interest for investment account.
        /// Interest applies when balance is above 1000.
        /// </summary>
        public override void CalculateInterest()
        {
            decimal interest = 0m;

            if (balance > 1000m)
            {
                interest = balance * (interestRate / 100m);
            }

            AddInterest(interest);
        }

        /// <summary>
        /// Withdraws money from investment account.
        /// This account cannot go below available balance.
        /// </summary>
        /// <param name="amount">Withdrawal amount.</param>
        /// <param name="staff">Shows whether customer is staff.</param>
        public override void Withdraw(decimal amount, bool staff)
        {
            if (amount <= 0)
            {
                last = "Amount must be positive.";
                return;
            }

            if (amount > balance)
            {
                decimal fee = staff ? failedFee / 2m : failedFee;
                balance -= fee;
                last = "Failed transaction. Fee " + fee.ToString("0.00") + ". Balance " + balance.ToString("0.00");

                throw new FailedWithdrawalException(accountID, GetAccountType(), amount, balance, staff, last);
            }

            base.Withdraw(amount, staff);
        }
    }
}