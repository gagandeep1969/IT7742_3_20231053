using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace BankApp
{
    /// <summary>
    /// Controls the main banking actions in the app.
    /// It manages customers, accounts, transfers, and saved data.
    /// </summary>
    public class BankController
    {
        /// <summary>
        /// Stores all customers in the system.
        /// </summary>
        private List<Customer> customers;

        /// <summary>
        /// Stores the next account number to use.
        /// </summary>
        private int nextAccountNumber;

        /// <summary>
        /// Creates a new bank controller object.
        /// </summary>
        public BankController()
        {
            customers = new List<Customer>();
            nextAccountNumber = 1001;
        }

        /// <summary>
        /// Returns all customers in the system.
        /// </summary>
        /// <returns>List of customers.</returns>
        public List<Customer> GetCustomers()
        {
            return customers;
        }

        /// <summary>
        /// Finds a customer using the customer number.
        /// </summary>
        /// <param name="number">Customer number.</param>
        /// <returns>The customer if found, otherwise null.</returns>
        public Customer FindCustomer(int number)
        {
            foreach (Customer c in customers)
            {
                if (c.GetCustomerNumber() == number)
                {
                    return c;
                }
            }

            return null;
        }

        /// <summary>
        /// Adds a new customer to the system.
        /// </summary>
        /// <param name="number">Customer number.</param>
        /// <param name="name">Customer name.</param>
        /// <param name="contact">Customer contact.</param>
        /// <param name="isStaff">Shows whether customer is staff.</param>
        /// <param name="msg">Result message.</param>
        /// <returns>True if added, otherwise false.</returns>
        public bool AddCustomer(int number, string name, string contact, bool isStaff, out string msg)
        {
            if (FindCustomer(number) != null)
            {
                msg = "Customer number already exists.";
                return false;
            }

            Customer customer = new Customer(number, name, contact, isStaff);
            customers.Add(customer);
            msg = "Customer added.";
            return true;
        }

        /// <summary>
        /// Updates an existing customer.
        /// </summary>
        /// <param name="number">Customer number.</param>
        /// <param name="name">New name.</param>
        /// <param name="contact">New contact.</param>
        /// <param name="isStaff">New staff flag.</param>
        /// <param name="msg">Result message.</param>
        /// <returns>True if updated, otherwise false.</returns>
        public bool UpdateCustomer(int number, string name, string contact, bool isStaff, out string msg)
        {
            Customer customer = FindCustomer(number);

            if (customer == null)
            {
                msg = "Customer not found.";
                return false;
            }

            customer.SetName(name);
            customer.SetContact(contact);
            customer.SetStaff(isStaff);

            msg = "Customer updated.";
            return true;
        }

        /// <summary>
        /// Deletes a customer from the system.
        /// </summary>
        /// <param name="number">Customer number.</param>
        /// <param name="msg">Result message.</param>
        /// <returns>True if deleted, otherwise false.</returns>
        public bool DeleteCustomer(int number, out string msg)
        {
            Customer customer = FindCustomer(number);

            if (customer == null)
            {
                msg = "Customer not found.";
                return false;
            }

            customers.Remove(customer);
            msg = "Customer deleted.";
            return true;
        }

        /// <summary>
        /// Adds an account to a selected customer.
        /// </summary>
        /// <param name="customerNumber">Customer number.</param>
        /// <param name="type">Account type.</param>
        /// <param name="opening">Opening balance.</param>
        /// <param name="msg">Result message.</param>
        /// <returns>True if account was added, otherwise false.</returns>
        public bool AddAccountToCustomer(int customerNumber, string type, decimal opening, out string msg)
        {
            Customer customer = FindCustomer(customerNumber);

            if (customer == null)
            {
                msg = "Customer not found.";
                return false;
            }

            if (opening < 0)
            {
                msg = "Opening balance cannot be negative.";
                return false;
            }

            Account acc = null;

            if (type == "Everyday")
            {
                acc = new EverydayAccount(nextAccountNumber, opening);
            }
            else if (type == "Investment")
            {
                acc = new InvestmentAccount(nextAccountNumber, opening, 5m, 15m);
            }
            else if (type == "Omni")
            {
                acc = new OmniAccount(nextAccountNumber, opening, 3m, 150m, 10m);
            }
            else
            {
                msg = "Invalid account type.";
                return false;
            }

            customer.AddAccount(acc);
            nextAccountNumber++;
            msg = "Account added.";
            return true;
        }

        /// <summary>
        /// Gets all accounts for one customer.
        /// </summary>
        /// <param name="customerNumber">Customer number.</param>
        /// <returns>List of accounts.</returns>
        public List<Account> GetCustomerAccounts(int customerNumber)
        {
            Customer customer = FindCustomer(customerNumber);

            if (customer == null)
            {
                return new List<Account>();
            }

            return customer.GetAccounts();
        }

        /// <summary>
        /// Deposits money into a selected account.
        /// </summary>
        /// <param name="customerNumber">Customer number.</param>
        /// <param name="accountId">Account id.</param>
        /// <param name="amount">Deposit amount.</param>
        /// <param name="msg">Result message.</param>
        /// <returns>True if deposit completed, otherwise false.</returns>
        public bool Deposit(int customerNumber, int accountId, decimal amount, out string msg)
        {
            Customer customer = FindCustomer(customerNumber);

            if (customer == null)
            {
                msg = "Customer not found.";
                return false;
            }

            Account acc = customer.FindAccount(accountId);

            if (acc == null)
            {
                msg = "Account not found.";
                return false;
            }

            acc.Deposit(amount);
            msg = acc.Last();
            return true;
        }

        /// <summary>
        /// Withdraws money from a selected account.
        /// </summary>
        /// <param name="customerNumber">Customer number.</param>
        /// <param name="accountId">Account id.</param>
        /// <param name="amount">Withdrawal amount.</param>
        /// <param name="msg">Result message.</param>
        /// <returns>True if withdrawal completed, otherwise false.</returns>
        public bool Withdraw(int customerNumber, int accountId, decimal amount, out string msg)
        {
            Customer customer = FindCustomer(customerNumber);

            if (customer == null)
            {
                msg = "Customer not found.";
                return false;
            }

            Account acc = customer.FindAccount(accountId);

            if (acc == null)
            {
                msg = "Account not found.";
                return false;
            }

            try
            {
                acc.Withdraw(amount, customer.IsStaff());
                msg = acc.Last();
                return true;
            }
            catch (System.Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Adds interest to a selected account.
        /// </summary>
        /// <param name="customerNumber">Customer number.</param>
        /// <param name="accountId">Account id.</param>
        /// <param name="msg">Result message.</param>
        /// <returns>True if interest was added, otherwise false.</returns>
        public bool AddInterest(int customerNumber, int accountId, out string msg)
        {
            Customer customer = FindCustomer(customerNumber);

            if (customer == null)
            {
                msg = "Customer not found.";
                return false;
            }

            Account acc = customer.FindAccount(accountId);

            if (acc == null)
            {
                msg = "Account not found.";
                return false;
            }

            acc.CalculateInterest();
            msg = acc.Last();
            return true;
        }

        /// <summary>
        /// Transfers money between two accounts of the same customer.
        /// </summary>
        /// <param name="customerNumber">Customer number.</param>
        /// <param name="fromId">Source account id.</param>
        /// <param name="toId">Target account id.</param>
        /// <param name="amount">Transfer amount.</param>
        /// <param name="msg">Result message.</param>
        /// <returns>True if transfer completed, otherwise false.</returns>
        public bool Transfer(int customerNumber, int fromId, int toId, decimal amount, out string msg)
        {
            Customer customer = FindCustomer(customerNumber);

            if (customer == null)
            {
                msg = "Customer not found.";
                return false;
            }

            Account fromAcc = customer.FindAccount(fromId);
            Account toAcc = customer.FindAccount(toId);

            if (fromAcc == null || toAcc == null)
            {
                msg = "One account was not found.";
                return false;
            }

            if (fromId == toId)
            {
                msg = "Cannot transfer to same account.";
                return false;
            }

            if (amount <= 0)
            {
                msg = "Amount must be positive.";
                return false;
            }

            try
            {
                fromAcc.Withdraw(amount, customer.IsStaff());
                toAcc.Deposit(amount);
                msg = "Transfer complete.";
                return true;
            }
            catch (System.Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Saves all customer and account data into an XML file.
        /// </summary>
        /// <param name="filePath">File path to save data.</param>
        public void SaveData(string filePath)
        {
            BankData data = new BankData();
            data.NextAccountNumber = nextAccountNumber;

            foreach (Customer customer in customers)
            {
                CustomerData cData = new CustomerData();
                cData.CustomerNumber = customer.GetCustomerNumber();
                cData.Name = customer.GetName();
                cData.Contact = customer.GetContact();
                cData.IsStaff = customer.IsStaff();

                foreach (Account acc in customer.GetAccounts())
                {
                    AccountData aData = new AccountData();
                    aData.AccountType = acc.GetAccountType();
                    aData.AccountID = acc.GetAccountID();
                    aData.Balance = acc.GetBalance();
                    aData.InterestRate = acc.GetInterestRate();
                    aData.OverdraftLimit = acc.GetOverdraftLimit();
                    aData.FailedFee = acc.GetFailedFee();

                    cData.Accounts.Add(aData);
                }

                data.Customers.Add(cData);
            }

            XmlSerializer serializer = new XmlSerializer(typeof(BankData));

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(fs, data);
            }
        }

        /// <summary>
        /// Loads customer and account data from an XML file.
        /// </summary>
        /// <param name="filePath">File path to load data from.</param>
        public void LoadData(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(BankData));

            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                BankData data = (BankData)serializer.Deserialize(fs);

                customers.Clear();

                if (data == null)
                {
                    nextAccountNumber = 1001;
                    return;
                }

                foreach (CustomerData cData in data.Customers)
                {
                    Customer customer = new Customer(
                        cData.CustomerNumber,
                        cData.Name,
                        cData.Contact,
                        cData.IsStaff
                    );

                    foreach (AccountData aData in cData.Accounts)
                    {
                        Account acc = CreateAccountFromData(aData);

                        if (acc != null)
                        {
                            customer.AddAccount(acc);
                        }
                    }

                    customers.Add(customer);
                }

                if (data.NextAccountNumber > 0)
                {
                    nextAccountNumber = data.NextAccountNumber;
                }
                else
                {
                    nextAccountNumber = GetNextAccountIdFromLoadedData();
                }
            }
        }

        /// <summary>
        /// Rebuilds an account object from saved account data.
        /// </summary>
        /// <param name="aData">Saved account data object.</param>
        /// <returns>The rebuilt account object.</returns>
        private Account CreateAccountFromData(AccountData aData)
        {
            if (aData.AccountType == "Everyday")
            {
                return new EverydayAccount(aData.AccountID, aData.Balance);
            }

            if (aData.AccountType == "Investment")
            {
                return new InvestmentAccount(
                    aData.AccountID,
                    aData.Balance,
                    aData.InterestRate,
                    aData.FailedFee
                );
            }

            if (aData.AccountType == "Omni")
            {
                return new OmniAccount(
                    aData.AccountID,
                    aData.Balance,
                    aData.InterestRate,
                    aData.OverdraftLimit,
                    aData.FailedFee
                );
            }

            return null;
        }

        /// <summary>
        /// Finds the next account id after loading saved data.
        /// </summary>
        /// <returns>The next account id value.</returns>
        private int GetNextAccountIdFromLoadedData()
        {
            int maxId = 1000;

            foreach (Customer customer in customers)
            {
                foreach (Account acc in customer.GetAccounts())
                {
                    if (acc.GetAccountID() > maxId)
                    {
                        maxId = acc.GetAccountID();
                    }
                }
            }

            return maxId + 1;
        }
    }
}