using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class OptionsComponentsPromptPlaybackControl : UserControl, IOptionsControl
{
	private IContainer components;

	private CheckBox chkAcceptDtmfInput;

	private Label lblPromptPlayback;

	public OptionsComponentsPromptPlaybackControl()
	{
		InitializeComponent();
		chkAcceptDtmfInput.Checked = Settings.Default.PromptPlaybackTemplateAcceptDtmfInput;
		lblPromptPlayback.Text = LocalizedResourceMgr.GetString("OptionsComponentsPromptPlaybackControl.lblPromptPlayback.Text");
		chkAcceptDtmfInput.Text = LocalizedResourceMgr.GetString("OptionsComponentsPromptPlaybackControl.chkAcceptDtmfInput.Text");
	}

	public void Save()
	{
		Settings.Default.PromptPlaybackTemplateAcceptDtmfInput = chkAcceptDtmfInput.Checked;
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
		this.chkAcceptDtmfInput = new System.Windows.Forms.CheckBox();
		this.lblPromptPlayback = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.chkAcceptDtmfInput.AutoSize = true;
		this.chkAcceptDtmfInput.Location = new System.Drawing.Point(12, 38);
		this.chkAcceptDtmfInput.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.chkAcceptDtmfInput.Name = "chkAcceptDtmfInput";
		this.chkAcceptDtmfInput.Size = new System.Drawing.Size(139, 21);
		this.chkAcceptDtmfInput.TabIndex = 1;
		this.chkAcceptDtmfInput.Text = "Accept DTMF Input During Prompt";
		this.chkAcceptDtmfInput.UseVisualStyleBackColor = true;
		this.lblPromptPlayback.AutoSize = true;
		this.lblPromptPlayback.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPromptPlayback.Location = new System.Drawing.Point(8, 8);
		this.lblPromptPlayback.Name = "lblPromptPlayback";
		this.lblPromptPlayback.Size = new System.Drawing.Size(150, 20);
		this.lblPromptPlayback.TabIndex = 0;
		this.lblPromptPlayback.Text = "Prompt Playback";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.lblPromptPlayback);
		base.Controls.Add(this.chkAcceptDtmfInput);
		base.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		base.Name = "OptionsComponentsPromptPlaybackControl";
		base.Size = new System.Drawing.Size(780, 665);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
