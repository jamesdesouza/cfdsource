using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;
using TCX.CFD.Controls;
using TCX.CFD.Properties;

namespace TCX.CFD.Forms;

public class CreditCardConfigurationForm : Form
{
	private CreditCardComponent creditCardComponent;

	private IContainer components;

	private UserInputConfigurationControl requestNumberUserInputConfigurationControl;

	private Button cancelButton;

	private Button okButton;

	private Label lblMaxRetryCount;

	private MaskedTextBox txtMaxRetryCount;

	private GroupBox grpBoxRequestNumber;

	private GroupBox grpBoxRequestExpiration;

	private UserInputConfigurationControl requestExpirationUserInputConfigurationControl;

	private CheckBox chkIsExpirationRequired;

	private ErrorProvider errorProvider;

	private GroupBox grpBoxRequestSecurityCode;

	private CheckBox chkIsSecurityCodeRequired;

	private UserInputConfigurationControl requestSecurityCodeUserInputConfigurationControl;

	public uint MaxRetryCount
	{
		get
		{
			if (!string.IsNullOrEmpty(txtMaxRetryCount.Text))
			{
				return Convert.ToUInt32(txtMaxRetryCount.Text);
			}
			return Settings.Default.CreditCardTemplateMaxRetryCount;
		}
	}

	public bool IsExpirationRequired => chkIsExpirationRequired.Checked;

	public bool IsSecurityCodeRequired => chkIsSecurityCodeRequired.Checked;

	public UserInputComponent RequestNumber => new UserInputComponent
	{
		AcceptDtmfInput = requestNumberUserInputConfigurationControl.AcceptDtmfInput,
		InitialPrompts = requestNumberUserInputConfigurationControl.InitialPrompts,
		SubsequentPrompts = requestNumberUserInputConfigurationControl.SubsequentPrompts,
		TimeoutPrompts = requestNumberUserInputConfigurationControl.TimeoutPrompts,
		InvalidDigitPrompts = requestNumberUserInputConfigurationControl.InvalidDigitPrompts,
		MaxRetryCount = requestNumberUserInputConfigurationControl.MaxRetryCount,
		FirstDigitTimeout = requestNumberUserInputConfigurationControl.FirstDigitTimeout,
		InterDigitTimeout = requestNumberUserInputConfigurationControl.InterDigitTimeout,
		FinalDigitTimeout = requestNumberUserInputConfigurationControl.FinalDigitTimeout,
		MinDigits = requestNumberUserInputConfigurationControl.MinDigits,
		MaxDigits = requestNumberUserInputConfigurationControl.MaxDigits,
		StopDigit = requestNumberUserInputConfigurationControl.StopDigit,
		IsValidDigit_0 = requestNumberUserInputConfigurationControl.IsValidDigit_0,
		IsValidDigit_1 = requestNumberUserInputConfigurationControl.IsValidDigit_1,
		IsValidDigit_2 = requestNumberUserInputConfigurationControl.IsValidDigit_2,
		IsValidDigit_3 = requestNumberUserInputConfigurationControl.IsValidDigit_3,
		IsValidDigit_4 = requestNumberUserInputConfigurationControl.IsValidDigit_4,
		IsValidDigit_5 = requestNumberUserInputConfigurationControl.IsValidDigit_5,
		IsValidDigit_6 = requestNumberUserInputConfigurationControl.IsValidDigit_6,
		IsValidDigit_7 = requestNumberUserInputConfigurationControl.IsValidDigit_7,
		IsValidDigit_8 = requestNumberUserInputConfigurationControl.IsValidDigit_8,
		IsValidDigit_9 = requestNumberUserInputConfigurationControl.IsValidDigit_9,
		IsValidDigit_Star = requestNumberUserInputConfigurationControl.IsValidDigit_Star,
		IsValidDigit_Pound = requestNumberUserInputConfigurationControl.IsValidDigit_Pound
	};

	public UserInputComponent RequestExpiration => new UserInputComponent
	{
		AcceptDtmfInput = requestExpirationUserInputConfigurationControl.AcceptDtmfInput,
		InitialPrompts = requestExpirationUserInputConfigurationControl.InitialPrompts,
		SubsequentPrompts = requestExpirationUserInputConfigurationControl.SubsequentPrompts,
		TimeoutPrompts = requestExpirationUserInputConfigurationControl.TimeoutPrompts,
		InvalidDigitPrompts = requestExpirationUserInputConfigurationControl.InvalidDigitPrompts,
		MaxRetryCount = requestExpirationUserInputConfigurationControl.MaxRetryCount,
		FirstDigitTimeout = requestExpirationUserInputConfigurationControl.FirstDigitTimeout,
		InterDigitTimeout = requestExpirationUserInputConfigurationControl.InterDigitTimeout,
		FinalDigitTimeout = requestExpirationUserInputConfigurationControl.FinalDigitTimeout,
		MinDigits = requestExpirationUserInputConfigurationControl.MinDigits,
		MaxDigits = requestExpirationUserInputConfigurationControl.MaxDigits,
		StopDigit = requestExpirationUserInputConfigurationControl.StopDigit,
		IsValidDigit_0 = requestExpirationUserInputConfigurationControl.IsValidDigit_0,
		IsValidDigit_1 = requestExpirationUserInputConfigurationControl.IsValidDigit_1,
		IsValidDigit_2 = requestExpirationUserInputConfigurationControl.IsValidDigit_2,
		IsValidDigit_3 = requestExpirationUserInputConfigurationControl.IsValidDigit_3,
		IsValidDigit_4 = requestExpirationUserInputConfigurationControl.IsValidDigit_4,
		IsValidDigit_5 = requestExpirationUserInputConfigurationControl.IsValidDigit_5,
		IsValidDigit_6 = requestExpirationUserInputConfigurationControl.IsValidDigit_6,
		IsValidDigit_7 = requestExpirationUserInputConfigurationControl.IsValidDigit_7,
		IsValidDigit_8 = requestExpirationUserInputConfigurationControl.IsValidDigit_8,
		IsValidDigit_9 = requestExpirationUserInputConfigurationControl.IsValidDigit_9,
		IsValidDigit_Star = requestExpirationUserInputConfigurationControl.IsValidDigit_Star,
		IsValidDigit_Pound = requestExpirationUserInputConfigurationControl.IsValidDigit_Pound
	};

	public UserInputComponent RequestSecurityCode => new UserInputComponent
	{
		AcceptDtmfInput = requestSecurityCodeUserInputConfigurationControl.AcceptDtmfInput,
		InitialPrompts = requestSecurityCodeUserInputConfigurationControl.InitialPrompts,
		SubsequentPrompts = requestSecurityCodeUserInputConfigurationControl.SubsequentPrompts,
		TimeoutPrompts = requestSecurityCodeUserInputConfigurationControl.TimeoutPrompts,
		InvalidDigitPrompts = requestSecurityCodeUserInputConfigurationControl.InvalidDigitPrompts,
		MaxRetryCount = requestSecurityCodeUserInputConfigurationControl.MaxRetryCount,
		FirstDigitTimeout = requestSecurityCodeUserInputConfigurationControl.FirstDigitTimeout,
		InterDigitTimeout = requestSecurityCodeUserInputConfigurationControl.InterDigitTimeout,
		FinalDigitTimeout = requestSecurityCodeUserInputConfigurationControl.FinalDigitTimeout,
		MinDigits = requestSecurityCodeUserInputConfigurationControl.MinDigits,
		MaxDigits = requestSecurityCodeUserInputConfigurationControl.MaxDigits,
		StopDigit = requestSecurityCodeUserInputConfigurationControl.StopDigit,
		IsValidDigit_0 = requestSecurityCodeUserInputConfigurationControl.IsValidDigit_0,
		IsValidDigit_1 = requestSecurityCodeUserInputConfigurationControl.IsValidDigit_1,
		IsValidDigit_2 = requestSecurityCodeUserInputConfigurationControl.IsValidDigit_2,
		IsValidDigit_3 = requestSecurityCodeUserInputConfigurationControl.IsValidDigit_3,
		IsValidDigit_4 = requestSecurityCodeUserInputConfigurationControl.IsValidDigit_4,
		IsValidDigit_5 = requestSecurityCodeUserInputConfigurationControl.IsValidDigit_5,
		IsValidDigit_6 = requestSecurityCodeUserInputConfigurationControl.IsValidDigit_6,
		IsValidDigit_7 = requestSecurityCodeUserInputConfigurationControl.IsValidDigit_7,
		IsValidDigit_8 = requestSecurityCodeUserInputConfigurationControl.IsValidDigit_8,
		IsValidDigit_9 = requestSecurityCodeUserInputConfigurationControl.IsValidDigit_9,
		IsValidDigit_Star = requestSecurityCodeUserInputConfigurationControl.IsValidDigit_Star,
		IsValidDigit_Pound = requestSecurityCodeUserInputConfigurationControl.IsValidDigit_Pound
	};

	private void MaskedTextBox_GotFocus(object sender, EventArgs e)
	{
		(sender as MaskedTextBox).SelectAll();
	}

	private void TxtMaxRetryCount_Validating(object sender, CancelEventArgs e)
	{
		errorProvider.SetError(txtMaxRetryCount, string.IsNullOrEmpty(txtMaxRetryCount.Text) ? LocalizedResourceMgr.GetString("CreditCardConfigurationForm.Error.MaxRetryCountIsMandatory") : ((Convert.ToUInt32(txtMaxRetryCount.Text) == 0) ? LocalizedResourceMgr.GetString("CreditCardConfigurationForm.Error.MaxRetryCountInvalidRange") : string.Empty));
	}

	private void ChkIsExpirationRequired_CheckedChanged(object sender, EventArgs e)
	{
		grpBoxRequestExpiration.Enabled = chkIsExpirationRequired.Checked;
		chkIsSecurityCodeRequired.Enabled = chkIsExpirationRequired.Checked;
		grpBoxRequestSecurityCode.Enabled = chkIsExpirationRequired.Checked && chkIsSecurityCodeRequired.Checked;
	}

	private void ChkIsSecurityCodeRequired_CheckedChanged(object sender, EventArgs e)
	{
		grpBoxRequestSecurityCode.Enabled = chkIsExpirationRequired.Checked && chkIsSecurityCodeRequired.Checked;
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(txtMaxRetryCount.Text))
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("CreditCardConfigurationForm.Error.MaxRetryCountIsMandatory"), LocalizedResourceMgr.GetString("CreditCardConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			txtMaxRetryCount.Focus();
		}
		else if (Convert.ToUInt32(txtMaxRetryCount.Text) == 0)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("CreditCardConfigurationForm.Error.MaxRetryCountInvalidRange"), LocalizedResourceMgr.GetString("CreditCardConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			txtMaxRetryCount.Focus();
		}
		else if (requestNumberUserInputConfigurationControl.ValidateFieldsOnSave() && (!grpBoxRequestExpiration.Enabled || requestExpirationUserInputConfigurationControl.ValidateFieldsOnSave()) && (!grpBoxRequestSecurityCode.Enabled || requestSecurityCodeUserInputConfigurationControl.ValidateFieldsOnSave()))
		{
			base.DialogResult = DialogResult.OK;
			Close();
		}
	}

	private void CancelButton_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.Cancel;
		Close();
	}

	public CreditCardConfigurationForm(CreditCardComponent creditCardComponent)
	{
		InitializeComponent();
		this.creditCardComponent = creditCardComponent;
		requestNumberUserInputConfigurationControl.SetUserInputComponent(creditCardComponent, creditCardComponent.RequestNumber, 435);
		requestExpirationUserInputConfigurationControl.SetUserInputComponent(creditCardComponent, creditCardComponent.RequestExpiration, 0);
		requestSecurityCodeUserInputConfigurationControl.SetUserInputComponent(creditCardComponent, creditCardComponent.RequestSecurityCode, 0);
		txtMaxRetryCount.Text = Convert.ToString(creditCardComponent.MaxRetryCount);
		chkIsExpirationRequired.Checked = creditCardComponent.IsExpirationRequired;
		chkIsSecurityCodeRequired.Checked = creditCardComponent.IsSecurityCodeRequired;
		ChkIsExpirationRequired_CheckedChanged(chkIsExpirationRequired, EventArgs.Empty);
		ChkIsSecurityCodeRequired_CheckedChanged(chkIsSecurityCodeRequired, EventArgs.Empty);
		TxtMaxRetryCount_Validating(txtMaxRetryCount, new CancelEventArgs());
		Text = LocalizedResourceMgr.GetString("CreditCardConfigurationForm.Title");
		lblMaxRetryCount.Text = LocalizedResourceMgr.GetString("CreditCardConfigurationForm.lblMaxRetryCount.Text");
		grpBoxRequestNumber.Text = LocalizedResourceMgr.GetString("CreditCardConfigurationForm.grpBoxRequestNumber.Text");
		chkIsExpirationRequired.Text = LocalizedResourceMgr.GetString("CreditCardConfigurationForm.chkIsExpirationRequired.Text");
		chkIsSecurityCodeRequired.Text = LocalizedResourceMgr.GetString("CreditCardConfigurationForm.chkIsSecurityCodeRequired.Text");
		okButton.Text = LocalizedResourceMgr.GetString("CreditCardConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("CreditCardConfigurationForm.cancelButton.Text");
	}

	private void CreditCardConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		creditCardComponent.ShowHelp();
	}

	private void CreditCardConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		creditCardComponent.ShowHelp();
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
		this.cancelButton = new System.Windows.Forms.Button();
		this.okButton = new System.Windows.Forms.Button();
		this.lblMaxRetryCount = new System.Windows.Forms.Label();
		this.txtMaxRetryCount = new System.Windows.Forms.MaskedTextBox();
		this.grpBoxRequestNumber = new System.Windows.Forms.GroupBox();
		this.requestNumberUserInputConfigurationControl = new TCX.CFD.Controls.UserInputConfigurationControl();
		this.grpBoxRequestExpiration = new System.Windows.Forms.GroupBox();
		this.requestExpirationUserInputConfigurationControl = new TCX.CFD.Controls.UserInputConfigurationControl();
		this.chkIsExpirationRequired = new System.Windows.Forms.CheckBox();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.grpBoxRequestSecurityCode = new System.Windows.Forms.GroupBox();
		this.requestSecurityCodeUserInputConfigurationControl = new TCX.CFD.Controls.UserInputConfigurationControl();
		this.chkIsSecurityCodeRequired = new System.Windows.Forms.CheckBox();
		this.grpBoxRequestNumber.SuspendLayout();
		this.grpBoxRequestExpiration.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		this.grpBoxRequestSecurityCode.SuspendLayout();
		base.SuspendLayout();
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(1065, 807);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 6;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.cancelButton.Click += new System.EventHandler(CancelButton_Click);
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(957, 807);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 5;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.lblMaxRetryCount.AutoSize = true;
		this.lblMaxRetryCount.Location = new System.Drawing.Point(12, 11);
		this.lblMaxRetryCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblMaxRetryCount.Name = "lblMaxRetryCount";
		this.lblMaxRetryCount.Size = new System.Drawing.Size(377, 17);
		this.lblMaxRetryCount.TabIndex = 0;
		this.lblMaxRetryCount.Text = "Max Retry Count for Number, Expiration and Security Code";
		this.txtMaxRetryCount.HidePromptOnLeave = true;
		this.txtMaxRetryCount.Location = new System.Drawing.Point(397, 7);
		this.txtMaxRetryCount.Margin = new System.Windows.Forms.Padding(4);
		this.txtMaxRetryCount.Mask = "99";
		this.txtMaxRetryCount.Name = "txtMaxRetryCount";
		this.txtMaxRetryCount.Size = new System.Drawing.Size(133, 22);
		this.txtMaxRetryCount.TabIndex = 1;
		this.txtMaxRetryCount.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtMaxRetryCount.Validating += new System.ComponentModel.CancelEventHandler(TxtMaxRetryCount_Validating);
		this.grpBoxRequestNumber.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.grpBoxRequestNumber.Controls.Add(this.requestNumberUserInputConfigurationControl);
		this.grpBoxRequestNumber.Location = new System.Drawing.Point(16, 43);
		this.grpBoxRequestNumber.Margin = new System.Windows.Forms.Padding(4);
		this.grpBoxRequestNumber.Name = "grpBoxRequestNumber";
		this.grpBoxRequestNumber.Padding = new System.Windows.Forms.Padding(4);
		this.grpBoxRequestNumber.Size = new System.Drawing.Size(1149, 373);
		this.grpBoxRequestNumber.TabIndex = 2;
		this.grpBoxRequestNumber.TabStop = false;
		this.grpBoxRequestNumber.Text = "User Input for Credit Card Number";
		this.requestNumberUserInputConfigurationControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.requestNumberUserInputConfigurationControl.Location = new System.Drawing.Point(8, 23);
		this.requestNumberUserInputConfigurationControl.Margin = new System.Windows.Forms.Padding(5);
		this.requestNumberUserInputConfigurationControl.MinimumSize = new System.Drawing.Size(549, 346);
		this.requestNumberUserInputConfigurationControl.Name = "requestNumberUserInputConfigurationControl";
		this.requestNumberUserInputConfigurationControl.Size = new System.Drawing.Size(1131, 346);
		this.requestNumberUserInputConfigurationControl.TabIndex = 0;
		this.grpBoxRequestExpiration.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.grpBoxRequestExpiration.Controls.Add(this.requestExpirationUserInputConfigurationControl);
		this.grpBoxRequestExpiration.Location = new System.Drawing.Point(16, 423);
		this.grpBoxRequestExpiration.Margin = new System.Windows.Forms.Padding(4);
		this.grpBoxRequestExpiration.Name = "grpBoxRequestExpiration";
		this.grpBoxRequestExpiration.Padding = new System.Windows.Forms.Padding(4);
		this.grpBoxRequestExpiration.Size = new System.Drawing.Size(568, 377);
		this.grpBoxRequestExpiration.TabIndex = 3;
		this.grpBoxRequestExpiration.TabStop = false;
		this.requestExpirationUserInputConfigurationControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.requestExpirationUserInputConfigurationControl.Location = new System.Drawing.Point(8, 23);
		this.requestExpirationUserInputConfigurationControl.Margin = new System.Windows.Forms.Padding(5);
		this.requestExpirationUserInputConfigurationControl.MinimumSize = new System.Drawing.Size(549, 346);
		this.requestExpirationUserInputConfigurationControl.Name = "requestExpirationUserInputConfigurationControl";
		this.requestExpirationUserInputConfigurationControl.Size = new System.Drawing.Size(549, 348);
		this.requestExpirationUserInputConfigurationControl.TabIndex = 1;
		this.chkIsExpirationRequired.AutoSize = true;
		this.chkIsExpirationRequired.Location = new System.Drawing.Point(28, 423);
		this.chkIsExpirationRequired.Margin = new System.Windows.Forms.Padding(4);
		this.chkIsExpirationRequired.Name = "chkIsExpirationRequired";
		this.chkIsExpirationRequired.Size = new System.Drawing.Size(183, 21);
		this.chkIsExpirationRequired.TabIndex = 0;
		this.chkIsExpirationRequired.Text = "Request Expiration Date";
		this.chkIsExpirationRequired.UseVisualStyleBackColor = true;
		this.chkIsExpirationRequired.CheckedChanged += new System.EventHandler(ChkIsExpirationRequired_CheckedChanged);
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.grpBoxRequestSecurityCode.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.grpBoxRequestSecurityCode.Controls.Add(this.requestSecurityCodeUserInputConfigurationControl);
		this.grpBoxRequestSecurityCode.Location = new System.Drawing.Point(597, 423);
		this.grpBoxRequestSecurityCode.Margin = new System.Windows.Forms.Padding(4);
		this.grpBoxRequestSecurityCode.Name = "grpBoxRequestSecurityCode";
		this.grpBoxRequestSecurityCode.Padding = new System.Windows.Forms.Padding(4);
		this.grpBoxRequestSecurityCode.Size = new System.Drawing.Size(568, 377);
		this.grpBoxRequestSecurityCode.TabIndex = 4;
		this.grpBoxRequestSecurityCode.TabStop = false;
		this.requestSecurityCodeUserInputConfigurationControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.requestSecurityCodeUserInputConfigurationControl.Location = new System.Drawing.Point(8, 23);
		this.requestSecurityCodeUserInputConfigurationControl.Margin = new System.Windows.Forms.Padding(5);
		this.requestSecurityCodeUserInputConfigurationControl.MinimumSize = new System.Drawing.Size(549, 346);
		this.requestSecurityCodeUserInputConfigurationControl.Name = "requestSecurityCodeUserInputConfigurationControl";
		this.requestSecurityCodeUserInputConfigurationControl.Size = new System.Drawing.Size(549, 348);
		this.requestSecurityCodeUserInputConfigurationControl.TabIndex = 1;
		this.chkIsSecurityCodeRequired.AutoSize = true;
		this.chkIsSecurityCodeRequired.Location = new System.Drawing.Point(609, 423);
		this.chkIsSecurityCodeRequired.Margin = new System.Windows.Forms.Padding(4);
		this.chkIsSecurityCodeRequired.Name = "chkIsSecurityCodeRequired";
		this.chkIsSecurityCodeRequired.Size = new System.Drawing.Size(175, 21);
		this.chkIsSecurityCodeRequired.TabIndex = 0;
		this.chkIsSecurityCodeRequired.Text = "Request Security Code";
		this.chkIsSecurityCodeRequired.UseVisualStyleBackColor = true;
		this.chkIsSecurityCodeRequired.CheckedChanged += new System.EventHandler(ChkIsSecurityCodeRequired_CheckedChanged);
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(1181, 841);
		base.Controls.Add(this.chkIsSecurityCodeRequired);
		base.Controls.Add(this.chkIsExpirationRequired);
		base.Controls.Add(this.grpBoxRequestSecurityCode);
		base.Controls.Add(this.grpBoxRequestExpiration);
		base.Controls.Add(this.grpBoxRequestNumber);
		base.Controls.Add(this.txtMaxRetryCount);
		base.Controls.Add(this.lblMaxRetryCount);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(1199, 888);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(1199, 888);
		base.Name = "CreditCardConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Credit Card";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(CreditCardConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(CreditCardConfigurationForm_HelpRequested);
		this.grpBoxRequestNumber.ResumeLayout(false);
		this.grpBoxRequestExpiration.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		this.grpBoxRequestSecurityCode.ResumeLayout(false);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
