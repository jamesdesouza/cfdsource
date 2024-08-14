using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class StartPageNewOrOpenProjectRowControl : UserControl
{
	public delegate void RowClickedHandler();

	private IContainer components;

	private Label lblDescription;

	private Label lblTitle;

	private PictureBox pictureBox;

	public string RowTitle
	{
		set
		{
			lblTitle.Text = value;
		}
	}

	public string RowDescription
	{
		set
		{
			lblDescription.Text = value;
		}
	}

	public Image RowPicture
	{
		set
		{
			pictureBox.Image = value;
		}
	}

	public event RowClickedHandler OnRowClicked;

	public StartPageNewOrOpenProjectRowControl()
	{
		InitializeComponent();
	}

	private void Label_Click(object sender, EventArgs e)
	{
		this.OnRowClicked?.Invoke();
	}

	private void Label_MouseEnter(object sender, EventArgs e)
	{
		lblTitle.ForeColor = Color.FromArgb(5, 151, 212);
		lblDescription.ForeColor = Color.FromArgb(5, 151, 212);
		Cursor = Cursors.Hand;
	}

	private void Label_MouseLeave(object sender, EventArgs e)
	{
		lblTitle.ForeColor = Color.Black;
		lblDescription.ForeColor = Color.FromArgb(64, 64, 64);
		Cursor = Cursors.Default;
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
		this.lblDescription = new System.Windows.Forms.Label();
		this.lblTitle = new System.Windows.Forms.Label();
		this.pictureBox = new System.Windows.Forms.PictureBox();
		((System.ComponentModel.ISupportInitialize)this.pictureBox).BeginInit();
		base.SuspendLayout();
		this.lblDescription.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lblDescription.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDescription.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.lblDescription.Location = new System.Drawing.Point(41, 25);
		this.lblDescription.Name = "lblDescription";
		this.lblDescription.Size = new System.Drawing.Size(250, 35);
		this.lblDescription.TabIndex = 10;
		this.lblDescription.Text = "Process inbound calls according to your business needs";
		this.lblDescription.Click += new System.EventHandler(Label_Click);
		this.lblDescription.MouseEnter += new System.EventHandler(Label_MouseEnter);
		this.lblDescription.MouseLeave += new System.EventHandler(Label_MouseLeave);
		this.lblTitle.AutoSize = true;
		this.lblTitle.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTitle.ForeColor = System.Drawing.Color.Black;
		this.lblTitle.Location = new System.Drawing.Point(41, 3);
		this.lblTitle.Name = "lblTitle";
		this.lblTitle.Size = new System.Drawing.Size(71, 18);
		this.lblTitle.TabIndex = 9;
		this.lblTitle.Text = "Callflow";
		this.lblTitle.Click += new System.EventHandler(Label_Click);
		this.lblTitle.MouseEnter += new System.EventHandler(Label_MouseEnter);
		this.lblTitle.MouseLeave += new System.EventHandler(Label_MouseLeave);
		this.pictureBox.Image = TCX.CFD.Properties.Resources.StartPage_NewProject;
		this.pictureBox.Location = new System.Drawing.Point(3, 9);
		this.pictureBox.Name = "pictureBox";
		this.pictureBox.Size = new System.Drawing.Size(32, 32);
		this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pictureBox.TabIndex = 8;
		this.pictureBox.TabStop = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.Controls.Add(this.lblDescription);
		base.Controls.Add(this.lblTitle);
		base.Controls.Add(this.pictureBox);
		this.ForeColor = System.Drawing.Color.Black;
		base.Name = "StartPageNewOrOpenProjectRowControl";
		base.Size = new System.Drawing.Size(294, 65);
		((System.ComponentModel.ISupportInitialize)this.pictureBox).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
