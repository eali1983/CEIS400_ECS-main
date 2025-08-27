using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEIS400_ECS
{
    public class BasicTools : Trackable
    {
        // Attributes
        public override string Source { get => ToolID; set => ToolID = value; }

        protected string _toolID;
        protected string _name;
        protected DateTime? _inDate;
        protected DateTime? _outDate;
        protected InvStatus _status;
        protected List<string> _included;
        protected string _remarks;
        protected BindingList<CheckoutRecord> _checkoutRecords;

        // constructors
        public BasicTools()
        {
            _toolID = "";
            _name = "";
            _inDate = null;
            _outDate = null;
            _status = InvStatus.In;
            _included = new List<string>();
            _remarks = "";
            _checkoutRecords = new BindingList<CheckoutRecord>();
            Barcode = new Barcode();
        }

        public BasicTools(string toolID, string name, DateTime? inDate, DateTime? outDate, InvStatus status, List<string> included, string remarks, BindingList<CheckoutRecord> checkoutRecords)
        {
            ToolID = toolID;
            Name = name;
            InDate = inDate;
            OutDate = outDate;
            Status = status;
            Included = included;
            Remarks = remarks;
            CheckoutRecords = checkoutRecords;
        }

        // behaviors

        public override void GenerateID()
        {
            // Generate Unique ID for DB
            // ID Bindings equip to checkoutRecords
            ToolID = Guid.NewGuid().ToString();
        }

        public override void CheckIn(ref BindingList<CheckoutRecord> records, int index, Customer customer)
        {
            // Sets item status to out and adds DateTime to InDate
            // removes current OutDate value
            // CheckoutRecords list will hold all timestamps for each transaction
            InDate = DateTime.Now;
            Status = InvStatus.In;
            OutDate = null;

            CheckoutRecord updateRecord = records[index];

            updateRecord.DateIn = Convert.ToDateTime(InDate);

            customer.OutItems.Remove(updateRecord.Source);
        }

        public override void CheckOut(Customer customer)
        {
            var isInStock = CheckStock();
            // Sets item status to out and adds DateTime to OutDate
            // removes current InDate value
            // CheckoutRecords list will hold all timestamps for each transaction
            if (customer == null)
            {
                // If not EmpID is entered for checkout
                throw new ArgumentNullException("Customer required for checkout");
            }

            // Checks for EmpStatus. Makes sure customer has an active account
            if (customer.Status != EmpStatus.Active)
            {
                MessageBox.Show("Not Active", "Account not active. Check out denied", MessageBoxButtons.OK);
            }

            if (isInStock == Convert.ToBoolean(InvStatus.In))
            {
                Status = InvStatus.Out;
                OutDate = DateTime.Now;
                InDate = null;

                // Creates the CheckoutRecord for the item and adds the item to customers checkout list
                CheckoutRecord newRecord = new CheckoutRecord()
                {
                    RecordID = Guid.NewGuid().ToString(),
                    EmpID = customer.EmpID,
                    DateOut = Convert.ToDateTime(OutDate),
                    DateIn = Convert.ToDateTime(InDate),
                    Condition = Remarks,
                    Source = this
                };

                CheckoutRecords.Add(newRecord);
                customer.OutItems.Add(this);
            }

        }

        public override bool CheckStock()
        {
            // Checking if item is In or Out
            // If InDate has date it is in Stock
            if (InDate.HasValue)
            {
                return Status == InvStatus.In;
            }

            // If no InDate, then item is Out of Stock;
            return Status == InvStatus.Out;
        }

        public override bool IsMissing()
        {
            // Check for if OutDate has a date and if Equip status is "Out For Service"
            if (OutDate.HasValue && Status != InvStatus.OutForService)
            {
                // If out and not "Out For Service
                // Calculate the number of days missing
                // Greater than (2) Days will return Status as Missing and update Status to Missing.
                double daysLate = (DateTime.Now - OutDate.Value).TotalDays;
                if (daysLate >= CONST.DAYS_LATE_FLAG)
                {
                    Status = InvStatus.Missing;
                    return Status == InvStatus.Missing;
                }
            }

            // Otherwise it could still be out and in use
            // Return Status as Out
            return Status == InvStatus.Out;
        }

        // -- Class specific --
        public bool isIncluded()
        {
            // Checks if anything is in the Included list
            // Included can be for a toolbox that contains many tools
            // Will need to setup a ToString() method for the Included Items to print to screen or report
            if (Included.Count > 0)
            {
                return true;
            }

            // If nothing is Included
            return false;
        }

        public override string ToString()
        {
            return "temp text";
        }



        // properties
        public string ToolID { get { return _toolID; } set { _toolID = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public override DateTime? InDate { get { return _inDate; } set { _inDate = value; } }
        public override DateTime? OutDate { get { return _outDate; } set { _outDate = value; } }
        public override InvStatus Status { get { return _status; } set { _status = value; } }
        public List<string> Included { get { return _included; } set { _included = value; } }
        public string Remarks { get { return _remarks; } set { _remarks = value; } }
        public override BindingList<CheckoutRecord> CheckoutRecords { get { return _checkoutRecords; } set { _checkoutRecords = value; } }

    }
}
