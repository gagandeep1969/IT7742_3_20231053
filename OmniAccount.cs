namespace BankApp
{
    /// <summary>
    /// Represents an omni account.
    /// This account allows overdraft and can earn interest.
    /// </summary>
    public class OmniAccount : Account
    {
        /// <summary>
        /// Creates a new omni account.
        /// </summary>
        /// <param name="id">Account id.</param>
        /// <param name="opening">Opening balance.</param>
        /// <param name="rate">Interest rate.</param>
        /// <param name="od">Overdraft limit.</param>
        /// <param name="fee">Failed transaction fee.</param>
        public OmniAccount(int id, decimal opening, decimal rate, decimal od, decimal fee)
            : base(id, opening, rate, od, fee)
        {
        }

        /// <summary>
        /// Calculates interest for omni account.
        /// Interest applies on the amount above 1500.
        /// </summary>
        public override void CalculateInterest()
        {
            decimal interest = 0m;

            if (balance > 1500m)
            {
                interest = (balance - 1500m) * (interestRate / 100m);
            }

            AddInterest(interest);
        }

        /// <summary>
        /// Withdraws money using the base omni account rules.
        /// </summary>
        /// <param name="amount">Withdrawal amount.</param>
        /// <param name="staff">Shows whether customer is staff.</param>
        public override void Withdraw(decimal amount, bool staff)
        {
            base.Withdraw(amount, staff);
        }
    }
}