using System.Collections.Generic;

namespace BankApp
{
    /// <summary>
    /// Stores customer details and the accounts owned by the customer.
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// Stores the customer number.
        /// </summary>
        private int customerNumber;

        /// <summary>
        /// Stores the customer name.
        /// </summary>
        private string name;

        /// <summary>
        /// Stores the contact detail.
        /// </summary>
        private string contact;

        /// <summary>
        /// Shows whether the customer is a staff member.
        /// </summary>
        private bool isStaff;

        /// <summary>
        /// Stores all accounts for this customer.
        /// </summary>
        private List<Account> accounts;

        /// <summary>
        /// Creates a new customer object.
        /// </summary>
        /// <param name="number">Customer number.</param>
        /// <param name="name">Customer name.</param>
        /// <param name="contact">Customer contact.</param>
        /// <param name="staffFlag">Staff flag.</param>
        public Customer(int number, string name, string contact, bool staffFlag)
        {
            customerNumber = number;
            this.name = name;
            this.contact = contact;
            isStaff = staffFlag;
            accounts = new List<Account>();
        }

        /// <summary>
        /// Returns the customer number.
        /// </summary>
        /// <returns>Customer number.</returns>
        public int GetCustomerNumber()
        {
            return customerNumber;
        }

        /// <summary>
        /// Returns the customer name.
        /// </summary>
        /// <returns>Customer name.</returns>
        public string GetName()
        {
            return name;
        }

        /// <summary>
        /// Returns the customer contact.
        /// </summary>
        /// <returns>Customer contact.</returns>
        public string GetContact()
        {
            return contact;
        }

        /// <summary>
        /// Returns true if the customer is staff.
        /// </summary>
        /// <returns>True for staff, otherwise false.</returns>
        public bool IsStaff()
        {
            return isStaff;
        }

        /// <summary>
        /// Updates the customer name.
        /// </summary>
        /// <param name="value">New name.</param>
        public void SetName(string value)
        {
            name = value;
        }

        /// <summary>
        /// Updates the customer contact.
        /// </summary>
        /// <param name="value">New contact.</param>
        public void SetContact(string value)
        {
            contact = value;
        }

        /// <summary>
        /// Updates the staff status.
        /// </summary>
        /// <param name="value">New staff flag.</param>
        public void SetStaff(bool value)
        {
            isStaff = value;
        }

        /// <summary>
        /// Adds an account to the customer.
        /// </summary>
        /// <param name="acc">Account object.</param>
        public void AddAccount(Account acc)
        {
            if (acc != null)
            {
                accounts.Add(acc);
            }
        }

        /// <summary>
        /// Returns all accounts owned by the customer.
        /// </summary>
        /// <returns>List of accounts.</returns>
        public List<Account> GetAccounts()
        {
            return accounts;
        }

        /// <summary>
        /// Finds an account using the account id.
        /// </summary>
        /// <param name="id">Account id.</param>
        /// <returns>The account if found, otherwise null.</returns>
        public Account FindAccount(int id)
        {
            foreach (Account acc in accounts)
            {
                if (acc.GetAccountID() == id)
                {
                    return acc;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns customer display text.
        /// </summary>
        /// <returns>Formatted customer text.</returns>
        public override string ToString()
        {
            string staffText = isStaff ? "Staff" : "Customer";
            return customerNumber + " - " + name + " (" + staffText + ")";
        }
    }
}