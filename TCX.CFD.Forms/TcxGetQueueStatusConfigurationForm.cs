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

public class TcxGetQueueStatusConfigurationForm : Form
{
	private readonly TcxGetQueueStatusComponent tcxGetQueueStatusComponent;

	private readonly List<string> validVariables;

	private IContainer components;

	private Label lblQueueExtension;

	private Button cancelButton;

	private Button okButton;

	private ErrorProvider errorProvider;

	private Button editQueueExtensionButton;

	private TextBox txtQueueExtension;

	public string QueueExtension => txtQueueExtension.Text;

	public TcxGetQueueStatusConfigurationForm(TcxGetQueueStatusComponent tcxGetQueueStatusComponent)
	{
		InitializeComponent();
		this.tcxGetQueueStatusComponent = tcxGetQueueStatusComponent;
		validVariables = ExpressionHelper.GetValidVariables(tcxGetQueueStatusComponent);
		txtQueueExtension.Text = tcxGetQueueStatusComponent.QueueExtension;
		TxtQueueExtension_Validating(txtQueueExtension, new CancelEventArgs());
		Text = LocalizedResourceMgr.GetString("TcxGetQueueStatusConfigurationForm.Title");
		lblQueueExtension.Text = LocalizedResourceMgr.GetString("TcxGetQueueStatusConfigurationForm.lblQueueExtension.Text");
		okButton.Text = LocalizedResourceMgr.GetString("TcxGetQueueStatusConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("TcxGetQueueStatusConfigurationForm.cancelButton.Text");
	}

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void TxtQueueExtension_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtQueueExtension.Text))
		{
			errorProvider.SetError(editQueueExtensionButton, LocalizedResourceMgr.GetString("TcxGetQueueStatusConfigurationForm.Error.QueueExtensionIsMandatory"));
			return;
		}
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtQueueExtension.Text);
		errorProvider.SetError(editQueueExtensionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("TcxGetQueueStatusConfigurationForm.Error.QueueExtensionIsInvalid"));
	}

	private void EditQueueExtensionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(tcxGetQueueStatusComponent);
		expressionEditorForm.Expression = txtQueueExtension.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtQueueExtension.Text = expressionEditorForm.Expression;
			TxtQueueExtension_Validating(txtQueueExtension, new CancelEventArgs());
		}
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void TcxGetQueueStatusConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		tcxGetQueueStatusComponent.ShowHelp();
	}

	private void TcxGetQueueStatusConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		tcxGetQueueStatusComponent.ShowHelp();
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
		this.lblQueueExtension = new System.Windows.Forms.Label();
		this.cancelButton = new System.Windows.Forms.Button();
		this.okButton = new System.Windows.Forms.Button();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.editQueueExtensionButton = new System.Windows.Forms.Button();
		this.txtQueueExtension = new System.Windows.Forms.TextBox();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.lblQueueExtension.AutoSize = true;
		this.lblQueueExtension.Location = new System.Drawing.Point(16, 11);
		this.lblQueueExtension.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblQueueExtension.Name = "lblQueueExtension";
		this.lblQueueExtension.Size = new System.Drawing.Size(116, 17);
		this.lblQueueExtension.TabIndex = 0;
		this.lblQueueExtension.Text = "Queue Extension";
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(451, 45);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 6;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(343, 45);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 5;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.editQueueExtensionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.editQueueExtensionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.editQueueExtensionButton.Location = new System.Drawing.Point(559, 5);
		this.editQueueExtensionButton.Margin = new System.Windows.Forms.Padding(4);
		this.editQueueExtensionButton.Name = "editQueueExtensionButton";
		this.editQueueExtensionButton.Size = new System.Drawing.Size(39, 28);
		this.editQueueExtensionButton.TabIndex = 2;
		this.editQueueExtensionButton.UseVisualStyleBackColor = true;
		this.editQueueExtensionButton.Click += new System.EventHandler(EditQueueExtensionButton_Click);
		this.txtQueueExtension.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtQueueExtension.Location = new System.Drawing.Point(141, 7);
		this.txtQueueExtension.Margin = new System.Windows.Forms.Padding(4);
		this.txtQueueExtension.MaxLength = 8192;
		this.txtQueueExtension.Name = "txtQueueExtension";
		this.txtQueueExtension.Size = new System.Drawing.Size(408, 22);
		this.txtQueueExtension.TabIndex = 1;
		this.txtQueueExtension.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtQueueExtension.Validating += new System.ComponentModel.CancelEventHandler(TxtQueueExtension_Validating);
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(619, 78);
		base.Controls.Add(this.editQueueExtensionButton);
		base.Controls.Add(this.txtQueueExtension);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.Controls.Add(this.lblQueueExtension);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(1359, 125);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(634, 125);
		base.Name = "TcxGetQueueStatusConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Get Queue Status";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(TcxGetQueueStatusConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(TcxGetQueueStatusConfigurationForm_HelpRequested);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
