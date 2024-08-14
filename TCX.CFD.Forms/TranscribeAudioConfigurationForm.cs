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

public class TranscribeAudioConfigurationForm : Form
{
	private readonly TranscribeAudioComponent transcribeAudioComponent;

	private readonly VoiceInputHintsEditorControl hintsEditorControl;

	private readonly List<string> validVariables;

	private IContainer components;

	private Button cancelButton;

	private Button okButton;

	private ErrorProvider errorProvider;

	private Label lblLanguageCode;

	private ComboBox comboLanguageCode;

	private GroupBox grpBoxHints;

	private Button filenameExpressionButton;

	private Label lblFileName;

	private TextBox txtFileName;

	public string LanguageCode => comboLanguageCode.Text;

	public string FileName => txtFileName.Text;

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
		if (!string.IsNullOrEmpty(transcribeAudioComponent.LanguageCode) && !comboLanguageCode.Items.Contains(transcribeAudioComponent.LanguageCode))
		{
			comboLanguageCode.Items.Add(transcribeAudioComponent.LanguageCode);
		}
	}

	public TranscribeAudioConfigurationForm(TranscribeAudioComponent transcribeAudioComponent)
	{
		InitializeComponent();
		this.transcribeAudioComponent = transcribeAudioComponent;
		validVariables = ExpressionHelper.GetValidVariables(transcribeAudioComponent);
		FillComboLanguageCode();
		comboLanguageCode.SelectedItem = transcribeAudioComponent.LanguageCode;
		txtFileName.Text = transcribeAudioComponent.FileName;
		TxtFileName_Validating(txtFileName, new CancelEventArgs());
		hintsEditorControl = new VoiceInputHintsEditorControl(LanguageCode);
		hintsEditorControl.Dock = DockStyle.Fill;
		hintsEditorControl.Hints = transcribeAudioComponent.Hints;
		grpBoxHints.Controls.Add(hintsEditorControl);
		Text = LocalizedResourceMgr.GetString("TranscribeAudioConfigurationForm.Title");
		lblLanguageCode.Text = LocalizedResourceMgr.GetString("TranscribeAudioConfigurationForm.lblLanguageCode.Text");
		lblFileName.Text = LocalizedResourceMgr.GetString("TranscribeAudioConfigurationForm.lblFileName.Text");
		grpBoxHints.Text = LocalizedResourceMgr.GetString("TranscribeAudioConfigurationForm.grpBoxHints.Text");
		okButton.Text = LocalizedResourceMgr.GetString("TranscribeAudioConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("TranscribeAudioConfigurationForm.cancelButton.Text");
	}

	private void ComboLanguageCode_TextChanged(object sender, EventArgs e)
	{
		hintsEditorControl?.UpdateLanguage(LanguageCode);
	}

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void TxtFileName_Validating(object sender, CancelEventArgs e)
	{
		if (txtFileName.Enabled)
		{
			if (string.IsNullOrEmpty(txtFileName.Text))
			{
				errorProvider.SetError(filenameExpressionButton, LocalizedResourceMgr.GetString("TranscribeAudioConfigurationForm.Error.FileNameIsMandatory"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, txtFileName.Text).IsSafeExpression())
			{
				errorProvider.SetError(filenameExpressionButton, LocalizedResourceMgr.GetString("TranscribeAudioConfigurationForm.Error.FileNameIsInvalid"));
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

	private void FilenameExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(transcribeAudioComponent);
		expressionEditorForm.Expression = txtFileName.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtFileName.Text = expressionEditorForm.Expression;
			TxtFileName_Validating(txtFileName, new CancelEventArgs());
		}
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		Hints = hintsEditorControl.Hints;
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void CancelButton_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.Cancel;
		Close();
	}

	private void TranscribeAudioConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		transcribeAudioComponent.ShowHelp();
	}

	private void TranscribeAudioConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		transcribeAudioComponent.ShowHelp();
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
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.lblLanguageCode = new System.Windows.Forms.Label();
		this.comboLanguageCode = new System.Windows.Forms.ComboBox();
		this.grpBoxHints = new System.Windows.Forms.GroupBox();
		this.filenameExpressionButton = new System.Windows.Forms.Button();
		this.lblFileName = new System.Windows.Forms.Label();
		this.txtFileName = new System.Windows.Forms.TextBox();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(775, 312);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 7;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.cancelButton.Click += new System.EventHandler(CancelButton_Click);
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(667, 312);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 6;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.lblLanguageCode.AutoSize = true;
		this.lblLanguageCode.Location = new System.Drawing.Point(13, 16);
		this.lblLanguageCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblLanguageCode.Name = "lblLanguageCode";
		this.lblLanguageCode.Size = new System.Drawing.Size(109, 17);
		this.lblLanguageCode.TabIndex = 0;
		this.lblLanguageCode.Text = "Language Code";
		this.comboLanguageCode.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboLanguageCode.Location = new System.Drawing.Point(141, 13);
		this.comboLanguageCode.Margin = new System.Windows.Forms.Padding(4);
		this.comboLanguageCode.MaxLength = 1024;
		this.comboLanguageCode.Name = "comboLanguageCode";
		this.comboLanguageCode.Size = new System.Drawing.Size(734, 24);
		this.comboLanguageCode.TabIndex = 1;
		this.comboLanguageCode.TextChanged += new System.EventHandler(ComboLanguageCode_TextChanged);
		this.grpBoxHints.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.grpBoxHints.Location = new System.Drawing.Point(13, 75);
		this.grpBoxHints.Name = "grpBoxHints";
		this.grpBoxHints.Size = new System.Drawing.Size(862, 228);
		this.grpBoxHints.TabIndex = 5;
		this.grpBoxHints.TabStop = false;
		this.grpBoxHints.Text = "Hints";
		this.filenameExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.filenameExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.filenameExpressionButton.Location = new System.Drawing.Point(883, 40);
		this.filenameExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.filenameExpressionButton.Name = "filenameExpressionButton";
		this.filenameExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.filenameExpressionButton.TabIndex = 4;
		this.filenameExpressionButton.UseVisualStyleBackColor = true;
		this.filenameExpressionButton.Click += new System.EventHandler(FilenameExpressionButton_Click);
		this.lblFileName.AutoSize = true;
		this.lblFileName.Location = new System.Drawing.Point(13, 46);
		this.lblFileName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblFileName.Name = "lblFileName";
		this.lblFileName.Size = new System.Drawing.Size(120, 17);
		this.lblFileName.TabIndex = 2;
		this.lblFileName.Text = "Source File Name";
		this.txtFileName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtFileName.Location = new System.Drawing.Point(141, 43);
		this.txtFileName.Margin = new System.Windows.Forms.Padding(4);
		this.txtFileName.MaxLength = 8192;
		this.txtFileName.Name = "txtFileName";
		this.txtFileName.Size = new System.Drawing.Size(734, 22);
		this.txtFileName.TabIndex = 3;
		this.txtFileName.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtFileName.Validating += new System.ComponentModel.CancelEventHandler(TxtFileName_Validating);
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(943, 353);
		base.Controls.Add(this.filenameExpressionButton);
		base.Controls.Add(this.lblFileName);
		base.Controls.Add(this.txtFileName);
		base.Controls.Add(this.grpBoxHints);
		base.Controls.Add(this.comboLanguageCode);
		base.Controls.Add(this.lblLanguageCode);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(961, 400);
		base.Name = "TranscribeAudioConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Voice Input";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(TranscribeAudioConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(TranscribeAudioConfigurationForm_HelpRequested);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
