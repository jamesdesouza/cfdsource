using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class OptionsComponentsMenuControl : UserControl, IOptionsControl
{
	private IContainer components;

	private GroupBox grpBoxValidOptions;

	private Label lblMaxRetryCount;

	private MaskedTextBox txtMaxRetryCount;

	private Label lblTimeout;

	private MaskedTextBox txtTimeout;

	private CheckBox chkAcceptDtmfInput;

	private CheckedListBox validOptionsListBox;

	private ErrorProvider errorProvider;

	private Label lblMenu;

	private bool IsMenuOptionValid(int index)
	{
		return validOptionsListBox.GetItemChecked(index);
	}

	private void ValidOptionsListBox_LostFocus(object sender, EventArgs e)
	{
		validOptionsListBox.SelectedIndex = -1;
	}

	private void MaskedTextBox_GotFocus(object sender, EventArgs e)
	{
		(sender as MaskedTextBox).SelectAll();
	}

	private void ValidateTimeout()
	{
		if (string.IsNullOrEmpty(txtTimeout.Text))
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsMenuControl.Error.TimeoutIsMandatory"));
		}
		if (Convert.ToUInt32(txtTimeout.Text) == 0)
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsMenuControl.Error.TimeoutInvalidRange"));
		}
	}

	private void ValidateMaxRetryCount()
	{
		if (string.IsNullOrEmpty(txtMaxRetryCount.Text))
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsMenuControl.Error.MaxRetryCountIsMandatory"));
		}
		if (Convert.ToUInt32(txtMaxRetryCount.Text) == 0)
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsMenuControl.Error.MaxRetryCountInvalidRange"));
		}
	}

	private void ValidateFields()
	{
		ValidateTimeout();
		ValidateMaxRetryCount();
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

	private void TxtMaxRetryCount_Validating(object sender, CancelEventArgs e)
	{
		try
		{
			ValidateMaxRetryCount();
			errorProvider.SetError(txtMaxRetryCount, string.Empty);
		}
		catch (Exception exc)
		{
			errorProvider.SetError(txtMaxRetryCount, ErrorHelper.GetErrorDescription(exc));
		}
	}

	public OptionsComponentsMenuControl()
	{
		InitializeComponent();
		chkAcceptDtmfInput.Checked = Settings.Default.MenuTemplateAcceptDtmfInput;
		txtTimeout.Text = Settings.Default.MenuTemplateTimeout.ToString();
		txtMaxRetryCount.Text = Settings.Default.MenuTemplateMaxRetryCount.ToString();
		validOptionsListBox.Items.Add(LocalizedResourceMgr.GetString("OptionsComponentsMenuControl.validOptionsListBox.OptionText") + " 0", Settings.Default.MenuTemplateIsValidOption0);
		validOptionsListBox.Items.Add(LocalizedResourceMgr.GetString("OptionsComponentsMenuControl.validOptionsListBox.OptionText") + " 1", Settings.Default.MenuTemplateIsValidOption1);
		validOptionsListBox.Items.Add(LocalizedResourceMgr.GetString("OptionsComponentsMenuControl.validOptionsListBox.OptionText") + " 2", Settings.Default.MenuTemplateIsValidOption2);
		validOptionsListBox.Items.Add(LocalizedResourceMgr.GetString("OptionsComponentsMenuControl.validOptionsListBox.OptionText") + " 3", Settings.Default.MenuTemplateIsValidOption3);
		validOptionsListBox.Items.Add(LocalizedResourceMgr.GetString("OptionsComponentsMenuControl.validOptionsListBox.OptionText") + " 4", Settings.Default.MenuTemplateIsValidOption4);
		validOptionsListBox.Items.Add(LocalizedResourceMgr.GetString("OptionsComponentsMenuControl.validOptionsListBox.OptionText") + " 5", Settings.Default.MenuTemplateIsValidOption5);
		validOptionsListBox.Items.Add(LocalizedResourceMgr.GetString("OptionsComponentsMenuControl.validOptionsListBox.OptionText") + " 6", Settings.Default.MenuTemplateIsValidOption6);
		validOptionsListBox.Items.Add(LocalizedResourceMgr.GetString("OptionsComponentsMenuControl.validOptionsListBox.OptionText") + " 7", Settings.Default.MenuTemplateIsValidOption7);
		validOptionsListBox.Items.Add(LocalizedResourceMgr.GetString("OptionsComponentsMenuControl.validOptionsListBox.OptionText") + " 8", Settings.Default.MenuTemplateIsValidOption8);
		validOptionsListBox.Items.Add(LocalizedResourceMgr.GetString("OptionsComponentsMenuControl.validOptionsListBox.OptionText") + " 9", Settings.Default.MenuTemplateIsValidOption9);
		validOptionsListBox.Items.Add(LocalizedResourceMgr.GetString("OptionsComponentsMenuControl.validOptionsListBox.OptionText") + " *", Settings.Default.MenuTemplateIsValidOptionStar);
		validOptionsListBox.Items.Add(LocalizedResourceMgr.GetString("OptionsComponentsMenuControl.validOptionsListBox.OptionText") + " #", Settings.Default.MenuTemplateIsValidOptionPound);
		lblMenu.Text = LocalizedResourceMgr.GetString("OptionsComponentsMenuControl.lblMenu.Text");
		chkAcceptDtmfInput.Text = LocalizedResourceMgr.GetString("OptionsComponentsMenuControl.chkAcceptDtmfInput.Text");
		lblTimeout.Text = LocalizedResourceMgr.GetString("OptionsComponentsMenuControl.lblTimeout.Text");
		lblMaxRetryCount.Text = LocalizedResourceMgr.GetString("OptionsComponentsMenuControl.lblMaxRetryCount.Text");
		grpBoxValidOptions.Text = LocalizedResourceMgr.GetString("OptionsComponentsMenuControl.grpBoxValidOptions.Text");
	}

	public void Save()
	{
		ValidateFields();
		Settings.Default.MenuTemplateAcceptDtmfInput = chkAcceptDtmfInput.Checked;
		Settings.Default.MenuTemplateTimeout = Convert.ToUInt32(txtTimeout.Text);
		Settings.Default.MenuTemplateMaxRetryCount = Convert.ToUInt32(txtMaxRetryCount.Text);
		Settings.Default.MenuTemplateIsValidOption0 = IsMenuOptionValid(0);
		Settings.Default.MenuTemplateIsValidOption1 = IsMenuOptionValid(1);
		Settings.Default.MenuTemplateIsValidOption2 = IsMenuOptionValid(2);
		Settings.Default.MenuTemplateIsValidOption3 = IsMenuOptionValid(3);
		Settings.Default.MenuTemplateIsValidOption4 = IsMenuOptionValid(4);
		Settings.Default.MenuTemplateIsValidOption5 = IsMenuOptionValid(5);
		Settings.Default.MenuTemplateIsValidOption6 = IsMenuOptionValid(6);
		Settings.Default.MenuTemplateIsValidOption7 = IsMenuOptionValid(7);
		Settings.Default.MenuTemplateIsValidOption8 = IsMenuOptionValid(8);
		Settings.Default.MenuTemplateIsValidOption9 = IsMenuOptionValid(9);
		Settings.Default.MenuTemplateIsValidOptionStar = IsMenuOptionValid(10);
		Settings.Default.MenuTemplateIsValidOptionPound = IsMenuOptionValid(11);
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
		this.grpBoxValidOptions = new System.Windows.Forms.GroupBox();
		this.validOptionsListBox = new System.Windows.Forms.CheckedListBox();
		this.lblMaxRetryCount = new System.Windows.Forms.Label();
		this.txtMaxRetryCount = new System.Windows.Forms.MaskedTextBox();
		this.lblTimeout = new System.Windows.Forms.Label();
		this.txtTimeout = new System.Windows.Forms.MaskedTextBox();
		this.chkAcceptDtmfInput = new System.Windows.Forms.CheckBox();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.lblMenu = new System.Windows.Forms.Label();
		this.grpBoxValidOptions.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.grpBoxValidOptions.Controls.Add(this.validOptionsListBox);
		this.grpBoxValidOptions.Location = new System.Drawing.Point(8, 161);
		this.grpBoxValidOptions.Margin = new System.Windows.Forms.Padding(4);
		this.grpBoxValidOptions.Name = "grpBoxValidOptions";
		this.grpBoxValidOptions.Padding = new System.Windows.Forms.Padding(4);
		this.grpBoxValidOptions.Size = new System.Drawing.Size(768, 500);
		this.grpBoxValidOptions.TabIndex = 6;
		this.grpBoxValidOptions.TabStop = false;
		this.grpBoxValidOptions.Text = "Valid Options";
		this.validOptionsListBox.CheckOnClick = true;
		this.validOptionsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
		this.validOptionsListBox.FormattingEnabled = true;
		this.validOptionsListBox.Location = new System.Drawing.Point(4, 19);
		this.validOptionsListBox.Margin = new System.Windows.Forms.Padding(4);
		this.validOptionsListBox.MultiColumn = true;
		this.validOptionsListBox.Name = "validOptionsListBox";
		this.validOptionsListBox.Size = new System.Drawing.Size(760, 477);
		this.validOptionsListBox.TabIndex = 0;
		this.validOptionsListBox.LostFocus += new System.EventHandler(ValidOptionsListBox_LostFocus);
		this.lblMaxRetryCount.AutoSize = true;
		this.lblMaxRetryCount.Location = new System.Drawing.Point(9, 110);
		this.lblMaxRetryCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblMaxRetryCount.Name = "lblMaxRetryCount";
		this.lblMaxRetryCount.Size = new System.Drawing.Size(112, 17);
		this.lblMaxRetryCount.TabIndex = 4;
		this.lblMaxRetryCount.Text = "Max Retry Count";
		this.txtMaxRetryCount.HidePromptOnLeave = true;
		this.txtMaxRetryCount.Location = new System.Drawing.Point(12, 131);
		this.txtMaxRetryCount.Margin = new System.Windows.Forms.Padding(4);
		this.txtMaxRetryCount.Mask = "99";
		this.txtMaxRetryCount.Name = "txtMaxRetryCount";
		this.txtMaxRetryCount.Size = new System.Drawing.Size(300, 22);
		this.txtMaxRetryCount.TabIndex = 5;
		this.txtMaxRetryCount.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtMaxRetryCount.Validating += new System.ComponentModel.CancelEventHandler(TxtMaxRetryCount_Validating);
		this.lblTimeout.AutoSize = true;
		this.lblTimeout.Location = new System.Drawing.Point(9, 63);
		this.lblTimeout.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblTimeout.Name = "lblTimeout";
		this.lblTimeout.Size = new System.Drawing.Size(102, 17);
		this.lblTimeout.TabIndex = 2;
		this.lblTimeout.Text = "Timeout (secs)";
		this.txtTimeout.HidePromptOnLeave = true;
		this.txtTimeout.Location = new System.Drawing.Point(12, 84);
		this.txtTimeout.Margin = new System.Windows.Forms.Padding(4);
		this.txtTimeout.Mask = "99";
		this.txtTimeout.Name = "txtTimeout";
		this.txtTimeout.Size = new System.Drawing.Size(300, 22);
		this.txtTimeout.TabIndex = 3;
		this.txtTimeout.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtTimeout.Validating += new System.ComponentModel.CancelEventHandler(TxtTimeout_Validating);
		this.chkAcceptDtmfInput.AutoSize = true;
		this.chkAcceptDtmfInput.Location = new System.Drawing.Point(12, 38);
		this.chkAcceptDtmfInput.Margin = new System.Windows.Forms.Padding(4);
		this.chkAcceptDtmfInput.Name = "chkAcceptDtmfInput";
		this.chkAcceptDtmfInput.Size = new System.Drawing.Size(139, 21);
		this.chkAcceptDtmfInput.TabIndex = 1;
		this.chkAcceptDtmfInput.Text = "Accept DTMF Input During Prompt";
		this.chkAcceptDtmfInput.UseVisualStyleBackColor = true;
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.lblMenu.AutoSize = true;
		this.lblMenu.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblMenu.Location = new System.Drawing.Point(8, 8);
		this.lblMenu.Name = "lblMenu";
		this.lblMenu.Size = new System.Drawing.Size(54, 20);
		this.lblMenu.TabIndex = 0;
		this.lblMenu.Text = "Menu";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.lblMenu);
		base.Controls.Add(this.grpBoxValidOptions);
		base.Controls.Add(this.lblMaxRetryCount);
		base.Controls.Add(this.txtMaxRetryCount);
		base.Controls.Add(this.lblTimeout);
		base.Controls.Add(this.txtTimeout);
		base.Controls.Add(this.chkAcceptDtmfInput);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "OptionsComponentsMenuControl";
		base.Size = new System.Drawing.Size(780, 665);
		this.grpBoxValidOptions.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
