using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CEIS400_ECS
{
    public class Admin : Employee
    {
        public Admin(string empID, string name, string email, string phone, EmpStatus status, string title, string passwordHash, string passwordSalt)
            : base(empID, name, email, phone, status, title, passwordHash, passwordSalt, Roles.Admin)
        {
            
        }

        private void isAdmin()
        {
            if (Role != Roles.Admin)
            {
                throw new UnauthorizedAccessException("Only program administrators can perform this function");
            }
        }

        public Employee CreateUserAccount(string empID, string name, string email, string phone, EmpStatus status, string title, Roles role)
        {
            isAdmin();

            string defaultPassword = "Change123";
            string salt;
            string hashedPassword = PasswordControl.HashPassword(defaultPassword, out salt);

            Employee newEmp = new Employee(empID, name, email, phone, status, title, hashedPassword, salt, role);

            return newEmp;
        }

        public void DeactivateUserAccount(Employee employee)
        {
            isAdmin();

            employee.Status = EmpStatus.Terminated;
        }

        public void UpdateUserAccount(Employee employee, string newName, string newEmail, string newPhone, Roles newRole)
        {
            isAdmin();

            employee.Name = newName;
            employee.Email = newEmail;
            employee.Phone = newPhone;
            employee.Role = newRole;

        }
    }
}
