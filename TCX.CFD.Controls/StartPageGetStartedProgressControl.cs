using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;

namespace TCX.CFD.Controls;

public class StartPageGetStartedProgressControl : UserControl
{
	private IContainer components;

	private Label lblDownloadingFeed;

	public StartPageGetStartedProgressControl()
	{
		InitializeComponent();
		lblDownloadingFeed.Text = LocalizedResourceMgr.GetString("StartPageControl.lblDownloadingFeed.Text");
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
		this.lblDownloadingFeed = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.lblDownloadingFeed.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblDownloadingFeed.Font = new System.Drawing.Font("Verdana", 10f);
		this.lblDownloadingFeed.Location = new System.Drawing.Point(0, 0);
		this.lblDownloadingFeed.Name = "lblDownloadingFeed";
		this.lblDownloadingFeed.Size = new System.Drawing.Size(660, 339);
		this.lblDownloadingFeed.TabIndex = 1;
		this.lblDownloadingFeed.Text = "Downloading latest articles and videos, please wait...";
		this.lblDownloadingFeed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.lblDownloadingFeed);
		base.Name = "StartPageGetStartedProgressControl";
		base.Size = new System.Drawing.Size(660, 339);
		base.ResumeLayout(false);
	}
}
