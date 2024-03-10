using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MarkRichardson.SubnetScan
{

    /// <summary>
    /// Scans an IP address for information and records the results.
    /// </summary>
    public class IPv4AddressScanner
    {

        /// <summary>
        /// Initialises a new instance of the IPv4AddressScanner class.
        /// </summary>
        /// <param name="data">The data object that contains the IP address and will receive the scan results.</param>
        public IPv4AddressScanner(IPv4AddressInformation data)
        {
            this.Data = data;
        }

        /// <summary>
        /// Gets the data for this scan.
        /// </summary>
        /// <value>The data for this scan.</value>
        public IPv4AddressInformation Data { get; private set; }

        /// <summary>
        /// Performs scans the IP address and records the results.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Data.IPAddress is null.</exception>
        public void Start()
        {
            if (Data.IPAddress == null)
                throw new ArgumentNullException("Data.IPAddress");

            Ping();
            
            if (Data.PingReply.Status == IPStatus.Success)
                ReadHostName();
        }

        /// <summary>
        /// Pings the IP address and records the result.
        /// </summary>
        public void Ping()
        {
            using (Ping ping = new Ping())
            {
                Data.PingReply = ping.Send(Data.IPAddress);
            }
        }

        /// <summary>
        /// Reads the IP address's host name from the DNS.
        /// </summary>
        public void ReadHostName()
        {
            try
            {
                Data.HostName = Dns.GetHostEntry(Data.IPAddress).HostName;
            }
            catch (SocketException)
            {
                Data.HostName = null;
            }
        }
    }
}
