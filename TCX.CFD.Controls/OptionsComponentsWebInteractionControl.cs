using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class OptionsComponentsWebInteractionControl : UserControl, IOptionsControl
{
	private IContainer components;

	private Label lblTimeout;

	private MaskedTextBox txtTimeout;

	private ComboBox comboHttpRequestType;

	private Label lblHttpRequestType;

	private ErrorProvider errorProvider;

	private Label lblHttpRequest;

	public OptionsComponentsWebInteractionControl()
	{
		InitializeComponent();
		comboHttpRequestType.Items.AddRange(new object[7]
		{
			HttpRequestTypes.DELETE,
			HttpRequestTypes.GET,
			HttpRequestTypes.HEAD,
			HttpRequestTypes.OPTIONS,
			HttpRequestTypes.POST,
			HttpRequestTypes.PUT,
			HttpRequestTypes.TRACE
		});
		comboHttpRequestType.SelectedItem = EnumHelper.StringToHttpRequestType(Settings.Default.WebInteractionTemplateHttpRequestType);
		txtTimeout.Text = Settings.Default.WebInteractionTemplateTimeout.ToString();
		lblHttpRequest.Text = LocalizedResourceMgr.GetString("OptionsComponentsWebInteractionControl.lblHttpRequest.Text");
		lblHttpRequestType.Text = LocalizedResourceMgr.GetString("OptionsComponentsWebInteractionControl.lblHttpRequestType.Text");
		lblTimeout.Text = LocalizedResourceMgr.GetString("OptionsComponentsWebInteractionControl.lblTimeout.Text");
	}

	private void MaskedTextBox_GotFocus(object sender, EventArgs e)
	{
		(sender as MaskedTextBox).SelectAll();
	}

	private void ValidateTimeout()
	{
		if (string.IsNullOrEmpty(txtTimeout.Text))
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsWebInteractionControl.Error.TimeoutIsMandatory"));
		}
	}

	private void ValidateFields()
	{
		ValidateTimeout();
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
		Settings.Default.WebInteractionTemplateHttpRequestType = EnumHelper.HttpRequestTypeToString((HttpRequestTypes)comboHttpRequestType.SelectedItem);
		Settings.Default.WebInteractionTemplateTimeout = Convert.ToUInt32(txtTimeout.Text);
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
		this.lblTimeout = new System.Windows.Forms.Label();
		this.txtTimeout = new System.Windows.Forms.MaskedTextBox();
		this.comboHttpRequestType = new System.Windows.Forms.ComboBox();
		this.lblHttpRequestType = new System.Windows.Forms.Label();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.lblHttpRequest = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.lblTimeout.AutoSize = true;
		this.lblTimeout.Location = new System.Drawing.Point(9, 92);
		this.lblTimeout.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblTimeout.Name = "lblTimeout";
		this.lblTimeout.Size = new System.Drawing.Size(102, 17);
		this.lblTimeout.TabIndex = 3;
		this.lblTimeout.Text = "Timeout (secs)";
		this.txtTimeout.HidePromptOnLeave = true;
		this.txtTimeout.Location = new System.Drawing.Point(12, 113);
		this.txtTimeout.Margin = new System.Windows.Forms.Padding(4);
		this.txtTimeout.Mask = "999";
		this.txtTimeout.Name = "txtTimeout";
		this.txtTimeout.Size = new System.Drawing.Size(300, 22);
		this.txtTimeout.TabIndex = 4;
		this.txtTimeout.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtTimeout.Validating += new System.ComponentModel.CancelEventHandler(TxtTimeout_Validating);
		this.comboHttpRequestType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboHttpRequestType.FormattingEnabled = true;
		this.comboHttpRequestType.Location = new System.Drawing.Point(12, 59);
		this.comboHttpRequestType.Margin = new System.Windows.Forms.Padding(4);
		this.comboHttpRequestType.Name = "comboHttpRequestType";
		this.comboHttpRequestType.Size = new System.Drawing.Size(300, 24);
		this.comboHttpRequestType.TabIndex = 2;
		this.lblHttpRequestType.AutoSize = true;
		this.lblHttpRequestType.Location = new System.Drawing.Point(9, 38);
		this.lblHttpRequestType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblHttpRequestType.Name = "lblHttpRequestType";
		this.lblHttpRequestType.Size = new System.Drawing.Size(138, 17);
		this.lblHttpRequestType.TabIndex = 1;
		this.lblHttpRequestType.Text = "HTTP Request Type";
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.lblHttpRequest.AutoSize = true;
		this.lblHttpRequest.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblHttpRequest.Location = new System.Drawing.Point(8, 8);
		this.lblHttpRequest.Name = "lblHttpRequest";
		this.lblHttpRequest.Size = new System.Drawing.Size(132, 20);
		this.lblHttpRequest.TabIndex = 0;
		this.lblHttpRequest.Text = "HTTP Request";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.lblHttpRequest);
		base.Controls.Add(this.lblTimeout);
		base.Controls.Add(this.txtTimeout);
		base.Controls.Add(this.comboHttpRequestType);
		base.Controls.Add(this.lblHttpRequestType);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "OptionsComponentsWebInteractionControl";
		base.Size = new System.Drawing.Size(780, 665);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
