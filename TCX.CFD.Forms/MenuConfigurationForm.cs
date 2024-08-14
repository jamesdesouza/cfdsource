using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;
using TCX.CFD.Properties;

namespace TCX.CFD.Forms;

public class MenuConfigurationForm : Form
{
	private readonly MenuComponent menuComponent;

	private IContainer components;

	private Button cancelButton;

	private Button okButton;

	private CheckBox chkAcceptDtmfInput;

	private Label lblMaxRetryCount;

	private MaskedTextBox txtMaxRetryCount;

	private Label lblTimeout;

	private MaskedTextBox txtTimeout;

	private GroupBox grpBoxValidOptions;

	private CheckedListBox validOptionsListBox;

	private ErrorProvider errorProvider;

	private GroupBox grpBoxPrompts;

	private Button invalidDigitPromptsButton;

	private Button timeoutPromptsButton;

	private Button subsequentPromptsButton;

	private Button initialPromptsButton;

	private ComboBox comboRepeatOption;

	private Label lblRepeatMenuOption;

	public bool AcceptDtmfInput => chkAcceptDtmfInput.Checked;

	public List<Prompt> InitialPrompts { get; private set; }

	public List<Prompt> SubsequentPrompts { get; private set; }

	public List<Prompt> TimeoutPrompts { get; private set; }

	public List<Prompt> InvalidDigitPrompts { get; private set; }

	public uint Timeout
	{
		get
		{
			if (!string.IsNullOrEmpty(txtTimeout.Text))
			{
				return Convert.ToUInt32(txtTimeout.Text);
			}
			return 5u;
		}
	}

	public uint MaxRetryCount
	{
		get
		{
			if (!string.IsNullOrEmpty(txtMaxRetryCount.Text))
			{
				return Convert.ToUInt32(txtMaxRetryCount.Text);
			}
			return 3u;
		}
	}

	public DtmfDigits RepeatOption => (DtmfDigits)comboRepeatOption.SelectedItem;

	public bool IsValidOption_0 => IsMenuOptionValid(0);

	public bool IsValidOption_1 => IsMenuOptionValid(1);

	public bool IsValidOption_2 => IsMenuOptionValid(2);

	public bool IsValidOption_3 => IsMenuOptionValid(3);

	public bool IsValidOption_4 => IsMenuOptionValid(4);

	public bool IsValidOption_5 => IsMenuOptionValid(5);

	public bool IsValidOption_6 => IsMenuOptionValid(6);

	public bool IsValidOption_7 => IsMenuOptionValid(7);

	public bool IsValidOption_8 => IsMenuOptionValid(8);

	public bool IsValidOption_9 => IsMenuOptionValid(9);

	public bool IsValidOption_Star => IsMenuOptionValid(10);

	public bool IsValidOption_Pound => IsMenuOptionValid(11);

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

	private void TxtTimeout_Validating(object sender, CancelEventArgs e)
	{
		errorProvider.SetError(txtTimeout, string.IsNullOrEmpty(txtTimeout.Text) ? LocalizedResourceMgr.GetString("MenuConfigurationForm.Error.TimeoutIsMandatory") : ((Convert.ToUInt32(txtTimeout.Text) == 0) ? LocalizedResourceMgr.GetString("MenuConfigurationForm.Error.TimeoutInvalidRange") : string.Empty));
	}

	private void TxtMaxRetryCount_Validating(object sender, CancelEventArgs e)
	{
		errorProvider.SetError(txtMaxRetryCount, string.IsNullOrEmpty(txtMaxRetryCount.Text) ? LocalizedResourceMgr.GetString("MenuConfigurationForm.Error.MaxRetryCountIsMandatory") : ((Convert.ToUInt32(txtMaxRetryCount.Text) == 0) ? LocalizedResourceMgr.GetString("MenuConfigurationForm.Error.MaxRetryCountInvalidRange") : string.Empty));
	}

	private void InitialPromptsButton_Click(object sender, EventArgs e)
	{
		PromptCollectionEditorForm promptCollectionEditorForm = new PromptCollectionEditorForm(menuComponent);
		promptCollectionEditorForm.PromptList = InitialPrompts;
		if (promptCollectionEditorForm.ShowDialog() == DialogResult.OK)
		{
			InitialPrompts = promptCollectionEditorForm.PromptList;
		}
	}

	private void SubsequentPromptsButton_Click(object sender, EventArgs e)
	{
		PromptCollectionEditorForm promptCollectionEditorForm = new PromptCollectionEditorForm(menuComponent);
		promptCollectionEditorForm.PromptList = SubsequentPrompts;
		if (promptCollectionEditorForm.ShowDialog() == DialogResult.OK)
		{
			SubsequentPrompts = promptCollectionEditorForm.PromptList;
		}
	}

	private void TimeoutPromptsButton_Click(object sender, EventArgs e)
	{
		PromptCollectionEditorForm promptCollectionEditorForm = new PromptCollectionEditorForm(menuComponent);
		promptCollectionEditorForm.PromptList = TimeoutPrompts;
		if (promptCollectionEditorForm.ShowDialog() == DialogResult.OK)
		{
			TimeoutPrompts = promptCollectionEditorForm.PromptList;
		}
	}

	private void InvalidDigitPromptsButton_Click(object sender, EventArgs e)
	{
		PromptCollectionEditorForm promptCollectionEditorForm = new PromptCollectionEditorForm(menuComponent);
		promptCollectionEditorForm.PromptList = InvalidDigitPrompts;
		if (promptCollectionEditorForm.ShowDialog() == DialogResult.OK)
		{
			InvalidDigitPrompts = promptCollectionEditorForm.PromptList;
		}
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(txtTimeout.Text))
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("MenuConfigurationForm.Error.TimeoutIsMandatory"), LocalizedResourceMgr.GetString("MenuConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			txtTimeout.Focus();
		}
		else if (Convert.ToUInt32(txtTimeout.Text) == 0)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("MenuConfigurationForm.Error.TimeoutInvalidRange"), LocalizedResourceMgr.GetString("MenuConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			txtTimeout.Focus();
		}
		else if (string.IsNullOrEmpty(txtMaxRetryCount.Text))
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("MenuConfigurationForm.Error.MaxRetryCountIsMandatory"), LocalizedResourceMgr.GetString("MenuConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			txtMaxRetryCount.Focus();
		}
		else if (Convert.ToUInt32(txtMaxRetryCount.Text) == 0)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("MenuConfigurationForm.Error.MaxRetryCountInvalidRange"), LocalizedResourceMgr.GetString("MenuConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			txtMaxRetryCount.Focus();
		}
		else
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

	public MenuConfigurationForm(MenuComponent menuComponent)
	{
		InitializeComponent();
		this.menuComponent = menuComponent;
		InitialPrompts = new List<Prompt>(menuComponent.InitialPrompts);
		SubsequentPrompts = new List<Prompt>(menuComponent.SubsequentPrompts);
		TimeoutPrompts = new List<Prompt>(menuComponent.TimeoutPrompts);
		InvalidDigitPrompts = new List<Prompt>(menuComponent.InvalidDigitPrompts);
		comboRepeatOption.Items.AddRange(new object[13]
		{
			DtmfDigits.Digit0,
			DtmfDigits.Digit1,
			DtmfDigits.Digit2,
			DtmfDigits.Digit3,
			DtmfDigits.Digit4,
			DtmfDigits.Digit5,
			DtmfDigits.Digit6,
			DtmfDigits.Digit7,
			DtmfDigits.Digit8,
			DtmfDigits.Digit9,
			DtmfDigits.DigitStar,
			DtmfDigits.DigitPound,
			DtmfDigits.None
		});
		chkAcceptDtmfInput.Checked = menuComponent.AcceptDtmfInput;
		txtTimeout.Text = menuComponent.Timeout.ToString();
		txtMaxRetryCount.Text = menuComponent.MaxRetryCount.ToString();
		comboRepeatOption.SelectedItem = menuComponent.RepeatOption;
		validOptionsListBox.Items.Add(LocalizedResourceMgr.GetString("MenuConfigurationForm.validOptionsListBox.OptionText") + " 0", menuComponent.IsValidOption_0);
		validOptionsListBox.Items.Add(LocalizedResourceMgr.GetString("MenuConfigurationForm.validOptionsListBox.OptionText") + " 1", menuComponent.IsValidOption_1);
		validOptionsListBox.Items.Add(LocalizedResourceMgr.GetString("MenuConfigurationForm.validOptionsListBox.OptionText") + " 2", menuComponent.IsValidOption_2);
		validOptionsListBox.Items.Add(LocalizedResourceMgr.GetString("MenuConfigurationForm.validOptionsListBox.OptionText") + " 3", menuComponent.IsValidOption_3);
		validOptionsListBox.Items.Add(LocalizedResourceMgr.GetString("MenuConfigurationForm.validOptionsListBox.OptionText") + " 4", menuComponent.IsValidOption_4);
		validOptionsListBox.Items.Add(LocalizedResourceMgr.GetString("MenuConfigurationForm.validOptionsListBox.OptionText") + " 5", menuComponent.IsValidOption_5);
		validOptionsListBox.Items.Add(LocalizedResourceMgr.GetString("MenuConfigurationForm.validOptionsListBox.OptionText") + " 6", menuComponent.IsValidOption_6);
		validOptionsListBox.Items.Add(LocalizedResourceMgr.GetString("MenuConfigurationForm.validOptionsListBox.OptionText") + " 7", menuComponent.IsValidOption_7);
		validOptionsListBox.Items.Add(LocalizedResourceMgr.GetString("MenuConfigurationForm.validOptionsListBox.OptionText") + " 8", menuComponent.IsValidOption_8);
		validOptionsListBox.Items.Add(LocalizedResourceMgr.GetString("MenuConfigurationForm.validOptionsListBox.OptionText") + " 9", menuComponent.IsValidOption_9);
		validOptionsListBox.Items.Add(LocalizedResourceMgr.GetString("MenuConfigurationForm.validOptionsListBox.OptionText") + " *", menuComponent.IsValidOption_Star);
		validOptionsListBox.Items.Add(LocalizedResourceMgr.GetString("MenuConfigurationForm.validOptionsListBox.OptionText") + " #", menuComponent.IsValidOption_Pound);
		Text = LocalizedResourceMgr.GetString("MenuConfigurationForm.Title");
		chkAcceptDtmfInput.Text = LocalizedResourceMgr.GetString("MenuConfigurationForm.chkAcceptDtmfInput.Text");
		lblTimeout.Text = LocalizedResourceMgr.GetString("MenuConfigurationForm.lblTimeout.Text");
		lblMaxRetryCount.Text = LocalizedResourceMgr.GetString("MenuConfigurationForm.lblMaxRetryCount.Text");
		lblRepeatMenuOption.Text = LocalizedResourceMgr.GetString("MenuConfigurationForm.lblRepeatMenuOption.Text");
		grpBoxValidOptions.Text = LocalizedResourceMgr.GetString("MenuConfigurationForm.grpBoxValidOptions.Text");
		grpBoxPrompts.Text = LocalizedResourceMgr.GetString("MenuConfigurationForm.grpBoxPrompts.Text");
		initialPromptsButton.Text = LocalizedResourceMgr.GetString("MenuConfigurationForm.initialPromptsButton.Text");
		subsequentPromptsButton.Text = LocalizedResourceMgr.GetString("MenuConfigurationForm.subsequentPromptsButton.Text");
		timeoutPromptsButton.Text = LocalizedResourceMgr.GetString("MenuConfigurationForm.timeoutPromptsButton.Text");
		invalidDigitPromptsButton.Text = LocalizedResourceMgr.GetString("MenuConfigurationForm.invalidDigitPromptsButton.Text");
		okButton.Text = LocalizedResourceMgr.GetString("MenuConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("MenuConfigurationForm.cancelButton.Text");
	}

	private void MenuConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		menuComponent.ShowHelp();
	}

	private void MenuConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		menuComponent.ShowHelp();
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
		this.chkAcceptDtmfInput = new System.Windows.Forms.CheckBox();
		this.lblMaxRetryCount = new System.Windows.Forms.Label();
		this.txtMaxRetryCount = new System.Windows.Forms.MaskedTextBox();
		this.lblTimeout = new System.Windows.Forms.Label();
		this.txtTimeout = new System.Windows.Forms.MaskedTextBox();
		this.grpBoxValidOptions = new System.Windows.Forms.GroupBox();
		this.validOptionsListBox = new System.Windows.Forms.CheckedListBox();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.grpBoxPrompts = new System.Windows.Forms.GroupBox();
		this.invalidDigitPromptsButton = new System.Windows.Forms.Button();
		this.timeoutPromptsButton = new System.Windows.Forms.Button();
		this.subsequentPromptsButton = new System.Windows.Forms.Button();
		this.initialPromptsButton = new System.Windows.Forms.Button();
		this.comboRepeatOption = new System.Windows.Forms.ComboBox();
		this.lblRepeatMenuOption = new System.Windows.Forms.Label();
		this.grpBoxValidOptions.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		this.grpBoxPrompts.SuspendLayout();
		base.SuspendLayout();
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(503, 441);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 10;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.cancelButton.Click += new System.EventHandler(CancelButton_Click);
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(395, 441);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 9;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.chkAcceptDtmfInput.AutoSize = true;
		this.chkAcceptDtmfInput.Location = new System.Drawing.Point(16, 15);
		this.chkAcceptDtmfInput.Margin = new System.Windows.Forms.Padding(4);
		this.chkAcceptDtmfInput.Name = "chkAcceptDtmfInput";
		this.chkAcceptDtmfInput.Size = new System.Drawing.Size(245, 21);
		this.chkAcceptDtmfInput.TabIndex = 0;
		this.chkAcceptDtmfInput.Text = "Accept DTMF Input During Prompt";
		this.chkAcceptDtmfInput.UseVisualStyleBackColor = true;
		this.lblMaxRetryCount.AutoSize = true;
		this.lblMaxRetryCount.Location = new System.Drawing.Point(12, 82);
		this.lblMaxRetryCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblMaxRetryCount.Name = "lblMaxRetryCount";
		this.lblMaxRetryCount.Size = new System.Drawing.Size(82, 17);
		this.lblMaxRetryCount.TabIndex = 3;
		this.lblMaxRetryCount.Text = "Max Retries";
		this.txtMaxRetryCount.HidePromptOnLeave = true;
		this.txtMaxRetryCount.Location = new System.Drawing.Point(121, 79);
		this.txtMaxRetryCount.Margin = new System.Windows.Forms.Padding(4);
		this.txtMaxRetryCount.Mask = "99";
		this.txtMaxRetryCount.Name = "txtMaxRetryCount";
		this.txtMaxRetryCount.Size = new System.Drawing.Size(139, 22);
		this.txtMaxRetryCount.TabIndex = 4;
		this.txtMaxRetryCount.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtMaxRetryCount.Validating += new System.ComponentModel.CancelEventHandler(TxtMaxRetryCount_Validating);
		this.lblTimeout.AutoSize = true;
		this.lblTimeout.Location = new System.Drawing.Point(12, 50);
		this.lblTimeout.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblTimeout.Name = "lblTimeout";
		this.lblTimeout.Size = new System.Drawing.Size(102, 17);
		this.lblTimeout.TabIndex = 1;
		this.lblTimeout.Text = "Timeout (secs)";
		this.txtTimeout.HidePromptOnLeave = true;
		this.txtTimeout.Location = new System.Drawing.Point(121, 47);
		this.txtTimeout.Margin = new System.Windows.Forms.Padding(4);
		this.txtTimeout.Mask = "99";
		this.txtTimeout.Name = "txtTimeout";
		this.txtTimeout.Size = new System.Drawing.Size(139, 22);
		this.txtTimeout.TabIndex = 2;
		this.txtTimeout.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtTimeout.Validating += new System.ComponentModel.CancelEventHandler(TxtTimeout_Validating);
		this.grpBoxValidOptions.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.grpBoxValidOptions.Controls.Add(this.validOptionsListBox);
		this.grpBoxValidOptions.Location = new System.Drawing.Point(16, 141);
		this.grpBoxValidOptions.Margin = new System.Windows.Forms.Padding(4);
		this.grpBoxValidOptions.Name = "grpBoxValidOptions";
		this.grpBoxValidOptions.Padding = new System.Windows.Forms.Padding(4);
		this.grpBoxValidOptions.Size = new System.Drawing.Size(252, 293);
		this.grpBoxValidOptions.TabIndex = 7;
		this.grpBoxValidOptions.TabStop = false;
		this.grpBoxValidOptions.Text = "Valid Options";
		this.validOptionsListBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.validOptionsListBox.CheckOnClick = true;
		this.validOptionsListBox.FormattingEnabled = true;
		this.validOptionsListBox.Location = new System.Drawing.Point(4, 20);
		this.validOptionsListBox.Margin = new System.Windows.Forms.Padding(4);
		this.validOptionsListBox.MultiColumn = true;
		this.validOptionsListBox.Name = "validOptionsListBox";
		this.validOptionsListBox.Size = new System.Drawing.Size(243, 259);
		this.validOptionsListBox.TabIndex = 0;
		this.validOptionsListBox.LostFocus += new System.EventHandler(ValidOptionsListBox_LostFocus);
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.grpBoxPrompts.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.grpBoxPrompts.Controls.Add(this.invalidDigitPromptsButton);
		this.grpBoxPrompts.Controls.Add(this.timeoutPromptsButton);
		this.grpBoxPrompts.Controls.Add(this.subsequentPromptsButton);
		this.grpBoxPrompts.Controls.Add(this.initialPromptsButton);
		this.grpBoxPrompts.Location = new System.Drawing.Point(276, 141);
		this.grpBoxPrompts.Margin = new System.Windows.Forms.Padding(4);
		this.grpBoxPrompts.Name = "grpBoxPrompts";
		this.grpBoxPrompts.Padding = new System.Windows.Forms.Padding(4);
		this.grpBoxPrompts.Size = new System.Drawing.Size(327, 293);
		this.grpBoxPrompts.TabIndex = 8;
		this.grpBoxPrompts.TabStop = false;
		this.grpBoxPrompts.Text = "Prompts";
		this.invalidDigitPromptsButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.invalidDigitPromptsButton.Location = new System.Drawing.Point(8, 130);
		this.invalidDigitPromptsButton.Margin = new System.Windows.Forms.Padding(4);
		this.invalidDigitPromptsButton.Name = "invalidDigitPromptsButton";
		this.invalidDigitPromptsButton.Size = new System.Drawing.Size(311, 28);
		this.invalidDigitPromptsButton.TabIndex = 3;
		this.invalidDigitPromptsButton.Text = "Invalid Digit Prompts";
		this.invalidDigitPromptsButton.UseVisualStyleBackColor = true;
		this.invalidDigitPromptsButton.Click += new System.EventHandler(InvalidDigitPromptsButton_Click);
		this.timeoutPromptsButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.timeoutPromptsButton.Location = new System.Drawing.Point(8, 95);
		this.timeoutPromptsButton.Margin = new System.Windows.Forms.Padding(4);
		this.timeoutPromptsButton.Name = "timeoutPromptsButton";
		this.timeoutPromptsButton.Size = new System.Drawing.Size(311, 28);
		this.timeoutPromptsButton.TabIndex = 2;
		this.timeoutPromptsButton.Text = "Timeout Prompts";
		this.timeoutPromptsButton.UseVisualStyleBackColor = true;
		this.timeoutPromptsButton.Click += new System.EventHandler(TimeoutPromptsButton_Click);
		this.subsequentPromptsButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.subsequentPromptsButton.Location = new System.Drawing.Point(8, 59);
		this.subsequentPromptsButton.Margin = new System.Windows.Forms.Padding(4);
		this.subsequentPromptsButton.Name = "subsequentPromptsButton";
		this.subsequentPromptsButton.Size = new System.Drawing.Size(311, 28);
		this.subsequentPromptsButton.TabIndex = 1;
		this.subsequentPromptsButton.Text = "Subsequent Prompts";
		this.subsequentPromptsButton.UseVisualStyleBackColor = true;
		this.subsequentPromptsButton.Click += new System.EventHandler(SubsequentPromptsButton_Click);
		this.initialPromptsButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.initialPromptsButton.Location = new System.Drawing.Point(8, 23);
		this.initialPromptsButton.Margin = new System.Windows.Forms.Padding(4);
		this.initialPromptsButton.Name = "initialPromptsButton";
		this.initialPromptsButton.Size = new System.Drawing.Size(311, 28);
		this.initialPromptsButton.TabIndex = 0;
		this.initialPromptsButton.Text = "Initial Prompts";
		this.initialPromptsButton.UseVisualStyleBackColor = true;
		this.initialPromptsButton.Click += new System.EventHandler(InitialPromptsButton_Click);
		this.comboRepeatOption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboRepeatOption.FormattingEnabled = true;
		this.comboRepeatOption.Location = new System.Drawing.Point(121, 109);
		this.comboRepeatOption.Margin = new System.Windows.Forms.Padding(4);
		this.comboRepeatOption.Name = "comboRepeatOption";
		this.comboRepeatOption.Size = new System.Drawing.Size(139, 24);
		this.comboRepeatOption.TabIndex = 6;
		this.lblRepeatMenuOption.AutoSize = true;
		this.lblRepeatMenuOption.Location = new System.Drawing.Point(13, 112);
		this.lblRepeatMenuOption.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblRepeatMenuOption.Name = "lblRepeatMenuOption";
		this.lblRepeatMenuOption.Size = new System.Drawing.Size(93, 17);
		this.lblRepeatMenuOption.TabIndex = 5;
		this.lblRepeatMenuOption.Text = "Repeat Option";
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(619, 484);
		base.Controls.Add(this.comboRepeatOption);
		base.Controls.Add(this.lblRepeatMenuOption);
		base.Controls.Add(this.grpBoxPrompts);
		base.Controls.Add(this.grpBoxValidOptions);
		base.Controls.Add(this.lblMaxRetryCount);
		base.Controls.Add(this.txtMaxRetryCount);
		base.Controls.Add(this.lblTimeout);
		base.Controls.Add(this.txtTimeout);
		base.Controls.Add(this.chkAcceptDtmfInput);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(1359, 777);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(634, 469);
		base.Name = "MenuConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Menu";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(MenuConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(MenuConfigurationForm_HelpRequested);
		this.grpBoxValidOptions.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		this.grpBoxPrompts.ResumeLayout(false);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
