using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;

namespace TCX.CFD.Controls;

public class OnlineServicesSpeechToTextControl : UserControl
{
	public delegate void EngineChangedHandler();

	private IContainer components;

	private Label lblSpeechToText;

	private ComboBox comboOnlineService;

	private Label lblOnlineService;

	public SpeechToTextEngines SpeechToTextEngine
	{
		get
		{
			if (comboOnlineService.SelectedIndex == 0)
			{
				return SpeechToTextEngines.None;
			}
			return SpeechToTextEngines.GoogleCloud;
		}
		set
		{
			if (value == SpeechToTextEngines.GoogleCloud)
			{
				comboOnlineService.SelectedIndex = 1;
			}
			else
			{
				comboOnlineService.SelectedIndex = 0;
			}
		}
	}

	public event EngineChangedHandler EngineChanged;

	public OnlineServicesSpeechToTextControl()
	{
		InitializeComponent();
		lblSpeechToText.Text = LocalizedResourceMgr.GetString("OnlineServicesSpeechToTextControl.lblSpeechToText.Text");
		lblOnlineService.Text = LocalizedResourceMgr.GetString("OnlineServicesSpeechToTextControl.lblOnlineService.Text");
		comboOnlineService.Items.Add(LocalizedResourceMgr.GetString("OnlineServicesSpeechToTextControl.OnlineService.None"));
		comboOnlineService.Items.Add(LocalizedResourceMgr.GetString("OnlineServicesSpeechToTextControl.OnlineService.GoogleCloud"));
	}

	private void ComboOnlineService_SelectedIndexChanged(object sender, EventArgs e)
	{
		this.EngineChanged?.Invoke();
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
		this.lblSpeechToText = new System.Windows.Forms.Label();
		this.comboOnlineService = new System.Windows.Forms.ComboBox();
		this.lblOnlineService = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.lblSpeechToText.AutoSize = true;
		this.lblSpeechToText.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSpeechToText.Location = new System.Drawing.Point(8, 8);
		this.lblSpeechToText.Name = "lblSpeechToText";
		this.lblSpeechToText.Size = new System.Drawing.Size(140, 20);
		this.lblSpeechToText.TabIndex = 4;
		this.lblSpeechToText.Text = "Speech To Text";
		this.comboOnlineService.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboOnlineService.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboOnlineService.FormattingEnabled = true;
		this.comboOnlineService.Location = new System.Drawing.Point(116, 42);
		this.comboOnlineService.Margin = new System.Windows.Forms.Padding(4);
		this.comboOnlineService.Name = "comboOnlineService";
		this.comboOnlineService.Size = new System.Drawing.Size(317, 24);
		this.comboOnlineService.TabIndex = 6;
		this.comboOnlineService.SelectedIndexChanged += new System.EventHandler(ComboOnlineService_SelectedIndexChanged);
		this.lblOnlineService.AutoSize = true;
		this.lblOnlineService.Location = new System.Drawing.Point(9, 45);
		this.lblOnlineService.Name = "lblOnlineService";
		this.lblOnlineService.Size = new System.Drawing.Size(100, 17);
		this.lblOnlineService.TabIndex = 5;
		this.lblOnlineService.Text = "Online Service";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.lblSpeechToText);
		base.Controls.Add(this.comboOnlineService);
		base.Controls.Add(this.lblOnlineService);
		base.Name = "OnlineServicesSpeechToTextControl";
		base.Size = new System.Drawing.Size(457, 68);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
