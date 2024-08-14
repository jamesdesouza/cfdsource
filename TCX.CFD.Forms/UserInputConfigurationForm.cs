using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;
using TCX.CFD.Controls;
using TCX.CFD.Properties;

namespace TCX.CFD.Forms;

public class UserInputConfigurationForm : Form
{
	private UserInputComponent userInputComponent;

	private IContainer components;

	private UserInputConfigurationControl userInputConfigurationControl;

	private Button cancelButton;

	private Button okButton;

	public bool AcceptDtmfInput => userInputConfigurationControl.AcceptDtmfInput;

	public List<Prompt> InitialPrompts => userInputConfigurationControl.InitialPrompts;

	public List<Prompt> SubsequentPrompts => userInputConfigurationControl.SubsequentPrompts;

	public List<Prompt> TimeoutPrompts => userInputConfigurationControl.TimeoutPrompts;

	public List<Prompt> InvalidDigitPrompts => userInputConfigurationControl.InvalidDigitPrompts;

	public uint MaxRetryCount => userInputConfigurationControl.MaxRetryCount;

	public uint FirstDigitTimeout => userInputConfigurationControl.FirstDigitTimeout;

	public uint InterDigitTimeout => userInputConfigurationControl.InterDigitTimeout;

	public uint FinalDigitTimeout => userInputConfigurationControl.FinalDigitTimeout;

	public uint MinDigits => userInputConfigurationControl.MinDigits;

	public uint MaxDigits => userInputConfigurationControl.MaxDigits;

	public DtmfDigits StopDigit => userInputConfigurationControl.StopDigit;

	public bool IsValidDigit_0 => userInputConfigurationControl.IsValidDigit_0;

	public bool IsValidDigit_1 => userInputConfigurationControl.IsValidDigit_1;

	public bool IsValidDigit_2 => userInputConfigurationControl.IsValidDigit_2;

	public bool IsValidDigit_3 => userInputConfigurationControl.IsValidDigit_3;

	public bool IsValidDigit_4 => userInputConfigurationControl.IsValidDigit_4;

	public bool IsValidDigit_5 => userInputConfigurationControl.IsValidDigit_5;

	public bool IsValidDigit_6 => userInputConfigurationControl.IsValidDigit_6;

	public bool IsValidDigit_7 => userInputConfigurationControl.IsValidDigit_7;

	public bool IsValidDigit_8 => userInputConfigurationControl.IsValidDigit_8;

	public bool IsValidDigit_9 => userInputConfigurationControl.IsValidDigit_9;

	public bool IsValidDigit_Star => userInputConfigurationControl.IsValidDigit_Star;

	public bool IsValidDigit_Pound => userInputConfigurationControl.IsValidDigit_Pound;

	private void OkButton_Click(object sender, EventArgs e)
	{
		if (userInputConfigurationControl.ValidateFieldsOnSave())
		{
			base.DialogResult = DialogResult.OK;
			Close();
		}
	}

	private void CancelButton_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.Cancel;
		Close();
	}

	public UserInputConfigurationForm(UserInputComponent userInputComponent)
	{
		InitializeComponent();
		this.userInputComponent = userInputComponent;
		userInputConfigurationControl.SetUserInputComponent(userInputComponent, userInputComponent, 0);
		Text = LocalizedResourceMgr.GetString("UserInputConfigurationForm.Title");
		okButton.Text = LocalizedResourceMgr.GetString("UserInputConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("UserInputConfigurationForm.cancelButton.Text");
	}

	private void UserInputConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		userInputComponent.ShowHelp();
	}

	private void UserInputConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		userInputComponent.ShowHelp();
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
		this.userInputConfigurationControl = new TCX.CFD.Controls.UserInputConfigurationControl();
		base.SuspendLayout();
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(447, 345);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 2;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.cancelButton.Click += new System.EventHandler(CancelButton_Click);
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(339, 345);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 1;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.userInputConfigurationControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.userInputConfigurationControl.Location = new System.Drawing.Point(0, 0);
		this.userInputConfigurationControl.Margin = new System.Windows.Forms.Padding(5);
		this.userInputConfigurationControl.MinimumSize = new System.Drawing.Size(549, 346);
		this.userInputConfigurationControl.Name = "userInputConfigurationControl";
		this.userInputConfigurationControl.Size = new System.Drawing.Size(549, 346);
		this.userInputConfigurationControl.TabIndex = 0;
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(560, 386);
		base.Controls.Add(this.okButton);
		base.Controls.Add(this.userInputConfigurationControl);
		base.Controls.Add(this.cancelButton);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(578, 433);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(578, 433);
		base.Name = "UserInputConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "User Input";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(UserInputConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(UserInputConfigurationForm_HelpRequested);
		base.ResumeLayout(false);
	}
}
