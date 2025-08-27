using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CEIS400_ECS
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        public static DatabaseAccess db;
        public static BindingList<ITrackable> trackables;
        public static BindingList<Employee> employees;

        [STAThread]
        static void Main()
        {
            // Initialize DB connection
            DatabaseAccess db = new DatabaseAccess();

            // Load DB Data
            BindingList<ITrackable> trackables = db.LoadTrackables();
            BindingList<Employee> employees = db.LoadEmployees(trackables);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // This is for when the user closes, the program will save to the DB
            Form1 mainForm = new Form1();
            mainForm.FormClosing += MainForm_FormClosing;

            Application.Run(new Form1());
        }

        private static void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Save to DB on Close
            db.SaveEmployees(employees);
            db.SaveTrackables(trackables);
        }
    }
}
