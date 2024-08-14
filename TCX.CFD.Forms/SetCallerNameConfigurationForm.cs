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

public class SetCallerNameConfigurationForm : Form
{
	private readonly SetCallerNameComponent setCallerNameComponent;

	private readonly List<string> validVariables;

	private IContainer components;

	private Label lblDisplayName;

	private TextBox txtDisplayName;

	private Button cancelButton;

	private Button okButton;

	private Button displayNameExpressionButton;

	private ErrorProvider errorProvider;

	public string DisplayName => txtDisplayName.Text;

	public SetCallerNameConfigurationForm(SetCallerNameComponent setCallerNameComponent)
	{
		InitializeComponent();
		this.setCallerNameComponent = setCallerNameComponent;
		validVariables = ExpressionHelper.GetValidVariables(setCallerNameComponent);
		txtDisplayName.Text = setCallerNameComponent.DisplayName;
		TxtDisplayName_Validating(txtDisplayName, new CancelEventArgs());
		Text = LocalizedResourceMgr.GetString("SetCallerNameConfigurationForm.Title");
		lblDisplayName.Text = LocalizedResourceMgr.GetString("SetCallerNameConfigurationForm.lblDisplayName.Text");
		okButton.Text = LocalizedResourceMgr.GetString("SetCallerNameConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("SetCallerNameConfigurationForm.cancelButton.Text");
	}

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void TxtDisplayName_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtDisplayName.Text))
		{
			errorProvider.SetError(displayNameExpressionButton, LocalizedResourceMgr.GetString("SetCallerNameConfigurationForm.Error.DisplayNameIsMandatory"));
			return;
		}
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtDisplayName.Text);
		errorProvider.SetError(displayNameExpressionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("SetCallerNameConfigurationForm.Error.DisplayNameIsInvalid"));
	}

	private void DisplayNameExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(setCallerNameComponent);
		expressionEditorForm.Expression = txtDisplayName.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtDisplayName.Text = expressionEditorForm.Expression;
			TxtDisplayName_Validating(txtDisplayName, new CancelEventArgs());
		}
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void SetCallerNameConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		setCallerNameComponent.ShowHelp();
	}

	private void SetCallerNameConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		setCallerNameComponent.ShowHelp();
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
		this.lblDisplayName = new System.Windows.Forms.Label();
		this.txtDisplayName = new System.Windows.Forms.TextBox();
		this.cancelButton = new System.Windows.Forms.Button();
		this.okButton = new System.Windows.Forms.Button();
		this.displayNameExpressionButton = new System.Windows.Forms.Button();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.lblDisplayName.AutoSize = true;
		this.lblDisplayName.Location = new System.Drawing.Point(16, 11);
		this.lblDisplayName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblDisplayName.Name = "lblDisplayName";
		this.lblDisplayName.Size = new System.Drawing.Size(95, 17);
		this.lblDisplayName.TabIndex = 0;
		this.lblDisplayName.Text = "Display Name";
		this.txtDisplayName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtDisplayName.Location = new System.Drawing.Point(119, 7);
		this.txtDisplayName.Margin = new System.Windows.Forms.Padding(4);
		this.txtDisplayName.Name = "txtDisplayName";
		this.txtDisplayName.Size = new System.Drawing.Size(393, 22);
		this.txtDisplayName.TabIndex = 1;
		this.txtDisplayName.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtDisplayName.Validating += new System.ComponentModel.CancelEventHandler(TxtDisplayName_Validating);
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(412, 47);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 4;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(304, 47);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 3;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.displayNameExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.displayNameExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.displayNameExpressionButton.Location = new System.Drawing.Point(522, 5);
		this.displayNameExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.displayNameExpressionButton.Name = "displayNameExpressionButton";
		this.displayNameExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.displayNameExpressionButton.TabIndex = 2;
		this.displayNameExpressionButton.UseVisualStyleBackColor = true;
		this.displayNameExpressionButton.Click += new System.EventHandler(DisplayNameExpressionButton_Click);
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(582, 88);
		base.Controls.Add(this.displayNameExpressionButton);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.Controls.Add(this.txtDisplayName);
		base.Controls.Add(this.lblDisplayName);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(1200, 135);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(600, 135);
		base.Name = "SetCallerNameConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Set Caller Name";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(SetCallerNameConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(SetCallerNameConfigurationForm_HelpRequested);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
