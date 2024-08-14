using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;

namespace TCX.CFD.Controls;

public class OnlineServicesTextToSpeechControl : UserControl
{
	public delegate void EngineChangedHandler();

	private IContainer components;

	private ComboBox comboOnlineService;

	private Label lblOnlineService;

	private Label lblTextToSpeech;

	public TextToSpeechEngines TextToSpeechEngine
	{
		get
		{
			if (comboOnlineService.SelectedIndex == 0)
			{
				return TextToSpeechEngines.None;
			}
			if (comboOnlineService.SelectedIndex == 1)
			{
				return TextToSpeechEngines.AmazonPolly;
			}
			return TextToSpeechEngines.GoogleCloud;
		}
		set
		{
			switch (value)
			{
			case TextToSpeechEngines.AmazonPolly:
				comboOnlineService.SelectedIndex = 1;
				break;
			case TextToSpeechEngines.GoogleCloud:
				comboOnlineService.SelectedIndex = 2;
				break;
			default:
				comboOnlineService.SelectedIndex = 0;
				break;
			}
		}
	}

	public event EngineChangedHandler EngineChanged;

	public OnlineServicesTextToSpeechControl()
	{
		InitializeComponent();
		lblTextToSpeech.Text = LocalizedResourceMgr.GetString("OnlineServicesTextToSpeechControl.lblTextToSpeech.Text");
		lblOnlineService.Text = LocalizedResourceMgr.GetString("OnlineServicesTextToSpeechControl.lblOnlineService.Text");
		comboOnlineService.Items.Add(LocalizedResourceMgr.GetString("OnlineServicesTextToSpeechControl.OnlineService.None"));
		comboOnlineService.Items.Add(LocalizedResourceMgr.GetString("OnlineServicesTextToSpeechControl.OnlineService.AmazonPolly"));
		comboOnlineService.Items.Add(LocalizedResourceMgr.GetString("OnlineServicesTextToSpeechControl.OnlineService.GoogleCloud"));
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
		this.comboOnlineService = new System.Windows.Forms.ComboBox();
		this.lblOnlineService = new System.Windows.Forms.Label();
		this.lblTextToSpeech = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.comboOnlineService.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboOnlineService.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboOnlineService.FormattingEnabled = true;
		this.comboOnlineService.Location = new System.Drawing.Point(116, 42);
		this.comboOnlineService.Margin = new System.Windows.Forms.Padding(4);
		this.comboOnlineService.Name = "comboOnlineService";
		this.comboOnlineService.Size = new System.Drawing.Size(317, 24);
		this.comboOnlineService.TabIndex = 2;
		this.comboOnlineService.SelectedIndexChanged += new System.EventHandler(ComboOnlineService_SelectedIndexChanged);
		this.lblOnlineService.AutoSize = true;
		this.lblOnlineService.Location = new System.Drawing.Point(9, 45);
		this.lblOnlineService.Name = "lblOnlineService";
		this.lblOnlineService.Size = new System.Drawing.Size(100, 17);
		this.lblOnlineService.TabIndex = 1;
		this.lblOnlineService.Text = "Online Service";
		this.lblTextToSpeech.AutoSize = true;
		this.lblTextToSpeech.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTextToSpeech.Location = new System.Drawing.Point(8, 8);
		this.lblTextToSpeech.Name = "lblTextToSpeech";
		this.lblTextToSpeech.Size = new System.Drawing.Size(140, 20);
		this.lblTextToSpeech.TabIndex = 0;
		this.lblTextToSpeech.Text = "Text To Speech";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.lblTextToSpeech);
		base.Controls.Add(this.comboOnlineService);
		base.Controls.Add(this.lblOnlineService);
		base.Name = "OnlineServicesTextToSpeechControl";
		base.Size = new System.Drawing.Size(457, 68);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
