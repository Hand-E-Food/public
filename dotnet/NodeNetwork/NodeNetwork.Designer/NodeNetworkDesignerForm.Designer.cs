namespace NodeNetwork.Designer
{
    partial class NodeNetworkDesignerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NodeNetworkDesignerForm));
            this.openButton = new System.Windows.Forms.Button();
            this.saveButon = new System.Windows.Forms.Button();
            this.newButton = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.nodeNetworkDesignerPanel = new NodeNetwork.Designer.NodeNetworkDesignerPanel();
            this.SuspendLayout();
            // 
            // openButton
            // 
            this.openButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.openButton.Location = new System.Drawing.Point(186, 240);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(48, 48);
            this.openButton.TabIndex = 2;
            this.openButton.Text = "&Open";
            this.openButton.UseVisualStyleBackColor = true;
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // saveButon
            // 
            this.saveButon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButon.Location = new System.Drawing.Point(240, 240);
            this.saveButon.Name = "saveButon";
            this.saveButon.Size = new System.Drawing.Size(48, 48);
            this.saveButon.TabIndex = 3;
            this.saveButon.Text = "Save";
            this.saveButon.UseVisualStyleBackColor = true;
            this.saveButon.Click += new System.EventHandler(this.saveButon_Click);
            // 
            // newButton
            // 
            this.newButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.newButton.Location = new System.Drawing.Point(132, 240);
            this.newButton.Name = "newButton";
            this.newButton.Size = new System.Drawing.Size(48, 48);
            this.newButton.TabIndex = 1;
            this.newButton.Text = "&New";
            this.newButton.UseVisualStyleBackColor = true;
            this.newButton.Click += new System.EventHandler(this.newButton_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "json";
            this.openFileDialog.Filter = "JSON Files|*.json|All Files|*.*";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "json";
            this.saveFileDialog.Filter = "JSON Files|*.json|All Files|*.*";
            // 
            // nodeNetworkDesignerPanel
            // 
            this.nodeNetworkDesignerPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.nodeNetworkDesignerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nodeNetworkDesignerPanel.Location = new System.Drawing.Point(0, 0);
            this.nodeNetworkDesignerPanel.Name = "nodeNetworkDesignerPanel";
            this.nodeNetworkDesignerPanel.Origin = ((System.Drawing.PointF)(resources.GetObject("nodeNetworkDesignerPanel.Origin")));
            this.nodeNetworkDesignerPanel.Size = new System.Drawing.Size(300, 300);
            this.nodeNetworkDesignerPanel.TabIndex = 0;
            // 
            // NodeNetworkDesignerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 300);
            this.Controls.Add(this.saveButon);
            this.Controls.Add(this.newButton);
            this.Controls.Add(this.openButton);
            this.Controls.Add(this.nodeNetworkDesignerPanel);
            this.Name = "NodeNetworkDesignerForm";
            this.Text = "Node Network Designer";
            this.ResumeLayout(false);

        }

        #endregion

        private NodeNetworkDesignerPanel nodeNetworkDesignerPanel;
        private System.Windows.Forms.Button openButton;
        private System.Windows.Forms.Button saveButon;
        private System.Windows.Forms.Button newButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}

