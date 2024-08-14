using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class VoiceInputHintBuilderTokenControl : UserControl
{
	private IContainer components;

	private Label lblToken;

	private TextBox txtToken;

	private PictureBox menuPicture;

	private ContextMenuStrip contextMenu;

	private ToolStripMenuItem deleteMenuOption;

	public VoiceInputHintToken Token
	{
		set
		{
			txtToken.Text = value.GetText();
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

	public VoiceInputHintBuilderTokenControl()
	{
		InitializeComponent();
		lblToken.Text = LocalizedResourceMgr.GetString("VoiceInputHintBuilderTokenControl.lblToken.Text");
		deleteMenuOption.Text = LocalizedResourceMgr.GetString("VoiceInputHintBuilderTokenControl.deleteMenuOption.Text");
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
		this.lblToken = new System.Windows.Forms.Label();
		this.txtToken = new System.Windows.Forms.TextBox();
		this.menuPicture = new System.Windows.Forms.PictureBox();
		this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.deleteMenuOption = new System.Windows.Forms.ToolStripMenuItem();
		((System.ComponentModel.ISupportInitialize)this.menuPicture).BeginInit();
		this.contextMenu.SuspendLayout();
		base.SuspendLayout();
		this.lblToken.AutoSize = true;
		this.lblToken.Location = new System.Drawing.Point(0, 0);
		this.lblToken.Name = "lblToken";
		this.lblToken.Size = new System.Drawing.Size(48, 17);
		this.lblToken.TabIndex = 0;
		this.lblToken.Text = "Token";
		this.txtToken.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtToken.Location = new System.Drawing.Point(0, 24);
		this.txtToken.Name = "txtToken";
		this.txtToken.ReadOnly = true;
		this.txtToken.Size = new System.Drawing.Size(422, 22);
		this.txtToken.TabIndex = 1;
		this.menuPicture.Image = TCX.CFD.Properties.Resources.Expression_Menu;
		this.menuPicture.Location = new System.Drawing.Point(54, 0);
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
		base.Controls.Add(this.txtToken);
		base.Controls.Add(this.lblToken);
		base.Name = "VoiceInputHintBuilderTokenControl";
		base.Size = new System.Drawing.Size(425, 50);
		((System.ComponentModel.ISupportInitialize)this.menuPicture).EndInit();
		this.contextMenu.ResumeLayout(false);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
