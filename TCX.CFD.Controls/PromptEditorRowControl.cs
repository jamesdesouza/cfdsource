using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;

namespace TCX.CFD.Controls;

public class PromptEditorRowControl : UserControl
{
	private Prompt prompt;

	private readonly AudioFilePrompt audioFilePrompt;

	private readonly DynamicAudioFilePrompt dynamicAudioFilePrompt;

	private readonly RecordedAudioPrompt recordedAudioPrompt;

	private readonly NumberPrompt numberPrompt;

	private readonly TextToSpeechAudioPrompt textToSpeechAudioPrompt;

	private AbsPromptEditorRowControl promptEditorRowControl;

	private IContainer components;

	private ComboBox comboAudioType;

	private CheckBox chkSelection;

	public bool IsChecked
	{
		get
		{
			return chkSelection.Checked;
		}
		set
		{
			chkSelection.Checked = value;
		}
	}

	public event EventHandler CheckedChanged;

	public event EventHandler OnPlaybackStarted;

	public event EventHandler OnPlaybackFinished;

	private void CreatePromptEditorRowControl()
	{
		if (promptEditorRowControl != null)
		{
			base.Controls.Remove(promptEditorRowControl);
		}
		promptEditorRowControl = prompt.CreatePromptEditorRowControl();
		promptEditorRowControl.Location = new Point(comboAudioType.Location.X + comboAudioType.Width + 6, 0);
		promptEditorRowControl.Size = new Size(base.Width - promptEditorRowControl.Location.X - 6, base.Height);
		promptEditorRowControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		promptEditorRowControl.OnPlaybackStarted += PromptEditorRowControl_OnPlaybackStarted;
		promptEditorRowControl.OnPlaybackFinished += PromptEditorRowControl_OnPlaybackFinished;
		base.Controls.Add(promptEditorRowControl);
	}

	private void PromptEditorRowControl_OnPlaybackStarted(object sender, EventArgs e)
	{
		this.OnPlaybackStarted?.Invoke(sender, e);
	}

	private void PromptEditorRowControl_OnPlaybackFinished(object sender, EventArgs e)
	{
		this.OnPlaybackFinished?.Invoke(sender, e);
	}

	public PromptEditorRowControl(IVadActivity activity, Prompt prompt)
	{
		InitializeComponent();
		this.prompt = prompt;
		this.prompt.ContainerActivity = activity;
		audioFilePrompt = new AudioFilePrompt();
		audioFilePrompt.ContainerActivity = activity;
		dynamicAudioFilePrompt = new DynamicAudioFilePrompt();
		dynamicAudioFilePrompt.ContainerActivity = activity;
		recordedAudioPrompt = new RecordedAudioPrompt();
		recordedAudioPrompt.ContainerActivity = activity;
		numberPrompt = new NumberPrompt();
		numberPrompt.ContainerActivity = activity;
		textToSpeechAudioPrompt = new TextToSpeechAudioPrompt();
		textToSpeechAudioPrompt.ContainerActivity = activity;
		comboAudioType.Items.AddRange(new object[5]
		{
			LocalizedResourceMgr.GetString("PromptEditorRowControl.comboAudioType.AudioFilePrompt"),
			LocalizedResourceMgr.GetString("PromptEditorRowControl.comboAudioType.DynamicAudioFilePrompt"),
			LocalizedResourceMgr.GetString("PromptEditorRowControl.comboAudioType.RecordedAudioPrompt"),
			LocalizedResourceMgr.GetString("PromptEditorRowControl.comboAudioType.NumberPrompt"),
			LocalizedResourceMgr.GetString("PromptEditorRowControl.comboAudioType.TextToSpeechAudioPrompt")
		});
		if (prompt is AudioFilePrompt)
		{
			audioFilePrompt = prompt as AudioFilePrompt;
			comboAudioType.SelectedIndex = 0;
		}
		else if (prompt is DynamicAudioFilePrompt)
		{
			dynamicAudioFilePrompt = prompt as DynamicAudioFilePrompt;
			comboAudioType.SelectedIndex = 1;
		}
		else if (prompt is RecordedAudioPrompt)
		{
			recordedAudioPrompt = prompt as RecordedAudioPrompt;
			comboAudioType.SelectedIndex = 2;
		}
		else if (prompt is NumberPrompt)
		{
			numberPrompt = prompt as NumberPrompt;
			comboAudioType.SelectedIndex = 3;
		}
		else
		{
			textToSpeechAudioPrompt = prompt as TextToSpeechAudioPrompt;
			comboAudioType.SelectedIndex = 4;
		}
	}

	private void ChkSelection_CheckedChanged(object sender, EventArgs e)
	{
		this.CheckedChanged?.Invoke(this, e);
	}

	private void ComboAudioType_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (promptEditorRowControl != null)
		{
			promptEditorRowControl.Save();
		}
		switch (comboAudioType.SelectedIndex)
		{
		case 0:
			prompt = audioFilePrompt;
			break;
		case 1:
			prompt = dynamicAudioFilePrompt;
			break;
		case 2:
			prompt = recordedAudioPrompt;
			break;
		case 3:
			prompt = numberPrompt;
			break;
		case 4:
			prompt = textToSpeechAudioPrompt;
			break;
		}
		CreatePromptEditorRowControl();
	}

	public void DisablePlayback(object playingControl)
	{
		promptEditorRowControl.DisablePlayback(playingControl);
	}

	public void EnablePlayback()
	{
		promptEditorRowControl.EnablePlayback();
	}

	public void StopPlayback()
	{
		promptEditorRowControl.StopPlayback();
	}

	public Prompt Save()
	{
		return promptEditorRowControl.Save();
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
		this.comboAudioType = new System.Windows.Forms.ComboBox();
		this.chkSelection = new System.Windows.Forms.CheckBox();
		base.SuspendLayout();
		this.comboAudioType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboAudioType.FormattingEnabled = true;
		this.comboAudioType.Location = new System.Drawing.Point(24, 23);
		this.comboAudioType.Name = "comboAudioType";
		this.comboAudioType.Size = new System.Drawing.Size(150, 21);
		this.comboAudioType.TabIndex = 1;
		this.comboAudioType.SelectedIndexChanged += new System.EventHandler(ComboAudioType_SelectedIndexChanged);
		this.chkSelection.AutoSize = true;
		this.chkSelection.Location = new System.Drawing.Point(3, 26);
		this.chkSelection.Name = "chkSelection";
		this.chkSelection.Size = new System.Drawing.Size(15, 14);
		this.chkSelection.TabIndex = 0;
		this.chkSelection.UseVisualStyleBackColor = true;
		this.chkSelection.CheckedChanged += new System.EventHandler(ChkSelection_CheckedChanged);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.chkSelection);
		base.Controls.Add(this.comboAudioType);
		base.Name = "PromptEditorRowControl";
		base.Size = new System.Drawing.Size(585, 49);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
