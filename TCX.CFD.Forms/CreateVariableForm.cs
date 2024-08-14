using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.FileSystem;
using TCX.CFD.Properties;

namespace TCX.CFD.Forms;

public class CreateVariableForm : Form
{
	private readonly IVadActivity vadActivity;

	private IContainer components;

	private TextBox txtVariableName;

	private Label lblVariableName;

	private Button cancelButton;

	private Button okButton;

	public string CreatedVariableName => "callflow$." + txtVariableName.Text;

	public CreateVariableForm(IVadActivity vadActivity)
	{
		InitializeComponent();
		this.vadActivity = vadActivity;
		Text = LocalizedResourceMgr.GetString("CreateVariableForm.Text");
		lblVariableName.Text = LocalizedResourceMgr.GetString("CreateVariableForm.lblVariableName.Text");
		okButton.Text = LocalizedResourceMgr.GetString("CreateVariableForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("CreateVariableForm.cancelButton.Text");
	}

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(txtVariableName.Text))
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("CreateVariableForm.MessageBox.Error.VariableNameRequired"), LocalizedResourceMgr.GetString("CreateVariableForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			txtVariableName.Focus();
			return;
		}
		RootFlow rootFlow = vadActivity.GetRootFlow();
		foreach (Variable property in rootFlow.Properties)
		{
			if (property.Name == txtVariableName.Text)
			{
				MessageBox.Show(LocalizedResourceMgr.GetString("CreateVariableForm.MessageBox.Error.VariableNameAlreadyExists"), LocalizedResourceMgr.GetString("CreateVariableForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				txtVariableName.Focus();
				return;
			}
		}
		List<Variable> properties = rootFlow.Properties;
		properties.Add(new Variable(txtVariableName.Text, VariableScopes.Public, VariableAccessibilities.ReadWrite));
		rootFlow.FileObject.Variables = properties;
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void CreateVariableForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		vadActivity.ShowHelp();
	}

	private void CreateVariableForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		vadActivity.ShowHelp();
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
		this.txtVariableName = new System.Windows.Forms.TextBox();
		this.lblVariableName = new System.Windows.Forms.Label();
		this.cancelButton = new System.Windows.Forms.Button();
		this.okButton = new System.Windows.Forms.Button();
		base.SuspendLayout();
		this.txtVariableName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtVariableName.Location = new System.Drawing.Point(125, 11);
		this.txtVariableName.Margin = new System.Windows.Forms.Padding(4);
		this.txtVariableName.MaxLength = 8192;
		this.txtVariableName.Name = "txtVariableName";
		this.txtVariableName.Size = new System.Drawing.Size(478, 22);
		this.txtVariableName.TabIndex = 1;
		this.txtVariableName.Enter += new System.EventHandler(TxtBox_Enter);
		this.lblVariableName.AutoSize = true;
		this.lblVariableName.Location = new System.Drawing.Point(16, 11);
		this.lblVariableName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblVariableName.Name = "lblVariableName";
		this.lblVariableName.Size = new System.Drawing.Size(101, 17);
		this.lblVariableName.TabIndex = 0;
		this.lblVariableName.Text = "Variable Name";
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(503, 48);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 3;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(395, 48);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 2;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(616, 89);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.Controls.Add(this.lblVariableName);
		base.Controls.Add(this.txtVariableName);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(1359, 136);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(634, 136);
		base.Name = "CreateVariableForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Create Variable";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(CreateVariableForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(CreateVariableForm_HelpRequested);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
