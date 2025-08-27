using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEIS400_ECS
{
    public class Supervisor : Employee
    {
        public Supervisor(string empID, string name, string email, string phone, EmpStatus status, string title, string passwordHash, string passwordSalt)
            : base(empID, name, email, phone, status, title, passwordHash, passwordSalt, Roles.Supervisor)
        {

        }

        private void IsSupervisor()
        {
            if (Role != Roles.Supervisor)
            {
                throw new UnauthorizedAccessException("Only a supervisor can perform this action");
            }
        }

        public string GenerateReports(Reports reportType)
        {
            // For the various reports from the report class
            var type = reportType.ToString();
            try
            {
                switch (type)
                {
                    case "Missing":
                        foreach (var item in reportType.GetMissingItems())
                        {
                            string totalDays = Convert.ToString((DateTime.Now - item.OutDate.Value).TotalDays);
                            return $"{item.Source} | {item.OutDate} | {totalDays} | {item.CheckoutRecords.LastOrDefault().EmpID}";
                        }
                        break;

                    case "OutForService":
                        foreach (var item in reportType.GetOutForServiceItems())
                        {
                            string totalDays = Convert.ToString((DateTime.Now - item.OutDate.Value).TotalDays);
                            return $"{item.Source} | {item.OutDate} | {totalDays}";
                        }
                        break;

                    case "OverDue":
                        foreach (var item in reportType.GetOverDueItems())
                        {
                            string totalDays = Convert.ToString((DateTime.Now - item.OutDate.Value).TotalDays);
                            return $"{item.Source} | {item.OutDate} | {totalDays} | {item.CheckoutRecords.LastOrDefault().EmpID}";
                        }
                        break;

                    case "CalDue":
                        foreach (var item in reportType.GetCalDueItems())
                        {
                            return $"{item.Source} | {item.CalDate} | {item.CalDue}";
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to generate report", ex);
            }

            return string.Empty;
        }

        public void PlaceHold(Employee employee)
        {
            IsSupervisor();
            employee.Status = EmpStatus.Hold;
        }

        public void AssignCerts(Customer employee, ITrackable certToAdd)
        {
            IsSupervisor();
            // Assign Certs to customer accounts
            if (!employee.Certs.Contains(certToAdd))
            {
                employee.Certs.Add(certToAdd);
                MessageBox.Show("Success", $"{certToAdd} added to {employee.Name}");
            }
            else
            {
                MessageBox.Show("Attention", "Employee currently has certification", MessageBoxButtons.OK);
            }
        }

        public void UpdateCerts()
        {
            IsSupervisor();
            // Update certs to customer accounts. Cert could be suspended or pending.
            // For future use, May require Certification class to manage expiration dates, certified date, etc.
        }

        public void RevokeCerts(Customer employee, ITrackable certToRemove)
        {
            IsSupervisor();
            // Delete cert from customer account
            if (employee.Certs.Contains(certToRemove))
            {
                employee.Certs.Remove(certToRemove);
            }
            else
            {
                MessageBox.Show("Attention", "Employee currently does not have certification", MessageBoxButtons.OK);
            }
        }
    }
}
