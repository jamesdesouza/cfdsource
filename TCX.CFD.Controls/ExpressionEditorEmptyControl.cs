using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class ExpressionEditorEmptyControl : UserControl, IExpressionEditorControl
{
	private IContainer components;

	private Label lblDragAndDrop;

	private Panel borderPanel;

	private PictureBox deletePicture;

	public bool DeleteEnabled
	{
		set
		{
			deletePicture.Visible = value;
		}
	}

	public AbsArgument GetArgument()
	{
		return new DotNetExpressionArgument("");
	}

	public void UpdateConstantValues()
	{
	}

	public ExpressionEditorEmptyControl()
	{
		InitializeComponent();
		lblDragAndDrop.Text = LocalizedResourceMgr.GetString("ExpressionEditorEmptyControl.lblDragAndDrop.Text");
	}

	private void BorderPanel_Paint(object sender, PaintEventArgs e)
	{
		ControlPaint.DrawBorder(e.Graphics, borderPanel.ClientRectangle, Color.Black, ButtonBorderStyle.Dashed);
	}

	private void BorderPanel_Resize(object sender, EventArgs e)
	{
		borderPanel.Invalidate();
	}

	private void DeletePicture_Click(object sender, EventArgs e)
	{
		(base.Parent?.Parent as ExpressionEditorRowControl)?.DeleteRow();
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
		this.lblDragAndDrop = new System.Windows.Forms.Label();
		this.borderPanel = new System.Windows.Forms.Panel();
		this.deletePicture = new System.Windows.Forms.PictureBox();
		this.borderPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.deletePicture).BeginInit();
		base.SuspendLayout();
		this.lblDragAndDrop.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lblDragAndDrop.Location = new System.Drawing.Point(5, 5);
		this.lblDragAndDrop.Name = "lblDragAndDrop";
		this.lblDragAndDrop.Size = new System.Drawing.Size(415, 40);
		this.lblDragAndDrop.TabIndex = 0;
		this.lblDragAndDrop.Text = "Drag n Drop an Expression Element.";
		this.lblDragAndDrop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.borderPanel.Controls.Add(this.deletePicture);
		this.borderPanel.Controls.Add(this.lblDragAndDrop);
		this.borderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.borderPanel.Location = new System.Drawing.Point(0, 0);
		this.borderPanel.Name = "borderPanel";
		this.borderPanel.Size = new System.Drawing.Size(425, 50);
		this.borderPanel.TabIndex = 1;
		this.borderPanel.Paint += new System.Windows.Forms.PaintEventHandler(BorderPanel_Paint);
		this.borderPanel.Resize += new System.EventHandler(BorderPanel_Resize);
		this.deletePicture.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.deletePicture.Image = TCX.CFD.Properties.Resources.Expression_DeleteRow;
		this.deletePicture.Location = new System.Drawing.Point(390, 15);
		this.deletePicture.Name = "deletePicture";
		this.deletePicture.Size = new System.Drawing.Size(20, 20);
		this.deletePicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		this.deletePicture.TabIndex = 1;
		this.deletePicture.TabStop = false;
		this.deletePicture.Visible = false;
		this.deletePicture.Click += new System.EventHandler(DeletePicture_Click);
		this.AllowDrop = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.borderPanel);
		base.Name = "ExpressionEditorEmptyControl";
		base.Size = new System.Drawing.Size(425, 50);
		this.borderPanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.deletePicture).EndInit();
		base.ResumeLayout(false);
	}
}
