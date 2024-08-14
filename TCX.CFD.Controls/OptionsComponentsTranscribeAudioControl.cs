using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class OptionsComponentsTranscribeAudioControl : UserControl, IOptionsControl
{
	private IContainer components;

	private Label lblTranscribeAudio;

	private Label lblLanguageCode;

	private ComboBox comboLanguageCode;

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
		if (!string.IsNullOrEmpty(Settings.Default.TranscribeAudioTemplateLanguageCode) && !comboLanguageCode.Items.Contains(Settings.Default.TranscribeAudioTemplateLanguageCode))
		{
			comboLanguageCode.Items.Add(Settings.Default.TranscribeAudioTemplateLanguageCode);
		}
	}

	public OptionsComponentsTranscribeAudioControl()
	{
		InitializeComponent();
		FillComboLanguageCode();
		comboLanguageCode.SelectedItem = Settings.Default.TranscribeAudioTemplateLanguageCode;
		lblTranscribeAudio.Text = LocalizedResourceMgr.GetString("OptionsComponentsTranscribeAudioControl.lblTranscribeAudio.Text");
		lblLanguageCode.Text = LocalizedResourceMgr.GetString("OptionsComponentsTranscribeAudioControl.lblLanguageCode.Text");
	}

	public void Save()
	{
		Settings.Default.TranscribeAudioTemplateLanguageCode = comboLanguageCode.Text;
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
		this.lblTranscribeAudio = new System.Windows.Forms.Label();
		this.lblLanguageCode = new System.Windows.Forms.Label();
		this.comboLanguageCode = new System.Windows.Forms.ComboBox();
		base.SuspendLayout();
		this.lblTranscribeAudio.AutoSize = true;
		this.lblTranscribeAudio.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTranscribeAudio.Location = new System.Drawing.Point(8, 8);
		this.lblTranscribeAudio.Name = "lblTranscribeAudio";
		this.lblTranscribeAudio.Size = new System.Drawing.Size(152, 20);
		this.lblTranscribeAudio.TabIndex = 0;
		this.lblTranscribeAudio.Text = "Transcribe Audio";
		this.lblLanguageCode.AutoSize = true;
		this.lblLanguageCode.Location = new System.Drawing.Point(9, 38);
		this.lblLanguageCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblLanguageCode.Name = "lblLanguageCode";
		this.lblLanguageCode.Size = new System.Drawing.Size(109, 17);
		this.lblLanguageCode.TabIndex = 1;
		this.lblLanguageCode.Text = "Language Code";
		this.comboLanguageCode.Location = new System.Drawing.Point(12, 59);
		this.comboLanguageCode.Margin = new System.Windows.Forms.Padding(4);
		this.comboLanguageCode.MaxLength = 1024;
		this.comboLanguageCode.Name = "comboLanguageCode";
		this.comboLanguageCode.Size = new System.Drawing.Size(300, 24);
		this.comboLanguageCode.TabIndex = 2;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.comboLanguageCode);
		base.Controls.Add(this.lblLanguageCode);
		base.Controls.Add(this.lblTranscribeAudio);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "OptionsComponentsTranscribeAudioControl";
		base.Size = new System.Drawing.Size(780, 665);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
