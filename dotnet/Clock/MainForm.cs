using Microsoft.Win32;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clock
{
    public partial class MainForm : Form
    {
        // https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings
        private const string DateTimeFormat = "dddd";

        private static readonly Properties.Settings Settings = Properties.Settings.Default;

        private static TimeSpan UntilNextUpdate => DateTime.Today.AddDays(1) - DateTime.Now;

        private CancellationTokenSource cancellation = new();

        private bool isRunning = true;

        public MainForm()
        {
            InitializeComponent();
            SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;
            SystemEvents.TimeChanged += SystemEvents_TimeChanged;
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            Icon = Properties.Resources.ClockIcon;
            TopMost = Settings.TopMost;
            alwaysOnTopToolStripMenuItem.Checked = TopMost;
            if (Settings.Size != Size.Empty)
            {
                Location = Settings.Location;
                Size = Settings.Size;
            }
            ResizeFont();
            await RunLoop();
        }

        private async Task RunLoop()
        {
            while (isRunning)
            {
                timeLabel.Text = DateTime.Now.ToString(DateTimeFormat);
                try
                {
                    await Task.Delay(UntilNextUpdate, cancellation.Token);
                }
                catch (OperationCanceledException)
                { }
            }
        }

        private void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            if (e.Mode == PowerModes.Resume) UpdateClock();
        }

        private void SystemEvents_TimeChanged(object sender, EventArgs e) => UpdateClock();

        private void UpdateClock()
        {
            var oldCancellation = cancellation;
            cancellation = new CancellationTokenSource();
            oldCancellation.Cancel();
        }

        private void timeLabel_SizeChanged(object sender, EventArgs e) => ResizeFont();

        private void ResizeFont()
        {
            using var graphics = CreateGraphics();

            var biggestSize = SizeF.Empty;
            for (var offset = 0; offset < 7; offset++)
            {
                var weekday = DateTime.Today.AddDays(offset).ToString(DateTimeFormat);
                var weekdaySize = graphics.MeasureString(weekday, Font);

                if (biggestSize.Width < weekdaySize.Width)
                    biggestSize.Width = weekdaySize.Width;
                if (biggestSize.Height < weekdaySize.Height)
                    biggestSize.Height = weekdaySize.Height;
            }

            var ratio = Math.Min(
                timeLabel.Width / biggestSize.Width,
                timeLabel.Height / biggestSize.Height
            );

            if (ratio > 0)
            {
                if (timeLabel.Font != Font) timeLabel.Font.Dispose();
                timeLabel.Font = new Font(Font.Name, Font.Size * ratio * 0.9f, Font.Style);
            }
        }

        private void timeLabel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                DragWindow();
        }

        private void DragWindow()
        {
            User32.ReleaseCapture();
            User32.SendMessage(Handle, User32.WM_NCLBUTTONDOWN, User32.HTCAPTION, 0);
        }

        private void alwaysOnTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TopMost = alwaysOnTopToolStripMenuItem.Checked;
        }

        private void minimizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) => Close();

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            WindowState = FormWindowState.Normal;
            Settings.Location = Location;
            Settings.Size = Size;
            Settings.TopMost = TopMost;
            isRunning = false;
            cancellation.Cancel();
        }
    }
}
