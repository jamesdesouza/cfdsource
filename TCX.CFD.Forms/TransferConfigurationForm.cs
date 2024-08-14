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

public class TransferConfigurationForm : Form
{
	private readonly TransferComponent transferComponent;

	private readonly List<string> validVariables;

	private IContainer components;

	private Label lblDestination;

	private TextBox txtDestination;

	private Button cancelButton;

	private Button okButton;

	private Button destinationExpressionButton;

	private ErrorProvider errorProvider;

	private CheckBox chkTransferToVoicemail;

	private MaskedTextBox txtDelayMilliseconds;

	private Label lblDelayMilliseconds;

	public string Destination => txtDestination.Text;

	public bool TransferToVoicemail => chkTransferToVoicemail.Checked;

	public uint DelayMilliseconds => Convert.ToUInt32(txtDelayMilliseconds.Text);

	public TransferConfigurationForm(TransferComponent transferComponent)
	{
		InitializeComponent();
		this.transferComponent = transferComponent;
		validVariables = ExpressionHelper.GetValidVariables(transferComponent);
		txtDestination.Text = transferComponent.Destination;
		TxtDestination_Validating(txtDestination, new CancelEventArgs());
		chkTransferToVoicemail.Checked = transferComponent.TransferToVoicemail;
		txtDelayMilliseconds.Text = transferComponent.DelayMilliseconds.ToString();
		TxtDelayMilliseconds_Validating(txtDelayMilliseconds, new CancelEventArgs());
		Text = LocalizedResourceMgr.GetString("TransferConfigurationForm.Title");
		lblDestination.Text = LocalizedResourceMgr.GetString("TransferConfigurationForm.lblDestination.Text");
		chkTransferToVoicemail.Text = LocalizedResourceMgr.GetString("TransferConfigurationForm.chkTransferToVoicemail.Text");
		lblDelayMilliseconds.Text = LocalizedResourceMgr.GetString("TransferConfigurationForm.lblDelayMilliseconds.Text");
		okButton.Text = LocalizedResourceMgr.GetString("TransferConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("TransferConfigurationForm.cancelButton.Text");
	}

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void MaskedTextBox_GotFocus(object sender, EventArgs e)
	{
		(sender as MaskedTextBox).SelectAll();
	}

	private void TxtDestination_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtDestination.Text))
		{
			errorProvider.SetError(destinationExpressionButton, LocalizedResourceMgr.GetString("TransferConfigurationForm.Error.DestinationIsMandatory"));
			return;
		}
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtDestination.Text);
		errorProvider.SetError(destinationExpressionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("TransferConfigurationForm.Error.DestinationIsInvalid"));
	}

	private void TxtDelayMilliseconds_Validating(object sender, CancelEventArgs e)
	{
		errorProvider.SetError(txtDelayMilliseconds, string.IsNullOrEmpty(txtDelayMilliseconds.Text) ? LocalizedResourceMgr.GetString("TransferConfigurationForm.Error.DelayIsMandatory") : ((Convert.ToUInt32(txtDelayMilliseconds.Text) > 2000) ? LocalizedResourceMgr.GetString("TransferConfigurationForm.Error.DelayInvalidRange") : string.Empty));
	}

	private void DestinationExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(transferComponent);
		expressionEditorForm.Expression = txtDestination.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtDestination.Text = expressionEditorForm.Expression;
			TxtDestination_Validating(txtDestination, new CancelEventArgs());
		}
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(txtDelayMilliseconds.Text))
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("TransferConfigurationForm.Error.DelayIsMandatory"), LocalizedResourceMgr.GetString("TransferConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			txtDelayMilliseconds.Focus();
		}
		else if (Convert.ToUInt32(txtDelayMilliseconds.Text) > 2000)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("TransferConfigurationForm.Error.DelayInvalidRange"), LocalizedResourceMgr.GetString("TransferConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			txtDelayMilliseconds.Focus();
		}
		else
		{
			base.DialogResult = DialogResult.OK;
			Close();
		}
	}

	private void TransferConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		transferComponent.ShowHelp();
	}

	private void TransferConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		transferComponent.ShowHelp();
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
		this.lblDestination = new System.Windows.Forms.Label();
		this.txtDestination = new System.Windows.Forms.TextBox();
		this.cancelButton = new System.Windows.Forms.Button();
		this.okButton = new System.Windows.Forms.Button();
		this.destinationExpressionButton = new System.Windows.Forms.Button();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.chkTransferToVoicemail = new System.Windows.Forms.CheckBox();
		this.txtDelayMilliseconds = new System.Windows.Forms.MaskedTextBox();
		this.lblDelayMilliseconds = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.lblDestination.AutoSize = true;
		this.lblDestination.Location = new System.Drawing.Point(16, 11);
		this.lblDestination.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblDestination.Name = "lblDestination";
		this.lblDestination.Size = new System.Drawing.Size(110, 17);
		this.lblDestination.TabIndex = 0;
		this.lblDestination.Text = "Transfer Call To";
		this.txtDestination.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtDestination.Location = new System.Drawing.Point(158, 7);
		this.txtDestination.Margin = new System.Windows.Forms.Padding(4);
		this.txtDestination.Name = "txtDestination";
		this.txtDestination.Size = new System.Drawing.Size(354, 22);
		this.txtDestination.TabIndex = 1;
		this.txtDestination.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtDestination.Validating += new System.ComponentModel.CancelEventHandler(TxtDestination_Validating);
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(412, 103);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 7;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(304, 103);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 6;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.destinationExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.destinationExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.destinationExpressionButton.Location = new System.Drawing.Point(522, 5);
		this.destinationExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.destinationExpressionButton.Name = "destinationExpressionButton";
		this.destinationExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.destinationExpressionButton.TabIndex = 2;
		this.destinationExpressionButton.UseVisualStyleBackColor = true;
		this.destinationExpressionButton.Click += new System.EventHandler(DestinationExpressionButton_Click);
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.chkTransferToVoicemail.AutoSize = true;
		this.chkTransferToVoicemail.Location = new System.Drawing.Point(158, 37);
		this.chkTransferToVoicemail.Name = "chkTransferToVoicemail";
		this.chkTransferToVoicemail.Size = new System.Drawing.Size(229, 21);
		this.chkTransferToVoicemail.TabIndex = 3;
		this.chkTransferToVoicemail.Text = "Transfer to Extension Voicemail";
		this.chkTransferToVoicemail.UseVisualStyleBackColor = true;
		this.txtDelayMilliseconds.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtDelayMilliseconds.Location = new System.Drawing.Point(158, 65);
		this.txtDelayMilliseconds.Margin = new System.Windows.Forms.Padding(4);
		this.txtDelayMilliseconds.Mask = "9999";
		this.txtDelayMilliseconds.Name = "txtDelayMilliseconds";
		this.txtDelayMilliseconds.Size = new System.Drawing.Size(354, 22);
		this.txtDelayMilliseconds.TabIndex = 5;
		this.txtDelayMilliseconds.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtDelayMilliseconds.Validating += new System.ComponentModel.CancelEventHandler(TxtDelayMilliseconds_Validating);
		this.lblDelayMilliseconds.AutoSize = true;
		this.lblDelayMilliseconds.Location = new System.Drawing.Point(16, 68);
		this.lblDelayMilliseconds.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblDelayMilliseconds.Name = "lblDelayMilliseconds";
		this.lblDelayMilliseconds.Size = new System.Drawing.Size(134, 17);
		this.lblDelayMilliseconds.TabIndex = 4;
		this.lblDelayMilliseconds.Text = "Delay (milliseconds)";
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(582, 144);
		base.Controls.Add(this.txtDelayMilliseconds);
		base.Controls.Add(this.lblDelayMilliseconds);
		base.Controls.Add(this.chkTransferToVoicemail);
		base.Controls.Add(this.destinationExpressionButton);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.Controls.Add(this.txtDestination);
		base.Controls.Add(this.lblDestination);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(1200, 191);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(600, 191);
		base.Name = "TransferConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Transfer";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(TransferConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(TransferConfigurationForm_HelpRequested);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
