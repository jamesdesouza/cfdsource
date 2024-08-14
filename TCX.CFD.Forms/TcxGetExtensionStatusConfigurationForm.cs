using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Properties;

namespace TCX.CFD.Forms;

public class TcxGetExtensionStatusConfigurationForm : Form
{
	private readonly TcxGetExtensionStatusComponent tcxGetExtensionStatusComponent;

	private readonly List<string> validVariables;

	private IContainer components;

	private Label lblExtension;

	private Button cancelButton;

	private Button okButton;

	private ErrorProvider errorProvider;

	private Button editExtensionButton;

	private TextBox txtExtension;

	public string Extension => txtExtension.Text;

	public TcxGetExtensionStatusConfigurationForm(TcxGetExtensionStatusComponent tcxGetExtensionStatusComponent)
	{
		InitializeComponent();
		this.tcxGetExtensionStatusComponent = tcxGetExtensionStatusComponent;
		validVariables = ExpressionHelper.GetValidVariables(tcxGetExtensionStatusComponent);
		txtExtension.Text = tcxGetExtensionStatusComponent.Extension;
		TxtExtension_Validating(txtExtension, new CancelEventArgs());
		Text = LocalizedResourceMgr.GetString("TcxGetExtensionStatusConfigurationForm.Title");
		lblExtension.Text = LocalizedResourceMgr.GetString("TcxGetExtensionStatusConfigurationForm.lblExtension.Text");
		okButton.Text = LocalizedResourceMgr.GetString("TcxGetExtensionStatusConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("TcxGetExtensionStatusConfigurationForm.cancelButton.Text");
	}

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void TxtExtension_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtExtension.Text))
		{
			errorProvider.SetError(editExtensionButton, LocalizedResourceMgr.GetString("TcxGetExtensionStatusConfigurationForm.Error.ExtensionIsMandatory"));
			return;
		}
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtExtension.Text);
		errorProvider.SetError(editExtensionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("TcxGetExtensionStatusConfigurationForm.Error.ExtensionIsInvalid"));
	}

	private void EditExtensionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(tcxGetExtensionStatusComponent);
		expressionEditorForm.Expression = txtExtension.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtExtension.Text = expressionEditorForm.Expression;
			TxtExtension_Validating(txtExtension, new CancelEventArgs());
		}
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void TcxGetExtensionStatusConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		tcxGetExtensionStatusComponent.ShowHelp();
	}

	private void TcxGetExtensionStatusConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		tcxGetExtensionStatusComponent.ShowHelp();
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
		this.lblExtension = new System.Windows.Forms.Label();
		this.cancelButton = new System.Windows.Forms.Button();
		this.okButton = new System.Windows.Forms.Button();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.editExtensionButton = new System.Windows.Forms.Button();
		this.txtExtension = new System.Windows.Forms.TextBox();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.lblExtension.AutoSize = true;
		this.lblExtension.Location = new System.Drawing.Point(16, 11);
		this.lblExtension.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblExtension.Name = "lblExtension";
		this.lblExtension.Size = new System.Drawing.Size(69, 17);
		this.lblExtension.TabIndex = 0;
		this.lblExtension.Text = "Extension";
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(451, 44);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 7;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(343, 44);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 6;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.editExtensionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.editExtensionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.editExtensionButton.Location = new System.Drawing.Point(559, 5);
		this.editExtensionButton.Margin = new System.Windows.Forms.Padding(4);
		this.editExtensionButton.Name = "editExtensionButton";
		this.editExtensionButton.Size = new System.Drawing.Size(39, 28);
		this.editExtensionButton.TabIndex = 2;
		this.editExtensionButton.UseVisualStyleBackColor = true;
		this.editExtensionButton.Click += new System.EventHandler(EditExtensionButton_Click);
		this.txtExtension.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtExtension.Location = new System.Drawing.Point(127, 7);
		this.txtExtension.Margin = new System.Windows.Forms.Padding(4);
		this.txtExtension.MaxLength = 8192;
		this.txtExtension.Name = "txtExtension";
		this.txtExtension.Size = new System.Drawing.Size(423, 22);
		this.txtExtension.TabIndex = 1;
		this.txtExtension.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtExtension.Validating += new System.ComponentModel.CancelEventHandler(TxtExtension_Validating);
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(619, 78);
		base.Controls.Add(this.editExtensionButton);
		base.Controls.Add(this.txtExtension);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.Controls.Add(this.lblExtension);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(1359, 125);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(634, 125);
		base.Name = "TcxGetExtensionStatusConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Get Extension Status";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(TcxGetExtensionStatusConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(TcxGetExtensionStatusConfigurationForm_HelpRequested);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
