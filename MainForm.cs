using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BankApp
{
    /// <summary>
    /// Main form used to manage customers in the banking app.
    /// It allows add, update, delete, select, and open account management.
    /// </summary>
    public class MainForm : Form
    {
        /// <summary>
        /// Stores the main bank controller.
        /// </summary>
        private BankController controller;

        /// <summary>
        /// Stores the file path for saved XML data.
        /// </summary>
        private string dataFile;

        /// <summary>
        /// Input box for customer number.
        /// </summary>
        private TextBox txtNumber;

        /// <summary>
        /// Input box for customer name.
        /// </summary>
        private TextBox txtName;

        /// <summary>
        /// Input box for customer contact.
        /// </summary>
        private TextBox txtContact;

        /// <summary>
        /// Checkbox for bank staff status.
        /// </summary>
        private CheckBox chkStaff;

        /// <summary>
        /// Button to add a customer.
        /// </summary>
        private Button btnAdd;

        /// <summary>
        /// Button to update a customer.
        /// </summary>
        private Button btnUpdate;

        /// <summary>
        /// Button to delete a customer.
        /// </summary>
        private Button btnDelete;

        /// <summary>
        /// Button to clear the input fields.
        /// </summary>
        private Button btnClear;

        /// <summary>
        /// Button to open account management form.
        /// </summary>
        private Button btnManage;

        /// <summary>
        /// Grid showing all customers.
        /// </summary>
        private DataGridView dgvCustomers;

        /// <summary>
        /// Creates the main form and loads saved data.
        /// </summary>
        public MainForm()
        {
            controller = new BankController();
            string folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "BankApp"
            );

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            dataFile = Path.Combine(folder, "bankdata.xml");
            BuildUi();
            controller.LoadData(dataFile);
            RefreshCustomerGrid();

            FormClosing += MainForm_FormClosing;
        }

        /// <summary>
        /// Builds all controls used on the form.
        /// </summary>
        private void BuildUi()
        {
            Text = "MyBank";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(980, 620);
            BackColor = Color.FromArgb(245, 247, 250);

            Panel header = new Panel();
            header.Dock = DockStyle.Top;
            header.Height = 80;
            header.BackColor = Color.FromArgb(30, 39, 46);
            Controls.Add(header);

            Label lblTitle = new Label();
            lblTitle.Text = "MyBank - Edit Customers";
            lblTitle.ForeColor = Color.White;
            lblTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(20, 22);
            header.Controls.Add(lblTitle);

            Label lblNumber = new Label();
            lblNumber.Text = "Customer No";
            lblNumber.Location = new Point(30, 120);
            lblNumber.AutoSize = true;
            lblNumber.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            Controls.Add(lblNumber);

            txtNumber = new TextBox();
            txtNumber.Location = new Point(150, 116);
            txtNumber.Size = new Size(220, 30);
            Controls.Add(txtNumber);

            Label lblName = new Label();
            lblName.Text = "Name";
            lblName.Location = new Point(30, 165);
            lblName.AutoSize = true;
            lblName.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            Controls.Add(lblName);

            txtName = new TextBox();
            txtName.Location = new Point(150, 161);
            txtName.Size = new Size(220, 30);
            Controls.Add(txtName);

            Label lblContact = new Label();
            lblContact.Text = "Contact";
            lblContact.Location = new Point(30, 210);
            lblContact.AutoSize = true;
            lblContact.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            Controls.Add(lblContact);

            txtContact = new TextBox();
            txtContact.Location = new Point(150, 206);
            txtContact.Size = new Size(220, 30);
            Controls.Add(txtContact);

            chkStaff = new CheckBox();
            chkStaff.Text = "Bank Staff";
            chkStaff.Location = new Point(150, 248);
            chkStaff.AutoSize = true;
            chkStaff.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            Controls.Add(chkStaff);

            btnAdd = new Button();
            btnAdd.Text = "Add";
            btnAdd.Location = new Point(30, 300);
            btnAdd.Size = new Size(100, 38);
            btnAdd.BackColor = Color.FromArgb(52, 152, 219);
            btnAdd.ForeColor = Color.White;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Click += BtnAdd_Click;
            Controls.Add(btnAdd);

            btnUpdate = new Button();
            btnUpdate.Text = "Update";
            btnUpdate.Location = new Point(145, 300);
            btnUpdate.Size = new Size(100, 38);
            btnUpdate.BackColor = Color.FromArgb(39, 174, 96);
            btnUpdate.ForeColor = Color.White;
            btnUpdate.FlatStyle = FlatStyle.Flat;
            btnUpdate.Click += BtnUpdate_Click;
            Controls.Add(btnUpdate);

            btnDelete = new Button();
            btnDelete.Text = "Delete";
            btnDelete.Location = new Point(260, 300);
            btnDelete.Size = new Size(100, 38);
            btnDelete.BackColor = Color.FromArgb(231, 76, 60);
            btnDelete.ForeColor = Color.White;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Click += BtnDelete_Click;
            Controls.Add(btnDelete);

            btnClear = new Button();
            btnClear.Text = "Clear";
            btnClear.Location = new Point(30, 355);
            btnClear.Size = new Size(100, 38);
            btnClear.BackColor = Color.FromArgb(127, 140, 141);
            btnClear.ForeColor = Color.White;
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.Click += BtnClear_Click;
            Controls.Add(btnClear);

            btnManage = new Button();
            btnManage.Text = "Manage Accounts";
            btnManage.Location = new Point(145, 355);
            btnManage.Size = new Size(215, 38);
            btnManage.BackColor = Color.FromArgb(142, 68, 173);
            btnManage.ForeColor = Color.White;
            btnManage.FlatStyle = FlatStyle.Flat;
            btnManage.Click += BtnManage_Click;
            Controls.Add(btnManage);

            dgvCustomers = new DataGridView();
            dgvCustomers.Location = new Point(420, 116);
            dgvCustomers.Size = new Size(520, 400);
            dgvCustomers.AllowUserToAddRows = false;
            dgvCustomers.AllowUserToDeleteRows = false;
            dgvCustomers.ReadOnly = true;
            dgvCustomers.MultiSelect = false;
            dgvCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCustomers.RowHeadersVisible = false;
            dgvCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCustomers.BackgroundColor = Color.White;
            dgvCustomers.Columns.Add("CustomerNo", "Customer No");
            dgvCustomers.Columns.Add("Name", "Name");
            dgvCustomers.Columns.Add("Contact", "Contact");
            dgvCustomers.Columns.Add("Staff", "Staff");
            dgvCustomers.SelectionChanged += DgvCustomers_SelectionChanged;
            Controls.Add(dgvCustomers);
        }

        /// <summary>
        /// Reloads all customers into the grid.
        /// </summary>
        private void RefreshCustomerGrid()
        {
            dgvCustomers.Rows.Clear();

            foreach (Customer customer in controller.GetCustomers())
            {
                string staffText = customer.IsStaff() ? "Yes" : "No";

                dgvCustomers.Rows.Add(
                    customer.GetCustomerNumber(),
                    customer.GetName(),
                    customer.GetContact(),
                    staffText
                );
            }
        }

        /// <summary>
        /// Loads the selected customer values into the input boxes.
        /// </summary>
        private void LoadSelectedCustomer()
        {
            if (dgvCustomers.CurrentRow == null)
            {
                return;
            }

            if (dgvCustomers.CurrentRow.Cells[0].Value == null)
            {
                return;
            }

            txtNumber.Text = dgvCustomers.CurrentRow.Cells[0].Value.ToString();
            txtName.Text = dgvCustomers.CurrentRow.Cells[1].Value.ToString();
            txtContact.Text = dgvCustomers.CurrentRow.Cells[2].Value.ToString();
            chkStaff.Checked = dgvCustomers.CurrentRow.Cells[3].Value.ToString() == "Yes";
        }

        /// <summary>
        /// Clears the customer input controls.
        /// </summary>
        private void ClearInputs()
        {
            txtNumber.Text = "";
            txtName.Text = "";
            txtContact.Text = "";
            chkStaff.Checked = false;
        }

        /// <summary>
        /// Saves the current data when the form closes.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Form closing event data.</param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            controller.SaveData(dataFile);
        }

        /// <summary>
        /// Runs when selected customer row changes.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event data.</param>
        private void DgvCustomers_SelectionChanged(object sender, EventArgs e)
        {
            LoadSelectedCustomer();
        }

        /// <summary>
        /// Adds a new customer into the system.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event data.</param>
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            int number;

            if (!int.TryParse(txtNumber.Text.Trim(), out number))
            {
                MessageBox.Show("Enter valid customer number.");
                return;
            }

            string name = txtName.Text.Trim();
            string contact = txtContact.Text.Trim();
            bool isStaff = chkStaff.Checked;

            if (name == "")
            {
                MessageBox.Show("Enter customer name.");
                return;
            }

            string msg;
            controller.AddCustomer(number, name, contact, isStaff, out msg);
            MessageBox.Show(msg);
            RefreshCustomerGrid();
            ClearInputs();
        }

        /// <summary>
        /// Updates the selected customer details.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event data.</param>
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            int number;

            if (!int.TryParse(txtNumber.Text.Trim(), out number))
            {
                MessageBox.Show("Enter valid customer number.");
                return;
            }

            string name = txtName.Text.Trim();
            string contact = txtContact.Text.Trim();
            bool isStaff = chkStaff.Checked;

            if (name == "")
            {
                MessageBox.Show("Enter customer name.");
                return;
            }

            string msg;
            controller.UpdateCustomer(number, name, contact, isStaff, out msg);
            MessageBox.Show(msg);
            RefreshCustomerGrid();
        }

        /// <summary>
        /// Deletes the selected customer.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event data.</param>
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            int number;

            if (!int.TryParse(txtNumber.Text.Trim(), out number))
            {
                MessageBox.Show("Select a customer first.");
                return;
            }

            string msg;
            controller.DeleteCustomer(number, out msg);
            MessageBox.Show(msg);
            RefreshCustomerGrid();
            ClearInputs();
        }

        /// <summary>
        /// Clears the form input fields.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event data.</param>
        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        /// <summary>
        /// Opens the account management form for the selected customer.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event data.</param>
        private void BtnManage_Click(object sender, EventArgs e)
        {
            int number;

            if (!int.TryParse(txtNumber.Text.Trim(), out number))
            {
                MessageBox.Show("Select a customer first.");
                return;
            }

            Customer customer = controller.FindCustomer(number);

            if (customer == null)
            {
                MessageBox.Show("Customer not found.");
                return;
            }

            ManageAccountsForm frm = new ManageAccountsForm(controller, customer);
            frm.ShowDialog();
            RefreshCustomerGrid();
        }
    }
}