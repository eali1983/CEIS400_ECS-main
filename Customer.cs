using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEIS400_ECS
{
    public class Customer : Employee
    {
        private List<ITrackable> _certs;
        private List<ITrackable> _outItems;

        public Customer(string empID, string name, string email, string phone, EmpStatus status, string title, string passwordHash, string passwordSalt, List<ITrackable> certs, List<ITrackable> outItems)
            : base(empID, name, email, phone, status, title, passwordHash, passwordSalt, Roles.Customer)
        {
            Certs = certs ?? new List<ITrackable>();
            OutItems = outItems ?? new List<ITrackable>();
        }

        public Customer()
        {

        }

        public string ViewCerts()
        {
            return Certs.ToString();
        }

        public bool IsCertified(ITrackable certToCheck)
        {
            // Checks to see if the Cert required equip cert
            // matches the Customers certs list
            // If a match they can check out the item
            return Certs.Contains(certToCheck);
        }

        public string ViewItemsOut()
        {
            // Checks it items are currently out
            // Return list of items
            // Or return no items out message
            if (OutItems.Count > 0)
            {
                return OutItems.ToString();
            }

            return "No items currently checked out.";
        }

        public void RequestEquip()
        {
            // For future use.
        }

        public string AcknowledgeReturn()
        {
            // For future use, add IMAP/POP service layer that sends acknowledgement emails to Customers once equip checked in
            return "";
        }

        // Properties
        public List<ITrackable> Certs { get { return _certs; } set { _certs = value; } }
        public List<ITrackable> OutItems { get { return _outItems; } set { _outItems = value; } }
    }
}
