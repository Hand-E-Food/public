namespace RandomVectorMap.Forms;

partial class MainForm
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
        if (disposing && (components is not null))
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
        this.MainPanel = new System.Windows.Forms.TableLayoutPanel();
        this.ResetButon = new System.Windows.Forms.Button();
        this.ReseedTextBox = new System.Windows.Forms.TextBox();
        this.RandomButton = new System.Windows.Forms.Button();
        this.FinishButton = new System.Windows.Forms.Button();
        this.ShowDebugCheckBox = new System.Windows.Forms.CheckBox();
        this.TaskButton = new System.Windows.Forms.Button();
        this.StepButton = new System.Windows.Forms.Button();
        this.SizeLabel = new System.Windows.Forms.Label();
        this.SizeTextBox = new System.Windows.Forms.TextBox();
        this.StatusLabel = new System.Windows.Forms.Label();
        this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
        this.MoveUpButton = new System.Windows.Forms.Button();
        this.MoveDownButton = new System.Windows.Forms.Button();
        this.MoveLeftButton = new System.Windows.Forms.Button();
        this.MoveRightButton = new System.Windows.Forms.Button();
        this.ZoomInButton = new System.Windows.Forms.Button();
        this.ZoomOutButton = new System.Windows.Forms.Button();
        this.ZoomFitButton = new System.Windows.Forms.Button();
        this.ZoomActualButton = new System.Windows.Forms.Button();
        this.MapView = new RandomVectorMap.Forms.MapViewControl();
        this.MainPanel.SuspendLayout();
        this.tableLayoutPanel1.SuspendLayout();
        this.SuspendLayout();
        // 
        // MainPanel
        // 
        this.MainPanel.ColumnCount = 9;
        this.MainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.MainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
        this.MainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.MainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
        this.MainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.MainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.MainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.MainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.MainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.MainPanel.Controls.Add(this.ResetButon, 4, 0);
        this.MainPanel.Controls.Add(this.ReseedTextBox, 3, 0);
        this.MainPanel.Controls.Add(this.RandomButton, 2, 0);
        this.MainPanel.Controls.Add(this.FinishButton, 7, 0);
        this.MainPanel.Controls.Add(this.ShowDebugCheckBox, 8, 0);
        this.MainPanel.Controls.Add(this.TaskButton, 6, 0);
        this.MainPanel.Controls.Add(this.StepButton, 5, 0);
        this.MainPanel.Controls.Add(this.SizeLabel, 0, 0);
        this.MainPanel.Controls.Add(this.SizeTextBox, 1, 0);
        this.MainPanel.Controls.Add(this.StatusLabel, 0, 1);
        this.MainPanel.Controls.Add(this.tableLayoutPanel1, 0, 2);
        this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
        this.MainPanel.Location = new System.Drawing.Point(0, 0);
        this.MainPanel.Name = "MainPanel";
        this.MainPanel.RowCount = 3;
        this.MainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.MainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.MainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.MainPanel.Size = new System.Drawing.Size(684, 562);
        this.MainPanel.TabIndex = 0;
        // 
        // ResetButon
        // 
        this.ResetButon.Anchor = System.Windows.Forms.AnchorStyles.Left;
        this.ResetButon.AutoSize = true;
        this.ResetButon.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.ResetButon.Location = new System.Drawing.Point(277, 3);
        this.ResetButon.Name = "ResetButon";
        this.ResetButon.Size = new System.Drawing.Size(75, 23);
        this.ResetButon.TabIndex = 4;
        this.ResetButon.Text = "&Reset";
        this.ResetButon.UseVisualStyleBackColor = true;
        this.ResetButon.Click += new System.EventHandler(this.ResetButton_Click);
        // 
        // ReseedTextBox
        // 
        this.ReseedTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
        this.ReseedTextBox.Location = new System.Drawing.Point(205, 4);
        this.ReseedTextBox.Name = "ReseedTextBox";
        this.ReseedTextBox.Size = new System.Drawing.Size(66, 20);
        this.ReseedTextBox.TabIndex = 3;
        // 
        // RandomButton
        // 
        this.RandomButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
        this.RandomButton.AutoSize = true;
        this.RandomButton.Location = new System.Drawing.Point(111, 3);
        this.RandomButton.Name = "RandomButton";
        this.RandomButton.Size = new System.Drawing.Size(88, 23);
        this.RandomButton.TabIndex = 2;
        this.RandomButton.Text = "Ra&ndom Seed:";
        this.RandomButton.UseVisualStyleBackColor = true;
        this.RandomButton.Click += new System.EventHandler(this.RandomButton_Click);
        // 
        // FinishButton
        // 
        this.FinishButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
        this.FinishButton.AutoSize = true;
        this.FinishButton.Location = new System.Drawing.Point(520, 3);
        this.FinishButton.Name = "FinishButton";
        this.FinishButton.Size = new System.Drawing.Size(75, 23);
        this.FinishButton.TabIndex = 7;
        this.FinishButton.Text = "&Finish";
        this.FinishButton.UseVisualStyleBackColor = true;
        this.FinishButton.Click += new System.EventHandler(this.FinishButton_Click);
        // 
        // ShowDebugCheckBox
        // 
        this.ShowDebugCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
        this.ShowDebugCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
        this.ShowDebugCheckBox.AutoSize = true;
        this.ShowDebugCheckBox.Location = new System.Drawing.Point(601, 3);
        this.ShowDebugCheckBox.Name = "ShowDebugCheckBox";
        this.ShowDebugCheckBox.Size = new System.Drawing.Size(79, 23);
        this.ShowDebugCheckBox.TabIndex = 8;
        this.ShowDebugCheckBox.Text = "Show &Debug";
        this.ShowDebugCheckBox.UseVisualStyleBackColor = true;
        this.ShowDebugCheckBox.CheckedChanged += new System.EventHandler(this.ShowDebugCheckBox_CheckedChanged);
        // 
        // TaskButton
        // 
        this.TaskButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
        this.TaskButton.AutoSize = true;
        this.TaskButton.Location = new System.Drawing.Point(439, 3);
        this.TaskButton.Name = "TaskButton";
        this.TaskButton.Size = new System.Drawing.Size(75, 23);
        this.TaskButton.TabIndex = 6;
        this.TaskButton.Text = "&Task";
        this.TaskButton.UseVisualStyleBackColor = true;
        this.TaskButton.Click += new System.EventHandler(this.TaskButton_Click);
        // 
        // StepButton
        // 
        this.StepButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
        this.StepButton.AutoSize = true;
        this.StepButton.Location = new System.Drawing.Point(358, 3);
        this.StepButton.Name = "StepButton";
        this.StepButton.Size = new System.Drawing.Size(75, 23);
        this.StepButton.TabIndex = 5;
        this.StepButton.Text = "&Step";
        this.StepButton.UseVisualStyleBackColor = true;
        this.StepButton.Click += new System.EventHandler(this.StepButton_Click);
        // 
        // SizeLabel
        // 
        this.SizeLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
        this.SizeLabel.AutoSize = true;
        this.SizeLabel.Location = new System.Drawing.Point(3, 8);
        this.SizeLabel.Name = "SizeLabel";
        this.SizeLabel.Size = new System.Drawing.Size(30, 13);
        this.SizeLabel.TabIndex = 0;
        this.SizeLabel.Text = "Si&ze:";
        // 
        // SizeTextBox
        // 
        this.SizeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
        this.SizeTextBox.Location = new System.Drawing.Point(39, 4);
        this.SizeTextBox.Name = "SizeTextBox";
        this.SizeTextBox.Size = new System.Drawing.Size(66, 20);
        this.SizeTextBox.TabIndex = 1;
        this.SizeTextBox.Text = "100";
        // 
        // StatusLabel
        // 
        this.StatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
        this.StatusLabel.AutoSize = true;
        this.MainPanel.SetColumnSpan(this.StatusLabel, 9);
        this.StatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.StatusLabel.Location = new System.Drawing.Point(3, 29);
        this.StatusLabel.Name = "StatusLabel";
        this.StatusLabel.Size = new System.Drawing.Size(678, 17);
        this.StatusLabel.TabIndex = 98;
        this.StatusLabel.Text = "Not generating a map at the moment.";
        this.StatusLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
        // 
        // tableLayoutPanel1
        // 
        this.tableLayoutPanel1.ColumnCount = 3;
        this.MainPanel.SetColumnSpan(this.tableLayoutPanel1, 9);
        this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
        this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
        this.tableLayoutPanel1.Controls.Add(this.MapView, 1, 1);
        this.tableLayoutPanel1.Controls.Add(this.MoveUpButton, 1, 0);
        this.tableLayoutPanel1.Controls.Add(this.MoveDownButton, 1, 2);
        this.tableLayoutPanel1.Controls.Add(this.MoveLeftButton, 0, 1);
        this.tableLayoutPanel1.Controls.Add(this.MoveRightButton, 2, 1);
        this.tableLayoutPanel1.Controls.Add(this.ZoomInButton, 2, 0);
        this.tableLayoutPanel1.Controls.Add(this.ZoomOutButton, 2, 2);
        this.tableLayoutPanel1.Controls.Add(this.ZoomFitButton, 0, 2);
        this.tableLayoutPanel1.Controls.Add(this.ZoomActualButton, 0, 0);
        this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 46);
        this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
        this.tableLayoutPanel1.Name = "tableLayoutPanel1";
        this.tableLayoutPanel1.RowCount = 3;
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
        this.tableLayoutPanel1.Size = new System.Drawing.Size(684, 516);
        this.tableLayoutPanel1.TabIndex = 99;
        // 
        // MoveUpButton
        // 
        this.MoveUpButton.Dock = System.Windows.Forms.DockStyle.Fill;
        this.MoveUpButton.Location = new System.Drawing.Point(25, 0);
        this.MoveUpButton.Margin = new System.Windows.Forms.Padding(0);
        this.MoveUpButton.Name = "MoveUpButton";
        this.MoveUpButton.Size = new System.Drawing.Size(634, 25);
        this.MoveUpButton.TabIndex = 0;
        this.MoveUpButton.Text = "↑";
        this.MoveUpButton.UseVisualStyleBackColor = true;
        this.MoveUpButton.Click += new System.EventHandler(this.MoveUpButton_Click);
        // 
        // MoveDownButton
        // 
        this.MoveDownButton.Dock = System.Windows.Forms.DockStyle.Fill;
        this.MoveDownButton.Location = new System.Drawing.Point(25, 491);
        this.MoveDownButton.Margin = new System.Windows.Forms.Padding(0);
        this.MoveDownButton.Name = "MoveDownButton";
        this.MoveDownButton.Size = new System.Drawing.Size(634, 25);
        this.MoveDownButton.TabIndex = 1;
        this.MoveDownButton.Text = "↓";
        this.MoveDownButton.UseVisualStyleBackColor = true;
        this.MoveDownButton.Click += new System.EventHandler(this.MoveDownButton_Click);
        // 
        // MoveLeftButton
        // 
        this.MoveLeftButton.Dock = System.Windows.Forms.DockStyle.Fill;
        this.MoveLeftButton.Location = new System.Drawing.Point(0, 25);
        this.MoveLeftButton.Margin = new System.Windows.Forms.Padding(0);
        this.MoveLeftButton.Name = "MoveLeftButton";
        this.MoveLeftButton.Size = new System.Drawing.Size(25, 466);
        this.MoveLeftButton.TabIndex = 2;
        this.MoveLeftButton.Text = "←";
        this.MoveLeftButton.UseVisualStyleBackColor = true;
        this.MoveLeftButton.Click += new System.EventHandler(this.MoveLeftButton_Click);
        // 
        // MoveRightButton
        // 
        this.MoveRightButton.Dock = System.Windows.Forms.DockStyle.Fill;
        this.MoveRightButton.Location = new System.Drawing.Point(659, 25);
        this.MoveRightButton.Margin = new System.Windows.Forms.Padding(0);
        this.MoveRightButton.Name = "MoveRightButton";
        this.MoveRightButton.Size = new System.Drawing.Size(25, 466);
        this.MoveRightButton.TabIndex = 3;
        this.MoveRightButton.Text = "→";
        this.MoveRightButton.UseVisualStyleBackColor = true;
        this.MoveRightButton.Click += new System.EventHandler(this.MoveRightButton_Click);
        // 
        // ZoomInButton
        // 
        this.ZoomInButton.Dock = System.Windows.Forms.DockStyle.Fill;
        this.ZoomInButton.Location = new System.Drawing.Point(659, 0);
        this.ZoomInButton.Margin = new System.Windows.Forms.Padding(0);
        this.ZoomInButton.Name = "ZoomInButton";
        this.ZoomInButton.Size = new System.Drawing.Size(25, 25);
        this.ZoomInButton.TabIndex = 4;
        this.ZoomInButton.Text = "+";
        this.ZoomInButton.UseVisualStyleBackColor = true;
        this.ZoomInButton.Click += new System.EventHandler(this.ZoomInButton_Click);
        // 
        // ZoomOutButton
        // 
        this.ZoomOutButton.Dock = System.Windows.Forms.DockStyle.Fill;
        this.ZoomOutButton.Location = new System.Drawing.Point(659, 491);
        this.ZoomOutButton.Margin = new System.Windows.Forms.Padding(0);
        this.ZoomOutButton.Name = "ZoomOutButton";
        this.ZoomOutButton.Size = new System.Drawing.Size(25, 25);
        this.ZoomOutButton.TabIndex = 5;
        this.ZoomOutButton.Text = "−";
        this.ZoomOutButton.UseVisualStyleBackColor = true;
        this.ZoomOutButton.Click += new System.EventHandler(this.ZoomOutButton_Click);
        // 
        // ZoomFitButton
        // 
        this.ZoomFitButton.Dock = System.Windows.Forms.DockStyle.Fill;
        this.ZoomFitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.ZoomFitButton.Location = new System.Drawing.Point(0, 491);
        this.ZoomFitButton.Margin = new System.Windows.Forms.Padding(0);
        this.ZoomFitButton.Name = "ZoomFitButton";
        this.ZoomFitButton.Size = new System.Drawing.Size(25, 25);
        this.ZoomFitButton.TabIndex = 7;
        this.ZoomFitButton.Text = "&Fit";
        this.ZoomFitButton.UseVisualStyleBackColor = true;
        this.ZoomFitButton.Click += new System.EventHandler(this.ZoomFitButton_Click);
        // 
        // ZoomActualButton
        // 
        this.ZoomActualButton.Dock = System.Windows.Forms.DockStyle.Fill;
        this.ZoomActualButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.ZoomActualButton.Location = new System.Drawing.Point(0, 0);
        this.ZoomActualButton.Margin = new System.Windows.Forms.Padding(0);
        this.ZoomActualButton.Name = "ZoomActualButton";
        this.ZoomActualButton.Size = new System.Drawing.Size(25, 25);
        this.ZoomActualButton.TabIndex = 6;
        this.ZoomActualButton.Text = "&1:1";
        this.ZoomActualButton.UseVisualStyleBackColor = true;
        this.ZoomActualButton.Click += new System.EventHandler(this.ZoomActualButton_Click);
        // 
        // MapView
        // 
        this.MapView.CenterPoint = new System.Drawing.Point(0, 0);
        this.MapView.Dock = System.Windows.Forms.DockStyle.Fill;
        this.MapView.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.MapView.Location = new System.Drawing.Point(27, 27);
        this.MapView.Margin = new System.Windows.Forms.Padding(2);
        this.MapView.Name = "MapView";
        this.MapView.Size = new System.Drawing.Size(630, 462);
        this.MapView.TabIndex = 8;
        this.MapView.Zoom = 0D;
        this.MapView.SizeChanged += new System.EventHandler(this.MapView_SizeChanged);
        // 
        // MainForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.CancelButton = this.ResetButon;
        this.ClientSize = new System.Drawing.Size(684, 562);
        this.Controls.Add(this.MainPanel);
        this.Name = "MainForm";
        this.Text = "Random Vector Map";
        this.MainPanel.ResumeLayout(false);
        this.MainPanel.PerformLayout();
        this.tableLayoutPanel1.ResumeLayout(false);
        this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel MainPanel;
    private System.Windows.Forms.Button StepButton;
    private System.Windows.Forms.Button TaskButton;
    private System.Windows.Forms.Button FinishButton;
    private System.Windows.Forms.TextBox ReseedTextBox;
    private System.Windows.Forms.Button ResetButon;
    private MapViewControl MapView;
    private System.Windows.Forms.Label StatusLabel;
    private System.Windows.Forms.Button RandomButton;
    private System.Windows.Forms.CheckBox ShowDebugCheckBox;
    private System.Windows.Forms.Label SizeLabel;
    private System.Windows.Forms.TextBox SizeTextBox;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.Button MoveUpButton;
    private System.Windows.Forms.Button MoveDownButton;
    private System.Windows.Forms.Button MoveLeftButton;
    private System.Windows.Forms.Button MoveRightButton;
    private System.Windows.Forms.Button ZoomInButton;
    private System.Windows.Forms.Button ZoomOutButton;
    private System.Windows.Forms.Button ZoomFitButton;
    private System.Windows.Forms.Button ZoomActualButton;
}

