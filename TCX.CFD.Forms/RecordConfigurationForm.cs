using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Properties;

namespace TCX.CFD.Forms;

public class RecordConfigurationForm : Form
{
	private readonly RecordComponent recordComponent;

	private readonly List<string> validVariables;

	private IContainer components;

	private Label lblMaxTime;

	private MaskedTextBox txtMaxTime;

	private TextBox txtFileName;

	private Label lblFileName;

	private Button cancelButton;

	private Button okButton;

	private CheckBox chkBoxTerminateByDtmf;

	private Label lblSaveToFile;

	private Button saveToFileExpressionButton;

	private TextBox txtSaveToFile;

	private Button filenameExpressionButton;

	private CheckBox chkBeep;

	private ErrorProvider errorProvider;

	private Button editPromptsButton;

	private Label lblPrompts;

	public List<Prompt> Prompts { get; private set; }

	public bool Beep => chkBeep.Checked;

	public uint MaxTime
	{
		get
		{
			if (!string.IsNullOrEmpty(txtMaxTime.Text))
			{
				return Convert.ToUInt32(txtMaxTime.Text);
			}
			return Settings.Default.RecordTemplateMaxTime;
		}
	}

	public bool TerminateByDtmf => chkBoxTerminateByDtmf.Checked;

	public string SaveToFile => txtSaveToFile.Text;

	public string FileName => txtFileName.Text;

	public RecordConfigurationForm(RecordComponent recordComponent)
	{
		InitializeComponent();
		this.recordComponent = recordComponent;
		validVariables = ExpressionHelper.GetValidVariables(recordComponent);
		Prompts = new List<Prompt>(recordComponent.Prompts);
		chkBeep.Checked = recordComponent.Beep;
		txtMaxTime.Text = recordComponent.MaxTime.ToString();
		chkBoxTerminateByDtmf.Checked = recordComponent.TerminateByDtmf;
		txtSaveToFile.Text = recordComponent.SaveToFile;
		txtFileName.Text = recordComponent.FileName;
		txtFileName.Enabled = txtSaveToFile.Text != "false";
		filenameExpressionButton.Enabled = txtFileName.Enabled;
		TxtMaxTime_Validating(txtMaxTime, new CancelEventArgs());
		TxtSaveToFile_Validating(txtSaveToFile, new CancelEventArgs());
		TxtFileName_Validating(txtFileName, new CancelEventArgs());
		Text = LocalizedResourceMgr.GetString("RecordConfigurationForm.Title");
		lblPrompts.Text = LocalizedResourceMgr.GetString("RecordConfigurationForm.lblPrompts.Text");
		editPromptsButton.Text = LocalizedResourceMgr.GetString("RecordConfigurationForm.editPromptsButton.Text");
		chkBeep.Text = LocalizedResourceMgr.GetString("RecordConfigurationForm.chkBeep.Text");
		lblMaxTime.Text = LocalizedResourceMgr.GetString("RecordConfigurationForm.lblMaxTime.Text");
		chkBoxTerminateByDtmf.Text = LocalizedResourceMgr.GetString("RecordConfigurationForm.chkBoxTerminateByDtmf.Text");
		lblSaveToFile.Text = LocalizedResourceMgr.GetString("RecordConfigurationForm.lblSaveToFile.Text");
		lblFileName.Text = LocalizedResourceMgr.GetString("RecordConfigurationForm.lblFileName.Text");
		okButton.Text = LocalizedResourceMgr.GetString("RecordConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("RecordConfigurationForm.cancelButton.Text");
	}

	private void EditPromptsButton_Click(object sender, EventArgs e)
	{
		PromptCollectionEditorForm promptCollectionEditorForm = new PromptCollectionEditorForm(recordComponent);
		promptCollectionEditorForm.PromptList = Prompts;
		if (promptCollectionEditorForm.ShowDialog() == DialogResult.OK)
		{
			Prompts = promptCollectionEditorForm.PromptList;
		}
	}

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void MaskedTextBox_GotFocus(object sender, EventArgs e)
	{
		(sender as MaskedTextBox).SelectAll();
	}

	private void TxtMaxTime_Validating(object sender, CancelEventArgs e)
	{
		int result;
		if (string.IsNullOrEmpty(txtMaxTime.Text))
		{
			errorProvider.SetError(txtMaxTime, LocalizedResourceMgr.GetString("RecordConfigurationForm.Error.MaxTimeIsMandatory"));
		}
		else if (!int.TryParse(txtMaxTime.Text, out result) || result < 1 || result > 99999)
		{
			errorProvider.SetError(txtMaxTime, string.Format(LocalizedResourceMgr.GetString("RecordConfigurationForm.Error.InvalidMaxTimeValue"), 1, 99999));
		}
		else
		{
			errorProvider.SetError(txtMaxTime, string.Empty);
		}
	}

	private void TxtSaveToFile_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtSaveToFile.Text))
		{
			errorProvider.SetError(saveToFileExpressionButton, LocalizedResourceMgr.GetString("RecordConfigurationForm.Error.SaveToFileIsMandatory"));
		}
		else if (!AbsArgument.BuildArgument(validVariables, txtSaveToFile.Text).IsSafeExpression())
		{
			errorProvider.SetError(saveToFileExpressionButton, LocalizedResourceMgr.GetString("RecordConfigurationForm.Error.SaveToFileIsInvalid"));
		}
		else
		{
			errorProvider.SetError(saveToFileExpressionButton, string.Empty);
		}
	}

	private void TxtSaveToFile_Validated(object sender, EventArgs e)
	{
		txtFileName.Enabled = txtSaveToFile.Text != "false";
		filenameExpressionButton.Enabled = txtFileName.Enabled;
		TxtFileName_Validating(txtFileName, new CancelEventArgs());
	}

	private void TxtFileName_Validating(object sender, CancelEventArgs e)
	{
		if (txtFileName.Enabled)
		{
			if (string.IsNullOrEmpty(txtFileName.Text))
			{
				errorProvider.SetError(filenameExpressionButton, LocalizedResourceMgr.GetString("RecordConfigurationForm.Error.FileNameIsMandatory"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, txtFileName.Text).IsSafeExpression())
			{
				errorProvider.SetError(filenameExpressionButton, LocalizedResourceMgr.GetString("RecordConfigurationForm.Error.FileNameIsInvalid"));
			}
			else
			{
				errorProvider.SetError(filenameExpressionButton, string.Empty);
			}
		}
		else
		{
			errorProvider.SetError(filenameExpressionButton, string.Empty);
		}
	}

	private void SaveToFileExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(recordComponent);
		expressionEditorForm.Expression = txtSaveToFile.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtSaveToFile.Text = expressionEditorForm.Expression;
			TxtSaveToFile_Validating(txtSaveToFile, new CancelEventArgs());
			TxtSaveToFile_Validated(txtSaveToFile, EventArgs.Empty);
		}
	}

	private void FilenameExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(recordComponent);
		expressionEditorForm.Expression = txtFileName.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtFileName.Text = expressionEditorForm.Expression;
			TxtFileName_Validating(txtFileName, new CancelEventArgs());
		}
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void CancelButton_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.Cancel;
		Close();
	}

	private void RecordConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		recordComponent.ShowHelp();
	}

	private void RecordConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		recordComponent.ShowHelp();
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
		this.lblMaxTime = new System.Windows.Forms.Label();
		this.txtMaxTime = new System.Windows.Forms.MaskedTextBox();
		this.txtFileName = new System.Windows.Forms.TextBox();
		this.lblFileName = new System.Windows.Forms.Label();
		this.cancelButton = new System.Windows.Forms.Button();
		this.okButton = new System.Windows.Forms.Button();
		this.chkBoxTerminateByDtmf = new System.Windows.Forms.CheckBox();
		this.lblSaveToFile = new System.Windows.Forms.Label();
		this.saveToFileExpressionButton = new System.Windows.Forms.Button();
		this.txtSaveToFile = new System.Windows.Forms.TextBox();
		this.filenameExpressionButton = new System.Windows.Forms.Button();
		this.chkBeep = new System.Windows.Forms.CheckBox();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.lblPrompts = new System.Windows.Forms.Label();
		this.editPromptsButton = new System.Windows.Forms.Button();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.lblMaxTime.AutoSize = true;
		this.lblMaxTime.Location = new System.Drawing.Point(12, 73);
		this.lblMaxTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblMaxTime.Name = "lblMaxTime";
		this.lblMaxTime.Size = new System.Drawing.Size(220, 17);
		this.lblMaxTime.TabIndex = 3;
		this.lblMaxTime.Text = "Max recording duration (seconds)";
		this.txtMaxTime.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtMaxTime.HidePromptOnLeave = true;
		this.txtMaxTime.Location = new System.Drawing.Point(239, 69);
		this.txtMaxTime.Margin = new System.Windows.Forms.Padding(4);
		this.txtMaxTime.Mask = "99999";
		this.txtMaxTime.Name = "txtMaxTime";
		this.txtMaxTime.Size = new System.Drawing.Size(311, 22);
		this.txtMaxTime.TabIndex = 4;
		this.txtMaxTime.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtMaxTime.Validating += new System.ComponentModel.CancelEventHandler(TxtMaxTime_Validating);
		this.txtFileName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtFileName.Location = new System.Drawing.Point(239, 161);
		this.txtFileName.Margin = new System.Windows.Forms.Padding(4);
		this.txtFileName.MaxLength = 8192;
		this.txtFileName.Name = "txtFileName";
		this.txtFileName.Size = new System.Drawing.Size(311, 22);
		this.txtFileName.TabIndex = 10;
		this.txtFileName.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtFileName.Validating += new System.ComponentModel.CancelEventHandler(TxtFileName_Validating);
		this.lblFileName.AutoSize = true;
		this.lblFileName.Location = new System.Drawing.Point(12, 165);
		this.lblFileName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblFileName.Name = "lblFileName";
		this.lblFileName.Size = new System.Drawing.Size(69, 17);
		this.lblFileName.TabIndex = 9;
		this.lblFileName.Text = "File name";
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(451, 209);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 13;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.cancelButton.Click += new System.EventHandler(CancelButton_Click);
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(343, 209);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 12;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.chkBoxTerminateByDtmf.AutoSize = true;
		this.chkBoxTerminateByDtmf.Location = new System.Drawing.Point(16, 101);
		this.chkBoxTerminateByDtmf.Margin = new System.Windows.Forms.Padding(4);
		this.chkBoxTerminateByDtmf.Name = "chkBoxTerminateByDtmf";
		this.chkBoxTerminateByDtmf.Size = new System.Drawing.Size(269, 21);
		this.chkBoxTerminateByDtmf.TabIndex = 5;
		this.chkBoxTerminateByDtmf.Text = "Stop recording by pressing any DTMF";
		this.chkBoxTerminateByDtmf.UseVisualStyleBackColor = true;
		this.lblSaveToFile.AutoSize = true;
		this.lblSaveToFile.Location = new System.Drawing.Point(12, 133);
		this.lblSaveToFile.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblSaveToFile.Name = "lblSaveToFile";
		this.lblSaveToFile.Size = new System.Drawing.Size(78, 17);
		this.lblSaveToFile.TabIndex = 6;
		this.lblSaveToFile.Text = "Save to file";
		this.saveToFileExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.saveToFileExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.saveToFileExpressionButton.Location = new System.Drawing.Point(559, 127);
		this.saveToFileExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.saveToFileExpressionButton.Name = "saveToFileExpressionButton";
		this.saveToFileExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.saveToFileExpressionButton.TabIndex = 8;
		this.saveToFileExpressionButton.UseVisualStyleBackColor = true;
		this.saveToFileExpressionButton.Click += new System.EventHandler(SaveToFileExpressionButton_Click);
		this.txtSaveToFile.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtSaveToFile.AutoCompleteCustomSource.AddRange(new string[2] { "True", "False" });
		this.txtSaveToFile.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
		this.txtSaveToFile.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtSaveToFile.Location = new System.Drawing.Point(239, 129);
		this.txtSaveToFile.Margin = new System.Windows.Forms.Padding(4);
		this.txtSaveToFile.MaxLength = 8192;
		this.txtSaveToFile.Name = "txtSaveToFile";
		this.txtSaveToFile.Size = new System.Drawing.Size(311, 22);
		this.txtSaveToFile.TabIndex = 7;
		this.txtSaveToFile.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtSaveToFile.Validating += new System.ComponentModel.CancelEventHandler(TxtSaveToFile_Validating);
		this.txtSaveToFile.Validated += new System.EventHandler(TxtSaveToFile_Validated);
		this.filenameExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.filenameExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.filenameExpressionButton.Location = new System.Drawing.Point(559, 159);
		this.filenameExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.filenameExpressionButton.Name = "filenameExpressionButton";
		this.filenameExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.filenameExpressionButton.TabIndex = 11;
		this.filenameExpressionButton.UseVisualStyleBackColor = true;
		this.filenameExpressionButton.Click += new System.EventHandler(FilenameExpressionButton_Click);
		this.chkBeep.AutoSize = true;
		this.chkBeep.Location = new System.Drawing.Point(16, 41);
		this.chkBeep.Margin = new System.Windows.Forms.Padding(4);
		this.chkBeep.Name = "chkBeep";
		this.chkBeep.Size = new System.Drawing.Size(241, 21);
		this.chkBeep.TabIndex = 2;
		this.chkBeep.Text = "Play beep before recording starts";
		this.chkBeep.UseVisualStyleBackColor = true;
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.lblPrompts.AutoSize = true;
		this.lblPrompts.Location = new System.Drawing.Point(16, 11);
		this.lblPrompts.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblPrompts.Name = "lblPrompts";
		this.lblPrompts.Size = new System.Drawing.Size(60, 17);
		this.lblPrompts.TabIndex = 0;
		this.lblPrompts.Text = "Prompts";
		this.editPromptsButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.editPromptsButton.Location = new System.Drawing.Point(239, 5);
		this.editPromptsButton.Margin = new System.Windows.Forms.Padding(4);
		this.editPromptsButton.Name = "editPromptsButton";
		this.editPromptsButton.Size = new System.Drawing.Size(312, 28);
		this.editPromptsButton.TabIndex = 1;
		this.editPromptsButton.Text = "Edit Prompts";
		this.editPromptsButton.UseVisualStyleBackColor = true;
		this.editPromptsButton.Click += new System.EventHandler(EditPromptsButton_Click);
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(619, 242);
		base.Controls.Add(this.editPromptsButton);
		base.Controls.Add(this.lblPrompts);
		base.Controls.Add(this.chkBeep);
		base.Controls.Add(this.filenameExpressionButton);
		base.Controls.Add(this.txtSaveToFile);
		base.Controls.Add(this.saveToFileExpressionButton);
		base.Controls.Add(this.lblSaveToFile);
		base.Controls.Add(this.chkBoxTerminateByDtmf);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.Controls.Add(this.lblFileName);
		base.Controls.Add(this.txtFileName);
		base.Controls.Add(this.txtMaxTime);
		base.Controls.Add(this.lblMaxTime);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(1359, 289);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(634, 289);
		base.Name = "RecordConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Record";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(RecordConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(RecordConfigurationForm_HelpRequested);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
