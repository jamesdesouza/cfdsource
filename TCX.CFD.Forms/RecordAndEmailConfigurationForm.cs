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

public class RecordAndEmailConfigurationForm : Form
{
	private readonly RecordAndEmailComponent recordAndEmailComponent;

	private readonly List<string> validVariables;

	private IContainer components;

	private Label lblServer;

	private Label lblPriority;

	private ComboBox comboPriority;

	private Button cancelButton;

	private Button okButton;

	private TextBox txtServer;

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

	private ErrorProvider errorProvider;

	private Button serverPortExpressionButton;

	private Label lblServerPort;

	private TextBox txtServerPort;

	private Button enableSslExpressionButton;

	private Label lblEnableSSL;

	private TextBox txtEnableSSL;

	private CheckBox chkBoxIsBodyHtml;

	private CheckBox chkBoxUseServerSettings;

	private GroupBox grpBoxRecording;

	private Button editPromptsButton;

	private Label lblPrompts;

	private CheckBox chkBeep;

	private CheckBox chkBoxTerminateByDtmf;

	private MaskedTextBox txtMaxTime;

	private Label lblMaxTime;

	private GroupBox grpBoxEmail;

	public List<Prompt> Prompts { get; private set; }

	public bool Beep => chkBeep.Checked;

	public uint MaxTime
	{
		get
		{
			if (!string.IsNullOrEmpty(txtMaxTime.Text))
			{
				return Convert.ToUInt32(txtMaxTime.Text);
			}
			return Settings.Default.RecordTemplateMaxTime;
		}
	}

	public bool TerminateByDtmf => chkBoxTerminateByDtmf.Checked;

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

	public RecordAndEmailConfigurationForm(RecordAndEmailComponent recordAndEmailComponent)
	{
		InitializeComponent();
		this.recordAndEmailComponent = recordAndEmailComponent;
		validVariables = ExpressionHelper.GetValidVariables(recordAndEmailComponent);
		Prompts = new List<Prompt>(recordAndEmailComponent.Prompts);
		chkBeep.Checked = recordAndEmailComponent.Beep;
		txtMaxTime.Text = recordAndEmailComponent.MaxTime.ToString();
		chkBoxTerminateByDtmf.Checked = recordAndEmailComponent.TerminateByDtmf;
		TxtMaxTime_Validating(txtMaxTime, new CancelEventArgs());
		comboPriority.Items.AddRange(new object[3]
		{
			MailPriority.Low,
			MailPriority.Normal,
			MailPriority.High
		});
		chkBoxUseServerSettings.Checked = recordAndEmailComponent.UseServerSettings;
		txtServer.Text = recordAndEmailComponent.Server;
		txtServerPort.Text = recordAndEmailComponent.Port;
		txtEnableSSL.Text = recordAndEmailComponent.EnableSSL;
		txtUserName.Text = recordAndEmailComponent.UserName;
		txtPassword.Text = recordAndEmailComponent.Password;
		txtFrom.Text = recordAndEmailComponent.From;
		txtTo.Text = recordAndEmailComponent.To;
		txtCC.Text = recordAndEmailComponent.CC;
		txtBCC.Text = recordAndEmailComponent.BCC;
		txtSubject.Text = recordAndEmailComponent.Subject;
		chkBoxIsBodyHtml.Checked = recordAndEmailComponent.IsBodyHtml;
		txtBody.Text = recordAndEmailComponent.Body;
		comboPriority.SelectedItem = recordAndEmailComponent.Priority;
		TxtServer_Validating(txtServer, new CancelEventArgs());
		TxtServerPort_Validating(txtServerPort, new CancelEventArgs());
		TxtEnableSSL_Validating(txtEnableSSL, new CancelEventArgs());
		TxtUserName_Validating(txtUserName, new CancelEventArgs());
		TxtPassword_Validating(txtPassword, new CancelEventArgs());
		TxtFrom_Validating(txtFrom, new CancelEventArgs());
		TxtDestination_Validating(txtTo, new CancelEventArgs());
		TxtSubject_Validating(txtSubject, new CancelEventArgs());
		TxtBody_Validating(txtBody, new CancelEventArgs());
		Text = LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.Title");
		grpBoxRecording.Text = LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.grpBoxRecording.Text");
		lblPrompts.Text = LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.lblPrompts.Text");
		editPromptsButton.Text = LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.editPromptsButton.Text");
		chkBeep.Text = LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.chkBeep.Text");
		lblMaxTime.Text = LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.lblMaxTime.Text");
		chkBoxTerminateByDtmf.Text = LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.chkBoxTerminateByDtmf.Text");
		grpBoxEmail.Text = LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.grpBoxEmail.Text");
		chkBoxUseServerSettings.Text = LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.chkBoxUseServerSettings.Text");
		lblServer.Text = LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.lblServer.Text");
		lblServerPort.Text = LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.lblServerPort.Text");
		lblEnableSSL.Text = LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.lblEnableSSL.Text");
		lblUserName.Text = LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.lblUserName.Text");
		lblPassword.Text = LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.lblPassword.Text");
		lblFrom.Text = LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.lblFrom.Text");
		lblTo.Text = LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.lblTo.Text");
		lblCC.Text = LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.lblCC.Text");
		lblBCC.Text = LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.lblBCC.Text");
		lblSubject.Text = LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.lblSubject.Text");
		chkBoxIsBodyHtml.Text = LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.chkBoxIsBodyHtml.Text");
		lblBody.Text = LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.lblBody.Text");
		lblPriority.Text = LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.lblPriority.Text");
		okButton.Text = LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.cancelButton.Text");
	}

	private void EditPromptsButton_Click(object sender, EventArgs e)
	{
		PromptCollectionEditorForm promptCollectionEditorForm = new PromptCollectionEditorForm(recordAndEmailComponent);
		promptCollectionEditorForm.PromptList = Prompts;
		if (promptCollectionEditorForm.ShowDialog() == DialogResult.OK)
		{
			Prompts = promptCollectionEditorForm.PromptList;
		}
	}

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void MaskedTextBox_GotFocus(object sender, EventArgs e)
	{
		(sender as MaskedTextBox).SelectAll();
	}

	private void TxtMaxTime_Validating(object sender, CancelEventArgs e)
	{
		int result;
		if (string.IsNullOrEmpty(txtMaxTime.Text))
		{
			errorProvider.SetError(txtMaxTime, LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.Error.MaxTimeIsMandatory"));
		}
		else if (!int.TryParse(txtMaxTime.Text, out result) || result < 1 || result > 99999)
		{
			errorProvider.SetError(txtMaxTime, string.Format(LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.Error.InvalidMaxTimeValue"), 1, 99999));
		}
		else
		{
			errorProvider.SetError(txtMaxTime, string.Empty);
		}
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

	private void TxtServer_Validating(object sender, CancelEventArgs e)
	{
		if (txtServer.Enabled)
		{
			if (string.IsNullOrEmpty(txtServer.Text))
			{
				errorProvider.SetError(serverExpressionButton, LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.Error.ServerIsMandatory"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, txtServer.Text).IsSafeExpression())
			{
				errorProvider.SetError(serverExpressionButton, LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.Error.ServerIsInvalid"));
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
				errorProvider.SetError(serverPortExpressionButton, LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.Error.ServerPortIsInvalid"));
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
				errorProvider.SetError(enableSslExpressionButton, LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.Error.EnableSslIsMandatory"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, txtEnableSSL.Text).IsSafeExpression())
			{
				errorProvider.SetError(enableSslExpressionButton, LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.Error.EnableSslIsInvalid"));
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
				errorProvider.SetError(userNameExpressionButton, LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.Error.UserNameIsInvalid"));
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
				errorProvider.SetError(passwordExpressionButton, LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.Error.PasswordIsInvalid"));
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
				errorProvider.SetError(fromExpressionButton, LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.Error.FromIsMandatory"));
			}
			else if (txtFrom.Text.Length > 2 && ExpressionHelper.IsStringLiteral(txtFrom.Text) && !EMailValidator.IsEmail(txtFrom.Text.Substring(1, txtFrom.Text.Length - 2)))
			{
				errorProvider.SetError(fromExpressionButton, LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.Error.FromIsInvalid"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, txtFrom.Text).IsSafeExpression())
			{
				errorProvider.SetError(fromExpressionButton, LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.Error.FromIsInvalidExpression"));
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
			errorProvider.SetError(toExpressionButton, LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.Error.ToIsInvalid"));
		}
		else if (!AbsArgument.BuildArgument(validVariables, txtTo.Text).IsSafeExpression())
		{
			errorProvider.SetError(toExpressionButton, LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.Error.ToIsInvalidExpression"));
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
			errorProvider.SetError(ccExpressionButton, LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.Error.CCIsInvalid"));
		}
		else if (!AbsArgument.BuildArgument(validVariables, txtCC.Text).IsSafeExpression())
		{
			errorProvider.SetError(ccExpressionButton, LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.Error.CCIsInvalidExpression"));
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
			errorProvider.SetError(bccExpressionButton, LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.Error.BCCIsInvalid"));
		}
		else if (!AbsArgument.BuildArgument(validVariables, txtBCC.Text).IsSafeExpression())
		{
			errorProvider.SetError(bccExpressionButton, LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.Error.BCCIsInvalidExpression"));
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
			errorProvider.SetError(subjectExpressionButton, string.IsNullOrEmpty(txtBody.Text) ? LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.Error.SubjectOrBodyIsMandatory") : string.Empty);
		}
		else if (!AbsArgument.BuildArgument(validVariables, txtSubject.Text).IsSafeExpression())
		{
			errorProvider.SetError(subjectExpressionButton, LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.Error.SubjectIsInvalid"));
		}
		else
		{
			errorProvider.SetError(subjectExpressionButton, string.Empty);
		}
		if (string.IsNullOrEmpty(txtBody.Text))
		{
			errorProvider.SetError(bodyExpressionButton, string.IsNullOrEmpty(txtSubject.Text) ? LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.Error.SubjectOrBodyIsMandatory") : string.Empty);
		}
		else if (!AbsArgument.BuildArgument(validVariables, txtBody.Text).IsSafeExpression())
		{
			errorProvider.SetError(bodyExpressionButton, LocalizedResourceMgr.GetString("RecordAndEmailConfigurationForm.Error.BodyIsInvalid"));
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
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(recordAndEmailComponent);
		expressionEditorForm.Expression = txtServer.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtServer.Text = expressionEditorForm.Expression;
			TxtServer_Validating(txtServer, new CancelEventArgs());
		}
	}

	private void ServerPortExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(recordAndEmailComponent);
		expressionEditorForm.Expression = txtServerPort.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtServerPort.Text = expressionEditorForm.Expression;
			TxtServerPort_Validating(txtServerPort, new CancelEventArgs());
		}
	}

	private void EnableSslExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(recordAndEmailComponent);
		expressionEditorForm.Expression = txtEnableSSL.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtEnableSSL.Text = expressionEditorForm.Expression;
			TxtEnableSSL_Validating(txtEnableSSL, new CancelEventArgs());
		}
	}

	private void UserNameExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(recordAndEmailComponent);
		expressionEditorForm.Expression = txtUserName.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtUserName.Text = expressionEditorForm.Expression;
			TxtUserName_Validating(txtUserName, new CancelEventArgs());
		}
	}

	private void PasswordExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(recordAndEmailComponent);
		expressionEditorForm.Expression = txtPassword.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtPassword.Text = expressionEditorForm.Expression;
			TxtPassword_Validating(txtPassword, new CancelEventArgs());
		}
	}

	private void FromExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(recordAndEmailComponent);
		expressionEditorForm.Expression = txtFrom.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtFrom.Text = expressionEditorForm.Expression;
			TxtFrom_Validating(txtFrom, new CancelEventArgs());
		}
	}

	private void ToExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(recordAndEmailComponent);
		expressionEditorForm.Expression = txtTo.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtTo.Text = expressionEditorForm.Expression;
			TxtDestination_Validating(txtTo, new CancelEventArgs());
		}
	}

	private void CcExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(recordAndEmailComponent);
		expressionEditorForm.Expression = txtCC.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtCC.Text = expressionEditorForm.Expression;
			TxtDestination_Validating(txtCC, new CancelEventArgs());
		}
	}

	private void BccExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(recordAndEmailComponent);
		expressionEditorForm.Expression = txtBCC.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtBCC.Text = expressionEditorForm.Expression;
			TxtDestination_Validating(txtBCC, new CancelEventArgs());
		}
	}

	private void SubjectExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(recordAndEmailComponent);
		expressionEditorForm.Expression = txtSubject.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtSubject.Text = expressionEditorForm.Expression;
			TxtSubject_Validating(txtSubject, new CancelEventArgs());
		}
	}

	private void BodyExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(recordAndEmailComponent);
		expressionEditorForm.Expression = txtBody.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtBody.Text = expressionEditorForm.Expression;
			TxtBody_Validating(txtBody, new CancelEventArgs());
		}
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void RecordAndEmailConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		recordAndEmailComponent.ShowHelp();
	}

	private void RecordAndEmailConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		recordAndEmailComponent.ShowHelp();
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
		this.lblServer = new System.Windows.Forms.Label();
		this.lblPriority = new System.Windows.Forms.Label();
		this.comboPriority = new System.Windows.Forms.ComboBox();
		this.cancelButton = new System.Windows.Forms.Button();
		this.okButton = new System.Windows.Forms.Button();
		this.txtServer = new System.Windows.Forms.TextBox();
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
		this.serverPortExpressionButton = new System.Windows.Forms.Button();
		this.lblServerPort = new System.Windows.Forms.Label();
		this.txtServerPort = new System.Windows.Forms.TextBox();
		this.enableSslExpressionButton = new System.Windows.Forms.Button();
		this.lblEnableSSL = new System.Windows.Forms.Label();
		this.txtEnableSSL = new System.Windows.Forms.TextBox();
		this.chkBoxIsBodyHtml = new System.Windows.Forms.CheckBox();
		this.chkBoxUseServerSettings = new System.Windows.Forms.CheckBox();
		this.grpBoxRecording = new System.Windows.Forms.GroupBox();
		this.editPromptsButton = new System.Windows.Forms.Button();
		this.lblPrompts = new System.Windows.Forms.Label();
		this.chkBeep = new System.Windows.Forms.CheckBox();
		this.chkBoxTerminateByDtmf = new System.Windows.Forms.CheckBox();
		this.txtMaxTime = new System.Windows.Forms.MaskedTextBox();
		this.lblMaxTime = new System.Windows.Forms.Label();
		this.grpBoxEmail = new System.Windows.Forms.GroupBox();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		this.grpBoxRecording.SuspendLayout();
		this.grpBoxEmail.SuspendLayout();
		base.SuspendLayout();
		this.lblServer.AutoSize = true;
		this.lblServer.Location = new System.Drawing.Point(7, 55);
		this.lblServer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblServer.Name = "lblServer";
		this.lblServer.Size = new System.Drawing.Size(92, 17);
		this.lblServer.TabIndex = 1;
		this.lblServer.Text = "SMTP Server";
		this.lblPriority.AutoSize = true;
		this.lblPriority.Location = new System.Drawing.Point(7, 434);
		this.lblPriority.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblPriority.Name = "lblPriority";
		this.lblPriority.Size = new System.Drawing.Size(52, 17);
		this.lblPriority.TabIndex = 35;
		this.lblPriority.Text = "Priority";
		this.comboPriority.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboPriority.FormattingEnabled = true;
		this.comboPriority.Location = new System.Drawing.Point(110, 430);
		this.comboPriority.Margin = new System.Windows.Forms.Padding(4);
		this.comboPriority.Name = "comboPriority";
		this.comboPriority.Size = new System.Drawing.Size(409, 24);
		this.comboPriority.TabIndex = 36;
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(497, 654);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 3;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(389, 654);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 2;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.txtServer.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtServer.Location = new System.Drawing.Point(110, 51);
		this.txtServer.Margin = new System.Windows.Forms.Padding(4);
		this.txtServer.MaxLength = 8192;
		this.txtServer.Name = "txtServer";
		this.txtServer.Size = new System.Drawing.Size(409, 22);
		this.txtServer.TabIndex = 2;
		this.txtServer.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtServer.Validating += new System.ComponentModel.CancelEventHandler(TxtServer_Validating);
		this.serverExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.serverExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.serverExpressionButton.Location = new System.Drawing.Point(527, 49);
		this.serverExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.serverExpressionButton.Name = "serverExpressionButton";
		this.serverExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.serverExpressionButton.TabIndex = 3;
		this.serverExpressionButton.UseVisualStyleBackColor = true;
		this.serverExpressionButton.Click += new System.EventHandler(ServerExpressionButton_Click);
		this.userNameExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.userNameExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.userNameExpressionButton.Location = new System.Drawing.Point(527, 144);
		this.userNameExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.userNameExpressionButton.Name = "userNameExpressionButton";
		this.userNameExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.userNameExpressionButton.TabIndex = 12;
		this.userNameExpressionButton.UseVisualStyleBackColor = true;
		this.userNameExpressionButton.Click += new System.EventHandler(UserNameExpressionButton_Click);
		this.txtUserName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtUserName.Location = new System.Drawing.Point(110, 147);
		this.txtUserName.Margin = new System.Windows.Forms.Padding(4);
		this.txtUserName.MaxLength = 8192;
		this.txtUserName.Name = "txtUserName";
		this.txtUserName.Size = new System.Drawing.Size(409, 22);
		this.txtUserName.TabIndex = 11;
		this.txtUserName.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtUserName.Validating += new System.ComponentModel.CancelEventHandler(TxtUserName_Validating);
		this.lblUserName.AutoSize = true;
		this.lblUserName.Location = new System.Drawing.Point(7, 151);
		this.lblUserName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblUserName.Name = "lblUserName";
		this.lblUserName.Size = new System.Drawing.Size(79, 17);
		this.lblUserName.TabIndex = 10;
		this.lblUserName.Text = "User Name";
		this.passwordExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.passwordExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.passwordExpressionButton.Location = new System.Drawing.Point(527, 176);
		this.passwordExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.passwordExpressionButton.Name = "passwordExpressionButton";
		this.passwordExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.passwordExpressionButton.TabIndex = 15;
		this.passwordExpressionButton.UseVisualStyleBackColor = true;
		this.passwordExpressionButton.Click += new System.EventHandler(PasswordExpressionButton_Click);
		this.txtPassword.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtPassword.Location = new System.Drawing.Point(110, 179);
		this.txtPassword.Margin = new System.Windows.Forms.Padding(4);
		this.txtPassword.MaxLength = 8192;
		this.txtPassword.Name = "txtPassword";
		this.txtPassword.Size = new System.Drawing.Size(409, 22);
		this.txtPassword.TabIndex = 14;
		this.txtPassword.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtPassword.Validating += new System.ComponentModel.CancelEventHandler(TxtPassword_Validating);
		this.lblPassword.AutoSize = true;
		this.lblPassword.Location = new System.Drawing.Point(7, 183);
		this.lblPassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblPassword.Name = "lblPassword";
		this.lblPassword.Size = new System.Drawing.Size(69, 17);
		this.lblPassword.TabIndex = 13;
		this.lblPassword.Text = "Password";
		this.fromExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.fromExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.fromExpressionButton.Location = new System.Drawing.Point(527, 208);
		this.fromExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.fromExpressionButton.Name = "fromExpressionButton";
		this.fromExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.fromExpressionButton.TabIndex = 18;
		this.fromExpressionButton.UseVisualStyleBackColor = true;
		this.fromExpressionButton.Click += new System.EventHandler(FromExpressionButton_Click);
		this.txtFrom.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtFrom.Location = new System.Drawing.Point(110, 211);
		this.txtFrom.Margin = new System.Windows.Forms.Padding(4);
		this.txtFrom.MaxLength = 8192;
		this.txtFrom.Name = "txtFrom";
		this.txtFrom.Size = new System.Drawing.Size(409, 22);
		this.txtFrom.TabIndex = 17;
		this.txtFrom.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtFrom.Validating += new System.ComponentModel.CancelEventHandler(TxtFrom_Validating);
		this.lblFrom.AutoSize = true;
		this.lblFrom.Location = new System.Drawing.Point(7, 214);
		this.lblFrom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblFrom.Name = "lblFrom";
		this.lblFrom.Size = new System.Drawing.Size(40, 17);
		this.lblFrom.TabIndex = 16;
		this.lblFrom.Text = "From";
		this.toExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.toExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.toExpressionButton.Location = new System.Drawing.Point(527, 240);
		this.toExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.toExpressionButton.Name = "toExpressionButton";
		this.toExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.toExpressionButton.TabIndex = 21;
		this.toExpressionButton.UseVisualStyleBackColor = true;
		this.toExpressionButton.Click += new System.EventHandler(ToExpressionButton_Click);
		this.txtTo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtTo.Location = new System.Drawing.Point(110, 243);
		this.txtTo.Margin = new System.Windows.Forms.Padding(4);
		this.txtTo.MaxLength = 8192;
		this.txtTo.Name = "txtTo";
		this.txtTo.Size = new System.Drawing.Size(409, 22);
		this.txtTo.TabIndex = 20;
		this.txtTo.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtTo.Validating += new System.ComponentModel.CancelEventHandler(TxtDestination_Validating);
		this.lblTo.AutoSize = true;
		this.lblTo.Location = new System.Drawing.Point(7, 246);
		this.lblTo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblTo.Name = "lblTo";
		this.lblTo.Size = new System.Drawing.Size(25, 17);
		this.lblTo.TabIndex = 19;
		this.lblTo.Text = "To";
		this.ccExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ccExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.ccExpressionButton.Location = new System.Drawing.Point(527, 272);
		this.ccExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.ccExpressionButton.Name = "ccExpressionButton";
		this.ccExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.ccExpressionButton.TabIndex = 24;
		this.ccExpressionButton.UseVisualStyleBackColor = true;
		this.ccExpressionButton.Click += new System.EventHandler(CcExpressionButton_Click);
		this.txtCC.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtCC.Location = new System.Drawing.Point(110, 275);
		this.txtCC.Margin = new System.Windows.Forms.Padding(4);
		this.txtCC.MaxLength = 8192;
		this.txtCC.Name = "txtCC";
		this.txtCC.Size = new System.Drawing.Size(409, 22);
		this.txtCC.TabIndex = 23;
		this.txtCC.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtCC.Validating += new System.ComponentModel.CancelEventHandler(TxtDestination_Validating);
		this.lblCC.AutoSize = true;
		this.lblCC.Location = new System.Drawing.Point(7, 278);
		this.lblCC.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblCC.Name = "lblCC";
		this.lblCC.Size = new System.Drawing.Size(26, 17);
		this.lblCC.TabIndex = 22;
		this.lblCC.Text = "CC";
		this.bccExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.bccExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.bccExpressionButton.Location = new System.Drawing.Point(527, 304);
		this.bccExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.bccExpressionButton.Name = "bccExpressionButton";
		this.bccExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.bccExpressionButton.TabIndex = 27;
		this.bccExpressionButton.UseVisualStyleBackColor = true;
		this.bccExpressionButton.Click += new System.EventHandler(BccExpressionButton_Click);
		this.txtBCC.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtBCC.Location = new System.Drawing.Point(110, 307);
		this.txtBCC.Margin = new System.Windows.Forms.Padding(4);
		this.txtBCC.MaxLength = 8192;
		this.txtBCC.Name = "txtBCC";
		this.txtBCC.Size = new System.Drawing.Size(409, 22);
		this.txtBCC.TabIndex = 26;
		this.txtBCC.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtBCC.Validating += new System.ComponentModel.CancelEventHandler(TxtDestination_Validating);
		this.lblBCC.AutoSize = true;
		this.lblBCC.Location = new System.Drawing.Point(7, 310);
		this.lblBCC.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblBCC.Name = "lblBCC";
		this.lblBCC.Size = new System.Drawing.Size(35, 17);
		this.lblBCC.TabIndex = 25;
		this.lblBCC.Text = "BCC";
		this.subjectExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.subjectExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.subjectExpressionButton.Location = new System.Drawing.Point(527, 336);
		this.subjectExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.subjectExpressionButton.Name = "subjectExpressionButton";
		this.subjectExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.subjectExpressionButton.TabIndex = 30;
		this.subjectExpressionButton.UseVisualStyleBackColor = true;
		this.subjectExpressionButton.Click += new System.EventHandler(SubjectExpressionButton_Click);
		this.txtSubject.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtSubject.Location = new System.Drawing.Point(110, 339);
		this.txtSubject.Margin = new System.Windows.Forms.Padding(4);
		this.txtSubject.MaxLength = 8192;
		this.txtSubject.Name = "txtSubject";
		this.txtSubject.Size = new System.Drawing.Size(409, 22);
		this.txtSubject.TabIndex = 29;
		this.txtSubject.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtSubject.Validating += new System.ComponentModel.CancelEventHandler(TxtSubject_Validating);
		this.lblSubject.AutoSize = true;
		this.lblSubject.Location = new System.Drawing.Point(7, 342);
		this.lblSubject.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblSubject.Name = "lblSubject";
		this.lblSubject.Size = new System.Drawing.Size(55, 17);
		this.lblSubject.TabIndex = 28;
		this.lblSubject.Text = "Subject";
		this.bodyExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.bodyExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.bodyExpressionButton.Location = new System.Drawing.Point(527, 368);
		this.bodyExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.bodyExpressionButton.Name = "bodyExpressionButton";
		this.bodyExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.bodyExpressionButton.TabIndex = 33;
		this.bodyExpressionButton.UseVisualStyleBackColor = true;
		this.bodyExpressionButton.Click += new System.EventHandler(BodyExpressionButton_Click);
		this.txtBody.AcceptsReturn = true;
		this.txtBody.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtBody.Location = new System.Drawing.Point(110, 371);
		this.txtBody.Margin = new System.Windows.Forms.Padding(4);
		this.txtBody.MaxLength = 8192;
		this.txtBody.Name = "txtBody";
		this.txtBody.ScrollBars = System.Windows.Forms.ScrollBars.Both;
		this.txtBody.Size = new System.Drawing.Size(409, 22);
		this.txtBody.TabIndex = 32;
		this.txtBody.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtBody.Validating += new System.ComponentModel.CancelEventHandler(TxtBody_Validating);
		this.lblBody.AutoSize = true;
		this.lblBody.Location = new System.Drawing.Point(7, 374);
		this.lblBody.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblBody.Name = "lblBody";
		this.lblBody.Size = new System.Drawing.Size(40, 17);
		this.lblBody.TabIndex = 31;
		this.lblBody.Text = "Body";
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.serverPortExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.serverPortExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.serverPortExpressionButton.Location = new System.Drawing.Point(527, 80);
		this.serverPortExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.serverPortExpressionButton.Name = "serverPortExpressionButton";
		this.serverPortExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.serverPortExpressionButton.TabIndex = 6;
		this.serverPortExpressionButton.UseVisualStyleBackColor = true;
		this.serverPortExpressionButton.Click += new System.EventHandler(ServerPortExpressionButton_Click);
		this.lblServerPort.AutoSize = true;
		this.lblServerPort.Location = new System.Drawing.Point(7, 87);
		this.lblServerPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblServerPort.Name = "lblServerPort";
		this.lblServerPort.Size = new System.Drawing.Size(80, 17);
		this.lblServerPort.TabIndex = 4;
		this.lblServerPort.Text = "Server Port";
		this.txtServerPort.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtServerPort.Location = new System.Drawing.Point(110, 83);
		this.txtServerPort.Margin = new System.Windows.Forms.Padding(4);
		this.txtServerPort.MaxLength = 8192;
		this.txtServerPort.Name = "txtServerPort";
		this.txtServerPort.Size = new System.Drawing.Size(409, 22);
		this.txtServerPort.TabIndex = 5;
		this.txtServerPort.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtServerPort.Validating += new System.ComponentModel.CancelEventHandler(TxtServerPort_Validating);
		this.enableSslExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.enableSslExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.enableSslExpressionButton.Location = new System.Drawing.Point(527, 112);
		this.enableSslExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.enableSslExpressionButton.Name = "enableSslExpressionButton";
		this.enableSslExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.enableSslExpressionButton.TabIndex = 9;
		this.enableSslExpressionButton.UseVisualStyleBackColor = true;
		this.enableSslExpressionButton.Click += new System.EventHandler(EnableSslExpressionButton_Click);
		this.lblEnableSSL.AutoSize = true;
		this.lblEnableSSL.Location = new System.Drawing.Point(7, 119);
		this.lblEnableSSL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblEnableSSL.Name = "lblEnableSSL";
		this.lblEnableSSL.Size = new System.Drawing.Size(82, 17);
		this.lblEnableSSL.TabIndex = 7;
		this.lblEnableSSL.Text = "Enable SSL";
		this.txtEnableSSL.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtEnableSSL.Location = new System.Drawing.Point(110, 115);
		this.txtEnableSSL.Margin = new System.Windows.Forms.Padding(4);
		this.txtEnableSSL.MaxLength = 8192;
		this.txtEnableSSL.Name = "txtEnableSSL";
		this.txtEnableSSL.Size = new System.Drawing.Size(409, 22);
		this.txtEnableSSL.TabIndex = 8;
		this.txtEnableSSL.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtEnableSSL.Validating += new System.ComponentModel.CancelEventHandler(TxtEnableSSL_Validating);
		this.chkBoxIsBodyHtml.AutoSize = true;
		this.chkBoxIsBodyHtml.Location = new System.Drawing.Point(110, 401);
		this.chkBoxIsBodyHtml.Margin = new System.Windows.Forms.Padding(4);
		this.chkBoxIsBodyHtml.Name = "chkBoxIsBodyHtml";
		this.chkBoxIsBodyHtml.Size = new System.Drawing.Size(118, 21);
		this.chkBoxIsBodyHtml.TabIndex = 34;
		this.chkBoxIsBodyHtml.Text = "Body is HTML";
		this.chkBoxIsBodyHtml.UseVisualStyleBackColor = true;
		this.chkBoxUseServerSettings.AutoSize = true;
		this.chkBoxUseServerSettings.Location = new System.Drawing.Point(7, 22);
		this.chkBoxUseServerSettings.Margin = new System.Windows.Forms.Padding(4);
		this.chkBoxUseServerSettings.Name = "chkBoxUseServerSettings";
		this.chkBoxUseServerSettings.Size = new System.Drawing.Size(257, 21);
		this.chkBoxUseServerSettings.TabIndex = 0;
		this.chkBoxUseServerSettings.Text = "Use 3CX Server connection settings";
		this.chkBoxUseServerSettings.UseVisualStyleBackColor = true;
		this.chkBoxUseServerSettings.CheckedChanged += new System.EventHandler(ChkBoxUseServerSettings_CheckedChanged);
		this.grpBoxRecording.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.grpBoxRecording.Controls.Add(this.editPromptsButton);
		this.grpBoxRecording.Controls.Add(this.lblPrompts);
		this.grpBoxRecording.Controls.Add(this.chkBeep);
		this.grpBoxRecording.Controls.Add(this.chkBoxTerminateByDtmf);
		this.grpBoxRecording.Controls.Add(this.txtMaxTime);
		this.grpBoxRecording.Controls.Add(this.lblMaxTime);
		this.grpBoxRecording.Location = new System.Drawing.Point(12, 12);
		this.grpBoxRecording.Name = "grpBoxRecording";
		this.grpBoxRecording.Size = new System.Drawing.Size(595, 158);
		this.grpBoxRecording.TabIndex = 0;
		this.grpBoxRecording.TabStop = false;
		this.grpBoxRecording.Text = "Recording settings";
		this.editPromptsButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.editPromptsButton.Location = new System.Drawing.Point(230, 24);
		this.editPromptsButton.Margin = new System.Windows.Forms.Padding(4);
		this.editPromptsButton.Name = "editPromptsButton";
		this.editPromptsButton.Size = new System.Drawing.Size(336, 28);
		this.editPromptsButton.TabIndex = 1;
		this.editPromptsButton.Text = "Edit Prompts";
		this.editPromptsButton.UseVisualStyleBackColor = true;
		this.editPromptsButton.Click += new System.EventHandler(EditPromptsButton_Click);
		this.lblPrompts.AutoSize = true;
		this.lblPrompts.Location = new System.Drawing.Point(7, 30);
		this.lblPrompts.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblPrompts.Name = "lblPrompts";
		this.lblPrompts.Size = new System.Drawing.Size(60, 17);
		this.lblPrompts.TabIndex = 0;
		this.lblPrompts.Text = "Prompts";
		this.chkBeep.AutoSize = true;
		this.chkBeep.Location = new System.Drawing.Point(7, 60);
		this.chkBeep.Margin = new System.Windows.Forms.Padding(4);
		this.chkBeep.Name = "chkBeep";
		this.chkBeep.Size = new System.Drawing.Size(241, 21);
		this.chkBeep.TabIndex = 2;
		this.chkBeep.Text = "Play beep before recording starts";
		this.chkBeep.UseVisualStyleBackColor = true;
		this.chkBoxTerminateByDtmf.AutoSize = true;
		this.chkBoxTerminateByDtmf.Location = new System.Drawing.Point(7, 120);
		this.chkBoxTerminateByDtmf.Margin = new System.Windows.Forms.Padding(4);
		this.chkBoxTerminateByDtmf.Name = "chkBoxTerminateByDtmf";
		this.chkBoxTerminateByDtmf.Size = new System.Drawing.Size(269, 21);
		this.chkBoxTerminateByDtmf.TabIndex = 5;
		this.chkBoxTerminateByDtmf.Text = "Stop recording by pressing any DTMF";
		this.chkBoxTerminateByDtmf.UseVisualStyleBackColor = true;
		this.txtMaxTime.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtMaxTime.HidePromptOnLeave = true;
		this.txtMaxTime.Location = new System.Drawing.Point(230, 88);
		this.txtMaxTime.Margin = new System.Windows.Forms.Padding(4);
		this.txtMaxTime.Mask = "99999";
		this.txtMaxTime.Name = "txtMaxTime";
		this.txtMaxTime.Size = new System.Drawing.Size(336, 22);
		this.txtMaxTime.TabIndex = 4;
		this.txtMaxTime.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtMaxTime.Validating += new System.ComponentModel.CancelEventHandler(TxtMaxTime_Validating);
		this.lblMaxTime.AutoSize = true;
		this.lblMaxTime.Location = new System.Drawing.Point(3, 92);
		this.lblMaxTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblMaxTime.Name = "lblMaxTime";
		this.lblMaxTime.Size = new System.Drawing.Size(220, 17);
		this.lblMaxTime.TabIndex = 3;
		this.lblMaxTime.Text = "Max recording duration (seconds)";
		this.grpBoxEmail.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.grpBoxEmail.Controls.Add(this.chkBoxUseServerSettings);
		this.grpBoxEmail.Controls.Add(this.lblServer);
		this.grpBoxEmail.Controls.Add(this.chkBoxIsBodyHtml);
		this.grpBoxEmail.Controls.Add(this.lblPriority);
		this.grpBoxEmail.Controls.Add(this.enableSslExpressionButton);
		this.grpBoxEmail.Controls.Add(this.comboPriority);
		this.grpBoxEmail.Controls.Add(this.lblEnableSSL);
		this.grpBoxEmail.Controls.Add(this.txtServer);
		this.grpBoxEmail.Controls.Add(this.txtEnableSSL);
		this.grpBoxEmail.Controls.Add(this.serverExpressionButton);
		this.grpBoxEmail.Controls.Add(this.serverPortExpressionButton);
		this.grpBoxEmail.Controls.Add(this.lblUserName);
		this.grpBoxEmail.Controls.Add(this.lblServerPort);
		this.grpBoxEmail.Controls.Add(this.txtUserName);
		this.grpBoxEmail.Controls.Add(this.txtServerPort);
		this.grpBoxEmail.Controls.Add(this.userNameExpressionButton);
		this.grpBoxEmail.Controls.Add(this.bodyExpressionButton);
		this.grpBoxEmail.Controls.Add(this.lblPassword);
		this.grpBoxEmail.Controls.Add(this.txtBody);
		this.grpBoxEmail.Controls.Add(this.txtPassword);
		this.grpBoxEmail.Controls.Add(this.lblBody);
		this.grpBoxEmail.Controls.Add(this.passwordExpressionButton);
		this.grpBoxEmail.Controls.Add(this.subjectExpressionButton);
		this.grpBoxEmail.Controls.Add(this.lblFrom);
		this.grpBoxEmail.Controls.Add(this.txtSubject);
		this.grpBoxEmail.Controls.Add(this.txtFrom);
		this.grpBoxEmail.Controls.Add(this.lblSubject);
		this.grpBoxEmail.Controls.Add(this.fromExpressionButton);
		this.grpBoxEmail.Controls.Add(this.bccExpressionButton);
		this.grpBoxEmail.Controls.Add(this.lblTo);
		this.grpBoxEmail.Controls.Add(this.txtBCC);
		this.grpBoxEmail.Controls.Add(this.txtTo);
		this.grpBoxEmail.Controls.Add(this.lblBCC);
		this.grpBoxEmail.Controls.Add(this.toExpressionButton);
		this.grpBoxEmail.Controls.Add(this.ccExpressionButton);
		this.grpBoxEmail.Controls.Add(this.lblCC);
		this.grpBoxEmail.Controls.Add(this.txtCC);
		this.grpBoxEmail.Location = new System.Drawing.Point(12, 176);
		this.grpBoxEmail.Name = "grpBoxEmail";
		this.grpBoxEmail.Size = new System.Drawing.Size(595, 471);
		this.grpBoxEmail.TabIndex = 1;
		this.grpBoxEmail.TabStop = false;
		this.grpBoxEmail.Text = "Email settings";
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(619, 697);
		base.Controls.Add(this.grpBoxEmail);
		base.Controls.Add(this.grpBoxRecording);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(1359, 744);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(637, 744);
		base.Name = "RecordAndEmailConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Record and Email";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(RecordAndEmailConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(RecordAndEmailConfigurationForm_HelpRequested);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		this.grpBoxRecording.ResumeLayout(false);
		this.grpBoxRecording.PerformLayout();
		this.grpBoxEmail.ResumeLayout(false);
		this.grpBoxEmail.PerformLayout();
		base.ResumeLayout(false);
	}
}
