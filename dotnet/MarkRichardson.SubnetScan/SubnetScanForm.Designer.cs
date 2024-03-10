namespace MarkRichardson.SubnetScan
{
    partial class SubnetScanForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.NetworkInterfaceLabel = new System.Windows.Forms.Label();
            this.NetworkInterfaceComboBox = new System.Windows.Forms.ComboBox();
            this.ResultsGrid = new System.Windows.Forms.DataGridView();
            this.IpAddressColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HostNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StatusColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RefreshNetworkInterfacesTimer = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ResultsGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.NetworkInterfaceLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.NetworkInterfaceComboBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.ResultsGrid, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(284, 262);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // NetworkInterfaceLabel
            // 
            this.NetworkInterfaceLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.NetworkInterfaceLabel.AutoSize = true;
            this.NetworkInterfaceLabel.Location = new System.Drawing.Point(12, 16);
            this.NetworkInterfaceLabel.Margin = new System.Windows.Forms.Padding(12, 9, 3, 0);
            this.NetworkInterfaceLabel.Name = "NetworkInterfaceLabel";
            this.NetworkInterfaceLabel.Size = new System.Drawing.Size(95, 13);
            this.NetworkInterfaceLabel.TabIndex = 0;
            this.NetworkInterfaceLabel.Text = "Network Interface:";
            // 
            // NetworkInterfaceComboBox
            // 
            this.NetworkInterfaceComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NetworkInterfaceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.NetworkInterfaceComboBox.FormattingEnabled = true;
            this.NetworkInterfaceComboBox.Location = new System.Drawing.Point(113, 12);
            this.NetworkInterfaceComboBox.Margin = new System.Windows.Forms.Padding(3, 12, 12, 3);
            this.NetworkInterfaceComboBox.Name = "NetworkInterfaceComboBox";
            this.NetworkInterfaceComboBox.Size = new System.Drawing.Size(159, 21);
            this.NetworkInterfaceComboBox.TabIndex = 1;
            this.NetworkInterfaceComboBox.SelectedIndexChanged += new System.EventHandler(this.NetworkInterfaceComboBox_SelectedIndexChanged);
            // 
            // ResultsGrid
            // 
            this.ResultsGrid.AllowUserToAddRows = false;
            this.ResultsGrid.AllowUserToDeleteRows = false;
            this.ResultsGrid.AllowUserToOrderColumns = true;
            this.ResultsGrid.AllowUserToResizeRows = false;
            this.ResultsGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.ResultsGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ResultsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ResultsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IpAddressColumn,
            this.HostNameColumn,
            this.StatusColumn});
            this.tableLayoutPanel1.SetColumnSpan(this.ResultsGrid, 2);
            this.ResultsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResultsGrid.Location = new System.Drawing.Point(0, 36);
            this.ResultsGrid.Margin = new System.Windows.Forms.Padding(0);
            this.ResultsGrid.Name = "ResultsGrid";
            this.ResultsGrid.RowHeadersVisible = false;
            this.ResultsGrid.Size = new System.Drawing.Size(284, 226);
            this.ResultsGrid.TabIndex = 2;
            // 
            // IpAddressColumn
            // 
            this.IpAddressColumn.Frozen = true;
            this.IpAddressColumn.HeaderText = "IP Address";
            this.IpAddressColumn.Name = "IpAddressColumn";
            this.IpAddressColumn.ReadOnly = true;
            this.IpAddressColumn.Width = 83;
            // 
            // HostNameColumn
            // 
            this.HostNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.HostNameColumn.HeaderText = "Host Name";
            this.HostNameColumn.Name = "HostNameColumn";
            this.HostNameColumn.ReadOnly = true;
            this.HostNameColumn.Width = 85;
            // 
            // StatusColumn
            // 
            this.StatusColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.StatusColumn.HeaderText = "Status";
            this.StatusColumn.Name = "StatusColumn";
            this.StatusColumn.ReadOnly = true;
            this.StatusColumn.Width = 62;
            // 
            // RefreshNetworkInterfacesTimer
            // 
            this.RefreshNetworkInterfacesTimer.Enabled = true;
            this.RefreshNetworkInterfacesTimer.Interval = 5000;
            this.RefreshNetworkInterfacesTimer.Tick += new System.EventHandler(this.RefreshNetworkInterfacesTimer_Tick);
            // 
            // SubnetScanForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SubnetScanForm";
            this.Text = "Subnet Scan";
            this.Load += new System.EventHandler(this.SubnetScanForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ResultsGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label NetworkInterfaceLabel;
        private System.Windows.Forms.ComboBox NetworkInterfaceComboBox;
        private System.Windows.Forms.DataGridView ResultsGrid;
        private System.Windows.Forms.Timer RefreshNetworkInterfacesTimer;
        private System.Windows.Forms.DataGridViewTextBoxColumn IpAddressColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn HostNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn StatusColumn;

    }
}

