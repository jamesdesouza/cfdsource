using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Controls;
using TCX.CFD.Properties;

namespace TCX.CFD.Forms;

public class VoiceInputConfigurationForm : Form
{
	private readonly VoiceInputComponent voiceInputComponent;

	private readonly VoiceInputHintsEditorControl voiceInputHintsEditorControl;

	private readonly VoiceInputDictionaryEditorControl voiceInputDictionaryEditorControl;

	private readonly List<string> validVariables;

	private IContainer components;

	private Button cancelButton;

	private Button okButton;

	private Label lblMaxRetryCount;

	private MaskedTextBox txtMaxRetryCount;

	private Label lblInputTimeout;

	private MaskedTextBox txtTimeout;

	private ErrorProvider errorProvider;

	private Label lblLanguageCode;

	private GroupBox grpBoxPrompts;

	private Button invalidInputPromptsButton;

	private Button timeoutPromptsButton;

	private Button subsequentPromptsButton;

	private Button initialPromptsButton;

	private GroupBox grpBoxDictionary;

	private ComboBox comboLanguageCode;

	private GroupBox grpBoxHints;

	private GroupBox grpBoxAcceptDtmf;

	private CheckBox chkAcceptDtmfInput;

	private ComboBox comboStopDigit;

	private Label lblStopDigit;

	private MaskedTextBox txtDtmfTimeout;

	private Label lblDtmfTimeout;

	private Label lblMaxDigits;

	private Label lblMinDigits;

	private MaskedTextBox txtMaxDigits;

	private MaskedTextBox txtMinDigits;

	private CheckedListBox validDigitsListBox;

	private Label lblValidDigits;

	private Button filenameExpressionButton;

	private TextBox txtSaveToFile;

	private Button saveToFileExpressionButton;

	private Label lblSaveToFile;

	private Label lblFileName;

	private TextBox txtFileName;

	private GroupBox grpBoxSaveToFile;

	public List<Prompt> InitialPrompts { get; private set; }

	public List<Prompt> SubsequentPrompts { get; private set; }

	public List<Prompt> TimeoutPrompts { get; private set; }

	public List<Prompt> InvalidInputPrompts { get; private set; }

	public uint InputTimeout
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

	public string LanguageCode => comboLanguageCode.Text;

	public string SaveToFile => txtSaveToFile.Text;

	public string FileName => txtFileName.Text;

	public bool AcceptDtmfInput => chkAcceptDtmfInput.Checked;

	public uint DtmfTimeout
	{
		get
		{
			if (!string.IsNullOrEmpty(txtDtmfTimeout.Text) && Convert.ToUInt32(txtDtmfTimeout.Text) != 0)
			{
				return Convert.ToUInt32(txtDtmfTimeout.Text);
			}
			return Settings.Default.VoiceInputTemplateDtmfTimeout;
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
			return Settings.Default.VoiceInputTemplateMinDigits;
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
			return Settings.Default.VoiceInputTemplateMaxDigits;
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

	public List<string> Dictionary { get; private set; }

	public List<string> Hints { get; private set; }

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
		comboLanguageCode.Items.Add("ar-YE");
		comboLanguageCode.Items.Add("az-AZ");
		comboLanguageCode.Items.Add("bg-BG");
		comboLanguageCode.Items.Add("bn-BD");
		comboLanguageCode.Items.Add("bn-IN");
		comboLanguageCode.Items.Add("bs-BA");
		comboLanguageCode.Items.Add("ca-ES");
		comboLanguageCode.Items.Add("cs-CZ");
		comboLanguageCode.Items.Add("da-DK");
		comboLanguageCode.Items.Add("de-AT");
		comboLanguageCode.Items.Add("de-CH");
		comboLanguageCode.Items.Add("de-DE");
		comboLanguageCode.Items.Add("el-GR");
		comboLanguageCode.Items.Add("en-AU");
		comboLanguageCode.Items.Add("en-CA");
		comboLanguageCode.Items.Add("en-GB");
		comboLanguageCode.Items.Add("en-GH");
		comboLanguageCode.Items.Add("en-HK");
		comboLanguageCode.Items.Add("en-IE");
		comboLanguageCode.Items.Add("en-IN");
		comboLanguageCode.Items.Add("en-KE");
		comboLanguageCode.Items.Add("en-NG");
		comboLanguageCode.Items.Add("en-NZ");
		comboLanguageCode.Items.Add("en-PH");
		comboLanguageCode.Items.Add("en-PK");
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
		comboLanguageCode.Items.Add("et-EE");
		comboLanguageCode.Items.Add("eu-ES");
		comboLanguageCode.Items.Add("fa-IR");
		comboLanguageCode.Items.Add("fi-FI");
		comboLanguageCode.Items.Add("fil-PH");
		comboLanguageCode.Items.Add("fr-BE");
		comboLanguageCode.Items.Add("fr-CA");
		comboLanguageCode.Items.Add("fr-CH");
		comboLanguageCode.Items.Add("fr-FR");
		comboLanguageCode.Items.Add("gl-ES");
		comboLanguageCode.Items.Add("gu-IN");
		comboLanguageCode.Items.Add("hi-IN");
		comboLanguageCode.Items.Add("hr-HR");
		comboLanguageCode.Items.Add("hu-HU");
		comboLanguageCode.Items.Add("hy-AM");
		comboLanguageCode.Items.Add("id-ID");
		comboLanguageCode.Items.Add("is-IS");
		comboLanguageCode.Items.Add("it-CH");
		comboLanguageCode.Items.Add("it-IT");
		comboLanguageCode.Items.Add("iw-IL");
		comboLanguageCode.Items.Add("ja-JP");
		comboLanguageCode.Items.Add("jv-ID");
		comboLanguageCode.Items.Add("ka-GE");
		comboLanguageCode.Items.Add("kk-KZ");
		comboLanguageCode.Items.Add("km-KH");
		comboLanguageCode.Items.Add("kn-IN");
		comboLanguageCode.Items.Add("ko-KR");
		comboLanguageCode.Items.Add("lo-LA");
		comboLanguageCode.Items.Add("lt-LT");
		comboLanguageCode.Items.Add("lv-LV");
		comboLanguageCode.Items.Add("mk-MK");
		comboLanguageCode.Items.Add("ml-IN");
		comboLanguageCode.Items.Add("mn-MN");
		comboLanguageCode.Items.Add("mr-IN");
		comboLanguageCode.Items.Add("ms-MY");
		comboLanguageCode.Items.Add("my-MM");
		comboLanguageCode.Items.Add("ne-NP");
		comboLanguageCode.Items.Add("nl-BE");
		comboLanguageCode.Items.Add("nl-NL");
		comboLanguageCode.Items.Add("no-NO");
		comboLanguageCode.Items.Add("pa-Guru-IN");
		comboLanguageCode.Items.Add("pl-PL");
		comboLanguageCode.Items.Add("pt-BR");
		comboLanguageCode.Items.Add("pt-PT");
		comboLanguageCode.Items.Add("ro-RO");
		comboLanguageCode.Items.Add("ru-RU");
		comboLanguageCode.Items.Add("si-LK");
		comboLanguageCode.Items.Add("sk-SK");
		comboLanguageCode.Items.Add("sl-SI");
		comboLanguageCode.Items.Add("sq-AL");
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
		comboLanguageCode.Items.Add("uz-UZ");
		comboLanguageCode.Items.Add("vi-VN");
		comboLanguageCode.Items.Add("yue-Hant-HK");
		comboLanguageCode.Items.Add("zh");
		comboLanguageCode.Items.Add("zh-TW");
		comboLanguageCode.Items.Add("zu-ZA");
		if (!string.IsNullOrEmpty(voiceInputComponent.LanguageCode) && !comboLanguageCode.Items.Contains(voiceInputComponent.LanguageCode))
		{
			comboLanguageCode.Items.Add(voiceInputComponent.LanguageCode);
		}
	}

	private bool IsDigitValid(int index)
	{
		return validDigitsListBox.GetItemChecked(index);
	}

	public VoiceInputConfigurationForm(VoiceInputComponent voiceInputComponent)
	{
		InitializeComponent();
		this.voiceInputComponent = voiceInputComponent;
		InitialPrompts = new List<Prompt>(voiceInputComponent.InitialPrompts);
		SubsequentPrompts = new List<Prompt>(voiceInputComponent.SubsequentPrompts);
		TimeoutPrompts = new List<Prompt>(voiceInputComponent.TimeoutPrompts);
		InvalidInputPrompts = new List<Prompt>(voiceInputComponent.InvalidInputPrompts);
		validVariables = ExpressionHelper.GetValidVariables(voiceInputComponent);
		FillComboLanguageCode();
		txtTimeout.Text = voiceInputComponent.InputTimeout.ToString();
		txtMaxRetryCount.Text = voiceInputComponent.MaxRetryCount.ToString();
		comboLanguageCode.SelectedItem = voiceInputComponent.LanguageCode;
		txtSaveToFile.Text = voiceInputComponent.SaveToFile;
		txtFileName.Text = voiceInputComponent.FileName;
		txtFileName.Enabled = txtSaveToFile.Text != "false";
		filenameExpressionButton.Enabled = txtFileName.Enabled;
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
		chkAcceptDtmfInput.Checked = voiceInputComponent.AcceptDtmfInput;
		txtDtmfTimeout.Text = voiceInputComponent.DtmfTimeout.ToString();
		txtMinDigits.Text = voiceInputComponent.MinDigits.ToString();
		txtMaxDigits.Text = voiceInputComponent.MaxDigits.ToString();
		comboStopDigit.SelectedItem = voiceInputComponent.StopDigit;
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.validDigitsListBox.DigitText") + " 0", voiceInputComponent.IsValidDigit_0);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.validDigitsListBox.DigitText") + " 1", voiceInputComponent.IsValidDigit_1);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.validDigitsListBox.DigitText") + " 2", voiceInputComponent.IsValidDigit_2);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.validDigitsListBox.DigitText") + " 3", voiceInputComponent.IsValidDigit_3);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.validDigitsListBox.DigitText") + " 4", voiceInputComponent.IsValidDigit_4);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.validDigitsListBox.DigitText") + " 5", voiceInputComponent.IsValidDigit_5);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.validDigitsListBox.DigitText") + " 6", voiceInputComponent.IsValidDigit_6);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.validDigitsListBox.DigitText") + " 7", voiceInputComponent.IsValidDigit_7);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.validDigitsListBox.DigitText") + " 8", voiceInputComponent.IsValidDigit_8);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.validDigitsListBox.DigitText") + " 9", voiceInputComponent.IsValidDigit_9);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.validDigitsListBox.DigitText") + " *", voiceInputComponent.IsValidDigit_Star);
		validDigitsListBox.Items.Add(LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.validDigitsListBox.DigitText") + " #", voiceInputComponent.IsValidDigit_Pound);
		ChkAcceptDtmfInput_CheckedChanged(chkAcceptDtmfInput, EventArgs.Empty);
		TxtTimeout_Validating(txtTimeout, new CancelEventArgs());
		TxtMaxRetryCount_Validating(txtMaxRetryCount, new CancelEventArgs());
		TxtSaveToFile_Validating(txtSaveToFile, new CancelEventArgs());
		TxtFileName_Validating(txtFileName, new CancelEventArgs());
		voiceInputHintsEditorControl = new VoiceInputHintsEditorControl(LanguageCode);
		voiceInputHintsEditorControl.Dock = DockStyle.Fill;
		voiceInputHintsEditorControl.Hints = voiceInputComponent.Hints;
		grpBoxHints.Controls.Add(voiceInputHintsEditorControl);
		voiceInputDictionaryEditorControl = new VoiceInputDictionaryEditorControl(voiceInputComponent);
		voiceInputDictionaryEditorControl.Dock = DockStyle.Fill;
		voiceInputDictionaryEditorControl.Dictionary = voiceInputComponent.Dictionary;
		grpBoxDictionary.Controls.Add(voiceInputDictionaryEditorControl);
		Text = LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.Title");
		lblInputTimeout.Text = LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.lblInputTimeout.Text");
		lblMaxRetryCount.Text = LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.lblMaxRetryCount.Text");
		lblLanguageCode.Text = LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.lblLanguageCode.Text");
		chkAcceptDtmfInput.Text = LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.chkAcceptDtmfInput.Text");
		lblDtmfTimeout.Text = LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.lblDtmfTimeout.Text");
		lblMinDigits.Text = LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.lblMinDigits.Text");
		lblMaxDigits.Text = LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.lblMaxDigits.Text");
		lblStopDigit.Text = LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.lblStopDigit.Text");
		lblValidDigits.Text = LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.lblValidDigits.Text");
		grpBoxSaveToFile.Text = LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.grpBoxSaveToFile.Text");
		lblSaveToFile.Text = LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.lblSaveToFile.Text");
		lblFileName.Text = LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.lblFileName.Text");
		grpBoxPrompts.Text = LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.grpBoxPrompts.Text");
		initialPromptsButton.Text = LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.initialPromptsButton.Text");
		subsequentPromptsButton.Text = LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.subsequentPromptsButton.Text");
		timeoutPromptsButton.Text = LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.timeoutPromptsButton.Text");
		invalidInputPromptsButton.Text = LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.invalidInputPromptsButton.Text");
		grpBoxHints.Text = LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.grpBoxHints.Text");
		grpBoxDictionary.Text = LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.grpBoxDictionary.Text");
		okButton.Text = LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.cancelButton.Text");
	}

	private void MaskedTextBox_GotFocus(object sender, EventArgs e)
	{
		(sender as MaskedTextBox).SelectAll();
	}

	private void TxtTimeout_Validating(object sender, CancelEventArgs e)
	{
		errorProvider.SetError(txtTimeout, string.IsNullOrEmpty(txtTimeout.Text) ? LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.Error.TimeoutIsMandatory") : ((Convert.ToUInt32(txtTimeout.Text) == 0) ? LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.Error.TimeoutInvalidRange") : string.Empty));
	}

	private void TxtMaxRetryCount_Validating(object sender, CancelEventArgs e)
	{
		errorProvider.SetError(txtMaxRetryCount, string.IsNullOrEmpty(txtMaxRetryCount.Text) ? LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.Error.MaxRetryCountIsMandatory") : ((Convert.ToUInt32(txtMaxRetryCount.Text) == 0) ? LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.Error.MaxRetryCountInvalidRange") : string.Empty));
	}

	private void ComboLanguageCode_TextChanged(object sender, EventArgs e)
	{
		voiceInputHintsEditorControl?.UpdateLanguage(LanguageCode);
	}

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void TxtSaveToFile_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtSaveToFile.Text))
		{
			errorProvider.SetError(saveToFileExpressionButton, LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.Error.SaveToFileIsMandatory"));
		}
		else if (!AbsArgument.BuildArgument(validVariables, txtSaveToFile.Text).IsSafeExpression())
		{
			errorProvider.SetError(saveToFileExpressionButton, LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.Error.SaveToFileIsInvalid"));
		}
		else
		{
			errorProvider.SetError(saveToFileExpressionButton, string.Empty);
		}
	}

	private void TxtSaveToFile_Validated(object sender, EventArgs e)
	{
		txtFileName.Enabled = txtSaveToFile.Text != "false";
		filenameExpressionButton.Enabled = txtFileName.Enabled;
		TxtFileName_Validating(txtFileName, new CancelEventArgs());
	}

	private void TxtFileName_Validating(object sender, CancelEventArgs e)
	{
		if (txtFileName.Enabled)
		{
			if (string.IsNullOrEmpty(txtFileName.Text))
			{
				errorProvider.SetError(filenameExpressionButton, LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.Error.FileNameIsMandatory"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, txtFileName.Text).IsSafeExpression())
			{
				errorProvider.SetError(filenameExpressionButton, LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.Error.FileNameIsInvalid"));
			}
			else
			{
				errorProvider.SetError(filenameExpressionButton, string.Empty);
			}
		}
		else
		{
			errorProvider.SetError(filenameExpressionButton, string.Empty);
		}
	}

	private void SaveToFileExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(voiceInputComponent);
		expressionEditorForm.Expression = txtSaveToFile.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtSaveToFile.Text = expressionEditorForm.Expression;
			TxtSaveToFile_Validating(txtSaveToFile, new CancelEventArgs());
			TxtSaveToFile_Validated(txtSaveToFile, EventArgs.Empty);
		}
	}

	private void FilenameExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(voiceInputComponent);
		expressionEditorForm.Expression = txtFileName.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtFileName.Text = expressionEditorForm.Expression;
			TxtFileName_Validating(txtFileName, new CancelEventArgs());
		}
	}

	private void ChkAcceptDtmfInput_CheckedChanged(object sender, EventArgs e)
	{
		grpBoxAcceptDtmf.Enabled = chkAcceptDtmfInput.Checked;
		if (chkAcceptDtmfInput.Checked)
		{
			TxtDtmfTimeout_Validating(txtDtmfTimeout, new CancelEventArgs());
			ValidateMinDigits();
			ValidateMaxDigits();
		}
		else
		{
			errorProvider.SetError(txtDtmfTimeout, string.Empty);
			errorProvider.SetError(txtMinDigits, string.Empty);
			errorProvider.SetError(txtMaxDigits, string.Empty);
		}
	}

	private void ValidDigitsListBox_LostFocus(object sender, EventArgs e)
	{
		validDigitsListBox.SelectedIndex = -1;
	}

	private void TxtDtmfTimeout_Validating(object sender, CancelEventArgs e)
	{
		errorProvider.SetError(txtDtmfTimeout, string.IsNullOrEmpty(txtDtmfTimeout.Text) ? LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.Error.DtmfTimeoutIsMandatory") : ((Convert.ToUInt32(txtDtmfTimeout.Text) == 0) ? LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.Error.DtmfTimeoutInvalidRange") : string.Empty));
	}

	private void ValidateMinDigits()
	{
		if (string.IsNullOrEmpty(txtMinDigits.Text))
		{
			errorProvider.SetError(txtMinDigits, LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.Error.MinDigitsIsMandatory"));
			return;
		}
		int num = Convert.ToInt32(txtMinDigits.Text);
		if (num < 1 || num > 999)
		{
			errorProvider.SetError(txtMinDigits, string.Format(LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.Error.InvalidMinDigitsValue"), 1, 999));
		}
		else if (!string.IsNullOrEmpty(txtMaxDigits.Text))
		{
			int num2 = Convert.ToInt32(txtMaxDigits.Text);
			errorProvider.SetError(txtMinDigits, (num > num2) ? LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.Error.MinDigitsCantBeGreaterThanMaxDigits") : string.Empty);
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
			errorProvider.SetError(txtMaxDigits, LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.Error.MaxDigitsIsMandatory"));
			return;
		}
		int num = Convert.ToInt32(txtMaxDigits.Text);
		if (num < 1 || num > 999)
		{
			errorProvider.SetError(txtMaxDigits, string.Format(LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.Error.InvalidMaxDigitsValue"), 1, 999));
		}
		else if (!string.IsNullOrEmpty(txtMinDigits.Text))
		{
			int num2 = Convert.ToInt32(txtMinDigits.Text);
			errorProvider.SetError(txtMaxDigits, (num2 > num) ? LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.Error.MinDigitsCantBeGreaterThanMaxDigits") : string.Empty);
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
		PromptCollectionEditorForm promptCollectionEditorForm = new PromptCollectionEditorForm(voiceInputComponent);
		promptCollectionEditorForm.PromptList = InitialPrompts;
		if (promptCollectionEditorForm.ShowDialog() == DialogResult.OK)
		{
			InitialPrompts = promptCollectionEditorForm.PromptList;
		}
	}

	private void SubsequentPromptsButton_Click(object sender, EventArgs e)
	{
		PromptCollectionEditorForm promptCollectionEditorForm = new PromptCollectionEditorForm(voiceInputComponent);
		promptCollectionEditorForm.PromptList = SubsequentPrompts;
		if (promptCollectionEditorForm.ShowDialog() == DialogResult.OK)
		{
			SubsequentPrompts = promptCollectionEditorForm.PromptList;
		}
	}

	private void TimeoutPromptsButton_Click(object sender, EventArgs e)
	{
		PromptCollectionEditorForm promptCollectionEditorForm = new PromptCollectionEditorForm(voiceInputComponent);
		promptCollectionEditorForm.PromptList = TimeoutPrompts;
		if (promptCollectionEditorForm.ShowDialog() == DialogResult.OK)
		{
			TimeoutPrompts = promptCollectionEditorForm.PromptList;
		}
	}

	private void InvalidInputPromptsButton_Click(object sender, EventArgs e)
	{
		PromptCollectionEditorForm promptCollectionEditorForm = new PromptCollectionEditorForm(voiceInputComponent);
		promptCollectionEditorForm.PromptList = InvalidInputPrompts;
		if (promptCollectionEditorForm.ShowDialog() == DialogResult.OK)
		{
			InvalidInputPrompts = promptCollectionEditorForm.PromptList;
		}
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(txtTimeout.Text))
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.Error.TimeoutIsMandatory"), LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			txtTimeout.Focus();
			return;
		}
		if (Convert.ToUInt32(txtTimeout.Text) == 0)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.Error.TimeoutInvalidRange"), LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			txtTimeout.Focus();
			return;
		}
		if (string.IsNullOrEmpty(txtMaxRetryCount.Text))
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.Error.MaxRetryCountIsMandatory"), LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			txtMaxRetryCount.Focus();
			return;
		}
		if (Convert.ToUInt32(txtMaxRetryCount.Text) == 0)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.Error.MaxRetryCountInvalidRange"), LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			txtMaxRetryCount.Focus();
			return;
		}
		if (chkAcceptDtmfInput.Checked)
		{
			if (string.IsNullOrEmpty(txtDtmfTimeout.Text))
			{
				MessageBox.Show(LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.Error.DtmfTimeoutIsMandatory"), LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				txtDtmfTimeout.Focus();
				return;
			}
			if (Convert.ToUInt32(txtDtmfTimeout.Text) == 0)
			{
				MessageBox.Show(LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.Error.DtmfTimeoutInvalidRange"), LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				txtDtmfTimeout.Focus();
				return;
			}
			if (string.IsNullOrEmpty(txtMinDigits.Text))
			{
				MessageBox.Show(LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.Error.MinDigitsIsMandatory"), LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				txtMinDigits.Focus();
				return;
			}
			if (Convert.ToUInt32(txtMinDigits.Text) == 0)
			{
				MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.Error.InvalidMinDigitsValue"), 1, 999), LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				txtMinDigits.Focus();
				return;
			}
			if (string.IsNullOrEmpty(txtMaxDigits.Text))
			{
				MessageBox.Show(LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.Error.MaxDigitsIsMandatory"), LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				txtMaxDigits.Focus();
				return;
			}
			if (Convert.ToUInt32(txtMaxDigits.Text) == 0)
			{
				MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.Error.InvalidMaxDigitsValue"), 1, 999), LocalizedResourceMgr.GetString("VoiceInputConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				txtMaxDigits.Focus();
				return;
			}
		}
		Hints = voiceInputHintsEditorControl.Hints;
		Dictionary = voiceInputDictionaryEditorControl.Dictionary;
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void CancelButton_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.Cancel;
		Close();
	}

	private void VoiceInputConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		voiceInputComponent.ShowHelp();
	}

	private void VoiceInputConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		voiceInputComponent.ShowHelp();
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
		this.lblMaxRetryCount = new System.Windows.Forms.Label();
		this.txtMaxRetryCount = new System.Windows.Forms.MaskedTextBox();
		this.lblInputTimeout = new System.Windows.Forms.Label();
		this.txtTimeout = new System.Windows.Forms.MaskedTextBox();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.lblLanguageCode = new System.Windows.Forms.Label();
		this.grpBoxPrompts = new System.Windows.Forms.GroupBox();
		this.invalidInputPromptsButton = new System.Windows.Forms.Button();
		this.timeoutPromptsButton = new System.Windows.Forms.Button();
		this.subsequentPromptsButton = new System.Windows.Forms.Button();
		this.initialPromptsButton = new System.Windows.Forms.Button();
		this.grpBoxDictionary = new System.Windows.Forms.GroupBox();
		this.comboLanguageCode = new System.Windows.Forms.ComboBox();
		this.grpBoxHints = new System.Windows.Forms.GroupBox();
		this.grpBoxAcceptDtmf = new System.Windows.Forms.GroupBox();
		this.validDigitsListBox = new System.Windows.Forms.CheckedListBox();
		this.lblValidDigits = new System.Windows.Forms.Label();
		this.lblMaxDigits = new System.Windows.Forms.Label();
		this.lblMinDigits = new System.Windows.Forms.Label();
		this.txtMaxDigits = new System.Windows.Forms.MaskedTextBox();
		this.txtMinDigits = new System.Windows.Forms.MaskedTextBox();
		this.comboStopDigit = new System.Windows.Forms.ComboBox();
		this.lblStopDigit = new System.Windows.Forms.Label();
		this.txtDtmfTimeout = new System.Windows.Forms.MaskedTextBox();
		this.lblDtmfTimeout = new System.Windows.Forms.Label();
		this.chkAcceptDtmfInput = new System.Windows.Forms.CheckBox();
		this.filenameExpressionButton = new System.Windows.Forms.Button();
		this.txtSaveToFile = new System.Windows.Forms.TextBox();
		this.saveToFileExpressionButton = new System.Windows.Forms.Button();
		this.lblSaveToFile = new System.Windows.Forms.Label();
		this.lblFileName = new System.Windows.Forms.Label();
		this.txtFileName = new System.Windows.Forms.TextBox();
		this.grpBoxSaveToFile = new System.Windows.Forms.GroupBox();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		this.grpBoxPrompts.SuspendLayout();
		this.grpBoxAcceptDtmf.SuspendLayout();
		this.grpBoxSaveToFile.SuspendLayout();
		base.SuspendLayout();
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(832, 790);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 13;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.cancelButton.Click += new System.EventHandler(CancelButton_Click);
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(724, 790);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 12;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.lblMaxRetryCount.AutoSize = true;
		this.lblMaxRetryCount.Location = new System.Drawing.Point(13, 46);
		this.lblMaxRetryCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblMaxRetryCount.Name = "lblMaxRetryCount";
		this.lblMaxRetryCount.Size = new System.Drawing.Size(82, 17);
		this.lblMaxRetryCount.TabIndex = 2;
		this.lblMaxRetryCount.Text = "Max Retries";
		this.txtMaxRetryCount.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtMaxRetryCount.HidePromptOnLeave = true;
		this.txtMaxRetryCount.Location = new System.Drawing.Point(157, 43);
		this.txtMaxRetryCount.Margin = new System.Windows.Forms.Padding(4);
		this.txtMaxRetryCount.Mask = "99";
		this.txtMaxRetryCount.Name = "txtMaxRetryCount";
		this.txtMaxRetryCount.Size = new System.Drawing.Size(175, 22);
		this.txtMaxRetryCount.TabIndex = 3;
		this.txtMaxRetryCount.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtMaxRetryCount.Validating += new System.ComponentModel.CancelEventHandler(TxtMaxRetryCount_Validating);
		this.lblInputTimeout.AutoSize = true;
		this.lblInputTimeout.Location = new System.Drawing.Point(13, 16);
		this.lblInputTimeout.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblInputTimeout.Name = "lblInputTimeout";
		this.lblInputTimeout.Size = new System.Drawing.Size(137, 17);
		this.lblInputTimeout.TabIndex = 0;
		this.lblInputTimeout.Text = "Input Timeout (secs)";
		this.txtTimeout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtTimeout.HidePromptOnLeave = true;
		this.txtTimeout.Location = new System.Drawing.Point(157, 13);
		this.txtTimeout.Margin = new System.Windows.Forms.Padding(4);
		this.txtTimeout.Mask = "99";
		this.txtTimeout.Name = "txtTimeout";
		this.txtTimeout.Size = new System.Drawing.Size(175, 22);
		this.txtTimeout.TabIndex = 1;
		this.txtTimeout.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtTimeout.Validating += new System.ComponentModel.CancelEventHandler(TxtTimeout_Validating);
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.lblLanguageCode.AutoSize = true;
		this.lblLanguageCode.Location = new System.Drawing.Point(13, 76);
		this.lblLanguageCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblLanguageCode.Name = "lblLanguageCode";
		this.lblLanguageCode.Size = new System.Drawing.Size(109, 17);
		this.lblLanguageCode.TabIndex = 4;
		this.lblLanguageCode.Text = "Language Code";
		this.grpBoxPrompts.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.grpBoxPrompts.Controls.Add(this.invalidInputPromptsButton);
		this.grpBoxPrompts.Controls.Add(this.timeoutPromptsButton);
		this.grpBoxPrompts.Controls.Add(this.subsequentPromptsButton);
		this.grpBoxPrompts.Controls.Add(this.initialPromptsButton);
		this.grpBoxPrompts.Location = new System.Drawing.Point(12, 307);
		this.grpBoxPrompts.Margin = new System.Windows.Forms.Padding(4);
		this.grpBoxPrompts.Name = "grpBoxPrompts";
		this.grpBoxPrompts.Padding = new System.Windows.Forms.Padding(4);
		this.grpBoxPrompts.Size = new System.Drawing.Size(919, 103);
		this.grpBoxPrompts.TabIndex = 9;
		this.grpBoxPrompts.TabStop = false;
		this.grpBoxPrompts.Text = "Prompts";
		this.invalidInputPromptsButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.invalidInputPromptsButton.Location = new System.Drawing.Point(466, 59);
		this.invalidInputPromptsButton.Margin = new System.Windows.Forms.Padding(4);
		this.invalidInputPromptsButton.Name = "invalidInputPromptsButton";
		this.invalidInputPromptsButton.Size = new System.Drawing.Size(445, 28);
		this.invalidInputPromptsButton.TabIndex = 3;
		this.invalidInputPromptsButton.Text = "Invalid Input Prompts";
		this.invalidInputPromptsButton.UseVisualStyleBackColor = true;
		this.invalidInputPromptsButton.Click += new System.EventHandler(InvalidInputPromptsButton_Click);
		this.timeoutPromptsButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.timeoutPromptsButton.Location = new System.Drawing.Point(466, 24);
		this.timeoutPromptsButton.Margin = new System.Windows.Forms.Padding(4);
		this.timeoutPromptsButton.Name = "timeoutPromptsButton";
		this.timeoutPromptsButton.Size = new System.Drawing.Size(445, 28);
		this.timeoutPromptsButton.TabIndex = 2;
		this.timeoutPromptsButton.Text = "Timeout Prompts";
		this.timeoutPromptsButton.UseVisualStyleBackColor = true;
		this.timeoutPromptsButton.Click += new System.EventHandler(TimeoutPromptsButton_Click);
		this.subsequentPromptsButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.subsequentPromptsButton.Location = new System.Drawing.Point(8, 59);
		this.subsequentPromptsButton.Margin = new System.Windows.Forms.Padding(4);
		this.subsequentPromptsButton.Name = "subsequentPromptsButton";
		this.subsequentPromptsButton.Size = new System.Drawing.Size(450, 28);
		this.subsequentPromptsButton.TabIndex = 1;
		this.subsequentPromptsButton.Text = "Subsequent Prompts";
		this.subsequentPromptsButton.UseVisualStyleBackColor = true;
		this.subsequentPromptsButton.Click += new System.EventHandler(SubsequentPromptsButton_Click);
		this.initialPromptsButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.initialPromptsButton.Location = new System.Drawing.Point(8, 23);
		this.initialPromptsButton.Margin = new System.Windows.Forms.Padding(4);
		this.initialPromptsButton.Name = "initialPromptsButton";
		this.initialPromptsButton.Size = new System.Drawing.Size(450, 28);
		this.initialPromptsButton.TabIndex = 0;
		this.initialPromptsButton.Text = "Initial Prompts";
		this.initialPromptsButton.UseVisualStyleBackColor = true;
		this.initialPromptsButton.Click += new System.EventHandler(InitialPromptsButton_Click);
		this.grpBoxDictionary.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.grpBoxDictionary.Location = new System.Drawing.Point(12, 623);
		this.grpBoxDictionary.Name = "grpBoxDictionary";
		this.grpBoxDictionary.Size = new System.Drawing.Size(919, 160);
		this.grpBoxDictionary.TabIndex = 11;
		this.grpBoxDictionary.TabStop = false;
		this.grpBoxDictionary.Text = "Dictionary";
		this.comboLanguageCode.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboLanguageCode.Location = new System.Drawing.Point(157, 73);
		this.comboLanguageCode.Margin = new System.Windows.Forms.Padding(4);
		this.comboLanguageCode.MaxLength = 1024;
		this.comboLanguageCode.Name = "comboLanguageCode";
		this.comboLanguageCode.Size = new System.Drawing.Size(175, 24);
		this.comboLanguageCode.TabIndex = 5;
		this.comboLanguageCode.TextChanged += new System.EventHandler(ComboLanguageCode_TextChanged);
		this.grpBoxHints.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.grpBoxHints.Location = new System.Drawing.Point(13, 417);
		this.grpBoxHints.Name = "grpBoxHints";
		this.grpBoxHints.Size = new System.Drawing.Size(919, 200);
		this.grpBoxHints.TabIndex = 10;
		this.grpBoxHints.TabStop = false;
		this.grpBoxHints.Text = "Hints";
		this.grpBoxAcceptDtmf.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.grpBoxAcceptDtmf.Controls.Add(this.validDigitsListBox);
		this.grpBoxAcceptDtmf.Controls.Add(this.lblValidDigits);
		this.grpBoxAcceptDtmf.Controls.Add(this.lblMaxDigits);
		this.grpBoxAcceptDtmf.Controls.Add(this.lblMinDigits);
		this.grpBoxAcceptDtmf.Controls.Add(this.txtMaxDigits);
		this.grpBoxAcceptDtmf.Controls.Add(this.txtMinDigits);
		this.grpBoxAcceptDtmf.Controls.Add(this.comboStopDigit);
		this.grpBoxAcceptDtmf.Controls.Add(this.lblStopDigit);
		this.grpBoxAcceptDtmf.Controls.Add(this.txtDtmfTimeout);
		this.grpBoxAcceptDtmf.Controls.Add(this.lblDtmfTimeout);
		this.grpBoxAcceptDtmf.Location = new System.Drawing.Point(353, 12);
		this.grpBoxAcceptDtmf.Name = "grpBoxAcceptDtmf";
		this.grpBoxAcceptDtmf.Size = new System.Drawing.Size(579, 191);
		this.grpBoxAcceptDtmf.TabIndex = 7;
		this.grpBoxAcceptDtmf.TabStop = false;
		this.validDigitsListBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.validDigitsListBox.CheckOnClick = true;
		this.validDigitsListBox.FormattingEnabled = true;
		this.validDigitsListBox.Location = new System.Drawing.Point(11, 106);
		this.validDigitsListBox.Margin = new System.Windows.Forms.Padding(4);
		this.validDigitsListBox.MinimumSize = new System.Drawing.Size(520, 78);
		this.validDigitsListBox.MultiColumn = true;
		this.validDigitsListBox.Name = "validDigitsListBox";
		this.validDigitsListBox.Size = new System.Drawing.Size(540, 72);
		this.validDigitsListBox.TabIndex = 9;
		this.validDigitsListBox.LostFocus += new System.EventHandler(ValidDigitsListBox_LostFocus);
		this.lblValidDigits.AutoSize = true;
		this.lblValidDigits.Location = new System.Drawing.Point(11, 87);
		this.lblValidDigits.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblValidDigits.Name = "lblValidDigits";
		this.lblValidDigits.Size = new System.Drawing.Size(78, 17);
		this.lblValidDigits.TabIndex = 8;
		this.lblValidDigits.Text = "Valid Digits";
		this.lblMaxDigits.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.lblMaxDigits.AutoSize = true;
		this.lblMaxDigits.Location = new System.Drawing.Point(332, 57);
		this.lblMaxDigits.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblMaxDigits.Name = "lblMaxDigits";
		this.lblMaxDigits.Size = new System.Drawing.Size(72, 17);
		this.lblMaxDigits.TabIndex = 6;
		this.lblMaxDigits.Text = "Max Digits";
		this.lblMinDigits.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.lblMinDigits.AutoSize = true;
		this.lblMinDigits.Location = new System.Drawing.Point(332, 26);
		this.lblMinDigits.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblMinDigits.Name = "lblMinDigits";
		this.lblMinDigits.Size = new System.Drawing.Size(69, 17);
		this.lblMinDigits.TabIndex = 4;
		this.lblMinDigits.Text = "Min Digits";
		this.txtMaxDigits.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.txtMaxDigits.HidePromptOnLeave = true;
		this.txtMaxDigits.Location = new System.Drawing.Point(412, 55);
		this.txtMaxDigits.Margin = new System.Windows.Forms.Padding(4);
		this.txtMaxDigits.Mask = "999";
		this.txtMaxDigits.Name = "txtMaxDigits";
		this.txtMaxDigits.Size = new System.Drawing.Size(139, 22);
		this.txtMaxDigits.TabIndex = 7;
		this.txtMaxDigits.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtMaxDigits.Validating += new System.ComponentModel.CancelEventHandler(TxtMaxDigits_Validating);
		this.txtMinDigits.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.txtMinDigits.HidePromptOnLeave = true;
		this.txtMinDigits.Location = new System.Drawing.Point(412, 23);
		this.txtMinDigits.Margin = new System.Windows.Forms.Padding(4);
		this.txtMinDigits.Mask = "999";
		this.txtMinDigits.Name = "txtMinDigits";
		this.txtMinDigits.Size = new System.Drawing.Size(139, 22);
		this.txtMinDigits.TabIndex = 5;
		this.txtMinDigits.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtMinDigits.Validating += new System.ComponentModel.CancelEventHandler(TxtMinDigits_Validating);
		this.comboStopDigit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboStopDigit.FormattingEnabled = true;
		this.comboStopDigit.Location = new System.Drawing.Point(163, 53);
		this.comboStopDigit.Margin = new System.Windows.Forms.Padding(4);
		this.comboStopDigit.Name = "comboStopDigit";
		this.comboStopDigit.Size = new System.Drawing.Size(139, 24);
		this.comboStopDigit.TabIndex = 3;
		this.lblStopDigit.AutoSize = true;
		this.lblStopDigit.Location = new System.Drawing.Point(11, 57);
		this.lblStopDigit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblStopDigit.Name = "lblStopDigit";
		this.lblStopDigit.Size = new System.Drawing.Size(69, 17);
		this.lblStopDigit.TabIndex = 2;
		this.lblStopDigit.Text = "Stop Digit";
		this.txtDtmfTimeout.HidePromptOnLeave = true;
		this.txtDtmfTimeout.Location = new System.Drawing.Point(163, 23);
		this.txtDtmfTimeout.Margin = new System.Windows.Forms.Padding(4);
		this.txtDtmfTimeout.Mask = "99";
		this.txtDtmfTimeout.Name = "txtDtmfTimeout";
		this.txtDtmfTimeout.Size = new System.Drawing.Size(139, 22);
		this.txtDtmfTimeout.TabIndex = 1;
		this.txtDtmfTimeout.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtDtmfTimeout.Validating += new System.ComponentModel.CancelEventHandler(TxtDtmfTimeout_Validating);
		this.lblDtmfTimeout.AutoSize = true;
		this.lblDtmfTimeout.Location = new System.Drawing.Point(11, 26);
		this.lblDtmfTimeout.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblDtmfTimeout.Name = "lblDtmfTimeout";
		this.lblDtmfTimeout.Size = new System.Drawing.Size(144, 17);
		this.lblDtmfTimeout.TabIndex = 0;
		this.lblDtmfTimeout.Text = "DTMF Timeout (secs)";
		this.chkAcceptDtmfInput.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkAcceptDtmfInput.AutoSize = true;
		this.chkAcceptDtmfInput.Location = new System.Drawing.Point(369, 9);
		this.chkAcceptDtmfInput.Margin = new System.Windows.Forms.Padding(4);
		this.chkAcceptDtmfInput.Name = "chkAcceptDtmfInput";
		this.chkAcceptDtmfInput.Size = new System.Drawing.Size(150, 21);
		this.chkAcceptDtmfInput.TabIndex = 6;
		this.chkAcceptDtmfInput.Text = "Accept DTMF Input";
		this.chkAcceptDtmfInput.UseVisualStyleBackColor = true;
		this.chkAcceptDtmfInput.CheckedChanged += new System.EventHandler(ChkAcceptDtmfInput_CheckedChanged);
		this.filenameExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.filenameExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.filenameExpressionButton.Location = new System.Drawing.Point(852, 55);
		this.filenameExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.filenameExpressionButton.Name = "filenameExpressionButton";
		this.filenameExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.filenameExpressionButton.TabIndex = 5;
		this.filenameExpressionButton.UseVisualStyleBackColor = true;
		this.filenameExpressionButton.Click += new System.EventHandler(FilenameExpressionButton_Click);
		this.txtSaveToFile.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtSaveToFile.AutoCompleteCustomSource.AddRange(new string[2] { "True", "False" });
		this.txtSaveToFile.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
		this.txtSaveToFile.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtSaveToFile.Location = new System.Drawing.Point(97, 26);
		this.txtSaveToFile.Margin = new System.Windows.Forms.Padding(4);
		this.txtSaveToFile.MaxLength = 8192;
		this.txtSaveToFile.Name = "txtSaveToFile";
		this.txtSaveToFile.Size = new System.Drawing.Size(747, 22);
		this.txtSaveToFile.TabIndex = 1;
		this.txtSaveToFile.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtSaveToFile.Validating += new System.ComponentModel.CancelEventHandler(TxtSaveToFile_Validating);
		this.txtSaveToFile.Validated += new System.EventHandler(TxtSaveToFile_Validated);
		this.saveToFileExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.saveToFileExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.saveToFileExpressionButton.Location = new System.Drawing.Point(852, 23);
		this.saveToFileExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.saveToFileExpressionButton.Name = "saveToFileExpressionButton";
		this.saveToFileExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.saveToFileExpressionButton.TabIndex = 2;
		this.saveToFileExpressionButton.UseVisualStyleBackColor = true;
		this.saveToFileExpressionButton.Click += new System.EventHandler(SaveToFileExpressionButton_Click);
		this.lblSaveToFile.AutoSize = true;
		this.lblSaveToFile.Location = new System.Drawing.Point(7, 29);
		this.lblSaveToFile.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblSaveToFile.Name = "lblSaveToFile";
		this.lblSaveToFile.Size = new System.Drawing.Size(82, 17);
		this.lblSaveToFile.TabIndex = 0;
		this.lblSaveToFile.Text = "Save to File";
		this.lblFileName.AutoSize = true;
		this.lblFileName.Location = new System.Drawing.Point(7, 61);
		this.lblFileName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblFileName.Name = "lblFileName";
		this.lblFileName.Size = new System.Drawing.Size(71, 17);
		this.lblFileName.TabIndex = 3;
		this.lblFileName.Text = "File Name";
		this.txtFileName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtFileName.Location = new System.Drawing.Point(97, 58);
		this.txtFileName.Margin = new System.Windows.Forms.Padding(4);
		this.txtFileName.MaxLength = 8192;
		this.txtFileName.Name = "txtFileName";
		this.txtFileName.Size = new System.Drawing.Size(747, 22);
		this.txtFileName.TabIndex = 4;
		this.txtFileName.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtFileName.Validating += new System.ComponentModel.CancelEventHandler(TxtFileName_Validating);
		this.grpBoxSaveToFile.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.grpBoxSaveToFile.Controls.Add(this.lblSaveToFile);
		this.grpBoxSaveToFile.Controls.Add(this.filenameExpressionButton);
		this.grpBoxSaveToFile.Controls.Add(this.txtFileName);
		this.grpBoxSaveToFile.Controls.Add(this.txtSaveToFile);
		this.grpBoxSaveToFile.Controls.Add(this.lblFileName);
		this.grpBoxSaveToFile.Controls.Add(this.saveToFileExpressionButton);
		this.grpBoxSaveToFile.Location = new System.Drawing.Point(13, 209);
		this.grpBoxSaveToFile.Name = "grpBoxSaveToFile";
		this.grpBoxSaveToFile.Size = new System.Drawing.Size(918, 91);
		this.grpBoxSaveToFile.TabIndex = 8;
		this.grpBoxSaveToFile.TabStop = false;
		this.grpBoxSaveToFile.Text = "Voice Input to File";
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(948, 833);
		base.Controls.Add(this.grpBoxSaveToFile);
		base.Controls.Add(this.chkAcceptDtmfInput);
		base.Controls.Add(this.grpBoxAcceptDtmf);
		base.Controls.Add(this.grpBoxHints);
		base.Controls.Add(this.comboLanguageCode);
		base.Controls.Add(this.grpBoxDictionary);
		base.Controls.Add(this.grpBoxPrompts);
		base.Controls.Add(this.lblLanguageCode);
		base.Controls.Add(this.lblMaxRetryCount);
		base.Controls.Add(this.txtMaxRetryCount);
		base.Controls.Add(this.lblInputTimeout);
		base.Controls.Add(this.txtTimeout);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(966, 880);
		base.Name = "VoiceInputConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Voice Input";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(VoiceInputConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(VoiceInputConfigurationForm_HelpRequested);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		this.grpBoxPrompts.ResumeLayout(false);
		this.grpBoxAcceptDtmf.ResumeLayout(false);
		this.grpBoxAcceptDtmf.PerformLayout();
		this.grpBoxSaveToFile.ResumeLayout(false);
		this.grpBoxSaveToFile.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
