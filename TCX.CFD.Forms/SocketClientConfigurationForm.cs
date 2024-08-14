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

public class SocketClientConfigurationForm : Form
{
	private readonly SocketClientComponent socketClientComponent;

	private readonly List<string> validVariables;

	private IContainer components;

	private Button cancelButton;

	private Button okButton;

	private Label lblConnectionType;

	private ComboBox comboConnectionType;

	private Label lblPort;

	private Label lblHost;

	private TextBox txtPort;

	private TextBox txtHost;

	private Button hostExpressionButton;

	private Label lblData;

	private TextBox txtData;

	private Button dataExpressionButton;

	private Button portExpressionButton;

	private CheckBox chkWaitForResponse;

	private ErrorProvider errorProvider;

	public SocketConnectionTypes ConnectionType => (SocketConnectionTypes)comboConnectionType.SelectedItem;

	public string Host => txtHost.Text;

	public string Port => txtPort.Text;

	public string Data => txtData.Text;

	public bool WaitForResponse => chkWaitForResponse.Checked;

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void TxtHost_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtHost.Text))
		{
			errorProvider.SetError(hostExpressionButton, LocalizedResourceMgr.GetString("SocketClientConfigurationForm.Error.HostIsMandatory"));
			return;
		}
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtHost.Text);
		errorProvider.SetError(hostExpressionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("SocketClientConfigurationForm.Error.HostIsInvalid"));
	}

	private void TxtPort_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtPort.Text))
		{
			errorProvider.SetError(portExpressionButton, LocalizedResourceMgr.GetString("SocketClientConfigurationForm.Error.PortIsMandatory"));
			return;
		}
		if (int.TryParse(txtPort.Text, out var result) && (result < 1 || result > 65535))
		{
			errorProvider.SetError(portExpressionButton, string.Format(LocalizedResourceMgr.GetString("SocketClientConfigurationForm.Error.InvalidPortValue"), 1, 65535));
			return;
		}
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtPort.Text);
		errorProvider.SetError(portExpressionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("SocketClientConfigurationForm.Error.PortIsInvalid"));
	}

	private void TxtData_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtData.Text))
		{
			errorProvider.SetError(dataExpressionButton, LocalizedResourceMgr.GetString("SocketClientConfigurationForm.Error.DataIsMandatory"));
			return;
		}
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtData.Text);
		errorProvider.SetError(dataExpressionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("SocketClientConfigurationForm.Error.DataIsInvalid"));
	}

	private void HostExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(socketClientComponent);
		expressionEditorForm.Expression = txtHost.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtHost.Text = expressionEditorForm.Expression;
			TxtHost_Validating(txtHost, new CancelEventArgs());
		}
	}

	private void PortExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(socketClientComponent);
		expressionEditorForm.Expression = txtPort.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtPort.Text = expressionEditorForm.Expression;
			TxtPort_Validating(txtPort, new CancelEventArgs());
		}
	}

	private void DataExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(socketClientComponent);
		expressionEditorForm.Expression = txtData.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtData.Text = expressionEditorForm.Expression;
			TxtData_Validating(txtData, new CancelEventArgs());
		}
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.OK;
		Close();
	}

	public SocketClientConfigurationForm(SocketClientComponent socketClientComponent)
	{
		InitializeComponent();
		this.socketClientComponent = socketClientComponent;
		validVariables = ExpressionHelper.GetValidVariables(socketClientComponent);
		comboConnectionType.Items.AddRange(new object[2]
		{
			SocketConnectionTypes.TCP,
			SocketConnectionTypes.UDP
		});
		comboConnectionType.SelectedItem = socketClientComponent.ConnectionType;
		txtHost.Text = socketClientComponent.Host;
		txtPort.Text = socketClientComponent.Port;
		txtData.Text = socketClientComponent.Data;
		chkWaitForResponse.Checked = socketClientComponent.WaitForResponse;
		TxtHost_Validating(txtHost, new CancelEventArgs());
		TxtPort_Validating(txtPort, new CancelEventArgs());
		TxtData_Validating(txtData, new CancelEventArgs());
		Text = LocalizedResourceMgr.GetString("SocketClientConfigurationForm.Title");
		lblConnectionType.Text = LocalizedResourceMgr.GetString("SocketClientConfigurationForm.lblConnectionType.Text");
		lblHost.Text = LocalizedResourceMgr.GetString("SocketClientConfigurationForm.lblHost.Text");
		lblPort.Text = LocalizedResourceMgr.GetString("SocketClientConfigurationForm.lblPort.Text");
		lblData.Text = LocalizedResourceMgr.GetString("SocketClientConfigurationForm.lblData.Text");
		chkWaitForResponse.Text = LocalizedResourceMgr.GetString("SocketClientConfigurationForm.chkWaitForResponse.Text");
		okButton.Text = LocalizedResourceMgr.GetString("SocketClientConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("SocketClientConfigurationForm.cancelButton.Text");
	}

	private void SocketClientConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		socketClientComponent.ShowHelp();
	}

	private void SocketClientConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		socketClientComponent.ShowHelp();
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
		this.chkWaitForResponse = new System.Windows.Forms.CheckBox();
		this.dataExpressionButton = new System.Windows.Forms.Button();
		this.portExpressionButton = new System.Windows.Forms.Button();
		this.lblData = new System.Windows.Forms.Label();
		this.txtData = new System.Windows.Forms.TextBox();
		this.hostExpressionButton = new System.Windows.Forms.Button();
		this.lblPort = new System.Windows.Forms.Label();
		this.lblHost = new System.Windows.Forms.Label();
		this.txtPort = new System.Windows.Forms.TextBox();
		this.txtHost = new System.Windows.Forms.TextBox();
		this.comboConnectionType = new System.Windows.Forms.ComboBox();
		this.lblConnectionType = new System.Windows.Forms.Label();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(497, 177);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 13;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(389, 177);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 12;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.chkWaitForResponse.AutoSize = true;
		this.chkWaitForResponse.Location = new System.Drawing.Point(20, 144);
		this.chkWaitForResponse.Margin = new System.Windows.Forms.Padding(4);
		this.chkWaitForResponse.Name = "chkWaitForResponse";
		this.chkWaitForResponse.Size = new System.Drawing.Size(218, 21);
		this.chkWaitForResponse.TabIndex = 11;
		this.chkWaitForResponse.Text = "Wait for response from server";
		this.chkWaitForResponse.UseVisualStyleBackColor = true;
		this.dataExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.dataExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.dataExpressionButton.Location = new System.Drawing.Point(559, 110);
		this.dataExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.dataExpressionButton.Name = "dataExpressionButton";
		this.dataExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.dataExpressionButton.TabIndex = 10;
		this.dataExpressionButton.UseVisualStyleBackColor = true;
		this.dataExpressionButton.Click += new System.EventHandler(DataExpressionButton_Click);
		this.portExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.portExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.portExpressionButton.Location = new System.Drawing.Point(559, 78);
		this.portExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.portExpressionButton.Name = "portExpressionButton";
		this.portExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.portExpressionButton.TabIndex = 7;
		this.portExpressionButton.UseVisualStyleBackColor = true;
		this.portExpressionButton.Click += new System.EventHandler(PortExpressionButton_Click);
		this.lblData.AutoSize = true;
		this.lblData.Location = new System.Drawing.Point(16, 116);
		this.lblData.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblData.Name = "lblData";
		this.lblData.Size = new System.Drawing.Size(89, 17);
		this.lblData.TabIndex = 8;
		this.lblData.Text = "Data to send";
		this.txtData.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtData.Location = new System.Drawing.Point(141, 112);
		this.txtData.Margin = new System.Windows.Forms.Padding(4);
		this.txtData.MaxLength = 8192;
		this.txtData.Name = "txtData";
		this.txtData.Size = new System.Drawing.Size(408, 22);
		this.txtData.TabIndex = 9;
		this.txtData.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtData.Validating += new System.ComponentModel.CancelEventHandler(TxtData_Validating);
		this.hostExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.hostExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.hostExpressionButton.Location = new System.Drawing.Point(559, 46);
		this.hostExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.hostExpressionButton.Name = "hostExpressionButton";
		this.hostExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.hostExpressionButton.TabIndex = 4;
		this.hostExpressionButton.UseVisualStyleBackColor = true;
		this.hostExpressionButton.Click += new System.EventHandler(HostExpressionButton_Click);
		this.lblPort.AutoSize = true;
		this.lblPort.Location = new System.Drawing.Point(16, 84);
		this.lblPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblPort.Name = "lblPort";
		this.lblPort.Size = new System.Drawing.Size(34, 17);
		this.lblPort.TabIndex = 5;
		this.lblPort.Text = "Port";
		this.lblHost.AutoSize = true;
		this.lblHost.Location = new System.Drawing.Point(16, 52);
		this.lblHost.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblHost.Name = "lblHost";
		this.lblHost.Size = new System.Drawing.Size(83, 17);
		this.lblHost.TabIndex = 2;
		this.lblHost.Text = "Server Host";
		this.txtPort.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtPort.Location = new System.Drawing.Point(141, 80);
		this.txtPort.Margin = new System.Windows.Forms.Padding(4);
		this.txtPort.MaxLength = 8192;
		this.txtPort.Name = "txtPort";
		this.txtPort.Size = new System.Drawing.Size(408, 22);
		this.txtPort.TabIndex = 6;
		this.txtPort.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtPort.Validating += new System.ComponentModel.CancelEventHandler(TxtPort_Validating);
		this.txtHost.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtHost.Location = new System.Drawing.Point(141, 48);
		this.txtHost.Margin = new System.Windows.Forms.Padding(4);
		this.txtHost.MaxLength = 8192;
		this.txtHost.Name = "txtHost";
		this.txtHost.Size = new System.Drawing.Size(408, 22);
		this.txtHost.TabIndex = 3;
		this.txtHost.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtHost.Validating += new System.ComponentModel.CancelEventHandler(TxtHost_Validating);
		this.comboConnectionType.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboConnectionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboConnectionType.FormattingEnabled = true;
		this.comboConnectionType.Location = new System.Drawing.Point(141, 15);
		this.comboConnectionType.Margin = new System.Windows.Forms.Padding(4);
		this.comboConnectionType.Name = "comboConnectionType";
		this.comboConnectionType.Size = new System.Drawing.Size(455, 24);
		this.comboConnectionType.TabIndex = 1;
		this.lblConnectionType.AutoSize = true;
		this.lblConnectionType.Location = new System.Drawing.Point(16, 18);
		this.lblConnectionType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblConnectionType.Name = "lblConnectionType";
		this.lblConnectionType.Size = new System.Drawing.Size(115, 17);
		this.lblConnectionType.TabIndex = 0;
		this.lblConnectionType.Text = "Connection Type";
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(619, 210);
		base.Controls.Add(this.chkWaitForResponse);
		base.Controls.Add(this.comboConnectionType);
		base.Controls.Add(this.dataExpressionButton);
		base.Controls.Add(this.lblConnectionType);
		base.Controls.Add(this.portExpressionButton);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.lblData);
		base.Controls.Add(this.okButton);
		base.Controls.Add(this.txtData);
		base.Controls.Add(this.lblHost);
		base.Controls.Add(this.hostExpressionButton);
		base.Controls.Add(this.txtHost);
		base.Controls.Add(this.lblPort);
		base.Controls.Add(this.txtPort);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(1359, 257);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(634, 257);
		base.Name = "SocketClientConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Open a Socket";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(SocketClientConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(SocketClientConfigurationForm_HelpRequested);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
