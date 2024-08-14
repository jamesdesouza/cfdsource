using System;
using System.ComponentModel;
using System.Drawing;
using System.Net.Mail;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class OptionsComponentsEMailSenderControl : UserControl, IOptionsControl
{
	private IContainer components;

	private TextBox txtBody;

	private Label lblBody;

	private TextBox txtSubject;

	private Label lblSubject;

	private TextBox txtBCC;

	private Label lblBCC;

	private TextBox txtCC;

	private Label lblCC;

	private TextBox txtTo;

	private Label lblTo;

	private TextBox txtFrom;

	private Label lblFrom;

	private TextBox txtPassword;

	private Label lblPassword;

	private TextBox txtUserName;

	private Label lblUserName;

	private TextBox txtServer;

	private ComboBox comboPriority;

	private Label lblPriority;

	private Label lblServer;

	private ErrorProvider errorProvider;

	private CheckBox chkBoxIgnoreMissingAttachments;

	private Label lblServerPort;

	private MaskedTextBox txtServerPort;

	private CheckBox chkEnableSSL;

	private CheckBox chkBoxIsBodyHtml;

	private CheckBox chkBoxUseServerSettings;

	private Label lblEmailSender;

	private MailPriority GetPriority(string str)
	{
		if (!(str == "High"))
		{
			if (str == "Low")
			{
				return MailPriority.Low;
			}
			return MailPriority.Normal;
		}
		return MailPriority.High;
	}

	private string GetPriorityAsStr(MailPriority priority)
	{
		return priority switch
		{
			MailPriority.High => "High", 
			MailPriority.Low => "Low", 
			_ => "Normal", 
		};
	}

	public OptionsComponentsEMailSenderControl()
	{
		InitializeComponent();
		comboPriority.Items.AddRange(new object[3]
		{
			MailPriority.Low,
			MailPriority.Normal,
			MailPriority.High
		});
		chkBoxUseServerSettings.Checked = Settings.Default.EMailSenderTemplateUseServerSettings;
		txtServer.Text = Settings.Default.EMailSenderTemplateServer;
		txtServerPort.Text = Convert.ToString(Settings.Default.EMailSenderTemplateServerPort);
		chkEnableSSL.Checked = Settings.Default.EMailSenderTemplateServerEnableSSL;
		txtUserName.Text = Settings.Default.EMailSenderTemplateUserName;
		txtPassword.Text = Settings.Default.EMailSenderTemplatePassword;
		txtFrom.Text = Settings.Default.EMailSenderTemplateFrom;
		txtTo.Text = Settings.Default.EMailSenderTemplateTo;
		txtCC.Text = Settings.Default.EMailSenderTemplateCC;
		txtBCC.Text = Settings.Default.EMailSenderTemplateBCC;
		txtSubject.Text = Settings.Default.EMailSenderTemplateSubject;
		chkBoxIsBodyHtml.Checked = Settings.Default.EMailSenderTemplateIsBodyHtml;
		txtBody.Text = Settings.Default.EMailSenderTemplateBody;
		comboPriority.SelectedItem = GetPriority(Settings.Default.EMailSenderTemplatePriority);
		chkBoxIgnoreMissingAttachments.Checked = Settings.Default.EMailSenderTemplateIgnoreMissingAttachments;
		lblEmailSender.Text = LocalizedResourceMgr.GetString("OptionsComponentsEMailSenderControl.lblEmailSender.Text");
		chkBoxUseServerSettings.Text = LocalizedResourceMgr.GetString("OptionsComponentsEMailSenderControl.chkBoxUseServerSettings.Text");
		lblServer.Text = LocalizedResourceMgr.GetString("OptionsComponentsEMailSenderControl.lblServer.Text");
		lblServerPort.Text = LocalizedResourceMgr.GetString("OptionsComponentsEMailSenderControl.lblServerPort.Text");
		chkEnableSSL.Text = LocalizedResourceMgr.GetString("OptionsComponentsEMailSenderControl.chkEnableSSL.Text");
		lblUserName.Text = LocalizedResourceMgr.GetString("OptionsComponentsEMailSenderControl.lblUserName.Text");
		lblPassword.Text = LocalizedResourceMgr.GetString("OptionsComponentsEMailSenderControl.lblPassword.Text");
		lblFrom.Text = LocalizedResourceMgr.GetString("OptionsComponentsEMailSenderControl.lblFrom.Text");
		lblTo.Text = LocalizedResourceMgr.GetString("OptionsComponentsEMailSenderControl.lblTo.Text");
		lblCC.Text = LocalizedResourceMgr.GetString("OptionsComponentsEMailSenderControl.lblCC.Text");
		lblBCC.Text = LocalizedResourceMgr.GetString("OptionsComponentsEMailSenderControl.lblBCC.Text");
		lblSubject.Text = LocalizedResourceMgr.GetString("OptionsComponentsEMailSenderControl.lblSubject.Text");
		chkBoxIsBodyHtml.Text = LocalizedResourceMgr.GetString("OptionsComponentsEMailSenderControl.chkBoxIsBodyHtml.Text");
		lblBody.Text = LocalizedResourceMgr.GetString("OptionsComponentsEMailSenderControl.lblBody.Text");
		lblPriority.Text = LocalizedResourceMgr.GetString("OptionsComponentsEMailSenderControl.lblPriority.Text");
		chkBoxIgnoreMissingAttachments.Text = LocalizedResourceMgr.GetString("OptionsComponentsEMailSenderControl.chkBoxIgnoreMissingAttachments.Text");
	}

	private void chkBoxUseServerSettings_CheckedChanged(object sender, EventArgs e)
	{
		txtServer.Enabled = !chkBoxUseServerSettings.Checked;
		txtServerPort.Enabled = !chkBoxUseServerSettings.Checked;
		chkEnableSSL.Enabled = !chkBoxUseServerSettings.Checked;
		txtUserName.Enabled = !chkBoxUseServerSettings.Checked;
		txtPassword.Enabled = !chkBoxUseServerSettings.Checked;
		txtFrom.Enabled = !chkBoxUseServerSettings.Checked;
	}

	private void txtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void maskedTxtBox_Enter(object sender, EventArgs e)
	{
		(sender as MaskedTextBox).SelectAll();
	}

	private void validateSingleMail(string mail)
	{
		if (!string.IsNullOrEmpty(mail) && !EMailValidator.IsEmail(mail))
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsEMailSenderControl.Error.InvalidEMail"));
		}
	}

	private void validateMultipleMail(string mailList)
	{
		if (!string.IsNullOrEmpty(mailList) && !EMailValidator.IsEmailList(mailList))
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsEMailSenderControl.Error.InvalidEMailList"));
		}
	}

	private void validateFields()
	{
		if (!chkBoxUseServerSettings.Checked)
		{
			validateSingleMail(txtFrom.Text);
		}
		validateMultipleMail(txtTo.Text);
		validateMultipleMail(txtCC.Text);
		validateMultipleMail(txtBCC.Text);
	}

	private void txtSingleMail_Validating(object sender, CancelEventArgs e)
	{
		TextBox textBox = sender as TextBox;
		try
		{
			validateSingleMail(textBox.Text);
			errorProvider.SetError(textBox, string.Empty);
		}
		catch (Exception exc)
		{
			errorProvider.SetError(textBox, ErrorHelper.GetErrorDescription(exc));
		}
	}

	private void txtMultipleMail_Validating(object sender, CancelEventArgs e)
	{
		TextBox textBox = sender as TextBox;
		try
		{
			validateMultipleMail(textBox.Text);
			errorProvider.SetError(textBox, string.Empty);
		}
		catch (Exception exc)
		{
			errorProvider.SetError(textBox, ErrorHelper.GetErrorDescription(exc));
		}
	}

	public void Save()
	{
		validateFields();
		Settings.Default.EMailSenderTemplateUseServerSettings = chkBoxUseServerSettings.Checked;
		Settings.Default.EMailSenderTemplateServer = txtServer.Text;
		Settings.Default.EMailSenderTemplateServerPort = Convert.ToInt32(txtServerPort.Text);
		Settings.Default.EMailSenderTemplateServerEnableSSL = chkEnableSSL.Checked;
		Settings.Default.EMailSenderTemplateUserName = txtUserName.Text;
		Settings.Default.EMailSenderTemplatePassword = txtPassword.Text;
		Settings.Default.EMailSenderTemplateFrom = txtFrom.Text;
		Settings.Default.EMailSenderTemplateTo = txtTo.Text;
		Settings.Default.EMailSenderTemplateCC = txtCC.Text;
		Settings.Default.EMailSenderTemplateBCC = txtBCC.Text;
		Settings.Default.EMailSenderTemplateSubject = txtSubject.Text;
		Settings.Default.EMailSenderTemplateIsBodyHtml = chkBoxIsBodyHtml.Checked;
		Settings.Default.EMailSenderTemplateBody = txtBody.Text;
		Settings.Default.EMailSenderTemplatePriority = GetPriorityAsStr((MailPriority)comboPriority.SelectedItem);
		Settings.Default.EMailSenderTemplateIgnoreMissingAttachments = chkBoxIgnoreMissingAttachments.Checked;
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
		this.txtBody = new System.Windows.Forms.TextBox();
		this.lblBody = new System.Windows.Forms.Label();
		this.txtSubject = new System.Windows.Forms.TextBox();
		this.lblSubject = new System.Windows.Forms.Label();
		this.txtBCC = new System.Windows.Forms.TextBox();
		this.lblBCC = new System.Windows.Forms.Label();
		this.txtCC = new System.Windows.Forms.TextBox();
		this.lblCC = new System.Windows.Forms.Label();
		this.txtTo = new System.Windows.Forms.TextBox();
		this.lblTo = new System.Windows.Forms.Label();
		this.txtFrom = new System.Windows.Forms.TextBox();
		this.lblFrom = new System.Windows.Forms.Label();
		this.txtPassword = new System.Windows.Forms.TextBox();
		this.lblPassword = new System.Windows.Forms.Label();
		this.txtUserName = new System.Windows.Forms.TextBox();
		this.lblUserName = new System.Windows.Forms.Label();
		this.txtServer = new System.Windows.Forms.TextBox();
		this.comboPriority = new System.Windows.Forms.ComboBox();
		this.lblPriority = new System.Windows.Forms.Label();
		this.lblServer = new System.Windows.Forms.Label();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.chkBoxIgnoreMissingAttachments = new System.Windows.Forms.CheckBox();
		this.lblServerPort = new System.Windows.Forms.Label();
		this.txtServerPort = new System.Windows.Forms.MaskedTextBox();
		this.chkEnableSSL = new System.Windows.Forms.CheckBox();
		this.chkBoxIsBodyHtml = new System.Windows.Forms.CheckBox();
		this.chkBoxUseServerSettings = new System.Windows.Forms.CheckBox();
		this.lblEmailSender = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.txtBody.AcceptsReturn = true;
		this.txtBody.Location = new System.Drawing.Point(14, 462);
		this.txtBody.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtBody.MaxLength = 4096;
		this.txtBody.Multiline = true;
		this.txtBody.Name = "txtBody";
		this.txtBody.ScrollBars = System.Windows.Forms.ScrollBars.Both;
		this.txtBody.Size = new System.Drawing.Size(733, 90);
		this.txtBody.TabIndex = 23;
		this.txtBody.Enter += new System.EventHandler(txtBox_Enter);
		this.lblBody.AutoSize = true;
		this.lblBody.Location = new System.Drawing.Point(11, 441);
		this.lblBody.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblBody.Name = "lblBody";
		this.lblBody.Size = new System.Drawing.Size(40, 17);
		this.lblBody.TabIndex = 22;
		this.lblBody.Text = "Body";
		this.txtSubject.Location = new System.Drawing.Point(14, 377);
		this.txtSubject.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtSubject.MaxLength = 1024;
		this.txtSubject.Name = "txtSubject";
		this.txtSubject.Size = new System.Drawing.Size(733, 22);
		this.txtSubject.TabIndex = 20;
		this.txtSubject.Enter += new System.EventHandler(txtBox_Enter);
		this.lblSubject.AutoSize = true;
		this.lblSubject.Location = new System.Drawing.Point(11, 356);
		this.lblSubject.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblSubject.Name = "lblSubject";
		this.lblSubject.Size = new System.Drawing.Size(55, 17);
		this.lblSubject.TabIndex = 19;
		this.lblSubject.Text = "Subject";
		this.txtBCC.Location = new System.Drawing.Point(397, 325);
		this.txtBCC.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtBCC.MaxLength = 1024;
		this.txtBCC.Name = "txtBCC";
		this.txtBCC.Size = new System.Drawing.Size(350, 22);
		this.txtBCC.TabIndex = 18;
		this.txtBCC.Enter += new System.EventHandler(txtBox_Enter);
		this.txtBCC.Validating += new System.ComponentModel.CancelEventHandler(txtMultipleMail_Validating);
		this.lblBCC.AutoSize = true;
		this.lblBCC.Location = new System.Drawing.Point(394, 304);
		this.lblBCC.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblBCC.Name = "lblBCC";
		this.lblBCC.Size = new System.Drawing.Size(35, 17);
		this.lblBCC.TabIndex = 17;
		this.lblBCC.Text = "BCC";
		this.txtCC.Location = new System.Drawing.Point(13, 325);
		this.txtCC.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtCC.MaxLength = 1024;
		this.txtCC.Name = "txtCC";
		this.txtCC.Size = new System.Drawing.Size(351, 22);
		this.txtCC.TabIndex = 16;
		this.txtCC.Enter += new System.EventHandler(txtBox_Enter);
		this.txtCC.Validating += new System.ComponentModel.CancelEventHandler(txtMultipleMail_Validating);
		this.lblCC.AutoSize = true;
		this.lblCC.Location = new System.Drawing.Point(11, 304);
		this.lblCC.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblCC.Name = "lblCC";
		this.lblCC.Size = new System.Drawing.Size(26, 17);
		this.lblCC.TabIndex = 15;
		this.lblCC.Text = "CC";
		this.txtTo.Location = new System.Drawing.Point(397, 273);
		this.txtTo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtTo.MaxLength = 1024;
		this.txtTo.Name = "txtTo";
		this.txtTo.Size = new System.Drawing.Size(350, 22);
		this.txtTo.TabIndex = 14;
		this.txtTo.Enter += new System.EventHandler(txtBox_Enter);
		this.txtTo.Validating += new System.ComponentModel.CancelEventHandler(txtMultipleMail_Validating);
		this.lblTo.AutoSize = true;
		this.lblTo.Location = new System.Drawing.Point(394, 252);
		this.lblTo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblTo.Name = "lblTo";
		this.lblTo.Size = new System.Drawing.Size(25, 17);
		this.lblTo.TabIndex = 13;
		this.lblTo.Text = "To";
		this.txtFrom.Location = new System.Drawing.Point(14, 273);
		this.txtFrom.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtFrom.MaxLength = 1024;
		this.txtFrom.Name = "txtFrom";
		this.txtFrom.Size = new System.Drawing.Size(350, 22);
		this.txtFrom.TabIndex = 12;
		this.txtFrom.Enter += new System.EventHandler(txtBox_Enter);
		this.txtFrom.Validating += new System.ComponentModel.CancelEventHandler(txtSingleMail_Validating);
		this.lblFrom.AutoSize = true;
		this.lblFrom.Location = new System.Drawing.Point(11, 252);
		this.lblFrom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblFrom.Name = "lblFrom";
		this.lblFrom.Size = new System.Drawing.Size(40, 17);
		this.lblFrom.TabIndex = 11;
		this.lblFrom.Text = "From";
		this.txtPassword.Location = new System.Drawing.Point(397, 221);
		this.txtPassword.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtPassword.MaxLength = 1024;
		this.txtPassword.Name = "txtPassword";
		this.txtPassword.PasswordChar = '*';
		this.txtPassword.Size = new System.Drawing.Size(350, 22);
		this.txtPassword.TabIndex = 10;
		this.txtPassword.Enter += new System.EventHandler(txtBox_Enter);
		this.lblPassword.AutoSize = true;
		this.lblPassword.Location = new System.Drawing.Point(394, 200);
		this.lblPassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblPassword.Name = "lblPassword";
		this.lblPassword.Size = new System.Drawing.Size(69, 17);
		this.lblPassword.TabIndex = 9;
		this.lblPassword.Text = "Password";
		this.txtUserName.Location = new System.Drawing.Point(14, 221);
		this.txtUserName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtUserName.MaxLength = 1024;
		this.txtUserName.Name = "txtUserName";
		this.txtUserName.Size = new System.Drawing.Size(350, 22);
		this.txtUserName.TabIndex = 8;
		this.txtUserName.Enter += new System.EventHandler(txtBox_Enter);
		this.lblUserName.AutoSize = true;
		this.lblUserName.Location = new System.Drawing.Point(10, 200);
		this.lblUserName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblUserName.Name = "lblUserName";
		this.lblUserName.Size = new System.Drawing.Size(79, 17);
		this.lblUserName.TabIndex = 7;
		this.lblUserName.Text = "User Name";
		this.txtServer.Location = new System.Drawing.Point(14, 93);
		this.txtServer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtServer.MaxLength = 1024;
		this.txtServer.Name = "txtServer";
		this.txtServer.Size = new System.Drawing.Size(733, 22);
		this.txtServer.TabIndex = 3;
		this.txtServer.Enter += new System.EventHandler(txtBox_Enter);
		this.comboPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboPriority.FormattingEnabled = true;
		this.comboPriority.Location = new System.Drawing.Point(17, 582);
		this.comboPriority.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.comboPriority.Name = "comboPriority";
		this.comboPriority.Size = new System.Drawing.Size(730, 24);
		this.comboPriority.TabIndex = 25;
		this.lblPriority.AutoSize = true;
		this.lblPriority.Location = new System.Drawing.Point(14, 561);
		this.lblPriority.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblPriority.Name = "lblPriority";
		this.lblPriority.Size = new System.Drawing.Size(52, 17);
		this.lblPriority.TabIndex = 24;
		this.lblPriority.Text = "Priority";
		this.lblServer.AutoSize = true;
		this.lblServer.Location = new System.Drawing.Point(11, 72);
		this.lblServer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblServer.Name = "lblServer";
		this.lblServer.Size = new System.Drawing.Size(92, 17);
		this.lblServer.TabIndex = 2;
		this.lblServer.Text = "SMTP Server";
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.chkBoxIgnoreMissingAttachments.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.chkBoxIgnoreMissingAttachments.AutoSize = true;
		this.chkBoxIgnoreMissingAttachments.Location = new System.Drawing.Point(17, 619);
		this.chkBoxIgnoreMissingAttachments.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.chkBoxIgnoreMissingAttachments.Name = "chkBoxIgnoreMissingAttachments";
		this.chkBoxIgnoreMissingAttachments.Size = new System.Drawing.Size(203, 21);
		this.chkBoxIgnoreMissingAttachments.TabIndex = 26;
		this.chkBoxIgnoreMissingAttachments.Text = "Ignore Missing Attachments";
		this.chkBoxIgnoreMissingAttachments.UseVisualStyleBackColor = true;
		this.lblServerPort.AutoSize = true;
		this.lblServerPort.Location = new System.Drawing.Point(11, 119);
		this.lblServerPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblServerPort.Name = "lblServerPort";
		this.lblServerPort.Size = new System.Drawing.Size(80, 17);
		this.lblServerPort.TabIndex = 4;
		this.lblServerPort.Text = "Server Port";
		this.txtServerPort.HidePromptOnLeave = true;
		this.txtServerPort.Location = new System.Drawing.Point(14, 140);
		this.txtServerPort.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtServerPort.Mask = "99999";
		this.txtServerPort.Name = "txtServerPort";
		this.txtServerPort.Size = new System.Drawing.Size(733, 22);
		this.txtServerPort.TabIndex = 5;
		this.txtServerPort.Enter += new System.EventHandler(maskedTxtBox_Enter);
		this.chkEnableSSL.AutoSize = true;
		this.chkEnableSSL.Location = new System.Drawing.Point(12, 170);
		this.chkEnableSSL.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.chkEnableSSL.Name = "chkEnableSSL";
		this.chkEnableSSL.Size = new System.Drawing.Size(270, 21);
		this.chkEnableSSL.TabIndex = 6;
		this.chkEnableSSL.Text = "Connect to the SMTP server over SSL";
		this.chkEnableSSL.UseVisualStyleBackColor = true;
		this.chkBoxIsBodyHtml.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.chkBoxIsBodyHtml.AutoSize = true;
		this.chkBoxIsBodyHtml.Location = new System.Drawing.Point(14, 412);
		this.chkBoxIsBodyHtml.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.chkBoxIsBodyHtml.Name = "chkBoxIsBodyHtml";
		this.chkBoxIsBodyHtml.Size = new System.Drawing.Size(118, 21);
		this.chkBoxIsBodyHtml.TabIndex = 21;
		this.chkBoxIsBodyHtml.Text = "Body is HTML";
		this.chkBoxIsBodyHtml.UseVisualStyleBackColor = true;
		this.chkBoxUseServerSettings.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.chkBoxUseServerSettings.AutoSize = true;
		this.chkBoxUseServerSettings.Location = new System.Drawing.Point(12, 38);
		this.chkBoxUseServerSettings.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.chkBoxUseServerSettings.Name = "chkBoxUseServerSettings";
		this.chkBoxUseServerSettings.Size = new System.Drawing.Size(257, 21);
		this.chkBoxUseServerSettings.TabIndex = 1;
		this.chkBoxUseServerSettings.Text = "Use 3CX Server connection settings";
		this.chkBoxUseServerSettings.UseVisualStyleBackColor = true;
		this.chkBoxUseServerSettings.CheckedChanged += new System.EventHandler(chkBoxUseServerSettings_CheckedChanged);
		this.lblEmailSender.AutoSize = true;
		this.lblEmailSender.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblEmailSender.Location = new System.Drawing.Point(8, 8);
		this.lblEmailSender.Name = "lblEmailSender";
		this.lblEmailSender.Size = new System.Drawing.Size(121, 20);
		this.lblEmailSender.TabIndex = 0;
		this.lblEmailSender.Text = "EMail Sender";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.lblEmailSender);
		base.Controls.Add(this.chkBoxUseServerSettings);
		base.Controls.Add(this.chkBoxIsBodyHtml);
		base.Controls.Add(this.chkEnableSSL);
		base.Controls.Add(this.lblServerPort);
		base.Controls.Add(this.txtServerPort);
		base.Controls.Add(this.chkBoxIgnoreMissingAttachments);
		base.Controls.Add(this.txtBody);
		base.Controls.Add(this.lblBody);
		base.Controls.Add(this.txtSubject);
		base.Controls.Add(this.lblSubject);
		base.Controls.Add(this.txtBCC);
		base.Controls.Add(this.lblBCC);
		base.Controls.Add(this.txtCC);
		base.Controls.Add(this.lblCC);
		base.Controls.Add(this.txtTo);
		base.Controls.Add(this.lblTo);
		base.Controls.Add(this.txtFrom);
		base.Controls.Add(this.lblFrom);
		base.Controls.Add(this.txtPassword);
		base.Controls.Add(this.lblPassword);
		base.Controls.Add(this.txtUserName);
		base.Controls.Add(this.lblUserName);
		base.Controls.Add(this.txtServer);
		base.Controls.Add(this.comboPriority);
		base.Controls.Add(this.lblPriority);
		base.Controls.Add(this.lblServer);
		base.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		base.Name = "OptionsComponentsEMailSenderControl";
		base.Size = new System.Drawing.Size(780, 665);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
