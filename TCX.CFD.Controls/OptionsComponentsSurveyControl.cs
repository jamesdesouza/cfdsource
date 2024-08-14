using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class OptionsComponentsSurveyControl : UserControl, IOptionsControl
{
	private IContainer components;

	private Label lblMaxRetryCount;

	private MaskedTextBox txtMaxRetryCount;

	private Label lblTimeout;

	private MaskedTextBox txtTimeout;

	private CheckBox chkAcceptDtmfInput;

	private ErrorProvider errorProvider;

	private Label lblSurvey;

	private void MaskedTextBox_GotFocus(object sender, EventArgs e)
	{
		(sender as MaskedTextBox).SelectAll();
	}

	private void ValidateTimeout()
	{
		if (string.IsNullOrEmpty(txtTimeout.Text))
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsSurveyControl.Error.TimeoutIsMandatory"));
		}
		if (Convert.ToUInt32(txtTimeout.Text) == 0)
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsSurveyControl.Error.TimeoutInvalidRange"));
		}
	}

	private void ValidateMaxRetryCount()
	{
		if (string.IsNullOrEmpty(txtMaxRetryCount.Text))
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsSurveyControl.Error.MaxRetryCountIsMandatory"));
		}
		if (Convert.ToUInt32(txtMaxRetryCount.Text) == 0)
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsSurveyControl.Error.MaxRetryCountInvalidRange"));
		}
	}

	private void ValidateFields()
	{
		ValidateTimeout();
		ValidateMaxRetryCount();
	}

	private void TxtTimeout_Validating(object sender, CancelEventArgs e)
	{
		try
		{
			ValidateTimeout();
			errorProvider.SetError(txtTimeout, string.Empty);
		}
		catch (Exception exc)
		{
			errorProvider.SetError(txtTimeout, ErrorHelper.GetErrorDescription(exc));
		}
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

	public OptionsComponentsSurveyControl()
	{
		InitializeComponent();
		chkAcceptDtmfInput.Checked = Settings.Default.SurveyTemplateAcceptDtmfInput;
		txtTimeout.Text = Settings.Default.SurveyTemplateTimeout.ToString();
		txtMaxRetryCount.Text = Settings.Default.SurveyTemplateMaxRetryCount.ToString();
		lblSurvey.Text = LocalizedResourceMgr.GetString("OptionsComponentsSurveyControl.lblSurvey.Text");
		chkAcceptDtmfInput.Text = LocalizedResourceMgr.GetString("OptionsComponentsSurveyControl.chkAcceptDtmfInput.Text");
		lblTimeout.Text = LocalizedResourceMgr.GetString("OptionsComponentsSurveyControl.lblTimeout.Text");
		lblMaxRetryCount.Text = LocalizedResourceMgr.GetString("OptionsComponentsSurveyControl.lblMaxRetryCount.Text");
	}

	public void Save()
	{
		ValidateFields();
		Settings.Default.SurveyTemplateAcceptDtmfInput = chkAcceptDtmfInput.Checked;
		Settings.Default.SurveyTemplateTimeout = Convert.ToUInt32(txtTimeout.Text);
		Settings.Default.SurveyTemplateMaxRetryCount = Convert.ToUInt32(txtMaxRetryCount.Text);
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
		this.lblTimeout = new System.Windows.Forms.Label();
		this.txtTimeout = new System.Windows.Forms.MaskedTextBox();
		this.chkAcceptDtmfInput = new System.Windows.Forms.CheckBox();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.lblSurvey = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.lblMaxRetryCount.AutoSize = true;
		this.lblMaxRetryCount.Location = new System.Drawing.Point(9, 110);
		this.lblMaxRetryCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblMaxRetryCount.Name = "lblMaxRetryCount";
		this.lblMaxRetryCount.Size = new System.Drawing.Size(112, 17);
		this.lblMaxRetryCount.TabIndex = 4;
		this.lblMaxRetryCount.Text = "Max Retry Count";
		this.txtMaxRetryCount.HidePromptOnLeave = true;
		this.txtMaxRetryCount.Location = new System.Drawing.Point(12, 131);
		this.txtMaxRetryCount.Margin = new System.Windows.Forms.Padding(4);
		this.txtMaxRetryCount.Mask = "99";
		this.txtMaxRetryCount.Name = "txtMaxRetryCount";
		this.txtMaxRetryCount.Size = new System.Drawing.Size(300, 22);
		this.txtMaxRetryCount.TabIndex = 5;
		this.txtMaxRetryCount.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtMaxRetryCount.Validating += new System.ComponentModel.CancelEventHandler(TxtMaxRetryCount_Validating);
		this.lblTimeout.AutoSize = true;
		this.lblTimeout.Location = new System.Drawing.Point(9, 63);
		this.lblTimeout.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblTimeout.Name = "lblTimeout";
		this.lblTimeout.Size = new System.Drawing.Size(102, 17);
		this.lblTimeout.TabIndex = 2;
		this.lblTimeout.Text = "Timeout (secs)";
		this.txtTimeout.HidePromptOnLeave = true;
		this.txtTimeout.Location = new System.Drawing.Point(12, 84);
		this.txtTimeout.Margin = new System.Windows.Forms.Padding(4);
		this.txtTimeout.Mask = "99";
		this.txtTimeout.Name = "txtTimeout";
		this.txtTimeout.Size = new System.Drawing.Size(300, 22);
		this.txtTimeout.TabIndex = 3;
		this.txtTimeout.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtTimeout.Validating += new System.ComponentModel.CancelEventHandler(TxtTimeout_Validating);
		this.chkAcceptDtmfInput.AutoSize = true;
		this.chkAcceptDtmfInput.Location = new System.Drawing.Point(12, 38);
		this.chkAcceptDtmfInput.Margin = new System.Windows.Forms.Padding(4);
		this.chkAcceptDtmfInput.Name = "chkAcceptDtmfInput";
		this.chkAcceptDtmfInput.Size = new System.Drawing.Size(245, 21);
		this.chkAcceptDtmfInput.TabIndex = 1;
		this.chkAcceptDtmfInput.Text = "Accept DTMF Input During Prompt";
		this.chkAcceptDtmfInput.UseVisualStyleBackColor = true;
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.lblSurvey.AutoSize = true;
		this.lblSurvey.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSurvey.Location = new System.Drawing.Point(8, 8);
		this.lblSurvey.Name = "lblSurvey";
		this.lblSurvey.Size = new System.Drawing.Size(66, 20);
		this.lblSurvey.TabIndex = 0;
		this.lblSurvey.Text = "Survey";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.lblSurvey);
		base.Controls.Add(this.lblMaxRetryCount);
		base.Controls.Add(this.txtMaxRetryCount);
		base.Controls.Add(this.lblTimeout);
		base.Controls.Add(this.txtTimeout);
		base.Controls.Add(this.chkAcceptDtmfInput);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "OptionsComponentsSurveyControl";
		base.Size = new System.Drawing.Size(780, 665);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
