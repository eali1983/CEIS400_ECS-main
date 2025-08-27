using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEIS400_ECS
{
    public class DeptStaff : Employee
    {
        public DeptStaff(string empID, string name, string email, string phone, EmpStatus status, string title, string passwordHash, string passwordSalt)
            : base(empID, name, email, phone, status, title, passwordHash, passwordSalt, Roles.DeptStaff)
        {

        }

        public void CheckInEquip(ITrackable source, string recordID, Customer customer)
        {
            if (Role == Roles.DeptStaff || SecondRole.HasValue && SecondRole == Roles.DeptStaff)
            {
                var records = source.CheckoutRecords;
                CheckoutRecord recordToClose = null;
                recordToClose = records.FirstOrDefault(r => r.RecordID == recordID);
                int index = source.CheckoutRecords.IndexOf(recordToClose);
                source.CheckIn(ref records, index, customer);
                MessageBox.Show("Success", "Check in successful.", MessageBoxButtons.OK);
            }
            // Add that equip back into the system
        }

        public void CheckOutEquip(ITrackable source, Customer customer)
        {
            // Checks for DeptStaff role
            if (!(Role == Roles.DeptStaff || SecondRole.HasValue && SecondRole == Roles.DeptStaff))
            {
                // Checks if customer is Active to checkout
                if (customer.Status != EmpStatus.Active)
                {
                    string msg = customer.Status == EmpStatus.Hold ? "Account on hold. Please see Supervisor for details" : "Acount removed";
                    MessageBox.Show("Denied", msg);
                    return;
                }

                // Checks if tool requires Cert
                bool requiresCert = (source is Vehicle v && v.CertRequired) || (source is SpecialTool st && st.CertRequired);

                // Checks if customer has required Cert
                if (requiresCert && !customer.IsCertified(source))
                {
                    MessageBox.Show("Denied", "Not certified for checkout", MessageBoxButtons.OK);
                    return;
                }

                // If Everything is good, then check out
                source.CheckOut(customer);
                MessageBox.Show("Success", "Check out successful.", MessageBoxButtons.OK);
            }
        }

        public void UpdateEquip(ITrackable source, List<ITrackable> equipList)
        {
            if (!(Role == Roles.DeptStaff || SecondRole.HasValue && SecondRole == Roles.DeptStaff))
            {
                return;
            }

            // Determine if source is BasicTool, SpecialTool or Vehilce
            ITrackable exisiting = equipList.FirstOrDefault(e =>
            (e is BasicTools bt && source is BasicTools sbt && bt.ToolID == sbt.ToolID) ||
            (e is SpecialTool st && source is SpecialTool sst && st.SToolID == sst.SToolID) ||
            (e is Vehicle v && source is Vehicle svt && v.VehicleID == svt.VehicleID));

            if (exisiting != null)
            {
                // If the source exists at all update the common ITrackable attributes
                exisiting.Status = source.Status;
                exisiting.InDate = source.InDate;
                exisiting.OutDate = source.OutDate;
                exisiting.CheckoutRecords = source.CheckoutRecords;

                // If its a BasicTool update sub-class attributes
                if (exisiting is BasicTools b && source is BasicTools updatedB)
                {
                    b.Name = updatedB.Name;
                    b.Remarks = updatedB.Remarks;
                    b.Included = updatedB.Included;
                }

                // If it is a SpecialTool update sub-class attributes
                else if (exisiting is SpecialTool s && source is SpecialTool updatedS)
                {
                    s.Type = updatedS.Type;
                    s.CalDate = updatedS.CalDate;
                    s.CalDue = updatedS.CalDue;
                    s.Remarks = updatedS.Remarks;
                    s.Included = updatedS.Included;
                    s.CertRequired = updatedS.CertRequired;
                }

                // If it is a Vehilce update sub-class attributes
                else if (exisiting is Vehicle v && source is Vehicle updatedV)
                {
                    v.Make = updatedV.Make;
                    v.Model = updatedV.Model;
                    v.Year = updatedV.Year;
                    v.SerialNum = updatedV.SerialNum;
                    v.CertRequired = updatedV.CertRequired;
                    v.Remarks = updatedV.Remarks;
                }
            }
            // General for updating inventory
            // Could be checked out/in for Repairs/Serrvice
        }
    }
}
