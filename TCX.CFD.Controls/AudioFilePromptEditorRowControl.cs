using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Media;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.FileSystem;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class AudioFilePromptEditorRowControl : AbsPromptEditorRowControl
{
	private readonly AudioFilePrompt prompt;

	private readonly ProjectObject projectObject;

	private bool isPlayingPrompt;

	private long wavFileDurationMillis;

	private DateTime playStartDt = DateTime.MinValue;

	private readonly SoundPlayer soundPlayer = new SoundPlayer();

	private IContainer components;

	private ComboBox comboAudioFiles;

	private Button browseButton;

	private Label lblAudioFile;

	private Button playStopButton;

	private ProgressBar promptPlaybackProgressBar;

	private Timer playTimer;

	private void FillAudioFileCombo(string selectedFileName)
	{
		comboAudioFiles.Items.Clear();
		DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(projectObject.GetFolderPath(), "Audio"));
		if (directoryInfo.Exists)
		{
			FileInfo[] files = directoryInfo.GetFiles("*.wav", SearchOption.TopDirectoryOnly);
			foreach (FileInfo fileInfo in files)
			{
				comboAudioFiles.Items.Add(fileInfo.Name);
			}
		}
		int num = comboAudioFiles.Items.IndexOf(selectedFileName);
		if (num != -1)
		{
			comboAudioFiles.SelectedIndex = num;
		}
	}

	public AudioFilePromptEditorRowControl(IVadActivity activity, AudioFilePrompt prompt)
	{
		InitializeComponent();
		this.prompt = prompt;
		projectObject = activity.GetRootFlow().FileObject.GetProjectObject();
		lblAudioFile.Text = LocalizedResourceMgr.GetString("AudioFilePromptEditorRowControl.lblAudioFile.Text");
		browseButton.Text = LocalizedResourceMgr.GetString("AudioFilePromptEditorRowControl.browseButton.Text");
		FillAudioFileCombo(prompt.AudioFileName);
	}

	private void ComboAudioFiles_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (comboAudioFiles.SelectedIndex == -1)
		{
			playStopButton.Enabled = false;
			soundPlayer.SoundLocation = string.Empty;
			return;
		}
		string text = Path.Combine(projectObject.GetFolderPath(), "Audio", comboAudioFiles.Items[comboAudioFiles.SelectedIndex].ToString());
		FileInfo fileInfo = new FileInfo(text);
		using (FileStream wavFileStream = fileInfo.OpenRead())
		{
			WavFileFormat wavFileFormat = WavFileFormatReader.GetWavFileFormat(wavFileStream);
			wavFileDurationMillis = 1000 * fileInfo.Length / (wavFileFormat.Channels * wavFileFormat.SampleRate * wavFileFormat.BitsPerSample / 8);
		}
		soundPlayer.SoundLocation = text;
		soundPlayer.Load();
		playStopButton.Enabled = true;
	}

	private void BrowseButton_Click(object sender, EventArgs e)
	{
		OpenFileDialog openFileDialog = new OpenFileDialog();
		openFileDialog.DefaultExt = "wav";
		openFileDialog.Filter = "WAV Files (*.wav)|*.wav";
		openFileDialog.RestoreDirectory = true;
		openFileDialog.SupportMultiDottedExtensions = true;
		openFileDialog.Title = "Open WAV File";
		openFileDialog.FileName = string.Empty;
		if (openFileDialog.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		try
		{
			FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
			string text = Path.Combine(projectObject.GetFolderPath(), "Audio", fileInfo.Name);
			if (Path.GetFullPath(text).ToLower() != Path.GetFullPath(openFileDialog.FileName).ToLower())
			{
				File.Copy(openFileDialog.FileName, text, overwrite: true);
			}
			FillAudioFileCombo(fileInfo.Name);
		}
		catch (Exception exc)
		{
			MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("AudioFilePromptEditorRowControl.MessageBox.Error.CopyingFile"), ErrorHelper.GetErrorDescription(exc)), LocalizedResourceMgr.GetString("AudioFilePromptEditorRowControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void PlayStopButton_Click(object sender, EventArgs e)
	{
		if (isPlayingPrompt)
		{
			soundPlayer.Stop();
			isPlayingPrompt = false;
			lblAudioFile.Text = LocalizedResourceMgr.GetString("AudioFilePromptEditorRowControl.lblAudioFile.Text");
			promptPlaybackProgressBar.Visible = false;
			comboAudioFiles.Visible = true;
			browseButton.Visible = true;
			playStopButton.Image = Resources.Prompt_Play;
			NotifyOnPlaybackFinished();
		}
		else
		{
			NotifyOnPlaybackStarted();
			isPlayingPrompt = true;
			playStartDt = DateTime.Now;
			lblAudioFile.Text = LocalizedResourceMgr.GetString("AudioFilePromptEditorRowControl.lblAudioFile.Playing.Text");
			comboAudioFiles.Visible = false;
			browseButton.Visible = false;
			promptPlaybackProgressBar.Visible = true;
			promptPlaybackProgressBar.Value = 0;
			playStopButton.Image = Resources.Prompt_Stop;
			soundPlayer.Play();
			playTimer.Start();
		}
	}

	private void PlayTimer_Tick(object sender, EventArgs e)
	{
		TimeSpan timeSpan = DateTime.Now - playStartDt;
		int num = Convert.ToInt32(100.0 * timeSpan.TotalMilliseconds / (double)wavFileDurationMillis);
		promptPlaybackProgressBar.Value = Math.Min(100, num + 1);
		promptPlaybackProgressBar.Value = Math.Min(100, num);
		if (timeSpan.TotalMilliseconds > (double)wavFileDurationMillis)
		{
			StopPlayback();
		}
	}

	public override void DisablePlayback(object playingControl)
	{
		if (playingControl != this)
		{
			playStopButton.Enabled = false;
		}
	}

	public override void EnablePlayback()
	{
		playStopButton.Enabled = comboAudioFiles.SelectedIndex != -1;
	}

	public override void StopPlayback()
	{
		if (isPlayingPrompt)
		{
			playStopButton.PerformClick();
			playTimer.Stop();
		}
	}

	public override Prompt Save()
	{
		prompt.AudioFileName = ((comboAudioFiles.SelectedIndex == -1) ? string.Empty : comboAudioFiles.SelectedItem.ToString());
		return prompt;
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
		this.comboAudioFiles = new System.Windows.Forms.ComboBox();
		this.browseButton = new System.Windows.Forms.Button();
		this.lblAudioFile = new System.Windows.Forms.Label();
		this.playStopButton = new System.Windows.Forms.Button();
		this.promptPlaybackProgressBar = new System.Windows.Forms.ProgressBar();
		this.playTimer = new System.Windows.Forms.Timer(this.components);
		base.SuspendLayout();
		this.comboAudioFiles.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboAudioFiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboAudioFiles.FormattingEnabled = true;
		this.comboAudioFiles.Location = new System.Drawing.Point(8, 28);
		this.comboAudioFiles.Margin = new System.Windows.Forms.Padding(4);
		this.comboAudioFiles.Name = "comboAudioFiles";
		this.comboAudioFiles.Size = new System.Drawing.Size(404, 24);
		this.comboAudioFiles.Sorted = true;
		this.comboAudioFiles.TabIndex = 1;
		this.comboAudioFiles.SelectedIndexChanged += new System.EventHandler(ComboAudioFiles_SelectedIndexChanged);
		this.browseButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.browseButton.Location = new System.Drawing.Point(420, 26);
		this.browseButton.Margin = new System.Windows.Forms.Padding(4);
		this.browseButton.Name = "browseButton";
		this.browseButton.Size = new System.Drawing.Size(86, 28);
		this.browseButton.TabIndex = 2;
		this.browseButton.Text = "Browse";
		this.browseButton.UseVisualStyleBackColor = true;
		this.browseButton.Click += new System.EventHandler(BrowseButton_Click);
		this.lblAudioFile.AutoSize = true;
		this.lblAudioFile.Location = new System.Drawing.Point(4, 9);
		this.lblAudioFile.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblAudioFile.Name = "lblAudioFile";
		this.lblAudioFile.Size = new System.Drawing.Size(113, 17);
		this.lblAudioFile.TabIndex = 0;
		this.lblAudioFile.Text = "Select Audio File";
		this.playStopButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.playStopButton.Enabled = false;
		this.playStopButton.Image = TCX.CFD.Properties.Resources.Prompt_Play;
		this.playStopButton.Location = new System.Drawing.Point(514, 26);
		this.playStopButton.Margin = new System.Windows.Forms.Padding(4);
		this.playStopButton.Name = "playStopButton";
		this.playStopButton.Size = new System.Drawing.Size(39, 28);
		this.playStopButton.TabIndex = 3;
		this.playStopButton.UseVisualStyleBackColor = true;
		this.playStopButton.Click += new System.EventHandler(PlayStopButton_Click);
		this.promptPlaybackProgressBar.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.promptPlaybackProgressBar.Location = new System.Drawing.Point(7, 28);
		this.promptPlaybackProgressBar.Name = "promptPlaybackProgressBar";
		this.promptPlaybackProgressBar.Size = new System.Drawing.Size(499, 26);
		this.promptPlaybackProgressBar.TabIndex = 4;
		this.promptPlaybackProgressBar.Visible = false;
		this.playTimer.Interval = 50;
		this.playTimer.Tick += new System.EventHandler(PlayTimer_Tick);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.comboAudioFiles);
		base.Controls.Add(this.promptPlaybackProgressBar);
		base.Controls.Add(this.playStopButton);
		base.Controls.Add(this.lblAudioFile);
		base.Controls.Add(this.browseButton);
		base.Margin = new System.Windows.Forms.Padding(5);
		base.Name = "AudioFilePromptEditorRowControl";
		base.Size = new System.Drawing.Size(557, 60);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
