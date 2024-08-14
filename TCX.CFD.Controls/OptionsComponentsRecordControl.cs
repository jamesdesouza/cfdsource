using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class OptionsComponentsRecordControl : UserControl, IOptionsControl
{
	private IContainer components;

	private CheckBox chkBoxSaveToFile;

	private CheckBox chkBoxTerminateByDtmf;

	private MaskedTextBox txtMaxTime;

	private Label lblMaxTime;

	private CheckBox chkBeep;

	private ErrorProvider errorProvider;

	private Label lblRecord;

	public OptionsComponentsRecordControl()
	{
		InitializeComponent();
		chkBeep.Checked = Settings.Default.RecordTemplateBeep;
		txtMaxTime.Text = Settings.Default.RecordTemplateMaxTime.ToString();
		chkBoxTerminateByDtmf.Checked = Settings.Default.RecordTemplateTerminateByDtmf;
		chkBoxSaveToFile.Checked = Settings.Default.RecordTemplateSaveToFile;
		lblRecord.Text = LocalizedResourceMgr.GetString("OptionsComponentsRecordControl.lblRecord.Text");
		chkBeep.Text = LocalizedResourceMgr.GetString("OptionsComponentsRecordControl.chkBeep.Text");
		lblMaxTime.Text = LocalizedResourceMgr.GetString("OptionsComponentsRecordControl.lblMaxTime.Text");
		chkBoxTerminateByDtmf.Text = LocalizedResourceMgr.GetString("OptionsComponentsRecordControl.chkBoxTerminateByDtmf.Text");
		chkBoxSaveToFile.Text = LocalizedResourceMgr.GetString("OptionsComponentsRecordControl.chkBoxSaveToFile.Text");
	}

	private void MaskedTextBox_GotFocus(object sender, EventArgs e)
	{
		(sender as MaskedTextBox).SelectAll();
	}

	private void ValidateMaxTime()
	{
		if (string.IsNullOrEmpty(txtMaxTime.Text))
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("OptionsComponentsRecordControl.Error.MaxTimeIsMandatory"));
		}
		int num = Convert.ToInt32(txtMaxTime.Text);
		if (num < 1 || num > 99999)
		{
			throw new ApplicationException(string.Format(LocalizedResourceMgr.GetString("OptionsComponentsRecordControl.Error.InvalidMaxTimeValue"), 1, 99999));
		}
	}

	private void ValidateFields()
	{
		ValidateMaxTime();
	}

	private void TxtMaxTime_Validating(object sender, CancelEventArgs e)
	{
		try
		{
			ValidateMaxTime();
			errorProvider.SetError(txtMaxTime, string.Empty);
		}
		catch (Exception exc)
		{
			errorProvider.SetError(txtMaxTime, ErrorHelper.GetErrorDescription(exc));
		}
	}

	public void Save()
	{
		ValidateFields();
		Settings.Default.RecordTemplateBeep = chkBeep.Checked;
		Settings.Default.RecordTemplateMaxTime = Convert.ToUInt32(txtMaxTime.Text);
		Settings.Default.RecordTemplateTerminateByDtmf = chkBoxTerminateByDtmf.Checked;
		Settings.Default.RecordTemplateSaveToFile = chkBoxSaveToFile.Checked;
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
		this.chkBoxSaveToFile = new System.Windows.Forms.CheckBox();
		this.chkBoxTerminateByDtmf = new System.Windows.Forms.CheckBox();
		this.txtMaxTime = new System.Windows.Forms.MaskedTextBox();
		this.lblMaxTime = new System.Windows.Forms.Label();
		this.chkBeep = new System.Windows.Forms.CheckBox();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.lblRecord = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.chkBoxSaveToFile.AutoSize = true;
		this.chkBoxSaveToFile.Location = new System.Drawing.Point(12, 148);
		this.chkBoxSaveToFile.Margin = new System.Windows.Forms.Padding(4);
		this.chkBoxSaveToFile.Name = "chkBoxSaveToFile";
		this.chkBoxSaveToFile.Size = new System.Drawing.Size(100, 21);
		this.chkBoxSaveToFile.TabIndex = 5;
		this.chkBoxSaveToFile.Text = "Save to file";
		this.chkBoxSaveToFile.UseVisualStyleBackColor = true;
		this.chkBoxTerminateByDtmf.AutoSize = true;
		this.chkBoxTerminateByDtmf.Location = new System.Drawing.Point(12, 119);
		this.chkBoxTerminateByDtmf.Margin = new System.Windows.Forms.Padding(4);
		this.chkBoxTerminateByDtmf.Name = "chkBoxTerminateByDtmf";
		this.chkBoxTerminateByDtmf.Size = new System.Drawing.Size(269, 21);
		this.chkBoxTerminateByDtmf.TabIndex = 4;
		this.chkBoxTerminateByDtmf.Text = "Stop recording by pressing any DTMF";
		this.chkBoxTerminateByDtmf.UseVisualStyleBackColor = true;
		this.txtMaxTime.HidePromptOnLeave = true;
		this.txtMaxTime.Location = new System.Drawing.Point(12, 89);
		this.txtMaxTime.Margin = new System.Windows.Forms.Padding(4);
		this.txtMaxTime.Mask = "99999";
		this.txtMaxTime.Name = "txtMaxTime";
		this.txtMaxTime.Size = new System.Drawing.Size(300, 22);
		this.txtMaxTime.TabIndex = 3;
		this.txtMaxTime.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtMaxTime.Validating += new System.ComponentModel.CancelEventHandler(TxtMaxTime_Validating);
		this.lblMaxTime.AutoSize = true;
		this.lblMaxTime.Location = new System.Drawing.Point(9, 68);
		this.lblMaxTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblMaxTime.Name = "lblMaxTime";
		this.lblMaxTime.Size = new System.Drawing.Size(220, 17);
		this.lblMaxTime.TabIndex = 2;
		this.lblMaxTime.Text = "Max recording duration (seconds)";
		this.chkBeep.AutoSize = true;
		this.chkBeep.Location = new System.Drawing.Point(12, 38);
		this.chkBeep.Margin = new System.Windows.Forms.Padding(4);
		this.chkBeep.Name = "chkBeep";
		this.chkBeep.Size = new System.Drawing.Size(241, 21);
		this.chkBeep.TabIndex = 1;
		this.chkBeep.Text = "Play beep before recording starts";
		this.chkBeep.UseVisualStyleBackColor = true;
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.lblRecord.AutoSize = true;
		this.lblRecord.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblRecord.Location = new System.Drawing.Point(8, 8);
		this.lblRecord.Name = "lblRecord";
		this.lblRecord.Size = new System.Drawing.Size(69, 20);
		this.lblRecord.TabIndex = 0;
		this.lblRecord.Text = "Record";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.lblRecord);
		base.Controls.Add(this.chkBeep);
		base.Controls.Add(this.chkBoxSaveToFile);
		base.Controls.Add(this.chkBoxTerminateByDtmf);
		base.Controls.Add(this.txtMaxTime);
		base.Controls.Add(this.lblMaxTime);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "OptionsComponentsRecordControl";
		base.Size = new System.Drawing.Size(780, 665);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
