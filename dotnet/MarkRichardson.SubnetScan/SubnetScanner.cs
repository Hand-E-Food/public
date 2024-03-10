using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;

namespace MarkRichardson.SubnetScan
{

    /// <summary>
    /// Peforms an asynchronous scan of the subnets of a network interface.
    /// </summary>
    public class SubnetScanner : IDisposable
    {

        /// <summary>
        /// Initialises a new instance of the SubnetScanner class.
        /// </summary>
        public SubnetScanner(NetworkInterface networkInterface)
        {
            this.NetworkInterface = networkInterface;
            Cancellation = new CancellationTokenSource();
            TaskFactory = new TaskFactory(Cancellation.Token);
            Data = GetIpAddressList().Select(ipAddress => new IPv4AddressInformation(ipAddress)).ToArray();

            foreach (var datum in Data)
            {
                datum.PingReplyChanged += Data_PingReplyChanged;
                datum.HostNameChanged += Data_HostNameChanged;
            }
        }

        /// <summary>
        /// Gets a list of all IP addresses on the network interface's unicast subnets.
        /// </summary>
        /// <returns>A list of all IP addresses on the network interface's unicast subnets.</returns>
        private IEnumerable<IPAddress> GetIpAddressList()
        {
            return NetworkInterface
                .GetIPProperties()
                .UnicastAddresses
                .Where(ipAddressInfo => ipAddressInfo.Address.AddressFamily == AddressFamily.InterNetwork)
                .SelectMany(ipAddressInfo => GetIpAddressList(ipAddressInfo));
        }

        /// <summary>
        /// Gets a list of all IP addresses specified by the unicast subnet.
        /// </summary>
        /// <param name="networkInterface">The unicast subnet.</param>
        /// <returns>A list of all IP addresses specified by the unicast subnet.</returns>
        private static IEnumerable<IPAddress> GetIpAddressList(UnicastIPAddressInformation ipAddressInfo)
        {
            // Calculate the subnet base address.
            byte[] ipAddress = ipAddressInfo.Address.GetAddressBytes();
            byte[] subnetMask = ipAddressInfo.IPv4Mask.GetAddressBytes();
            byte[] subnet = new byte[subnetMask.Length];
            int i;
            for (i = 0; i < subnet.Length; i++)
            {
                subnet[i] = (byte)(ipAddress[i] & subnetMask[i]);
                subnetMask[i] = (byte)(~subnetMask[i]);
            }
            // Determine how many IP addresses are in this subnet
            int count = 0;
            foreach (byte b in subnetMask)
                count = (count << 8) | b;
            // Generate each address by incrementing the least-significant byte and ovwerflowing to the next byte as necessary.
            List<IPAddress> result = new List<IPAddress>(count + 1);
            for (; count >= 0; count--)
            {
                result.Add(new IPAddress(subnet));
                for (i = subnetMask.Length - 1; subnet[i] == 0xFF; i--)
                {
                    subnet[i] = 0;
                }
                subnet[i]++;
            }
            // Return the results.
            return result;
        }

        /// <summary>
        /// Gets or sets the current session canceller.
        /// </summary>
        /// <value>The current session canceller.</value>
        /// <remarks>This is used to prevent asynchronous methods from accidentally updating a new session when
        /// the intended session was discarded.</remarks>
        private CancellationTokenSource Cancellation { get; set; }

        /// <summary>
        /// Gets or sets the local addresses from which scanning will start.
        /// </summary>
        public IEnumerable<IPv4AddressInformation> Data { get; private set; }

        /// <summary>
        /// Gets the network interface being scanned.
        /// </summary>
        /// <value>The network interface being scanned.</value>
        public NetworkInterface NetworkInterface { get; private set; }

        /// <summary>
        /// The task factory used to generate tasks.
        /// </summary>
        private TaskFactory TaskFactory { get; set; }

        /// <summary>
        /// Scans the subnet for all network devices and reports the results.
        /// </summary>
        public void Start()
        {
            foreach (var datum in Data)
            {
                IPv4AddressScanner task = new IPv4AddressScanner(datum);
                TaskFactory.StartNew(task.Start);
            }
        }

        /// <summary>
        /// Forwards the event to listeners.
        /// </summary>
        private void Data_PingReplyChanged(object sender, EventArgs e)
        {
            OnPingReplyChanged(new IPv4AddressInformationChangedEventArgs((IPv4AddressInformation)sender));
        }
        /// <summary>
        /// Raises the PingReplyChanged event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnPingReplyChanged(IPv4AddressInformationChangedEventArgs e)
        {
            if (PingReplyChanged != null)
                PingReplyChanged(this, e);
        }
        /// <summary>
        /// Raised when the PingReply property of one of the elements of Data is changed.
        /// </summary>
        public event EventHandler<IPv4AddressInformationChangedEventArgs> PingReplyChanged;

        /// <summary>
        /// Forwards the event to listeners.
        /// </summary>
        private void Data_HostNameChanged(object sender, EventArgs e)
        {
            OnHostNameChanged(new IPv4AddressInformationChangedEventArgs((IPv4AddressInformation)sender));
        }
        /// <summary>
        /// Raises the HostNameChanged event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnHostNameChanged(IPv4AddressInformationChangedEventArgs e)
        {
            if (HostNameChanged != null)
                HostNameChanged(this, e);
        }
        /// <summary>
        /// Raised when the HostName property of one of the elements of Data is changed.
        /// </summary>
        public event EventHandler<IPv4AddressInformationChangedEventArgs> HostNameChanged;

        /// <summary>
        /// Disposes of all resources used by this object.
        /// </summary>
        #region public void Dispose()
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Disposes of all resources used by this object.
        /// </summary>
        /// <param name="disposing">True to dispose of managed and unmanaged resources.  False to dispose of
        /// unmanaged resources only.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    Cancellation.Cancel();
                }
                IsDisposed = true;
            }
        }
        /// <summary>
        /// Gets a value indicating whether this object is disposed.
        /// </summary>
        /// <value>True if this object is disposed.  Otherwise, false.</value>
        protected bool IsDisposed { get; private set; }
        /// <summary>
        /// Disposes of unmanaged resources.
        /// </summary>
        ~SubnetScanner()
        {
            Dispose(false);
        }
        #endregion
    }
}
