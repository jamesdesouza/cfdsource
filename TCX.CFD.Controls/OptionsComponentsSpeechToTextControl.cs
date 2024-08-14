using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.FileSystem;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class OptionsComponentsSpeechToTextControl : UserControl, IOptionsControl
{
	public delegate void GoogleCloudConfigurationChangedHandler(GoogleCloudSettings settings);

	private readonly OnlineServicesSpeechToTextControl speechToTextControl;

	private readonly GoogleCloudConfigurationControl googleCloudConfigurationControl;

	private IContainer components;

	private Panel engineSettingsPanel;

	private Panel engineSelectionPanel;

	public event GoogleCloudConfigurationChangedHandler GoogleCloudConfigurationChanged;

	public OptionsComponentsSpeechToTextControl()
	{
		InitializeComponent();
		speechToTextControl = new OnlineServicesSpeechToTextControl
		{
			Dock = DockStyle.Fill,
			Location = new Point(0, 0),
			Margin = new Padding(5)
		};
		speechToTextControl.EngineChanged += SpeechToTextControl_EngineChanged;
		engineSelectionPanel.Controls.Add(speechToTextControl);
		googleCloudConfigurationControl = new GoogleCloudConfigurationControl
		{
			Dock = DockStyle.Fill,
			Location = new Point(0, 0),
			Margin = new Padding(5),
			Visible = false
		};
		googleCloudConfigurationControl.ConfigurationChanged += GoogleCloudConfigurationControl_ConfigurationChanged;
		engineSettingsPanel.Controls.Add(googleCloudConfigurationControl);
		googleCloudConfigurationControl.GoogleCloudSettings = new GoogleCloudSettings
		{
			ServiceAccountKeyFileName = Settings.Default.GoogleCloudServiceAccountKeyFileName,
			ServiceAccountKeyJSON = Settings.Default.GoogleCloudServiceAccountKeyJSON
		};
		speechToTextControl.SpeechToTextEngine = EnumHelper.StringToSpeechToTextEngines(Settings.Default.OnlineServicesSpeechToTextEngine);
	}

	private void SpeechToTextControl_EngineChanged()
	{
		ShowEngineSettings(speechToTextControl.SpeechToTextEngine);
	}

	private void GoogleCloudConfigurationControl_ConfigurationChanged()
	{
		this.GoogleCloudConfigurationChanged?.Invoke(googleCloudConfigurationControl.GoogleCloudSettings);
	}

	private void ShowEngineSettings(SpeechToTextEngines engine)
	{
		googleCloudConfigurationControl.Visible = engine == SpeechToTextEngines.GoogleCloud;
	}

	public void UpdateGoogleCloudConfiguration(GoogleCloudSettings settings)
	{
		googleCloudConfigurationControl.GoogleCloudSettings = settings;
	}

	private void ValidateFields()
	{
		if (speechToTextControl.SpeechToTextEngine == SpeechToTextEngines.GoogleCloud && !googleCloudConfigurationControl.ValidateSettings())
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsSpeechToTextControl.Error.GoogleCloudConfiguration"));
		}
	}

	public void Save()
	{
		Settings.Default.OnlineServicesSpeechToTextEngine = EnumHelper.SpeechToTextEnginesToString(speechToTextControl.SpeechToTextEngine);
		if (speechToTextControl.SpeechToTextEngine == SpeechToTextEngines.GoogleCloud)
		{
			GoogleCloudSettings googleCloudSettings = googleCloudConfigurationControl.GoogleCloudSettings;
			Settings.Default.GoogleCloudServiceAccountKeyFileName = googleCloudSettings.ServiceAccountKeyFileName;
			Settings.Default.GoogleCloudServiceAccountKeyJSON = googleCloudSettings.ServiceAccountKeyJSON;
		}
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
		this.engineSettingsPanel = new System.Windows.Forms.Panel();
		this.engineSelectionPanel = new System.Windows.Forms.Panel();
		base.SuspendLayout();
		this.engineSettingsPanel.Location = new System.Drawing.Point(3, 75);
		this.engineSettingsPanel.Name = "engineSettingsPanel";
		this.engineSettingsPanel.Size = new System.Drawing.Size(774, 99);
		this.engineSettingsPanel.TabIndex = 1;
		this.engineSelectionPanel.Location = new System.Drawing.Point(3, 3);
		this.engineSelectionPanel.Name = "engineSelectionPanel";
		this.engineSelectionPanel.Size = new System.Drawing.Size(774, 68);
		this.engineSelectionPanel.TabIndex = 0;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.engineSettingsPanel);
		base.Controls.Add(this.engineSelectionPanel);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "OptionsComponentsSpeechToTextControl";
		base.Size = new System.Drawing.Size(780, 665);
		base.ResumeLayout(false);
	}
}
