using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TCX.CFD.Controls;

public class StartPageGetStartedRowControl : UserControl
{
	public delegate void RowClickedHandler(StartPageGetStartedRowControl rowControl);

	private IContainer components;

	private Label lblTitle;

	public string Title
	{
		set
		{
			lblTitle.Text = value;
			RecalculateHeight();
		}
	}

	public event RowClickedHandler OnRowClicked;

	public StartPageGetStartedRowControl()
	{
		InitializeComponent();
	}

	private void LblTitle_MouseEnter(object sender, EventArgs e)
	{
		lblTitle.ForeColor = Color.FromArgb(5, 151, 212);
		Cursor = Cursors.Hand;
	}

	private void LblTitle_MouseLeave(object sender, EventArgs e)
	{
		lblTitle.ForeColor = Color.Black;
		Cursor = Cursors.Default;
	}

	private void LblTitle_Click(object sender, EventArgs e)
	{
		this.OnRowClicked?.Invoke(this);
	}

	private void RecalculateHeight()
	{
		using Graphics graphics = CreateGraphics();
		base.Height = (int)Math.Ceiling(graphics.MeasureString(lblTitle.Text, lblTitle.Font, lblTitle.Width - lblTitle.Margin.Horizontal - lblTitle.Padding.Horizontal - 21).Height);
	}

	private void LblTitle_SizeChanged(object sender, EventArgs e)
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
		this.lblTitle = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTitle.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTitle.ForeColor = System.Drawing.Color.Black;
		this.lblTitle.Location = new System.Drawing.Point(0, 0);
		this.lblTitle.Name = "lblTitle";
		this.lblTitle.Size = new System.Drawing.Size(547, 27);
		this.lblTitle.TabIndex = 0;
		this.lblTitle.Text = "Get Started";
		this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblTitle.SizeChanged += new System.EventHandler(LblTitle_SizeChanged);
		this.lblTitle.Click += new System.EventHandler(LblTitle_Click);
		this.lblTitle.MouseEnter += new System.EventHandler(LblTitle_MouseEnter);
		this.lblTitle.MouseLeave += new System.EventHandler(LblTitle_MouseLeave);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.Controls.Add(this.lblTitle);
		base.Name = "StartPageGetStartedRowControl";
		base.Size = new System.Drawing.Size(547, 27);
		base.ResumeLayout(false);
	}
}
