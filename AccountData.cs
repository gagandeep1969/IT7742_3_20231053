namespace BankApp
{
    /// <summary>
    /// Stores account data in a simple format for XML saving and loading.
    /// </summary>
    public class AccountData
    {
        /// <summary>
        /// Stores the account type.
        /// </summary>
        public string AccountType { get; set; }

        /// <summary>
        /// Stores the account id.
        /// </summary>
        public int AccountID { get; set; }

        /// <summary>
        /// Stores the saved balance.
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// Stores the interest rate.
        /// </summary>
        public decimal InterestRate { get; set; }

        /// <summary>
        /// Stores the overdraft limit.
        /// </summary>
        public decimal OverdraftLimit { get; set; }

        /// <summary>
        /// Stores the failed transaction fee.
        /// </summary>
        public decimal FailedFee { get; set; }
    }
}