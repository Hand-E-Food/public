using MarkRichardson.SingleProcessMonitor.Properties;
using Microsoft.WindowsAPICodePack.Taskbar;
using System;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Windows.Forms;

namespace MarkRichardson.SingleProcessMonitor
{

    /// <summary>
    /// The form that is used purely for the sake of the taskbar icon.
    /// </summary>
    public partial class MainForm : Form
    {

        #region Properties ...

        /// <summary>
        /// The CPU Usage percentage at which to display yellow.
        /// </summary>
        private int CpuUsageWarning;

        /// <summary>
        /// The CPU Usage percentage at which to display red.
        /// </summary>
        private int CpuUsageCritical;

        /// <summary>
        /// The name of the process to monitor.
        /// </summary>
        private string ImageName;

        /// <summary>
        /// The process being monitored.
        /// </summary>
        private Process Process;

        /// <summary>
        /// The name of the user owning the process to monitor.
        /// </summary>
        private string UserName;

        #endregion

        /// <summary>
        /// Initialises a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            CpuUsageCritical = Settings.Default.CpuUsageCritical;
            CpuUsageWarning = Settings.Default.CpuUsageWarning;
            ImageName = Settings.Default.ImageName;
            UserName = Settings.Default.UserName;
            if (string.IsNullOrWhiteSpace(UserName))
                UserName = Environment.UserName;

            FindProcess();
        }

        /// <summary>
        /// Finds the configured process and initialises the performance counter.
        /// </summary>
        private void FindProcess()
        {
            var processes = Process.GetProcessesByName(ImageName).Where(p => !p.HasExited);
            Process = processes.FirstOrDefault(p => GetProcessOwner(p) == UserName);
            if (Process != null)
            {
                CpuUsage.InstanceName = Process.ProcessName;
                CpuUsage.NextValue();
            }
        }

        /// <summary>
        /// Returns the owner of a process.
        /// </summary>
        /// <param name="process">The process for which to retrieve the owner.</param>
        /// <returns>The process's owner.</returns>
        private string GetProcessOwner(Process process)
        {
            ManagementObjectSearcher Processes = new ManagementObjectSearcher("SELECT * FROM Win32_Process WHERE ProcessId = " + process.Id);

            var processMO = Processes
                .Get()
                .Cast<ManagementObject>()
                .FirstOrDefault();
        
            if (processMO == null)
                throw new InvalidOperationException("Failed to read process information.");

            string[] owner = new string[2];
            processMO.InvokeMethod("GetOwner", (object[])owner);
            return owner[0];
        }

        /// <summary>
        /// Processes Windows messages.
        /// </summary>
        /// <param name="m">The Windows <see cref="System.Windows.Forms.Message"/> to process.</param>
        protected override void WndProc(ref Message m)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_RESTORE = 0xF120;
            const int SC_MAXIMIZE = 0xF030;

            // Check if a restore or maximize message is received.
            if ((m.Msg == WM_SYSCOMMAND) && ((int)m.WParam == SC_RESTORE || (int)m.WParam == SC_MAXIMIZE))
            {
                // If a restore or maximize message is received...
                // Ignore the message.
            }
            else
            {
                // If the message is something else...
                // Process the message as normal.
                base.WndProc(ref m);
            }
        }

        /// <summary>
        /// Monitors the process.
        /// </summary>
        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            TaskbarProgressBarState state;
            if (Process == null || Process.HasExited)
            {
                state = TaskbarProgressBarState.Indeterminate;
                if (Process != null)
                {
                    Process.Dispose();
                    Process = null;
                }
                FindProcess();
            }
            else
            {
                int value = (int)(CpuUsage.NextValue() / Environment.ProcessorCount);
                TaskbarManager.Instance.SetProgressValue(value, 100);

                if (value < CpuUsageWarning)
                    state = TaskbarProgressBarState.Normal;
                else if (value < CpuUsageCritical)
                    state = TaskbarProgressBarState.Paused;
                else
                    state = TaskbarProgressBarState.Error;
            }
            TaskbarManager.Instance.SetProgressState(state);
        }
    }
}
