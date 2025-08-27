using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace CEIS400_ECS
{
    internal static class SystemTest
    {
        public static int Run()
        {
            var log = new List<string>();
            void Write(string s) { Console.WriteLine(s); log.Add(s); }

            Write($"=== System Test @ {DateTime.Now} ===");

            // sample checks – safe for any project
            Write(File.Exists("App.config")
                ? "PASS - App.config exists"
                : "FAIL - App.config missing");

            Write(File.Exists("CEIS400_ECS.csproj")
                ? "PASS - Project file exists"
                : "FAIL - Project file missing");

            Directory.CreateDirectory("test-output");
            var path = Path.Combine("test-output", $"test-results-{DateTime.Now:yyyyMMdd-HHmmss}.txt");
            File.WriteAllLines(path, log);

            // show where the file is so you can grab it for submission
            MessageBox.Show($"System test complete.\n\nSaved: {Path.GetFullPath(path)}",
                            "System Test", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // return 0 if all lines start with PASS (simple rule)
            var failed = log.Exists(l => l.StartsWith("FAIL", StringComparison.OrdinalIgnoreCase));
            return failed ? 1 : 0;
        }
    }
}
