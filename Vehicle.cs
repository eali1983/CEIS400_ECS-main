using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEIS400_ECS
{
    public class Vehicle : Trackable
    {
        // Attributes
        public override string Source { get { return VehicleID; } set { VehicleID = value; } }

        protected string _vehicleID;
        protected string _make;
        protected string _model;
        protected int _year;
        protected string _serialNum;
        protected InvStatus _status;
        protected DateTime? _inDate;
        protected DateTime? _outDate;
        protected string _remarks;
        protected bool _certRequired;
        protected BindingList<CheckoutRecord> _checkoutRecords;


        // Constructors
        public Vehicle()
        {
            _vehicleID = "";
            _make = "";
            _model = "";
            _year = 0000;
            _serialNum = "";
            _status = InvStatus.In;
            _inDate = null;
            _outDate = null;
            _remarks = "";
            _certRequired = false;
            _checkoutRecords = new BindingList<CheckoutRecord>();
            Barcode = new Barcode();
        }

        public Vehicle(string vehicleID, string make, string model, int year, string serialNum, InvStatus status, DateTime? inDate, DateTime? outDate, string remarks, bool certRequired, BindingList<CheckoutRecord> checkoutRecords)
        {
            VehicleID = vehicleID;
            Make = make;
            Model = model;
            Year = year;
            SerialNum = serialNum;
            Status = status;
            InDate = inDate;
            OutDate = outDate;
            Remarks = remarks;
            CertRequired = certRequired;
            CheckoutRecords = checkoutRecords;
        }

        // Behaviors

        // -- Class Specific --
        public override string ToString()
        {
            return "temp text";
        }

        // -- Interface methods -- 

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
        public string VehicleID { get { return _vehicleID; } set { _vehicleID = value; } }
        public string Make { get { return _make; } set { _make = value; } }
        public string Model { get { return _model; } set { _model = value; } }
        public int Year { get { return _year; } set { _year = value; } }
        public string SerialNum { get { return _serialNum; } set { _serialNum = value; } }
        public override InvStatus Status { get { return _status; } set { _status = value; } }
        public override DateTime? InDate { get { return _inDate; } set { _inDate = value; } }
        public override DateTime? OutDate { get { return _outDate; } set { _outDate = value; } }
        public string Remarks { get { return _remarks; } set { _remarks = value; } }
        public bool CertRequired { get { return _certRequired; } set { _certRequired = value; } }
        public override BindingList<CheckoutRecord> CheckoutRecords { get { return _checkoutRecords; } set { _checkoutRecords = value; } }
    }
}
