using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;

namespace TCX.CFD.Controls;

public class JsonXmlParserEmptyMappingControl : UserControl
{
	private IContainer components;

	private Label lblDragAndDrop;

	private Panel borderPanel;

	public JsonXmlParserEmptyMappingControl()
	{
		InitializeComponent();
		lblDragAndDrop.Text = LocalizedResourceMgr.GetString("JsonXmlParserEmptyMappingControl.lblDragAndDrop.Text");
	}

	private void BorderPanel_Paint(object sender, PaintEventArgs e)
	{
		ControlPaint.DrawBorder(e.Graphics, borderPanel.ClientRectangle, Color.Black, ButtonBorderStyle.Dashed);
	}

	private void BorderPanel_Resize(object sender, EventArgs e)
	{
		borderPanel.Invalidate();
	}

	private void JsonXmlParserEmptyMappingControl_DragEnter(object sender, DragEventArgs e)
	{
		e.Effect = (e.Data.GetDataPresent(DataFormats.Text) ? DragDropEffects.Move : DragDropEffects.None);
	}

	private void JsonXmlParserEmptyMappingControl_DragDrop(object sender, DragEventArgs e)
	{
		string path = e.Data.GetData(DataFormats.Text).ToString();
		(base.Parent?.Parent?.Parent as JsonXmlParserWizardPage2)?.AddResponseMapping(new ResponseMapping(path, ""));
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
		this.borderPanel.SuspendLayout();
		base.SuspendLayout();
		this.lblDragAndDrop.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lblDragAndDrop.Location = new System.Drawing.Point(5, 5);
		this.lblDragAndDrop.Name = "lblDragAndDrop";
		this.lblDragAndDrop.Size = new System.Drawing.Size(390, 45);
		this.lblDragAndDrop.TabIndex = 0;
		this.lblDragAndDrop.Text = "Drag-n-drop a node from the tree-view on the left.";
		this.lblDragAndDrop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.borderPanel.Controls.Add(this.lblDragAndDrop);
		this.borderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.borderPanel.Location = new System.Drawing.Point(0, 0);
		this.borderPanel.Name = "borderPanel";
		this.borderPanel.Size = new System.Drawing.Size(400, 55);
		this.borderPanel.TabIndex = 1;
		this.borderPanel.Paint += new System.Windows.Forms.PaintEventHandler(BorderPanel_Paint);
		this.borderPanel.Resize += new System.EventHandler(BorderPanel_Resize);
		this.AllowDrop = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.borderPanel);
		base.Name = "JsonXmlParserEmptyMappingControl";
		base.Size = new System.Drawing.Size(400, 55);
		base.DragDrop += new System.Windows.Forms.DragEventHandler(JsonXmlParserEmptyMappingControl_DragDrop);
		base.DragEnter += new System.Windows.Forms.DragEventHandler(JsonXmlParserEmptyMappingControl_DragEnter);
		this.borderPanel.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
