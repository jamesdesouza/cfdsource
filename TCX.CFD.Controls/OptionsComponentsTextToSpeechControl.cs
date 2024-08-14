using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.FileSystem;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class OptionsComponentsTextToSpeechControl : UserControl, IOptionsControl
{
	public delegate void GoogleCloudConfigurationChangedHandler(GoogleCloudSettings settings);

	private readonly OnlineServicesTextToSpeechControl textToSpeechControl;

	private readonly AmazonPollyConfigurationControl amazonPollyConfigurationControl;

	private readonly GoogleCloudConfigurationControl googleCloudConfigurationControl;

	private readonly Point amazonPollyControlLocationLblVoice;

	private readonly Point amazonPollyControlLocationLblType;

	private readonly Point amazonPollyControlLocationComboVoices;

	private readonly Point amazonPollyControlLocationComboFormat;

	private const int SHIFT = 90;

	private IContainer components;

	private Label lblVoice;

	private ComboBox comboVoices;

	private ComboBox comboFormat;

	private Label lblType;

	private Panel engineSettingsPanel;

	private Panel engineSelectionPanel;

	public event GoogleCloudConfigurationChangedHandler GoogleCloudConfigurationChanged;

	public OptionsComponentsTextToSpeechControl()
	{
		InitializeComponent();
		amazonPollyControlLocationLblVoice = lblVoice.Location;
		amazonPollyControlLocationLblType = lblType.Location;
		amazonPollyControlLocationComboVoices = comboVoices.Location;
		amazonPollyControlLocationComboFormat = comboFormat.Location;
		textToSpeechControl = new OnlineServicesTextToSpeechControl
		{
			Dock = DockStyle.Fill,
			Location = new Point(0, 0),
			Margin = new Padding(5)
		};
		textToSpeechControl.EngineChanged += TextToSpeechControl_EngineChanged;
		engineSelectionPanel.Controls.Add(textToSpeechControl);
		amazonPollyConfigurationControl = new AmazonPollyConfigurationControl
		{
			Dock = DockStyle.Fill,
			Location = new Point(0, 0),
			Margin = new Padding(5),
			Visible = false
		};
		engineSettingsPanel.Controls.Add(amazonPollyConfigurationControl);
		googleCloudConfigurationControl = new GoogleCloudConfigurationControl
		{
			Dock = DockStyle.Fill,
			Location = new Point(0, 0),
			Margin = new Padding(5),
			Visible = false
		};
		googleCloudConfigurationControl.ConfigurationChanged += GoogleCloudConfigurationControl_ConfigurationChanged;
		engineSettingsPanel.Controls.Add(googleCloudConfigurationControl);
		amazonPollyConfigurationControl.AmazonPollySettings = new AmazonPollySettings
		{
			ClientID = Settings.Default.AmazonPollyClientID,
			ClientSecret = Settings.Default.AmazonPollyClientSecret,
			Region = EnumHelper.StringToTextToSpeechAmazonRegion(Settings.Default.AmazonRegion),
			Lexicons = new List<string>(Settings.Default.AmazonPollyLexicons.Split(new string[1] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
		};
		googleCloudConfigurationControl.GoogleCloudSettings = new GoogleCloudSettings
		{
			ServiceAccountKeyFileName = Settings.Default.GoogleCloudServiceAccountKeyFileName,
			ServiceAccountKeyJSON = Settings.Default.GoogleCloudServiceAccountKeyJSON
		};
		textToSpeechControl.TextToSpeechEngine = EnumHelper.StringToTextToSpeechEngines(Settings.Default.OnlineServicesTextToSpeechEngine);
		comboFormat.Items.AddRange(new object[2]
		{
			TextToSpeechFormats.Text,
			TextToSpeechFormats.SSML
		});
		comboFormat.SelectedItem = EnumHelper.StringToTextToSpeechFormat(Settings.Default.DefaultTtsFormat);
		lblVoice.Text = LocalizedResourceMgr.GetString("OptionsComponentsTextToSpeechControl.lblVoice.Text");
		lblType.Text = LocalizedResourceMgr.GetString("OptionsComponentsTextToSpeechControl.lblType.Text");
	}

	private void TextToSpeechControl_EngineChanged()
	{
		ShowEngineSettings(textToSpeechControl.TextToSpeechEngine);
		TextToSpeechHelper.FillVoicesCombo(comboVoices, Settings.Default.DefaultTtsVoiceName, EnumHelper.StringToTextToSpeechVoiceType(Settings.Default.DefaultTtsVoiceType), textToSpeechControl.TextToSpeechEngine);
	}

	private void GoogleCloudConfigurationControl_ConfigurationChanged()
	{
		this.GoogleCloudConfigurationChanged?.Invoke(googleCloudConfigurationControl.GoogleCloudSettings);
	}

	private void ShowEngineSettings(TextToSpeechEngines engine)
	{
		amazonPollyConfigurationControl.Visible = engine == TextToSpeechEngines.AmazonPolly;
		googleCloudConfigurationControl.Visible = engine == TextToSpeechEngines.GoogleCloud;
		lblType.Visible = engine != TextToSpeechEngines.None;
		comboFormat.Visible = engine != TextToSpeechEngines.None;
		lblVoice.Visible = engine != TextToSpeechEngines.None;
		comboVoices.Visible = engine != TextToSpeechEngines.None;
		switch (engine)
		{
		case TextToSpeechEngines.AmazonPolly:
			lblVoice.Location = amazonPollyControlLocationLblVoice;
			lblType.Location = amazonPollyControlLocationLblType;
			comboVoices.Location = amazonPollyControlLocationComboVoices;
			comboFormat.Location = amazonPollyControlLocationComboFormat;
			break;
		case TextToSpeechEngines.GoogleCloud:
			lblVoice.Location = new Point(amazonPollyControlLocationLblVoice.X, amazonPollyControlLocationLblVoice.Y - 90);
			lblType.Location = new Point(amazonPollyControlLocationLblType.X, amazonPollyControlLocationLblType.Y - 90);
			comboVoices.Location = new Point(amazonPollyControlLocationComboVoices.X, amazonPollyControlLocationComboVoices.Y - 90);
			comboFormat.Location = new Point(amazonPollyControlLocationComboFormat.X, amazonPollyControlLocationComboFormat.Y - 90);
			break;
		}
	}

	public void UpdateGoogleCloudConfiguration(GoogleCloudSettings settings)
	{
		googleCloudConfigurationControl.GoogleCloudSettings = settings;
	}

	private void ValidateFields()
	{
		if (textToSpeechControl.TextToSpeechEngine == TextToSpeechEngines.AmazonPolly && !amazonPollyConfigurationControl.ValidateSettings())
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsTextToSpeechControl.Error.AmazonPollyConfiguration"));
		}
		if (textToSpeechControl.TextToSpeechEngine == TextToSpeechEngines.GoogleCloud && !googleCloudConfigurationControl.ValidateSettings())
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsTextToSpeechControl.Error.GoogleCloudConfiguration"));
		}
	}

	public void Save()
	{
		ValidateFields();
		Settings.Default.OnlineServicesTextToSpeechEngine = EnumHelper.TextToSpeechEnginesToString(textToSpeechControl.TextToSpeechEngine);
		if (textToSpeechControl.TextToSpeechEngine == TextToSpeechEngines.AmazonPolly)
		{
			AmazonPollySettings amazonPollySettings = amazonPollyConfigurationControl.AmazonPollySettings;
			Settings.Default.AmazonPollyClientID = amazonPollySettings.ClientID;
			Settings.Default.AmazonPollyClientSecret = amazonPollySettings.ClientSecret;
			Settings.Default.AmazonRegion = EnumHelper.TextToSpeechAmazonRegionToString(amazonPollySettings.Region);
			Settings.Default.AmazonPollyLexicons = string.Join(Environment.NewLine, amazonPollySettings.Lexicons);
		}
		else if (textToSpeechControl.TextToSpeechEngine == TextToSpeechEngines.GoogleCloud)
		{
			GoogleCloudSettings googleCloudSettings = googleCloudConfigurationControl.GoogleCloudSettings;
			Settings.Default.GoogleCloudServiceAccountKeyFileName = googleCloudSettings.ServiceAccountKeyFileName;
			Settings.Default.GoogleCloudServiceAccountKeyJSON = googleCloudSettings.ServiceAccountKeyJSON;
		}
		string text = ((comboVoices.SelectedIndex == -1) ? comboVoices.Text : (comboVoices.SelectedItem as string));
		string defaultTtsVoiceType = (text.Contains(" - Neural") ? "Neural" : (text.Contains("Wavenet") ? "Wavenet" : "Standard"));
		int num = text.IndexOf('(');
		if (num != -1)
		{
			text = text.Substring(0, num).Trim();
		}
		Settings.Default.DefaultTtsVoiceName = text;
		Settings.Default.DefaultTtsVoiceType = defaultTtsVoiceType;
		Settings.Default.DefaultTtsFormat = EnumHelper.TextToSpeechFormatToString((TextToSpeechFormats)comboFormat.SelectedItem);
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
		this.lblVoice = new System.Windows.Forms.Label();
		this.comboVoices = new System.Windows.Forms.ComboBox();
		this.comboFormat = new System.Windows.Forms.ComboBox();
		this.lblType = new System.Windows.Forms.Label();
		this.engineSettingsPanel = new System.Windows.Forms.Panel();
		this.engineSelectionPanel = new System.Windows.Forms.Panel();
		base.SuspendLayout();
		this.lblVoice.AutoSize = true;
		this.lblVoice.Location = new System.Drawing.Point(12, 252);
		this.lblVoice.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblVoice.Name = "lblVoice";
		this.lblVoice.Size = new System.Drawing.Size(43, 17);
		this.lblVoice.TabIndex = 2;
		this.lblVoice.Text = "Voice";
		this.comboVoices.FormattingEnabled = true;
		this.comboVoices.Location = new System.Drawing.Point(119, 249);
		this.comboVoices.Margin = new System.Windows.Forms.Padding(4);
		this.comboVoices.Name = "comboVoices";
		this.comboVoices.Size = new System.Drawing.Size(633, 24);
		this.comboVoices.TabIndex = 3;
		this.comboFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboFormat.FormattingEnabled = true;
		this.comboFormat.Location = new System.Drawing.Point(119, 281);
		this.comboFormat.Margin = new System.Windows.Forms.Padding(4);
		this.comboFormat.Name = "comboFormat";
		this.comboFormat.Size = new System.Drawing.Size(633, 24);
		this.comboFormat.TabIndex = 5;
		this.lblType.AutoSize = true;
		this.lblType.Location = new System.Drawing.Point(12, 284);
		this.lblType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblType.Name = "lblType";
		this.lblType.Size = new System.Drawing.Size(40, 17);
		this.lblType.TabIndex = 4;
		this.lblType.Text = "Type";
		this.engineSettingsPanel.Location = new System.Drawing.Point(3, 75);
		this.engineSettingsPanel.Name = "engineSettingsPanel";
		this.engineSettingsPanel.Size = new System.Drawing.Size(774, 169);
		this.engineSettingsPanel.TabIndex = 1;
		this.engineSelectionPanel.Location = new System.Drawing.Point(3, 3);
		this.engineSelectionPanel.Name = "engineSelectionPanel";
		this.engineSelectionPanel.Size = new System.Drawing.Size(774, 68);
		this.engineSelectionPanel.TabIndex = 0;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.comboFormat);
		base.Controls.Add(this.lblType);
		base.Controls.Add(this.comboVoices);
		base.Controls.Add(this.lblVoice);
		base.Controls.Add(this.engineSettingsPanel);
		base.Controls.Add(this.engineSelectionPanel);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "OptionsComponentsTextToSpeechControl";
		base.Size = new System.Drawing.Size(780, 665);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
