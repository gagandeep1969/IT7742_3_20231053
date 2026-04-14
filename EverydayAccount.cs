namespace BankApp
{
    /// <summary>
    /// Represents an everyday account.
    /// This account has no interest and no overdraft.
    /// </summary>
    public class EverydayAccount : Account
    {
        /// <summary>
        /// Creates a new everyday account.
        /// </summary>
        /// <param name="id">Account id.</param>
        /// <param name="opening">Opening balance.</param>
        public EverydayAccount(int id, decimal opening)
            : base(id, opening, 0m, 0m, 0m)
        {
        }

        /// <summary>
        /// Everyday account does not earn interest.
        /// </summary>
        public override void CalculateInterest()
        {
            AddInterest(0m);
        }
    }
}