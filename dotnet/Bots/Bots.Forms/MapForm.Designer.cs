namespace Bots.Forms;
partial class MapForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        mapTextBox = new RichTextBox();
        SuspendLayout();
        // 
        // mapTextBox
        // 
        mapTextBox.BorderStyle = BorderStyle.None;
        mapTextBox.Dock = DockStyle.Fill;
        mapTextBox.Enabled = false;
        mapTextBox.Font = new Font("Consolas", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        mapTextBox.Location = new Point(0, 0);
        mapTextBox.Name = "mapTextBox";
        mapTextBox.ReadOnly = true;
        mapTextBox.Size = new Size(800, 450);
        mapTextBox.TabIndex = 0;
        mapTextBox.Text = "";
        mapTextBox.Click += mapTextBox_Click;
        // 
        // MapForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(mapTextBox);
        DoubleBuffered = true;
        Name = "MapForm";
        Text = "Bots";
        ResumeLayout(false);
    }

    #endregion

    private RichTextBox mapTextBox;
}
