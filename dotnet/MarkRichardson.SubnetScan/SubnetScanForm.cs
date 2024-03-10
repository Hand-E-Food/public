using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MarkRichardson.SubnetScan
{

    /// <summary>
    /// The main form that reports the subnet information.
    /// </summary>
    public partial class SubnetScanForm : Form
    {

        /// <summary>
        /// Initialises a new instance of the SubnetScanForm.
        /// </summary>
        public SubnetScanForm()
        {
            InitializeComponent();
            SubnetScanner_PingReplyChanged_Invoker = new EventHandler<IPv4AddressInformationChangedEventArgs>(SubnetScanner_PingReplyChanged);
            SubnetScanner_HostNameChanged_Invoker = new EventHandler<IPv4AddressInformationChangedEventArgs>(SubnetScanner_HostNameChanged);
        }

        /// <summary>
        /// The list of supported network interface types.
        /// </summary>
        #region private static readonly NetworkInterfaceType[] ValidNetworkInterfaceTypes = ...
        private static readonly NetworkInterfaceType[] ValidNetworkInterfaceTypes = 
        {
            NetworkInterfaceType.Ethernet,
            NetworkInterfaceType.Wireless80211,
        };
        #endregion

        /// <summary>
        /// Gets the selected network interface.
        /// </summary>
        /// <value>The selected network interface.</value>
        public NetworkInterface SelectedNetworkInterface { get; private set; }

        /// <summary>
        /// Gets or sets the active subnet scanner.
        /// </summary>
        /// <value>The active subnet scanner.</value>
        #region private SubnetScanner SubnetScanner ...
        private SubnetScanner SubnetScanner { get; set; }

        /// <summary>
        /// Updates the row's status.
        /// </summary>
        private void SubnetScanner_PingReplyChanged(object sender, IPv4AddressInformationChangedEventArgs e)
        {
            // Ensure this method runs on the correct thread.
            if (InvokeRequired)
            {
                BeginInvoke(SubnetScanner_PingReplyChanged_Invoker, sender, e);
                return;
            }
            // Update the row.
            DataGridViewRow row = (DataGridViewRow)e.Data.Tag;
            row.Cells[StatusColumn.Index].Value = e.Data.PingReply.Status.ToWords();
            row.DefaultCellStyle.BackColor =
                e.Data.PingReply.Status == IPStatus.Success
                ? Color.PaleGreen
                : Color.LightPink;
        }
        private EventHandler<IPv4AddressInformationChangedEventArgs> SubnetScanner_PingReplyChanged_Invoker;

        /// <summary>
        /// Updates the row's host name.
        /// </summary>
        private void SubnetScanner_HostNameChanged(object sender, IPv4AddressInformationChangedEventArgs e)
        {
            // Ensure this method runs on the correct thread.
            if (InvokeRequired)
            {
                BeginInvoke(SubnetScanner_HostNameChanged_Invoker, sender, e);
                return;
            }
            // Update the row.
            ((DataGridViewRow)e.Data.Tag).Cells[HostNameColumn.Index].Value = e.Data.HostName;
        }
        private EventHandler<IPv4AddressInformationChangedEventArgs> SubnetScanner_HostNameChanged_Invoker;
        #endregion

        /// <summary>
        /// Gets or sets a value indicating whether the AdapterComboBox is being updated.
        /// </summary>
        /// <value>True if the AdapterComboBox is being updated, otherwise false.</value>
        private bool UpdatingNetworkInterfaceComboBox
        {
            get { return updatingNetworkInterfaceComboBox; }
            set
            {
                if (value && !updatingNetworkInterfaceComboBox)
                    NetworkInterfaceComboBox.BeginUpdate();
                if (!value && updatingNetworkInterfaceComboBox)
                    NetworkInterfaceComboBox.EndUpdate();
                updatingNetworkInterfaceComboBox = value;
            }
        }
        private bool updatingNetworkInterfaceComboBox = false;

        /// <summary>
        /// Populates the form.
        /// </summary>
        private void SubnetScanForm_Load(object sender, EventArgs e)
        {
            NetworkInterfaceComboBox.DisplayMember = "Name";
            PopulateNetworkInterfaceComboBox();
        }

        /// <summary>
        /// Refreshes the NetworkInterfaceComboBox.
        /// </summary>
        private void RefreshNetworkInterfacesTimer_Tick(object sender, EventArgs e)
        {
            PopulateNetworkInterfaceComboBox();
        }

        /// <summary>
        /// Repopulates the network interface combo box.
        /// </summary>
        private void PopulateNetworkInterfaceComboBox()
        {
            // Get all ethernet interfaces.
            NetworkInterface[] interfaces = GetSupportedNetworkInterfaces();
            // Record the combo box's previous value.
            string previousValue = NetworkInterfaceComboBox.Text;
            // Repopulate the combo box.
            UpdatingNetworkInterfaceComboBox = true;
            NetworkInterfaceComboBox.Items.Clear();
            NetworkInterfaceComboBox.Items.AddRange(interfaces);
            // Find the previous value in the new ethernet interface list.
            int i;
            for (i = interfaces.Length - 1; i >= 0; i--)
            {
                if (interfaces[i].Name == previousValue)
                    break;
            }
            // Select the previous network interface.
            if (i >= 0)
                NetworkInterfaceComboBox.SelectedIndex = i;
            else
                NetworkInterfaceComboBox.Text = previousValue;
            // Finish.
            UpdatingNetworkInterfaceComboBox = false;
        }

        /// <summary>
        /// Gets all supported network interfaces.
        /// </summary>
        /// <returns>An array of all detected and supported network interfaces.</returns>
        private static NetworkInterface[] GetSupportedNetworkInterfaces()
        {
            return NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(networkInterface => Array.IndexOf(ValidNetworkInterfaceTypes, networkInterface.NetworkInterfaceType) >= 0)
                .ToArray();
        }

        /// <summary>
        /// Runs a scan on the selected network adapter's subnet.
        /// </summary>
        private void NetworkInterfaceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // If the network interface combo box is updating, don't do anything.
            if (UpdatingNetworkInterfaceComboBox) return;
            // Record the selected network interface.
            SelectedNetworkInterface = NetworkInterfaceComboBox.SelectedItem as NetworkInterface;
            // Cancel the previous scanner.
            if (SubnetScanner != null)
            {
                SubnetScanner.PingReplyChanged -= SubnetScanner_PingReplyChanged;
                SubnetScanner.HostNameChanged -= SubnetScanner_HostNameChanged;
                SubnetScanner.Dispose();
            }
            // Create a new scanner.
            if (SelectedNetworkInterface != null)
            {
                SubnetScanner = new SubnetScanner(SelectedNetworkInterface);
                PopulateIPAddresses();
                SubnetScanner.PingReplyChanged += SubnetScanner_PingReplyChanged;
                SubnetScanner.HostNameChanged += SubnetScanner_HostNameChanged;
                SubnetScanner.Start();
            }
            else
            {
                SubnetScanner = null;
            }
        }

        /// <summary>
        /// Populates the results grid with IP addresses.
        /// </summary>
        private void PopulateIPAddresses()
        {
            ResultsGrid.SuspendLayout();
            ResultsGrid.Rows.Clear();
            ResultsGrid.Rows.Add(SubnetScanner.Data.Count());
            int i = 0;
            foreach (var datum in SubnetScanner.Data)
            {
                DataGridViewRow row = ResultsGrid.Rows[i];
                datum.Tag = row;
                row.Cells[IpAddressColumn.Index].Value = datum.IPAddress;
                i++;
            }
            ResultsGrid.AutoResizeColumn(IpAddressColumn.Index, DataGridViewAutoSizeColumnMode.AllCells);
            ResultsGrid.ResumeLayout();
        }
    }
}
