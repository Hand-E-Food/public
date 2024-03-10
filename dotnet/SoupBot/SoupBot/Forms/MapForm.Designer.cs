namespace SoupBot.Forms;

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
        mapControl = new MapControl();
        scheduler = new System.ComponentModel.BackgroundWorker();
        SuspendLayout();
        // 
        // mapControl
        // 
        mapControl.Dock = DockStyle.Fill;
        mapControl.Location = new Point(0, 0);
        mapControl.Map = null;
        mapControl.Name = "mapControl";
        mapControl.Size = new Size(800, 450);
        mapControl.TabIndex = 0;
        // 
        // scheduler
        // 
        scheduler.DoWork += scheduler_DoWork;
        // 
        // MapForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(mapControl);
        Name = "MapForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "soup.bot";
        FormClosing += MapForm_FormClosing;
        Shown += MapForm_Shown;
        KeyDown += MapForm_KeyDown;
        KeyUp += MapForm_KeyUp;
        ResumeLayout(false);
    }

    #endregion

    private MapControl mapControl;
    private System.ComponentModel.BackgroundWorker scheduler;
}