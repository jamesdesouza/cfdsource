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

public class WebServiceRestConfigurationForm : Form
{
	private readonly WebServiceRestComponent webServiceRestComponent;

	private readonly List<string> validVariables;

	private IContainer components;

	private Label lblURI;

	private Label lblRequestType;

	private ComboBox comboRequestType;

	private MaskedTextBox txtTimeout;

	private Label lblTimeout;

	private Button cancelButton;

	private Button okButton;

	private TextBox txtURI;

	private ErrorProvider errorProvider;

	private Button uriExpressionButton;

	private Label lblContentType;

	private Label lblContent;

	private Button contentExpressionButton;

	private TextBox txtContent;

	private ComboBox comboContentType;

	private DataGridView headersGrid;

	private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn valueDataGridViewTextBoxColumn;

	private DataGridViewButtonColumn expressionBuilderColumn;

	private BindingSource headerBindingSource;

	private Label lblHeaders;

	private GroupBox grpBoxAuthentication;

	private RadioButton rbAuthenticationNone;

	private RadioButton rbAuthenticationBasicApiKey;

	private Button authenticationAccessTokenExpressionButton;

	private Button authenticationApiKeyExpressionButton;

	private Button authenticationPasswordExpressionButton;

	private Label lblAuthenticationApiKey;

	private TextBox txtAuthenticationApiKey;

	private Button authenticationUserNameExpressionButton;

	private Label lblAuthenticationPassword;

	private Label lblAuthenticationUserName;

	private TextBox txtAuthenticationPassword;

	private TextBox txtAuthenticationUserName;

	private RadioButton rbAuthenticationOAuth2;

	private RadioButton rbAuthenticationBasicUserNamePassword;

	private Label lblAuthenticationAccessToken;

	private TextBox txtAuthenticationAccessToken;

	public string URI => txtURI.Text;

	public HttpRequestTypes RequestType => (HttpRequestTypes)comboRequestType.SelectedItem;

	public string ContentType => comboContentType.Text;

	public string Content => txtContent.Text;

	public WebServiceAuthenticationTypes AuthenticationType
	{
		get
		{
			if (!rbAuthenticationNone.Checked)
			{
				if (!rbAuthenticationBasicUserNamePassword.Checked)
				{
					if (!rbAuthenticationBasicApiKey.Checked)
					{
						return WebServiceAuthenticationTypes.OAuth2;
					}
					return WebServiceAuthenticationTypes.BasicApiKey;
				}
				return WebServiceAuthenticationTypes.BasicUserPassword;
			}
			return WebServiceAuthenticationTypes.None;
		}
	}

	public string AuthenticationUserName => txtAuthenticationUserName.Text;

	public string AuthenticationPassword => txtAuthenticationPassword.Text;

	public string AuthenticationApiKey => txtAuthenticationApiKey.Text;

	public string AuthenticationOAuth2AccessToken => txtAuthenticationAccessToken.Text;

	public uint Timeout
	{
		get
		{
			if (!string.IsNullOrEmpty(txtTimeout.Text))
			{
				return Convert.ToUInt32(txtTimeout.Text);
			}
			return Settings.Default.WebServiceRestTemplateTimeout;
		}
	}

	public List<Parameter> Headers
	{
		get
		{
			List<Parameter> list = new List<Parameter>();
			foreach (Parameter item in headerBindingSource.List)
			{
				list.Add(item);
			}
			return list;
		}
	}

	private void FillComboContentType()
	{
		comboContentType.Items.Clear();
		comboContentType.Items.Add("application/javascript");
		comboContentType.Items.Add("application/json");
		comboContentType.Items.Add("application/x-www-form-urlencoded");
		comboContentType.Items.Add("application/pdf");
		comboContentType.Items.Add("application/xml");
		comboContentType.Items.Add("application/zip");
		comboContentType.Items.Add("multipart/form-data");
		comboContentType.Items.Add("text/css");
		comboContentType.Items.Add("text/html");
		comboContentType.Items.Add("text/plain");
		comboContentType.Items.Add("image/png");
		comboContentType.Items.Add("image/jpeg");
		comboContentType.Items.Add("image/gif");
		if (!comboContentType.Items.Contains(webServiceRestComponent.ContentType))
		{
			comboContentType.Items.Add(webServiceRestComponent.ContentType);
		}
	}

	public WebServiceRestConfigurationForm(WebServiceRestComponent webServiceRestComponent)
	{
		InitializeComponent();
		this.webServiceRestComponent = webServiceRestComponent;
		validVariables = ExpressionHelper.GetValidVariables(webServiceRestComponent);
		comboRequestType.Items.AddRange(new object[7]
		{
			HttpRequestTypes.DELETE,
			HttpRequestTypes.GET,
			HttpRequestTypes.HEAD,
			HttpRequestTypes.OPTIONS,
			HttpRequestTypes.POST,
			HttpRequestTypes.PUT,
			HttpRequestTypes.TRACE
		});
		FillComboContentType();
		txtURI.Text = webServiceRestComponent.URI;
		comboRequestType.SelectedItem = webServiceRestComponent.HttpRequestType;
		comboContentType.SelectedItem = webServiceRestComponent.ContentType;
		txtContent.Text = webServiceRestComponent.Content;
		txtTimeout.Text = webServiceRestComponent.Timeout.ToString();
		rbAuthenticationNone.Checked = webServiceRestComponent.AuthenticationType == WebServiceAuthenticationTypes.None;
		rbAuthenticationBasicUserNamePassword.Checked = webServiceRestComponent.AuthenticationType == WebServiceAuthenticationTypes.BasicUserPassword;
		rbAuthenticationBasicApiKey.Checked = webServiceRestComponent.AuthenticationType == WebServiceAuthenticationTypes.BasicApiKey;
		rbAuthenticationOAuth2.Checked = webServiceRestComponent.AuthenticationType == WebServiceAuthenticationTypes.OAuth2;
		txtAuthenticationUserName.Text = webServiceRestComponent.AuthenticationUserName;
		txtAuthenticationPassword.Text = webServiceRestComponent.AuthenticationPassword;
		txtAuthenticationApiKey.Text = webServiceRestComponent.AuthenticationApiKey;
		txtAuthenticationAccessToken.Text = webServiceRestComponent.AuthenticationOAuth2AccessToken;
		TxtURI_Validating(txtURI, new CancelEventArgs());
		TxtContent_Validating(txtContent, new CancelEventArgs());
		TxtTimeout_Validating(txtTimeout, new CancelEventArgs());
		TxtAuthenticationUserName_Validating(txtAuthenticationUserName, new CancelEventArgs());
		TxtAuthenticationPassword_Validating(txtAuthenticationPassword, new CancelEventArgs());
		TxtAuthenticationApiKey_Validating(txtAuthenticationApiKey, new CancelEventArgs());
		TxtAuthenticationAccessToken_Validating(txtAuthenticationAccessToken, new CancelEventArgs());
		foreach (Parameter header in webServiceRestComponent.Headers)
		{
			headerBindingSource.List.Add(header);
		}
		Text = LocalizedResourceMgr.GetString("WebServiceRestConfigurationForm.Title");
		lblURI.Text = LocalizedResourceMgr.GetString("WebServiceRestConfigurationForm.lblURI.Text");
		lblRequestType.Text = LocalizedResourceMgr.GetString("WebServiceRestConfigurationForm.lblRequestType.Text");
		lblContentType.Text = LocalizedResourceMgr.GetString("WebServiceRestConfigurationForm.lblContentType.Text");
		lblContent.Text = LocalizedResourceMgr.GetString("WebServiceRestConfigurationForm.lblContent.Text");
		lblTimeout.Text = LocalizedResourceMgr.GetString("WebServiceRestConfigurationForm.lblTimeout.Text");
		grpBoxAuthentication.Text = LocalizedResourceMgr.GetString("WebServiceRestConfigurationForm.grpBoxAuthentication.Text");
		rbAuthenticationNone.Text = LocalizedResourceMgr.GetString("WebServiceRestConfigurationForm.rbAuthenticationNone.Text");
		rbAuthenticationBasicUserNamePassword.Text = LocalizedResourceMgr.GetString("WebServiceRestConfigurationForm.rbAuthenticationBasicUserNamePassword.Text");
		rbAuthenticationBasicApiKey.Text = LocalizedResourceMgr.GetString("WebServiceRestConfigurationForm.rbAuthenticationBasicApiKey.Text");
		rbAuthenticationOAuth2.Text = LocalizedResourceMgr.GetString("WebServiceRestConfigurationForm.rbAuthenticationOAuth2.Text");
		lblAuthenticationUserName.Text = LocalizedResourceMgr.GetString("WebServiceRestConfigurationForm.lblAuthenticationUserName.Text");
		lblAuthenticationPassword.Text = LocalizedResourceMgr.GetString("WebServiceRestConfigurationForm.lblAuthenticationPassword.Text");
		lblAuthenticationApiKey.Text = LocalizedResourceMgr.GetString("WebServiceRestConfigurationForm.lblAuthenticationApiKey.Text");
		lblAuthenticationAccessToken.Text = LocalizedResourceMgr.GetString("WebServiceRestConfigurationForm.lblAuthenticationAccessToken.Text");
		lblHeaders.Text = LocalizedResourceMgr.GetString("WebServiceRestConfigurationForm.lblHeaders.Text");
		okButton.Text = LocalizedResourceMgr.GetString("WebServiceRestConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("WebServiceRestConfigurationForm.cancelButton.Text");
	}

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void MaskedTextBox_GotFocus(object sender, EventArgs e)
	{
		(sender as MaskedTextBox).SelectAll();
	}

	private void TxtURI_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtURI.Text))
		{
			errorProvider.SetError(uriExpressionButton, LocalizedResourceMgr.GetString("WebServiceRestConfigurationForm.Error.UriIsMandatory"));
			return;
		}
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtURI.Text);
		errorProvider.SetError(uriExpressionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("WebServiceRestConfigurationForm.Error.UriIsInvalid"));
	}

	private void UriExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(webServiceRestComponent);
		expressionEditorForm.Expression = txtURI.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtURI.Text = expressionEditorForm.Expression;
			TxtURI_Validating(txtURI, new CancelEventArgs());
		}
	}

	private void ComboContentType_TextChanged(object sender, EventArgs e)
	{
		TxtContent_Validating(txtContent, new CancelEventArgs());
	}

	private void TxtContent_Validating(object sender, CancelEventArgs e)
	{
		if ((string.IsNullOrEmpty(txtContent.Text) && !string.IsNullOrEmpty(comboContentType.Text)) || (!string.IsNullOrEmpty(txtContent.Text) && string.IsNullOrEmpty(comboContentType.Text)))
		{
			errorProvider.SetError(contentExpressionButton, LocalizedResourceMgr.GetString("WebServiceRestConfigurationForm.Error.ContentAndContentTypeRequired"));
			return;
		}
		if (string.IsNullOrEmpty(txtContent.Text))
		{
			errorProvider.SetError(contentExpressionButton, string.Empty);
			return;
		}
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtContent.Text);
		errorProvider.SetError(contentExpressionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("WebServiceRestConfigurationForm.Error.ContentIsInvalid"));
	}

	private void ContentExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(webServiceRestComponent);
		expressionEditorForm.Expression = txtContent.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtContent.Text = expressionEditorForm.Expression;
			TxtContent_Validating(txtContent, new CancelEventArgs());
		}
	}

	private void TxtTimeout_Validating(object sender, CancelEventArgs e)
	{
		errorProvider.SetError(txtTimeout, string.IsNullOrEmpty(txtTimeout.Text) ? LocalizedResourceMgr.GetString("WebServiceRestConfigurationForm.Error.TimeoutIsMandatory") : string.Empty);
	}

	private void RbAuthentication_CheckedChanged(object sender, EventArgs e)
	{
		txtAuthenticationUserName.Enabled = rbAuthenticationBasicUserNamePassword.Checked;
		txtAuthenticationPassword.Enabled = rbAuthenticationBasicUserNamePassword.Checked;
		authenticationUserNameExpressionButton.Enabled = rbAuthenticationBasicUserNamePassword.Checked;
		authenticationPasswordExpressionButton.Enabled = rbAuthenticationBasicUserNamePassword.Checked;
		txtAuthenticationApiKey.Enabled = rbAuthenticationBasicApiKey.Checked;
		authenticationApiKeyExpressionButton.Enabled = rbAuthenticationBasicApiKey.Checked;
		txtAuthenticationAccessToken.Enabled = rbAuthenticationOAuth2.Checked;
		authenticationAccessTokenExpressionButton.Enabled = rbAuthenticationOAuth2.Checked;
		TxtAuthenticationUserName_Validating(txtAuthenticationUserName, new CancelEventArgs());
		TxtAuthenticationPassword_Validating(txtAuthenticationPassword, new CancelEventArgs());
		TxtAuthenticationApiKey_Validating(txtAuthenticationApiKey, new CancelEventArgs());
		TxtAuthenticationAccessToken_Validating(txtAuthenticationAccessToken, new CancelEventArgs());
	}

	private void TxtAuthenticationUserName_Validating(object sender, CancelEventArgs e)
	{
		if (rbAuthenticationBasicUserNamePassword.Checked)
		{
			if (string.IsNullOrEmpty(txtAuthenticationUserName.Text))
			{
				errorProvider.SetError(authenticationUserNameExpressionButton, LocalizedResourceMgr.GetString("WebServiceRestConfigurationForm.Error.AuthenticationUserNameRequired"));
				return;
			}
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtAuthenticationUserName.Text);
			errorProvider.SetError(authenticationUserNameExpressionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("WebServiceRestConfigurationForm.Error.AuthenticationUserNameIsInvalid"));
		}
		else
		{
			errorProvider.SetError(authenticationUserNameExpressionButton, string.Empty);
		}
	}

	private void TxtAuthenticationPassword_Validating(object sender, CancelEventArgs e)
	{
		if (rbAuthenticationBasicUserNamePassword.Checked)
		{
			if (string.IsNullOrEmpty(txtAuthenticationPassword.Text))
			{
				errorProvider.SetError(authenticationPasswordExpressionButton, LocalizedResourceMgr.GetString("WebServiceRestConfigurationForm.Error.AuthenticationPasswordRequired"));
				return;
			}
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtAuthenticationPassword.Text);
			errorProvider.SetError(authenticationPasswordExpressionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("WebServiceRestConfigurationForm.Error.AuthenticationPasswordIsInvalid"));
		}
		else
		{
			errorProvider.SetError(authenticationPasswordExpressionButton, string.Empty);
		}
	}

	private void TxtAuthenticationApiKey_Validating(object sender, CancelEventArgs e)
	{
		if (rbAuthenticationBasicApiKey.Checked)
		{
			if (string.IsNullOrEmpty(txtAuthenticationApiKey.Text))
			{
				errorProvider.SetError(authenticationApiKeyExpressionButton, LocalizedResourceMgr.GetString("WebServiceRestConfigurationForm.Error.AuthenticationApiKeyRequired"));
				return;
			}
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtAuthenticationApiKey.Text);
			errorProvider.SetError(authenticationApiKeyExpressionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("WebServiceRestConfigurationForm.Error.AuthenticationApiKeyIsInvalid"));
		}
		else
		{
			errorProvider.SetError(authenticationApiKeyExpressionButton, string.Empty);
		}
	}

	private void TxtAuthenticationAccessToken_Validating(object sender, CancelEventArgs e)
	{
		if (rbAuthenticationOAuth2.Checked)
		{
			if (string.IsNullOrEmpty(txtAuthenticationAccessToken.Text))
			{
				errorProvider.SetError(authenticationAccessTokenExpressionButton, LocalizedResourceMgr.GetString("WebServiceRestConfigurationForm.Error.AuthenticationAccessTokenRequired"));
				return;
			}
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtAuthenticationAccessToken.Text);
			errorProvider.SetError(authenticationAccessTokenExpressionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("WebServiceRestConfigurationForm.Error.AuthenticationAccessTokenIsInvalid"));
		}
		else
		{
			errorProvider.SetError(authenticationAccessTokenExpressionButton, string.Empty);
		}
	}

	private void AuthenticationUserNameExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(webServiceRestComponent);
		expressionEditorForm.Expression = txtAuthenticationUserName.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtAuthenticationUserName.Text = expressionEditorForm.Expression;
			TxtAuthenticationUserName_Validating(txtAuthenticationUserName, new CancelEventArgs());
		}
	}

	private void AuthenticationPasswordExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(webServiceRestComponent);
		expressionEditorForm.Expression = txtAuthenticationPassword.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtAuthenticationPassword.Text = expressionEditorForm.Expression;
			TxtAuthenticationPassword_Validating(txtAuthenticationPassword, new CancelEventArgs());
		}
	}

	private void AuthenticationApiKeyExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(webServiceRestComponent);
		expressionEditorForm.Expression = txtAuthenticationApiKey.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtAuthenticationApiKey.Text = expressionEditorForm.Expression;
			TxtAuthenticationApiKey_Validating(txtAuthenticationApiKey, new CancelEventArgs());
		}
	}

	private void AuthenticationAccessTokenExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(webServiceRestComponent);
		expressionEditorForm.Expression = txtAuthenticationAccessToken.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtAuthenticationAccessToken.Text = expressionEditorForm.Expression;
			TxtAuthenticationAccessToken_Validating(txtAuthenticationAccessToken, new CancelEventArgs());
		}
	}

	private void HeadersGrid_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		if (e.ColumnIndex == 2 && e.RowIndex >= 0)
		{
			ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(webServiceRestComponent);
			expressionEditorForm.Expression = ((headersGrid[1, e.RowIndex].Value == null) ? string.Empty : headersGrid[1, e.RowIndex].Value.ToString());
			if (expressionEditorForm.ShowDialog() == DialogResult.OK)
			{
				headersGrid.CurrentCell = headersGrid[1, e.RowIndex];
				SendKeys.SendWait(" ");
				headersGrid.EndEdit();
				headersGrid.CurrentCell = null;
				headersGrid[1, e.RowIndex].Value = expressionEditorForm.Expression;
			}
		}
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		List<string> list = new List<string>();
		foreach (Parameter item in headerBindingSource.List)
		{
			if (!string.IsNullOrEmpty(item.Name) || !string.IsNullOrEmpty(item.Value))
			{
				if (string.IsNullOrEmpty(item.Name) || string.IsNullOrEmpty(item.Value))
				{
					MessageBox.Show(LocalizedResourceMgr.GetString("WebServiceRestConfigurationForm.MessageBox.Error.EmptyValuesNotAllowed"), LocalizedResourceMgr.GetString("WebServiceRestConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
					headerBindingSource.Position = headerBindingSource.IndexOf(item);
					return;
				}
				if (list.Contains(item.Name))
				{
					MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("WebServiceRestConfigurationForm.MessageBox.Error.DuplicatedNamesNotAllowed"), item.Name), LocalizedResourceMgr.GetString("WebServiceRestConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
					headerBindingSource.Position = headerBindingSource.IndexOf(item);
					return;
				}
				list.Add(item.Name);
			}
		}
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void WebServiceRestConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		webServiceRestComponent.ShowHelp();
	}

	private void WebServiceRestConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		webServiceRestComponent.ShowHelp();
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
		this.lblURI = new System.Windows.Forms.Label();
		this.lblRequestType = new System.Windows.Forms.Label();
		this.comboRequestType = new System.Windows.Forms.ComboBox();
		this.txtTimeout = new System.Windows.Forms.MaskedTextBox();
		this.lblTimeout = new System.Windows.Forms.Label();
		this.cancelButton = new System.Windows.Forms.Button();
		this.okButton = new System.Windows.Forms.Button();
		this.txtURI = new System.Windows.Forms.TextBox();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.uriExpressionButton = new System.Windows.Forms.Button();
		this.lblContentType = new System.Windows.Forms.Label();
		this.comboContentType = new System.Windows.Forms.ComboBox();
		this.contentExpressionButton = new System.Windows.Forms.Button();
		this.txtContent = new System.Windows.Forms.TextBox();
		this.lblContent = new System.Windows.Forms.Label();
		this.headerBindingSource = new System.Windows.Forms.BindingSource(this.components);
		this.headersGrid = new System.Windows.Forms.DataGridView();
		this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.valueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.expressionBuilderColumn = new System.Windows.Forms.DataGridViewButtonColumn();
		this.lblHeaders = new System.Windows.Forms.Label();
		this.grpBoxAuthentication = new System.Windows.Forms.GroupBox();
		this.rbAuthenticationNone = new System.Windows.Forms.RadioButton();
		this.rbAuthenticationBasicApiKey = new System.Windows.Forms.RadioButton();
		this.authenticationAccessTokenExpressionButton = new System.Windows.Forms.Button();
		this.authenticationApiKeyExpressionButton = new System.Windows.Forms.Button();
		this.authenticationPasswordExpressionButton = new System.Windows.Forms.Button();
		this.lblAuthenticationApiKey = new System.Windows.Forms.Label();
		this.txtAuthenticationApiKey = new System.Windows.Forms.TextBox();
		this.authenticationUserNameExpressionButton = new System.Windows.Forms.Button();
		this.lblAuthenticationPassword = new System.Windows.Forms.Label();
		this.lblAuthenticationUserName = new System.Windows.Forms.Label();
		this.txtAuthenticationPassword = new System.Windows.Forms.TextBox();
		this.txtAuthenticationUserName = new System.Windows.Forms.TextBox();
		this.rbAuthenticationOAuth2 = new System.Windows.Forms.RadioButton();
		this.rbAuthenticationBasicUserNamePassword = new System.Windows.Forms.RadioButton();
		this.lblAuthenticationAccessToken = new System.Windows.Forms.Label();
		this.txtAuthenticationAccessToken = new System.Windows.Forms.TextBox();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.headerBindingSource).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.headersGrid).BeginInit();
		this.grpBoxAuthentication.SuspendLayout();
		base.SuspendLayout();
		this.lblURI.AutoSize = true;
		this.lblURI.Location = new System.Drawing.Point(16, 11);
		this.lblURI.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblURI.Name = "lblURI";
		this.lblURI.Size = new System.Drawing.Size(31, 17);
		this.lblURI.TabIndex = 0;
		this.lblURI.Text = "URI";
		this.lblRequestType.AutoSize = true;
		this.lblRequestType.Location = new System.Drawing.Point(16, 46);
		this.lblRequestType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblRequestType.Name = "lblRequestType";
		this.lblRequestType.Size = new System.Drawing.Size(97, 17);
		this.lblRequestType.TabIndex = 3;
		this.lblRequestType.Text = "Request Type";
		this.comboRequestType.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboRequestType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboRequestType.FormattingEnabled = true;
		this.comboRequestType.Location = new System.Drawing.Point(125, 42);
		this.comboRequestType.Margin = new System.Windows.Forms.Padding(4);
		this.comboRequestType.Name = "comboRequestType";
		this.comboRequestType.Size = new System.Drawing.Size(471, 24);
		this.comboRequestType.TabIndex = 4;
		this.txtTimeout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtTimeout.HidePromptOnLeave = true;
		this.txtTimeout.Location = new System.Drawing.Point(125, 142);
		this.txtTimeout.Margin = new System.Windows.Forms.Padding(4);
		this.txtTimeout.Mask = "999";
		this.txtTimeout.Name = "txtTimeout";
		this.txtTimeout.Size = new System.Drawing.Size(471, 22);
		this.txtTimeout.TabIndex = 11;
		this.txtTimeout.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtTimeout.Validating += new System.ComponentModel.CancelEventHandler(TxtTimeout_Validating);
		this.lblTimeout.AutoSize = true;
		this.lblTimeout.Location = new System.Drawing.Point(16, 145);
		this.lblTimeout.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblTimeout.Name = "lblTimeout";
		this.lblTimeout.Size = new System.Drawing.Size(102, 17);
		this.lblTimeout.TabIndex = 10;
		this.lblTimeout.Text = "Timeout (secs)";
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(497, 650);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 16;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(389, 650);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 15;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.txtURI.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtURI.Location = new System.Drawing.Point(125, 7);
		this.txtURI.Margin = new System.Windows.Forms.Padding(4);
		this.txtURI.MaxLength = 8192;
		this.txtURI.Name = "txtURI";
		this.txtURI.Size = new System.Drawing.Size(424, 22);
		this.txtURI.TabIndex = 1;
		this.txtURI.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtURI.Validating += new System.ComponentModel.CancelEventHandler(TxtURI_Validating);
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.uriExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.uriExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.uriExpressionButton.Location = new System.Drawing.Point(559, 5);
		this.uriExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.uriExpressionButton.Name = "uriExpressionButton";
		this.uriExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.uriExpressionButton.TabIndex = 2;
		this.uriExpressionButton.UseVisualStyleBackColor = true;
		this.uriExpressionButton.Click += new System.EventHandler(UriExpressionButton_Click);
		this.lblContentType.AutoSize = true;
		this.lblContentType.Location = new System.Drawing.Point(16, 79);
		this.lblContentType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblContentType.Name = "lblContentType";
		this.lblContentType.Size = new System.Drawing.Size(93, 17);
		this.lblContentType.TabIndex = 5;
		this.lblContentType.Text = "Content Type";
		this.comboContentType.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboContentType.Location = new System.Drawing.Point(125, 75);
		this.comboContentType.Margin = new System.Windows.Forms.Padding(4);
		this.comboContentType.MaxLength = 1024;
		this.comboContentType.Name = "comboContentType";
		this.comboContentType.Size = new System.Drawing.Size(471, 24);
		this.comboContentType.TabIndex = 6;
		this.comboContentType.TextChanged += new System.EventHandler(ComboContentType_TextChanged);
		this.contentExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.contentExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.contentExpressionButton.Location = new System.Drawing.Point(559, 106);
		this.contentExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.contentExpressionButton.Name = "contentExpressionButton";
		this.contentExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.contentExpressionButton.TabIndex = 9;
		this.contentExpressionButton.UseVisualStyleBackColor = true;
		this.contentExpressionButton.Click += new System.EventHandler(ContentExpressionButton_Click);
		this.txtContent.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtContent.Location = new System.Drawing.Point(125, 108);
		this.txtContent.Margin = new System.Windows.Forms.Padding(4);
		this.txtContent.MaxLength = 8192;
		this.txtContent.Name = "txtContent";
		this.txtContent.Size = new System.Drawing.Size(424, 22);
		this.txtContent.TabIndex = 8;
		this.txtContent.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtContent.Validating += new System.ComponentModel.CancelEventHandler(TxtContent_Validating);
		this.lblContent.AutoSize = true;
		this.lblContent.Location = new System.Drawing.Point(16, 112);
		this.lblContent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblContent.Name = "lblContent";
		this.lblContent.Size = new System.Drawing.Size(57, 17);
		this.lblContent.TabIndex = 7;
		this.lblContent.Text = "Content";
		this.headerBindingSource.DataSource = typeof(TCX.CFD.Classes.Components.Parameter);
		this.headersGrid.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.headersGrid.AutoGenerateColumns = false;
		this.headersGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
		this.headersGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
		this.headersGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.headersGrid.Columns.AddRange(this.nameDataGridViewTextBoxColumn, this.valueDataGridViewTextBoxColumn, this.expressionBuilderColumn);
		this.headersGrid.DataSource = this.headerBindingSource;
		this.headersGrid.Location = new System.Drawing.Point(20, 486);
		this.headersGrid.Margin = new System.Windows.Forms.Padding(4);
		this.headersGrid.Name = "headersGrid";
		this.headersGrid.RowHeadersWidth = 51;
		this.headersGrid.Size = new System.Drawing.Size(577, 156);
		this.headersGrid.TabIndex = 14;
		this.headersGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(HeadersGrid_CellClick);
		this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
		this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
		this.nameDataGridViewTextBoxColumn.MaxInputLength = 256;
		this.nameDataGridViewTextBoxColumn.MinimumWidth = 6;
		this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
		this.valueDataGridViewTextBoxColumn.DataPropertyName = "Value";
		this.valueDataGridViewTextBoxColumn.HeaderText = "Value";
		this.valueDataGridViewTextBoxColumn.MaxInputLength = 1024;
		this.valueDataGridViewTextBoxColumn.MinimumWidth = 6;
		this.valueDataGridViewTextBoxColumn.Name = "valueDataGridViewTextBoxColumn";
		dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle.NullValue = "...";
		this.expressionBuilderColumn.DefaultCellStyle = dataGridViewCellStyle;
		this.expressionBuilderColumn.FillWeight = 20f;
		this.expressionBuilderColumn.HeaderText = "";
		this.expressionBuilderColumn.MinimumWidth = 6;
		this.expressionBuilderColumn.Name = "expressionBuilderColumn";
		this.expressionBuilderColumn.Text = "";
		this.lblHeaders.AutoSize = true;
		this.lblHeaders.Location = new System.Drawing.Point(20, 466);
		this.lblHeaders.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblHeaders.Name = "lblHeaders";
		this.lblHeaders.Size = new System.Drawing.Size(62, 17);
		this.lblHeaders.TabIndex = 13;
		this.lblHeaders.Text = "Headers";
		this.grpBoxAuthentication.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.grpBoxAuthentication.Controls.Add(this.rbAuthenticationNone);
		this.grpBoxAuthentication.Controls.Add(this.rbAuthenticationBasicApiKey);
		this.grpBoxAuthentication.Controls.Add(this.authenticationAccessTokenExpressionButton);
		this.grpBoxAuthentication.Controls.Add(this.authenticationApiKeyExpressionButton);
		this.grpBoxAuthentication.Controls.Add(this.authenticationPasswordExpressionButton);
		this.grpBoxAuthentication.Controls.Add(this.lblAuthenticationApiKey);
		this.grpBoxAuthentication.Controls.Add(this.txtAuthenticationApiKey);
		this.grpBoxAuthentication.Controls.Add(this.authenticationUserNameExpressionButton);
		this.grpBoxAuthentication.Controls.Add(this.lblAuthenticationPassword);
		this.grpBoxAuthentication.Controls.Add(this.lblAuthenticationUserName);
		this.grpBoxAuthentication.Controls.Add(this.txtAuthenticationPassword);
		this.grpBoxAuthentication.Controls.Add(this.txtAuthenticationUserName);
		this.grpBoxAuthentication.Controls.Add(this.rbAuthenticationOAuth2);
		this.grpBoxAuthentication.Controls.Add(this.rbAuthenticationBasicUserNamePassword);
		this.grpBoxAuthentication.Controls.Add(this.lblAuthenticationAccessToken);
		this.grpBoxAuthentication.Controls.Add(this.txtAuthenticationAccessToken);
		this.grpBoxAuthentication.Location = new System.Drawing.Point(16, 174);
		this.grpBoxAuthentication.Margin = new System.Windows.Forms.Padding(4);
		this.grpBoxAuthentication.Name = "grpBoxAuthentication";
		this.grpBoxAuthentication.Padding = new System.Windows.Forms.Padding(4);
		this.grpBoxAuthentication.Size = new System.Drawing.Size(581, 276);
		this.grpBoxAuthentication.TabIndex = 12;
		this.grpBoxAuthentication.TabStop = false;
		this.grpBoxAuthentication.Text = "Authentication";
		this.rbAuthenticationNone.AutoSize = true;
		this.rbAuthenticationNone.Location = new System.Drawing.Point(8, 23);
		this.rbAuthenticationNone.Margin = new System.Windows.Forms.Padding(4);
		this.rbAuthenticationNone.Name = "rbAuthenticationNone";
		this.rbAuthenticationNone.Size = new System.Drawing.Size(63, 21);
		this.rbAuthenticationNone.TabIndex = 0;
		this.rbAuthenticationNone.TabStop = true;
		this.rbAuthenticationNone.Text = "None";
		this.rbAuthenticationNone.UseVisualStyleBackColor = true;
		this.rbAuthenticationNone.CheckedChanged += new System.EventHandler(RbAuthentication_CheckedChanged);
		this.rbAuthenticationBasicApiKey.AutoSize = true;
		this.rbAuthenticationBasicApiKey.Location = new System.Drawing.Point(8, 143);
		this.rbAuthenticationBasicApiKey.Margin = new System.Windows.Forms.Padding(4);
		this.rbAuthenticationBasicApiKey.Name = "rbAuthenticationBasicApiKey";
		this.rbAuthenticationBasicApiKey.Size = new System.Drawing.Size(125, 21);
		this.rbAuthenticationBasicApiKey.TabIndex = 8;
		this.rbAuthenticationBasicApiKey.TabStop = true;
		this.rbAuthenticationBasicApiKey.Text = "Basic - API Key";
		this.rbAuthenticationBasicApiKey.UseVisualStyleBackColor = true;
		this.rbAuthenticationBasicApiKey.CheckedChanged += new System.EventHandler(RbAuthentication_CheckedChanged);
		this.authenticationAccessTokenExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.authenticationAccessTokenExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.authenticationAccessTokenExpressionButton.Location = new System.Drawing.Point(512, 228);
		this.authenticationAccessTokenExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.authenticationAccessTokenExpressionButton.Name = "authenticationAccessTokenExpressionButton";
		this.authenticationAccessTokenExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.authenticationAccessTokenExpressionButton.TabIndex = 15;
		this.authenticationAccessTokenExpressionButton.UseVisualStyleBackColor = true;
		this.authenticationAccessTokenExpressionButton.Click += new System.EventHandler(AuthenticationAccessTokenExpressionButton_Click);
		this.authenticationApiKeyExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.authenticationApiKeyExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.authenticationApiKeyExpressionButton.Location = new System.Drawing.Point(512, 169);
		this.authenticationApiKeyExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.authenticationApiKeyExpressionButton.Name = "authenticationApiKeyExpressionButton";
		this.authenticationApiKeyExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.authenticationApiKeyExpressionButton.TabIndex = 11;
		this.authenticationApiKeyExpressionButton.UseVisualStyleBackColor = true;
		this.authenticationApiKeyExpressionButton.Click += new System.EventHandler(AuthenticationApiKeyExpressionButton_Click);
		this.authenticationPasswordExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.authenticationPasswordExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.authenticationPasswordExpressionButton.Location = new System.Drawing.Point(512, 110);
		this.authenticationPasswordExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.authenticationPasswordExpressionButton.Name = "authenticationPasswordExpressionButton";
		this.authenticationPasswordExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.authenticationPasswordExpressionButton.TabIndex = 7;
		this.authenticationPasswordExpressionButton.UseVisualStyleBackColor = true;
		this.authenticationPasswordExpressionButton.Click += new System.EventHandler(AuthenticationPasswordExpressionButton_Click);
		this.lblAuthenticationApiKey.AutoSize = true;
		this.lblAuthenticationApiKey.Location = new System.Drawing.Point(31, 175);
		this.lblAuthenticationApiKey.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblAuthenticationApiKey.Name = "lblAuthenticationApiKey";
		this.lblAuthenticationApiKey.Size = new System.Drawing.Size(57, 17);
		this.lblAuthenticationApiKey.TabIndex = 9;
		this.lblAuthenticationApiKey.Text = "API Key";
		this.txtAuthenticationApiKey.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtAuthenticationApiKey.Location = new System.Drawing.Point(140, 171);
		this.txtAuthenticationApiKey.Margin = new System.Windows.Forms.Padding(4);
		this.txtAuthenticationApiKey.MaxLength = 8192;
		this.txtAuthenticationApiKey.Name = "txtAuthenticationApiKey";
		this.txtAuthenticationApiKey.Size = new System.Drawing.Size(363, 22);
		this.txtAuthenticationApiKey.TabIndex = 10;
		this.txtAuthenticationApiKey.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtAuthenticationApiKey.Validating += new System.ComponentModel.CancelEventHandler(TxtAuthenticationApiKey_Validating);
		this.authenticationUserNameExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.authenticationUserNameExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.authenticationUserNameExpressionButton.Location = new System.Drawing.Point(512, 78);
		this.authenticationUserNameExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.authenticationUserNameExpressionButton.Name = "authenticationUserNameExpressionButton";
		this.authenticationUserNameExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.authenticationUserNameExpressionButton.TabIndex = 4;
		this.authenticationUserNameExpressionButton.UseVisualStyleBackColor = true;
		this.authenticationUserNameExpressionButton.Click += new System.EventHandler(AuthenticationUserNameExpressionButton_Click);
		this.lblAuthenticationPassword.AutoSize = true;
		this.lblAuthenticationPassword.Location = new System.Drawing.Point(31, 116);
		this.lblAuthenticationPassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblAuthenticationPassword.Name = "lblAuthenticationPassword";
		this.lblAuthenticationPassword.Size = new System.Drawing.Size(69, 17);
		this.lblAuthenticationPassword.TabIndex = 5;
		this.lblAuthenticationPassword.Text = "Password";
		this.lblAuthenticationUserName.AutoSize = true;
		this.lblAuthenticationUserName.Location = new System.Drawing.Point(31, 84);
		this.lblAuthenticationUserName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblAuthenticationUserName.Name = "lblAuthenticationUserName";
		this.lblAuthenticationUserName.Size = new System.Drawing.Size(79, 17);
		this.lblAuthenticationUserName.TabIndex = 2;
		this.lblAuthenticationUserName.Text = "User Name";
		this.txtAuthenticationPassword.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtAuthenticationPassword.Location = new System.Drawing.Point(140, 112);
		this.txtAuthenticationPassword.Margin = new System.Windows.Forms.Padding(4);
		this.txtAuthenticationPassword.MaxLength = 8192;
		this.txtAuthenticationPassword.Name = "txtAuthenticationPassword";
		this.txtAuthenticationPassword.Size = new System.Drawing.Size(363, 22);
		this.txtAuthenticationPassword.TabIndex = 6;
		this.txtAuthenticationPassword.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtAuthenticationPassword.Validating += new System.ComponentModel.CancelEventHandler(TxtAuthenticationPassword_Validating);
		this.txtAuthenticationUserName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtAuthenticationUserName.Location = new System.Drawing.Point(140, 80);
		this.txtAuthenticationUserName.Margin = new System.Windows.Forms.Padding(4);
		this.txtAuthenticationUserName.MaxLength = 8192;
		this.txtAuthenticationUserName.Name = "txtAuthenticationUserName";
		this.txtAuthenticationUserName.Size = new System.Drawing.Size(363, 22);
		this.txtAuthenticationUserName.TabIndex = 3;
		this.txtAuthenticationUserName.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtAuthenticationUserName.Validating += new System.ComponentModel.CancelEventHandler(TxtAuthenticationUserName_Validating);
		this.rbAuthenticationOAuth2.AutoSize = true;
		this.rbAuthenticationOAuth2.Location = new System.Drawing.Point(8, 203);
		this.rbAuthenticationOAuth2.Margin = new System.Windows.Forms.Padding(4);
		this.rbAuthenticationOAuth2.Name = "rbAuthenticationOAuth2";
		this.rbAuthenticationOAuth2.Size = new System.Drawing.Size(77, 21);
		this.rbAuthenticationOAuth2.TabIndex = 12;
		this.rbAuthenticationOAuth2.TabStop = true;
		this.rbAuthenticationOAuth2.Text = "OAuth2";
		this.rbAuthenticationOAuth2.UseVisualStyleBackColor = true;
		this.rbAuthenticationOAuth2.CheckedChanged += new System.EventHandler(RbAuthentication_CheckedChanged);
		this.rbAuthenticationBasicUserNamePassword.AutoSize = true;
		this.rbAuthenticationBasicUserNamePassword.Location = new System.Drawing.Point(8, 52);
		this.rbAuthenticationBasicUserNamePassword.Margin = new System.Windows.Forms.Padding(4);
		this.rbAuthenticationBasicUserNamePassword.Name = "rbAuthenticationBasicUserNamePassword";
		this.rbAuthenticationBasicUserNamePassword.Size = new System.Drawing.Size(236, 21);
		this.rbAuthenticationBasicUserNamePassword.TabIndex = 1;
		this.rbAuthenticationBasicUserNamePassword.TabStop = true;
		this.rbAuthenticationBasicUserNamePassword.Text = "Basic - UserName and Password";
		this.rbAuthenticationBasicUserNamePassword.UseVisualStyleBackColor = true;
		this.rbAuthenticationBasicUserNamePassword.CheckedChanged += new System.EventHandler(RbAuthentication_CheckedChanged);
		this.lblAuthenticationAccessToken.AutoSize = true;
		this.lblAuthenticationAccessToken.Location = new System.Drawing.Point(31, 235);
		this.lblAuthenticationAccessToken.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblAuthenticationAccessToken.Name = "lblAuthenticationAccessToken";
		this.lblAuthenticationAccessToken.Size = new System.Drawing.Size(97, 17);
		this.lblAuthenticationAccessToken.TabIndex = 13;
		this.lblAuthenticationAccessToken.Text = "Access Token";
		this.txtAuthenticationAccessToken.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtAuthenticationAccessToken.Location = new System.Drawing.Point(140, 231);
		this.txtAuthenticationAccessToken.Margin = new System.Windows.Forms.Padding(4);
		this.txtAuthenticationAccessToken.MaxLength = 8192;
		this.txtAuthenticationAccessToken.Name = "txtAuthenticationAccessToken";
		this.txtAuthenticationAccessToken.Size = new System.Drawing.Size(363, 22);
		this.txtAuthenticationAccessToken.TabIndex = 14;
		this.txtAuthenticationAccessToken.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtAuthenticationAccessToken.Validating += new System.ComponentModel.CancelEventHandler(TxtAuthenticationAccessToken_Validating);
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(619, 693);
		base.Controls.Add(this.grpBoxAuthentication);
		base.Controls.Add(this.headersGrid);
		base.Controls.Add(this.lblHeaders);
		base.Controls.Add(this.lblContent);
		base.Controls.Add(this.contentExpressionButton);
		base.Controls.Add(this.txtContent);
		base.Controls.Add(this.comboContentType);
		base.Controls.Add(this.lblContentType);
		base.Controls.Add(this.uriExpressionButton);
		base.Controls.Add(this.txtURI);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.Controls.Add(this.lblTimeout);
		base.Controls.Add(this.txtTimeout);
		base.Controls.Add(this.comboRequestType);
		base.Controls.Add(this.lblRequestType);
		base.Controls.Add(this.lblURI);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(1359, 875);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(634, 730);
		base.Name = "WebServiceRestConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Web Service REST";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(WebServiceRestConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(WebServiceRestConfigurationForm_HelpRequested);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		((System.ComponentModel.ISupportInitialize)this.headerBindingSource).EndInit();
		((System.ComponentModel.ISupportInitialize)this.headersGrid).EndInit();
		this.grpBoxAuthentication.ResumeLayout(false);
		this.grpBoxAuthentication.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
