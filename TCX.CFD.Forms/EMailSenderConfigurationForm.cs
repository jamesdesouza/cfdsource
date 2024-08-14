using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Net.Mail;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Properties;

namespace TCX.CFD.Forms;

public class EMailSenderConfigurationForm : Form
{
	private readonly EMailSenderComponent eMailSenderComponent;

	private readonly List<string> validVariables;

	private IContainer components;

	private Label lblServer;

	private Label lblPriority;

	private ComboBox comboPriority;

	private Button cancelButton;

	private Button okButton;

	private TextBox txtServer;

	private Label lblAttachments;

	private DataGridView attachmentsGrid;

	private BindingSource attachmentBindingSource;

	private Button serverExpressionButton;

	private Button userNameExpressionButton;

	private TextBox txtUserName;

	private Label lblUserName;

	private Button passwordExpressionButton;

	private TextBox txtPassword;

	private Label lblPassword;

	private Button fromExpressionButton;

	private TextBox txtFrom;

	private Label lblFrom;

	private Button toExpressionButton;

	private TextBox txtTo;

	private Label lblTo;

	private Button ccExpressionButton;

	private TextBox txtCC;

	private Label lblCC;

	private Button bccExpressionButton;

	private TextBox txtBCC;

	private Label lblBCC;

	private Button subjectExpressionButton;

	private TextBox txtSubject;

	private Label lblSubject;

	private Button bodyExpressionButton;

	private TextBox txtBody;

	private Label lblBody;

	private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn File;

	private DataGridViewButtonColumn expressionBuilderColumn;

	private ErrorProvider errorProvider;

	private CheckBox chkBoxIgnoreMissingAttachments;

	private Button serverPortExpressionButton;

	private Label lblServerPort;

	private TextBox txtServerPort;

	private Button enableSslExpressionButton;

	private Label lblEnableSSL;

	private TextBox txtEnableSSL;

	private CheckBox chkBoxIsBodyHtml;

	private CheckBox chkBoxUseServerSettings;

	public bool UseServerSettings => chkBoxUseServerSettings.Checked;

	public string Server => txtServer.Text;

	public string Port => txtServerPort.Text;

	public string EnableSSL => txtEnableSSL.Text;

	public string UserName => txtUserName.Text;

	public string Password => txtPassword.Text;

	public string From => txtFrom.Text;

	public string To => txtTo.Text;

	public string CC => txtCC.Text;

	public string BCC => txtBCC.Text;

	public string Subject => txtSubject.Text;

	public bool IsBodyHtml => chkBoxIsBodyHtml.Checked;

	public string Body => txtBody.Text;

	public MailPriority Priority => (MailPriority)comboPriority.SelectedItem;

	public bool IgnoreMissingAttachments => chkBoxIgnoreMissingAttachments.Checked;

	public List<MailAttachment> Attachments
	{
		get
		{
			List<MailAttachment> list = new List<MailAttachment>();
			foreach (MailAttachment item in attachmentBindingSource.List)
			{
				list.Add(item);
			}
			return list;
		}
	}

	public EMailSenderConfigurationForm(EMailSenderComponent eMailSenderComponent)
	{
		InitializeComponent();
		this.eMailSenderComponent = eMailSenderComponent;
		validVariables = ExpressionHelper.GetValidVariables(eMailSenderComponent);
		comboPriority.Items.AddRange(new object[3]
		{
			MailPriority.Low,
			MailPriority.Normal,
			MailPriority.High
		});
		chkBoxUseServerSettings.Checked = eMailSenderComponent.UseServerSettings;
		txtServer.Text = eMailSenderComponent.Server;
		txtServerPort.Text = eMailSenderComponent.Port;
		txtEnableSSL.Text = eMailSenderComponent.EnableSSL;
		txtUserName.Text = eMailSenderComponent.UserName;
		txtPassword.Text = eMailSenderComponent.Password;
		txtFrom.Text = eMailSenderComponent.From;
		txtTo.Text = eMailSenderComponent.To;
		txtCC.Text = eMailSenderComponent.CC;
		txtBCC.Text = eMailSenderComponent.BCC;
		txtSubject.Text = eMailSenderComponent.Subject;
		chkBoxIsBodyHtml.Checked = eMailSenderComponent.IsBodyHtml;
		txtBody.Text = eMailSenderComponent.Body;
		comboPriority.SelectedItem = eMailSenderComponent.Priority;
		chkBoxIgnoreMissingAttachments.Checked = eMailSenderComponent.IgnoreMissingAttachments;
		foreach (MailAttachment attachment in eMailSenderComponent.Attachments)
		{
			attachmentBindingSource.List.Add(new MailAttachment(attachment.Name, attachment.File));
		}
		TxtServer_Validating(txtServer, new CancelEventArgs());
		TxtServerPort_Validating(txtServerPort, new CancelEventArgs());
		TxtEnableSSL_Validating(txtEnableSSL, new CancelEventArgs());
		TxtUserName_Validating(txtUserName, new CancelEventArgs());
		TxtPassword_Validating(txtPassword, new CancelEventArgs());
		TxtFrom_Validating(txtFrom, new CancelEventArgs());
		TxtDestination_Validating(txtTo, new CancelEventArgs());
		TxtSubject_Validating(txtSubject, new CancelEventArgs());
		TxtBody_Validating(txtBody, new CancelEventArgs());
		Text = LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.Title");
		chkBoxUseServerSettings.Text = LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.chkBoxUseServerSettings.Text");
		lblServer.Text = LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.lblServer.Text");
		lblServerPort.Text = LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.lblServerPort.Text");
		lblEnableSSL.Text = LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.lblEnableSSL.Text");
		lblUserName.Text = LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.lblUserName.Text");
		lblPassword.Text = LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.lblPassword.Text");
		lblFrom.Text = LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.lblFrom.Text");
		lblTo.Text = LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.lblTo.Text");
		lblCC.Text = LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.lblCC.Text");
		lblBCC.Text = LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.lblBCC.Text");
		lblSubject.Text = LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.lblSubject.Text");
		chkBoxIsBodyHtml.Text = LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.chkBoxIsBodyHtml.Text");
		lblBody.Text = LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.lblBody.Text");
		lblPriority.Text = LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.lblPriority.Text");
		chkBoxIgnoreMissingAttachments.Text = LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.chkBoxIgnoreMissingAttachments.Text");
		lblAttachments.Text = LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.lblAttachments.Text");
		okButton.Text = LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.cancelButton.Text");
	}

	private void ChkBoxUseServerSettings_CheckedChanged(object sender, EventArgs e)
	{
		txtServer.Enabled = !chkBoxUseServerSettings.Checked;
		txtServerPort.Enabled = !chkBoxUseServerSettings.Checked;
		txtEnableSSL.Enabled = !chkBoxUseServerSettings.Checked;
		txtUserName.Enabled = !chkBoxUseServerSettings.Checked;
		txtPassword.Enabled = !chkBoxUseServerSettings.Checked;
		txtFrom.Enabled = !chkBoxUseServerSettings.Checked;
		serverExpressionButton.Enabled = !chkBoxUseServerSettings.Checked;
		serverPortExpressionButton.Enabled = !chkBoxUseServerSettings.Checked;
		enableSslExpressionButton.Enabled = !chkBoxUseServerSettings.Checked;
		userNameExpressionButton.Enabled = !chkBoxUseServerSettings.Checked;
		passwordExpressionButton.Enabled = !chkBoxUseServerSettings.Checked;
		fromExpressionButton.Enabled = !chkBoxUseServerSettings.Checked;
		TxtServer_Validating(txtServer, new CancelEventArgs());
		TxtServerPort_Validating(txtServerPort, new CancelEventArgs());
		TxtEnableSSL_Validating(txtEnableSSL, new CancelEventArgs());
		TxtUserName_Validating(txtUserName, new CancelEventArgs());
		TxtPassword_Validating(txtPassword, new CancelEventArgs());
		TxtFrom_Validating(txtFrom, new CancelEventArgs());
	}

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void TxtServer_Validating(object sender, CancelEventArgs e)
	{
		if (txtServer.Enabled)
		{
			if (string.IsNullOrEmpty(txtServer.Text))
			{
				errorProvider.SetError(serverExpressionButton, LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.Error.ServerIsMandatory"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, txtServer.Text).IsSafeExpression())
			{
				errorProvider.SetError(serverExpressionButton, LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.Error.ServerIsInvalid"));
			}
			else
			{
				errorProvider.SetError(serverExpressionButton, string.Empty);
			}
		}
		else
		{
			errorProvider.SetError(serverExpressionButton, string.Empty);
		}
	}

	private void TxtServerPort_Validating(object sender, CancelEventArgs e)
	{
		if (txtServerPort.Enabled && !string.IsNullOrEmpty(txtServerPort.Text))
		{
			if (!AbsArgument.BuildArgument(validVariables, txtServerPort.Text).IsSafeExpression())
			{
				errorProvider.SetError(serverPortExpressionButton, LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.Error.ServerPortIsInvalid"));
			}
			else
			{
				errorProvider.SetError(serverPortExpressionButton, string.Empty);
			}
		}
		else
		{
			errorProvider.SetError(serverPortExpressionButton, string.Empty);
		}
	}

	private void TxtEnableSSL_Validating(object sender, CancelEventArgs e)
	{
		if (txtEnableSSL.Enabled)
		{
			if (string.IsNullOrEmpty(txtEnableSSL.Text))
			{
				errorProvider.SetError(enableSslExpressionButton, LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.Error.EnableSslIsMandatory"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, txtEnableSSL.Text).IsSafeExpression())
			{
				errorProvider.SetError(enableSslExpressionButton, LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.Error.EnableSslIsInvalid"));
			}
			else
			{
				errorProvider.SetError(enableSslExpressionButton, string.Empty);
			}
		}
		else
		{
			errorProvider.SetError(enableSslExpressionButton, string.Empty);
		}
	}

	private void TxtUserName_Validating(object sender, CancelEventArgs e)
	{
		if (txtUserName.Enabled && !string.IsNullOrEmpty(txtUserName.Text))
		{
			if (!AbsArgument.BuildArgument(validVariables, txtUserName.Text).IsSafeExpression())
			{
				errorProvider.SetError(userNameExpressionButton, LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.Error.UserNameIsInvalid"));
			}
			else
			{
				errorProvider.SetError(userNameExpressionButton, string.Empty);
			}
		}
		else
		{
			errorProvider.SetError(userNameExpressionButton, string.Empty);
		}
	}

	private void TxtPassword_Validating(object sender, CancelEventArgs e)
	{
		if (txtPassword.Enabled && !string.IsNullOrEmpty(txtPassword.Text))
		{
			if (!AbsArgument.BuildArgument(validVariables, txtPassword.Text).IsSafeExpression())
			{
				errorProvider.SetError(passwordExpressionButton, LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.Error.PasswordIsInvalid"));
			}
			else
			{
				errorProvider.SetError(passwordExpressionButton, string.Empty);
			}
		}
		else
		{
			errorProvider.SetError(passwordExpressionButton, string.Empty);
		}
	}

	private void TxtFrom_Validating(object sender, CancelEventArgs e)
	{
		if (txtFrom.Enabled)
		{
			if (string.IsNullOrEmpty(txtFrom.Text))
			{
				errorProvider.SetError(fromExpressionButton, LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.Error.FromIsMandatory"));
			}
			else if (txtFrom.Text.Length > 2 && ExpressionHelper.IsStringLiteral(txtFrom.Text) && !EMailValidator.IsEmail(txtFrom.Text.Substring(1, txtFrom.Text.Length - 2)))
			{
				errorProvider.SetError(fromExpressionButton, LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.Error.FromIsInvalid"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, txtFrom.Text).IsSafeExpression())
			{
				errorProvider.SetError(fromExpressionButton, LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.Error.FromIsInvalidExpression"));
			}
			else
			{
				errorProvider.SetError(fromExpressionButton, string.Empty);
			}
		}
		else
		{
			errorProvider.SetError(fromExpressionButton, string.Empty);
		}
	}

	private void TxtDestination_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtTo.Text))
		{
			errorProvider.SetError(toExpressionButton, (string.IsNullOrEmpty(txtCC.Text) && string.IsNullOrEmpty(txtBCC.Text)) ? LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.Error.ToCCorBCCIsMandatory") : string.Empty);
		}
		else if (txtTo.Text.Length > 2 && ExpressionHelper.IsStringLiteral(txtTo.Text) && !EMailValidator.IsEmailList(txtTo.Text.Substring(1, txtTo.Text.Length - 2)))
		{
			errorProvider.SetError(toExpressionButton, LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.Error.ToIsInvalid"));
		}
		else if (!AbsArgument.BuildArgument(validVariables, txtTo.Text).IsSafeExpression())
		{
			errorProvider.SetError(toExpressionButton, LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.Error.ToIsInvalidExpression"));
		}
		else
		{
			errorProvider.SetError(toExpressionButton, string.Empty);
		}
		if (string.IsNullOrEmpty(txtCC.Text))
		{
			errorProvider.SetError(ccExpressionButton, (string.IsNullOrEmpty(txtTo.Text) && string.IsNullOrEmpty(txtBCC.Text)) ? LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.Error.ToCCorBCCIsMandatory") : string.Empty);
		}
		else if (txtCC.Text.Length > 2 && ExpressionHelper.IsStringLiteral(txtCC.Text) && !EMailValidator.IsEmailList(txtCC.Text.Substring(1, txtCC.Text.Length - 2)))
		{
			errorProvider.SetError(ccExpressionButton, LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.Error.CCIsInvalid"));
		}
		else if (!AbsArgument.BuildArgument(validVariables, txtCC.Text).IsSafeExpression())
		{
			errorProvider.SetError(ccExpressionButton, LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.Error.CCIsInvalidExpression"));
		}
		else
		{
			errorProvider.SetError(ccExpressionButton, string.Empty);
		}
		if (string.IsNullOrEmpty(txtBCC.Text))
		{
			errorProvider.SetError(bccExpressionButton, (string.IsNullOrEmpty(txtTo.Text) && string.IsNullOrEmpty(txtCC.Text)) ? LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.Error.ToCCorBCCIsMandatory") : string.Empty);
		}
		else if (txtBCC.Text.Length > 2 && ExpressionHelper.IsStringLiteral(txtBCC.Text) && !EMailValidator.IsEmailList(txtBCC.Text.Substring(1, txtBCC.Text.Length - 2)))
		{
			errorProvider.SetError(bccExpressionButton, LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.Error.BCCIsInvalid"));
		}
		else if (!AbsArgument.BuildArgument(validVariables, txtBCC.Text).IsSafeExpression())
		{
			errorProvider.SetError(bccExpressionButton, LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.Error.BCCIsInvalidExpression"));
		}
		else
		{
			errorProvider.SetError(bccExpressionButton, string.Empty);
		}
	}

	private void ValidateSubjectBody()
	{
		if (string.IsNullOrEmpty(txtSubject.Text))
		{
			errorProvider.SetError(subjectExpressionButton, string.IsNullOrEmpty(txtBody.Text) ? LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.Error.SubjectOrBodyIsMandatory") : string.Empty);
		}
		else if (!AbsArgument.BuildArgument(validVariables, txtSubject.Text).IsSafeExpression())
		{
			errorProvider.SetError(subjectExpressionButton, LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.Error.SubjectIsInvalid"));
		}
		else
		{
			errorProvider.SetError(subjectExpressionButton, string.Empty);
		}
		if (string.IsNullOrEmpty(txtBody.Text))
		{
			errorProvider.SetError(bodyExpressionButton, string.IsNullOrEmpty(txtSubject.Text) ? LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.Error.SubjectOrBodyIsMandatory") : string.Empty);
		}
		else if (!AbsArgument.BuildArgument(validVariables, txtBody.Text).IsSafeExpression())
		{
			errorProvider.SetError(bodyExpressionButton, LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.Error.BodyIsInvalid"));
		}
		else
		{
			errorProvider.SetError(bodyExpressionButton, string.Empty);
		}
	}

	private void TxtSubject_Validating(object sender, CancelEventArgs e)
	{
		ValidateSubjectBody();
	}

	private void TxtBody_Validating(object sender, CancelEventArgs e)
	{
		ValidateSubjectBody();
	}

	private void ServerExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(eMailSenderComponent);
		expressionEditorForm.Expression = txtServer.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtServer.Text = expressionEditorForm.Expression;
			TxtServer_Validating(txtServer, new CancelEventArgs());
		}
	}

	private void ServerPortExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(eMailSenderComponent);
		expressionEditorForm.Expression = txtServerPort.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtServerPort.Text = expressionEditorForm.Expression;
			TxtServerPort_Validating(txtServerPort, new CancelEventArgs());
		}
	}

	private void EnableSslExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(eMailSenderComponent);
		expressionEditorForm.Expression = txtEnableSSL.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtEnableSSL.Text = expressionEditorForm.Expression;
			TxtEnableSSL_Validating(txtEnableSSL, new CancelEventArgs());
		}
	}

	private void UserNameExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(eMailSenderComponent);
		expressionEditorForm.Expression = txtUserName.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtUserName.Text = expressionEditorForm.Expression;
			TxtUserName_Validating(txtUserName, new CancelEventArgs());
		}
	}

	private void PasswordExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(eMailSenderComponent);
		expressionEditorForm.Expression = txtPassword.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtPassword.Text = expressionEditorForm.Expression;
			TxtPassword_Validating(txtPassword, new CancelEventArgs());
		}
	}

	private void FromExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(eMailSenderComponent);
		expressionEditorForm.Expression = txtFrom.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtFrom.Text = expressionEditorForm.Expression;
			TxtFrom_Validating(txtFrom, new CancelEventArgs());
		}
	}

	private void ToExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(eMailSenderComponent);
		expressionEditorForm.Expression = txtTo.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtTo.Text = expressionEditorForm.Expression;
			TxtDestination_Validating(txtTo, new CancelEventArgs());
		}
	}

	private void CcExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(eMailSenderComponent);
		expressionEditorForm.Expression = txtCC.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtCC.Text = expressionEditorForm.Expression;
			TxtDestination_Validating(txtCC, new CancelEventArgs());
		}
	}

	private void BccExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(eMailSenderComponent);
		expressionEditorForm.Expression = txtBCC.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtBCC.Text = expressionEditorForm.Expression;
			TxtDestination_Validating(txtBCC, new CancelEventArgs());
		}
	}

	private void SubjectExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(eMailSenderComponent);
		expressionEditorForm.Expression = txtSubject.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtSubject.Text = expressionEditorForm.Expression;
			TxtSubject_Validating(txtSubject, new CancelEventArgs());
		}
	}

	private void BodyExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(eMailSenderComponent);
		expressionEditorForm.Expression = txtBody.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtBody.Text = expressionEditorForm.Expression;
			TxtBody_Validating(txtBody, new CancelEventArgs());
		}
	}

	private void AttachmentsGrid_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		if (e.ColumnIndex == 2 && e.RowIndex >= 0)
		{
			ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(eMailSenderComponent);
			expressionEditorForm.Expression = ((attachmentsGrid[1, e.RowIndex].Value == null) ? string.Empty : attachmentsGrid[1, e.RowIndex].Value.ToString());
			if (expressionEditorForm.ShowDialog() == DialogResult.OK)
			{
				attachmentsGrid.CurrentCell = attachmentsGrid[1, e.RowIndex];
				SendKeys.SendWait(" ");
				attachmentsGrid.EndEdit();
				attachmentsGrid.CurrentCell = null;
				attachmentsGrid[1, e.RowIndex].Value = expressionEditorForm.Expression;
			}
		}
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		List<string> list = new List<string>();
		foreach (MailAttachment item in attachmentBindingSource.List)
		{
			if (!string.IsNullOrEmpty(item.Name) || !string.IsNullOrEmpty(item.File))
			{
				if (string.IsNullOrEmpty(item.Name) || string.IsNullOrEmpty(item.File))
				{
					MessageBox.Show(LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.MessageBox.Error.EmptyValuesNotAllowed"), LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
					attachmentBindingSource.Position = attachmentBindingSource.IndexOf(item);
					return;
				}
				if (list.Contains(item.Name))
				{
					MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.MessageBox.Error.DuplicatedNamesNotAllowed"), item.Name), LocalizedResourceMgr.GetString("EMailSenderConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
					attachmentBindingSource.Position = attachmentBindingSource.IndexOf(item);
					return;
				}
				list.Add(item.Name);
			}
		}
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void EMailSenderConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		eMailSenderComponent.ShowHelp();
	}

	private void EMailSenderConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		eMailSenderComponent.ShowHelp();
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
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
		this.lblServer = new System.Windows.Forms.Label();
		this.lblPriority = new System.Windows.Forms.Label();
		this.comboPriority = new System.Windows.Forms.ComboBox();
		this.cancelButton = new System.Windows.Forms.Button();
		this.okButton = new System.Windows.Forms.Button();
		this.txtServer = new System.Windows.Forms.TextBox();
		this.lblAttachments = new System.Windows.Forms.Label();
		this.attachmentsGrid = new System.Windows.Forms.DataGridView();
		this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.File = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.expressionBuilderColumn = new System.Windows.Forms.DataGridViewButtonColumn();
		this.attachmentBindingSource = new System.Windows.Forms.BindingSource(this.components);
		this.serverExpressionButton = new System.Windows.Forms.Button();
		this.userNameExpressionButton = new System.Windows.Forms.Button();
		this.txtUserName = new System.Windows.Forms.TextBox();
		this.lblUserName = new System.Windows.Forms.Label();
		this.passwordExpressionButton = new System.Windows.Forms.Button();
		this.txtPassword = new System.Windows.Forms.TextBox();
		this.lblPassword = new System.Windows.Forms.Label();
		this.fromExpressionButton = new System.Windows.Forms.Button();
		this.txtFrom = new System.Windows.Forms.TextBox();
		this.lblFrom = new System.Windows.Forms.Label();
		this.toExpressionButton = new System.Windows.Forms.Button();
		this.txtTo = new System.Windows.Forms.TextBox();
		this.lblTo = new System.Windows.Forms.Label();
		this.ccExpressionButton = new System.Windows.Forms.Button();
		this.txtCC = new System.Windows.Forms.TextBox();
		this.lblCC = new System.Windows.Forms.Label();
		this.bccExpressionButton = new System.Windows.Forms.Button();
		this.txtBCC = new System.Windows.Forms.TextBox();
		this.lblBCC = new System.Windows.Forms.Label();
		this.subjectExpressionButton = new System.Windows.Forms.Button();
		this.txtSubject = new System.Windows.Forms.TextBox();
		this.lblSubject = new System.Windows.Forms.Label();
		this.bodyExpressionButton = new System.Windows.Forms.Button();
		this.txtBody = new System.Windows.Forms.TextBox();
		this.lblBody = new System.Windows.Forms.Label();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.chkBoxIgnoreMissingAttachments = new System.Windows.Forms.CheckBox();
		this.serverPortExpressionButton = new System.Windows.Forms.Button();
		this.lblServerPort = new System.Windows.Forms.Label();
		this.txtServerPort = new System.Windows.Forms.TextBox();
		this.enableSslExpressionButton = new System.Windows.Forms.Button();
		this.lblEnableSSL = new System.Windows.Forms.Label();
		this.txtEnableSSL = new System.Windows.Forms.TextBox();
		this.chkBoxIsBodyHtml = new System.Windows.Forms.CheckBox();
		this.chkBoxUseServerSettings = new System.Windows.Forms.CheckBox();
		((System.ComponentModel.ISupportInitialize)this.attachmentsGrid).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.attachmentBindingSource).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.lblServer.AutoSize = true;
		this.lblServer.Location = new System.Drawing.Point(16, 47);
		this.lblServer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblServer.Name = "lblServer";
		this.lblServer.Size = new System.Drawing.Size(92, 17);
		this.lblServer.TabIndex = 1;
		this.lblServer.Text = "SMTP Server";
		this.lblPriority.AutoSize = true;
		this.lblPriority.Location = new System.Drawing.Point(16, 425);
		this.lblPriority.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblPriority.Name = "lblPriority";
		this.lblPriority.Size = new System.Drawing.Size(52, 17);
		this.lblPriority.TabIndex = 35;
		this.lblPriority.Text = "Priority";
		this.comboPriority.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboPriority.FormattingEnabled = true;
		this.comboPriority.Location = new System.Drawing.Point(119, 421);
		this.comboPriority.Margin = new System.Windows.Forms.Padding(4);
		this.comboPriority.Name = "comboPriority";
		this.comboPriority.Size = new System.Drawing.Size(431, 24);
		this.comboPriority.TabIndex = 36;
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(497, 649);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 41;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(389, 649);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 40;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.txtServer.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtServer.Location = new System.Drawing.Point(119, 43);
		this.txtServer.Margin = new System.Windows.Forms.Padding(4);
		this.txtServer.MaxLength = 8192;
		this.txtServer.Name = "txtServer";
		this.txtServer.Size = new System.Drawing.Size(431, 22);
		this.txtServer.TabIndex = 2;
		this.txtServer.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtServer.Validating += new System.ComponentModel.CancelEventHandler(TxtServer_Validating);
		this.lblAttachments.AutoSize = true;
		this.lblAttachments.Location = new System.Drawing.Point(17, 468);
		this.lblAttachments.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblAttachments.Name = "lblAttachments";
		this.lblAttachments.Size = new System.Drawing.Size(86, 17);
		this.lblAttachments.TabIndex = 37;
		this.lblAttachments.Text = "Attachments";
		this.attachmentsGrid.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.attachmentsGrid.AutoGenerateColumns = false;
		this.attachmentsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
		this.attachmentsGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
		this.attachmentsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.attachmentsGrid.Columns.AddRange(this.nameDataGridViewTextBoxColumn, this.File, this.expressionBuilderColumn);
		this.attachmentsGrid.DataSource = this.attachmentBindingSource;
		this.attachmentsGrid.Location = new System.Drawing.Point(17, 489);
		this.attachmentsGrid.Margin = new System.Windows.Forms.Padding(4);
		this.attachmentsGrid.Name = "attachmentsGrid";
		this.attachmentsGrid.RowHeadersWidth = 51;
		this.attachmentsGrid.Size = new System.Drawing.Size(581, 112);
		this.attachmentsGrid.TabIndex = 38;
		this.attachmentsGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(AttachmentsGrid_CellClick);
		this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
		this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
		this.nameDataGridViewTextBoxColumn.MaxInputLength = 256;
		this.nameDataGridViewTextBoxColumn.MinimumWidth = 6;
		this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
		this.File.DataPropertyName = "File";
		this.File.HeaderText = "File";
		this.File.MaxInputLength = 1024;
		this.File.MinimumWidth = 6;
		this.File.Name = "File";
		dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle.NullValue = "...";
		this.expressionBuilderColumn.DefaultCellStyle = dataGridViewCellStyle;
		this.expressionBuilderColumn.FillWeight = 20f;
		this.expressionBuilderColumn.HeaderText = "";
		this.expressionBuilderColumn.MinimumWidth = 6;
		this.expressionBuilderColumn.Name = "expressionBuilderColumn";
		this.expressionBuilderColumn.Text = "";
		this.attachmentBindingSource.DataSource = typeof(TCX.CFD.Classes.Components.MailAttachment);
		this.serverExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.serverExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.serverExpressionButton.Location = new System.Drawing.Point(559, 41);
		this.serverExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.serverExpressionButton.Name = "serverExpressionButton";
		this.serverExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.serverExpressionButton.TabIndex = 3;
		this.serverExpressionButton.UseVisualStyleBackColor = true;
		this.serverExpressionButton.Click += new System.EventHandler(ServerExpressionButton_Click);
		this.userNameExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.userNameExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.userNameExpressionButton.Location = new System.Drawing.Point(559, 137);
		this.userNameExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.userNameExpressionButton.Name = "userNameExpressionButton";
		this.userNameExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.userNameExpressionButton.TabIndex = 12;
		this.userNameExpressionButton.UseVisualStyleBackColor = true;
		this.userNameExpressionButton.Click += new System.EventHandler(UserNameExpressionButton_Click);
		this.txtUserName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtUserName.Location = new System.Drawing.Point(119, 139);
		this.txtUserName.Margin = new System.Windows.Forms.Padding(4);
		this.txtUserName.MaxLength = 8192;
		this.txtUserName.Name = "txtUserName";
		this.txtUserName.Size = new System.Drawing.Size(431, 22);
		this.txtUserName.TabIndex = 11;
		this.txtUserName.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtUserName.Validating += new System.ComponentModel.CancelEventHandler(TxtUserName_Validating);
		this.lblUserName.AutoSize = true;
		this.lblUserName.Location = new System.Drawing.Point(16, 143);
		this.lblUserName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblUserName.Name = "lblUserName";
		this.lblUserName.Size = new System.Drawing.Size(79, 17);
		this.lblUserName.TabIndex = 10;
		this.lblUserName.Text = "User Name";
		this.passwordExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.passwordExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.passwordExpressionButton.Location = new System.Drawing.Point(559, 169);
		this.passwordExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.passwordExpressionButton.Name = "passwordExpressionButton";
		this.passwordExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.passwordExpressionButton.TabIndex = 15;
		this.passwordExpressionButton.UseVisualStyleBackColor = true;
		this.passwordExpressionButton.Click += new System.EventHandler(PasswordExpressionButton_Click);
		this.txtPassword.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtPassword.Location = new System.Drawing.Point(119, 171);
		this.txtPassword.Margin = new System.Windows.Forms.Padding(4);
		this.txtPassword.MaxLength = 8192;
		this.txtPassword.Name = "txtPassword";
		this.txtPassword.Size = new System.Drawing.Size(431, 22);
		this.txtPassword.TabIndex = 14;
		this.txtPassword.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtPassword.Validating += new System.ComponentModel.CancelEventHandler(TxtPassword_Validating);
		this.lblPassword.AutoSize = true;
		this.lblPassword.Location = new System.Drawing.Point(16, 175);
		this.lblPassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblPassword.Name = "lblPassword";
		this.lblPassword.Size = new System.Drawing.Size(69, 17);
		this.lblPassword.TabIndex = 13;
		this.lblPassword.Text = "Password";
		this.fromExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.fromExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.fromExpressionButton.Location = new System.Drawing.Point(559, 201);
		this.fromExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.fromExpressionButton.Name = "fromExpressionButton";
		this.fromExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.fromExpressionButton.TabIndex = 18;
		this.fromExpressionButton.UseVisualStyleBackColor = true;
		this.fromExpressionButton.Click += new System.EventHandler(FromExpressionButton_Click);
		this.txtFrom.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtFrom.Location = new System.Drawing.Point(119, 203);
		this.txtFrom.Margin = new System.Windows.Forms.Padding(4);
		this.txtFrom.MaxLength = 8192;
		this.txtFrom.Name = "txtFrom";
		this.txtFrom.Size = new System.Drawing.Size(431, 22);
		this.txtFrom.TabIndex = 17;
		this.txtFrom.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtFrom.Validating += new System.ComponentModel.CancelEventHandler(TxtFrom_Validating);
		this.lblFrom.AutoSize = true;
		this.lblFrom.Location = new System.Drawing.Point(16, 207);
		this.lblFrom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblFrom.Name = "lblFrom";
		this.lblFrom.Size = new System.Drawing.Size(40, 17);
		this.lblFrom.TabIndex = 16;
		this.lblFrom.Text = "From";
		this.toExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.toExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.toExpressionButton.Location = new System.Drawing.Point(559, 233);
		this.toExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.toExpressionButton.Name = "toExpressionButton";
		this.toExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.toExpressionButton.TabIndex = 21;
		this.toExpressionButton.UseVisualStyleBackColor = true;
		this.toExpressionButton.Click += new System.EventHandler(ToExpressionButton_Click);
		this.txtTo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtTo.Location = new System.Drawing.Point(119, 235);
		this.txtTo.Margin = new System.Windows.Forms.Padding(4);
		this.txtTo.MaxLength = 8192;
		this.txtTo.Name = "txtTo";
		this.txtTo.Size = new System.Drawing.Size(431, 22);
		this.txtTo.TabIndex = 20;
		this.txtTo.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtTo.Validating += new System.ComponentModel.CancelEventHandler(TxtDestination_Validating);
		this.lblTo.AutoSize = true;
		this.lblTo.Location = new System.Drawing.Point(16, 239);
		this.lblTo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblTo.Name = "lblTo";
		this.lblTo.Size = new System.Drawing.Size(25, 17);
		this.lblTo.TabIndex = 19;
		this.lblTo.Text = "To";
		this.ccExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ccExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.ccExpressionButton.Location = new System.Drawing.Point(559, 265);
		this.ccExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.ccExpressionButton.Name = "ccExpressionButton";
		this.ccExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.ccExpressionButton.TabIndex = 24;
		this.ccExpressionButton.UseVisualStyleBackColor = true;
		this.ccExpressionButton.Click += new System.EventHandler(CcExpressionButton_Click);
		this.txtCC.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtCC.Location = new System.Drawing.Point(119, 267);
		this.txtCC.Margin = new System.Windows.Forms.Padding(4);
		this.txtCC.MaxLength = 8192;
		this.txtCC.Name = "txtCC";
		this.txtCC.Size = new System.Drawing.Size(431, 22);
		this.txtCC.TabIndex = 23;
		this.txtCC.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtCC.Validating += new System.ComponentModel.CancelEventHandler(TxtDestination_Validating);
		this.lblCC.AutoSize = true;
		this.lblCC.Location = new System.Drawing.Point(16, 271);
		this.lblCC.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblCC.Name = "lblCC";
		this.lblCC.Size = new System.Drawing.Size(26, 17);
		this.lblCC.TabIndex = 22;
		this.lblCC.Text = "CC";
		this.bccExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.bccExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.bccExpressionButton.Location = new System.Drawing.Point(559, 297);
		this.bccExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.bccExpressionButton.Name = "bccExpressionButton";
		this.bccExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.bccExpressionButton.TabIndex = 27;
		this.bccExpressionButton.UseVisualStyleBackColor = true;
		this.bccExpressionButton.Click += new System.EventHandler(BccExpressionButton_Click);
		this.txtBCC.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtBCC.Location = new System.Drawing.Point(119, 299);
		this.txtBCC.Margin = new System.Windows.Forms.Padding(4);
		this.txtBCC.MaxLength = 8192;
		this.txtBCC.Name = "txtBCC";
		this.txtBCC.Size = new System.Drawing.Size(431, 22);
		this.txtBCC.TabIndex = 26;
		this.txtBCC.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtBCC.Validating += new System.ComponentModel.CancelEventHandler(TxtDestination_Validating);
		this.lblBCC.AutoSize = true;
		this.lblBCC.Location = new System.Drawing.Point(16, 303);
		this.lblBCC.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblBCC.Name = "lblBCC";
		this.lblBCC.Size = new System.Drawing.Size(35, 17);
		this.lblBCC.TabIndex = 25;
		this.lblBCC.Text = "BCC";
		this.subjectExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.subjectExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.subjectExpressionButton.Location = new System.Drawing.Point(559, 329);
		this.subjectExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.subjectExpressionButton.Name = "subjectExpressionButton";
		this.subjectExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.subjectExpressionButton.TabIndex = 30;
		this.subjectExpressionButton.UseVisualStyleBackColor = true;
		this.subjectExpressionButton.Click += new System.EventHandler(SubjectExpressionButton_Click);
		this.txtSubject.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtSubject.Location = new System.Drawing.Point(119, 331);
		this.txtSubject.Margin = new System.Windows.Forms.Padding(4);
		this.txtSubject.MaxLength = 8192;
		this.txtSubject.Name = "txtSubject";
		this.txtSubject.Size = new System.Drawing.Size(431, 22);
		this.txtSubject.TabIndex = 29;
		this.txtSubject.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtSubject.Validating += new System.ComponentModel.CancelEventHandler(TxtSubject_Validating);
		this.lblSubject.AutoSize = true;
		this.lblSubject.Location = new System.Drawing.Point(16, 335);
		this.lblSubject.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblSubject.Name = "lblSubject";
		this.lblSubject.Size = new System.Drawing.Size(55, 17);
		this.lblSubject.TabIndex = 28;
		this.lblSubject.Text = "Subject";
		this.bodyExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.bodyExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.bodyExpressionButton.Location = new System.Drawing.Point(559, 361);
		this.bodyExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.bodyExpressionButton.Name = "bodyExpressionButton";
		this.bodyExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.bodyExpressionButton.TabIndex = 33;
		this.bodyExpressionButton.UseVisualStyleBackColor = true;
		this.bodyExpressionButton.Click += new System.EventHandler(BodyExpressionButton_Click);
		this.txtBody.AcceptsReturn = true;
		this.txtBody.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtBody.Location = new System.Drawing.Point(119, 363);
		this.txtBody.Margin = new System.Windows.Forms.Padding(4);
		this.txtBody.MaxLength = 8192;
		this.txtBody.Name = "txtBody";
		this.txtBody.ScrollBars = System.Windows.Forms.ScrollBars.Both;
		this.txtBody.Size = new System.Drawing.Size(431, 22);
		this.txtBody.TabIndex = 32;
		this.txtBody.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtBody.Validating += new System.ComponentModel.CancelEventHandler(TxtBody_Validating);
		this.lblBody.AutoSize = true;
		this.lblBody.Location = new System.Drawing.Point(16, 367);
		this.lblBody.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblBody.Name = "lblBody";
		this.lblBody.Size = new System.Drawing.Size(40, 17);
		this.lblBody.TabIndex = 31;
		this.lblBody.Text = "Body";
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.chkBoxIgnoreMissingAttachments.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.chkBoxIgnoreMissingAttachments.AutoSize = true;
		this.chkBoxIgnoreMissingAttachments.Location = new System.Drawing.Point(17, 609);
		this.chkBoxIgnoreMissingAttachments.Margin = new System.Windows.Forms.Padding(4);
		this.chkBoxIgnoreMissingAttachments.Name = "chkBoxIgnoreMissingAttachments";
		this.chkBoxIgnoreMissingAttachments.Size = new System.Drawing.Size(203, 21);
		this.chkBoxIgnoreMissingAttachments.TabIndex = 39;
		this.chkBoxIgnoreMissingAttachments.Text = "Ignore Missing Attachments";
		this.chkBoxIgnoreMissingAttachments.UseVisualStyleBackColor = true;
		this.serverPortExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.serverPortExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.serverPortExpressionButton.Location = new System.Drawing.Point(559, 73);
		this.serverPortExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.serverPortExpressionButton.Name = "serverPortExpressionButton";
		this.serverPortExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.serverPortExpressionButton.TabIndex = 6;
		this.serverPortExpressionButton.UseVisualStyleBackColor = true;
		this.serverPortExpressionButton.Click += new System.EventHandler(ServerPortExpressionButton_Click);
		this.lblServerPort.AutoSize = true;
		this.lblServerPort.Location = new System.Drawing.Point(16, 79);
		this.lblServerPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblServerPort.Name = "lblServerPort";
		this.lblServerPort.Size = new System.Drawing.Size(80, 17);
		this.lblServerPort.TabIndex = 4;
		this.lblServerPort.Text = "Server Port";
		this.txtServerPort.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtServerPort.Location = new System.Drawing.Point(119, 75);
		this.txtServerPort.Margin = new System.Windows.Forms.Padding(4);
		this.txtServerPort.MaxLength = 8192;
		this.txtServerPort.Name = "txtServerPort";
		this.txtServerPort.Size = new System.Drawing.Size(431, 22);
		this.txtServerPort.TabIndex = 5;
		this.txtServerPort.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtServerPort.Validating += new System.ComponentModel.CancelEventHandler(TxtServerPort_Validating);
		this.enableSslExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.enableSslExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.enableSslExpressionButton.Location = new System.Drawing.Point(559, 105);
		this.enableSslExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.enableSslExpressionButton.Name = "enableSslExpressionButton";
		this.enableSslExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.enableSslExpressionButton.TabIndex = 9;
		this.enableSslExpressionButton.UseVisualStyleBackColor = true;
		this.enableSslExpressionButton.Click += new System.EventHandler(EnableSslExpressionButton_Click);
		this.lblEnableSSL.AutoSize = true;
		this.lblEnableSSL.Location = new System.Drawing.Point(16, 111);
		this.lblEnableSSL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblEnableSSL.Name = "lblEnableSSL";
		this.lblEnableSSL.Size = new System.Drawing.Size(82, 17);
		this.lblEnableSSL.TabIndex = 7;
		this.lblEnableSSL.Text = "Enable SSL";
		this.txtEnableSSL.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtEnableSSL.Location = new System.Drawing.Point(119, 107);
		this.txtEnableSSL.Margin = new System.Windows.Forms.Padding(4);
		this.txtEnableSSL.MaxLength = 8192;
		this.txtEnableSSL.Name = "txtEnableSSL";
		this.txtEnableSSL.Size = new System.Drawing.Size(431, 22);
		this.txtEnableSSL.TabIndex = 8;
		this.txtEnableSSL.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtEnableSSL.Validating += new System.ComponentModel.CancelEventHandler(TxtEnableSSL_Validating);
		this.chkBoxIsBodyHtml.AutoSize = true;
		this.chkBoxIsBodyHtml.Location = new System.Drawing.Point(119, 392);
		this.chkBoxIsBodyHtml.Margin = new System.Windows.Forms.Padding(4);
		this.chkBoxIsBodyHtml.Name = "chkBoxIsBodyHtml";
		this.chkBoxIsBodyHtml.Size = new System.Drawing.Size(118, 21);
		this.chkBoxIsBodyHtml.TabIndex = 34;
		this.chkBoxIsBodyHtml.Text = "Body is HTML";
		this.chkBoxIsBodyHtml.UseVisualStyleBackColor = true;
		this.chkBoxUseServerSettings.AutoSize = true;
		this.chkBoxUseServerSettings.Location = new System.Drawing.Point(20, 15);
		this.chkBoxUseServerSettings.Margin = new System.Windows.Forms.Padding(4);
		this.chkBoxUseServerSettings.Name = "chkBoxUseServerSettings";
		this.chkBoxUseServerSettings.Size = new System.Drawing.Size(257, 21);
		this.chkBoxUseServerSettings.TabIndex = 0;
		this.chkBoxUseServerSettings.Text = "Use 3CX Server connection settings";
		this.chkBoxUseServerSettings.UseVisualStyleBackColor = true;
		this.chkBoxUseServerSettings.CheckedChanged += new System.EventHandler(ChkBoxUseServerSettings_CheckedChanged);
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(619, 692);
		base.Controls.Add(this.chkBoxUseServerSettings);
		base.Controls.Add(this.chkBoxIsBodyHtml);
		base.Controls.Add(this.enableSslExpressionButton);
		base.Controls.Add(this.lblEnableSSL);
		base.Controls.Add(this.txtEnableSSL);
		base.Controls.Add(this.serverPortExpressionButton);
		base.Controls.Add(this.lblServerPort);
		base.Controls.Add(this.txtServerPort);
		base.Controls.Add(this.chkBoxIgnoreMissingAttachments);
		base.Controls.Add(this.bodyExpressionButton);
		base.Controls.Add(this.txtBody);
		base.Controls.Add(this.lblBody);
		base.Controls.Add(this.subjectExpressionButton);
		base.Controls.Add(this.txtSubject);
		base.Controls.Add(this.lblSubject);
		base.Controls.Add(this.bccExpressionButton);
		base.Controls.Add(this.txtBCC);
		base.Controls.Add(this.lblBCC);
		base.Controls.Add(this.ccExpressionButton);
		base.Controls.Add(this.txtCC);
		base.Controls.Add(this.lblCC);
		base.Controls.Add(this.toExpressionButton);
		base.Controls.Add(this.txtTo);
		base.Controls.Add(this.lblTo);
		base.Controls.Add(this.fromExpressionButton);
		base.Controls.Add(this.txtFrom);
		base.Controls.Add(this.lblFrom);
		base.Controls.Add(this.passwordExpressionButton);
		base.Controls.Add(this.txtPassword);
		base.Controls.Add(this.lblPassword);
		base.Controls.Add(this.userNameExpressionButton);
		base.Controls.Add(this.txtUserName);
		base.Controls.Add(this.lblUserName);
		base.Controls.Add(this.serverExpressionButton);
		base.Controls.Add(this.attachmentsGrid);
		base.Controls.Add(this.lblAttachments);
		base.Controls.Add(this.txtServer);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.Controls.Add(this.comboPriority);
		base.Controls.Add(this.lblPriority);
		base.Controls.Add(this.lblServer);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(1359, 875);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(637, 739);
		base.Name = "EMailSenderConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "EMail Sender";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(EMailSenderConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(EMailSenderConfigurationForm_HelpRequested);
		((System.ComponentModel.ISupportInitialize)this.attachmentsGrid).EndInit();
		((System.ComponentModel.ISupportInitialize)this.attachmentBindingSource).EndInit();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
