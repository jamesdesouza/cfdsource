using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class OptionsComponentsUserInputControl : UserControl, IOptionsControl
{
	private IContainer components;

	private ErrorProvider errorProvider;

	private CheckedListBox validDigitsListBox;

	private ComboBox comboStopDigit;

	private Label lblMinDigits;

	private Label lblStopDigit;

	private MaskedTextBox txtMinDigits;

	private Label lblMaxDigits;

	private MaskedTextBox txtMaxDigits;

	private Label lblValidDigits;

	private Label lblMaxRetryCount;

	private MaskedTextBox txtMaxRetryCount;

	private MaskedTextBox txtFinalDigitTimeout;

	private MaskedTextBox txtInterDigitTimeout;

	private Label lblFirstDigitTimeout;

	private MaskedTextBox txtFirstDigitTimeout;

	private CheckBox chkAcceptDtmfInput;

	private Label lblInterDigitTimeout;

	private Label lblFinalDigitTimeout;

	private Label lblUserInput;

	private DtmfDigits GetStopDigit(string s)
	{
		return s switch
		{
			"0" => DtmfDigits.Digit0, 
			"1" => DtmfDigits.Digit1, 
			"2" => DtmfDigits.Digit2, 
			"3" => DtmfDigits.Digit3, 
			"4" => DtmfDigits.Digit4, 
			"5" => DtmfDigits.Digit5, 
			"6" => DtmfDigits.Digit6, 
			"7" => DtmfDigits.Digit7, 
			"8" => DtmfDigits.Digit8, 
			"9" => DtmfDigits.Digit9, 
			"*" => DtmfDigits.DigitStar, 
			"#" => DtmfDigits.DigitPound, 
			_ => DtmfDigits.None, 
		};
	}

	private string GetStopDigitStr(DtmfDigits stopDigit)
	{
		return stopDigit switch
		{
			DtmfDigits.Digit0 => "0", 
			DtmfDigits.Digit1 => "1", 
			DtmfDigits.Digit2 => "2", 
			DtmfDigits.Digit3 => "3", 
			DtmfDigits.Digit4 => "4", 
			DtmfDigits.Digit5 => "5", 
			DtmfDigits.Digit6 => "6", 
			DtmfDigits.Digit7 => "7", 
			DtmfDigits.Digit8 => "8", 
			DtmfDigits.Digit9 => "9", 
			DtmfDigits.DigitStar => "*", 
			DtmfDigits.DigitPound => "#", 
			_ => string.Empty, 
		};
	}

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

	private void ValidateNumericField(string fieldValue)
	{
		if (string.IsNullOrEmpty(fieldValue))
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsUserInputControl.Error.FieldIsMandatory"));
		}
	}

	private void ValidateNonZeroNumericField(string fieldValue)
	{
		ValidateNumericField(fieldValue);
		if (Convert.ToUInt32(fieldValue) == 0)
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsUserInputControl.Error.FieldCanNotBeZero"));
		}
	}

	private void ValidateMinDigits()
	{
		if (string.IsNullOrEmpty(txtMinDigits.Text))
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsUserInputControl.Error.MinDigitsIsMandatory"));
		}
		int num = Convert.ToInt32(txtMinDigits.Text);
		if (num < 1 || num > 999)
		{
			throw new ApplicationException(string.Format(LocalizedResourceMgr.GetString("OptionsComponentsUserInputControl.Error.InvalidMinDigitsValue"), 1, 999));
		}
		if (!string.IsNullOrEmpty(txtMaxDigits.Text))
		{
			int num2 = Convert.ToInt32(txtMaxDigits.Text);
			if (num > num2)
			{
				throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsUserInputControl.Error.MinDigitsCantBeGreaterThanMaxDigits"));
			}
		}
	}

	private void ValidateMaxDigits()
	{
		if (string.IsNullOrEmpty(txtMaxDigits.Text))
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsUserInputControl.Error.MaxDigitsIsMandatory"));
		}
		int num = Convert.ToInt32(txtMaxDigits.Text);
		if (num < 1 || num > 999)
		{
			throw new ApplicationException(string.Format(LocalizedResourceMgr.GetString("OptionsComponentsUserInputControl.Error.InvalidMaxDigitsValue"), 1, 999));
		}
		if (!string.IsNullOrEmpty(txtMinDigits.Text) && Convert.ToInt32(txtMinDigits.Text) > num)
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsUserInputControl.Error.MinDigitsCantBeGreaterThanMaxDigits"));
		}
	}

	private void ValidateStopDigitIsNotValidDigit()
	{
		string stopDigitStr = GetStopDigitStr((DtmfDigits)comboStopDigit.SelectedItem);
		int num = (string.IsNullOrEmpty(stopDigitStr) ? (-1) : Convert.ToInt32(stopDigitStr.Replace("*", "10").Replace("#", "11")));
		if (num >= 0 && IsDigitValid(num))
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsUserInputControl.Error.StopDigitIsValidDigit"));
		}
	}

	private void ValidateFields()
	{
		ValidateNonZeroNumericField(txtMaxRetryCount.Text);
		ValidateNonZeroNumericField(txtFirstDigitTimeout.Text);
		ValidateNonZeroNumericField(txtInterDigitTimeout.Text);
		ValidateNonZeroNumericField(txtFinalDigitTimeout.Text);
		ValidateMinDigits();
		ValidateMaxDigits();
		ValidateStopDigitIsNotValidDigit();
	}

	private void TxtNumeric_Validating(object sender, CancelEventArgs e)
	{
		MaskedTextBox maskedTextBox = sender as MaskedTextBox;
		try
		{
			ValidateNumericField(maskedTextBox.Text);
			errorProvider.SetError(maskedTextBox, string.Empty);
		}
		catch (Exception exc)
		{
			errorProvider.SetError(maskedTextBox, ErrorHelper.GetErrorDescription(exc));
		}
	}

	private void TxtNonZeroNumeric_Validating(object sender, CancelEventArgs e)
	{
		MaskedTextBox maskedTextBox = sender as MaskedTextBox;
		try
		{
			ValidateNonZeroNumericField(maskedTextBox.Text);
			errorProvider.SetError(maskedTextBox, string.Empty);
		}
		catch (Exception exc)
		{
			errorProvider.SetError(maskedTextBox, ErrorHelper.GetErrorDescription(exc));
		}
	}

	private void TxtMinDigits_Validating(object sender, CancelEventArgs e)
	{
		try
		{
			ValidateMinDigits();
			errorProvider.SetError(txtMinDigits, string.Empty);
			try
			{
				ValidateMaxDigits();
				errorProvider.SetError(txtMaxDigits, string.Empty);
			}
			catch (Exception exc)
			{
				errorProvider.SetError(txtMaxDigits, ErrorHelper.GetErrorDescription(exc));
			}
		}
		catch (Exception exc2)
		{
			errorProvider.SetError(txtMinDigits, ErrorHelper.GetErrorDescription(exc2));
		}
	}

	private void TxtMaxDigits_Validating(object sender, CancelEventArgs e)
	{
		try
		{
			ValidateMaxDigits();
			errorProvider.SetError(txtMaxDigits, string.Empty);
			try
			{
				ValidateMinDigits();
				errorProvider.SetError(txtMinDigits, string.Empty);
			}
			catch (Exception exc)
			{
				errorProvider.SetError(txtMinDigits, ErrorHelper.GetErrorDescription(exc));
			}
		}
		catch (Exception exc2)
		{
			errorProvider.SetError(txtMaxDigits, ErrorHelper.GetErrorDescription(exc2));
		}
	}

	public OptionsComponentsUserInputControl()
	{
		InitializeComponent();
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
		chkAcceptDtmfInput.Checked = Settings.Default.UserInputTemplateAcceptDtmfInput;
		txtMaxRetryCount.Text = Settings.Default.UserInputTemplateMaxRetryCount.ToString();
		txtFirstDigitTimeout.Text = Settings.Default.UserInputTemplateFirstDigitTimeout.ToString();
		txtInterDigitTimeout.Text = Settings.Default.UserInputTemplateInterDigitTimeout.ToString();
		txtFinalDigitTimeout.Text = Settings.Default.UserInputTemplateFinalDigitTimeout.ToString();
		txtMinDigits.Text = Settings.Default.UserInputTemplateMinDigits.ToString();
		txtMaxDigits.Text = Settings.Default.UserInputTemplateMaxDigits.ToString();
		comboStopDigit.SelectedItem = GetStopDigit(Settings.Default.UserInputTemplateStopDigit);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("OptionsComponentsUserInputControl.validDigitsListBox.DigitText") + " 0", Settings.Default.UserInputTemplateIsValidOption0);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("OptionsComponentsUserInputControl.validDigitsListBox.DigitText") + " 1", Settings.Default.UserInputTemplateIsValidOption1);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("OptionsComponentsUserInputControl.validDigitsListBox.DigitText") + " 2", Settings.Default.UserInputTemplateIsValidOption2);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("OptionsComponentsUserInputControl.validDigitsListBox.DigitText") + " 3", Settings.Default.UserInputTemplateIsValidOption3);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("OptionsComponentsUserInputControl.validDigitsListBox.DigitText") + " 4", Settings.Default.UserInputTemplateIsValidOption4);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("OptionsComponentsUserInputControl.validDigitsListBox.DigitText") + " 5", Settings.Default.UserInputTemplateIsValidOption5);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("OptionsComponentsUserInputControl.validDigitsListBox.DigitText") + " 6", Settings.Default.UserInputTemplateIsValidOption6);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("OptionsComponentsUserInputControl.validDigitsListBox.DigitText") + " 7", Settings.Default.UserInputTemplateIsValidOption7);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("OptionsComponentsUserInputControl.validDigitsListBox.DigitText") + " 8", Settings.Default.UserInputTemplateIsValidOption8);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("OptionsComponentsUserInputControl.validDigitsListBox.DigitText") + " 9", Settings.Default.UserInputTemplateIsValidOption9);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("OptionsComponentsUserInputControl.validDigitsListBox.DigitText") + " *", Settings.Default.UserInputTemplateIsValidOptionStar);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("OptionsComponentsUserInputControl.validDigitsListBox.DigitText") + " #", Settings.Default.UserInputTemplateIsValidOptionPound);
		lblUserInput.Text = LocalizedResourceMgr.GetString("OptionsComponentsUserInputControl.lblUserInput.Text");
		chkAcceptDtmfInput.Text = LocalizedResourceMgr.GetString("OptionsComponentsUserInputControl.chkAcceptDtmfInput.Text");
		lblMaxRetryCount.Text = LocalizedResourceMgr.GetString("OptionsComponentsUserInputControl.lblMaxRetryCount.Text");
		lblFirstDigitTimeout.Text = LocalizedResourceMgr.GetString("OptionsComponentsUserInputControl.lblFirstDigitTimeout.Text");
		lblInterDigitTimeout.Text = LocalizedResourceMgr.GetString("OptionsComponentsUserInputControl.lblInterDigitTimeout.Text");
		lblFinalDigitTimeout.Text = LocalizedResourceMgr.GetString("OptionsComponentsUserInputControl.lblFinalDigitTimeout.Text");
		lblMinDigits.Text = LocalizedResourceMgr.GetString("OptionsComponentsUserInputControl.lblMinDigits.Text");
		lblMaxDigits.Text = LocalizedResourceMgr.GetString("OptionsComponentsUserInputControl.lblMaxDigits.Text");
		lblStopDigit.Text = LocalizedResourceMgr.GetString("OptionsComponentsUserInputControl.lblStopDigit.Text");
		lblValidDigits.Text = LocalizedResourceMgr.GetString("OptionsComponentsUserInputControl.lblValidDigits.Text");
	}

	public void Save()
	{
		ValidateFields();
		Settings.Default.UserInputTemplateAcceptDtmfInput = chkAcceptDtmfInput.Checked;
		Settings.Default.UserInputTemplateMaxRetryCount = Convert.ToUInt32(txtMaxRetryCount.Text);
		Settings.Default.UserInputTemplateFirstDigitTimeout = Convert.ToUInt32(txtFirstDigitTimeout.Text);
		Settings.Default.UserInputTemplateInterDigitTimeout = Convert.ToUInt32(txtInterDigitTimeout.Text);
		Settings.Default.UserInputTemplateFinalDigitTimeout = Convert.ToUInt32(txtFinalDigitTimeout.Text);
		Settings.Default.UserInputTemplateMinDigits = Convert.ToUInt32(txtMinDigits.Text);
		Settings.Default.UserInputTemplateMaxDigits = Convert.ToUInt32(txtMaxDigits.Text);
		Settings.Default.UserInputTemplateStopDigit = GetStopDigitStr((DtmfDigits)comboStopDigit.SelectedItem);
		Settings.Default.UserInputTemplateIsValidOption0 = IsDigitValid(0);
		Settings.Default.UserInputTemplateIsValidOption1 = IsDigitValid(1);
		Settings.Default.UserInputTemplateIsValidOption2 = IsDigitValid(2);
		Settings.Default.UserInputTemplateIsValidOption3 = IsDigitValid(3);
		Settings.Default.UserInputTemplateIsValidOption4 = IsDigitValid(4);
		Settings.Default.UserInputTemplateIsValidOption5 = IsDigitValid(5);
		Settings.Default.UserInputTemplateIsValidOption6 = IsDigitValid(6);
		Settings.Default.UserInputTemplateIsValidOption7 = IsDigitValid(7);
		Settings.Default.UserInputTemplateIsValidOption8 = IsDigitValid(8);
		Settings.Default.UserInputTemplateIsValidOption9 = IsDigitValid(9);
		Settings.Default.UserInputTemplateIsValidOptionStar = IsDigitValid(10);
		Settings.Default.UserInputTemplateIsValidOptionPound = IsDigitValid(11);
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
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.validDigitsListBox = new System.Windows.Forms.CheckedListBox();
		this.comboStopDigit = new System.Windows.Forms.ComboBox();
		this.lblMinDigits = new System.Windows.Forms.Label();
		this.lblStopDigit = new System.Windows.Forms.Label();
		this.txtMinDigits = new System.Windows.Forms.MaskedTextBox();
		this.lblMaxDigits = new System.Windows.Forms.Label();
		this.txtMaxDigits = new System.Windows.Forms.MaskedTextBox();
		this.lblValidDigits = new System.Windows.Forms.Label();
		this.lblMaxRetryCount = new System.Windows.Forms.Label();
		this.txtMaxRetryCount = new System.Windows.Forms.MaskedTextBox();
		this.txtFinalDigitTimeout = new System.Windows.Forms.MaskedTextBox();
		this.txtInterDigitTimeout = new System.Windows.Forms.MaskedTextBox();
		this.lblFirstDigitTimeout = new System.Windows.Forms.Label();
		this.txtFirstDigitTimeout = new System.Windows.Forms.MaskedTextBox();
		this.chkAcceptDtmfInput = new System.Windows.Forms.CheckBox();
		this.lblInterDigitTimeout = new System.Windows.Forms.Label();
		this.lblFinalDigitTimeout = new System.Windows.Forms.Label();
		this.lblUserInput = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.validDigitsListBox.CheckOnClick = true;
		this.validDigitsListBox.FormattingEnabled = true;
		this.validDigitsListBox.Location = new System.Drawing.Point(12, 454);
		this.validDigitsListBox.Margin = new System.Windows.Forms.Padding(4);
		this.validDigitsListBox.MultiColumn = true;
		this.validDigitsListBox.Name = "validDigitsListBox";
		this.validDigitsListBox.Size = new System.Drawing.Size(764, 208);
		this.validDigitsListBox.TabIndex = 17;
		this.validDigitsListBox.LostFocus += new System.EventHandler(ValidDigitsListBox_LostFocus);
		this.comboStopDigit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboStopDigit.FormattingEnabled = true;
		this.comboStopDigit.Location = new System.Drawing.Point(12, 400);
		this.comboStopDigit.Margin = new System.Windows.Forms.Padding(4);
		this.comboStopDigit.Name = "comboStopDigit";
		this.comboStopDigit.Size = new System.Drawing.Size(300, 24);
		this.comboStopDigit.TabIndex = 15;
		this.lblMinDigits.AutoSize = true;
		this.lblMinDigits.Location = new System.Drawing.Point(9, 275);
		this.lblMinDigits.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblMinDigits.Name = "lblMinDigits";
		this.lblMinDigits.Size = new System.Drawing.Size(69, 17);
		this.lblMinDigits.TabIndex = 10;
		this.lblMinDigits.Text = "Min Digits";
		this.lblStopDigit.AutoSize = true;
		this.lblStopDigit.Location = new System.Drawing.Point(9, 379);
		this.lblStopDigit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblStopDigit.Name = "lblStopDigit";
		this.lblStopDigit.Size = new System.Drawing.Size(69, 17);
		this.lblStopDigit.TabIndex = 14;
		this.lblStopDigit.Text = "Stop Digit";
		this.txtMinDigits.HidePromptOnLeave = true;
		this.txtMinDigits.Location = new System.Drawing.Point(12, 296);
		this.txtMinDigits.Margin = new System.Windows.Forms.Padding(4);
		this.txtMinDigits.Mask = "999";
		this.txtMinDigits.Name = "txtMinDigits";
		this.txtMinDigits.Size = new System.Drawing.Size(300, 22);
		this.txtMinDigits.TabIndex = 11;
		this.txtMinDigits.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtMinDigits.Validating += new System.ComponentModel.CancelEventHandler(TxtMinDigits_Validating);
		this.lblMaxDigits.AutoSize = true;
		this.lblMaxDigits.Location = new System.Drawing.Point(9, 327);
		this.lblMaxDigits.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblMaxDigits.Name = "lblMaxDigits";
		this.lblMaxDigits.Size = new System.Drawing.Size(72, 17);
		this.lblMaxDigits.TabIndex = 12;
		this.lblMaxDigits.Text = "Max Digits";
		this.txtMaxDigits.HidePromptOnLeave = true;
		this.txtMaxDigits.Location = new System.Drawing.Point(12, 348);
		this.txtMaxDigits.Margin = new System.Windows.Forms.Padding(4);
		this.txtMaxDigits.Mask = "999";
		this.txtMaxDigits.Name = "txtMaxDigits";
		this.txtMaxDigits.Size = new System.Drawing.Size(300, 22);
		this.txtMaxDigits.TabIndex = 13;
		this.txtMaxDigits.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtMaxDigits.Validating += new System.ComponentModel.CancelEventHandler(TxtMaxDigits_Validating);
		this.lblValidDigits.AutoSize = true;
		this.lblValidDigits.Location = new System.Drawing.Point(9, 433);
		this.lblValidDigits.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblValidDigits.Name = "lblValidDigits";
		this.lblValidDigits.Size = new System.Drawing.Size(78, 17);
		this.lblValidDigits.TabIndex = 16;
		this.lblValidDigits.Text = "Valid Digits";
		this.lblMaxRetryCount.AutoSize = true;
		this.lblMaxRetryCount.Location = new System.Drawing.Point(9, 68);
		this.lblMaxRetryCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblMaxRetryCount.Name = "lblMaxRetryCount";
		this.lblMaxRetryCount.Size = new System.Drawing.Size(112, 17);
		this.lblMaxRetryCount.TabIndex = 2;
		this.lblMaxRetryCount.Text = "Max Retry Count";
		this.txtMaxRetryCount.HidePromptOnLeave = true;
		this.txtMaxRetryCount.Location = new System.Drawing.Point(12, 89);
		this.txtMaxRetryCount.Margin = new System.Windows.Forms.Padding(4);
		this.txtMaxRetryCount.Mask = "99";
		this.txtMaxRetryCount.Name = "txtMaxRetryCount";
		this.txtMaxRetryCount.Size = new System.Drawing.Size(300, 22);
		this.txtMaxRetryCount.TabIndex = 3;
		this.txtMaxRetryCount.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtMaxRetryCount.Validating += new System.ComponentModel.CancelEventHandler(TxtNumeric_Validating);
		this.txtFinalDigitTimeout.HidePromptOnLeave = true;
		this.txtFinalDigitTimeout.Location = new System.Drawing.Point(12, 244);
		this.txtFinalDigitTimeout.Margin = new System.Windows.Forms.Padding(4);
		this.txtFinalDigitTimeout.Mask = "99";
		this.txtFinalDigitTimeout.Name = "txtFinalDigitTimeout";
		this.txtFinalDigitTimeout.Size = new System.Drawing.Size(300, 22);
		this.txtFinalDigitTimeout.TabIndex = 9;
		this.txtFinalDigitTimeout.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtFinalDigitTimeout.Validating += new System.ComponentModel.CancelEventHandler(TxtNonZeroNumeric_Validating);
		this.txtInterDigitTimeout.HidePromptOnLeave = true;
		this.txtInterDigitTimeout.Location = new System.Drawing.Point(12, 192);
		this.txtInterDigitTimeout.Margin = new System.Windows.Forms.Padding(4);
		this.txtInterDigitTimeout.Mask = "99";
		this.txtInterDigitTimeout.Name = "txtInterDigitTimeout";
		this.txtInterDigitTimeout.Size = new System.Drawing.Size(300, 22);
		this.txtInterDigitTimeout.TabIndex = 7;
		this.txtInterDigitTimeout.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtInterDigitTimeout.Validating += new System.ComponentModel.CancelEventHandler(TxtNonZeroNumeric_Validating);
		this.lblFirstDigitTimeout.AutoSize = true;
		this.lblFirstDigitTimeout.Location = new System.Drawing.Point(9, 119);
		this.lblFirstDigitTimeout.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblFirstDigitTimeout.Name = "lblFirstDigitTimeout";
		this.lblFirstDigitTimeout.Size = new System.Drawing.Size(165, 17);
		this.lblFirstDigitTimeout.TabIndex = 4;
		this.lblFirstDigitTimeout.Text = "First Digit Timeout (secs)";
		this.txtFirstDigitTimeout.HidePromptOnLeave = true;
		this.txtFirstDigitTimeout.Location = new System.Drawing.Point(12, 140);
		this.txtFirstDigitTimeout.Margin = new System.Windows.Forms.Padding(4);
		this.txtFirstDigitTimeout.Mask = "99";
		this.txtFirstDigitTimeout.Name = "txtFirstDigitTimeout";
		this.txtFirstDigitTimeout.Size = new System.Drawing.Size(300, 22);
		this.txtFirstDigitTimeout.TabIndex = 5;
		this.txtFirstDigitTimeout.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtFirstDigitTimeout.Validating += new System.ComponentModel.CancelEventHandler(TxtNonZeroNumeric_Validating);
		this.chkAcceptDtmfInput.AutoSize = true;
		this.chkAcceptDtmfInput.Location = new System.Drawing.Point(12, 38);
		this.chkAcceptDtmfInput.Margin = new System.Windows.Forms.Padding(4);
		this.chkAcceptDtmfInput.Name = "chkAcceptDtmfInput";
		this.chkAcceptDtmfInput.Size = new System.Drawing.Size(139, 21);
		this.chkAcceptDtmfInput.TabIndex = 1;
		this.chkAcceptDtmfInput.Text = "Accept DTMF Input During Prompt";
		this.chkAcceptDtmfInput.UseVisualStyleBackColor = true;
		this.lblInterDigitTimeout.AutoSize = true;
		this.lblInterDigitTimeout.Location = new System.Drawing.Point(9, 171);
		this.lblInterDigitTimeout.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblInterDigitTimeout.Name = "lblInterDigitTimeout";
		this.lblInterDigitTimeout.Size = new System.Drawing.Size(166, 17);
		this.lblInterDigitTimeout.TabIndex = 6;
		this.lblInterDigitTimeout.Text = "Inter Digit Timeout (secs)";
		this.lblFinalDigitTimeout.AutoSize = true;
		this.lblFinalDigitTimeout.Location = new System.Drawing.Point(9, 223);
		this.lblFinalDigitTimeout.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblFinalDigitTimeout.Name = "lblFinalDigitTimeout";
		this.lblFinalDigitTimeout.Size = new System.Drawing.Size(168, 17);
		this.lblFinalDigitTimeout.TabIndex = 8;
		this.lblFinalDigitTimeout.Text = "Final Digit Timeout (secs)";
		this.lblUserInput.AutoSize = true;
		this.lblUserInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblUserInput.Location = new System.Drawing.Point(8, 8);
		this.lblUserInput.Name = "lblUserInput";
		this.lblUserInput.Size = new System.Drawing.Size(96, 20);
		this.lblUserInput.TabIndex = 0;
		this.lblUserInput.Text = "User Input";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.lblUserInput);
		base.Controls.Add(this.validDigitsListBox);
		base.Controls.Add(this.comboStopDigit);
		base.Controls.Add(this.lblMinDigits);
		base.Controls.Add(this.lblStopDigit);
		base.Controls.Add(this.txtMinDigits);
		base.Controls.Add(this.lblMaxDigits);
		base.Controls.Add(this.txtMaxDigits);
		base.Controls.Add(this.lblValidDigits);
		base.Controls.Add(this.lblMaxRetryCount);
		base.Controls.Add(this.txtMaxRetryCount);
		base.Controls.Add(this.txtFinalDigitTimeout);
		base.Controls.Add(this.txtInterDigitTimeout);
		base.Controls.Add(this.lblFirstDigitTimeout);
		base.Controls.Add(this.txtFirstDigitTimeout);
		base.Controls.Add(this.chkAcceptDtmfInput);
		base.Controls.Add(this.lblInterDigitTimeout);
		base.Controls.Add(this.lblFinalDigitTimeout);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "OptionsComponentsUserInputControl";
		base.Size = new System.Drawing.Size(780, 665);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
