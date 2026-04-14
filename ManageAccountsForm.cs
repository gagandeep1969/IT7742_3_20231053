using System;
using System.Drawing;
using System.Windows.Forms;

namespace BankApp
{
    /// <summary>
    /// Form used to manage the accounts of one selected customer.
    /// It allows adding accounts and performing account transactions.
    /// </summary>
    public class ManageAccountsForm : Form
    {
        /// <summary>
        /// Stores the main bank controller.
        /// </summary>
        private BankController controller;

        /// <summary>
        /// Stores the selected customer.
        /// </summary>
        private Customer customer;

        /// <summary>
        /// Grid showing all accounts for the customer.
        /// </summary>
        private DataGridView dgvAccounts;

        /// <summary>
        /// Lets the user choose a new account type.
        /// </summary>
        private ComboBox cmbType;

        /// <summary>
        /// Input for opening balance.
        /// </summary>
        private TextBox txtOpening;

        /// <summary>
        /// Input for transaction amount.
        /// </summary>
        private TextBox txtAmount;

        /// <summary>
        /// Lets the user choose the target account for transfer.
        /// </summary>
        private ComboBox cmbTarget;

        /// <summary>
        /// Shows selected customer details.
        /// </summary>
        private Label lblCustomer;

        /// <summary>
        /// Shows selected account details.
        /// </summary>
        private Label lblSelected;

        /// <summary>
        /// Shows the last result message.
        /// </summary>
        private Label lblLast;

        /// <summary>
        /// Button to add a new account.
        /// </summary>
        private Button btnAddAccount;

        /// <summary>
        /// Button to deposit money.
        /// </summary>
        private Button btnDeposit;

        /// <summary>
        /// Button to withdraw money.
        /// </summary>
        private Button btnWithdraw;

        /// <summary>
        /// Button to transfer money.
        /// </summary>
        private Button btnTransfer;

        /// <summary>
        /// Button to add interest.
        /// </summary>
        private Button btnInterest;

        /// <summary>
        /// Button to refresh the form display.
        /// </summary>
        private Button btnRefresh;

        /// <summary>
        /// Creates the manage accounts form for the selected customer.
        /// </summary>
        /// <param name="controller">Main bank controller.</param>
        /// <param name="customer">Selected customer.</param>
        public ManageAccountsForm(BankController controller, Customer customer)
        {
            this.controller = controller;
            this.customer = customer;

            BuildUi();
            RefreshAccountGrid();
            RefreshTargetList();
        }

        /// <summary>
        /// Builds all controls used on the form.
        /// </summary>
        private void BuildUi()
        {
            Text = "Manage Accounts";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(980, 620);
            BackColor = Color.FromArgb(248, 249, 250);

            Panel header = new Panel();
            header.Dock = DockStyle.Top;
            header.Height = 80;
            header.BackColor = Color.FromArgb(44, 62, 80);
            Controls.Add(header);

            Label lblTitle = new Label();
            lblTitle.Text = "MyBank - Manage Accounts";
            lblTitle.ForeColor = Color.White;
            lblTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(20, 22);
            header.Controls.Add(lblTitle);

            lblCustomer = new Label();
            string staffText = customer.IsStaff() ? "Staff" : "Customer";
            lblCustomer.Text = customer.GetCustomerNumber() + " - " + customer.GetName() + " (" + staffText + ")";
            lblCustomer.Location = new Point(20, 95);
            lblCustomer.AutoSize = true;
            lblCustomer.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            Controls.Add(lblCustomer);

            dgvAccounts = new DataGridView();
            dgvAccounts.Location = new Point(20, 130);
            dgvAccounts.Size = new Size(560, 300);
            dgvAccounts.AllowUserToAddRows = false;
            dgvAccounts.AllowUserToDeleteRows = false;
            dgvAccounts.ReadOnly = true;
            dgvAccounts.MultiSelect = false;
            dgvAccounts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAccounts.RowHeadersVisible = false;
            dgvAccounts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAccounts.BackgroundColor = Color.White;
            dgvAccounts.Columns.Add("AccountId", "Account ID");
            dgvAccounts.Columns.Add("Type", "Type");
            dgvAccounts.Columns.Add("Balance", "Balance");
            dgvAccounts.Columns.Add("Rate", "Interest %");
            dgvAccounts.Columns.Add("OD", "Overdraft");
            dgvAccounts.SelectionChanged += DgvAccounts_SelectionChanged;
            Controls.Add(dgvAccounts);

            GroupBox grpAdd = new GroupBox();
            grpAdd.Text = "Add New Account";
            grpAdd.Location = new Point(610, 130);
            grpAdd.Size = new Size(320, 145);
            Controls.Add(grpAdd);

            Label lblType = new Label();
            lblType.Text = "Type";
            lblType.Location = new Point(20, 35);
            lblType.AutoSize = true;
            grpAdd.Controls.Add(lblType);

            cmbType = new ComboBox();
            cmbType.Location = new Point(120, 31);
            cmbType.Size = new Size(160, 30);
            cmbType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbType.Items.Add("Everyday");
            cmbType.Items.Add("Investment");
            cmbType.Items.Add("Omni");
            cmbType.SelectedIndex = 0;
            grpAdd.Controls.Add(cmbType);

            Label lblOpening = new Label();
            lblOpening.Text = "Opening";
            lblOpening.Location = new Point(20, 75);
            lblOpening.AutoSize = true;
            grpAdd.Controls.Add(lblOpening);

            txtOpening = new TextBox();
            txtOpening.Location = new Point(120, 71);
            txtOpening.Size = new Size(160, 30);
            grpAdd.Controls.Add(txtOpening);

            btnAddAccount = new Button();
            btnAddAccount.Text = "Add Account";
            btnAddAccount.Location = new Point(120, 105);
            btnAddAccount.Size = new Size(160, 30);
            btnAddAccount.BackColor = Color.FromArgb(41, 128, 185);
            btnAddAccount.ForeColor = Color.White;
            btnAddAccount.FlatStyle = FlatStyle.Flat;
            btnAddAccount.Click += BtnAddAccount_Click;
            grpAdd.Controls.Add(btnAddAccount);

            GroupBox grpTrans = new GroupBox();
            grpTrans.Text = "Transactions";
            grpTrans.Location = new Point(610, 290);
            grpTrans.Size = new Size(320, 200);
            Controls.Add(grpTrans);

            Label lblAmount = new Label();
            lblAmount.Text = "Amount";
            lblAmount.Location = new Point(20, 35);
            lblAmount.AutoSize = true;
            grpTrans.Controls.Add(lblAmount);

            txtAmount = new TextBox();
            txtAmount.Location = new Point(120, 31);
            txtAmount.Size = new Size(160, 30);
            grpTrans.Controls.Add(txtAmount);

            btnDeposit = new Button();
            btnDeposit.Text = "Deposit";
            btnDeposit.Location = new Point(20, 70);
            btnDeposit.Size = new Size(120, 32);
            btnDeposit.BackColor = Color.FromArgb(39, 174, 96);
            btnDeposit.ForeColor = Color.White;
            btnDeposit.FlatStyle = FlatStyle.Flat;
            btnDeposit.Click += BtnDeposit_Click;
            grpTrans.Controls.Add(btnDeposit);

            btnWithdraw = new Button();
            btnWithdraw.Text = "Withdraw";
            btnWithdraw.Location = new Point(160, 70);
            btnWithdraw.Size = new Size(120, 32);
            btnWithdraw.BackColor = Color.FromArgb(192, 57, 43);
            btnWithdraw.ForeColor = Color.White;
            btnWithdraw.FlatStyle = FlatStyle.Flat;
            btnWithdraw.Click += BtnWithdraw_Click;
            grpTrans.Controls.Add(btnWithdraw);

            Label lblTarget = new Label();
            lblTarget.Text = "Target";
            lblTarget.Location = new Point(20, 115);
            lblTarget.AutoSize = true;
            grpTrans.Controls.Add(lblTarget);

            cmbTarget = new ComboBox();
            cmbTarget.Location = new Point(120, 111);
            cmbTarget.Size = new Size(160, 30);
            cmbTarget.DropDownStyle = ComboBoxStyle.DropDownList;
            grpTrans.Controls.Add(cmbTarget);

            btnTransfer = new Button();
            btnTransfer.Text = "Transfer";
            btnTransfer.Location = new Point(20, 150);
            btnTransfer.Size = new Size(120, 32);
            btnTransfer.BackColor = Color.FromArgb(142, 68, 173);
            btnTransfer.ForeColor = Color.White;
            btnTransfer.FlatStyle = FlatStyle.Flat;
            btnTransfer.Click += BtnTransfer_Click;
            grpTrans.Controls.Add(btnTransfer);

            btnInterest = new Button();
            btnInterest.Text = "Add Interest";
            btnInterest.Location = new Point(160, 150);
            btnInterest.Size = new Size(120, 32);
            btnInterest.BackColor = Color.FromArgb(243, 156, 18);
            btnInterest.ForeColor = Color.White;
            btnInterest.FlatStyle = FlatStyle.Flat;
            btnInterest.Click += BtnInterest_Click;
            grpTrans.Controls.Add(btnInterest);

            btnRefresh = new Button();
            btnRefresh.Text = "Refresh";
            btnRefresh.Location = new Point(810, 505);
            btnRefresh.Size = new Size(120, 34);
            btnRefresh.BackColor = Color.FromArgb(127, 140, 141);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Click += BtnRefresh_Click;
            Controls.Add(btnRefresh);

            lblSelected = new Label();
            lblSelected.Text = "Selected Account:";
            lblSelected.Location = new Point(20, 455);
            lblSelected.Size = new Size(560, 30);
            lblSelected.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            Controls.Add(lblSelected);

            lblLast = new Label();
            lblLast.Text = "Message:";
            lblLast.Location = new Point(20, 490);
            lblLast.Size = new Size(760, 50);
            lblLast.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            Controls.Add(lblLast);
        }

        /// <summary>
        /// Reloads the accounts into the grid.
        /// </summary>
        private void RefreshAccountGrid()
        {
            dgvAccounts.Rows.Clear();

            foreach (Account acc in controller.GetCustomerAccounts(customer.GetCustomerNumber()))
            {
                dgvAccounts.Rows.Add(
                    acc.GetAccountID(),
                    acc.GetAccountType(),
                    acc.GetBalance().ToString("0.00"),
                    acc.GetInterestRate().ToString("0.00"),
                    acc.GetOverdraftLimit().ToString("0.00")
                );
            }

            ShowSelectedInfo();
        }

        /// <summary>
        /// Reloads the target account list for transfers.
        /// </summary>
        private void RefreshTargetList()
        {
            cmbTarget.Items.Clear();

            Account selected = GetSelectedAccount();

            foreach (Account acc in controller.GetCustomerAccounts(customer.GetCustomerNumber()))
            {
                if (selected == null || acc.GetAccountID() != selected.GetAccountID())
                {
                    cmbTarget.Items.Add(acc);
                }
            }

            if (cmbTarget.Items.Count > 0)
            {
                cmbTarget.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Returns the selected account from the grid.
        /// </summary>
        /// <returns>The selected account or null.</returns>
        private Account GetSelectedAccount()
        {
            if (dgvAccounts.CurrentRow == null)
            {
                return null;
            }

            int id = Convert.ToInt32(dgvAccounts.CurrentRow.Cells[0].Value);
            return customer.FindAccount(id);
        }

        /// <summary>
        /// Shows details of the selected account.
        /// </summary>
        private void ShowSelectedInfo()
        {
            Account acc = GetSelectedAccount();

            if (acc == null)
            {
                lblSelected.Text = "Selected Account:";
                return;
            }

            lblSelected.Text = "Selected Account: " +
                               acc.GetAccountType() +
                               " | ID: " + acc.GetAccountID() +
                               " | Balance: " + acc.GetBalance().ToString("0.00");
        }

        /// <summary>
        /// Runs when selected account row changes.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event data.</param>
        private void DgvAccounts_SelectionChanged(object sender, EventArgs e)
        {
            ShowSelectedInfo();
            RefreshTargetList();
        }

        /// <summary>
        /// Adds a new account for the selected customer.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event data.</param>
        private void BtnAddAccount_Click(object sender, EventArgs e)
        {
            decimal opening;

            if (!decimal.TryParse(txtOpening.Text.Trim(), out opening))
            {
                MessageBox.Show("Enter valid opening amount.");
                return;
            }

            string msg;
            controller.AddAccountToCustomer(customer.GetCustomerNumber(), cmbType.Text, opening, out msg);
            MessageBox.Show(msg);

            txtOpening.Text = "";
            RefreshAccountGrid();
            RefreshTargetList();
        }

        /// <summary>
        /// Deposits money into the selected account.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event data.</param>
        private void BtnDeposit_Click(object sender, EventArgs e)
        {
            Account acc = GetSelectedAccount();

            if (acc == null)
            {
                MessageBox.Show("Select an account first.");
                return;
            }

            decimal amount;

            if (!decimal.TryParse(txtAmount.Text.Trim(), out amount))
            {
                MessageBox.Show("Enter valid amount.");
                return;
            }

            string msg;
            controller.Deposit(customer.GetCustomerNumber(), acc.GetAccountID(), amount, out msg);
            lblLast.Text = "Message: " + msg;
            RefreshAccountGrid();
        }

        /// <summary>
        /// Withdraws money from the selected account.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event data.</param>
        private void BtnWithdraw_Click(object sender, EventArgs e)
        {
            Account acc = GetSelectedAccount();

            if (acc == null)
            {
                MessageBox.Show("Select an account first.");
                return;
            }

            decimal amount;

            if (!decimal.TryParse(txtAmount.Text.Trim(), out amount))
            {
                MessageBox.Show("Enter valid amount.");
                return;
            }

            string msg;
            controller.Withdraw(customer.GetCustomerNumber(), acc.GetAccountID(), amount, out msg);
            lblLast.Text = "Message: " + msg;
            RefreshAccountGrid();
        }

        /// <summary>
        /// Transfers money from selected account to target account.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event data.</param>
        private void BtnTransfer_Click(object sender, EventArgs e)
        {
            Account fromAcc = GetSelectedAccount();

            if (fromAcc == null)
            {
                MessageBox.Show("Select source account.");
                return;
            }

            if (cmbTarget.SelectedItem == null)
            {
                MessageBox.Show("Select target account.");
                return;
            }

            Account toAcc = (Account)cmbTarget.SelectedItem;

            decimal amount;

            if (!decimal.TryParse(txtAmount.Text.Trim(), out amount))
            {
                MessageBox.Show("Enter valid amount.");
                return;
            }

            string msg;
            controller.Transfer(customer.GetCustomerNumber(), fromAcc.GetAccountID(), toAcc.GetAccountID(), amount, out msg);
            lblLast.Text = "Message: " + msg;
            RefreshAccountGrid();
            RefreshTargetList();
        }

        /// <summary>
        /// Adds interest to the selected account.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event data.</param>
        private void BtnInterest_Click(object sender, EventArgs e)
        {
            Account acc = GetSelectedAccount();

            if (acc == null)
            {
                MessageBox.Show("Select an account first.");
                return;
            }

            string msg;
            controller.AddInterest(customer.GetCustomerNumber(), acc.GetAccountID(), out msg);
            lblLast.Text = "Message: " + msg;
            RefreshAccountGrid();
        }

        /// <summary>
        /// Refreshes the form display values.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event data.</param>
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            RefreshAccountGrid();
            RefreshTargetList();
        }
    }
}
