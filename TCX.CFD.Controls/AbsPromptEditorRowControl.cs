using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes.Components;

namespace TCX.CFD.Controls;

public class AbsPromptEditorRowControl : UserControl
{
	private IContainer components;

	public event EventHandler OnPlaybackStarted;

	public event EventHandler OnPlaybackFinished;

	protected void NotifyOnPlaybackStarted()
	{
		this.OnPlaybackStarted?.Invoke(this, EventArgs.Empty);
	}

	protected void NotifyOnPlaybackFinished()
	{
		this.OnPlaybackFinished?.Invoke(this, EventArgs.Empty);
	}

	public AbsPromptEditorRowControl()
	{
		InitializeComponent();
	}

	public virtual void DisablePlayback(object playingControl)
	{
	}

	public virtual void EnablePlayback()
	{
	}

	public virtual void StopPlayback()
	{
	}

	public virtual Prompt Save()
	{
		throw new NotImplementedException("Please implement Save method.");
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
		base.SuspendLayout();
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "AbsPromptEditorRowControl";
		base.Size = new System.Drawing.Size(418, 27);
		base.ResumeLayout(false);
	}
}
