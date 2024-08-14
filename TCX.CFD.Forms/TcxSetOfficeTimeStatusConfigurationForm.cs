using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;
using TCX.CFD.Properties;

namespace TCX.CFD.Forms;

public class TcxSetOfficeTimeStatusConfigurationForm : Form
{
	private readonly TcxSetOfficeTimeStatusComponent tcxSetOfficeTimeStatusComponent;

	private IContainer components;

	private Button cancelButton;

	private Button okButton;

	private Label lblStatus;

	private ComboBox comboStatus;

	public OfficeTimeStatus Status => ComboIndexToOfficeTimeStatus(comboStatus.SelectedIndex);

	private int OfficeTimeStatusToComboIndex(OfficeTimeStatus status)
	{
		return status switch
		{
			OfficeTimeStatus.ForceInOffice => 0, 
			OfficeTimeStatus.ForceOutOfOffice => 1, 
			_ => 2, 
		};
	}

	private OfficeTimeStatus ComboIndexToOfficeTimeStatus(int index)
	{
		return index switch
		{
			0 => OfficeTimeStatus.ForceInOffice, 
			1 => OfficeTimeStatus.ForceOutOfOffice, 
			_ => OfficeTimeStatus.UseDefault, 
		};
	}

	public TcxSetOfficeTimeStatusConfigurationForm(TcxSetOfficeTimeStatusComponent tcxSetOfficeTimeStatusComponent)
	{
		InitializeComponent();
		this.tcxSetOfficeTimeStatusComponent = tcxSetOfficeTimeStatusComponent;
		comboStatus.SelectedIndex = OfficeTimeStatusToComboIndex(tcxSetOfficeTimeStatusComponent.Status);
		Text = LocalizedResourceMgr.GetString("TcxSetOfficeTimeStatusConfigurationForm.Title");
		lblStatus.Text = LocalizedResourceMgr.GetString("TcxSetOfficeTimeStatusConfigurationForm.lblStatus.Text");
		okButton.Text = LocalizedResourceMgr.GetString("TcxSetOfficeTimeStatusConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("TcxSetOfficeTimeStatusConfigurationForm.cancelButton.Text");
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void TcxSetOfficeTimeStatusConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		tcxSetOfficeTimeStatusComponent.ShowHelp();
	}

	private void TcxSetOfficeTimeStatusConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		tcxSetOfficeTimeStatusComponent.ShowHelp();
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
		this.cancelButton = new System.Windows.Forms.Button();
		this.okButton = new System.Windows.Forms.Button();
		this.lblStatus = new System.Windows.Forms.Label();
		this.comboStatus = new System.Windows.Forms.ComboBox();
		base.SuspendLayout();
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(506, 50);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 3;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(398, 50);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 2;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.lblStatus.AutoSize = true;
		this.lblStatus.Location = new System.Drawing.Point(16, 10);
		this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblStatus.Name = "lblStatus";
		this.lblStatus.Size = new System.Drawing.Size(48, 17);
		this.lblStatus.TabIndex = 0;
		this.lblStatus.Text = "Status";
		this.comboStatus.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboStatus.FormattingEnabled = true;
		this.comboStatus.Items.AddRange(new object[3] { "Force In Office", "Force Out of Office", "Use Default" });
		this.comboStatus.Location = new System.Drawing.Point(127, 7);
		this.comboStatus.Margin = new System.Windows.Forms.Padding(4);
		this.comboStatus.Name = "comboStatus";
		this.comboStatus.Size = new System.Drawing.Size(479, 24);
		this.comboStatus.TabIndex = 1;
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(619, 91);
		base.Controls.Add(this.comboStatus);
		base.Controls.Add(this.lblStatus);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(1359, 138);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(634, 138);
		base.Name = "TcxSetOfficeTimeStatusConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Set Office Time Status";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(TcxSetOfficeTimeStatusConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(TcxSetOfficeTimeStatusConfigurationForm_HelpRequested);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
