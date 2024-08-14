using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class OptionsComponentsSocketClientControl : UserControl, IOptionsControl
{
	private IContainer components;

	private CheckBox chkWaitForResponse;

	private ComboBox comboConnectionType;

	private Label lblConnectionType;

	private Label lblHost;

	private TextBox txtHost;

	private Label lblPort;

	private MaskedTextBox txtPort;

	private ErrorProvider errorProvider;

	private Label lblOpenASocket;

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void MaskedTextBox_GotFocus(object sender, EventArgs e)
	{
		(sender as MaskedTextBox).SelectAll();
	}

	private void ValidatePort()
	{
		if (string.IsNullOrEmpty(txtPort.Text))
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsSocketClientControl.Error.PortIsMandatory"));
		}
		int num = Convert.ToInt32(txtPort.Text);
		if (num < 1 || num > 65535)
		{
			throw new ApplicationException(string.Format(LocalizedResourceMgr.GetString("OptionsComponentsSocketClientControl.Error.InvalidPortValue"), 1, 65535));
		}
	}

	private void ValidateFields()
	{
		ValidatePort();
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

	public OptionsComponentsSocketClientControl()
	{
		InitializeComponent();
		comboConnectionType.Items.AddRange(new object[2]
		{
			SocketConnectionTypes.TCP,
			SocketConnectionTypes.UDP
		});
		comboConnectionType.SelectedItem = ((!(Settings.Default.SocketClientTemplateConnectionType == "TCP")) ? SocketConnectionTypes.UDP : SocketConnectionTypes.TCP);
		txtHost.Text = Settings.Default.SocketClientTemplateHost;
		txtPort.Text = Settings.Default.SocketClientTemplatePort.ToString();
		chkWaitForResponse.Checked = Settings.Default.SocketClientTemplateWaitForResponse;
		lblOpenASocket.Text = LocalizedResourceMgr.GetString("OptionsComponentsSocketClientControl.lblOpenASocket.Text");
		lblConnectionType.Text = LocalizedResourceMgr.GetString("OptionsComponentsSocketClientControl.lblConnectionType.Text");
		lblHost.Text = LocalizedResourceMgr.GetString("OptionsComponentsSocketClientControl.lblHost.Text");
		lblPort.Text = LocalizedResourceMgr.GetString("OptionsComponentsSocketClientControl.lblPort.Text");
		chkWaitForResponse.Text = LocalizedResourceMgr.GetString("OptionsComponentsSocketClientControl.chkWaitForResponse.Text");
	}

	public void Save()
	{
		ValidateFields();
		Settings.Default.SocketClientTemplateConnectionType = (((SocketConnectionTypes)comboConnectionType.SelectedItem == SocketConnectionTypes.TCP) ? "TCP" : "UDP");
		Settings.Default.SocketClientTemplateHost = txtHost.Text;
		Settings.Default.SocketClientTemplatePort = Convert.ToUInt32(txtPort.Text);
		Settings.Default.SocketClientTemplateWaitForResponse = chkWaitForResponse.Checked;
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
		this.chkWaitForResponse = new System.Windows.Forms.CheckBox();
		this.comboConnectionType = new System.Windows.Forms.ComboBox();
		this.lblConnectionType = new System.Windows.Forms.Label();
		this.lblHost = new System.Windows.Forms.Label();
		this.txtHost = new System.Windows.Forms.TextBox();
		this.lblPort = new System.Windows.Forms.Label();
		this.txtPort = new System.Windows.Forms.MaskedTextBox();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.lblOpenASocket = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.chkWaitForResponse.AutoSize = true;
		this.chkWaitForResponse.Location = new System.Drawing.Point(12, 185);
		this.chkWaitForResponse.Margin = new System.Windows.Forms.Padding(4);
		this.chkWaitForResponse.Name = "chkWaitForResponse";
		this.chkWaitForResponse.Size = new System.Drawing.Size(218, 21);
		this.chkWaitForResponse.TabIndex = 6;
		this.chkWaitForResponse.Text = "Wait for response from server";
		this.chkWaitForResponse.UseVisualStyleBackColor = true;
		this.comboConnectionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboConnectionType.FormattingEnabled = true;
		this.comboConnectionType.Location = new System.Drawing.Point(12, 59);
		this.comboConnectionType.Margin = new System.Windows.Forms.Padding(4);
		this.comboConnectionType.Name = "comboConnectionType";
		this.comboConnectionType.Size = new System.Drawing.Size(300, 24);
		this.comboConnectionType.TabIndex = 1;
		this.lblConnectionType.AutoSize = true;
		this.lblConnectionType.Location = new System.Drawing.Point(9, 38);
		this.lblConnectionType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblConnectionType.Name = "lblConnectionType";
		this.lblConnectionType.Size = new System.Drawing.Size(115, 17);
		this.lblConnectionType.TabIndex = 0;
		this.lblConnectionType.Text = "Connection Type";
		this.lblHost.AutoSize = true;
		this.lblHost.Location = new System.Drawing.Point(9, 87);
		this.lblHost.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblHost.Name = "lblHost";
		this.lblHost.Size = new System.Drawing.Size(83, 17);
		this.lblHost.TabIndex = 2;
		this.lblHost.Text = "Server Host";
		this.txtHost.Location = new System.Drawing.Point(12, 108);
		this.txtHost.Margin = new System.Windows.Forms.Padding(4);
		this.txtHost.MaxLength = 256;
		this.txtHost.Name = "txtHost";
		this.txtHost.Size = new System.Drawing.Size(300, 22);
		this.txtHost.TabIndex = 3;
		this.txtHost.Enter += new System.EventHandler(TxtBox_Enter);
		this.lblPort.AutoSize = true;
		this.lblPort.Location = new System.Drawing.Point(9, 134);
		this.lblPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblPort.Name = "lblPort";
		this.lblPort.Size = new System.Drawing.Size(34, 17);
		this.lblPort.TabIndex = 4;
		this.lblPort.Text = "Port";
		this.txtPort.HidePromptOnLeave = true;
		this.txtPort.Location = new System.Drawing.Point(12, 155);
		this.txtPort.Margin = new System.Windows.Forms.Padding(4);
		this.txtPort.Mask = "99999";
		this.txtPort.Name = "txtPort";
		this.txtPort.Size = new System.Drawing.Size(300, 22);
		this.txtPort.TabIndex = 5;
		this.txtPort.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtPort.Validating += new System.ComponentModel.CancelEventHandler(TxtPort_Validating);
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.lblOpenASocket.AutoSize = true;
		this.lblOpenASocket.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblOpenASocket.Location = new System.Drawing.Point(8, 8);
		this.lblOpenASocket.Name = "lblOpenASocket";
		this.lblOpenASocket.Size = new System.Drawing.Size(132, 20);
		this.lblOpenASocket.TabIndex = 7;
		this.lblOpenASocket.Text = "Open a Socket";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.lblOpenASocket);
		base.Controls.Add(this.chkWaitForResponse);
		base.Controls.Add(this.comboConnectionType);
		base.Controls.Add(this.lblConnectionType);
		base.Controls.Add(this.lblHost);
		base.Controls.Add(this.txtHost);
		base.Controls.Add(this.lblPort);
		base.Controls.Add(this.txtPort);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "OptionsComponentsSocketClientControl";
		base.Size = new System.Drawing.Size(780, 665);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
