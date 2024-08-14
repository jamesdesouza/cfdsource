using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class OptionsUpdatesControl : UserControl, IOptionsControl
{
	private IContainer components;

	private CheckBox chkAutomaticallyCheckForUpdates;

	private Label lblUpdates;

	public OptionsUpdatesControl()
	{
		InitializeComponent();
		lblUpdates.Text = LocalizedResourceMgr.GetString("OptionsUpdatesControl.lblUpdates.Text");
		chkAutomaticallyCheckForUpdates.Checked = Settings.Default.AutoUpdatesEnabled;
		chkAutomaticallyCheckForUpdates.Text = LocalizedResourceMgr.GetString("OptionsUpdatesControl.chkAutomaticallyCheckForUpdates.Text");
	}

	public void Save()
	{
		Settings.Default.AutoUpdatesEnabled = chkAutomaticallyCheckForUpdates.Checked;
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
		this.chkAutomaticallyCheckForUpdates = new System.Windows.Forms.CheckBox();
		this.lblUpdates = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.chkAutomaticallyCheckForUpdates.AutoSize = true;
		this.chkAutomaticallyCheckForUpdates.Location = new System.Drawing.Point(12, 38);
		this.chkAutomaticallyCheckForUpdates.Margin = new System.Windows.Forms.Padding(4);
		this.chkAutomaticallyCheckForUpdates.Name = "chkAutomaticallyCheckForUpdates";
		this.chkAutomaticallyCheckForUpdates.Size = new System.Drawing.Size(230, 21);
		this.chkAutomaticallyCheckForUpdates.TabIndex = 0;
		this.chkAutomaticallyCheckForUpdates.Text = "Automatically check for updates";
		this.chkAutomaticallyCheckForUpdates.UseVisualStyleBackColor = true;
		this.lblUpdates.AutoSize = true;
		this.lblUpdates.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblUpdates.Location = new System.Drawing.Point(8, 8);
		this.lblUpdates.Name = "lblUpdates";
		this.lblUpdates.Size = new System.Drawing.Size(78, 20);
		this.lblUpdates.TabIndex = 1;
		this.lblUpdates.Text = "Updates";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.lblUpdates);
		base.Controls.Add(this.chkAutomaticallyCheckForUpdates);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "OptionsUpdatesControl";
		base.Size = new System.Drawing.Size(780, 665);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
