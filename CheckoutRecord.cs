using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEIS400_ECS
{
    public class CheckoutRecord
    {
        // attributes
        protected string _recordID;
        protected string _empID;
        protected DateTime _dateOut;
        protected DateTime? _dateIn;
        protected string _condition;
        protected ITrackable _source;

        // constructors
        public CheckoutRecord()
        {
            _recordID = "";
            _empID = "";
            _dateOut = DateTime.Now;
            _dateIn = null;
            _condition = "";
            _source = null;
        }

        public CheckoutRecord(string recordID, string empID, DateTime dateOut, DateTime? dateIn, string condition, ITrackable source)
        {
            RecordID = recordID;
            EmpID = empID;
            DateOut = dateOut;
            DateIn = dateIn;
            Condition = condition;
            Source = source;
        }

        // behaviors

        // properties
        public string RecordID { get { return _recordID; } set { _recordID = value; } }
        public string EmpID { get { return _empID; } set { _empID = value; } }
        public DateTime DateOut { get { return _dateOut; } set { _dateOut = value; } }
        public DateTime? DateIn { get { return _dateIn; } set { _dateIn = value; } }
        public string Condition { get { return _condition; } set { _condition = value; } }
        public ITrackable Source { get { return _source; } set { _source = value; } }
    }
}
