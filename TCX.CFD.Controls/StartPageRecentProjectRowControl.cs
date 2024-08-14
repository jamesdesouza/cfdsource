using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class StartPageRecentProjectRowControl : UserControl
{
	public delegate void RecentProjectClickedHandler(string projectPath);

	public RecentProjectClickedHandler OnRecentProjectClicked;

	private IContainer components;

	private PictureBox pictureBox;

	private Label lblProjectName;

	private Label lblProjectPath;

	private ToolTip recentProjectToolTip;

	public string ProjectPath
	{
		set
		{
			FileInfo fileInfo = new FileInfo(value);
			lblProjectName.Text = fileInfo.Name;
			lblProjectPath.Text = value;
			string caption = "Open Project\n" + value;
			recentProjectToolTip.SetToolTip(lblProjectName, caption);
			recentProjectToolTip.SetToolTip(lblProjectPath, caption);
			recentProjectToolTip.SetToolTip(pictureBox, caption);
			recentProjectToolTip.SetToolTip(this, caption);
			RecalculateHeight();
		}
	}

	public StartPageRecentProjectRowControl()
	{
		InitializeComponent();
	}

	private void StartPageRecentProjectRowControl_MouseEnter(object sender, EventArgs e)
	{
		lblProjectName.ForeColor = Color.FromArgb(5, 151, 212);
		lblProjectPath.ForeColor = Color.FromArgb(5, 151, 212);
		Cursor = Cursors.Hand;
	}

	private void StartPageRecentProjectRowControl_MouseLeave(object sender, EventArgs e)
	{
		lblProjectName.ForeColor = Color.Black;
		lblProjectPath.ForeColor = Color.FromArgb(64, 64, 64);
		Cursor = Cursors.Default;
	}

	private void StartPageRecentProjectRowControl_Click(object sender, EventArgs e)
	{
		OnRecentProjectClicked?.Invoke(lblProjectPath.Text);
	}

	private void LblProjectName_Click(object sender, EventArgs e)
	{
		OnRecentProjectClicked?.Invoke(lblProjectPath.Text);
	}

	private void LblProjectPath_Click(object sender, EventArgs e)
	{
		OnRecentProjectClicked?.Invoke(lblProjectPath.Text);
	}

	private void PictureBox_Click(object sender, EventArgs e)
	{
		OnRecentProjectClicked?.Invoke(lblProjectPath.Text);
	}

	private void RecalculateHeight()
	{
		using Graphics graphics = CreateGraphics();
		SizeF sizeF = graphics.MeasureString(lblProjectPath.Text, lblProjectPath.Font, lblProjectPath.Width - lblProjectPath.Margin.Horizontal - lblProjectPath.Padding.Horizontal - 21, StringFormat.GenericTypographic);
		base.Height = 5 + lblProjectPath.Location.Y + (int)Math.Ceiling(sizeF.Height);
	}

	private void StartPageRecentProjectRowControl_SizeChanged(object sender, EventArgs e)
	{
		RecalculateHeight();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		this.pictureBox = new System.Windows.Forms.PictureBox();
		this.lblProjectName = new System.Windows.Forms.Label();
		this.lblProjectPath = new System.Windows.Forms.Label();
		this.recentProjectToolTip = new System.Windows.Forms.ToolTip(this.components);
		((System.ComponentModel.ISupportInitialize)this.pictureBox).BeginInit();
		base.SuspendLayout();
		this.pictureBox.Image = TCX.CFD.Properties.Resources.StartPage_OpenProject;
		this.pictureBox.Location = new System.Drawing.Point(3, 7);
		this.pictureBox.Name = "pictureBox";
		this.pictureBox.Size = new System.Drawing.Size(32, 32);
		this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pictureBox.TabIndex = 5;
		this.pictureBox.TabStop = false;
		this.pictureBox.Click += new System.EventHandler(PictureBox_Click);
		this.pictureBox.MouseEnter += new System.EventHandler(StartPageRecentProjectRowControl_MouseEnter);
		this.pictureBox.MouseLeave += new System.EventHandler(StartPageRecentProjectRowControl_MouseLeave);
		this.lblProjectName.AutoSize = true;
		this.lblProjectName.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblProjectName.ForeColor = System.Drawing.Color.Black;
		this.lblProjectName.Location = new System.Drawing.Point(41, 3);
		this.lblProjectName.Name = "lblProjectName";
		this.lblProjectName.Size = new System.Drawing.Size(118, 18);
		this.lblProjectName.TabIndex = 6;
		this.lblProjectName.Text = "Project name";
		this.lblProjectName.Click += new System.EventHandler(LblProjectName_Click);
		this.lblProjectName.MouseEnter += new System.EventHandler(StartPageRecentProjectRowControl_MouseEnter);
		this.lblProjectName.MouseLeave += new System.EventHandler(StartPageRecentProjectRowControl_MouseLeave);
		this.lblProjectPath.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lblProjectPath.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblProjectPath.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.lblProjectPath.Location = new System.Drawing.Point(41, 25);
		this.lblProjectPath.Name = "lblProjectPath";
		this.lblProjectPath.Size = new System.Drawing.Size(361, 30);
		this.lblProjectPath.TabIndex = 7;
		this.lblProjectPath.Text = "Project path";
		this.lblProjectPath.Click += new System.EventHandler(LblProjectPath_Click);
		this.lblProjectPath.MouseEnter += new System.EventHandler(StartPageRecentProjectRowControl_MouseEnter);
		this.lblProjectPath.MouseLeave += new System.EventHandler(StartPageRecentProjectRowControl_MouseLeave);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.Controls.Add(this.lblProjectPath);
		base.Controls.Add(this.lblProjectName);
		base.Controls.Add(this.pictureBox);
		base.Name = "StartPageRecentProjectRowControl";
		base.Size = new System.Drawing.Size(405, 60);
		base.SizeChanged += new System.EventHandler(StartPageRecentProjectRowControl_SizeChanged);
		base.Click += new System.EventHandler(StartPageRecentProjectRowControl_Click);
		base.MouseEnter += new System.EventHandler(StartPageRecentProjectRowControl_MouseEnter);
		base.MouseLeave += new System.EventHandler(StartPageRecentProjectRowControl_MouseLeave);
		((System.ComponentModel.ISupportInitialize)this.pictureBox).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
