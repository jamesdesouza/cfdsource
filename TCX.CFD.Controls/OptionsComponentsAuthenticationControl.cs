using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class OptionsComponentsAuthenticationControl : UserControl, IOptionsControl
{
	private IContainer components;

	private Label lblMaxRetryCount;

	private MaskedTextBox txtMaxRetryCount;

	private CheckBox chkIsPinRequired;

	private ErrorProvider errorProvider;

	private Label lblAuthentication;

	private void MaskedTextBox_GotFocus(object sender, EventArgs e)
	{
		(sender as MaskedTextBox).SelectAll();
	}

	private void ValidateMaxRetryCount()
	{
		if (string.IsNullOrEmpty(txtMaxRetryCount.Text))
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsAuthenticationControl.Error.MaxRetryCountIsMandatory"));
		}
		if (Convert.ToUInt32(txtMaxRetryCount.Text) == 0)
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsAuthenticationControl.Error.MaxRetryCountInvalidRange"));
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

	public OptionsComponentsAuthenticationControl()
	{
		InitializeComponent();
		txtMaxRetryCount.Text = Settings.Default.AuthenticationTemplateMaxRetryCount.ToString();
		chkIsPinRequired.Checked = Settings.Default.AuthenticationTemplateIsPinRequired;
		lblAuthentication.Text = LocalizedResourceMgr.GetString("OptionsComponentsAuthenticationControl.lblAuthentication.Text");
		lblMaxRetryCount.Text = LocalizedResourceMgr.GetString("OptionsComponentsAuthenticationControl.lblMaxRetryCount.Text");
		chkIsPinRequired.Text = LocalizedResourceMgr.GetString("OptionsComponentsAuthenticationControl.chkIsPinRequired.Text");
	}

	public void Save()
	{
		ValidateFields();
		Settings.Default.AuthenticationTemplateMaxRetryCount = Convert.ToUInt32(txtMaxRetryCount.Text);
		Settings.Default.AuthenticationTemplateIsPinRequired = chkIsPinRequired.Checked;
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
		this.chkIsPinRequired = new System.Windows.Forms.CheckBox();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.lblAuthentication = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.lblMaxRetryCount.AutoSize = true;
		this.lblMaxRetryCount.Location = new System.Drawing.Point(9, 38);
		this.lblMaxRetryCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblMaxRetryCount.Name = "lblMaxRetryCount";
		this.lblMaxRetryCount.Size = new System.Drawing.Size(204, 17);
		this.lblMaxRetryCount.TabIndex = 1;
		this.lblMaxRetryCount.Text = "Max Retry Count for ID and PIN";
		this.txtMaxRetryCount.HidePromptOnLeave = true;
		this.txtMaxRetryCount.Location = new System.Drawing.Point(12, 59);
		this.txtMaxRetryCount.Margin = new System.Windows.Forms.Padding(4);
		this.txtMaxRetryCount.Mask = "99";
		this.txtMaxRetryCount.Name = "txtMaxRetryCount";
		this.txtMaxRetryCount.Size = new System.Drawing.Size(300, 22);
		this.txtMaxRetryCount.TabIndex = 2;
		this.txtMaxRetryCount.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtMaxRetryCount.Validating += new System.ComponentModel.CancelEventHandler(TxtMaxRetryCount_Validating);
		this.chkIsPinRequired.AutoSize = true;
		this.chkIsPinRequired.Location = new System.Drawing.Point(12, 89);
		this.chkIsPinRequired.Margin = new System.Windows.Forms.Padding(4);
		this.chkIsPinRequired.Name = "chkIsPinRequired";
		this.chkIsPinRequired.Size = new System.Drawing.Size(109, 21);
		this.chkIsPinRequired.TabIndex = 3;
		this.chkIsPinRequired.Text = "Request PIN";
		this.chkIsPinRequired.UseVisualStyleBackColor = true;
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.lblAuthentication.AutoSize = true;
		this.lblAuthentication.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAuthentication.Location = new System.Drawing.Point(8, 8);
		this.lblAuthentication.Name = "lblAuthentication";
		this.lblAuthentication.Size = new System.Drawing.Size(129, 20);
		this.lblAuthentication.TabIndex = 0;
		this.lblAuthentication.Text = "Authentication";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.lblAuthentication);
		base.Controls.Add(this.lblMaxRetryCount);
		base.Controls.Add(this.txtMaxRetryCount);
		base.Controls.Add(this.chkIsPinRequired);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "OptionsComponentsAuthenticationControl";
		base.Size = new System.Drawing.Size(780, 665);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
