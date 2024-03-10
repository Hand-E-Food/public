using Microsoft.WindowsAPICodePack.Taskbar;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarkRichardson.Pinger
{
    public partial class MainForm : Form
    {

        private bool _isPinging = false;

        private bool _isRunning = false;

        private readonly Ping _ping;

        private readonly Dictionary<IPStatus, Action<PingReply>> _statusHandlers;

        private readonly TaskbarManager _taskbarManager = TaskbarManager.Instance;

        private int _timeout = 1000; // ms

        public MainForm()
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);

            MainPanel_SizeChanged(mainPanel, EventArgs.Empty);

            _ping = new Ping();
            _ping.PingCompleted += _ping_PingCompleted;

            Application.Idle += Application_Idle;

            _statusHandlers = new Dictionary<IPStatus, Action<PingReply>>
            {
                { IPStatus.BadDestination                , ReportWarning },
                { IPStatus.BadHeader                     , ReportWarning },
                { IPStatus.BadOption                     , ReportWarning },
                { IPStatus.BadRoute                      , ReportWarning },
                { IPStatus.DestinationHostUnreachable    , ReportError   },
                { IPStatus.DestinationNetworkUnreachable , ReportError   },
                { IPStatus.DestinationPortUnreachable    , ReportError   },
                { IPStatus.DestinationProtocolUnreachable, ReportError   },
                { IPStatus.DestinationScopeMismatch      , ReportError   },
                { IPStatus.DestinationUnreachable        , ReportError   },
                { IPStatus.HardwareError                 , ReportWarning },
                { IPStatus.IcmpError                     , ReportWarning },
                { IPStatus.NoResources                   , ReportWarning },
                { IPStatus.PacketTooBig                  , ReportWarning },
                { IPStatus.ParameterProblem              , ReportWarning },
                { IPStatus.SourceQuench                  , ReportError   },
                { IPStatus.Success                       , ReportSuccess },
                { IPStatus.TimedOut                      , ReportError   },
                { IPStatus.TimeExceeded                  , ReportError   },
                { IPStatus.TtlExpired                    , ReportError   },
                { IPStatus.TtlReassemblyTimeExceeded     , ReportError   },
                { IPStatus.Unknown                       , ReportError   },
                { IPStatus.UnrecognizedNextHeader        , ReportWarning },
            };
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            Application.Idle -= Application_Idle;
            _isRunning = true;
            PingAndReport();
        }

        private void PingAndReport()
        {
            try
            {
                if (_isRunning)
                {
                    _isPinging = true;
                    _ping.SendAsync(addressTextBox.Text, _timeout, DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                Action<Exception> ReportWarning = this.ReportWarning;
                Invoke(ReportWarning, ex);
                ScheduleNextPing(DateTime.Now);
            }
        }

        private void _ping_PingCompleted(object sender, PingCompletedEventArgs e)
        {
            _isPinging = false;
            Action<string> ReportIdle = this.ReportIdle;
            if (e.Cancelled)
            {
                Invoke(ReportIdle, "Idle");
            }
            else if (e.Error != null)
                Invoke(ReportIdle, e.Error.Message);
            else
            {
                Action<PingReply> statusHandler;
                if (!_statusHandlers.TryGetValue(e.Reply.Status, out statusHandler))
                    statusHandler = ReportError;
                Invoke(statusHandler, e.Reply);
            }
            ScheduleNextPing((DateTime)e.UserState);
        }

        private void ScheduleNextPing(DateTime lastPingTime)
        {
            if (_isRunning)
            {
                var delay = lastPingTime.AddMilliseconds(_timeout) - DateTime.Now;
                if (delay < TimeSpan.Zero)
                    delay = TimeSpan.Zero;
                Task.Delay(delay)
                    .ContinueWith(_ => PingAndReport());
            }
        }

        private void ReportIdle(string description)
        {
            _taskbarManager.SetProgressValue(0, 100);
            _taskbarManager.SetProgressState(TaskbarProgressBarState.NoProgress);
            statusLabel.Text = description;
            statusLabel.BackColor = Color.FromKnownColor(KnownColor.Control);
        }

        private void ReportSuccess(PingReply reply)
        {
            _taskbarManager.SetProgressValue(100 * (int)(_timeout - reply.RoundtripTime) / _timeout, 100);
            _taskbarManager.SetProgressState(TaskbarProgressBarState.Normal);
            statusLabel.Text = string.Format("{0} ms", reply.RoundtripTime);
            statusLabel.BackColor = Color.LightGreen;
        }

        private void ReportWarning(PingReply reply)
        {
            _taskbarManager.SetProgressValue(100, 100);
            _taskbarManager.SetProgressState(TaskbarProgressBarState.Paused);
            statusLabel.Text = reply.Status.ToString();
            statusLabel.BackColor = Color.LightYellow;
        }

        private void ReportWarning(Exception ex)
        {
            _taskbarManager.SetProgressValue(100, 100);
            _taskbarManager.SetProgressState(TaskbarProgressBarState.Paused);
            statusLabel.Text = ex.Message;
            statusLabel.BackColor = Color.LightYellow;
        }

        private void ReportError(PingReply reply)
        {
            _taskbarManager.SetProgressValue(100, 100);
            _taskbarManager.SetProgressState(TaskbarProgressBarState.Error);
            statusLabel.Text = reply.Status.ToString();
            statusLabel.BackColor = Color.Pink;
        }

        private void MainPanel_SizeChanged(object sender, EventArgs e)
        {
            ClientSize = new Size(mainPanel.Width + mainPanel.Left * 2, mainPanel.Height + mainPanel.Top * 2);
            MinimumSize = new Size(0, Height);
            MaximumSize = new Size(Screen.AllScreens.Max(x => x.WorkingArea.Width), Height);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _isRunning = false;
            _ping.SendAsyncCancel();
            DateTime timeout = DateTime.Now.AddMilliseconds(_timeout);
            while (_isPinging && DateTime.Now < timeout)
                Thread.Sleep(0);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _ping.Dispose();
        }
    }
}
