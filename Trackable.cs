using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEIS400_ECS
{
    public abstract class Trackable : ITrackable, IHasBarcode
    {
        public abstract string Source { get; set; }
        public Barcode Barcode { get; protected set; } = new Barcode();
        public virtual InvStatus Status { get; set; } = InvStatus.In;
        public abstract DateTime? InDate { get; set; }
        public abstract DateTime? OutDate { get; set; }
        public virtual BindingList<CheckoutRecord> CheckoutRecords { get; set; } = new BindingList<CheckoutRecord>();

        public virtual void GenerateBarcode() => Barcode.Generate(Source);

        public virtual void GenerateID() => Source = Guid.NewGuid().ToString();

        public virtual bool CheckStock()
        {
            return Status == InvStatus.In;
        }

        public virtual bool IsMissing()
        {
            return Status == InvStatus.Missing;
        }

        public virtual void CheckOut(Customer customer)
        {
            if (Status != InvStatus.In)
            {
                throw new InvalidOperationException("Not available for checkout");
            }

            Status = InvStatus.Out;
            OutDate = DateTime.Now;
        }

        public virtual void CheckIn(ref BindingList<CheckoutRecord> records, int index, Customer customer)
        {
            if (Status != InvStatus.Out)
            {
                throw new InvalidOperationException("Not checked out");
            }

            Status = InvStatus.In;
            InDate = DateTime.Now;
        }
    }
}
