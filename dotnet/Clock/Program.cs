using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

[assembly: AssemblyKeyFile(@"keypair.snk")]

namespace Clock
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var settings = Properties.Settings.Default;
            if (args.Contains("--reset")) settings.Reset();

            var mainForm = new MainForm();
            Application.Run(mainForm);

            settings.Save();
        }
    }
}
