using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class OptionsComponentsCreditCardControl : UserControl, IOptionsControl
{
	private IContainer components;

	private Label lblMaxRetryCount;

	private MaskedTextBox txtMaxRetryCount;

	private CheckBox chkIsExpirationRequired;

	private ErrorProvider errorProvider;

	private CheckBox chkIsSecurityCodeRequired;

	private Label lblCreditCard;

	private void MaskedTextBox_GotFocus(object sender, EventArgs e)
	{
		(sender as MaskedTextBox).SelectAll();
	}

	private void ValidateMaxRetryCount()
	{
		if (string.IsNullOrEmpty(txtMaxRetryCount.Text))
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsCreditCardControl.Error.MaxRetryCountIsMandatory"));
		}
		if (Convert.ToUInt32(txtMaxRetryCount.Text) == 0)
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsCreditCardControl.Error.MaxRetryCountInvalidRange"));
		}
	}

	private void ValidateFields()
	{
		ValidateMaxRetryCount();
	}

	private void TxtMaxRetryCount_Validating(object sender, CancelEventArgs e)
	{
		try
		{
			ValidateMaxRetryCount();
			errorProvider.SetError(txtMaxRetryCount, string.Empty);
		}
		catch (Exception exc)
		{
			errorProvider.SetError(txtMaxRetryCount, ErrorHelper.GetErrorDescription(exc));
		}
	}

	public OptionsComponentsCreditCardControl()
	{
		InitializeComponent();
		txtMaxRetryCount.Text = Settings.Default.CreditCardTemplateMaxRetryCount.ToString();
		chkIsExpirationRequired.Checked = Settings.Default.CreditCardTemplateIsExpirationRequired;
		chkIsSecurityCodeRequired.Checked = Settings.Default.CreditCardTemplateIsSecurityCodeRequired;
		lblCreditCard.Text = LocalizedResourceMgr.GetString("OptionsComponentsCreditCardControl.lblCreditCard.Text");
		lblMaxRetryCount.Text = LocalizedResourceMgr.GetString("OptionsComponentsCreditCardControl.lblMaxRetryCount.Text");
		chkIsExpirationRequired.Text = LocalizedResourceMgr.GetString("OptionsComponentsCreditCardControl.chkIsExpirationRequired.Text");
		chkIsSecurityCodeRequired.Text = LocalizedResourceMgr.GetString("OptionsComponentsCreditCardControl.chkIsSecurityCodeRequired.Text");
	}

	public void Save()
	{
		ValidateFields();
		Settings.Default.CreditCardTemplateMaxRetryCount = Convert.ToUInt32(txtMaxRetryCount.Text);
		Settings.Default.CreditCardTemplateIsExpirationRequired = chkIsExpirationRequired.Checked;
		Settings.Default.CreditCardTemplateIsSecurityCodeRequired = chkIsSecurityCodeRequired.Checked;
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
		this.lblMaxRetryCount = new System.Windows.Forms.Label();
		this.txtMaxRetryCount = new System.Windows.Forms.MaskedTextBox();
		this.chkIsExpirationRequired = new System.Windows.Forms.CheckBox();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.chkIsSecurityCodeRequired = new System.Windows.Forms.CheckBox();
		this.lblCreditCard = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.lblMaxRetryCount.AutoSize = true;
		this.lblMaxRetryCount.Location = new System.Drawing.Point(9, 38);
		this.lblMaxRetryCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblMaxRetryCount.Name = "lblMaxRetryCount";
		this.lblMaxRetryCount.Size = new System.Drawing.Size(377, 17);
		this.lblMaxRetryCount.TabIndex = 1;
		this.lblMaxRetryCount.Text = "Max Retry Count for Number, Expiration and Security Code";
		this.txtMaxRetryCount.HidePromptOnLeave = true;
		this.txtMaxRetryCount.Location = new System.Drawing.Point(12, 59);
		this.txtMaxRetryCount.Margin = new System.Windows.Forms.Padding(4);
		this.txtMaxRetryCount.Mask = "99";
		this.txtMaxRetryCount.Name = "txtMaxRetryCount";
		this.txtMaxRetryCount.Size = new System.Drawing.Size(300, 22);
		this.txtMaxRetryCount.TabIndex = 2;
		this.txtMaxRetryCount.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtMaxRetryCount.Validating += new System.ComponentModel.CancelEventHandler(TxtMaxRetryCount_Validating);
		this.chkIsExpirationRequired.AutoSize = true;
		this.chkIsExpirationRequired.Location = new System.Drawing.Point(12, 89);
		this.chkIsExpirationRequired.Margin = new System.Windows.Forms.Padding(4);
		this.chkIsExpirationRequired.Name = "chkIsExpirationRequired";
		this.chkIsExpirationRequired.Size = new System.Drawing.Size(183, 21);
		this.chkIsExpirationRequired.TabIndex = 3;
		this.chkIsExpirationRequired.Text = "Request Expiration Date";
		this.chkIsExpirationRequired.UseVisualStyleBackColor = true;
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.chkIsSecurityCodeRequired.AutoSize = true;
		this.chkIsSecurityCodeRequired.Location = new System.Drawing.Point(12, 118);
		this.chkIsSecurityCodeRequired.Margin = new System.Windows.Forms.Padding(4);
		this.chkIsSecurityCodeRequired.Name = "chkIsSecurityCodeRequired";
		this.chkIsSecurityCodeRequired.Size = new System.Drawing.Size(175, 21);
		this.chkIsSecurityCodeRequired.TabIndex = 4;
		this.chkIsSecurityCodeRequired.Text = "Request Security Code";
		this.chkIsSecurityCodeRequired.UseVisualStyleBackColor = true;
		this.lblCreditCard.AutoSize = true;
		this.lblCreditCard.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCreditCard.Location = new System.Drawing.Point(8, 8);
		this.lblCreditCard.Name = "lblCreditCard";
		this.lblCreditCard.Size = new System.Drawing.Size(106, 20);
		this.lblCreditCard.TabIndex = 0;
		this.lblCreditCard.Text = "Credit Card";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.lblCreditCard);
		base.Controls.Add(this.chkIsSecurityCodeRequired);
		base.Controls.Add(this.lblMaxRetryCount);
		base.Controls.Add(this.txtMaxRetryCount);
		base.Controls.Add(this.chkIsExpirationRequired);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "OptionsComponentsCreditCardControl";
		base.Size = new System.Drawing.Size(780, 665);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
