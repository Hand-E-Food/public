namespace NodeNetwork.Designer
{
    partial class NodeNetworkDesignerPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // NodeNetworkDesignerPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(255)))));
            this.DoubleBuffered = true;
            this.Name = "NodeNetworkDesignerPanel";
            this.Size = new System.Drawing.Size(300, 300);
            this.SizeChanged += new System.EventHandler(this.NodeNetworkDesignerPanel_SizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.NodeNetworkDesignerPanel_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.NodeNetworkDesignerPanel_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.NodeNetworkDesignerPanel_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.NodeNetworkDesignerPanel_MouseUp);
            this.Disposed += new System.EventHandler(this.NodeNetworkDesignerPanel_Disposed);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
