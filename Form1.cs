using System.Data;

namespace CEIS400_ECS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkOutButton_Click(object sender, EventArgs e)
        {
            // Lookup the tool object in SQL with equipID from textbox (assuming all tools stored in the same database)
            Trackable source;

            // Lookup the checkoutRecord object in SQL with equipID from textbox
            CheckoutRecord record; // <-- Needs SQL query

            // Get associated customer object
            // string record.EmpID; <-- Needs SQL query
            Customer customer;

            // deptstaff.CheckOutEquip(source, customer);
        }

        private void checkInButton_Click(object sender, EventArgs e)
        {
            // Lookup the tool object in SQL with equipID from textbox (assuming all tools stored in the same database)
            Trackable source;

            // Lookup the checkoutRecord object in SQL with equipID from textbox
            CheckoutRecord record; // <-- Needs SQL query

            // Get associated object
            // record.EmpID; <-- Needs SQL query
            Customer customer;

            // deptstaff.CheckInEquip(source, recordID, customer);
        }

        private void updateInventoryButton_Click(object sender, EventArgs e)
        {

        }

        private void trackInventoryButton_Click(object sender, EventArgs e)
        {

        }

        private void generateReportsButton_Click(object sender, EventArgs e)
        {

        }
    }
}
