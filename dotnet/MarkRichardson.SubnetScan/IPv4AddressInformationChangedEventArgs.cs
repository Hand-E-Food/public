using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkRichardson.SubnetScan
{

    /// <summary>
    /// Raised when a field is changed in a IPv4AddressInformation object.
    /// </summary>
    public class IPv4AddressInformationChangedEventArgs : EventArgs
    {

        /// <summary>
        /// Initialises a new instance of the IPv4AddressInformationChangedEventArgs class.
        /// </summary>
        /// <param name="data">Teh IPv4AddressInformation object that was changed.</param>
        public IPv4AddressInformationChangedEventArgs(IPv4AddressInformation data)
        {
            this.Data = data;
        }

        /// <summary>
        /// Gets the IPv4 address information that raised the event.
        /// </summary>
        /// <value>The IPv4 address information that raised the event.</value>
        public IPv4AddressInformation Data { get; private set; }
    }
}
