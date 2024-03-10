using System;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;

namespace MarkRichardson.AdminServices
{

    /// <summary>
    /// Executes the main program.
    /// </summary>
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
                MainAsExecutable(args);
            else
                MainAsService();
        }

        /// <summary>
        /// The Main method if this is being run as an executable.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        private static void MainAsExecutable(string[] args)
        {
            // Determine whether this is being run from a console window.
            bool isConsole = (Console.CursorLeft != 0 || Console.CursorTop != 0);

            // Select the execution mode.
            if (args.Length > 0 && args[0].ToLower() == "-i")
                InstallService();
            else if (args.Length > 0 && args[0].ToLower() == "-u")
                UninstallService();
            else
                RunService(args);

            // Pause before exit.
            if (!isConsole)
            {
                Console.WriteLine("Press [Esc] to close.");
                while (Console.ReadKey(true).Key != ConsoleKey.Escape);
            }
        }

        /// <summary>
        /// The Main method if this is being run by the Service Control Manager.
        /// </summary>
        private static void MainAsService()
        {
            ServiceBase.Run(new LocalWebService());
        }

        /// <summary>
        /// Installs this executable.
        /// </summary>
        private static void InstallService()
        {
            ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });
        }

        /// <summary>
        /// Uninstalls this executable.
        /// </summary>
        private static void UninstallService()
        {
            ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
        }

        /// <summary>
        /// Runs the service.
        /// </summary>
        private static void RunService(string[] args)
        {
            var service = new LocalWebService();
            try
            {
                service.Start(args);
                Console.WriteLine("Service started.  Press [Esc] to stop.");
                while (Console.ReadKey(true).Key != ConsoleKey.Escape) ;
                service.Stop();
                Console.Write("Service stopped.  ");
            }
#if !DEBUG
            catch (Exception ex)
            {
                service.EventLog.WriteEntry(ex.ToString(), EventLogEntryType.Error);
                Console.WriteLine("Service failed!");
                Console.WriteLine(ex.ToString());
            }
#endif
            finally { } // Required if catch block omitted by configuration.
        }
    }
}
