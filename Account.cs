using BankApp.Exceptions;

namespace BankApp
{
    /// <summary>
    /// Base class for all account types in the app.
    /// It stores common account data and common actions.
    /// </summary>
    public abstract class Account
    {
        /// <summary>
        /// Stores the account id.
        /// </summary>
        protected int accountID;

        /// <summary>
        /// Stores the current balance.
        /// </summary>
        protected decimal balance;

        /// <summary>
        /// Stores the interest rate.
        /// </summary>
        protected decimal interestRate;

        /// <summary>
        /// Stores the overdraft limit.
        /// </summary>
        protected decimal overdraftLimit;

        /// <summary>
        /// Stores the failed transaction fee.
        /// </summary>
        protected decimal failedFee;

        /// <summary>
        /// Stores the last account message.
        /// </summary>
        protected string last = "";

        /// <summary>
        /// Creates a new account object.
        /// </summary>
        /// <param name="id">Account id.</param>
        /// <param name="opening">Opening balance.</param>
        /// <param name="rate">Interest rate.</param>
        /// <param name="od">Overdraft limit.</param>
        /// <param name="fee">Failed fee.</param>
        protected Account(int id, decimal opening, decimal rate, decimal od, decimal fee)
        {
            accountID = id;
            balance = opening;
            interestRate = rate;
            overdraftLimit = od;
            failedFee = fee;
        }

        /// <summary>
        /// Returns the account id.
        /// </summary>
        /// <returns>Account id.</returns>
        public int GetAccountID()
        {
            return accountID;
        }

        /// <summary>
        /// Returns the current balance.
        /// </summary>
        /// <returns>Account balance.</returns>
        public decimal GetBalance()
        {
            return balance;
        }

        /// <summary>
        /// Returns the interest rate.
        /// </summary>
        /// <returns>Interest rate.</returns>
        public decimal GetInterestRate()
        {
            return interestRate;
        }

        /// <summary>
        /// Returns the overdraft limit.
        /// </summary>
        /// <returns>Overdraft limit.</returns>
        public decimal GetOverdraftLimit()
        {
            return overdraftLimit;
        }

        /// <summary>
        /// Returns the failed transaction fee.
        /// </summary>
        /// <returns>Failed fee.</returns>
        public decimal GetFailedFee()
        {
            return failedFee;
        }

        /// <summary>
        /// Returns the last account message.
        /// </summary>
        /// <returns>Last message text.</returns>
        public string Last()
        {
            return last;
        }

        /// <summary>
        /// Returns the account type name.
        /// </summary>
        /// <returns>Account type text.</returns>
        public string GetAccountType()
        {
            return GetType().Name.Replace("Account", "");
        }

        /// <summary>
        /// Adds money to the account.
        /// </summary>
        /// <param name="amount">Deposit amount.</param>
        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                last = "Amount must be positive.";
                return;
            }

            balance += amount;
            last = "Deposited " + amount.ToString("0.00") + ". Balance " + balance.ToString("0.00");
        }

        /// <summary>
        /// Withdraws money from the account.
        /// Staff discount applies on failed fee where needed.
        /// </summary>
        /// <param name="amount">Withdrawal amount.</param>
        /// <param name="staff">Shows whether customer is staff.</param>
        public virtual void Withdraw(decimal amount, bool staff)
        {
            if (amount <= 0)
            {
                last = "Amount must be positive.";
                return;
            }

            decimal limit = balance + overdraftLimit;

            if (amount > limit)
            {
                decimal fee = staff ? failedFee / 2m : failedFee;
                balance -= fee;
                last = "Failed transaction. Fee " + fee.ToString("0.00") + ". Balance " + balance.ToString("0.00");

                throw new FailedWithdrawalException(accountID, GetAccountType(), amount, limit, staff, last);
            }

            balance -= amount;
            last = "Withdrawn " + amount.ToString("0.00") + ". Balance " + balance.ToString("0.00");
        }

        /// <summary>
        /// Calculates account interest.
        /// Child classes use their own rule.
        /// </summary>
        public abstract void CalculateInterest();

        /// <summary>
        /// Adds calculated interest into balance.
        /// </summary>
        /// <param name="value">Interest value.</param>
        protected void AddInterest(decimal value)
        {
            balance += value;
            last = "Interest added " + value.ToString("0.00") + ". Balance " + balance.ToString("0.00");
        }

        /// <summary>
        /// Returns account display text.
        /// </summary>
        /// <returns>Formatted account text.</returns>
        public override string ToString()
        {
            return GetAccountType() + " " + accountID + " - $" + balance.ToString("0.00");
        }
    }
}