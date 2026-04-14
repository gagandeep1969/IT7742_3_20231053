using System.Collections.Generic;

namespace BankApp
{
    /// <summary>
    /// Stores customer data in a simple format for XML saving and loading.
    /// </summary>
    public class CustomerData
    {
        /// <summary>
        /// Stores the customer number.
        /// </summary>
        public int CustomerNumber { get; set; }

        /// <summary>
        /// Stores the customer name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Stores the customer contact.
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// Shows whether the customer is a staff member.
        /// </summary>
        public bool IsStaff { get; set; }

        /// <summary>
        /// Stores the saved accounts for this customer.
        /// </summary>
        public List<AccountData> Accounts { get; set; }

        /// <summary>
        /// Creates a new customer data object.
        /// </summary>
        public CustomerData()
        {
            Accounts = new List<AccountData>();
        }
    }
}