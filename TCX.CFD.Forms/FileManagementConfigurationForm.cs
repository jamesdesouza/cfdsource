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

public class FileManagementConfigurationForm : Form
{
	private readonly FileManagementComponent fileManagementComponent;

	private readonly List<string> validVariables;

	private IContainer components;

	private Button cancelButton;

	private Button okButton;

	private Label lblContent;

	private TextBox txtContent;

	private GroupBox grpBoxAction;

	private RadioButton rbActionWrite;

	private RadioButton rbActionRead;

	private Label lblOpenMode;

	private Label lblFileName;

	private TextBox txtFileName;

	private ComboBox comboOpenMode;

	private Label lblLinesToRead;

	private Label lblFirstLineToRead;

	private TextBox txtLinesToRead;

	private TextBox txtFirstLineToRead;

	private Button firstLineToReadExpressionButton;

	private Label lblReadToEnd;

	private TextBox txtReadToEnd;

	private Button contentExpressionButton;

	private Button readToEndExpressionButton;

	private Button linesToReadExpressionButton;

	private CheckBox chkAppendFinalCrLf;

	private ErrorProvider errorProvider;

	private Button fileNameExpressionButton;

	public string FileName => txtFileName.Text;

	public FileManagementOpenModes OpenMode => (FileManagementOpenModes)comboOpenMode.SelectedItem;

	public FileManagementActions Action
	{
		get
		{
			if (!rbActionRead.Checked)
			{
				return FileManagementActions.Write;
			}
			return FileManagementActions.Read;
		}
	}

	public string FirstLineToRead => txtFirstLineToRead.Text;

	public string LinesToRead => txtLinesToRead.Text;

	public string ReadToEnd => txtReadToEnd.Text;

	public string Content => txtContent.Text;

	public bool AppendFinalCrLf => chkAppendFinalCrLf.Checked;

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void TxtFileName_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtFileName.Text))
		{
			errorProvider.SetError(fileNameExpressionButton, LocalizedResourceMgr.GetString("FileManagementConfigurationForm.Error.FileNameIsMandatory"));
			return;
		}
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtFileName.Text);
		errorProvider.SetError(fileNameExpressionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("FileManagementConfigurationForm.Error.FileNameIsInvalid"));
	}

	private void ComboOpenMode_SelectedIndexChanged(object sender, EventArgs e)
	{
		errorProvider.SetError(comboOpenMode, (rbActionRead.Checked && (FileManagementOpenModes)comboOpenMode.SelectedItem != FileManagementOpenModes.Open && (FileManagementOpenModes)comboOpenMode.SelectedItem != FileManagementOpenModes.OpenOrCreate) ? LocalizedResourceMgr.GetString("FileManagementConfigurationForm.Error.InvalidOpenModeAction") : string.Empty);
	}

	private void TxtFirstLineToRead_Validating(object sender, CancelEventArgs e)
	{
		if (txtFirstLineToRead.Enabled)
		{
			if (string.IsNullOrEmpty(txtFirstLineToRead.Text))
			{
				errorProvider.SetError(firstLineToReadExpressionButton, LocalizedResourceMgr.GetString("FileManagementConfigurationForm.Error.FirstLineToReadIsMandatory"));
				return;
			}
			if (long.TryParse(txtFirstLineToRead.Text, out var result) && (result < 0 || result > uint.MaxValue))
			{
				errorProvider.SetError(firstLineToReadExpressionButton, string.Format(LocalizedResourceMgr.GetString("FileManagementConfigurationForm.Error.InvalidFirstLineToReadValue"), 0, uint.MaxValue));
				return;
			}
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtFirstLineToRead.Text);
			errorProvider.SetError(firstLineToReadExpressionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("FileManagementConfigurationForm.Error.FirstLineToReadIsInvalid"));
		}
		else
		{
			errorProvider.SetError(firstLineToReadExpressionButton, string.Empty);
		}
	}

	private void TxtLinesToRead_Validating(object sender, CancelEventArgs e)
	{
		if (txtLinesToRead.Enabled)
		{
			if (string.IsNullOrEmpty(txtLinesToRead.Text))
			{
				errorProvider.SetError(linesToReadExpressionButton, LocalizedResourceMgr.GetString("FileManagementConfigurationForm.Error.LinesToReadIsMandatory"));
				return;
			}
			if (long.TryParse(txtLinesToRead.Text, out var result) && (result < 0 || result > uint.MaxValue))
			{
				errorProvider.SetError(linesToReadExpressionButton, string.Format(LocalizedResourceMgr.GetString("FileManagementConfigurationForm.Error.InvalidLinesToReadValue"), 0, uint.MaxValue));
				return;
			}
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtLinesToRead.Text);
			errorProvider.SetError(linesToReadExpressionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("FileManagementConfigurationForm.Error.LinesToReadIsInvalid"));
		}
		else
		{
			errorProvider.SetError(linesToReadExpressionButton, string.Empty);
		}
	}

	private void TxtReadToEnd_TextChanged(object sender, EventArgs e)
	{
		txtLinesToRead.Enabled = txtReadToEnd.Text != "true";
		linesToReadExpressionButton.Enabled = txtLinesToRead.Enabled;
		TxtLinesToRead_Validating(txtLinesToRead, new CancelEventArgs());
	}

	private void TxtReadToEnd_Validating(object sender, CancelEventArgs e)
	{
		if (txtReadToEnd.Enabled)
		{
			if (string.IsNullOrEmpty(txtReadToEnd.Text))
			{
				errorProvider.SetError(readToEndExpressionButton, LocalizedResourceMgr.GetString("FileManagementConfigurationForm.Error.ReadToEndIsMandatory"));
				return;
			}
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtReadToEnd.Text);
			errorProvider.SetError(readToEndExpressionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("FileManagementConfigurationForm.Error.ReadToEndIsInvalid"));
		}
		else
		{
			errorProvider.SetError(readToEndExpressionButton, string.Empty);
		}
	}

	private void TxtContent_Validating(object sender, CancelEventArgs e)
	{
		if (txtContent.Enabled)
		{
			if (string.IsNullOrEmpty(txtContent.Text))
			{
				errorProvider.SetError(contentExpressionButton, LocalizedResourceMgr.GetString("FileManagementConfigurationForm.Error.ContentIsMandatory"));
				return;
			}
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtContent.Text);
			errorProvider.SetError(contentExpressionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("FileManagementConfigurationForm.Error.ContentIsInvalid"));
		}
		else
		{
			errorProvider.SetError(contentExpressionButton, string.Empty);
		}
	}

	private void RbAction_CheckedChanged(object sender, EventArgs e)
	{
		txtFirstLineToRead.Enabled = rbActionRead.Checked;
		txtLinesToRead.Enabled = rbActionRead.Checked && txtReadToEnd.Text != "true";
		txtReadToEnd.Enabled = rbActionRead.Checked;
		firstLineToReadExpressionButton.Enabled = txtFirstLineToRead.Enabled;
		linesToReadExpressionButton.Enabled = txtLinesToRead.Enabled;
		readToEndExpressionButton.Enabled = txtReadToEnd.Enabled;
		txtContent.Enabled = rbActionWrite.Checked;
		contentExpressionButton.Enabled = txtContent.Enabled;
		chkAppendFinalCrLf.Enabled = rbActionWrite.Checked;
		ComboOpenMode_SelectedIndexChanged(comboOpenMode, new EventArgs());
		TxtFirstLineToRead_Validating(txtFirstLineToRead, new CancelEventArgs());
		TxtLinesToRead_Validating(txtLinesToRead, new CancelEventArgs());
		TxtReadToEnd_Validating(txtReadToEnd, new CancelEventArgs());
		TxtContent_Validating(txtContent, new CancelEventArgs());
	}

	private void FileNameExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(fileManagementComponent)
		{
			Expression = txtFileName.Text
		};
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtFileName.Text = expressionEditorForm.Expression;
			TxtFileName_Validating(txtFileName, new CancelEventArgs());
		}
	}

	private void FirstLineToReadExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(fileManagementComponent)
		{
			Expression = txtFirstLineToRead.Text
		};
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtFirstLineToRead.Text = expressionEditorForm.Expression;
			TxtFirstLineToRead_Validating(txtFirstLineToRead, new CancelEventArgs());
		}
	}

	private void LinesToReadExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(fileManagementComponent)
		{
			Expression = txtLinesToRead.Text
		};
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtLinesToRead.Text = expressionEditorForm.Expression;
			TxtLinesToRead_Validating(txtLinesToRead, new CancelEventArgs());
		}
	}

	private void ReadToEndExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(fileManagementComponent)
		{
			Expression = txtReadToEnd.Text
		};
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtReadToEnd.Text = expressionEditorForm.Expression;
			TxtReadToEnd_Validating(txtReadToEnd, new CancelEventArgs());
		}
	}

	private void ContentExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(fileManagementComponent)
		{
			Expression = txtContent.Text
		};
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtContent.Text = expressionEditorForm.Expression;
			TxtContent_Validating(txtContent, new CancelEventArgs());
		}
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.OK;
		Close();
	}

	public FileManagementConfigurationForm(FileManagementComponent fileManagementComponent)
	{
		InitializeComponent();
		this.fileManagementComponent = fileManagementComponent;
		validVariables = ExpressionHelper.GetValidVariables(fileManagementComponent);
		comboOpenMode.Items.AddRange(new object[6]
		{
			FileManagementOpenModes.Append,
			FileManagementOpenModes.Create,
			FileManagementOpenModes.CreateNew,
			FileManagementOpenModes.Open,
			FileManagementOpenModes.OpenOrCreate,
			FileManagementOpenModes.Truncate
		});
		txtFileName.Text = fileManagementComponent.FileName;
		comboOpenMode.SelectedItem = fileManagementComponent.OpenMode;
		txtFirstLineToRead.Text = fileManagementComponent.FirstLineToRead;
		txtLinesToRead.Text = fileManagementComponent.LinesToRead;
		txtReadToEnd.Text = fileManagementComponent.ReadToEnd;
		txtContent.Text = fileManagementComponent.Content;
		chkAppendFinalCrLf.Checked = fileManagementComponent.AppendFinalCrLf;
		rbActionRead.Checked = fileManagementComponent.Action == FileManagementActions.Read;
		rbActionWrite.Checked = !rbActionRead.Checked;
		RbAction_CheckedChanged(rbActionRead, EventArgs.Empty);
		TxtFileName_Validating(txtFileName, new CancelEventArgs());
		Text = LocalizedResourceMgr.GetString("FileManagementConfigurationForm.Title");
		lblFileName.Text = LocalizedResourceMgr.GetString("FileManagementConfigurationForm.lblFileName.Text");
		lblOpenMode.Text = LocalizedResourceMgr.GetString("FileManagementConfigurationForm.lblOpenMode.Text");
		grpBoxAction.Text = LocalizedResourceMgr.GetString("FileManagementConfigurationForm.grpBoxAction.Text");
		rbActionRead.Text = LocalizedResourceMgr.GetString("FileManagementConfigurationForm.rbActionRead.Text");
		lblFirstLineToRead.Text = LocalizedResourceMgr.GetString("FileManagementConfigurationForm.lblFirstLineToRead.Text");
		lblLinesToRead.Text = LocalizedResourceMgr.GetString("FileManagementConfigurationForm.lblLinesToRead.Text");
		lblReadToEnd.Text = LocalizedResourceMgr.GetString("FileManagementConfigurationForm.lblReadToEnd.Text");
		rbActionWrite.Text = LocalizedResourceMgr.GetString("FileManagementConfigurationForm.rbActionWrite.Text");
		lblContent.Text = LocalizedResourceMgr.GetString("FileManagementConfigurationForm.lblContent.Text");
		chkAppendFinalCrLf.Text = LocalizedResourceMgr.GetString("FileManagementConfigurationForm.chkAppendFinalCrLf.Text");
		okButton.Text = LocalizedResourceMgr.GetString("FileManagementConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("FileManagementConfigurationForm.cancelButton.Text");
	}

	private void FileManagementConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		fileManagementComponent.ShowHelp();
	}

	private void FileManagementConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		fileManagementComponent.ShowHelp();
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
		this.cancelButton = new System.Windows.Forms.Button();
		this.okButton = new System.Windows.Forms.Button();
		this.lblContent = new System.Windows.Forms.Label();
		this.txtContent = new System.Windows.Forms.TextBox();
		this.grpBoxAction = new System.Windows.Forms.GroupBox();
		this.chkAppendFinalCrLf = new System.Windows.Forms.CheckBox();
		this.contentExpressionButton = new System.Windows.Forms.Button();
		this.readToEndExpressionButton = new System.Windows.Forms.Button();
		this.linesToReadExpressionButton = new System.Windows.Forms.Button();
		this.lblReadToEnd = new System.Windows.Forms.Label();
		this.txtReadToEnd = new System.Windows.Forms.TextBox();
		this.firstLineToReadExpressionButton = new System.Windows.Forms.Button();
		this.lblLinesToRead = new System.Windows.Forms.Label();
		this.lblFirstLineToRead = new System.Windows.Forms.Label();
		this.txtLinesToRead = new System.Windows.Forms.TextBox();
		this.txtFirstLineToRead = new System.Windows.Forms.TextBox();
		this.rbActionWrite = new System.Windows.Forms.RadioButton();
		this.rbActionRead = new System.Windows.Forms.RadioButton();
		this.comboOpenMode = new System.Windows.Forms.ComboBox();
		this.lblOpenMode = new System.Windows.Forms.Label();
		this.lblFileName = new System.Windows.Forms.Label();
		this.txtFileName = new System.Windows.Forms.TextBox();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.fileNameExpressionButton = new System.Windows.Forms.Button();
		this.grpBoxAction.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(503, 329);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 7;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(395, 329);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 6;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.lblContent.AutoSize = true;
		this.lblContent.Location = new System.Drawing.Point(35, 175);
		this.lblContent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblContent.Name = "lblContent";
		this.lblContent.Size = new System.Drawing.Size(106, 17);
		this.lblContent.TabIndex = 11;
		this.lblContent.Text = "Content to write";
		this.txtContent.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtContent.Location = new System.Drawing.Point(168, 171);
		this.txtContent.Margin = new System.Windows.Forms.Padding(4);
		this.txtContent.MaxLength = 8192;
		this.txtContent.Name = "txtContent";
		this.txtContent.Size = new System.Drawing.Size(347, 22);
		this.txtContent.TabIndex = 12;
		this.txtContent.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtContent.Validating += new System.ComponentModel.CancelEventHandler(TxtContent_Validating);
		this.grpBoxAction.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.grpBoxAction.Controls.Add(this.chkAppendFinalCrLf);
		this.grpBoxAction.Controls.Add(this.contentExpressionButton);
		this.grpBoxAction.Controls.Add(this.readToEndExpressionButton);
		this.grpBoxAction.Controls.Add(this.linesToReadExpressionButton);
		this.grpBoxAction.Controls.Add(this.lblReadToEnd);
		this.grpBoxAction.Controls.Add(this.txtReadToEnd);
		this.grpBoxAction.Controls.Add(this.firstLineToReadExpressionButton);
		this.grpBoxAction.Controls.Add(this.lblLinesToRead);
		this.grpBoxAction.Controls.Add(this.lblFirstLineToRead);
		this.grpBoxAction.Controls.Add(this.txtLinesToRead);
		this.grpBoxAction.Controls.Add(this.txtFirstLineToRead);
		this.grpBoxAction.Controls.Add(this.rbActionWrite);
		this.grpBoxAction.Controls.Add(this.rbActionRead);
		this.grpBoxAction.Controls.Add(this.lblContent);
		this.grpBoxAction.Controls.Add(this.txtContent);
		this.grpBoxAction.Location = new System.Drawing.Point(16, 73);
		this.grpBoxAction.Margin = new System.Windows.Forms.Padding(4);
		this.grpBoxAction.Name = "grpBoxAction";
		this.grpBoxAction.Padding = new System.Windows.Forms.Padding(4);
		this.grpBoxAction.Size = new System.Drawing.Size(587, 245);
		this.grpBoxAction.TabIndex = 5;
		this.grpBoxAction.TabStop = false;
		this.grpBoxAction.Text = "Action";
		this.chkAppendFinalCrLf.AutoSize = true;
		this.chkAppendFinalCrLf.Location = new System.Drawing.Point(39, 203);
		this.chkAppendFinalCrLf.Margin = new System.Windows.Forms.Padding(4);
		this.chkAppendFinalCrLf.Name = "chkAppendFinalCrLf";
		this.chkAppendFinalCrLf.Size = new System.Drawing.Size(152, 21);
		this.chkAppendFinalCrLf.TabIndex = 14;
		this.chkAppendFinalCrLf.Text = "Append final CR LF";
		this.chkAppendFinalCrLf.UseVisualStyleBackColor = true;
		this.contentExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.contentExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.contentExpressionButton.Location = new System.Drawing.Point(524, 169);
		this.contentExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.contentExpressionButton.Name = "contentExpressionButton";
		this.contentExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.contentExpressionButton.TabIndex = 13;
		this.contentExpressionButton.UseVisualStyleBackColor = true;
		this.contentExpressionButton.Click += new System.EventHandler(ContentExpressionButton_Click);
		this.readToEndExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.readToEndExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.readToEndExpressionButton.Location = new System.Drawing.Point(524, 106);
		this.readToEndExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.readToEndExpressionButton.Name = "readToEndExpressionButton";
		this.readToEndExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.readToEndExpressionButton.TabIndex = 9;
		this.readToEndExpressionButton.UseVisualStyleBackColor = true;
		this.readToEndExpressionButton.Click += new System.EventHandler(ReadToEndExpressionButton_Click);
		this.linesToReadExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.linesToReadExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.linesToReadExpressionButton.Location = new System.Drawing.Point(524, 74);
		this.linesToReadExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.linesToReadExpressionButton.Name = "linesToReadExpressionButton";
		this.linesToReadExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.linesToReadExpressionButton.TabIndex = 6;
		this.linesToReadExpressionButton.UseVisualStyleBackColor = true;
		this.linesToReadExpressionButton.Click += new System.EventHandler(LinesToReadExpressionButton_Click);
		this.lblReadToEnd.AutoSize = true;
		this.lblReadToEnd.Location = new System.Drawing.Point(35, 112);
		this.lblReadToEnd.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblReadToEnd.Name = "lblReadToEnd";
		this.lblReadToEnd.Size = new System.Drawing.Size(124, 17);
		this.lblReadToEnd.TabIndex = 7;
		this.lblReadToEnd.Text = "Read to end of file";
		this.txtReadToEnd.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtReadToEnd.Location = new System.Drawing.Point(168, 108);
		this.txtReadToEnd.Margin = new System.Windows.Forms.Padding(4);
		this.txtReadToEnd.MaxLength = 8192;
		this.txtReadToEnd.Name = "txtReadToEnd";
		this.txtReadToEnd.Size = new System.Drawing.Size(347, 22);
		this.txtReadToEnd.TabIndex = 8;
		this.txtReadToEnd.TextChanged += new System.EventHandler(TxtReadToEnd_TextChanged);
		this.txtReadToEnd.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtReadToEnd.Validating += new System.ComponentModel.CancelEventHandler(TxtReadToEnd_Validating);
		this.firstLineToReadExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.firstLineToReadExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.firstLineToReadExpressionButton.Location = new System.Drawing.Point(524, 41);
		this.firstLineToReadExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.firstLineToReadExpressionButton.Name = "firstLineToReadExpressionButton";
		this.firstLineToReadExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.firstLineToReadExpressionButton.TabIndex = 3;
		this.firstLineToReadExpressionButton.UseVisualStyleBackColor = true;
		this.firstLineToReadExpressionButton.Click += new System.EventHandler(FirstLineToReadExpressionButton_Click);
		this.lblLinesToRead.AutoSize = true;
		this.lblLinesToRead.Location = new System.Drawing.Point(35, 80);
		this.lblLinesToRead.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblLinesToRead.Name = "lblLinesToRead";
		this.lblLinesToRead.Size = new System.Drawing.Size(91, 17);
		this.lblLinesToRead.TabIndex = 4;
		this.lblLinesToRead.Text = "Lines to read";
		this.lblFirstLineToRead.AutoSize = true;
		this.lblFirstLineToRead.Location = new System.Drawing.Point(35, 48);
		this.lblFirstLineToRead.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblFirstLineToRead.Name = "lblFirstLineToRead";
		this.lblFirstLineToRead.Size = new System.Drawing.Size(110, 17);
		this.lblFirstLineToRead.TabIndex = 1;
		this.lblFirstLineToRead.Text = "First line to read";
		this.txtLinesToRead.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtLinesToRead.Location = new System.Drawing.Point(168, 76);
		this.txtLinesToRead.Margin = new System.Windows.Forms.Padding(4);
		this.txtLinesToRead.MaxLength = 8192;
		this.txtLinesToRead.Name = "txtLinesToRead";
		this.txtLinesToRead.Size = new System.Drawing.Size(347, 22);
		this.txtLinesToRead.TabIndex = 5;
		this.txtLinesToRead.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtLinesToRead.Validating += new System.ComponentModel.CancelEventHandler(TxtLinesToRead_Validating);
		this.txtFirstLineToRead.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtFirstLineToRead.Location = new System.Drawing.Point(168, 44);
		this.txtFirstLineToRead.Margin = new System.Windows.Forms.Padding(4);
		this.txtFirstLineToRead.MaxLength = 8192;
		this.txtFirstLineToRead.Name = "txtFirstLineToRead";
		this.txtFirstLineToRead.Size = new System.Drawing.Size(347, 22);
		this.txtFirstLineToRead.TabIndex = 2;
		this.txtFirstLineToRead.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtFirstLineToRead.Validating += new System.ComponentModel.CancelEventHandler(TxtFirstLineToRead_Validating);
		this.rbActionWrite.AutoSize = true;
		this.rbActionWrite.Location = new System.Drawing.Point(12, 143);
		this.rbActionWrite.Margin = new System.Windows.Forms.Padding(4);
		this.rbActionWrite.Name = "rbActionWrite";
		this.rbActionWrite.Size = new System.Drawing.Size(62, 21);
		this.rbActionWrite.TabIndex = 10;
		this.rbActionWrite.TabStop = true;
		this.rbActionWrite.Text = "Write";
		this.rbActionWrite.UseVisualStyleBackColor = true;
		this.rbActionWrite.CheckedChanged += new System.EventHandler(RbAction_CheckedChanged);
		this.rbActionRead.AutoSize = true;
		this.rbActionRead.Location = new System.Drawing.Point(12, 23);
		this.rbActionRead.Margin = new System.Windows.Forms.Padding(4);
		this.rbActionRead.Name = "rbActionRead";
		this.rbActionRead.Size = new System.Drawing.Size(63, 21);
		this.rbActionRead.TabIndex = 0;
		this.rbActionRead.TabStop = true;
		this.rbActionRead.Text = "Read";
		this.rbActionRead.UseVisualStyleBackColor = true;
		this.rbActionRead.CheckedChanged += new System.EventHandler(RbAction_CheckedChanged);
		this.comboOpenMode.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboOpenMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboOpenMode.FormattingEnabled = true;
		this.comboOpenMode.Location = new System.Drawing.Point(108, 39);
		this.comboOpenMode.Margin = new System.Windows.Forms.Padding(4);
		this.comboOpenMode.Name = "comboOpenMode";
		this.comboOpenMode.Size = new System.Drawing.Size(423, 24);
		this.comboOpenMode.TabIndex = 4;
		this.comboOpenMode.SelectedIndexChanged += new System.EventHandler(ComboOpenMode_SelectedIndexChanged);
		this.lblOpenMode.AutoSize = true;
		this.lblOpenMode.Location = new System.Drawing.Point(16, 43);
		this.lblOpenMode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblOpenMode.Name = "lblOpenMode";
		this.lblOpenMode.Size = new System.Drawing.Size(82, 17);
		this.lblOpenMode.TabIndex = 3;
		this.lblOpenMode.Text = "Open Mode";
		this.lblFileName.AutoSize = true;
		this.lblFileName.Location = new System.Drawing.Point(16, 11);
		this.lblFileName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblFileName.Name = "lblFileName";
		this.lblFileName.Size = new System.Drawing.Size(71, 17);
		this.lblFileName.TabIndex = 0;
		this.lblFileName.Text = "File Name";
		this.txtFileName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtFileName.Location = new System.Drawing.Point(108, 7);
		this.txtFileName.Margin = new System.Windows.Forms.Padding(4);
		this.txtFileName.MaxLength = 2048;
		this.txtFileName.Name = "txtFileName";
		this.txtFileName.Size = new System.Drawing.Size(423, 22);
		this.txtFileName.TabIndex = 1;
		this.txtFileName.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtFileName.Validating += new System.ComponentModel.CancelEventHandler(TxtFileName_Validating);
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.fileNameExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.fileNameExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.fileNameExpressionButton.Location = new System.Drawing.Point(540, 5);
		this.fileNameExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.fileNameExpressionButton.Name = "fileNameExpressionButton";
		this.fileNameExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.fileNameExpressionButton.TabIndex = 2;
		this.fileNameExpressionButton.UseVisualStyleBackColor = true;
		this.fileNameExpressionButton.Click += new System.EventHandler(FileNameExpressionButton_Click);
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(619, 372);
		base.Controls.Add(this.fileNameExpressionButton);
		base.Controls.Add(this.lblFileName);
		base.Controls.Add(this.comboOpenMode);
		base.Controls.Add(this.txtFileName);
		base.Controls.Add(this.grpBoxAction);
		base.Controls.Add(this.lblOpenMode);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(1359, 875);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(634, 409);
		base.Name = "FileManagementConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Read / Write to File";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(FileManagementConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(FileManagementConfigurationForm_HelpRequested);
		this.grpBoxAction.ResumeLayout(false);
		this.grpBoxAction.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
