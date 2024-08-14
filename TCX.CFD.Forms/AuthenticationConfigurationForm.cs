using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;
using TCX.CFD.Controls;
using TCX.CFD.Properties;

namespace TCX.CFD.Forms;

public class AuthenticationConfigurationForm : Form
{
	private AuthenticationComponent authenticationComponent;

	private IContainer components;

	private UserInputConfigurationControl requestIdUserInputConfigurationControl;

	private Button cancelButton;

	private Button okButton;

	private Label lblMaxRetryCount;

	private MaskedTextBox txtMaxRetryCount;

	private GroupBox grpBoxRequestID;

	private GroupBox grpBoxRequestPIN;

	private UserInputConfigurationControl requestPinUserInputConfigurationControl;

	private CheckBox chkIsPinRequired;

	private ErrorProvider errorProvider;

	public uint MaxRetryCount
	{
		get
		{
			if (!string.IsNullOrEmpty(txtMaxRetryCount.Text))
			{
				return Convert.ToUInt32(txtMaxRetryCount.Text);
			}
			return Settings.Default.AuthenticationTemplateMaxRetryCount;
		}
	}

	public bool IsPinRequired => chkIsPinRequired.Checked;

	public UserInputComponent RequestID => new UserInputComponent
	{
		AcceptDtmfInput = requestIdUserInputConfigurationControl.AcceptDtmfInput,
		InitialPrompts = requestIdUserInputConfigurationControl.InitialPrompts,
		SubsequentPrompts = requestIdUserInputConfigurationControl.SubsequentPrompts,
		TimeoutPrompts = requestIdUserInputConfigurationControl.TimeoutPrompts,
		InvalidDigitPrompts = requestIdUserInputConfigurationControl.InvalidDigitPrompts,
		MaxRetryCount = requestIdUserInputConfigurationControl.MaxRetryCount,
		FirstDigitTimeout = requestIdUserInputConfigurationControl.FirstDigitTimeout,
		InterDigitTimeout = requestIdUserInputConfigurationControl.InterDigitTimeout,
		FinalDigitTimeout = requestIdUserInputConfigurationControl.FinalDigitTimeout,
		MinDigits = requestIdUserInputConfigurationControl.MinDigits,
		MaxDigits = requestIdUserInputConfigurationControl.MaxDigits,
		StopDigit = requestIdUserInputConfigurationControl.StopDigit,
		IsValidDigit_0 = requestIdUserInputConfigurationControl.IsValidDigit_0,
		IsValidDigit_1 = requestIdUserInputConfigurationControl.IsValidDigit_1,
		IsValidDigit_2 = requestIdUserInputConfigurationControl.IsValidDigit_2,
		IsValidDigit_3 = requestIdUserInputConfigurationControl.IsValidDigit_3,
		IsValidDigit_4 = requestIdUserInputConfigurationControl.IsValidDigit_4,
		IsValidDigit_5 = requestIdUserInputConfigurationControl.IsValidDigit_5,
		IsValidDigit_6 = requestIdUserInputConfigurationControl.IsValidDigit_6,
		IsValidDigit_7 = requestIdUserInputConfigurationControl.IsValidDigit_7,
		IsValidDigit_8 = requestIdUserInputConfigurationControl.IsValidDigit_8,
		IsValidDigit_9 = requestIdUserInputConfigurationControl.IsValidDigit_9,
		IsValidDigit_Star = requestIdUserInputConfigurationControl.IsValidDigit_Star,
		IsValidDigit_Pound = requestIdUserInputConfigurationControl.IsValidDigit_Pound
	};

	public UserInputComponent RequestPIN => new UserInputComponent
	{
		AcceptDtmfInput = requestPinUserInputConfigurationControl.AcceptDtmfInput,
		InitialPrompts = requestPinUserInputConfigurationControl.InitialPrompts,
		SubsequentPrompts = requestPinUserInputConfigurationControl.SubsequentPrompts,
		TimeoutPrompts = requestPinUserInputConfigurationControl.TimeoutPrompts,
		InvalidDigitPrompts = requestPinUserInputConfigurationControl.InvalidDigitPrompts,
		MaxRetryCount = requestPinUserInputConfigurationControl.MaxRetryCount,
		FirstDigitTimeout = requestPinUserInputConfigurationControl.FirstDigitTimeout,
		InterDigitTimeout = requestPinUserInputConfigurationControl.InterDigitTimeout,
		FinalDigitTimeout = requestPinUserInputConfigurationControl.FinalDigitTimeout,
		MinDigits = requestPinUserInputConfigurationControl.MinDigits,
		MaxDigits = requestPinUserInputConfigurationControl.MaxDigits,
		StopDigit = requestPinUserInputConfigurationControl.StopDigit,
		IsValidDigit_0 = requestPinUserInputConfigurationControl.IsValidDigit_0,
		IsValidDigit_1 = requestPinUserInputConfigurationControl.IsValidDigit_1,
		IsValidDigit_2 = requestPinUserInputConfigurationControl.IsValidDigit_2,
		IsValidDigit_3 = requestPinUserInputConfigurationControl.IsValidDigit_3,
		IsValidDigit_4 = requestPinUserInputConfigurationControl.IsValidDigit_4,
		IsValidDigit_5 = requestPinUserInputConfigurationControl.IsValidDigit_5,
		IsValidDigit_6 = requestPinUserInputConfigurationControl.IsValidDigit_6,
		IsValidDigit_7 = requestPinUserInputConfigurationControl.IsValidDigit_7,
		IsValidDigit_8 = requestPinUserInputConfigurationControl.IsValidDigit_8,
		IsValidDigit_9 = requestPinUserInputConfigurationControl.IsValidDigit_9,
		IsValidDigit_Star = requestPinUserInputConfigurationControl.IsValidDigit_Star,
		IsValidDigit_Pound = requestPinUserInputConfigurationControl.IsValidDigit_Pound
	};

	private void MaskedTextBox_GotFocus(object sender, EventArgs e)
	{
		(sender as MaskedTextBox).SelectAll();
	}

	private void TxtMaxRetryCount_Validating(object sender, CancelEventArgs e)
	{
		errorProvider.SetError(txtMaxRetryCount, string.IsNullOrEmpty(txtMaxRetryCount.Text) ? LocalizedResourceMgr.GetString("AuthenticationConfigurationForm.Error.MaxRetryCountIsMandatory") : ((Convert.ToUInt32(txtMaxRetryCount.Text) == 0) ? LocalizedResourceMgr.GetString("AuthenticationConfigurationForm.Error.MaxRetryCountInvalidRange") : string.Empty));
	}

	private void ChkIsPinRequired_CheckedChanged(object sender, EventArgs e)
	{
		grpBoxRequestPIN.Enabled = chkIsPinRequired.Checked;
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(txtMaxRetryCount.Text))
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("AuthenticationConfigurationForm.Error.MaxRetryCountIsMandatory"), LocalizedResourceMgr.GetString("AuthenticationConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			txtMaxRetryCount.Focus();
		}
		else if (Convert.ToUInt32(txtMaxRetryCount.Text) == 0)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("AuthenticationConfigurationForm.Error.MaxRetryCountInvalidRange"), LocalizedResourceMgr.GetString("AuthenticationConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			txtMaxRetryCount.Focus();
		}
		else if (requestIdUserInputConfigurationControl.ValidateFieldsOnSave() && (!chkIsPinRequired.Checked || requestPinUserInputConfigurationControl.ValidateFieldsOnSave()))
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

	public AuthenticationConfigurationForm(AuthenticationComponent authenticationComponent)
	{
		InitializeComponent();
		this.authenticationComponent = authenticationComponent;
		requestIdUserInputConfigurationControl.SetUserInputComponent(authenticationComponent, authenticationComponent.RequestID, 0);
		requestPinUserInputConfigurationControl.SetUserInputComponent(authenticationComponent, authenticationComponent.RequestPIN, 0);
		txtMaxRetryCount.Text = Convert.ToString(authenticationComponent.MaxRetryCount);
		chkIsPinRequired.Checked = authenticationComponent.IsPinRequired;
		ChkIsPinRequired_CheckedChanged(chkIsPinRequired, EventArgs.Empty);
		TxtMaxRetryCount_Validating(txtMaxRetryCount, new CancelEventArgs());
		Text = LocalizedResourceMgr.GetString("AuthenticationConfigurationForm.Title");
		lblMaxRetryCount.Text = LocalizedResourceMgr.GetString("AuthenticationConfigurationForm.lblMaxRetryCount.Text");
		chkIsPinRequired.Text = LocalizedResourceMgr.GetString("AuthenticationConfigurationForm.chkIsPinRequired.Text");
		grpBoxRequestID.Text = LocalizedResourceMgr.GetString("AuthenticationConfigurationForm.grpBoxRequestID.Text");
		grpBoxRequestPIN.Text = LocalizedResourceMgr.GetString("AuthenticationConfigurationForm.grpBoxRequestPIN.Text");
		okButton.Text = LocalizedResourceMgr.GetString("AuthenticationConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("AuthenticationConfigurationForm.cancelButton.Text");
	}

	private void AuthenticationConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		authenticationComponent.ShowHelp();
	}

	private void AuthenticationConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		authenticationComponent.ShowHelp();
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
		this.grpBoxRequestID = new System.Windows.Forms.GroupBox();
		this.requestIdUserInputConfigurationControl = new TCX.CFD.Controls.UserInputConfigurationControl();
		this.grpBoxRequestPIN = new System.Windows.Forms.GroupBox();
		this.requestPinUserInputConfigurationControl = new TCX.CFD.Controls.UserInputConfigurationControl();
		this.chkIsPinRequired = new System.Windows.Forms.CheckBox();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.grpBoxRequestID.SuspendLayout();
		this.grpBoxRequestPIN.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(1063, 429);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 6;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.cancelButton.Click += new System.EventHandler(CancelButton_Click);
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(955, 429);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 5;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.lblMaxRetryCount.AutoSize = true;
		this.lblMaxRetryCount.Location = new System.Drawing.Point(17, 16);
		this.lblMaxRetryCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblMaxRetryCount.Name = "lblMaxRetryCount";
		this.lblMaxRetryCount.Size = new System.Drawing.Size(204, 17);
		this.lblMaxRetryCount.TabIndex = 0;
		this.lblMaxRetryCount.Text = "Max Retry Count for ID and PIN";
		this.txtMaxRetryCount.HidePromptOnLeave = true;
		this.txtMaxRetryCount.Location = new System.Drawing.Point(235, 12);
		this.txtMaxRetryCount.Margin = new System.Windows.Forms.Padding(4);
		this.txtMaxRetryCount.Mask = "99";
		this.txtMaxRetryCount.Name = "txtMaxRetryCount";
		this.txtMaxRetryCount.Size = new System.Drawing.Size(133, 22);
		this.txtMaxRetryCount.TabIndex = 1;
		this.txtMaxRetryCount.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtMaxRetryCount.Validating += new System.ComponentModel.CancelEventHandler(TxtMaxRetryCount_Validating);
		this.grpBoxRequestID.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.grpBoxRequestID.Controls.Add(this.requestIdUserInputConfigurationControl);
		this.grpBoxRequestID.Location = new System.Drawing.Point(21, 46);
		this.grpBoxRequestID.Margin = new System.Windows.Forms.Padding(4);
		this.grpBoxRequestID.Name = "grpBoxRequestID";
		this.grpBoxRequestID.Padding = new System.Windows.Forms.Padding(4);
		this.grpBoxRequestID.Size = new System.Drawing.Size(568, 376);
		this.grpBoxRequestID.TabIndex = 2;
		this.grpBoxRequestID.TabStop = false;
		this.grpBoxRequestID.Text = "User Input for ID";
		this.requestIdUserInputConfigurationControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.requestIdUserInputConfigurationControl.Location = new System.Drawing.Point(8, 23);
		this.requestIdUserInputConfigurationControl.Margin = new System.Windows.Forms.Padding(5);
		this.requestIdUserInputConfigurationControl.MinimumSize = new System.Drawing.Size(549, 346);
		this.requestIdUserInputConfigurationControl.Name = "requestIdUserInputConfigurationControl";
		this.requestIdUserInputConfigurationControl.Size = new System.Drawing.Size(549, 346);
		this.requestIdUserInputConfigurationControl.TabIndex = 0;
		this.grpBoxRequestPIN.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.grpBoxRequestPIN.Controls.Add(this.requestPinUserInputConfigurationControl);
		this.grpBoxRequestPIN.Location = new System.Drawing.Point(597, 46);
		this.grpBoxRequestPIN.Margin = new System.Windows.Forms.Padding(4);
		this.grpBoxRequestPIN.Name = "grpBoxRequestPIN";
		this.grpBoxRequestPIN.Padding = new System.Windows.Forms.Padding(4);
		this.grpBoxRequestPIN.Size = new System.Drawing.Size(566, 376);
		this.grpBoxRequestPIN.TabIndex = 4;
		this.grpBoxRequestPIN.TabStop = false;
		this.grpBoxRequestPIN.Text = "User Input for PIN";
		this.requestPinUserInputConfigurationControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.requestPinUserInputConfigurationControl.Location = new System.Drawing.Point(8, 23);
		this.requestPinUserInputConfigurationControl.Margin = new System.Windows.Forms.Padding(5);
		this.requestPinUserInputConfigurationControl.MinimumSize = new System.Drawing.Size(549, 346);
		this.requestPinUserInputConfigurationControl.Name = "requestPinUserInputConfigurationControl";
		this.requestPinUserInputConfigurationControl.Size = new System.Drawing.Size(549, 346);
		this.requestPinUserInputConfigurationControl.TabIndex = 0;
		this.chkIsPinRequired.AutoSize = true;
		this.chkIsPinRequired.Location = new System.Drawing.Point(597, 15);
		this.chkIsPinRequired.Margin = new System.Windows.Forms.Padding(4);
		this.chkIsPinRequired.Name = "chkIsPinRequired";
		this.chkIsPinRequired.Size = new System.Drawing.Size(109, 21);
		this.chkIsPinRequired.TabIndex = 3;
		this.chkIsPinRequired.Text = "Request PIN";
		this.chkIsPinRequired.UseVisualStyleBackColor = true;
		this.chkIsPinRequired.CheckedChanged += new System.EventHandler(ChkIsPinRequired_CheckedChanged);
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(1179, 472);
		base.Controls.Add(this.chkIsPinRequired);
		base.Controls.Add(this.grpBoxRequestPIN);
		base.Controls.Add(this.grpBoxRequestID);
		base.Controls.Add(this.txtMaxRetryCount);
		base.Controls.Add(this.lblMaxRetryCount);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(1197, 507);
		base.Name = "AuthenticationConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Authentication";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(AuthenticationConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(AuthenticationConfigurationForm_HelpRequested);
		this.grpBoxRequestID.ResumeLayout(false);
		this.grpBoxRequestPIN.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
