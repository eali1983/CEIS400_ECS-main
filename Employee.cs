using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEIS400_ECS
{
    public class Employee
    {
        // Attributes
        protected string _empID;
        protected string _name;
        protected string _email;
        protected string _phone;
        protected EmpStatus _status;
        protected string _title;
        protected string _passwordHash;
        protected string _passwordSalt;
        protected Roles _role;
        protected Roles? _secondRole;

        // Constructors
        public Employee()
        {
            _empID = "";
            _name = "";
            _email = "";
            _phone = "";
            _status = EmpStatus.Active;
            _title = "";
            _passwordHash = Convert.ToBase64String(new byte[0]);
            _passwordSalt = "";
        }

        public Employee(string empID, string name, string email, string phone, EmpStatus status, string title, string passwordHash, string passwordSalt, Roles role)
        {
            EmpID = empID;
            Name = name;
            Email = email;
            Phone = phone;
            Status = status;
            Title = title;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            Role = role;
        }

        // Behaviors

        public bool Login()
        {
            UserSession.Instance.Login(this);
            return true;
        }

        public void LogOut()
        {
            UserSession.Instance.Logout();
        }


        // Properties
        public string EmpID { get { return _empID; } set { _empID = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public string Email { get { return _email; } set { _email = value; } }
        public string Phone { get { return _phone; } set { _phone = value; } }
        public EmpStatus Status { get { return _status; } set { _status = value; } }
        public string Title { get { return _title; } set { _title = value; } }
        public string PasswordHash { get { return _passwordHash; } set { _passwordHash = value; } }
        public string PasswordSalt { get { return _passwordSalt; } set { _passwordSalt = value; } }
        public Roles Role { get { return _role; } set { _role = value; } }
        public Roles? SecondRole { get { return _secondRole; } set { _secondRole = value; } }
    }

    // This is a nested class for managing user sessions
    // It ensures that only one instance of an account exists at a time
    // This is implemented using the Singleton Pattern
    public sealed class UserSession
    {
        private static UserSession _instance;
        private static readonly object _lock = new object();

        public Employee CurrentUser { get; private set; }
        private UserSession() { }

        public static UserSession Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ?? new UserSession();
                }
            }
        }

        public void Login(Employee employee)
        {
            CurrentUser = employee;
        }

        public void Logout()
        {
            CurrentUser = null;
        }
    }
}
