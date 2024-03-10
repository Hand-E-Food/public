using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace MarkRichardson.SubnetScan
{

    /// <summary>
    /// Contians information about an IP address.
    /// </summary>
    public class IPv4AddressInformation
    {

        /// <summary>
        /// Initialises a new instance of the IPv4AddressInformation class.
        /// </summary>
        /// <param name="ipAddress">The IP address from which to gather information.</param>
        public IPv4AddressInformation(IPAddress ipAddress)
        {
            this.IPAddress = ipAddress;
        }

        /// <summary>
        /// Gets or sets the host name.
        /// </summary>
        /// <value>The host name.</value>
        public string HostName
        {
            get { return hostName; }
            set
            {
                if (hostName == value) return;
                hostName = value;
                OnHostNameChanged(EventArgs.Empty);
            }
        }
        private string hostName;
        /// <summary>
        /// Raises the HostNameChanged event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected void OnHostNameChanged(EventArgs e)
        {
            if (HostNameChanged != null)
                HostNameChanged(this, e);
        }
        /// <summary>
        /// Raised when the HostName property is changed.
        /// </summary>
        public event EventHandler HostNameChanged;

        /// <summary>
        /// Gets or sets the IP address.
        /// </summary>
        /// <value>The IP address.</value>
        /// <exception cref="System.ArgumentNullException">The value was set to null.</exception>
        /// <exception cref="System.ArgumentException">The value was set to a non-IPv4 address.</exception>
        public IPAddress IPAddress
        {
            get { return ipAddress; }
            private set
            {
                if (value == null)
                    throw new ArgumentNullException("IPAddress");
                if (value.AddressFamily != AddressFamily.InterNetwork)
                    throw new ArgumentException("The IP address must be an IPv4 address.");

                ipAddress = value;
            }
        }
        private IPAddress ipAddress;

        /// <summary>
        /// Gets or sets the reply from the ping attempt.
        /// </summary>
        /// <value>The reply from the ping attempt.</value>
        public PingReply PingReply
        {
            get { return pingReply; }
            set
            {
                if (pingReply == value) return;
                pingReply = value;
                OnPingReplyChanged(EventArgs.Empty);
            }
        }
        private PingReply pingReply;
        /// <summary>
        /// Raises the PingReplyChanged event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected void OnPingReplyChanged(EventArgs e)
        {
            if (PingReplyChanged != null)
                PingReplyChanged(this, e);
        }
        /// <summary>
        /// Raised when the PingReply property is changed.
        /// </summary>
        /// 
        public event EventHandler PingReplyChanged;

        /// <summary>
        /// Gets or sets the object's tag.
        /// </summary>
        /// <value>The object's tag.</value>
        public object Tag
        {
            get { return tag; }
            set
            {
                if (tag == value) return;
                tag = value;
                OnTagChanged(EventArgs.Empty);
            }
        }
        private object tag;
        /// <summary>
        /// Raises the TagChanged event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected void OnTagChanged(EventArgs e)
        {
            if (TagChanged != null)
                TagChanged(this, e);
        }
        /// <summary>
        /// Raised when the Tag property is changed.
        /// </summary>
        public event EventHandler TagChanged;
    }
}
