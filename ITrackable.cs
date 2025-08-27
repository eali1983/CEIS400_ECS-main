using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEIS400_ECS
{
    public interface ITrackable
    {
        string Source { get; }
        InvStatus Status { get; set; }
        DateTime? InDate { get; set; }
        DateTime? OutDate { get; set; }
        Barcode Barcode { get; }
        BindingList<CheckoutRecord> CheckoutRecords { get; set; }
        bool CheckStock();
        void CheckIn(ref BindingList<CheckoutRecord> records, int index, Customer customer);
        void CheckOut(Customer customer);
        bool IsMissing();
        void GenerateID();
    }
}