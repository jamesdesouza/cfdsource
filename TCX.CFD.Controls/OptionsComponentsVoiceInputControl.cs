using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class OptionsComponentsVoiceInputControl : UserControl, IOptionsControl
{
	private IContainer components;

	private Label lblMaxRetryCount;

	private MaskedTextBox txtMaxRetryCount;

	private Label lblInputTimeout;

	private MaskedTextBox txtInputTimeout;

	private ErrorProvider errorProvider;

	private Label lblVoiceInput;

	private Label lblLanguageCode;

	private ComboBox comboLanguageCode;

	private CheckBox chkAcceptDtmfInput;

	private GroupBox grpBoxAcceptDtmf;

	private CheckedListBox validDigitsListBox;

	private ComboBox comboStopDigit;

	private Label lblMinDigits;

	private Label lblStopDigit;

	private MaskedTextBox txtMinDigits;

	private Label lblMaxDigits;

	private MaskedTextBox txtMaxDigits;

	private Label lblValidDigits;

	private MaskedTextBox txtDtmfTimeout;

	private Label lblDtmfTimeout;

	private void FillComboLanguageCode()
	{
		comboLanguageCode.Items.Clear();
		comboLanguageCode.Items.Add("af-ZA");
		comboLanguageCode.Items.Add("am-ET");
		comboLanguageCode.Items.Add("ar-AE");
		comboLanguageCode.Items.Add("ar-BH");
		comboLanguageCode.Items.Add("ar-DZ");
		comboLanguageCode.Items.Add("ar-EG");
		comboLanguageCode.Items.Add("ar-IL");
		comboLanguageCode.Items.Add("ar-IQ");
		comboLanguageCode.Items.Add("ar-JO");
		comboLanguageCode.Items.Add("ar-KW");
		comboLanguageCode.Items.Add("ar-LB");
		comboLanguageCode.Items.Add("ar-MA");
		comboLanguageCode.Items.Add("ar-OM");
		comboLanguageCode.Items.Add("ar-PS");
		comboLanguageCode.Items.Add("ar-QA");
		comboLanguageCode.Items.Add("ar-SA");
		comboLanguageCode.Items.Add("ar-TN");
		comboLanguageCode.Items.Add("az-AZ");
		comboLanguageCode.Items.Add("bg-BG");
		comboLanguageCode.Items.Add("bn-BD");
		comboLanguageCode.Items.Add("bn-IN");
		comboLanguageCode.Items.Add("ca-ES");
		comboLanguageCode.Items.Add("cs-CZ");
		comboLanguageCode.Items.Add("da-DK");
		comboLanguageCode.Items.Add("de-DE");
		comboLanguageCode.Items.Add("el-GR");
		comboLanguageCode.Items.Add("en-AU");
		comboLanguageCode.Items.Add("en-CA");
		comboLanguageCode.Items.Add("en-GB");
		comboLanguageCode.Items.Add("en-GH");
		comboLanguageCode.Items.Add("en-IE");
		comboLanguageCode.Items.Add("en-IN");
		comboLanguageCode.Items.Add("en-KE");
		comboLanguageCode.Items.Add("en-NG");
		comboLanguageCode.Items.Add("en-NZ");
		comboLanguageCode.Items.Add("en-PH");
		comboLanguageCode.Items.Add("en-SG");
		comboLanguageCode.Items.Add("en-TZ");
		comboLanguageCode.Items.Add("en-US");
		comboLanguageCode.Items.Add("en-ZA");
		comboLanguageCode.Items.Add("es-AR");
		comboLanguageCode.Items.Add("es-BO");
		comboLanguageCode.Items.Add("es-CL");
		comboLanguageCode.Items.Add("es-CO");
		comboLanguageCode.Items.Add("es-CR");
		comboLanguageCode.Items.Add("es-DO");
		comboLanguageCode.Items.Add("es-EC");
		comboLanguageCode.Items.Add("es-ES");
		comboLanguageCode.Items.Add("es-GT");
		comboLanguageCode.Items.Add("es-HN");
		comboLanguageCode.Items.Add("es-MX");
		comboLanguageCode.Items.Add("es-NI");
		comboLanguageCode.Items.Add("es-PA");
		comboLanguageCode.Items.Add("es-PE");
		comboLanguageCode.Items.Add("es-PR");
		comboLanguageCode.Items.Add("es-PY");
		comboLanguageCode.Items.Add("es-SV");
		comboLanguageCode.Items.Add("es-US");
		comboLanguageCode.Items.Add("es-UY");
		comboLanguageCode.Items.Add("es-VE");
		comboLanguageCode.Items.Add("eu-ES");
		comboLanguageCode.Items.Add("fa-IR");
		comboLanguageCode.Items.Add("fi-FI");
		comboLanguageCode.Items.Add("fil-PH");
		comboLanguageCode.Items.Add("fr-CA");
		comboLanguageCode.Items.Add("fr-FR");
		comboLanguageCode.Items.Add("gl-ES");
		comboLanguageCode.Items.Add("gu-IN");
		comboLanguageCode.Items.Add("he-IL");
		comboLanguageCode.Items.Add("hi-IN");
		comboLanguageCode.Items.Add("hr-HR");
		comboLanguageCode.Items.Add("hu-HU");
		comboLanguageCode.Items.Add("hy-AM");
		comboLanguageCode.Items.Add("id-ID");
		comboLanguageCode.Items.Add("is-IS");
		comboLanguageCode.Items.Add("it-IT");
		comboLanguageCode.Items.Add("ja-JP");
		comboLanguageCode.Items.Add("jv-ID");
		comboLanguageCode.Items.Add("ka-GE");
		comboLanguageCode.Items.Add("km-KH");
		comboLanguageCode.Items.Add("kn-IN");
		comboLanguageCode.Items.Add("ko-KR");
		comboLanguageCode.Items.Add("lo-LA");
		comboLanguageCode.Items.Add("lt-LT");
		comboLanguageCode.Items.Add("lv-LV");
		comboLanguageCode.Items.Add("ml-IN");
		comboLanguageCode.Items.Add("mr-IN");
		comboLanguageCode.Items.Add("ms-MY");
		comboLanguageCode.Items.Add("nb-NO");
		comboLanguageCode.Items.Add("ne-NP");
		comboLanguageCode.Items.Add("nl-NL");
		comboLanguageCode.Items.Add("pl-PL");
		comboLanguageCode.Items.Add("pt-BR");
		comboLanguageCode.Items.Add("pt-PT");
		comboLanguageCode.Items.Add("ro-RO");
		comboLanguageCode.Items.Add("ru-RU");
		comboLanguageCode.Items.Add("si-LK");
		comboLanguageCode.Items.Add("sk-SK");
		comboLanguageCode.Items.Add("sl-SI");
		comboLanguageCode.Items.Add("sr-RS");
		comboLanguageCode.Items.Add("su-ID");
		comboLanguageCode.Items.Add("sv-SE");
		comboLanguageCode.Items.Add("sw-KE");
		comboLanguageCode.Items.Add("sw-TZ");
		comboLanguageCode.Items.Add("ta-IN");
		comboLanguageCode.Items.Add("ta-LK");
		comboLanguageCode.Items.Add("ta-MY");
		comboLanguageCode.Items.Add("ta-SG");
		comboLanguageCode.Items.Add("te-IN");
		comboLanguageCode.Items.Add("th-TH");
		comboLanguageCode.Items.Add("tr-TR");
		comboLanguageCode.Items.Add("uk-UA");
		comboLanguageCode.Items.Add("ur-IN");
		comboLanguageCode.Items.Add("ur-PK");
		comboLanguageCode.Items.Add("vi-VN");
		comboLanguageCode.Items.Add("yue-Hant-HK");
		comboLanguageCode.Items.Add("zh");
		comboLanguageCode.Items.Add("zh-HK");
		comboLanguageCode.Items.Add("zh-TW");
		comboLanguageCode.Items.Add("zu-ZA");
		if (!string.IsNullOrEmpty(Settings.Default.VoiceInputTemplateLanguageCode) && !comboLanguageCode.Items.Contains(Settings.Default.VoiceInputTemplateLanguageCode))
		{
			comboLanguageCode.Items.Add(Settings.Default.VoiceInputTemplateLanguageCode);
		}
	}

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

	private void ValidateTimeout()
	{
		if (string.IsNullOrEmpty(txtInputTimeout.Text))
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsVoiceInputControl.Error.InputTimeoutIsMandatory"));
		}
		if (Convert.ToUInt32(txtInputTimeout.Text) == 0)
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsVoiceInputControl.Error.InputTimeoutInvalidRange"));
		}
	}

	private void ValidateMaxRetryCount()
	{
		if (string.IsNullOrEmpty(txtMaxRetryCount.Text))
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsVoiceInputControl.Error.MaxRetryCountIsMandatory"));
		}
		if (Convert.ToUInt32(txtMaxRetryCount.Text) == 0)
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsVoiceInputControl.Error.MaxRetryCountInvalidRange"));
		}
	}

	private void ValidateNumericField(string fieldValue)
	{
		if (string.IsNullOrEmpty(fieldValue))
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsVoiceInputControl.Error.FieldIsMandatory"));
		}
	}

	private void ValidateNonZeroNumericField(string fieldValue)
	{
		ValidateNumericField(fieldValue);
		if (Convert.ToUInt32(fieldValue) == 0)
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsVoiceInputControl.Error.FieldCanNotBeZero"));
		}
	}

	private void ValidateMinDigits()
	{
		if (string.IsNullOrEmpty(txtMinDigits.Text))
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsVoiceInputControl.Error.MinDigitsIsMandatory"));
		}
		int num = Convert.ToInt32(txtMinDigits.Text);
		if (num < 1 || num > 999)
		{
			throw new ApplicationException(string.Format(LocalizedResourceMgr.GetString("OptionsComponentsVoiceInputControl.Error.InvalidMinDigitsValue"), 1, 999));
		}
		if (!string.IsNullOrEmpty(txtMaxDigits.Text))
		{
			int num2 = Convert.ToInt32(txtMaxDigits.Text);
			if (num > num2)
			{
				throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsVoiceInputControl.Error.MinDigitsCantBeGreaterThanMaxDigits"));
			}
		}
	}

	private void ValidateMaxDigits()
	{
		if (string.IsNullOrEmpty(txtMaxDigits.Text))
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsVoiceInputControl.Error.MaxDigitsIsMandatory"));
		}
		int num = Convert.ToInt32(txtMaxDigits.Text);
		if (num < 1 || num > 999)
		{
			throw new ApplicationException(string.Format(LocalizedResourceMgr.GetString("OptionsComponentsVoiceInputControl.Error.InvalidMaxDigitsValue"), 1, 999));
		}
		if (!string.IsNullOrEmpty(txtMinDigits.Text) && Convert.ToInt32(txtMinDigits.Text) > num)
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsVoiceInputControl.Error.MinDigitsCantBeGreaterThanMaxDigits"));
		}
	}

	private void ValidateStopDigitIsNotValidDigit()
	{
		string stopDigitStr = GetStopDigitStr((DtmfDigits)comboStopDigit.SelectedItem);
		int num = (string.IsNullOrEmpty(stopDigitStr) ? (-1) : Convert.ToInt32(stopDigitStr.Replace("*", "10").Replace("#", "11")));
		if (num >= 0 && IsDigitValid(num))
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsVoiceInputControl.Error.StopDigitIsValidDigit"));
		}
	}

	private void ValidateFields()
	{
		ValidateTimeout();
		ValidateMaxRetryCount();
		if (chkAcceptDtmfInput.Checked)
		{
			ValidateNonZeroNumericField(txtDtmfTimeout.Text);
			ValidateMinDigits();
			ValidateMaxDigits();
			ValidateStopDigitIsNotValidDigit();
		}
	}

	private void ChkAcceptDtmfInput_CheckedChanged(object sender, EventArgs e)
	{
		grpBoxAcceptDtmf.Enabled = chkAcceptDtmfInput.Checked;
	}

	private void TxtTimeout_Validating(object sender, CancelEventArgs e)
	{
		try
		{
			ValidateTimeout();
			errorProvider.SetError(txtInputTimeout, string.Empty);
		}
		catch (Exception exc)
		{
			errorProvider.SetError(txtInputTimeout, ErrorHelper.GetErrorDescription(exc));
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

	public OptionsComponentsVoiceInputControl()
	{
		InitializeComponent();
		FillComboLanguageCode();
		txtInputTimeout.Text = Settings.Default.VoiceInputTemplateInputTimeout.ToString();
		txtMaxRetryCount.Text = Settings.Default.VoiceInputTemplateMaxRetryCount.ToString();
		comboLanguageCode.SelectedItem = Settings.Default.VoiceInputTemplateLanguageCode;
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
		chkAcceptDtmfInput.Checked = Settings.Default.VoiceInputTemplateAcceptDtmfInput;
		txtDtmfTimeout.Text = Settings.Default.VoiceInputTemplateDtmfTimeout.ToString();
		txtMinDigits.Text = Settings.Default.VoiceInputTemplateMinDigits.ToString();
		txtMaxDigits.Text = Settings.Default.VoiceInputTemplateMaxDigits.ToString();
		comboStopDigit.SelectedItem = GetStopDigit(Settings.Default.VoiceInputTemplateStopDigit);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("OptionsComponentsVoiceInputControl.validDigitsListBox.DigitText") + " 0", Settings.Default.VoiceInputTemplateIsValidOption0);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("OptionsComponentsVoiceInputControl.validDigitsListBox.DigitText") + " 1", Settings.Default.VoiceInputTemplateIsValidOption1);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("OptionsComponentsVoiceInputControl.validDigitsListBox.DigitText") + " 2", Settings.Default.VoiceInputTemplateIsValidOption2);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("OptionsComponentsVoiceInputControl.validDigitsListBox.DigitText") + " 3", Settings.Default.VoiceInputTemplateIsValidOption3);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("OptionsComponentsVoiceInputControl.validDigitsListBox.DigitText") + " 4", Settings.Default.VoiceInputTemplateIsValidOption4);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("OptionsComponentsVoiceInputControl.validDigitsListBox.DigitText") + " 5", Settings.Default.VoiceInputTemplateIsValidOption5);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("OptionsComponentsVoiceInputControl.validDigitsListBox.DigitText") + " 6", Settings.Default.VoiceInputTemplateIsValidOption6);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("OptionsComponentsVoiceInputControl.validDigitsListBox.DigitText") + " 7", Settings.Default.VoiceInputTemplateIsValidOption7);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("OptionsComponentsVoiceInputControl.validDigitsListBox.DigitText") + " 8", Settings.Default.VoiceInputTemplateIsValidOption8);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("OptionsComponentsVoiceInputControl.validDigitsListBox.DigitText") + " 9", Settings.Default.VoiceInputTemplateIsValidOption9);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("OptionsComponentsVoiceInputControl.validDigitsListBox.DigitText") + " *", Settings.Default.VoiceInputTemplateIsValidOptionStar);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("OptionsComponentsVoiceInputControl.validDigitsListBox.DigitText") + " #", Settings.Default.VoiceInputTemplateIsValidOptionPound);
		ChkAcceptDtmfInput_CheckedChanged(chkAcceptDtmfInput, EventArgs.Empty);
		lblVoiceInput.Text = LocalizedResourceMgr.GetString("OptionsComponentsVoiceInputControl.lblVoiceInput.Text");
		lblInputTimeout.Text = LocalizedResourceMgr.GetString("OptionsComponentsVoiceInputControl.lblInputTimeout.Text");
		lblMaxRetryCount.Text = LocalizedResourceMgr.GetString("OptionsComponentsVoiceInputControl.lblMaxRetryCount.Text");
		lblLanguageCode.Text = LocalizedResourceMgr.GetString("OptionsComponentsVoiceInputControl.lblLanguageCode.Text");
		chkAcceptDtmfInput.Text = LocalizedResourceMgr.GetString("OptionsComponentsVoiceInputControl.chkAcceptDtmfInput.Text");
		lblDtmfTimeout.Text = LocalizedResourceMgr.GetString("OptionsComponentsVoiceInputControl.lblDtmfTimeout.Text");
		lblMinDigits.Text = LocalizedResourceMgr.GetString("OptionsComponentsVoiceInputControl.lblMinDigits.Text");
		lblMaxDigits.Text = LocalizedResourceMgr.GetString("OptionsComponentsVoiceInputControl.lblMaxDigits.Text");
		lblStopDigit.Text = LocalizedResourceMgr.GetString("OptionsComponentsVoiceInputControl.lblStopDigit.Text");
		lblValidDigits.Text = LocalizedResourceMgr.GetString("OptionsComponentsVoiceInputControl.lblValidDigits.Text");
	}

	public void Save()
	{
		ValidateFields();
		Settings.Default.VoiceInputTemplateInputTimeout = Convert.ToUInt32(txtInputTimeout.Text);
		Settings.Default.VoiceInputTemplateMaxRetryCount = Convert.ToUInt32(txtMaxRetryCount.Text);
		Settings.Default.VoiceInputTemplateLanguageCode = comboLanguageCode.Text;
		Settings.Default.VoiceInputTemplateAcceptDtmfInput = chkAcceptDtmfInput.Checked;
		Settings.Default.VoiceInputTemplateDtmfTimeout = Convert.ToUInt32(txtDtmfTimeout.Text);
		Settings.Default.VoiceInputTemplateMinDigits = Convert.ToUInt32(txtMinDigits.Text);
		Settings.Default.VoiceInputTemplateMaxDigits = Convert.ToUInt32(txtMaxDigits.Text);
		Settings.Default.VoiceInputTemplateStopDigit = GetStopDigitStr((DtmfDigits)comboStopDigit.SelectedItem);
		Settings.Default.VoiceInputTemplateIsValidOption0 = IsDigitValid(0);
		Settings.Default.VoiceInputTemplateIsValidOption1 = IsDigitValid(1);
		Settings.Default.VoiceInputTemplateIsValidOption2 = IsDigitValid(2);
		Settings.Default.VoiceInputTemplateIsValidOption3 = IsDigitValid(3);
		Settings.Default.VoiceInputTemplateIsValidOption4 = IsDigitValid(4);
		Settings.Default.VoiceInputTemplateIsValidOption5 = IsDigitValid(5);
		Settings.Default.VoiceInputTemplateIsValidOption6 = IsDigitValid(6);
		Settings.Default.VoiceInputTemplateIsValidOption7 = IsDigitValid(7);
		Settings.Default.VoiceInputTemplateIsValidOption8 = IsDigitValid(8);
		Settings.Default.VoiceInputTemplateIsValidOption9 = IsDigitValid(9);
		Settings.Default.VoiceInputTemplateIsValidOptionStar = IsDigitValid(10);
		Settings.Default.VoiceInputTemplateIsValidOptionPound = IsDigitValid(11);
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
		this.lblMaxRetryCount = new System.Windows.Forms.Label();
		this.txtMaxRetryCount = new System.Windows.Forms.MaskedTextBox();
		this.lblInputTimeout = new System.Windows.Forms.Label();
		this.txtInputTimeout = new System.Windows.Forms.MaskedTextBox();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.lblVoiceInput = new System.Windows.Forms.Label();
		this.lblLanguageCode = new System.Windows.Forms.Label();
		this.comboLanguageCode = new System.Windows.Forms.ComboBox();
		this.chkAcceptDtmfInput = new System.Windows.Forms.CheckBox();
		this.grpBoxAcceptDtmf = new System.Windows.Forms.GroupBox();
		this.validDigitsListBox = new System.Windows.Forms.CheckedListBox();
		this.comboStopDigit = new System.Windows.Forms.ComboBox();
		this.lblMinDigits = new System.Windows.Forms.Label();
		this.lblStopDigit = new System.Windows.Forms.Label();
		this.txtMinDigits = new System.Windows.Forms.MaskedTextBox();
		this.lblMaxDigits = new System.Windows.Forms.Label();
		this.txtMaxDigits = new System.Windows.Forms.MaskedTextBox();
		this.lblValidDigits = new System.Windows.Forms.Label();
		this.txtDtmfTimeout = new System.Windows.Forms.MaskedTextBox();
		this.lblDtmfTimeout = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		this.grpBoxAcceptDtmf.SuspendLayout();
		base.SuspendLayout();
		this.lblMaxRetryCount.AutoSize = true;
		this.lblMaxRetryCount.Location = new System.Drawing.Point(9, 85);
		this.lblMaxRetryCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblMaxRetryCount.Name = "lblMaxRetryCount";
		this.lblMaxRetryCount.Size = new System.Drawing.Size(112, 17);
		this.lblMaxRetryCount.TabIndex = 3;
		this.lblMaxRetryCount.Text = "Max Retry Count";
		this.txtMaxRetryCount.HidePromptOnLeave = true;
		this.txtMaxRetryCount.Location = new System.Drawing.Point(12, 106);
		this.txtMaxRetryCount.Margin = new System.Windows.Forms.Padding(4);
		this.txtMaxRetryCount.Mask = "99";
		this.txtMaxRetryCount.Name = "txtMaxRetryCount";
		this.txtMaxRetryCount.Size = new System.Drawing.Size(300, 22);
		this.txtMaxRetryCount.TabIndex = 4;
		this.txtMaxRetryCount.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtMaxRetryCount.Validating += new System.ComponentModel.CancelEventHandler(TxtMaxRetryCount_Validating);
		this.lblInputTimeout.AutoSize = true;
		this.lblInputTimeout.Location = new System.Drawing.Point(9, 38);
		this.lblInputTimeout.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblInputTimeout.Name = "lblInputTimeout";
		this.lblInputTimeout.Size = new System.Drawing.Size(137, 17);
		this.lblInputTimeout.TabIndex = 1;
		this.lblInputTimeout.Text = "Input Timeout (secs)";
		this.txtInputTimeout.HidePromptOnLeave = true;
		this.txtInputTimeout.Location = new System.Drawing.Point(12, 59);
		this.txtInputTimeout.Margin = new System.Windows.Forms.Padding(4);
		this.txtInputTimeout.Mask = "99";
		this.txtInputTimeout.Name = "txtInputTimeout";
		this.txtInputTimeout.Size = new System.Drawing.Size(300, 22);
		this.txtInputTimeout.TabIndex = 2;
		this.txtInputTimeout.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtInputTimeout.Validating += new System.ComponentModel.CancelEventHandler(TxtTimeout_Validating);
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.lblVoiceInput.AutoSize = true;
		this.lblVoiceInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblVoiceInput.Location = new System.Drawing.Point(8, 8);
		this.lblVoiceInput.Name = "lblVoiceInput";
		this.lblVoiceInput.Size = new System.Drawing.Size(103, 20);
		this.lblVoiceInput.TabIndex = 0;
		this.lblVoiceInput.Text = "Voice Input";
		this.lblLanguageCode.AutoSize = true;
		this.lblLanguageCode.Location = new System.Drawing.Point(9, 132);
		this.lblLanguageCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblLanguageCode.Name = "lblLanguageCode";
		this.lblLanguageCode.Size = new System.Drawing.Size(109, 17);
		this.lblLanguageCode.TabIndex = 5;
		this.lblLanguageCode.Text = "Language Code";
		this.comboLanguageCode.Location = new System.Drawing.Point(12, 153);
		this.comboLanguageCode.Margin = new System.Windows.Forms.Padding(4);
		this.comboLanguageCode.MaxLength = 1024;
		this.comboLanguageCode.Name = "comboLanguageCode";
		this.comboLanguageCode.Size = new System.Drawing.Size(300, 24);
		this.comboLanguageCode.TabIndex = 6;
		this.chkAcceptDtmfInput.AutoSize = true;
		this.chkAcceptDtmfInput.Location = new System.Drawing.Point(23, 185);
		this.chkAcceptDtmfInput.Margin = new System.Windows.Forms.Padding(4);
		this.chkAcceptDtmfInput.Name = "chkAcceptDtmfInput";
		this.chkAcceptDtmfInput.Size = new System.Drawing.Size(150, 21);
		this.chkAcceptDtmfInput.TabIndex = 7;
		this.chkAcceptDtmfInput.Text = "Accept DTMF Input";
		this.chkAcceptDtmfInput.UseVisualStyleBackColor = true;
		this.chkAcceptDtmfInput.CheckedChanged += new System.EventHandler(ChkAcceptDtmfInput_CheckedChanged);
		this.grpBoxAcceptDtmf.Controls.Add(this.validDigitsListBox);
		this.grpBoxAcceptDtmf.Controls.Add(this.comboStopDigit);
		this.grpBoxAcceptDtmf.Controls.Add(this.lblMinDigits);
		this.grpBoxAcceptDtmf.Controls.Add(this.lblStopDigit);
		this.grpBoxAcceptDtmf.Controls.Add(this.txtMinDigits);
		this.grpBoxAcceptDtmf.Controls.Add(this.lblMaxDigits);
		this.grpBoxAcceptDtmf.Controls.Add(this.txtMaxDigits);
		this.grpBoxAcceptDtmf.Controls.Add(this.lblValidDigits);
		this.grpBoxAcceptDtmf.Controls.Add(this.txtDtmfTimeout);
		this.grpBoxAcceptDtmf.Controls.Add(this.lblDtmfTimeout);
		this.grpBoxAcceptDtmf.Location = new System.Drawing.Point(12, 188);
		this.grpBoxAcceptDtmf.Name = "grpBoxAcceptDtmf";
		this.grpBoxAcceptDtmf.Size = new System.Drawing.Size(709, 364);
		this.grpBoxAcceptDtmf.TabIndex = 8;
		this.grpBoxAcceptDtmf.TabStop = false;
		this.validDigitsListBox.CheckOnClick = true;
		this.validDigitsListBox.FormattingEnabled = true;
		this.validDigitsListBox.Location = new System.Drawing.Point(10, 260);
		this.validDigitsListBox.Margin = new System.Windows.Forms.Padding(4);
		this.validDigitsListBox.MultiColumn = true;
		this.validDigitsListBox.Name = "validDigitsListBox";
		this.validDigitsListBox.Size = new System.Drawing.Size(692, 89);
		this.validDigitsListBox.TabIndex = 9;
		this.validDigitsListBox.LostFocus += new System.EventHandler(ValidDigitsListBox_LostFocus);
		this.comboStopDigit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboStopDigit.FormattingEnabled = true;
		this.comboStopDigit.Location = new System.Drawing.Point(10, 206);
		this.comboStopDigit.Margin = new System.Windows.Forms.Padding(4);
		this.comboStopDigit.Name = "comboStopDigit";
		this.comboStopDigit.Size = new System.Drawing.Size(300, 24);
		this.comboStopDigit.TabIndex = 7;
		this.lblMinDigits.AutoSize = true;
		this.lblMinDigits.Location = new System.Drawing.Point(7, 81);
		this.lblMinDigits.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblMinDigits.Name = "lblMinDigits";
		this.lblMinDigits.Size = new System.Drawing.Size(69, 17);
		this.lblMinDigits.TabIndex = 2;
		this.lblMinDigits.Text = "Min Digits";
		this.lblStopDigit.AutoSize = true;
		this.lblStopDigit.Location = new System.Drawing.Point(7, 185);
		this.lblStopDigit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblStopDigit.Name = "lblStopDigit";
		this.lblStopDigit.Size = new System.Drawing.Size(69, 17);
		this.lblStopDigit.TabIndex = 6;
		this.lblStopDigit.Text = "Stop Digit";
		this.txtMinDigits.HidePromptOnLeave = true;
		this.txtMinDigits.Location = new System.Drawing.Point(10, 102);
		this.txtMinDigits.Margin = new System.Windows.Forms.Padding(4);
		this.txtMinDigits.Mask = "999";
		this.txtMinDigits.Name = "txtMinDigits";
		this.txtMinDigits.Size = new System.Drawing.Size(300, 22);
		this.txtMinDigits.TabIndex = 3;
		this.txtMinDigits.Validating += new System.ComponentModel.CancelEventHandler(TxtMinDigits_Validating);
		this.lblMaxDigits.AutoSize = true;
		this.lblMaxDigits.Location = new System.Drawing.Point(7, 133);
		this.lblMaxDigits.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblMaxDigits.Name = "lblMaxDigits";
		this.lblMaxDigits.Size = new System.Drawing.Size(72, 17);
		this.lblMaxDigits.TabIndex = 4;
		this.lblMaxDigits.Text = "Max Digits";
		this.txtMaxDigits.HidePromptOnLeave = true;
		this.txtMaxDigits.Location = new System.Drawing.Point(10, 154);
		this.txtMaxDigits.Margin = new System.Windows.Forms.Padding(4);
		this.txtMaxDigits.Mask = "999";
		this.txtMaxDigits.Name = "txtMaxDigits";
		this.txtMaxDigits.Size = new System.Drawing.Size(300, 22);
		this.txtMaxDigits.TabIndex = 5;
		this.txtMaxDigits.Validating += new System.ComponentModel.CancelEventHandler(TxtMaxDigits_Validating);
		this.lblValidDigits.AutoSize = true;
		this.lblValidDigits.Location = new System.Drawing.Point(7, 239);
		this.lblValidDigits.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblValidDigits.Name = "lblValidDigits";
		this.lblValidDigits.Size = new System.Drawing.Size(78, 17);
		this.lblValidDigits.TabIndex = 8;
		this.lblValidDigits.Text = "Valid Digits";
		this.txtDtmfTimeout.HidePromptOnLeave = true;
		this.txtDtmfTimeout.Location = new System.Drawing.Point(10, 50);
		this.txtDtmfTimeout.Margin = new System.Windows.Forms.Padding(4);
		this.txtDtmfTimeout.Mask = "99";
		this.txtDtmfTimeout.Name = "txtDtmfTimeout";
		this.txtDtmfTimeout.Size = new System.Drawing.Size(300, 22);
		this.txtDtmfTimeout.TabIndex = 1;
		this.txtDtmfTimeout.Validating += new System.ComponentModel.CancelEventHandler(TxtNonZeroNumeric_Validating);
		this.lblDtmfTimeout.AutoSize = true;
		this.lblDtmfTimeout.Location = new System.Drawing.Point(7, 29);
		this.lblDtmfTimeout.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblDtmfTimeout.Name = "lblDtmfTimeout";
		this.lblDtmfTimeout.Size = new System.Drawing.Size(144, 17);
		this.lblDtmfTimeout.TabIndex = 0;
		this.lblDtmfTimeout.Text = "DTMF Timeout (secs)";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.chkAcceptDtmfInput);
		base.Controls.Add(this.grpBoxAcceptDtmf);
		base.Controls.Add(this.comboLanguageCode);
		base.Controls.Add(this.lblLanguageCode);
		base.Controls.Add(this.lblVoiceInput);
		base.Controls.Add(this.lblMaxRetryCount);
		base.Controls.Add(this.txtMaxRetryCount);
		base.Controls.Add(this.lblInputTimeout);
		base.Controls.Add(this.txtInputTimeout);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "OptionsComponentsVoiceInputControl";
		base.Size = new System.Drawing.Size(780, 665);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		this.grpBoxAcceptDtmf.ResumeLayout(false);
		this.grpBoxAcceptDtmf.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
