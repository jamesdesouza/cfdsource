using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Controls;
using TCX.CFD.Properties;

namespace TCX.CFD.Forms;

public class SurveyConfigurationForm : Form
{
	private readonly SurveyComponent surveyComponent;

	private readonly SurveyQuestionCollectionEditorControl questionEditorControl;

	private readonly List<string> validVariables;

	private IContainer components;

	private Button cancelButton;

	private Button okButton;

	private CheckBox chkAcceptDtmfInput;

	private Label lblMaxRetryCount;

	private MaskedTextBox txtMaxRetryCount;

	private Label lblTimeout;

	private MaskedTextBox txtTimeout;

	private ErrorProvider errorProvider;

	private GroupBox grpBoxPrompts;

	private Button invalidDigitPromptsButton;

	private Button timeoutPromptsButton;

	private Button goodbyePromptsButton;

	private Button introductoryPromptsButton;

	private Button exportToCSVFileExpressionButton;

	private TextBox txtRecordingsPath;

	private Button recordingsPathExpressionButton;

	private Label lblRecordingsPath;

	private Label lblExportToCSVFile;

	private TextBox txtExportToCSVFile;

	private BindingSource parameterBindingSource;

	private DataGridView outputFieldsGrid;

	private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn valueDataGridViewTextBoxColumn;

	private DataGridViewButtonColumn expressionBuilderColumn;

	private Label lblOutputFields;

	private GroupBox grpBoxQuestions;

	private CheckBox chkAllowPartialAnswers;

	public bool AcceptDtmfInput => chkAcceptDtmfInput.Checked;

	public uint Timeout
	{
		get
		{
			if (!string.IsNullOrEmpty(txtTimeout.Text))
			{
				return Convert.ToUInt32(txtTimeout.Text);
			}
			return 5u;
		}
	}

	public uint MaxRetryCount
	{
		get
		{
			if (!string.IsNullOrEmpty(txtMaxRetryCount.Text))
			{
				return Convert.ToUInt32(txtMaxRetryCount.Text);
			}
			return 3u;
		}
	}

	public bool AllowPartialAnswers => chkAllowPartialAnswers.Checked;

	public string RecordingsPath => txtRecordingsPath.Text;

	public string ExportToCSVFile => txtExportToCSVFile.Text;

	public List<Prompt> IntroductoryPrompts { get; private set; }

	public List<Prompt> GoodbyePrompts { get; private set; }

	public List<Prompt> TimeoutPrompts { get; private set; }

	public List<Prompt> InvalidDigitPrompts { get; private set; }

	public List<SurveyQuestion> SurveyQuestions { get; private set; }

	public List<Parameter> OutputFields
	{
		get
		{
			List<Parameter> list = new List<Parameter>();
			foreach (Parameter item in parameterBindingSource.List)
			{
				if (!string.IsNullOrEmpty(item.Name) && !string.IsNullOrEmpty(item.Value))
				{
					list.Add(item);
				}
			}
			return list;
		}
	}

	private void MaskedTextBox_GotFocus(object sender, EventArgs e)
	{
		(sender as MaskedTextBox).SelectAll();
	}

	private void TextBox_GotFocus(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void TxtTimeout_Validating(object sender, CancelEventArgs e)
	{
		errorProvider.SetError(txtTimeout, string.IsNullOrEmpty(txtTimeout.Text) ? LocalizedResourceMgr.GetString("SurveyConfigurationForm.Error.TimeoutIsMandatory") : ((Convert.ToUInt32(txtTimeout.Text) == 0) ? LocalizedResourceMgr.GetString("SurveyConfigurationForm.Error.TimeoutInvalidRange") : string.Empty));
	}

	private void TxtMaxRetryCount_Validating(object sender, CancelEventArgs e)
	{
		errorProvider.SetError(txtMaxRetryCount, string.IsNullOrEmpty(txtMaxRetryCount.Text) ? LocalizedResourceMgr.GetString("SurveyConfigurationForm.Error.MaxRetryCountIsMandatory") : ((Convert.ToUInt32(txtMaxRetryCount.Text) == 0) ? LocalizedResourceMgr.GetString("SurveyConfigurationForm.Error.MaxRetryCountInvalidRange") : string.Empty));
	}

	private void TxtRecordingsPath_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtRecordingsPath.Text))
		{
			errorProvider.SetError(recordingsPathExpressionButton, string.Empty);
			return;
		}
		bool flag = false;
		foreach (SurveyQuestion surveyQuestion in questionEditorControl.SurveyQuestionList)
		{
			if (surveyQuestion is RecordingSurveyQuestion)
			{
				flag = true;
			}
		}
		if (flag)
		{
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtRecordingsPath.Text);
			if (!absArgument.IsSafeExpression())
			{
				errorProvider.SetError(recordingsPathExpressionButton, LocalizedResourceMgr.GetString("SurveyConfigurationForm.Error.RecordingsPathIsInvalid"));
				return;
			}
			List<DotNetExpressionArgument> literalExpressionList = absArgument.GetLiteralExpressionList();
			bool flag2 = false;
			foreach (DotNetExpressionArgument item in literalExpressionList)
			{
				if (item.IsStringLiteral() && ExpressionHelper.UnescapeConstantString(item.GetString()).IndexOfAny(new char[6] { '<', '>', '"', '|', '?', '*' }) >= 0)
				{
					flag2 = true;
					break;
				}
			}
			errorProvider.SetError(recordingsPathExpressionButton, flag2 ? LocalizedResourceMgr.GetString("SurveyConfigurationForm.Error.RecordingsPathInvalidCharacters") : string.Empty);
		}
		else if (!AbsArgument.BuildArgument(validVariables, txtRecordingsPath.Text).IsSafeExpression())
		{
			errorProvider.SetError(recordingsPathExpressionButton, LocalizedResourceMgr.GetString("SurveyConfigurationForm.Error.RecordingsPathIsInvalid"));
		}
		else
		{
			errorProvider.SetError(recordingsPathExpressionButton, string.Empty);
		}
	}

	private void TxtExportToCSVFile_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtExportToCSVFile.Text))
		{
			errorProvider.SetError(exportToCSVFileExpressionButton, LocalizedResourceMgr.GetString("SurveyConfigurationForm.Error.ExportToCSVFileIsMandatory"));
			return;
		}
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtExportToCSVFile.Text);
		if (!absArgument.IsSafeExpression())
		{
			errorProvider.SetError(exportToCSVFileExpressionButton, LocalizedResourceMgr.GetString("SurveyConfigurationForm.Error.ExportToCSVFileIsInvalid"));
			return;
		}
		List<DotNetExpressionArgument> literalExpressionList = absArgument.GetLiteralExpressionList();
		bool flag = false;
		foreach (DotNetExpressionArgument item in literalExpressionList)
		{
			if (item.IsStringLiteral() && ExpressionHelper.UnescapeConstantString(item.GetString()).IndexOfAny(new char[6] { '<', '>', '"', '|', '?', '*' }) >= 0)
			{
				flag = true;
				break;
			}
		}
		errorProvider.SetError(exportToCSVFileExpressionButton, flag ? LocalizedResourceMgr.GetString("SurveyConfigurationForm.Error.ExportToCSVFileInvalidCharacters") : string.Empty);
	}

	private void RecordingsPathExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(surveyComponent);
		expressionEditorForm.Expression = txtRecordingsPath.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtRecordingsPath.Text = expressionEditorForm.Expression;
			TxtRecordingsPath_Validating(txtRecordingsPath, new CancelEventArgs());
		}
	}

	private void ExportToCSVFileExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(surveyComponent);
		expressionEditorForm.Expression = txtExportToCSVFile.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtExportToCSVFile.Text = expressionEditorForm.Expression;
			TxtExportToCSVFile_Validating(txtExportToCSVFile, new CancelEventArgs());
		}
	}

	private void IntroductoryPromptsButton_Click(object sender, EventArgs e)
	{
		PromptCollectionEditorForm promptCollectionEditorForm = new PromptCollectionEditorForm(surveyComponent);
		promptCollectionEditorForm.PromptList = IntroductoryPrompts;
		if (promptCollectionEditorForm.ShowDialog() == DialogResult.OK)
		{
			IntroductoryPrompts = promptCollectionEditorForm.PromptList;
		}
	}

	private void GoodbyePromptsButton_Click(object sender, EventArgs e)
	{
		PromptCollectionEditorForm promptCollectionEditorForm = new PromptCollectionEditorForm(surveyComponent);
		promptCollectionEditorForm.PromptList = GoodbyePrompts;
		if (promptCollectionEditorForm.ShowDialog() == DialogResult.OK)
		{
			GoodbyePrompts = promptCollectionEditorForm.PromptList;
		}
	}

	private void TimeoutPromptsButton_Click(object sender, EventArgs e)
	{
		PromptCollectionEditorForm promptCollectionEditorForm = new PromptCollectionEditorForm(surveyComponent);
		promptCollectionEditorForm.PromptList = TimeoutPrompts;
		if (promptCollectionEditorForm.ShowDialog() == DialogResult.OK)
		{
			TimeoutPrompts = promptCollectionEditorForm.PromptList;
		}
	}

	private void InvalidDigitPromptsButton_Click(object sender, EventArgs e)
	{
		PromptCollectionEditorForm promptCollectionEditorForm = new PromptCollectionEditorForm(surveyComponent);
		promptCollectionEditorForm.PromptList = InvalidDigitPrompts;
		if (promptCollectionEditorForm.ShowDialog() == DialogResult.OK)
		{
			InvalidDigitPrompts = promptCollectionEditorForm.PromptList;
		}
	}

	private void OutputFieldsGrid_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		if (e.ColumnIndex == 2)
		{
			ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(surveyComponent);
			expressionEditorForm.Expression = ((outputFieldsGrid[1, e.RowIndex].Value == null) ? string.Empty : outputFieldsGrid[1, e.RowIndex].Value.ToString());
			if (expressionEditorForm.ShowDialog() == DialogResult.OK)
			{
				outputFieldsGrid.CurrentCell = outputFieldsGrid[1, e.RowIndex];
				SendKeys.SendWait(" ");
				outputFieldsGrid.EndEdit();
				outputFieldsGrid.CurrentCell = null;
				outputFieldsGrid[1, e.RowIndex].Value = expressionEditorForm.Expression;
			}
		}
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(txtTimeout.Text))
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("SurveyConfigurationForm.Error.TimeoutIsMandatory"), LocalizedResourceMgr.GetString("SurveyConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			txtTimeout.Focus();
			return;
		}
		if (Convert.ToUInt32(txtTimeout.Text) == 0)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("SurveyConfigurationForm.Error.TimeoutInvalidRange"), LocalizedResourceMgr.GetString("SurveyConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			txtTimeout.Focus();
			return;
		}
		if (string.IsNullOrEmpty(txtMaxRetryCount.Text))
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("SurveyConfigurationForm.Error.MaxRetryCountIsMandatory"), LocalizedResourceMgr.GetString("SurveyConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			txtMaxRetryCount.Focus();
			return;
		}
		if (Convert.ToUInt32(txtMaxRetryCount.Text) == 0)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("SurveyConfigurationForm.Error.MaxRetryCountInvalidRange"), LocalizedResourceMgr.GetString("SurveyConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			txtMaxRetryCount.Focus();
			return;
		}
		SurveyQuestions = questionEditorControl.SurveyQuestionList;
		List<string> list = new List<string>();
		foreach (Parameter item in parameterBindingSource.List)
		{
			if (!string.IsNullOrEmpty(item.Name) || !string.IsNullOrEmpty(item.Value))
			{
				if (string.IsNullOrEmpty(item.Name) || string.IsNullOrEmpty(item.Value))
				{
					MessageBox.Show(LocalizedResourceMgr.GetString("SurveyConfigurationForm.MessageBox.Error.EmptyValuesNotAllowed"), LocalizedResourceMgr.GetString("SurveyConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
					parameterBindingSource.Position = parameterBindingSource.IndexOf(item);
					return;
				}
				if (list.Contains(item.Name))
				{
					MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("SurveyConfigurationForm.MessageBox.Error.DuplicatedNamesNotAllowed"), item.Name), LocalizedResourceMgr.GetString("SurveyConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
					parameterBindingSource.Position = parameterBindingSource.IndexOf(item);
					return;
				}
				list.Add(item.Name);
			}
		}
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void CancelButton_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.Cancel;
		Close();
	}

	public SurveyConfigurationForm(SurveyComponent surveyComponent)
	{
		InitializeComponent();
		this.surveyComponent = surveyComponent;
		IntroductoryPrompts = new List<Prompt>(surveyComponent.IntroductoryPrompts);
		GoodbyePrompts = new List<Prompt>(surveyComponent.GoodbyePrompts);
		TimeoutPrompts = new List<Prompt>(surveyComponent.TimeoutPrompts);
		InvalidDigitPrompts = new List<Prompt>(surveyComponent.InvalidDigitPrompts);
		SurveyQuestions = new List<SurveyQuestion>(surveyComponent.SurveyQuestions);
		questionEditorControl = new SurveyQuestionCollectionEditorControl(surveyComponent);
		questionEditorControl.Dock = DockStyle.Fill;
		questionEditorControl.SurveyQuestionList = SurveyQuestions;
		grpBoxQuestions.Controls.Add(questionEditorControl);
		validVariables = ExpressionHelper.GetValidVariables(surveyComponent);
		chkAcceptDtmfInput.Checked = surveyComponent.AcceptDtmfInput;
		txtTimeout.Text = surveyComponent.Timeout.ToString();
		txtMaxRetryCount.Text = surveyComponent.MaxRetryCount.ToString();
		chkAllowPartialAnswers.Checked = surveyComponent.AllowPartialAnswers;
		txtRecordingsPath.Text = surveyComponent.RecordingsPath;
		txtExportToCSVFile.Text = surveyComponent.ExportToCSVFile;
		TxtTimeout_Validating(txtTimeout, new CancelEventArgs());
		TxtMaxRetryCount_Validating(txtMaxRetryCount, new CancelEventArgs());
		TxtRecordingsPath_Validating(txtRecordingsPath, new CancelEventArgs());
		TxtExportToCSVFile_Validating(txtExportToCSVFile, new CancelEventArgs());
		foreach (Parameter outputField in surveyComponent.OutputFields)
		{
			parameterBindingSource.List.Add(outputField);
		}
		Text = LocalizedResourceMgr.GetString("SurveyConfigurationForm.Title");
		chkAcceptDtmfInput.Text = LocalizedResourceMgr.GetString("SurveyConfigurationForm.chkAcceptDtmfInput.Text");
		lblTimeout.Text = LocalizedResourceMgr.GetString("SurveyConfigurationForm.lblTimeout.Text");
		lblMaxRetryCount.Text = LocalizedResourceMgr.GetString("SurveyConfigurationForm.lblMaxRetryCount.Text");
		chkAllowPartialAnswers.Text = LocalizedResourceMgr.GetString("SurveyConfigurationForm.chkAllowPartialAnswers.Text");
		lblRecordingsPath.Text = LocalizedResourceMgr.GetString("SurveyConfigurationForm.lblRecordingsPath.Text");
		lblExportToCSVFile.Text = LocalizedResourceMgr.GetString("SurveyConfigurationForm.lblExportToCSVFile.Text");
		grpBoxPrompts.Text = LocalizedResourceMgr.GetString("SurveyConfigurationForm.grpBoxPrompts.Text");
		introductoryPromptsButton.Text = LocalizedResourceMgr.GetString("SurveyConfigurationForm.introductoryPromptsButton.Text");
		goodbyePromptsButton.Text = LocalizedResourceMgr.GetString("SurveyConfigurationForm.goodbyePromptsButton.Text");
		timeoutPromptsButton.Text = LocalizedResourceMgr.GetString("SurveyConfigurationForm.timeoutPromptsButton.Text");
		invalidDigitPromptsButton.Text = LocalizedResourceMgr.GetString("SurveyConfigurationForm.invalidDigitPromptsButton.Text");
		grpBoxQuestions.Text = LocalizedResourceMgr.GetString("SurveyConfigurationForm.grpBoxQuestions.Text");
		lblOutputFields.Text = LocalizedResourceMgr.GetString("SurveyConfigurationForm.lblOutputFields.Text");
		okButton.Text = LocalizedResourceMgr.GetString("SurveyConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("SurveyConfigurationForm.cancelButton.Text");
	}

	private void SurveyConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		surveyComponent.ShowHelp();
	}

	private void SurveyConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		surveyComponent.ShowHelp();
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
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
		this.cancelButton = new System.Windows.Forms.Button();
		this.okButton = new System.Windows.Forms.Button();
		this.chkAcceptDtmfInput = new System.Windows.Forms.CheckBox();
		this.lblMaxRetryCount = new System.Windows.Forms.Label();
		this.txtMaxRetryCount = new System.Windows.Forms.MaskedTextBox();
		this.lblTimeout = new System.Windows.Forms.Label();
		this.txtTimeout = new System.Windows.Forms.MaskedTextBox();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.grpBoxPrompts = new System.Windows.Forms.GroupBox();
		this.invalidDigitPromptsButton = new System.Windows.Forms.Button();
		this.timeoutPromptsButton = new System.Windows.Forms.Button();
		this.goodbyePromptsButton = new System.Windows.Forms.Button();
		this.introductoryPromptsButton = new System.Windows.Forms.Button();
		this.exportToCSVFileExpressionButton = new System.Windows.Forms.Button();
		this.txtRecordingsPath = new System.Windows.Forms.TextBox();
		this.recordingsPathExpressionButton = new System.Windows.Forms.Button();
		this.lblRecordingsPath = new System.Windows.Forms.Label();
		this.lblExportToCSVFile = new System.Windows.Forms.Label();
		this.txtExportToCSVFile = new System.Windows.Forms.TextBox();
		this.outputFieldsGrid = new System.Windows.Forms.DataGridView();
		this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.valueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.expressionBuilderColumn = new System.Windows.Forms.DataGridViewButtonColumn();
		this.parameterBindingSource = new System.Windows.Forms.BindingSource(this.components);
		this.lblOutputFields = new System.Windows.Forms.Label();
		this.grpBoxQuestions = new System.Windows.Forms.GroupBox();
		this.chkAllowPartialAnswers = new System.Windows.Forms.CheckBox();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		this.grpBoxPrompts.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.outputFieldsGrid).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.parameterBindingSource).BeginInit();
		base.SuspendLayout();
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(830, 753);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 17;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.cancelButton.Click += new System.EventHandler(CancelButton_Click);
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(722, 753);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 16;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.chkAcceptDtmfInput.AutoSize = true;
		this.chkAcceptDtmfInput.Location = new System.Drawing.Point(16, 15);
		this.chkAcceptDtmfInput.Margin = new System.Windows.Forms.Padding(4);
		this.chkAcceptDtmfInput.Name = "chkAcceptDtmfInput";
		this.chkAcceptDtmfInput.Size = new System.Drawing.Size(245, 21);
		this.chkAcceptDtmfInput.TabIndex = 0;
		this.chkAcceptDtmfInput.Text = "Accept DTMF Input During Prompt";
		this.chkAcceptDtmfInput.UseVisualStyleBackColor = true;
		this.lblMaxRetryCount.AutoSize = true;
		this.lblMaxRetryCount.Location = new System.Drawing.Point(285, 51);
		this.lblMaxRetryCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblMaxRetryCount.Name = "lblMaxRetryCount";
		this.lblMaxRetryCount.Size = new System.Drawing.Size(82, 17);
		this.lblMaxRetryCount.TabIndex = 3;
		this.lblMaxRetryCount.Text = "Max Retries";
		this.txtMaxRetryCount.HidePromptOnLeave = true;
		this.txtMaxRetryCount.Location = new System.Drawing.Point(375, 48);
		this.txtMaxRetryCount.Margin = new System.Windows.Forms.Padding(4);
		this.txtMaxRetryCount.Mask = "99";
		this.txtMaxRetryCount.Name = "txtMaxRetryCount";
		this.txtMaxRetryCount.Size = new System.Drawing.Size(117, 22);
		this.txtMaxRetryCount.TabIndex = 4;
		this.txtMaxRetryCount.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtMaxRetryCount.Validating += new System.ComponentModel.CancelEventHandler(TxtMaxRetryCount_Validating);
		this.lblTimeout.AutoSize = true;
		this.lblTimeout.Location = new System.Drawing.Point(12, 50);
		this.lblTimeout.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblTimeout.Name = "lblTimeout";
		this.lblTimeout.Size = new System.Drawing.Size(102, 17);
		this.lblTimeout.TabIndex = 1;
		this.lblTimeout.Text = "Timeout (secs)";
		this.txtTimeout.HidePromptOnLeave = true;
		this.txtTimeout.Location = new System.Drawing.Point(142, 47);
		this.txtTimeout.Margin = new System.Windows.Forms.Padding(4);
		this.txtTimeout.Mask = "99";
		this.txtTimeout.Name = "txtTimeout";
		this.txtTimeout.Size = new System.Drawing.Size(117, 22);
		this.txtTimeout.TabIndex = 2;
		this.txtTimeout.GotFocus += new System.EventHandler(MaskedTextBox_GotFocus);
		this.txtTimeout.Validating += new System.ComponentModel.CancelEventHandler(TxtTimeout_Validating);
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.grpBoxPrompts.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.grpBoxPrompts.Controls.Add(this.invalidDigitPromptsButton);
		this.grpBoxPrompts.Controls.Add(this.timeoutPromptsButton);
		this.grpBoxPrompts.Controls.Add(this.goodbyePromptsButton);
		this.grpBoxPrompts.Controls.Add(this.introductoryPromptsButton);
		this.grpBoxPrompts.Location = new System.Drawing.Point(16, 142);
		this.grpBoxPrompts.Margin = new System.Windows.Forms.Padding(4);
		this.grpBoxPrompts.Name = "grpBoxPrompts";
		this.grpBoxPrompts.Padding = new System.Windows.Forms.Padding(4);
		this.grpBoxPrompts.Size = new System.Drawing.Size(914, 103);
		this.grpBoxPrompts.TabIndex = 12;
		this.grpBoxPrompts.TabStop = false;
		this.grpBoxPrompts.Text = "Prompts";
		this.invalidDigitPromptsButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.invalidDigitPromptsButton.Location = new System.Drawing.Point(461, 59);
		this.invalidDigitPromptsButton.Margin = new System.Windows.Forms.Padding(4);
		this.invalidDigitPromptsButton.Name = "invalidDigitPromptsButton";
		this.invalidDigitPromptsButton.Size = new System.Drawing.Size(445, 28);
		this.invalidDigitPromptsButton.TabIndex = 3;
		this.invalidDigitPromptsButton.Text = "Invalid Digit Prompts";
		this.invalidDigitPromptsButton.UseVisualStyleBackColor = true;
		this.invalidDigitPromptsButton.Click += new System.EventHandler(InvalidDigitPromptsButton_Click);
		this.timeoutPromptsButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.timeoutPromptsButton.Location = new System.Drawing.Point(461, 24);
		this.timeoutPromptsButton.Margin = new System.Windows.Forms.Padding(4);
		this.timeoutPromptsButton.Name = "timeoutPromptsButton";
		this.timeoutPromptsButton.Size = new System.Drawing.Size(445, 28);
		this.timeoutPromptsButton.TabIndex = 2;
		this.timeoutPromptsButton.Text = "Timeout Prompts";
		this.timeoutPromptsButton.UseVisualStyleBackColor = true;
		this.timeoutPromptsButton.Click += new System.EventHandler(TimeoutPromptsButton_Click);
		this.goodbyePromptsButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.goodbyePromptsButton.Location = new System.Drawing.Point(8, 59);
		this.goodbyePromptsButton.Margin = new System.Windows.Forms.Padding(4);
		this.goodbyePromptsButton.Name = "goodbyePromptsButton";
		this.goodbyePromptsButton.Size = new System.Drawing.Size(445, 28);
		this.goodbyePromptsButton.TabIndex = 1;
		this.goodbyePromptsButton.Text = "Goodbye Prompts";
		this.goodbyePromptsButton.UseVisualStyleBackColor = true;
		this.goodbyePromptsButton.Click += new System.EventHandler(GoodbyePromptsButton_Click);
		this.introductoryPromptsButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.introductoryPromptsButton.Location = new System.Drawing.Point(8, 23);
		this.introductoryPromptsButton.Margin = new System.Windows.Forms.Padding(4);
		this.introductoryPromptsButton.Name = "introductoryPromptsButton";
		this.introductoryPromptsButton.Size = new System.Drawing.Size(445, 28);
		this.introductoryPromptsButton.TabIndex = 0;
		this.introductoryPromptsButton.Text = "Introductory Prompts";
		this.introductoryPromptsButton.UseVisualStyleBackColor = true;
		this.introductoryPromptsButton.Click += new System.EventHandler(IntroductoryPromptsButton_Click);
		this.exportToCSVFileExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.exportToCSVFileExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.exportToCSVFileExpressionButton.Location = new System.Drawing.Point(880, 106);
		this.exportToCSVFileExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.exportToCSVFileExpressionButton.Name = "exportToCSVFileExpressionButton";
		this.exportToCSVFileExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.exportToCSVFileExpressionButton.TabIndex = 11;
		this.exportToCSVFileExpressionButton.UseVisualStyleBackColor = true;
		this.exportToCSVFileExpressionButton.Click += new System.EventHandler(ExportToCSVFileExpressionButton_Click);
		this.txtRecordingsPath.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtRecordingsPath.AutoCompleteCustomSource.AddRange(new string[2] { "True", "False" });
		this.txtRecordingsPath.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
		this.txtRecordingsPath.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.txtRecordingsPath.Location = new System.Drawing.Point(142, 78);
		this.txtRecordingsPath.Margin = new System.Windows.Forms.Padding(4);
		this.txtRecordingsPath.MaxLength = 8192;
		this.txtRecordingsPath.Name = "txtRecordingsPath";
		this.txtRecordingsPath.Size = new System.Drawing.Size(730, 22);
		this.txtRecordingsPath.TabIndex = 7;
		this.txtRecordingsPath.GotFocus += new System.EventHandler(TextBox_GotFocus);
		this.txtRecordingsPath.Validating += new System.ComponentModel.CancelEventHandler(TxtRecordingsPath_Validating);
		this.recordingsPathExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.recordingsPathExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.recordingsPathExpressionButton.Location = new System.Drawing.Point(880, 76);
		this.recordingsPathExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.recordingsPathExpressionButton.Name = "recordingsPathExpressionButton";
		this.recordingsPathExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.recordingsPathExpressionButton.TabIndex = 8;
		this.recordingsPathExpressionButton.UseVisualStyleBackColor = true;
		this.recordingsPathExpressionButton.Click += new System.EventHandler(RecordingsPathExpressionButton_Click);
		this.lblRecordingsPath.AutoSize = true;
		this.lblRecordingsPath.Location = new System.Drawing.Point(13, 81);
		this.lblRecordingsPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblRecordingsPath.Name = "lblRecordingsPath";
		this.lblRecordingsPath.Size = new System.Drawing.Size(113, 17);
		this.lblRecordingsPath.TabIndex = 6;
		this.lblRecordingsPath.Text = "Recordings Path";
		this.lblExportToCSVFile.AutoSize = true;
		this.lblExportToCSVFile.Location = new System.Drawing.Point(13, 111);
		this.lblExportToCSVFile.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblExportToCSVFile.Name = "lblExportToCSVFile";
		this.lblExportToCSVFile.Size = new System.Drawing.Size(121, 17);
		this.lblExportToCSVFile.TabIndex = 9;
		this.lblExportToCSVFile.Text = "Export to CSV File";
		this.txtExportToCSVFile.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtExportToCSVFile.Location = new System.Drawing.Point(142, 108);
		this.txtExportToCSVFile.Margin = new System.Windows.Forms.Padding(4);
		this.txtExportToCSVFile.MaxLength = 8192;
		this.txtExportToCSVFile.Name = "txtExportToCSVFile";
		this.txtExportToCSVFile.Size = new System.Drawing.Size(730, 22);
		this.txtExportToCSVFile.TabIndex = 10;
		this.txtExportToCSVFile.GotFocus += new System.EventHandler(TextBox_GotFocus);
		this.txtExportToCSVFile.Validating += new System.ComponentModel.CancelEventHandler(TxtExportToCSVFile_Validating);
		this.outputFieldsGrid.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.outputFieldsGrid.AutoGenerateColumns = false;
		this.outputFieldsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
		this.outputFieldsGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
		this.outputFieldsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.outputFieldsGrid.Columns.AddRange(this.nameDataGridViewTextBoxColumn, this.valueDataGridViewTextBoxColumn, this.expressionBuilderColumn);
		this.outputFieldsGrid.DataSource = this.parameterBindingSource;
		this.outputFieldsGrid.Location = new System.Drawing.Point(15, 596);
		this.outputFieldsGrid.Margin = new System.Windows.Forms.Padding(4);
		this.outputFieldsGrid.Name = "outputFieldsGrid";
		this.outputFieldsGrid.RowHeadersWidth = 51;
		this.outputFieldsGrid.Size = new System.Drawing.Size(915, 149);
		this.outputFieldsGrid.TabIndex = 15;
		this.outputFieldsGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(OutputFieldsGrid_CellClick);
		this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
		this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
		this.nameDataGridViewTextBoxColumn.MaxInputLength = 256;
		this.nameDataGridViewTextBoxColumn.MinimumWidth = 6;
		this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
		this.valueDataGridViewTextBoxColumn.DataPropertyName = "Value";
		this.valueDataGridViewTextBoxColumn.HeaderText = "Value";
		this.valueDataGridViewTextBoxColumn.MaxInputLength = 1024;
		this.valueDataGridViewTextBoxColumn.MinimumWidth = 6;
		this.valueDataGridViewTextBoxColumn.Name = "valueDataGridViewTextBoxColumn";
		dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle.NullValue = "...";
		this.expressionBuilderColumn.DefaultCellStyle = dataGridViewCellStyle;
		this.expressionBuilderColumn.FillWeight = 20f;
		this.expressionBuilderColumn.HeaderText = "";
		this.expressionBuilderColumn.MinimumWidth = 6;
		this.expressionBuilderColumn.Name = "expressionBuilderColumn";
		this.expressionBuilderColumn.Text = "";
		this.parameterBindingSource.DataSource = typeof(TCX.CFD.Classes.Components.Parameter);
		this.lblOutputFields.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.lblOutputFields.AutoSize = true;
		this.lblOutputFields.Location = new System.Drawing.Point(12, 575);
		this.lblOutputFields.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblOutputFields.Name = "lblOutputFields";
		this.lblOutputFields.Size = new System.Drawing.Size(92, 17);
		this.lblOutputFields.TabIndex = 14;
		this.lblOutputFields.Text = "Output Fields";
		this.grpBoxQuestions.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.grpBoxQuestions.Location = new System.Drawing.Point(16, 249);
		this.grpBoxQuestions.Margin = new System.Windows.Forms.Padding(0);
		this.grpBoxQuestions.Name = "grpBoxQuestions";
		this.grpBoxQuestions.Padding = new System.Windows.Forms.Padding(1, 0, 1, 2);
		this.grpBoxQuestions.Size = new System.Drawing.Size(914, 323);
		this.grpBoxQuestions.TabIndex = 13;
		this.grpBoxQuestions.TabStop = false;
		this.grpBoxQuestions.Text = "Questions";
		this.chkAllowPartialAnswers.AutoSize = true;
		this.chkAllowPartialAnswers.Location = new System.Drawing.Point(529, 49);
		this.chkAllowPartialAnswers.Name = "chkAllowPartialAnswers";
		this.chkAllowPartialAnswers.Size = new System.Drawing.Size(163, 21);
		this.chkAllowPartialAnswers.TabIndex = 5;
		this.chkAllowPartialAnswers.Text = "Allow Partial Answers";
		this.chkAllowPartialAnswers.UseVisualStyleBackColor = true;
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(946, 796);
		base.Controls.Add(this.chkAllowPartialAnswers);
		base.Controls.Add(this.grpBoxQuestions);
		base.Controls.Add(this.outputFieldsGrid);
		base.Controls.Add(this.lblOutputFields);
		base.Controls.Add(this.exportToCSVFileExpressionButton);
		base.Controls.Add(this.txtRecordingsPath);
		base.Controls.Add(this.recordingsPathExpressionButton);
		base.Controls.Add(this.lblRecordingsPath);
		base.Controls.Add(this.lblExportToCSVFile);
		base.Controls.Add(this.txtExportToCSVFile);
		base.Controls.Add(this.grpBoxPrompts);
		base.Controls.Add(this.lblMaxRetryCount);
		base.Controls.Add(this.txtMaxRetryCount);
		base.Controls.Add(this.lblTimeout);
		base.Controls.Add(this.txtTimeout);
		base.Controls.Add(this.chkAcceptDtmfInput);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(964, 843);
		base.Name = "SurveyConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Survey";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(SurveyConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(SurveyConfigurationForm_HelpRequested);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		this.grpBoxPrompts.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.outputFieldsGrid).EndInit();
		((System.ComponentModel.ISupportInitialize)this.parameterBindingSource).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
