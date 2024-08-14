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

public class DatabaseAccessConfigurationForm : Form
{
	private readonly DatabaseAccessComponent databaseAccessComponent;

	private readonly List<string> validVariables;

	private IContainer components;

	private Label lblStatementType;

	private ComboBox comboStatementType;

	private MaskedTextBox txtTimeout;

	private Label lblTimeout;

	private Button cancelButton;

	private Button okButton;

	private Button passwordExpressionButton;

	private TextBox txtPassword;

	private Label lblPassword;

	private Button userNameExpressionButton;

	private TextBox txtUserName;

	private Label lblUserName;

	private Button serverExpressionButton;

	private TextBox txtServer;

	private Label lblServer;

	private ComboBox comboDatabaseType;

	private Label lblDatabaseType;

	private Button databaseExpressionButton;

	private TextBox txtDatabase;

	private Label lblDatabase;

	private TextBox txtSqlStatement;

	private Label lblSqlStatement;

	private Button sqlStatementExpressionButton;

	private ToolTip toolTip;

	private ErrorProvider errorProvider;

	private Button portExpressionButton;

	private TextBox txtPort;

	private Label lblPort;

	private RadioButton rbUseConnectionString;

	private RadioButton rbUseSeparatedSettings;

	private Button connectionStringExpressionButton;

	private TextBox txtConnectionString;

	private Label lblConnectionString;

	public DatabaseTypes DatabaseType => (DatabaseTypes)comboDatabaseType.SelectedItem;

	public bool UseConnectionString => rbUseConnectionString.Checked;

	public string ConnectionString => txtConnectionString.Text;

	public string Server => txtServer.Text;

	public string Port => txtPort.Text;

	public string Database => txtDatabase.Text;

	public string UserName => txtUserName.Text;

	public string Password => txtPassword.Text;

	public SqlStatementTypes StatementType => (SqlStatementTypes)comboStatementType.SelectedItem;

	public uint Timeout
	{
		get
		{
			if (!string.IsNullOrEmpty(txtTimeout.Text))
			{
				return Convert.ToUInt32(txtTimeout.Text);
			}
			return 0u;
		}
	}

	public string SqlStatement => txtSqlStatement.Text;

	public DatabaseAccessConfigurationForm(DatabaseAccessComponent databaseAccessComponent)
	{
		InitializeComponent();
		this.databaseAccessComponent = databaseAccessComponent;
		validVariables = ExpressionHelper.GetValidVariables(databaseAccessComponent);
		comboDatabaseType.Items.AddRange(new object[3]
		{
			DatabaseTypes.SqlServer,
			DatabaseTypes.PostgreSQL,
			DatabaseTypes.MySQL
		});
		comboStatementType.Items.AddRange(new object[3]
		{
			SqlStatementTypes.Query,
			SqlStatementTypes.NonQuery,
			SqlStatementTypes.Scalar
		});
		comboDatabaseType.SelectedItem = databaseAccessComponent.DatabaseType;
		rbUseConnectionString.Checked = databaseAccessComponent.UseConnectionString;
		rbUseSeparatedSettings.Checked = !databaseAccessComponent.UseConnectionString;
		txtConnectionString.Text = databaseAccessComponent.ConnectionString;
		txtServer.Text = databaseAccessComponent.Server;
		txtPort.Text = databaseAccessComponent.Port;
		txtDatabase.Text = databaseAccessComponent.Database;
		txtUserName.Text = databaseAccessComponent.UserName;
		txtPassword.Text = databaseAccessComponent.Password;
		comboStatementType.SelectedItem = databaseAccessComponent.StatementType;
		txtTimeout.Text = databaseAccessComponent.Timeout.ToString();
		txtSqlStatement.Text = databaseAccessComponent.SqlStatement;
		TxtConnectionString_Validating(txtConnectionString, new CancelEventArgs());
		TxtServer_Validating(txtServer, new CancelEventArgs());
		TxtPort_Validating(txtPort, new CancelEventArgs());
		TxtDatabase_Validating(txtDatabase, new CancelEventArgs());
		TxtUserName_Validating(txtUserName, new CancelEventArgs());
		TxtPassword_Validating(txtPassword, new CancelEventArgs());
		TxtTimeout_Validating(txtTimeout, new CancelEventArgs());
		TxtSqlStatement_Validating(txtSqlStatement, new CancelEventArgs());
		Text = LocalizedResourceMgr.GetString("DatabaseAccessConfigurationForm.Title");
		lblDatabaseType.Text = LocalizedResourceMgr.GetString("DatabaseAccessConfigurationForm.lblDatabaseType.Text");
		rbUseConnectionString.Text = LocalizedResourceMgr.GetString("DatabaseAccessConfigurationForm.rbUseConnectionString.Text");
		rbUseSeparatedSettings.Text = LocalizedResourceMgr.GetString("DatabaseAccessConfigurationForm.rbUseSeparatedSettings.Text");
		lblConnectionString.Text = LocalizedResourceMgr.GetString("DatabaseAccessConfigurationForm.lblConnectionString.Text");
		lblServer.Text = LocalizedResourceMgr.GetString("DatabaseAccessConfigurationForm.lblServer.Text");
		lblPort.Text = LocalizedResourceMgr.GetString("DatabaseAccessConfigurationForm.lblPort.Text");
		lblDatabase.Text = LocalizedResourceMgr.GetString("DatabaseAccessConfigurationForm.lblDatabase.Text");
		lblUserName.Text = LocalizedResourceMgr.GetString("DatabaseAccessConfigurationForm.lblUserName.Text");
		lblPassword.Text = LocalizedResourceMgr.GetString("DatabaseAccessConfigurationForm.lblPassword.Text");
		lblStatementType.Text = LocalizedResourceMgr.GetString("DatabaseAccessConfigurationForm.lblStatementType.Text");
		lblTimeout.Text = LocalizedResourceMgr.GetString("DatabaseAccessConfigurationForm.lblTimeout.Text");
		lblSqlStatement.Text = LocalizedResourceMgr.GetString("DatabaseAccessConfigurationForm.lblSqlStatement.Text");
		okButton.Text = LocalizedResourceMgr.GetString("DatabaseAccessConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("DatabaseAccessConfigurationForm.cancelButton.Text");
	}

	private void RbConnectionString_CheckedChanged(object sender, EventArgs e)
	{
		txtConnectionString.Enabled = rbUseConnectionString.Checked;
		connectionStringExpressionButton.Enabled = rbUseConnectionString.Checked;
		txtServer.Enabled = rbUseSeparatedSettings.Checked;
		serverExpressionButton.Enabled = rbUseSeparatedSettings.Checked;
		txtPort.Enabled = rbUseSeparatedSettings.Checked;
		portExpressionButton.Enabled = rbUseSeparatedSettings.Checked;
		txtDatabase.Enabled = rbUseSeparatedSettings.Checked;
		databaseExpressionButton.Enabled = rbUseSeparatedSettings.Checked;
		txtUserName.Enabled = rbUseSeparatedSettings.Checked;
		userNameExpressionButton.Enabled = rbUseSeparatedSettings.Checked;
		txtPassword.Enabled = rbUseSeparatedSettings.Checked;
		passwordExpressionButton.Enabled = rbUseSeparatedSettings.Checked;
		TxtConnectionString_Validating(txtConnectionString, new CancelEventArgs());
		TxtServer_Validating(txtServer, new CancelEventArgs());
		TxtPort_Validating(txtPort, new CancelEventArgs());
		TxtDatabase_Validating(txtDatabase, new CancelEventArgs());
		TxtUserName_Validating(txtUserName, new CancelEventArgs());
		TxtPassword_Validating(txtPassword, new CancelEventArgs());
	}

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void MaskedTextBox_GotFocus(object sender, EventArgs e)
	{
		(sender as MaskedTextBox).SelectAll();
	}

	private void TxtConnectionString_Validating(object sender, CancelEventArgs e)
	{
		if (txtConnectionString.Enabled)
		{
			if (string.IsNullOrEmpty(txtConnectionString.Text))
			{
				errorProvider.SetError(connectionStringExpressionButton, LocalizedResourceMgr.GetString("DatabaseAccessConfigurationForm.Error.ConnectionStringIsMandatory"));
				return;
			}
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtConnectionString.Text);
			errorProvider.SetError(connectionStringExpressionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("DatabaseAccessConfigurationForm.Error.ConnectionStringIsInvalid"));
		}
		else
		{
			errorProvider.SetError(connectionStringExpressionButton, string.Empty);
		}
	}

	private void TxtServer_Validating(object sender, CancelEventArgs e)
	{
		if (txtServer.Enabled)
		{
			if (string.IsNullOrEmpty(txtServer.Text))
			{
				errorProvider.SetError(serverExpressionButton, LocalizedResourceMgr.GetString("DatabaseAccessConfigurationForm.Error.ServerIsMandatory"));
				return;
			}
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtServer.Text);
			errorProvider.SetError(serverExpressionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("DatabaseAccessConfigurationForm.Error.ServerIsInvalid"));
		}
		else
		{
			errorProvider.SetError(serverExpressionButton, string.Empty);
		}
	}

	private void TxtPort_Validating(object sender, CancelEventArgs e)
	{
		if (txtPort.Enabled && !string.IsNullOrEmpty(txtPort.Text))
		{
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtPort.Text);
			errorProvider.SetError(portExpressionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("DatabaseAccessConfigurationForm.Error.PortIsInvalid"));
		}
		else
		{
			errorProvider.SetError(portExpressionButton, string.Empty);
		}
	}

	private void TxtDatabase_Validating(object sender, CancelEventArgs e)
	{
		if (txtDatabase.Enabled)
		{
			if (string.IsNullOrEmpty(txtDatabase.Text))
			{
				errorProvider.SetError(databaseExpressionButton, LocalizedResourceMgr.GetString("DatabaseAccessConfigurationForm.Error.DatabaseIsMandatory"));
				return;
			}
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtDatabase.Text);
			errorProvider.SetError(databaseExpressionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("DatabaseAccessConfigurationForm.Error.DatabaseIsInvalid"));
		}
		else
		{
			errorProvider.SetError(databaseExpressionButton, string.Empty);
		}
	}

	private void TxtUserName_Validating(object sender, CancelEventArgs e)
	{
		if (txtUserName.Enabled)
		{
			if (string.IsNullOrEmpty(txtUserName.Text))
			{
				errorProvider.SetError(userNameExpressionButton, LocalizedResourceMgr.GetString("DatabaseAccessConfigurationForm.Error.UserNameIsMandatory"));
				return;
			}
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtUserName.Text);
			errorProvider.SetError(userNameExpressionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("DatabaseAccessConfigurationForm.Error.UserNameIsInvalid"));
		}
		else
		{
			errorProvider.SetError(userNameExpressionButton, string.Empty);
		}
	}

	private void TxtPassword_Validating(object sender, CancelEventArgs e)
	{
		if (txtPassword.Enabled)
		{
			if (string.IsNullOrEmpty(txtPassword.Text))
			{
				errorProvider.SetError(passwordExpressionButton, LocalizedResourceMgr.GetString("DatabaseAccessConfigurationForm.Error.PasswordIsMandatory"));
				return;
			}
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtPassword.Text);
			errorProvider.SetError(passwordExpressionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("DatabaseAccessConfigurationForm.Error.PasswordIsInvalid"));
		}
		else
		{
			errorProvider.SetError(passwordExpressionButton, string.Empty);
		}
	}

	private void TxtTimeout_Validating(object sender, CancelEventArgs e)
	{
		errorProvider.SetError(txtTimeout, string.IsNullOrEmpty(txtTimeout.Text) ? LocalizedResourceMgr.GetString("DatabaseAccessConfigurationForm.Error.TimeoutIsMandatory") : string.Empty);
	}

	private void TxtSqlStatement_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtSqlStatement.Text))
		{
			errorProvider.SetError(sqlStatementExpressionButton, LocalizedResourceMgr.GetString("DatabaseAccessConfigurationForm.Error.SqlStatementIsMandatory"));
			return;
		}
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtSqlStatement.Text);
		errorProvider.SetError(sqlStatementExpressionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("DatabaseAccessConfigurationForm.Error.SqlStatementIsInvalid"));
	}

	private void ConnectionStringExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(databaseAccessComponent)
		{
			Expression = txtConnectionString.Text
		};
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtConnectionString.Text = expressionEditorForm.Expression;
			TxtConnectionString_Validating(txtConnectionString, new CancelEventArgs());
		}
	}

	private void ServerExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(databaseAccessComponent)
		{
			Expression = txtServer.Text
		};
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtServer.Text = expressionEditorForm.Expression;
			TxtServer_Validating(txtServer, new CancelEventArgs());
		}
	}

	private void PortExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(databaseAccessComponent)
		{
			Expression = txtPort.Text
		};
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtPort.Text = expressionEditorForm.Expression;
			TxtPort_Validating(txtPort, new CancelEventArgs());
		}
	}

	private void DatabaseExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(databaseAccessComponent)
		{
			Expression = txtDatabase.Text
		};
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtDatabase.Text = expressionEditorForm.Expression;
			TxtDatabase_Validating(txtDatabase, new CancelEventArgs());
		}
	}

	private void UserNameExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(databaseAccessComponent)
		{
			Expression = txtUserName.Text
		};
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtUserName.Text = expressionEditorForm.Expression;
			TxtUserName_Validating(txtUserName, new CancelEventArgs());
		}
	}

	private void PasswordExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(databaseAccessComponent)
		{
			Expression = txtPassword.Text
		};
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtPassword.Text = expressionEditorForm.Expression;
			TxtPassword_Validating(txtPassword, new CancelEventArgs());
		}
	}

	private void SqlStatementExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(databaseAccessComponent)
		{
			Expression = txtSqlStatement.Text
		};
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtSqlStatement.Text = expressionEditorForm.Expression;
			TxtSqlStatement_Validating(txtPassword, new CancelEventArgs());
		}
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void DatabaseAccessConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		databaseAccessComponent.ShowHelp();
	}

	private void DatabaseAccessConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		databaseAccessComponent.ShowHelp();
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
		this.lblStatementType = new System.Windows.Forms.Label();
		this.comboStatementType = new System.Windows.Forms.ComboBox();
		this.txtTimeout = new System.Windows.Forms.MaskedTextBox();
		this.lblTimeout = new System.Windows.Forms.Label();
		this.cancelButton = new System.Windows.Forms.Button();
		this.okButton = new System.Windows.Forms.Button();
		this.passwordExpressionButton = new System.Windows.Forms.Button();
		this.txtPassword = new System.Windows.Forms.TextBox();
		this.lblPassword = new System.Windows.Forms.Label();
		this.userNameExpressionButton = new System.Windows.Forms.Button();
		this.txtUserName = new System.Windows.Forms.TextBox();
		this.lblUserName = new System.Windows.Forms.Label();
		this.serverExpressionButton = new System.Windows.Forms.Button();
		this.txtServer = new System.Windows.Forms.TextBox();
		this.lblServer = new System.Windows.Forms.Label();
		this.comboDatabaseType = new System.Windows.Forms.ComboBox();
		this.lblDatabaseType = new System.Windows.Forms.Label();
		this.databaseExpressionButton = new System.Windows.Forms.Button();
		this.txtDatabase = new System.Windows.Forms.TextBox();
		this.lblDatabase = new System.Windows.Forms.Label();
		this.txtSqlStatement = new System.Windows.Forms.TextBox();
		this.lblSqlStatement = new System.Windows.Forms.Label();
		this.sqlStatementExpressionButton = new System.Windows.Forms.Button();
		this.toolTip = new System.Windows.Forms.ToolTip(this.components);
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.portExpressionButton = new System.Windows.Forms.Button();
		this.txtPort = new System.Windows.Forms.TextBox();
		this.lblPort = new System.Windows.Forms.Label();
		this.rbUseConnectionString = new System.Windows.Forms.RadioButton();
		this.connectionStringExpressionButton = new System.Windows.Forms.Button();
		this.txtConnectionString = new System.Windows.Forms.TextBox();
		this.lblConnectionString = new System.Windows.Forms.Label();
		this.rbUseSeparatedSettings = new System.Windows.Forms.RadioButton();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.lblStatementType.AutoSize = true;
		this.lblStatementType.Location = new System.Drawing.Point(16, 330);
		this.lblStatementType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblStatementType.Name = "lblStatementType";
		this.lblStatementType.Size = new System.Drawing.Size(108, 17);
		this.lblStatementType.TabIndex = 22;
		this.lblStatementType.Text = "Statement Type";
		this.comboStatementType.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboStatementType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboStatementType.FormattingEnabled = true;
		this.comboStatementType.Location = new System.Drawing.Point(133, 326);
		this.comboStatementType.Margin = new System.Windows.Forms.Padding(4);
		this.comboStatementType.Name = "comboStatementType";
		this.comboStatementType.Size = new System.Drawing.Size(416, 24);
		this.comboStatementType.TabIndex = 23;
		this.txtTimeout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtTimeout.HidePromptOnLeave = true;
		this.txtTimeout.Location = new System.Drawing.Point(133, 359);
		this.txtTimeout.Margin = new System.Windows.Forms.Padding(4);
		this.txtTimeout.Mask = "999";
		this.txtTimeout.Name = "txtTimeout";
		this.txtTimeout.Size = new System.Drawing.Size(416, 22);
		this.txtTimeout.TabIndex = 25;
		this.txtTimeout.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtTimeout.Validating += new System.ComponentModel.CancelEventHandler(TxtTimeout_Validating);
		this.lblTimeout.AutoSize = true;
		this.lblTimeout.Location = new System.Drawing.Point(16, 363);
		this.lblTimeout.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblTimeout.Name = "lblTimeout";
		this.lblTimeout.Size = new System.Drawing.Size(102, 17);
		this.lblTimeout.TabIndex = 24;
		this.lblTimeout.Text = "Timeout (secs)";
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(498, 436);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 30;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(390, 436);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 29;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.passwordExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.passwordExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.passwordExpressionButton.Location = new System.Drawing.Point(559, 265);
		this.passwordExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.passwordExpressionButton.Name = "passwordExpressionButton";
		this.passwordExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.passwordExpressionButton.TabIndex = 21;
		this.passwordExpressionButton.UseVisualStyleBackColor = true;
		this.passwordExpressionButton.Click += new System.EventHandler(PasswordExpressionButton_Click);
		this.txtPassword.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtPassword.Location = new System.Drawing.Point(164, 267);
		this.txtPassword.Margin = new System.Windows.Forms.Padding(4);
		this.txtPassword.MaxLength = 8192;
		this.txtPassword.Name = "txtPassword";
		this.txtPassword.Size = new System.Drawing.Size(385, 22);
		this.txtPassword.TabIndex = 20;
		this.txtPassword.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtPassword.Validating += new System.ComponentModel.CancelEventHandler(TxtPassword_Validating);
		this.lblPassword.AutoSize = true;
		this.lblPassword.Location = new System.Drawing.Point(37, 271);
		this.lblPassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblPassword.Name = "lblPassword";
		this.lblPassword.Size = new System.Drawing.Size(69, 17);
		this.lblPassword.TabIndex = 19;
		this.lblPassword.Text = "Password";
		this.userNameExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.userNameExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.userNameExpressionButton.Location = new System.Drawing.Point(559, 233);
		this.userNameExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.userNameExpressionButton.Name = "userNameExpressionButton";
		this.userNameExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.userNameExpressionButton.TabIndex = 18;
		this.userNameExpressionButton.UseVisualStyleBackColor = true;
		this.userNameExpressionButton.Click += new System.EventHandler(UserNameExpressionButton_Click);
		this.txtUserName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtUserName.Location = new System.Drawing.Point(164, 235);
		this.txtUserName.Margin = new System.Windows.Forms.Padding(4);
		this.txtUserName.MaxLength = 8192;
		this.txtUserName.Name = "txtUserName";
		this.txtUserName.Size = new System.Drawing.Size(385, 22);
		this.txtUserName.TabIndex = 17;
		this.txtUserName.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtUserName.Validating += new System.ComponentModel.CancelEventHandler(TxtUserName_Validating);
		this.lblUserName.AutoSize = true;
		this.lblUserName.Location = new System.Drawing.Point(37, 239);
		this.lblUserName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblUserName.Name = "lblUserName";
		this.lblUserName.Size = new System.Drawing.Size(79, 17);
		this.lblUserName.TabIndex = 16;
		this.lblUserName.Text = "User Name";
		this.serverExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.serverExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.serverExpressionButton.Location = new System.Drawing.Point(559, 135);
		this.serverExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.serverExpressionButton.Name = "serverExpressionButton";
		this.serverExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.serverExpressionButton.TabIndex = 9;
		this.serverExpressionButton.UseVisualStyleBackColor = true;
		this.serverExpressionButton.Click += new System.EventHandler(ServerExpressionButton_Click);
		this.txtServer.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtServer.Location = new System.Drawing.Point(164, 138);
		this.txtServer.Margin = new System.Windows.Forms.Padding(4);
		this.txtServer.MaxLength = 8192;
		this.txtServer.Name = "txtServer";
		this.txtServer.Size = new System.Drawing.Size(385, 22);
		this.txtServer.TabIndex = 8;
		this.txtServer.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtServer.Validating += new System.ComponentModel.CancelEventHandler(TxtServer_Validating);
		this.lblServer.AutoSize = true;
		this.lblServer.Location = new System.Drawing.Point(37, 142);
		this.lblServer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblServer.Name = "lblServer";
		this.lblServer.Size = new System.Drawing.Size(50, 17);
		this.lblServer.TabIndex = 7;
		this.lblServer.Text = "Server";
		this.comboDatabaseType.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboDatabaseType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboDatabaseType.FormattingEnabled = true;
		this.comboDatabaseType.Location = new System.Drawing.Point(133, 15);
		this.comboDatabaseType.Margin = new System.Windows.Forms.Padding(4);
		this.comboDatabaseType.Name = "comboDatabaseType";
		this.comboDatabaseType.Size = new System.Drawing.Size(416, 24);
		this.comboDatabaseType.TabIndex = 1;
		this.lblDatabaseType.AutoSize = true;
		this.lblDatabaseType.Location = new System.Drawing.Point(16, 18);
		this.lblDatabaseType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblDatabaseType.Name = "lblDatabaseType";
		this.lblDatabaseType.Size = new System.Drawing.Size(105, 17);
		this.lblDatabaseType.TabIndex = 0;
		this.lblDatabaseType.Text = "Database Type";
		this.databaseExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.databaseExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.databaseExpressionButton.Location = new System.Drawing.Point(559, 199);
		this.databaseExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.databaseExpressionButton.Name = "databaseExpressionButton";
		this.databaseExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.databaseExpressionButton.TabIndex = 15;
		this.databaseExpressionButton.UseVisualStyleBackColor = true;
		this.databaseExpressionButton.Click += new System.EventHandler(DatabaseExpressionButton_Click);
		this.txtDatabase.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtDatabase.Location = new System.Drawing.Point(164, 202);
		this.txtDatabase.Margin = new System.Windows.Forms.Padding(4);
		this.txtDatabase.MaxLength = 8192;
		this.txtDatabase.Name = "txtDatabase";
		this.txtDatabase.Size = new System.Drawing.Size(385, 22);
		this.txtDatabase.TabIndex = 14;
		this.txtDatabase.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtDatabase.Validating += new System.ComponentModel.CancelEventHandler(TxtDatabase_Validating);
		this.lblDatabase.AutoSize = true;
		this.lblDatabase.Location = new System.Drawing.Point(37, 206);
		this.lblDatabase.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblDatabase.Name = "lblDatabase";
		this.lblDatabase.Size = new System.Drawing.Size(69, 17);
		this.lblDatabase.TabIndex = 13;
		this.lblDatabase.Text = "Database";
		this.txtSqlStatement.AcceptsReturn = true;
		this.txtSqlStatement.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtSqlStatement.HideSelection = false;
		this.txtSqlStatement.Location = new System.Drawing.Point(133, 389);
		this.txtSqlStatement.Margin = new System.Windows.Forms.Padding(4);
		this.txtSqlStatement.MaxLength = 8192;
		this.txtSqlStatement.Name = "txtSqlStatement";
		this.txtSqlStatement.ScrollBars = System.Windows.Forms.ScrollBars.Both;
		this.txtSqlStatement.Size = new System.Drawing.Size(416, 22);
		this.txtSqlStatement.TabIndex = 27;
		this.txtSqlStatement.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtSqlStatement.Validating += new System.ComponentModel.CancelEventHandler(TxtSqlStatement_Validating);
		this.lblSqlStatement.AutoSize = true;
		this.lblSqlStatement.Location = new System.Drawing.Point(16, 392);
		this.lblSqlStatement.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblSqlStatement.Name = "lblSqlStatement";
		this.lblSqlStatement.Size = new System.Drawing.Size(104, 17);
		this.lblSqlStatement.TabIndex = 26;
		this.lblSqlStatement.Text = "SQL Statement";
		this.sqlStatementExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.sqlStatementExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.sqlStatementExpressionButton.Location = new System.Drawing.Point(559, 387);
		this.sqlStatementExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.sqlStatementExpressionButton.Name = "sqlStatementExpressionButton";
		this.sqlStatementExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.sqlStatementExpressionButton.TabIndex = 28;
		this.sqlStatementExpressionButton.UseVisualStyleBackColor = true;
		this.sqlStatementExpressionButton.Click += new System.EventHandler(SqlStatementExpressionButton_Click);
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.portExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.portExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.portExpressionButton.Location = new System.Drawing.Point(559, 167);
		this.portExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.portExpressionButton.Name = "portExpressionButton";
		this.portExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.portExpressionButton.TabIndex = 12;
		this.portExpressionButton.UseVisualStyleBackColor = true;
		this.portExpressionButton.Click += new System.EventHandler(PortExpressionButton_Click);
		this.txtPort.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtPort.Location = new System.Drawing.Point(164, 170);
		this.txtPort.Margin = new System.Windows.Forms.Padding(4);
		this.txtPort.MaxLength = 8192;
		this.txtPort.Name = "txtPort";
		this.txtPort.Size = new System.Drawing.Size(385, 22);
		this.txtPort.TabIndex = 11;
		this.txtPort.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtPort.Validating += new System.ComponentModel.CancelEventHandler(TxtPort_Validating);
		this.lblPort.AutoSize = true;
		this.lblPort.Location = new System.Drawing.Point(37, 174);
		this.lblPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblPort.Name = "lblPort";
		this.lblPort.Size = new System.Drawing.Size(34, 17);
		this.lblPort.TabIndex = 10;
		this.lblPort.Text = "Port";
		this.rbUseConnectionString.AutoSize = true;
		this.rbUseConnectionString.Location = new System.Drawing.Point(16, 49);
		this.rbUseConnectionString.Margin = new System.Windows.Forms.Padding(4);
		this.rbUseConnectionString.Name = "rbUseConnectionString";
		this.rbUseConnectionString.Size = new System.Drawing.Size(202, 21);
		this.rbUseConnectionString.TabIndex = 2;
		this.rbUseConnectionString.TabStop = true;
		this.rbUseConnectionString.Text = "Configure connection string";
		this.rbUseConnectionString.UseVisualStyleBackColor = true;
		this.rbUseConnectionString.CheckedChanged += new System.EventHandler(RbConnectionString_CheckedChanged);
		this.connectionStringExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.connectionStringExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.connectionStringExpressionButton.Location = new System.Drawing.Point(559, 75);
		this.connectionStringExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.connectionStringExpressionButton.Name = "connectionStringExpressionButton";
		this.connectionStringExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.connectionStringExpressionButton.TabIndex = 5;
		this.connectionStringExpressionButton.UseVisualStyleBackColor = true;
		this.connectionStringExpressionButton.Click += new System.EventHandler(ConnectionStringExpressionButton_Click);
		this.txtConnectionString.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtConnectionString.Location = new System.Drawing.Point(164, 78);
		this.txtConnectionString.Margin = new System.Windows.Forms.Padding(4);
		this.txtConnectionString.MaxLength = 8192;
		this.txtConnectionString.Name = "txtConnectionString";
		this.txtConnectionString.Size = new System.Drawing.Size(385, 22);
		this.txtConnectionString.TabIndex = 4;
		this.txtConnectionString.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtConnectionString.Validating += new System.ComponentModel.CancelEventHandler(TxtConnectionString_Validating);
		this.lblConnectionString.AutoSize = true;
		this.lblConnectionString.Location = new System.Drawing.Point(37, 81);
		this.lblConnectionString.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblConnectionString.Name = "lblConnectionString";
		this.lblConnectionString.Size = new System.Drawing.Size(118, 17);
		this.lblConnectionString.TabIndex = 3;
		this.lblConnectionString.Text = "Connection string";
		this.rbUseSeparatedSettings.AutoSize = true;
		this.rbUseSeparatedSettings.Location = new System.Drawing.Point(16, 110);
		this.rbUseSeparatedSettings.Margin = new System.Windows.Forms.Padding(4);
		this.rbUseSeparatedSettings.Name = "rbUseSeparatedSettings";
		this.rbUseSeparatedSettings.Size = new System.Drawing.Size(252, 21);
		this.rbUseSeparatedSettings.TabIndex = 6;
		this.rbUseSeparatedSettings.TabStop = true;
		this.rbUseSeparatedSettings.Text = "Configure each property separately";
		this.rbUseSeparatedSettings.UseVisualStyleBackColor = true;
		this.rbUseSeparatedSettings.CheckedChanged += new System.EventHandler(RbConnectionString_CheckedChanged);
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(619, 478);
		base.Controls.Add(this.rbUseSeparatedSettings);
		base.Controls.Add(this.connectionStringExpressionButton);
		base.Controls.Add(this.txtConnectionString);
		base.Controls.Add(this.lblConnectionString);
		base.Controls.Add(this.rbUseConnectionString);
		base.Controls.Add(this.portExpressionButton);
		base.Controls.Add(this.txtPort);
		base.Controls.Add(this.lblPort);
		base.Controls.Add(this.sqlStatementExpressionButton);
		base.Controls.Add(this.txtSqlStatement);
		base.Controls.Add(this.lblSqlStatement);
		base.Controls.Add(this.databaseExpressionButton);
		base.Controls.Add(this.txtDatabase);
		base.Controls.Add(this.lblDatabase);
		base.Controls.Add(this.comboDatabaseType);
		base.Controls.Add(this.lblDatabaseType);
		base.Controls.Add(this.passwordExpressionButton);
		base.Controls.Add(this.txtPassword);
		base.Controls.Add(this.lblPassword);
		base.Controls.Add(this.userNameExpressionButton);
		base.Controls.Add(this.txtUserName);
		base.Controls.Add(this.lblUserName);
		base.Controls.Add(this.serverExpressionButton);
		base.Controls.Add(this.txtServer);
		base.Controls.Add(this.lblServer);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.Controls.Add(this.lblTimeout);
		base.Controls.Add(this.txtTimeout);
		base.Controls.Add(this.comboStatementType);
		base.Controls.Add(this.lblStatementType);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(1359, 525);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(637, 525);
		base.Name = "DatabaseAccessConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Database Access";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(DatabaseAccessConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(DatabaseAccessConfigurationForm_HelpRequested);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
