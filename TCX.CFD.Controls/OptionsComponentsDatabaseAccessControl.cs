using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class OptionsComponentsDatabaseAccessControl : UserControl, IOptionsControl
{
	private IContainer components;

	private MaskedTextBox txtTimeout;

	private Label lblTimeout;

	private TextBox txtDatabase;

	private Label lblDatabase;

	private TextBox txtPassword;

	private Label lblPassword;

	private TextBox txtUserName;

	private Label lblUserName;

	private TextBox txtServer;

	private ComboBox comboStatementType;

	private Label lblStatementType;

	private Label lblServer;

	private ComboBox comboDatabaseType;

	private Label lblDatabaseType;

	private ErrorProvider errorProvider;

	private MaskedTextBox txtPort;

	private Label lblPort;

	private RadioButton rbUseSeparatedSettings;

	private TextBox txtConnectionString;

	private Label lblConnectionString;

	private RadioButton rbUseConnectionString;

	private Label lblDatabaseAccess;

	private GroupBox grpBoxSeparately;

	private GroupBox grpBoxConnectionString;

	private DatabaseTypes GetDatabaseType(string str)
	{
		if (!(str == "SqlServer"))
		{
			if (str == "PostgreSQL")
			{
				return DatabaseTypes.PostgreSQL;
			}
			return DatabaseTypes.MySQL;
		}
		return DatabaseTypes.SqlServer;
	}

	private string GetDatabaseTypeAsStr(DatabaseTypes databaseTypes)
	{
		return databaseTypes switch
		{
			DatabaseTypes.SqlServer => "SqlServer", 
			DatabaseTypes.PostgreSQL => "PostgreSQL", 
			_ => "MySQL", 
		};
	}

	private SqlStatementTypes GetStatementType(string str)
	{
		if (!(str == "Query"))
		{
			if (str == "NonQuery")
			{
				return SqlStatementTypes.NonQuery;
			}
			return SqlStatementTypes.Scalar;
		}
		return SqlStatementTypes.Query;
	}

	private string GetStatementTypeAsStr(SqlStatementTypes statementType)
	{
		return statementType switch
		{
			SqlStatementTypes.Query => "Query", 
			SqlStatementTypes.NonQuery => "NonQuery", 
			_ => "Scalar", 
		};
	}

	public OptionsComponentsDatabaseAccessControl()
	{
		InitializeComponent();
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
		comboDatabaseType.SelectedItem = GetDatabaseType(Settings.Default.DatabaseAccessTemplateDatabaseType);
		rbUseConnectionString.Checked = Settings.Default.DatabaseAccessTemplateUseConnectionString;
		rbUseSeparatedSettings.Checked = !Settings.Default.DatabaseAccessTemplateUseConnectionString;
		txtConnectionString.Text = Settings.Default.DatabaseAccessTemplateConnectionString;
		txtServer.Text = Settings.Default.DatabaseAccessTemplateServer;
		txtPort.Text = ((Settings.Default.DatabaseAccessTemplatePort == -1) ? string.Empty : Settings.Default.DatabaseAccessTemplatePort.ToString());
		txtDatabase.Text = Settings.Default.DatabaseAccessTemplateDatabase;
		txtUserName.Text = Settings.Default.DatabaseAccessTemplateUserName;
		txtPassword.Text = Settings.Default.DatabaseAccessTemplatePassword;
		comboStatementType.SelectedItem = GetStatementType(Settings.Default.DatabaseAccessTemplateStatementType);
		txtTimeout.Text = Settings.Default.DatabaseAccessTemplateTimeout.ToString();
		lblDatabaseAccess.Text = LocalizedResourceMgr.GetString("OptionsComponentsDatabaseAccessControl.lblDatabaseAccess.Text");
		lblDatabaseType.Text = LocalizedResourceMgr.GetString("OptionsComponentsDatabaseAccessControl.lblDatabaseType.Text");
		rbUseConnectionString.Text = LocalizedResourceMgr.GetString("OptionsComponentsDatabaseAccessControl.rbUseConnectionString.Text");
		rbUseSeparatedSettings.Text = LocalizedResourceMgr.GetString("OptionsComponentsDatabaseAccessControl.rbUseSeparatedSettings.Text");
		lblConnectionString.Text = LocalizedResourceMgr.GetString("OptionsComponentsDatabaseAccessControl.lblConnectionString.Text");
		lblServer.Text = LocalizedResourceMgr.GetString("OptionsComponentsDatabaseAccessControl.lblServer.Text");
		lblPort.Text = LocalizedResourceMgr.GetString("OptionsComponentsDatabaseAccessControl.lblPort.Text");
		lblDatabase.Text = LocalizedResourceMgr.GetString("OptionsComponentsDatabaseAccessControl.lblDatabase.Text");
		lblUserName.Text = LocalizedResourceMgr.GetString("OptionsComponentsDatabaseAccessControl.lblUserName.Text");
		lblPassword.Text = LocalizedResourceMgr.GetString("OptionsComponentsDatabaseAccessControl.lblPassword.Text");
		lblStatementType.Text = LocalizedResourceMgr.GetString("OptionsComponentsDatabaseAccessControl.lblStatementType.Text");
		lblTimeout.Text = LocalizedResourceMgr.GetString("OptionsComponentsDatabaseAccessControl.lblTimeout.Text");
	}

	private void RbConnectionString_CheckedChanged(object sender, EventArgs e)
	{
		txtConnectionString.Enabled = rbUseConnectionString.Checked;
		txtServer.Enabled = rbUseSeparatedSettings.Checked;
		txtPort.Enabled = rbUseSeparatedSettings.Checked;
		txtDatabase.Enabled = rbUseSeparatedSettings.Checked;
		txtUserName.Enabled = rbUseSeparatedSettings.Checked;
		txtPassword.Enabled = rbUseSeparatedSettings.Checked;
	}

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void MaskedTextBox_GotFocus(object sender, EventArgs e)
	{
		((MaskedTextBox)sender).SelectAll();
	}

	private void ValidatePort()
	{
		if (!string.IsNullOrEmpty(txtPort.Text))
		{
			int num = Convert.ToInt32(txtPort.Text);
			if (num < 1 || num > 65535)
			{
				throw new ApplicationException(string.Format(LocalizedResourceMgr.GetString("OptionsComponentsDatabaseAccessControl.Error.InvalidPortValue"), 1, 65535));
			}
		}
	}

	private void ValidateTimeout()
	{
		if (string.IsNullOrEmpty(txtTimeout.Text))
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsDatabaseAccessControl.Error.TimeoutIsMandatory"));
		}
	}

	private void ValidateFields()
	{
		ValidatePort();
		ValidateTimeout();
	}

	private void TxtPort_Validating(object sender, CancelEventArgs e)
	{
		try
		{
			ValidatePort();
			errorProvider.SetError(txtPort, string.Empty);
		}
		catch (Exception exc)
		{
			errorProvider.SetError(txtPort, ErrorHelper.GetErrorDescription(exc));
		}
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

	public void Save()
	{
		ValidateFields();
		Settings.Default.DatabaseAccessTemplateUseConnectionString = rbUseConnectionString.Checked;
		Settings.Default.DatabaseAccessTemplateConnectionString = txtConnectionString.Text;
		Settings.Default.DatabaseAccessTemplateDatabaseType = GetDatabaseTypeAsStr((DatabaseTypes)comboDatabaseType.SelectedItem);
		Settings.Default.DatabaseAccessTemplateServer = txtServer.Text;
		Settings.Default.DatabaseAccessTemplatePort = (string.IsNullOrEmpty(txtPort.Text) ? (-1) : Convert.ToInt32(txtPort.Text));
		Settings.Default.DatabaseAccessTemplateDatabase = txtDatabase.Text;
		Settings.Default.DatabaseAccessTemplateUserName = txtUserName.Text;
		Settings.Default.DatabaseAccessTemplatePassword = txtPassword.Text;
		Settings.Default.DatabaseAccessTemplateStatementType = GetStatementTypeAsStr((SqlStatementTypes)comboStatementType.SelectedItem);
		Settings.Default.DatabaseAccessTemplateTimeout = Convert.ToUInt32(txtTimeout.Text);
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
		this.txtTimeout = new System.Windows.Forms.MaskedTextBox();
		this.lblTimeout = new System.Windows.Forms.Label();
		this.txtDatabase = new System.Windows.Forms.TextBox();
		this.lblDatabase = new System.Windows.Forms.Label();
		this.txtPassword = new System.Windows.Forms.TextBox();
		this.lblPassword = new System.Windows.Forms.Label();
		this.txtUserName = new System.Windows.Forms.TextBox();
		this.lblUserName = new System.Windows.Forms.Label();
		this.txtServer = new System.Windows.Forms.TextBox();
		this.comboStatementType = new System.Windows.Forms.ComboBox();
		this.lblStatementType = new System.Windows.Forms.Label();
		this.lblServer = new System.Windows.Forms.Label();
		this.comboDatabaseType = new System.Windows.Forms.ComboBox();
		this.lblDatabaseType = new System.Windows.Forms.Label();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.txtPort = new System.Windows.Forms.MaskedTextBox();
		this.lblPort = new System.Windows.Forms.Label();
		this.rbUseConnectionString = new System.Windows.Forms.RadioButton();
		this.txtConnectionString = new System.Windows.Forms.TextBox();
		this.lblConnectionString = new System.Windows.Forms.Label();
		this.rbUseSeparatedSettings = new System.Windows.Forms.RadioButton();
		this.lblDatabaseAccess = new System.Windows.Forms.Label();
		this.grpBoxConnectionString = new System.Windows.Forms.GroupBox();
		this.grpBoxSeparately = new System.Windows.Forms.GroupBox();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		this.grpBoxConnectionString.SuspendLayout();
		this.grpBoxSeparately.SuspendLayout();
		base.SuspendLayout();
		this.txtTimeout.HidePromptOnLeave = true;
		this.txtTimeout.Location = new System.Drawing.Point(14, 541);
		this.txtTimeout.Margin = new System.Windows.Forms.Padding(4);
		this.txtTimeout.Mask = "999";
		this.txtTimeout.Name = "txtTimeout";
		this.txtTimeout.Size = new System.Drawing.Size(298, 22);
		this.txtTimeout.TabIndex = 19;
		this.txtTimeout.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtTimeout.Validating += new System.ComponentModel.CancelEventHandler(TxtTimeout_Validating);
		this.lblTimeout.AutoSize = true;
		this.lblTimeout.Location = new System.Drawing.Point(11, 520);
		this.lblTimeout.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblTimeout.Name = "lblTimeout";
		this.lblTimeout.Size = new System.Drawing.Size(102, 17);
		this.lblTimeout.TabIndex = 18;
		this.lblTimeout.Text = "Timeout (secs)";
		this.txtDatabase.Location = new System.Drawing.Point(10, 138);
		this.txtDatabase.Margin = new System.Windows.Forms.Padding(4);
		this.txtDatabase.MaxLength = 255;
		this.txtDatabase.Name = "txtDatabase";
		this.txtDatabase.Size = new System.Drawing.Size(678, 22);
		this.txtDatabase.TabIndex = 11;
		this.txtDatabase.Enter += new System.EventHandler(TxtBox_Enter);
		this.lblDatabase.AutoSize = true;
		this.lblDatabase.Location = new System.Drawing.Point(7, 117);
		this.lblDatabase.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblDatabase.Name = "lblDatabase";
		this.lblDatabase.Size = new System.Drawing.Size(69, 17);
		this.lblDatabase.TabIndex = 10;
		this.lblDatabase.Text = "Database";
		this.txtPassword.Location = new System.Drawing.Point(10, 232);
		this.txtPassword.Margin = new System.Windows.Forms.Padding(4);
		this.txtPassword.MaxLength = 255;
		this.txtPassword.Name = "txtPassword";
		this.txtPassword.PasswordChar = '*';
		this.txtPassword.Size = new System.Drawing.Size(678, 22);
		this.txtPassword.TabIndex = 15;
		this.txtPassword.Enter += new System.EventHandler(TxtBox_Enter);
		this.lblPassword.AutoSize = true;
		this.lblPassword.Location = new System.Drawing.Point(7, 211);
		this.lblPassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblPassword.Name = "lblPassword";
		this.lblPassword.Size = new System.Drawing.Size(69, 17);
		this.lblPassword.TabIndex = 14;
		this.lblPassword.Text = "Password";
		this.txtUserName.Location = new System.Drawing.Point(10, 185);
		this.txtUserName.Margin = new System.Windows.Forms.Padding(4);
		this.txtUserName.MaxLength = 255;
		this.txtUserName.Name = "txtUserName";
		this.txtUserName.Size = new System.Drawing.Size(678, 22);
		this.txtUserName.TabIndex = 13;
		this.txtUserName.Enter += new System.EventHandler(TxtBox_Enter);
		this.lblUserName.AutoSize = true;
		this.lblUserName.Location = new System.Drawing.Point(7, 164);
		this.lblUserName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblUserName.Name = "lblUserName";
		this.lblUserName.Size = new System.Drawing.Size(79, 17);
		this.lblUserName.TabIndex = 12;
		this.lblUserName.Text = "User Name";
		this.txtServer.Location = new System.Drawing.Point(10, 44);
		this.txtServer.Margin = new System.Windows.Forms.Padding(4);
		this.txtServer.MaxLength = 255;
		this.txtServer.Name = "txtServer";
		this.txtServer.Size = new System.Drawing.Size(678, 22);
		this.txtServer.TabIndex = 7;
		this.txtServer.Enter += new System.EventHandler(TxtBox_Enter);
		this.comboStatementType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboStatementType.FormattingEnabled = true;
		this.comboStatementType.Location = new System.Drawing.Point(14, 492);
		this.comboStatementType.Margin = new System.Windows.Forms.Padding(4);
		this.comboStatementType.Name = "comboStatementType";
		this.comboStatementType.Size = new System.Drawing.Size(298, 24);
		this.comboStatementType.TabIndex = 17;
		this.lblStatementType.AutoSize = true;
		this.lblStatementType.Location = new System.Drawing.Point(11, 471);
		this.lblStatementType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblStatementType.Name = "lblStatementType";
		this.lblStatementType.Size = new System.Drawing.Size(108, 17);
		this.lblStatementType.TabIndex = 16;
		this.lblStatementType.Text = "Statement Type";
		this.lblServer.AutoSize = true;
		this.lblServer.Location = new System.Drawing.Point(7, 23);
		this.lblServer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblServer.Name = "lblServer";
		this.lblServer.Size = new System.Drawing.Size(50, 17);
		this.lblServer.TabIndex = 6;
		this.lblServer.Text = "Server";
		this.comboDatabaseType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboDatabaseType.FormattingEnabled = true;
		this.comboDatabaseType.Location = new System.Drawing.Point(12, 59);
		this.comboDatabaseType.Margin = new System.Windows.Forms.Padding(4);
		this.comboDatabaseType.Name = "comboDatabaseType";
		this.comboDatabaseType.Size = new System.Drawing.Size(300, 24);
		this.comboDatabaseType.TabIndex = 1;
		this.lblDatabaseType.AutoSize = true;
		this.lblDatabaseType.Location = new System.Drawing.Point(9, 38);
		this.lblDatabaseType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblDatabaseType.Name = "lblDatabaseType";
		this.lblDatabaseType.Size = new System.Drawing.Size(105, 17);
		this.lblDatabaseType.TabIndex = 0;
		this.lblDatabaseType.Text = "Database Type";
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.txtPort.HidePromptOnLeave = true;
		this.txtPort.Location = new System.Drawing.Point(10, 91);
		this.txtPort.Margin = new System.Windows.Forms.Padding(4);
		this.txtPort.Mask = "99999";
		this.txtPort.Name = "txtPort";
		this.txtPort.Size = new System.Drawing.Size(678, 22);
		this.txtPort.TabIndex = 9;
		this.txtPort.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtPort.Validating += new System.ComponentModel.CancelEventHandler(TxtPort_Validating);
		this.lblPort.AutoSize = true;
		this.lblPort.Location = new System.Drawing.Point(7, 70);
		this.lblPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblPort.Name = "lblPort";
		this.lblPort.Size = new System.Drawing.Size(34, 17);
		this.lblPort.TabIndex = 8;
		this.lblPort.Text = "Port";
		this.rbUseConnectionString.AutoSize = true;
		this.rbUseConnectionString.Location = new System.Drawing.Point(20, 96);
		this.rbUseConnectionString.Margin = new System.Windows.Forms.Padding(4);
		this.rbUseConnectionString.Name = "rbUseConnectionString";
		this.rbUseConnectionString.Size = new System.Drawing.Size(202, 21);
		this.rbUseConnectionString.TabIndex = 2;
		this.rbUseConnectionString.TabStop = true;
		this.rbUseConnectionString.Text = "Configure connection string";
		this.rbUseConnectionString.UseVisualStyleBackColor = true;
		this.rbUseConnectionString.CheckedChanged += new System.EventHandler(RbConnectionString_CheckedChanged);
		this.txtConnectionString.Location = new System.Drawing.Point(10, 44);
		this.txtConnectionString.Margin = new System.Windows.Forms.Padding(4);
		this.txtConnectionString.MaxLength = 255;
		this.txtConnectionString.Name = "txtConnectionString";
		this.txtConnectionString.Size = new System.Drawing.Size(678, 22);
		this.txtConnectionString.TabIndex = 4;
		this.txtConnectionString.Enter += new System.EventHandler(TxtBox_Enter);
		this.lblConnectionString.AutoSize = true;
		this.lblConnectionString.Location = new System.Drawing.Point(7, 23);
		this.lblConnectionString.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblConnectionString.Name = "lblConnectionString";
		this.lblConnectionString.Size = new System.Drawing.Size(118, 17);
		this.lblConnectionString.TabIndex = 3;
		this.lblConnectionString.Text = "Connection string";
		this.rbUseSeparatedSettings.AutoSize = true;
		this.rbUseSeparatedSettings.Location = new System.Drawing.Point(20, 185);
		this.rbUseSeparatedSettings.Margin = new System.Windows.Forms.Padding(4);
		this.rbUseSeparatedSettings.Name = "rbUseSeparatedSettings";
		this.rbUseSeparatedSettings.Size = new System.Drawing.Size(252, 21);
		this.rbUseSeparatedSettings.TabIndex = 5;
		this.rbUseSeparatedSettings.TabStop = true;
		this.rbUseSeparatedSettings.Text = "Configure each property separately";
		this.rbUseSeparatedSettings.UseVisualStyleBackColor = true;
		this.rbUseSeparatedSettings.CheckedChanged += new System.EventHandler(RbConnectionString_CheckedChanged);
		this.lblDatabaseAccess.AutoSize = true;
		this.lblDatabaseAccess.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDatabaseAccess.Location = new System.Drawing.Point(8, 8);
		this.lblDatabaseAccess.Name = "lblDatabaseAccess";
		this.lblDatabaseAccess.Size = new System.Drawing.Size(157, 20);
		this.lblDatabaseAccess.TabIndex = 20;
		this.lblDatabaseAccess.Text = "Database Access";
		this.grpBoxConnectionString.Controls.Add(this.lblConnectionString);
		this.grpBoxConnectionString.Controls.Add(this.txtConnectionString);
		this.grpBoxConnectionString.Location = new System.Drawing.Point(14, 100);
		this.grpBoxConnectionString.Name = "grpBoxConnectionString";
		this.grpBoxConnectionString.Size = new System.Drawing.Size(722, 83);
		this.grpBoxConnectionString.TabIndex = 21;
		this.grpBoxConnectionString.TabStop = false;
		this.grpBoxSeparately.Controls.Add(this.lblServer);
		this.grpBoxSeparately.Controls.Add(this.txtServer);
		this.grpBoxSeparately.Controls.Add(this.txtPort);
		this.grpBoxSeparately.Controls.Add(this.lblUserName);
		this.grpBoxSeparately.Controls.Add(this.lblPort);
		this.grpBoxSeparately.Controls.Add(this.txtUserName);
		this.grpBoxSeparately.Controls.Add(this.lblPassword);
		this.grpBoxSeparately.Controls.Add(this.txtPassword);
		this.grpBoxSeparately.Controls.Add(this.lblDatabase);
		this.grpBoxSeparately.Controls.Add(this.txtDatabase);
		this.grpBoxSeparately.Location = new System.Drawing.Point(14, 189);
		this.grpBoxSeparately.Name = "grpBoxSeparately";
		this.grpBoxSeparately.Size = new System.Drawing.Size(722, 270);
		this.grpBoxSeparately.TabIndex = 22;
		this.grpBoxSeparately.TabStop = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.rbUseSeparatedSettings);
		base.Controls.Add(this.rbUseConnectionString);
		base.Controls.Add(this.grpBoxSeparately);
		base.Controls.Add(this.grpBoxConnectionString);
		base.Controls.Add(this.lblDatabaseAccess);
		base.Controls.Add(this.comboDatabaseType);
		base.Controls.Add(this.lblDatabaseType);
		base.Controls.Add(this.txtTimeout);
		base.Controls.Add(this.lblTimeout);
		base.Controls.Add(this.comboStatementType);
		base.Controls.Add(this.lblStatementType);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "OptionsComponentsDatabaseAccessControl";
		base.Size = new System.Drawing.Size(780, 665);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		this.grpBoxConnectionString.ResumeLayout(false);
		this.grpBoxConnectionString.PerformLayout();
		this.grpBoxSeparately.ResumeLayout(false);
		this.grpBoxSeparately.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
