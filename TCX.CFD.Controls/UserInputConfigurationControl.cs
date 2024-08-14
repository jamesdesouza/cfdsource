using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;
using TCX.CFD.Forms;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class UserInputConfigurationControl : UserControl
{
	private IVadActivity parentActivity;

	private IContainer components;

	private CheckBox chkAcceptDtmfInput;

	private Label lblFirstDigitTimeout;

	private Label lblFinalDigitTimeout;

	private Label lblInterDigitTimeout;

	private Label lblMaxRetryCount;

	private MaskedTextBox txtMaxRetryCount;

	private MaskedTextBox txtFinalDigitTimeout;

	private MaskedTextBox txtInterDigitTimeout;

	private MaskedTextBox txtFirstDigitTimeout;

	private ErrorProvider errorProvider;

	private CheckedListBox validDigitsListBox;

	private ComboBox comboStopDigit;

	private Label lblValidDigits;

	private Label lblMaxDigits;

	private Label lblStopDigit;

	private Label lblMinDigits;

	private MaskedTextBox txtMaxDigits;

	private MaskedTextBox txtMinDigits;

	private GroupBox grpBoxPrompts;

	private Button invalidDigitPromptsButton;

	private Button timeoutPromptsButton;

	private Button subsequentPromptsButton;

	private Button initialPromptsButton;

	public bool AcceptDtmfInput => chkAcceptDtmfInput.Checked;

	public List<Prompt> InitialPrompts { get; private set; }

	public List<Prompt> SubsequentPrompts { get; private set; }

	public List<Prompt> TimeoutPrompts { get; private set; }

	public List<Prompt> InvalidDigitPrompts { get; private set; }

	public uint MaxRetryCount
	{
		get
		{
			if (!string.IsNullOrEmpty(txtMaxRetryCount.Text) && Convert.ToUInt32(txtMaxRetryCount.Text) != 0)
			{
				return Convert.ToUInt32(txtMaxRetryCount.Text);
			}
			return Settings.Default.UserInputTemplateMaxRetryCount;
		}
	}

	public uint FirstDigitTimeout
	{
		get
		{
			if (!string.IsNullOrEmpty(txtFirstDigitTimeout.Text) && Convert.ToUInt32(txtFirstDigitTimeout.Text) != 0)
			{
				return Convert.ToUInt32(txtFirstDigitTimeout.Text);
			}
			return Settings.Default.UserInputTemplateFirstDigitTimeout;
		}
	}

	public uint InterDigitTimeout
	{
		get
		{
			if (!string.IsNullOrEmpty(txtInterDigitTimeout.Text) && Convert.ToUInt32(txtInterDigitTimeout.Text) != 0)
			{
				return Convert.ToUInt32(txtInterDigitTimeout.Text);
			}
			return Settings.Default.UserInputTemplateInterDigitTimeout;
		}
	}

	public uint FinalDigitTimeout
	{
		get
		{
			if (!string.IsNullOrEmpty(txtFinalDigitTimeout.Text) && Convert.ToUInt32(txtFinalDigitTimeout.Text) != 0)
			{
				return Convert.ToUInt32(txtFinalDigitTimeout.Text);
			}
			return Settings.Default.UserInputTemplateFinalDigitTimeout;
		}
	}

	public uint MinDigits
	{
		get
		{
			if (!string.IsNullOrEmpty(txtMinDigits.Text) && Convert.ToUInt32(txtMinDigits.Text) != 0)
			{
				return Convert.ToUInt32(txtMinDigits.Text);
			}
			return Settings.Default.UserInputTemplateMinDigits;
		}
	}

	public uint MaxDigits
	{
		get
		{
			if (!string.IsNullOrEmpty(txtMaxDigits.Text) && Convert.ToUInt32(txtMaxDigits.Text) != 0)
			{
				return Convert.ToUInt32(txtMaxDigits.Text);
			}
			return Settings.Default.UserInputTemplateMaxDigits;
		}
	}

	public DtmfDigits StopDigit => (DtmfDigits)comboStopDigit.SelectedItem;

	public bool IsValidDigit_0 => IsDigitValid(0);

	public bool IsValidDigit_1 => IsDigitValid(1);

	public bool IsValidDigit_2 => IsDigitValid(2);

	public bool IsValidDigit_3 => IsDigitValid(3);

	public bool IsValidDigit_4 => IsDigitValid(4);

	public bool IsValidDigit_5 => IsDigitValid(5);

	public bool IsValidDigit_6 => IsDigitValid(6);

	public bool IsValidDigit_7 => IsDigitValid(7);

	public bool IsValidDigit_8 => IsDigitValid(8);

	public bool IsValidDigit_9 => IsDigitValid(9);

	public bool IsValidDigit_Star => IsDigitValid(10);

	public bool IsValidDigit_Pound => IsDigitValid(11);

	private bool IsDigitValid(int index)
	{
		return validDigitsListBox.GetItemChecked(index);
	}

	private void ValidDigitsListBox_LostFocus(object sender, EventArgs e)
	{
		validDigitsListBox.SelectedIndex = -1;
	}

	private void MaskedTextBox_GotFocus(object sender, EventArgs e)
	{
		(sender as MaskedTextBox).SelectAll();
	}

	private void TxtMaxRetryCount_Validating(object sender, CancelEventArgs e)
	{
		errorProvider.SetError(txtMaxRetryCount, string.IsNullOrEmpty(txtMaxRetryCount.Text) ? LocalizedResourceMgr.GetString("UserInputConfigurationControl.Error.MaxRetryCountIsMandatory") : ((Convert.ToUInt32(txtMaxRetryCount.Text) == 0) ? LocalizedResourceMgr.GetString("UserInputConfigurationControl.Error.MaxRetryCountInvalidRange") : string.Empty));
	}

	private void TxtFirstDigitTimeout_Validating(object sender, CancelEventArgs e)
	{
		errorProvider.SetError(txtFirstDigitTimeout, string.IsNullOrEmpty(txtFirstDigitTimeout.Text) ? LocalizedResourceMgr.GetString("UserInputConfigurationControl.Error.FirstDigitTimeoutIsMandatory") : ((Convert.ToUInt32(txtFirstDigitTimeout.Text) == 0) ? LocalizedResourceMgr.GetString("UserInputConfigurationControl.Error.FirstDigitTimeoutInvalidRange") : string.Empty));
	}

	private void TxtInterDigitTimeout_Validating(object sender, CancelEventArgs e)
	{
		errorProvider.SetError(txtInterDigitTimeout, string.IsNullOrEmpty(txtInterDigitTimeout.Text) ? LocalizedResourceMgr.GetString("UserInputConfigurationControl.Error.InterDigitTimeoutIsMandatory") : ((Convert.ToUInt32(txtInterDigitTimeout.Text) == 0) ? LocalizedResourceMgr.GetString("UserInputConfigurationControl.Error.InterDigitTimeoutInvalidRange") : string.Empty));
	}

	private void TxtFinalDigitTimeout_Validating(object sender, CancelEventArgs e)
	{
		errorProvider.SetError(txtFinalDigitTimeout, string.IsNullOrEmpty(txtFinalDigitTimeout.Text) ? LocalizedResourceMgr.GetString("UserInputConfigurationControl.Error.FinalDigitTimeoutIsMandatory") : ((Convert.ToUInt32(txtFinalDigitTimeout.Text) == 0) ? LocalizedResourceMgr.GetString("UserInputConfigurationControl.Error.FinalDigitTimeoutInvalidRange") : string.Empty));
	}

	private void ValidateMinDigits()
	{
		if (string.IsNullOrEmpty(txtMinDigits.Text))
		{
			errorProvider.SetError(txtMinDigits, LocalizedResourceMgr.GetString("UserInputConfigurationControl.Error.MinDigitsIsMandatory"));
			return;
		}
		int num = Convert.ToInt32(txtMinDigits.Text);
		if (num < 1 || num > 999)
		{
			errorProvider.SetError(txtMinDigits, string.Format(LocalizedResourceMgr.GetString("UserInputConfigurationControl.Error.InvalidMinDigitsValue"), 1, 999));
		}
		else if (!string.IsNullOrEmpty(txtMaxDigits.Text))
		{
			int num2 = Convert.ToInt32(txtMaxDigits.Text);
			errorProvider.SetError(txtMinDigits, (num > num2) ? LocalizedResourceMgr.GetString("UserInputConfigurationControl.Error.MinDigitsCantBeGreaterThanMaxDigits") : string.Empty);
		}
		else
		{
			errorProvider.SetError(txtMinDigits, string.Empty);
		}
	}

	private void ValidateMaxDigits()
	{
		if (string.IsNullOrEmpty(txtMaxDigits.Text))
		{
			errorProvider.SetError(txtMaxDigits, LocalizedResourceMgr.GetString("UserInputConfigurationControl.Error.MaxDigitsIsMandatory"));
			return;
		}
		int num = Convert.ToInt32(txtMaxDigits.Text);
		if (num < 1 || num > 999)
		{
			errorProvider.SetError(txtMaxDigits, string.Format(LocalizedResourceMgr.GetString("UserInputConfigurationControl.Error.InvalidMaxDigitsValue"), 1, 999));
		}
		else if (!string.IsNullOrEmpty(txtMinDigits.Text))
		{
			int num2 = Convert.ToInt32(txtMinDigits.Text);
			errorProvider.SetError(txtMaxDigits, (num2 > num) ? LocalizedResourceMgr.GetString("UserInputConfigurationControl.Error.MinDigitsCantBeGreaterThanMaxDigits") : string.Empty);
		}
		else
		{
			errorProvider.SetError(txtMaxDigits, string.Empty);
		}
	}

	private void TxtMinDigits_Validating(object sender, CancelEventArgs e)
	{
		ValidateMinDigits();
		ValidateMaxDigits();
	}

	private void TxtMaxDigits_Validating(object sender, CancelEventArgs e)
	{
		ValidateMinDigits();
		ValidateMaxDigits();
	}

	private void InitialPromptsButton_Click(object sender, EventArgs e)
	{
		PromptCollectionEditorForm promptCollectionEditorForm = new PromptCollectionEditorForm(parentActivity);
		promptCollectionEditorForm.PromptList = InitialPrompts;
		if (promptCollectionEditorForm.ShowDialog() == DialogResult.OK)
		{
			InitialPrompts = promptCollectionEditorForm.PromptList;
		}
	}

	private void SubsequentPromptsButton_Click(object sender, EventArgs e)
	{
		PromptCollectionEditorForm promptCollectionEditorForm = new PromptCollectionEditorForm(parentActivity);
		promptCollectionEditorForm.PromptList = SubsequentPrompts;
		if (promptCollectionEditorForm.ShowDialog() == DialogResult.OK)
		{
			SubsequentPrompts = promptCollectionEditorForm.PromptList;
		}
	}

	private void TimeoutPromptsButton_Click(object sender, EventArgs e)
	{
		PromptCollectionEditorForm promptCollectionEditorForm = new PromptCollectionEditorForm(parentActivity);
		promptCollectionEditorForm.PromptList = TimeoutPrompts;
		if (promptCollectionEditorForm.ShowDialog() == DialogResult.OK)
		{
			TimeoutPrompts = promptCollectionEditorForm.PromptList;
		}
	}

	private void InvalidDigitPromptsButton_Click(object sender, EventArgs e)
	{
		PromptCollectionEditorForm promptCollectionEditorForm = new PromptCollectionEditorForm(parentActivity);
		promptCollectionEditorForm.PromptList = InvalidDigitPrompts;
		if (promptCollectionEditorForm.ShowDialog() == DialogResult.OK)
		{
			InvalidDigitPrompts = promptCollectionEditorForm.PromptList;
		}
	}

	public UserInputConfigurationControl()
	{
		InitializeComponent();
	}

	public void SetUserInputComponent(IVadActivity parentActivity, UserInputComponent userInputComponent, int extraSize)
	{
		this.parentActivity = parentActivity;
		InitialPrompts = new List<Prompt>(userInputComponent.InitialPrompts);
		SubsequentPrompts = new List<Prompt>(userInputComponent.SubsequentPrompts);
		TimeoutPrompts = new List<Prompt>(userInputComponent.TimeoutPrompts);
		InvalidDigitPrompts = new List<Prompt>(userInputComponent.InvalidDigitPrompts);
		comboStopDigit.Items.AddRange(new object[13]
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
		chkAcceptDtmfInput.Checked = userInputComponent.AcceptDtmfInput;
		txtMaxRetryCount.Text = userInputComponent.MaxRetryCount.ToString();
		txtFirstDigitTimeout.Text = userInputComponent.FirstDigitTimeout.ToString();
		txtInterDigitTimeout.Text = userInputComponent.InterDigitTimeout.ToString();
		txtFinalDigitTimeout.Text = userInputComponent.FinalDigitTimeout.ToString();
		txtMinDigits.Text = userInputComponent.MinDigits.ToString();
		txtMaxDigits.Text = userInputComponent.MaxDigits.ToString();
		comboStopDigit.SelectedItem = userInputComponent.StopDigit;
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("UserInputConfigurationControl.validDigitsListBox.DigitText") + " 0", userInputComponent.IsValidDigit_0);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("UserInputConfigurationControl.validDigitsListBox.DigitText") + " 1", userInputComponent.IsValidDigit_1);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("UserInputConfigurationControl.validDigitsListBox.DigitText") + " 2", userInputComponent.IsValidDigit_2);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("UserInputConfigurationControl.validDigitsListBox.DigitText") + " 3", userInputComponent.IsValidDigit_3);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("UserInputConfigurationControl.validDigitsListBox.DigitText") + " 4", userInputComponent.IsValidDigit_4);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("UserInputConfigurationControl.validDigitsListBox.DigitText") + " 5", userInputComponent.IsValidDigit_5);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("UserInputConfigurationControl.validDigitsListBox.DigitText") + " 6", userInputComponent.IsValidDigit_6);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("UserInputConfigurationControl.validDigitsListBox.DigitText") + " 7", userInputComponent.IsValidDigit_7);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("UserInputConfigurationControl.validDigitsListBox.DigitText") + " 8", userInputComponent.IsValidDigit_8);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("UserInputConfigurationControl.validDigitsListBox.DigitText") + " 9", userInputComponent.IsValidDigit_9);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("UserInputConfigurationControl.validDigitsListBox.DigitText") + " *", userInputComponent.IsValidDigit_Star);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("UserInputConfigurationControl.validDigitsListBox.DigitText") + " #", userInputComponent.IsValidDigit_Pound);
		TxtMaxRetryCount_Validating(txtMaxRetryCount, new CancelEventArgs());
		TxtFirstDigitTimeout_Validating(txtFirstDigitTimeout, new CancelEventArgs());
		TxtInterDigitTimeout_Validating(txtInterDigitTimeout, new CancelEventArgs());
		TxtFinalDigitTimeout_Validating(txtFinalDigitTimeout, new CancelEventArgs());
		chkAcceptDtmfInput.Text = LocalizedResourceMgr.GetString("UserInputConfigurationControl.chkAcceptDtmfInput.Text");
		lblMaxRetryCount.Text = LocalizedResourceMgr.GetString("UserInputConfigurationControl.lblMaxRetryCount.Text");
		lblFirstDigitTimeout.Text = LocalizedResourceMgr.GetString("UserInputConfigurationControl.lblFirstDigitTimeout.Text");
		lblInterDigitTimeout.Text = LocalizedResourceMgr.GetString("UserInputConfigurationControl.lblInterDigitTimeout.Text");
		lblFinalDigitTimeout.Text = LocalizedResourceMgr.GetString("UserInputConfigurationControl.lblFinalDigitTimeout.Text");
		lblMinDigits.Text = LocalizedResourceMgr.GetString("UserInputConfigurationControl.lblMinDigits.Text");
		lblMaxDigits.Text = LocalizedResourceMgr.GetString("UserInputConfigurationControl.lblMaxDigits.Text");
		lblStopDigit.Text = LocalizedResourceMgr.GetString("UserInputConfigurationControl.lblStopDigit.Text");
		lblValidDigits.Text = LocalizedResourceMgr.GetString("UserInputConfigurationControl.lblValidDigits.Text");
		grpBoxPrompts.Text = LocalizedResourceMgr.GetString("UserInputConfigurationControl.grpBoxPrompts.Text");
		initialPromptsButton.Text = LocalizedResourceMgr.GetString("UserInputConfigurationControl.initialPromptsButton.Text");
		subsequentPromptsButton.Text = LocalizedResourceMgr.GetString("UserInputConfigurationControl.subsequentPromptsButton.Text");
		timeoutPromptsButton.Text = LocalizedResourceMgr.GetString("UserInputConfigurationControl.timeoutPromptsButton.Text");
		invalidDigitPromptsButton.Text = LocalizedResourceMgr.GetString("UserInputConfigurationControl.invalidDigitPromptsButton.Text");
		if (extraSize > 0)
		{
			int num = extraSize / 2;
			txtMaxRetryCount.Width += num;
			grpBoxPrompts.Width += extraSize;
			initialPromptsButton.Width += num;
			subsequentPromptsButton.Width += num;
			timeoutPromptsButton.Width += num;
			invalidDigitPromptsButton.Width += num;
			txtFirstDigitTimeout.Width += num;
			txtInterDigitTimeout.Width += num;
			txtFinalDigitTimeout.Width += num;
			txtMinDigits.Width += num;
			txtMaxDigits.Width += num;
			comboStopDigit.Width += num;
			validDigitsListBox.Width += extraSize;
			lblMaxRetryCount.Location = new Point(lblMaxRetryCount.Location.X + num, lblMaxRetryCount.Location.Y);
			txtMaxRetryCount.Location = new Point(txtMaxRetryCount.Location.X + num, txtMaxRetryCount.Location.Y);
			timeoutPromptsButton.Location = new Point(timeoutPromptsButton.Location.X + num, timeoutPromptsButton.Location.Y);
			invalidDigitPromptsButton.Location = new Point(invalidDigitPromptsButton.Location.X + num, invalidDigitPromptsButton.Location.Y);
			lblMinDigits.Location = new Point(lblMinDigits.Location.X + num, lblMinDigits.Location.Y);
			txtMinDigits.Location = new Point(txtMinDigits.Location.X + num, txtMinDigits.Location.Y);
			lblMaxDigits.Location = new Point(lblMaxDigits.Location.X + num, lblMaxDigits.Location.Y);
			txtMaxDigits.Location = new Point(txtMaxDigits.Location.X + num, txtMaxDigits.Location.Y);
			lblStopDigit.Location = new Point(lblStopDigit.Location.X + num, lblStopDigit.Location.Y);
			comboStopDigit.Location = new Point(comboStopDigit.Location.X + num, comboStopDigit.Location.Y);
		}
	}

	public bool ValidateFieldsOnSave()
	{
		if (string.IsNullOrEmpty(txtMaxRetryCount.Text))
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("UserInputConfigurationControl.Error.MaxRetryCountIsMandatory"), LocalizedResourceMgr.GetString("UserInputConfigurationControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			txtMaxRetryCount.Focus();
			return false;
		}
		if (Convert.ToUInt32(txtMaxRetryCount.Text) == 0)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("UserInputConfigurationControl.Error.MaxRetryCountInvalidRange"), LocalizedResourceMgr.GetString("UserInputConfigurationControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			txtMaxRetryCount.Focus();
			return false;
		}
		if (string.IsNullOrEmpty(txtFirstDigitTimeout.Text))
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("UserInputConfigurationControl.Error.FirstDigitTimeoutIsMandatory"), LocalizedResourceMgr.GetString("UserInputConfigurationControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			txtFirstDigitTimeout.Focus();
			return false;
		}
		if (Convert.ToUInt32(txtFirstDigitTimeout.Text) == 0)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("UserInputConfigurationControl.Error.FirstDigitTimeoutInvalidRange"), LocalizedResourceMgr.GetString("UserInputConfigurationControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			txtFirstDigitTimeout.Focus();
			return false;
		}
		if (string.IsNullOrEmpty(txtInterDigitTimeout.Text))
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("UserInputConfigurationControl.Error.InterDigitTimeoutIsMandatory"), LocalizedResourceMgr.GetString("UserInputConfigurationControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			txtInterDigitTimeout.Focus();
			return false;
		}
		if (Convert.ToUInt32(txtInterDigitTimeout.Text) == 0)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("UserInputConfigurationControl.Error.InterDigitTimeoutInvalidRange"), LocalizedResourceMgr.GetString("UserInputConfigurationControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			txtInterDigitTimeout.Focus();
			return false;
		}
		if (string.IsNullOrEmpty(txtFinalDigitTimeout.Text))
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("UserInputConfigurationControl.Error.FinalDigitTimeoutIsMandatory"), LocalizedResourceMgr.GetString("UserInputConfigurationControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			txtFinalDigitTimeout.Focus();
			return false;
		}
		if (Convert.ToUInt32(txtFinalDigitTimeout.Text) == 0)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("UserInputConfigurationControl.Error.FinalDigitTimeoutInvalidRange"), LocalizedResourceMgr.GetString("UserInputConfigurationControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			txtFinalDigitTimeout.Focus();
			return false;
		}
		if (string.IsNullOrEmpty(txtMinDigits.Text))
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("UserInputConfigurationControl.Error.MinDigitsIsMandatory"), LocalizedResourceMgr.GetString("UserInputConfigurationControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			txtMinDigits.Focus();
			return false;
		}
		if (Convert.ToUInt32(txtMinDigits.Text) == 0)
		{
			MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("UserInputConfigurationControl.Error.InvalidMinDigitsValue"), 1, 999), LocalizedResourceMgr.GetString("UserInputConfigurationControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			txtMinDigits.Focus();
			return false;
		}
		if (string.IsNullOrEmpty(txtMaxDigits.Text))
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("UserInputConfigurationControl.Error.MaxDigitsIsMandatory"), LocalizedResourceMgr.GetString("UserInputConfigurationControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			txtMaxDigits.Focus();
			return false;
		}
		if (Convert.ToUInt32(txtMaxDigits.Text) == 0)
		{
			MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("UserInputConfigurationControl.Error.InvalidMaxDigitsValue"), 1, 999), LocalizedResourceMgr.GetString("UserInputConfigurationControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			txtMaxDigits.Focus();
			return false;
		}
		return true;
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
		this.chkAcceptDtmfInput = new System.Windows.Forms.CheckBox();
		this.txtFinalDigitTimeout = new System.Windows.Forms.MaskedTextBox();
		this.txtInterDigitTimeout = new System.Windows.Forms.MaskedTextBox();
		this.txtFirstDigitTimeout = new System.Windows.Forms.MaskedTextBox();
		this.lblFinalDigitTimeout = new System.Windows.Forms.Label();
		this.lblInterDigitTimeout = new System.Windows.Forms.Label();
		this.lblFirstDigitTimeout = new System.Windows.Forms.Label();
		this.lblMaxRetryCount = new System.Windows.Forms.Label();
		this.txtMaxRetryCount = new System.Windows.Forms.MaskedTextBox();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.validDigitsListBox = new System.Windows.Forms.CheckedListBox();
		this.comboStopDigit = new System.Windows.Forms.ComboBox();
		this.lblValidDigits = new System.Windows.Forms.Label();
		this.lblMaxDigits = new System.Windows.Forms.Label();
		this.lblStopDigit = new System.Windows.Forms.Label();
		this.lblMinDigits = new System.Windows.Forms.Label();
		this.txtMaxDigits = new System.Windows.Forms.MaskedTextBox();
		this.txtMinDigits = new System.Windows.Forms.MaskedTextBox();
		this.grpBoxPrompts = new System.Windows.Forms.GroupBox();
		this.invalidDigitPromptsButton = new System.Windows.Forms.Button();
		this.timeoutPromptsButton = new System.Windows.Forms.Button();
		this.subsequentPromptsButton = new System.Windows.Forms.Button();
		this.initialPromptsButton = new System.Windows.Forms.Button();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		this.grpBoxPrompts.SuspendLayout();
		base.SuspendLayout();
		this.chkAcceptDtmfInput.AutoSize = true;
		this.chkAcceptDtmfInput.Location = new System.Drawing.Point(4, 4);
		this.chkAcceptDtmfInput.Margin = new System.Windows.Forms.Padding(4);
		this.chkAcceptDtmfInput.Name = "chkAcceptDtmfInput";
		this.chkAcceptDtmfInput.Size = new System.Drawing.Size(245, 21);
		this.chkAcceptDtmfInput.TabIndex = 0;
		this.chkAcceptDtmfInput.Text = "Accept DTMF Input During Prompt";
		this.chkAcceptDtmfInput.UseVisualStyleBackColor = true;
		this.txtFinalDigitTimeout.HidePromptOnLeave = true;
		this.txtFinalDigitTimeout.Location = new System.Drawing.Point(179, 204);
		this.txtFinalDigitTimeout.Margin = new System.Windows.Forms.Padding(4);
		this.txtFinalDigitTimeout.Mask = "99";
		this.txtFinalDigitTimeout.Name = "txtFinalDigitTimeout";
		this.txtFinalDigitTimeout.Size = new System.Drawing.Size(91, 22);
		this.txtFinalDigitTimeout.TabIndex = 9;
		this.txtFinalDigitTimeout.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtFinalDigitTimeout.Validating += new System.ComponentModel.CancelEventHandler(TxtFinalDigitTimeout_Validating);
		this.txtInterDigitTimeout.HidePromptOnLeave = true;
		this.txtInterDigitTimeout.Location = new System.Drawing.Point(179, 172);
		this.txtInterDigitTimeout.Margin = new System.Windows.Forms.Padding(4);
		this.txtInterDigitTimeout.Mask = "99";
		this.txtInterDigitTimeout.Name = "txtInterDigitTimeout";
		this.txtInterDigitTimeout.Size = new System.Drawing.Size(91, 22);
		this.txtInterDigitTimeout.TabIndex = 7;
		this.txtInterDigitTimeout.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtInterDigitTimeout.Validating += new System.ComponentModel.CancelEventHandler(TxtInterDigitTimeout_Validating);
		this.txtFirstDigitTimeout.HidePromptOnLeave = true;
		this.txtFirstDigitTimeout.Location = new System.Drawing.Point(179, 140);
		this.txtFirstDigitTimeout.Margin = new System.Windows.Forms.Padding(4);
		this.txtFirstDigitTimeout.Mask = "99";
		this.txtFirstDigitTimeout.Name = "txtFirstDigitTimeout";
		this.txtFirstDigitTimeout.Size = new System.Drawing.Size(91, 22);
		this.txtFirstDigitTimeout.TabIndex = 5;
		this.txtFirstDigitTimeout.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtFirstDigitTimeout.Validating += new System.ComponentModel.CancelEventHandler(TxtFirstDigitTimeout_Validating);
		this.lblFinalDigitTimeout.AutoSize = true;
		this.lblFinalDigitTimeout.Location = new System.Drawing.Point(4, 208);
		this.lblFinalDigitTimeout.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblFinalDigitTimeout.Name = "lblFinalDigitTimeout";
		this.lblFinalDigitTimeout.Size = new System.Drawing.Size(168, 17);
		this.lblFinalDigitTimeout.TabIndex = 8;
		this.lblFinalDigitTimeout.Text = "Final Digit Timeout (secs)";
		this.lblInterDigitTimeout.AutoSize = true;
		this.lblInterDigitTimeout.Location = new System.Drawing.Point(4, 176);
		this.lblInterDigitTimeout.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblInterDigitTimeout.Name = "lblInterDigitTimeout";
		this.lblInterDigitTimeout.Size = new System.Drawing.Size(166, 17);
		this.lblInterDigitTimeout.TabIndex = 6;
		this.lblInterDigitTimeout.Text = "Inter Digit Timeout (secs)";
		this.lblFirstDigitTimeout.AutoSize = true;
		this.lblFirstDigitTimeout.Location = new System.Drawing.Point(4, 144);
		this.lblFirstDigitTimeout.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblFirstDigitTimeout.Name = "lblFirstDigitTimeout";
		this.lblFirstDigitTimeout.Size = new System.Drawing.Size(165, 17);
		this.lblFirstDigitTimeout.TabIndex = 4;
		this.lblFirstDigitTimeout.Text = "First Digit Timeout (secs)";
		this.lblMaxRetryCount.AutoSize = true;
		this.lblMaxRetryCount.Location = new System.Drawing.Point(297, 9);
		this.lblMaxRetryCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblMaxRetryCount.Name = "lblMaxRetryCount";
		this.lblMaxRetryCount.Size = new System.Drawing.Size(112, 17);
		this.lblMaxRetryCount.TabIndex = 1;
		this.lblMaxRetryCount.Text = "Max Retry Count";
		this.txtMaxRetryCount.HidePromptOnLeave = true;
		this.txtMaxRetryCount.Location = new System.Drawing.Point(420, 5);
		this.txtMaxRetryCount.Margin = new System.Windows.Forms.Padding(4);
		this.txtMaxRetryCount.Mask = "99";
		this.txtMaxRetryCount.Name = "txtMaxRetryCount";
		this.txtMaxRetryCount.Size = new System.Drawing.Size(99, 22);
		this.txtMaxRetryCount.TabIndex = 2;
		this.txtMaxRetryCount.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtMaxRetryCount.Validating += new System.ComponentModel.CancelEventHandler(TxtMaxRetryCount_Validating);
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.validDigitsListBox.CheckOnClick = true;
		this.validDigitsListBox.FormattingEnabled = true;
		this.validDigitsListBox.Location = new System.Drawing.Point(4, 257);
		this.validDigitsListBox.Margin = new System.Windows.Forms.Padding(4);
		this.validDigitsListBox.MinimumSize = new System.Drawing.Size(540, 78);
		this.validDigitsListBox.MultiColumn = true;
		this.validDigitsListBox.Name = "validDigitsListBox";
		this.validDigitsListBox.Size = new System.Drawing.Size(541, 72);
		this.validDigitsListBox.TabIndex = 17;
		this.validDigitsListBox.LostFocus += new System.EventHandler(ValidDigitsListBox_LostFocus);
		this.comboStopDigit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboStopDigit.FormattingEnabled = true;
		this.comboStopDigit.Location = new System.Drawing.Point(380, 204);
		this.comboStopDigit.Margin = new System.Windows.Forms.Padding(4);
		this.comboStopDigit.Name = "comboStopDigit";
		this.comboStopDigit.Size = new System.Drawing.Size(139, 24);
		this.comboStopDigit.TabIndex = 15;
		this.lblValidDigits.AutoSize = true;
		this.lblValidDigits.Location = new System.Drawing.Point(4, 238);
		this.lblValidDigits.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblValidDigits.Name = "lblValidDigits";
		this.lblValidDigits.Size = new System.Drawing.Size(78, 17);
		this.lblValidDigits.TabIndex = 16;
		this.lblValidDigits.Text = "Valid Digits";
		this.lblMaxDigits.AutoSize = true;
		this.lblMaxDigits.Location = new System.Drawing.Point(297, 176);
		this.lblMaxDigits.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblMaxDigits.Name = "lblMaxDigits";
		this.lblMaxDigits.Size = new System.Drawing.Size(72, 17);
		this.lblMaxDigits.TabIndex = 12;
		this.lblMaxDigits.Text = "Max Digits";
		this.lblStopDigit.AutoSize = true;
		this.lblStopDigit.Location = new System.Drawing.Point(297, 208);
		this.lblStopDigit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblStopDigit.Name = "lblStopDigit";
		this.lblStopDigit.Size = new System.Drawing.Size(69, 17);
		this.lblStopDigit.TabIndex = 14;
		this.lblStopDigit.Text = "Stop Digit";
		this.lblMinDigits.AutoSize = true;
		this.lblMinDigits.Location = new System.Drawing.Point(297, 144);
		this.lblMinDigits.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblMinDigits.Name = "lblMinDigits";
		this.lblMinDigits.Size = new System.Drawing.Size(69, 17);
		this.lblMinDigits.TabIndex = 10;
		this.lblMinDigits.Text = "Min Digits";
		this.txtMaxDigits.HidePromptOnLeave = true;
		this.txtMaxDigits.Location = new System.Drawing.Point(380, 172);
		this.txtMaxDigits.Margin = new System.Windows.Forms.Padding(4);
		this.txtMaxDigits.Mask = "999";
		this.txtMaxDigits.Name = "txtMaxDigits";
		this.txtMaxDigits.Size = new System.Drawing.Size(139, 22);
		this.txtMaxDigits.TabIndex = 13;
		this.txtMaxDigits.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtMaxDigits.Validating += new System.ComponentModel.CancelEventHandler(TxtMaxDigits_Validating);
		this.txtMinDigits.HidePromptOnLeave = true;
		this.txtMinDigits.Location = new System.Drawing.Point(380, 140);
		this.txtMinDigits.Margin = new System.Windows.Forms.Padding(4);
		this.txtMinDigits.Mask = "999";
		this.txtMinDigits.Name = "txtMinDigits";
		this.txtMinDigits.Size = new System.Drawing.Size(139, 22);
		this.txtMinDigits.TabIndex = 11;
		this.txtMinDigits.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtMinDigits.Validating += new System.ComponentModel.CancelEventHandler(TxtMinDigits_Validating);
		this.grpBoxPrompts.Controls.Add(this.invalidDigitPromptsButton);
		this.grpBoxPrompts.Controls.Add(this.timeoutPromptsButton);
		this.grpBoxPrompts.Controls.Add(this.subsequentPromptsButton);
		this.grpBoxPrompts.Controls.Add(this.initialPromptsButton);
		this.grpBoxPrompts.Location = new System.Drawing.Point(4, 32);
		this.grpBoxPrompts.Margin = new System.Windows.Forms.Padding(4);
		this.grpBoxPrompts.Name = "grpBoxPrompts";
		this.grpBoxPrompts.Padding = new System.Windows.Forms.Padding(4);
		this.grpBoxPrompts.Size = new System.Drawing.Size(541, 101);
		this.grpBoxPrompts.TabIndex = 3;
		this.grpBoxPrompts.TabStop = false;
		this.grpBoxPrompts.Text = "Prompts";
		this.invalidDigitPromptsButton.Location = new System.Drawing.Point(275, 59);
		this.invalidDigitPromptsButton.Margin = new System.Windows.Forms.Padding(4);
		this.invalidDigitPromptsButton.Name = "invalidDigitPromptsButton";
		this.invalidDigitPromptsButton.Size = new System.Drawing.Size(259, 28);
		this.invalidDigitPromptsButton.TabIndex = 3;
		this.invalidDigitPromptsButton.Text = "Invalid Digit Prompts";
		this.invalidDigitPromptsButton.UseVisualStyleBackColor = true;
		this.invalidDigitPromptsButton.Click += new System.EventHandler(InvalidDigitPromptsButton_Click);
		this.timeoutPromptsButton.Location = new System.Drawing.Point(275, 23);
		this.timeoutPromptsButton.Margin = new System.Windows.Forms.Padding(4);
		this.timeoutPromptsButton.Name = "timeoutPromptsButton";
		this.timeoutPromptsButton.Size = new System.Drawing.Size(259, 28);
		this.timeoutPromptsButton.TabIndex = 2;
		this.timeoutPromptsButton.Text = "Timeout Prompts";
		this.timeoutPromptsButton.UseVisualStyleBackColor = true;
		this.timeoutPromptsButton.Click += new System.EventHandler(TimeoutPromptsButton_Click);
		this.subsequentPromptsButton.Location = new System.Drawing.Point(8, 59);
		this.subsequentPromptsButton.Margin = new System.Windows.Forms.Padding(4);
		this.subsequentPromptsButton.Name = "subsequentPromptsButton";
		this.subsequentPromptsButton.Size = new System.Drawing.Size(259, 28);
		this.subsequentPromptsButton.TabIndex = 1;
		this.subsequentPromptsButton.Text = "Subsequent Prompts";
		this.subsequentPromptsButton.UseVisualStyleBackColor = true;
		this.subsequentPromptsButton.Click += new System.EventHandler(SubsequentPromptsButton_Click);
		this.initialPromptsButton.Location = new System.Drawing.Point(8, 23);
		this.initialPromptsButton.Margin = new System.Windows.Forms.Padding(4);
		this.initialPromptsButton.Name = "initialPromptsButton";
		this.initialPromptsButton.Size = new System.Drawing.Size(259, 28);
		this.initialPromptsButton.TabIndex = 0;
		this.initialPromptsButton.Text = "Initial Prompts";
		this.initialPromptsButton.UseVisualStyleBackColor = true;
		this.initialPromptsButton.Click += new System.EventHandler(InitialPromptsButton_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.grpBoxPrompts);
		base.Controls.Add(this.validDigitsListBox);
		base.Controls.Add(this.comboStopDigit);
		base.Controls.Add(this.lblValidDigits);
		base.Controls.Add(this.lblMaxDigits);
		base.Controls.Add(this.lblStopDigit);
		base.Controls.Add(this.lblMinDigits);
		base.Controls.Add(this.txtMaxDigits);
		base.Controls.Add(this.txtMinDigits);
		base.Controls.Add(this.lblMaxRetryCount);
		base.Controls.Add(this.txtFinalDigitTimeout);
		base.Controls.Add(this.txtMaxRetryCount);
		base.Controls.Add(this.txtInterDigitTimeout);
		base.Controls.Add(this.txtFirstDigitTimeout);
		base.Controls.Add(this.lblFinalDigitTimeout);
		base.Controls.Add(this.lblInterDigitTimeout);
		base.Controls.Add(this.lblFirstDigitTimeout);
		base.Controls.Add(this.chkAcceptDtmfInput);
		base.Margin = new System.Windows.Forms.Padding(5);
		base.Name = "UserInputConfigurationControl";
		base.Size = new System.Drawing.Size(549, 346);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		this.grpBoxPrompts.ResumeLayout(false);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
