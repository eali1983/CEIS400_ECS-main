using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CEIS400_ECS
{
    public class SpecialTool : Trackable
    {
        // Attributes
        public override string Source { get { return SToolID; } set { SToolID = value; } }

        protected string _sToolID;
        protected string _name;
        protected string _type;
        protected InvStatus _status;
        protected DateTime? _inDate;
        protected DateTime? _outDate;
        protected string _remarks;
        protected DateTime? _calDate;
        protected DateTime? _calDue;
        protected bool _certRequired;
        protected List<string> _included;
        protected BindingList<CheckoutRecord> _checkoutRecords;

        // Constructors
        public SpecialTool()
        {
            _sToolID = "";
            _name = "";
            _type = "";
            _status = InvStatus.In;
            _inDate = null;
            _outDate = null;
            _remarks = "";
            _calDate = null;
            _calDue = null;
            _certRequired = false;
            _included = new List<string>();
            _checkoutRecords = new BindingList<CheckoutRecord>();
            Barcode = new Barcode();
        }

        public SpecialTool(string sToolID, string name, string type, InvStatus status, DateTime? inDate, DateTime? outDate, string remarks, DateTime? calDate, DateTime? calDue, bool certRequired, List<string> included, BindingList<CheckoutRecord> checkoutRecords)
        {
            SToolID = sToolID;
            Name = name;
            Type = type;
            Status = status;
            InDate = inDate;
            OutDate = outDate;
            Remarks = remarks;
            CalDate = calDate;
            CalDue = calDue;
            CertRequired = certRequired;
            Included = included;
            CheckoutRecords = checkoutRecords;
        }

        // Behaviors
        // -- Class specific --
        public bool IsIncluded()
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

        public bool DueForCalibration()
        {
            // Check if the calibration due date is greater than or equal to today
            // Return true if it is
            if (DateTime.Now >= CalDue)
            {
                return true;
            }

            // Return false if it is not
            return false;
        }

        public override string ToString()
        {
            return "temp text";
        }

        // -- Interface methods --
        public override void GenerateID()
        {
            SToolID = Guid.NewGuid().ToString();
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
            // Sets item status to out and adds DateTime to OutDate
            // removes current InDate value
            // CheckoutRecords list will hold all timestamps for each transaction
            if (customer == null)
            {
                // If not EmpID is entered for checkout
                throw new ArgumentNullException("Customer required for checkout");
            }

            // Check it customer is certified for checking this item out
            if (CertRequired)
            {
                // Denied message if they are not certified
                if (!customer.IsCertified(this))
                {
                    MessageBox.Show("Denied", "Not certified for item checkout", MessageBoxButtons.OK);
                }
            }

            // Checks for EmpStatus. Makes sure customer has an active account
            if (customer.Status != EmpStatus.Active)
            {
                MessageBox.Show("Not Active", "Account not active. Check out denied", MessageBoxButtons.OK);
            }

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
                if (daysLate >= 2)
                {
                    Status = InvStatus.Missing;
                    return Status == InvStatus.Missing;
                }
            }

            // Otherwise it could still be out and in use
            // Return Status as Out
            return Status == InvStatus.Out;
        }


        // Properties
        public string SToolID { get { return _sToolID; } set { _sToolID = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public string Type { get { return _type; } set { _type = value; } }
        public override InvStatus Status { get { return _status; } set { _status = value; } }
        public override DateTime? InDate { get { return _inDate; } set { _inDate = value; } }
        public override DateTime? OutDate { get { return _outDate; } set { _outDate = value; } }
        public string Remarks { get { return _remarks; } set { _remarks = value; } }
        public DateTime? CalDate { get { return _calDate; } set { _calDate = value; } }
        public DateTime? CalDue { get { return _calDue; } set { _calDue = value; } }
        public bool CertRequired { get { return _certRequired; } set { _certRequired = value; } }
        public List<string> Included { get { return _included; } set { _included = value; } }
        public override BindingList<CheckoutRecord> CheckoutRecords { get { return _checkoutRecords; } set { _checkoutRecords = value; } }

    }
}
