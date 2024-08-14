using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class ExpressionEditorVariableControl : UserControl, IExpressionEditorControl
{
	private IContainer components;

	private Label lblVariable;

	private TextBox txtVariableName;

	private PictureBox menuPicture;

	private ContextMenuStrip contextMenu;

	private ToolStripMenuItem deleteMenuOption;

	public VariableNameArgument Variable
	{
		set
		{
			txtVariableName.Text = value.GetString();
			if (value.IsRecordResult() || value.IsMenuResult() || value.IsUserInputResult() || value.IsVoiceInputResult())
			{
				lblVariable.Text = LocalizedResourceMgr.GetString("ExpressionEditorVariableControl.lblConstantValue.Text");
				menuPicture.Location = new Point(6 + lblVariable.Width, menuPicture.Location.Y);
			}
		}
	}

	private void MenuPicture_Click(object sender, EventArgs e)
	{
		contextMenu.Show(menuPicture, 10, 10);
	}

	private void DeleteButton_Click(object sender, EventArgs e)
	{
		(base.Parent?.Parent as ExpressionEditorRowControl)?.DeleteRow();
	}

	public AbsArgument GetArgument()
	{
		return new VariableNameArgument(txtVariableName.Text);
	}

	public void UpdateConstantValues()
	{
	}

	public ExpressionEditorVariableControl()
	{
		InitializeComponent();
		lblVariable.Text = LocalizedResourceMgr.GetString("ExpressionEditorVariableControl.lblVariable.Text");
		deleteMenuOption.Text = LocalizedResourceMgr.GetString("ExpressionEditorVariableControl.deleteMenuOption.Text");
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
		this.lblVariable = new System.Windows.Forms.Label();
		this.txtVariableName = new System.Windows.Forms.TextBox();
		this.menuPicture = new System.Windows.Forms.PictureBox();
		this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.deleteMenuOption = new System.Windows.Forms.ToolStripMenuItem();
		((System.ComponentModel.ISupportInitialize)this.menuPicture).BeginInit();
		this.contextMenu.SuspendLayout();
		base.SuspendLayout();
		this.lblVariable.AutoSize = true;
		this.lblVariable.Location = new System.Drawing.Point(0, 0);
		this.lblVariable.Name = "lblVariable";
		this.lblVariable.Size = new System.Drawing.Size(60, 17);
		this.lblVariable.TabIndex = 0;
		this.lblVariable.Text = "Variable";
		this.txtVariableName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtVariableName.Location = new System.Drawing.Point(0, 24);
		this.txtVariableName.Name = "txtVariableName";
		this.txtVariableName.ReadOnly = true;
		this.txtVariableName.Size = new System.Drawing.Size(422, 22);
		this.txtVariableName.TabIndex = 1;
		this.menuPicture.Image = TCX.CFD.Properties.Resources.Expression_Menu;
		this.menuPicture.Location = new System.Drawing.Point(66, -1);
		this.menuPicture.Name = "menuPicture";
		this.menuPicture.Size = new System.Drawing.Size(18, 18);
		this.menuPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		this.menuPicture.TabIndex = 12;
		this.menuPicture.TabStop = false;
		this.menuPicture.Click += new System.EventHandler(MenuPicture_Click);
		this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.deleteMenuOption });
		this.contextMenu.Name = "contextMenu";
		this.contextMenu.Size = new System.Drawing.Size(211, 56);
		this.deleteMenuOption.Image = TCX.CFD.Properties.Resources.Edit_Remove;
		this.deleteMenuOption.Name = "deleteMenuOption";
		this.deleteMenuOption.Size = new System.Drawing.Size(210, 24);
		this.deleteMenuOption.Text = "Delete";
		this.deleteMenuOption.Click += new System.EventHandler(DeleteButton_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.menuPicture);
		base.Controls.Add(this.txtVariableName);
		base.Controls.Add(this.lblVariable);
		base.Name = "ExpressionEditorVariableControl";
		base.Size = new System.Drawing.Size(425, 50);
		((System.ComponentModel.ISupportInitialize)this.menuPicture).EndInit();
		this.contextMenu.ResumeLayout(false);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
