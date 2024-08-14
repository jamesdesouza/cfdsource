using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TCX.CFD.Controls;

public class ProjectTypeControl : UserControl
{
	private IContainer components;

	private Panel selectedPanel;

	private Panel typeNamePanel;

	private Label lblProjectType;

	private PictureBox pictureBox;

	public string Title
	{
		get
		{
			return lblProjectType.Text;
		}
		set
		{
			lblProjectType.Text = value;
		}
	}

	public string Description { get; set; }

	public Image ProjectImage
	{
		set
		{
			pictureBox.Image = value;
		}
	}

	public bool IsSelected
	{
		set
		{
			if (value)
			{
				selectedPanel.BorderStyle = BorderStyle.FixedSingle;
				typeNamePanel.BackColor = Color.FromArgb(5, 151, 212);
				lblProjectType.ForeColor = Color.White;
			}
			else
			{
				selectedPanel.BorderStyle = BorderStyle.None;
				typeNamePanel.BackColor = Color.White;
				lblProjectType.ForeColor = Color.FromArgb(5, 151, 212);
			}
		}
	}

	public event EventHandler OnProjectSelected;

	public ProjectTypeControl()
	{
		InitializeComponent();
	}

	private void ProjectTypeControl_Click(object sender, EventArgs e)
	{
		this.OnProjectSelected?.Invoke(this, EventArgs.Empty);
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
		this.selectedPanel = new System.Windows.Forms.Panel();
		this.typeNamePanel = new System.Windows.Forms.Panel();
		this.lblProjectType = new System.Windows.Forms.Label();
		this.pictureBox = new System.Windows.Forms.PictureBox();
		this.selectedPanel.SuspendLayout();
		this.typeNamePanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox).BeginInit();
		base.SuspendLayout();
		this.selectedPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.selectedPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.selectedPanel.Controls.Add(this.typeNamePanel);
		this.selectedPanel.Controls.Add(this.pictureBox);
		this.selectedPanel.ForeColor = System.Drawing.Color.FromArgb(5, 151, 212);
		this.selectedPanel.Location = new System.Drawing.Point(3, 3);
		this.selectedPanel.Name = "selectedPanel";
		this.selectedPanel.Size = new System.Drawing.Size(214, 174);
		this.selectedPanel.TabIndex = 0;
		this.selectedPanel.Click += new System.EventHandler(ProjectTypeControl_Click);
		this.typeNamePanel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.typeNamePanel.Controls.Add(this.lblProjectType);
		this.typeNamePanel.Location = new System.Drawing.Point(0, 137);
		this.typeNamePanel.Name = "typeNamePanel";
		this.typeNamePanel.Size = new System.Drawing.Size(214, 38);
		this.typeNamePanel.TabIndex = 1;
		this.typeNamePanel.Click += new System.EventHandler(ProjectTypeControl_Click);
		this.lblProjectType.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblProjectType.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblProjectType.Location = new System.Drawing.Point(0, 0);
		this.lblProjectType.Name = "lblProjectType";
		this.lblProjectType.Size = new System.Drawing.Size(214, 38);
		this.lblProjectType.TabIndex = 0;
		this.lblProjectType.Text = "Database Access";
		this.lblProjectType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblProjectType.Click += new System.EventHandler(ProjectTypeControl_Click);
		this.pictureBox.Location = new System.Drawing.Point(41, 3);
		this.pictureBox.Name = "pictureBox";
		this.pictureBox.Size = new System.Drawing.Size(128, 128);
		this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pictureBox.TabIndex = 0;
		this.pictureBox.TabStop = false;
		this.pictureBox.Click += new System.EventHandler(ProjectTypeControl_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.selectedPanel);
		base.Name = "ProjectTypeControl";
		base.Size = new System.Drawing.Size(220, 180);
		base.Click += new System.EventHandler(ProjectTypeControl_Click);
		this.selectedPanel.ResumeLayout(false);
		this.typeNamePanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.pictureBox).EndInit();
		base.ResumeLayout(false);
	}
}
