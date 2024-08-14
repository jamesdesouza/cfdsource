using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class VoiceInputHintBuilderConstantValueControl : UserControl
{
	private IContainer components;

	private TextBox txtConstantValue;

	private Label lblConstantValue;

	private PictureBox menuPicture;

	private ContextMenuStrip contextMenu;

	private ToolStripMenuItem deleteMenuOption;

	public VoiceInputHintConstantValue ConstantValue
	{
		set
		{
			txtConstantValue.Text = value.GetText();
		}
	}

	private void MenuPicture_Click(object sender, EventArgs e)
	{
		contextMenu.Show(menuPicture, 10, 10);
	}

	private void DeleteButton_Click(object sender, EventArgs e)
	{
		(base.Parent?.Parent as VoiceInputHintBuilderRowControl)?.DeleteRow();
	}

	private void TxtConstantValue_TextChanged(object sender, EventArgs e)
	{
		(base.Parent?.Parent as VoiceInputHintBuilderRowControl)?.UpdateRow(new VoiceInputHintConstantValue(txtConstantValue.Text));
	}

	public VoiceInputHintBuilderConstantValueControl()
	{
		InitializeComponent();
		lblConstantValue.Text = LocalizedResourceMgr.GetString("VoiceInputHintBuilderConstantValueControl.lblConstantValue.Text");
		deleteMenuOption.Text = LocalizedResourceMgr.GetString("VoiceInputHintBuilderConstantValueControl.deleteMenuOption.Text");
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
		this.txtConstantValue = new System.Windows.Forms.TextBox();
		this.lblConstantValue = new System.Windows.Forms.Label();
		this.menuPicture = new System.Windows.Forms.PictureBox();
		this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.deleteMenuOption = new System.Windows.Forms.ToolStripMenuItem();
		((System.ComponentModel.ISupportInitialize)this.menuPicture).BeginInit();
		this.contextMenu.SuspendLayout();
		base.SuspendLayout();
		this.txtConstantValue.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtConstantValue.Location = new System.Drawing.Point(0, 24);
		this.txtConstantValue.Name = "txtConstantValue";
		this.txtConstantValue.Size = new System.Drawing.Size(422, 22);
		this.txtConstantValue.TabIndex = 1;
		this.txtConstantValue.TextChanged += new System.EventHandler(TxtConstantValue_TextChanged);
		this.lblConstantValue.AutoSize = true;
		this.lblConstantValue.Location = new System.Drawing.Point(0, 0);
		this.lblConstantValue.Name = "lblConstantValue";
		this.lblConstantValue.Size = new System.Drawing.Size(104, 17);
		this.lblConstantValue.TabIndex = 4;
		this.lblConstantValue.Text = "Constant Value";
		this.menuPicture.Image = TCX.CFD.Properties.Resources.Expression_Menu;
		this.menuPicture.Location = new System.Drawing.Point(110, -1);
		this.menuPicture.Name = "menuPicture";
		this.menuPicture.Size = new System.Drawing.Size(18, 18);
		this.menuPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		this.menuPicture.TabIndex = 12;
		this.menuPicture.TabStop = false;
		this.menuPicture.Click += new System.EventHandler(MenuPicture_Click);
		this.contextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.deleteMenuOption });
		this.contextMenu.Name = "contextMenu";
		this.contextMenu.Size = new System.Drawing.Size(127, 30);
		this.deleteMenuOption.Image = TCX.CFD.Properties.Resources.Edit_Remove;
		this.deleteMenuOption.Name = "deleteMenuOption";
		this.deleteMenuOption.Size = new System.Drawing.Size(126, 26);
		this.deleteMenuOption.Text = "Delete";
		this.deleteMenuOption.Click += new System.EventHandler(DeleteButton_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.menuPicture);
		base.Controls.Add(this.lblConstantValue);
		base.Controls.Add(this.txtConstantValue);
		base.Name = "VoiceInputHintBuilderConstantValueControl";
		base.Size = new System.Drawing.Size(425, 50);
		((System.ComponentModel.ISupportInitialize)this.menuPicture).EndInit();
		this.contextMenu.ResumeLayout(false);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
