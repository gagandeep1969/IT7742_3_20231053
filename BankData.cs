using System.Collections.Generic;

namespace BankApp
{
    /// <summary>
    /// Stores the full saved bank data for XML serialization.
    /// It keeps the next account number and all customers.
    /// </summary>
    public class BankData
    {
        /// <summary>
        /// Stores the next account number.
        /// </summary>
        public int NextAccountNumber { get; set; }

        /// <summary>
        /// Stores all customer data objects.
        /// </summary>
        public List<CustomerData> Customers { get; set; }

        /// <summary>
        /// Creates a new bank data object.
        /// </summary>
        public BankData()
        {
            Customers = new List<CustomerData>();
        }
    }
}