using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;
using TCX.CFD.Properties;

namespace TCX.CFD.Forms;

public class ExternalCodeExecutionConfigurationForm : Form
{
	private readonly ExternalCodeExecutionComponent externalCodeExecutionComponent;

	private readonly string librariesDirectory;

	private IContainer components;

	private ComboBox comboFile;

	private Label lblFile;

	private Button cancelButton;

	private Button okButton;

	private Label lblClassName;

	private TextBox txtClassName;

	private Label lblMethodName;

	private TextBox txtMethodName;

	private Label lblParameters;

	private DataGridView parametersGrid;

	private BindingSource parameterBindingSource;

	private ErrorProvider errorProvider;

	private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;

	private DataGridViewComboBoxColumn typeDataGridViewComboBoxColumn;

	private DataGridViewTextBoxColumn valueDataGridViewTextBoxColumn;

	private DataGridViewButtonColumn expressionColumn;

	private CheckBox chkReturnsValue;

	private Label lblCode;

	private TextBox txtCode;

	public string LibraryFileName => comboFile.Text;

	public string ClassName => txtClassName.Text;

	public string MethodName => txtMethodName.Text;

	public bool ReturnsValue => chkReturnsValue.Checked;

	public List<ScriptParameter> Parameters
	{
		get
		{
			List<ScriptParameter> list = new List<ScriptParameter>();
			foreach (ScriptParameter item in parameterBindingSource.List)
			{
				list.Add(item);
			}
			return list;
		}
	}

	private void FillComboFile()
	{
		comboFile.Items.Clear();
		DirectoryInfo directoryInfo = new DirectoryInfo(librariesDirectory);
		if (directoryInfo.Exists)
		{
			string[] array = new string[1] { "*.cs" };
			foreach (string searchPattern in array)
			{
				FileInfo[] files = directoryInfo.GetFiles(searchPattern, SearchOption.TopDirectoryOnly);
				foreach (FileInfo fileInfo in files)
				{
					comboFile.Items.Add(fileInfo.Name);
				}
			}
		}
		comboFile.Items.Add(LocalizedResourceMgr.GetString("FileTypeConverters.Browse.Text"));
	}

	public ExternalCodeExecutionConfigurationForm(ExternalCodeExecutionComponent externalCodeExecutionComponent)
	{
		InitializeComponent();
		typeDataGridViewComboBoxColumn.ValueType = typeof(ScriptParameterTypes);
		typeDataGridViewComboBoxColumn.DataSource = Enum.GetValues(typeof(ScriptParameterTypes));
		this.externalCodeExecutionComponent = externalCodeExecutionComponent;
		librariesDirectory = Path.Combine(externalCodeExecutionComponent.GetRootFlow().FileObject.GetProjectObject().GetFolderPath(), "Libraries");
		FillComboFile();
		comboFile.Text = externalCodeExecutionComponent.LibraryFileName;
		txtClassName.Text = externalCodeExecutionComponent.ClassName;
		TxtClassName_Validating(txtClassName, new CancelEventArgs());
		txtMethodName.Text = externalCodeExecutionComponent.MethodName;
		TxtMethodName_Validating(txtMethodName, new CancelEventArgs());
		chkReturnsValue.Checked = externalCodeExecutionComponent.ReturnsValue;
		foreach (ScriptParameter parameter in externalCodeExecutionComponent.Parameters)
		{
			parameterBindingSource.List.Add(parameter);
		}
		Text = LocalizedResourceMgr.GetString("ExternalCodeExecutionConfigurationForm.Title");
		lblFile.Text = LocalizedResourceMgr.GetString("ExternalCodeExecutionConfigurationForm.lblFile.Text");
		lblCode.Text = LocalizedResourceMgr.GetString("ExternalCodeExecutionConfigurationForm.lblCode.Text");
		lblClassName.Text = LocalizedResourceMgr.GetString("ExternalCodeExecutionConfigurationForm.lblClassName.Text");
		lblMethodName.Text = LocalizedResourceMgr.GetString("ExternalCodeExecutionConfigurationForm.lblMethodName.Text");
		chkReturnsValue.Text = LocalizedResourceMgr.GetString("ExternalCodeExecutionConfigurationForm.chkReturnsValue.Text");
		lblParameters.Text = LocalizedResourceMgr.GetString("ExternalCodeExecutionConfigurationForm.lblParameters.Text");
		okButton.Text = LocalizedResourceMgr.GetString("ExternalCodeExecutionConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("ExternalCodeExecutionConfigurationForm.cancelButton.Text");
		ComboLibraryFileName_SelectedIndexChanged(comboFile, EventArgs.Empty);
	}

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void ComboLibraryFileName_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (comboFile.Text == LocalizedResourceMgr.GetString("FileTypeConverters.Browse.Text"))
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.DefaultExt = "cs";
			openFileDialog.Filter = "CS Files (*.cs)|*.cs";
			openFileDialog.RestoreDirectory = true;
			openFileDialog.SupportMultiDottedExtensions = true;
			openFileDialog.Title = "Open CS File";
			openFileDialog.FileName = string.Empty;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				try
				{
					FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
					string text = Path.Combine(librariesDirectory, fileInfo.Name);
					if (Path.GetFullPath(text).ToLower() != Path.GetFullPath(openFileDialog.FileName).ToLower())
					{
						File.Copy(openFileDialog.FileName, text, overwrite: true);
						FillComboFile();
					}
					comboFile.SelectedText = fileInfo.Name;
					comboFile.SelectedItem = fileInfo.Name;
					return;
				}
				catch (Exception exc)
				{
					MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("ExternalCodeExecutionConfigurationForm.MessageBox.Error.CopyingFile"), ErrorHelper.GetErrorDescription(exc)), LocalizedResourceMgr.GetString("ExternalCodeExecutionConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
			}
			comboFile.SelectedText = string.Empty;
			comboFile.SelectedItem = null;
			return;
		}
		try
		{
			string text2 = File.ReadAllText(Path.Combine(librariesDirectory, comboFile.Text));
			txtCode.Text = text2;
		}
		catch (Exception)
		{
			txtCode.Text = LocalizedResourceMgr.GetString("ExternalCodeExecutionConfigurationForm.Error.FileCouldNotBeLoaded");
		}
	}

	private void ComboFile_Validating(object sender, CancelEventArgs e)
	{
		if (comboFile.Enabled)
		{
			if (string.IsNullOrEmpty(comboFile.Text))
			{
				errorProvider.SetError(comboFile, LocalizedResourceMgr.GetString("ExternalCodeExecutionConfigurationForm.Error.FileIsMandatory"));
			}
			else
			{
				errorProvider.SetError(comboFile, File.Exists(Path.Combine(librariesDirectory, comboFile.Text)) ? string.Empty : LocalizedResourceMgr.GetString("ExternalCodeExecutionConfigurationForm.Error.InvalidFilePath"));
			}
		}
		else
		{
			errorProvider.SetError(comboFile, string.Empty);
		}
	}

	private void TxtClassName_Validating(object sender, CancelEventArgs e)
	{
		errorProvider.SetError(txtClassName, string.IsNullOrEmpty(txtClassName.Text) ? LocalizedResourceMgr.GetString("ExternalCodeExecutionConfigurationForm.Error.ClassNameIsMandatory") : string.Empty);
	}

	private void TxtMethodName_Validating(object sender, CancelEventArgs e)
	{
		errorProvider.SetError(txtMethodName, string.IsNullOrEmpty(txtMethodName.Text) ? LocalizedResourceMgr.GetString("ExternalCodeExecutionConfigurationForm.Error.MethodNameIsMandatory") : string.Empty);
	}

	private void ParametersGrid_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		if (e == null || e.ColumnIndex != 3 || e.RowIndex < 0)
		{
			return;
		}
		if (parametersGrid == null)
		{
			MessageBox.Show("parametersGrid is null, something is wrong, can't show dialog...", LocalizedResourceMgr.GetString("ExternalCodeExecutionConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			return;
		}
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(externalCodeExecutionComponent)
		{
			Expression = ((parametersGrid[2, e.RowIndex]?.Value == null) ? string.Empty : parametersGrid[2, e.RowIndex].Value.ToString())
		};
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			parametersGrid.CurrentCell = parametersGrid[2, e.RowIndex];
			SendKeys.SendWait(" ");
			parametersGrid.EndEdit();
			parametersGrid.CurrentCell = null;
			parametersGrid[2, e.RowIndex].Value = expressionEditorForm.Expression;
		}
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		List<string> list = new List<string>();
		foreach (ScriptParameter item in parameterBindingSource.List)
		{
			if (string.IsNullOrEmpty(item.Name) || string.IsNullOrEmpty(item.Value))
			{
				MessageBox.Show(LocalizedResourceMgr.GetString("ExternalCodeExecutionConfigurationForm.MessageBox.Error.EmptyValuesNotAllowed"), LocalizedResourceMgr.GetString("ExternalCodeExecutionConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				parameterBindingSource.Position = parameterBindingSource.IndexOf(item);
				return;
			}
			if (list.Contains(item.Name))
			{
				MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("ExternalCodeExecutionConfigurationForm.MessageBox.Error.DuplicatedNamesNotAllowed"), item.Name), LocalizedResourceMgr.GetString("ExternalCodeExecutionConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				parameterBindingSource.Position = parameterBindingSource.IndexOf(item);
				return;
			}
			list.Add(item.Name);
		}
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void ExternalCodeExecutionConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		externalCodeExecutionComponent.ShowHelp();
	}

	private void ExternalCodeExecutionConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		externalCodeExecutionComponent.ShowHelp();
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
		this.comboFile = new System.Windows.Forms.ComboBox();
		this.lblFile = new System.Windows.Forms.Label();
		this.cancelButton = new System.Windows.Forms.Button();
		this.okButton = new System.Windows.Forms.Button();
		this.lblClassName = new System.Windows.Forms.Label();
		this.txtClassName = new System.Windows.Forms.TextBox();
		this.lblMethodName = new System.Windows.Forms.Label();
		this.txtMethodName = new System.Windows.Forms.TextBox();
		this.lblParameters = new System.Windows.Forms.Label();
		this.parametersGrid = new System.Windows.Forms.DataGridView();
		this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.typeDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
		this.valueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.expressionColumn = new System.Windows.Forms.DataGridViewButtonColumn();
		this.parameterBindingSource = new System.Windows.Forms.BindingSource(this.components);
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.chkReturnsValue = new System.Windows.Forms.CheckBox();
		this.txtCode = new System.Windows.Forms.TextBox();
		this.lblCode = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.parametersGrid).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.parameterBindingSource).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.comboFile.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboFile.Location = new System.Drawing.Point(117, 6);
		this.comboFile.Margin = new System.Windows.Forms.Padding(4);
		this.comboFile.MaxLength = 1024;
		this.comboFile.Name = "comboFile";
		this.comboFile.Size = new System.Drawing.Size(476, 24);
		this.comboFile.TabIndex = 1;
		this.comboFile.SelectedIndexChanged += new System.EventHandler(ComboLibraryFileName_SelectedIndexChanged);
		this.comboFile.Validating += new System.ComponentModel.CancelEventHandler(ComboFile_Validating);
		this.lblFile.AutoSize = true;
		this.lblFile.Location = new System.Drawing.Point(13, 9);
		this.lblFile.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblFile.Name = "lblFile";
		this.lblFile.Size = new System.Drawing.Size(51, 17);
		this.lblFile.TabIndex = 0;
		this.lblFile.Text = "C# File";
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(493, 590);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 12;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(385, 590);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 11;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.lblClassName.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.lblClassName.AutoSize = true;
		this.lblClassName.Location = new System.Drawing.Point(13, 264);
		this.lblClassName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblClassName.Name = "lblClassName";
		this.lblClassName.Size = new System.Drawing.Size(83, 17);
		this.lblClassName.TabIndex = 4;
		this.lblClassName.Text = "Class Name";
		this.txtClassName.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtClassName.Location = new System.Drawing.Point(117, 261);
		this.txtClassName.Margin = new System.Windows.Forms.Padding(4);
		this.txtClassName.MaxLength = 1024;
		this.txtClassName.Name = "txtClassName";
		this.txtClassName.Size = new System.Drawing.Size(476, 22);
		this.txtClassName.TabIndex = 5;
		this.txtClassName.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtClassName.Validating += new System.ComponentModel.CancelEventHandler(TxtClassName_Validating);
		this.lblMethodName.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.lblMethodName.AutoSize = true;
		this.lblMethodName.Location = new System.Drawing.Point(13, 296);
		this.lblMethodName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblMethodName.Name = "lblMethodName";
		this.lblMethodName.Size = new System.Drawing.Size(96, 17);
		this.lblMethodName.TabIndex = 6;
		this.lblMethodName.Text = "Method Name";
		this.txtMethodName.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtMethodName.Location = new System.Drawing.Point(117, 293);
		this.txtMethodName.Margin = new System.Windows.Forms.Padding(4);
		this.txtMethodName.MaxLength = 1024;
		this.txtMethodName.Name = "txtMethodName";
		this.txtMethodName.Size = new System.Drawing.Size(476, 22);
		this.txtMethodName.TabIndex = 7;
		this.txtMethodName.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtMethodName.Validating += new System.ComponentModel.CancelEventHandler(TxtMethodName_Validating);
		this.lblParameters.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.lblParameters.AutoSize = true;
		this.lblParameters.Location = new System.Drawing.Point(13, 358);
		this.lblParameters.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblParameters.Name = "lblParameters";
		this.lblParameters.Size = new System.Drawing.Size(120, 17);
		this.lblParameters.TabIndex = 9;
		this.lblParameters.Text = "Input Parameters:";
		this.parametersGrid.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.parametersGrid.AutoGenerateColumns = false;
		this.parametersGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
		this.parametersGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
		this.parametersGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.parametersGrid.Columns.AddRange(this.nameDataGridViewTextBoxColumn, this.typeDataGridViewComboBoxColumn, this.valueDataGridViewTextBoxColumn, this.expressionColumn);
		this.parametersGrid.DataSource = this.parameterBindingSource;
		this.parametersGrid.Location = new System.Drawing.Point(16, 379);
		this.parametersGrid.Margin = new System.Windows.Forms.Padding(4);
		this.parametersGrid.Name = "parametersGrid";
		this.parametersGrid.Size = new System.Drawing.Size(577, 204);
		this.parametersGrid.TabIndex = 10;
		this.parametersGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(ParametersGrid_CellClick);
		this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
		this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
		this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
		this.typeDataGridViewComboBoxColumn.DataPropertyName = "Type";
		this.typeDataGridViewComboBoxColumn.DisplayStyleForCurrentCellOnly = true;
		this.typeDataGridViewComboBoxColumn.HeaderText = "Type";
		this.typeDataGridViewComboBoxColumn.Name = "typeDataGridViewComboBoxColumn";
		this.typeDataGridViewComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.typeDataGridViewComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
		this.valueDataGridViewTextBoxColumn.DataPropertyName = "Value";
		this.valueDataGridViewTextBoxColumn.HeaderText = "Value";
		this.valueDataGridViewTextBoxColumn.Name = "valueDataGridViewTextBoxColumn";
		dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle.NullValue = "...";
		this.expressionColumn.DefaultCellStyle = dataGridViewCellStyle;
		this.expressionColumn.FillWeight = 20f;
		this.expressionColumn.HeaderText = "";
		this.expressionColumn.Name = "expressionColumn";
		this.parameterBindingSource.DataSource = typeof(TCX.CFD.Classes.Components.ScriptParameter);
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.chkReturnsValue.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.chkReturnsValue.AutoSize = true;
		this.chkReturnsValue.Location = new System.Drawing.Point(16, 327);
		this.chkReturnsValue.Name = "chkReturnsValue";
		this.chkReturnsValue.Size = new System.Drawing.Size(176, 21);
		this.chkReturnsValue.TabIndex = 8;
		this.chkReturnsValue.Text = "Method returns a value";
		this.chkReturnsValue.UseVisualStyleBackColor = true;
		this.txtCode.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtCode.Location = new System.Drawing.Point(117, 38);
		this.txtCode.Multiline = true;
		this.txtCode.Name = "txtCode";
		this.txtCode.ReadOnly = true;
		this.txtCode.ScrollBars = System.Windows.Forms.ScrollBars.Both;
		this.txtCode.Size = new System.Drawing.Size(476, 216);
		this.txtCode.TabIndex = 3;
		this.txtCode.WordWrap = false;
		this.txtCode.Enter += new System.EventHandler(TxtBox_Enter);
		this.lblCode.AutoSize = true;
		this.lblCode.Location = new System.Drawing.Point(13, 41);
		this.lblCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblCode.Name = "lblCode";
		this.lblCode.Size = new System.Drawing.Size(62, 17);
		this.lblCode.TabIndex = 2;
		this.lblCode.Text = "C# Code";
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(619, 633);
		base.Controls.Add(this.lblCode);
		base.Controls.Add(this.txtCode);
		base.Controls.Add(this.chkReturnsValue);
		base.Controls.Add(this.parametersGrid);
		base.Controls.Add(this.lblParameters);
		base.Controls.Add(this.lblMethodName);
		base.Controls.Add(this.txtMethodName);
		base.Controls.Add(this.lblClassName);
		base.Controls.Add(this.txtClassName);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.Controls.Add(this.lblFile);
		base.Controls.Add(this.comboFile);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(1359, 875);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(634, 680);
		base.Name = "ExternalCodeExecutionConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Execute C# File";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(ExternalCodeExecutionConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(ExternalCodeExecutionConfigurationForm_HelpRequested);
		((System.ComponentModel.ISupportInitialize)this.parametersGrid).EndInit();
		((System.ComponentModel.ISupportInitialize)this.parameterBindingSource).EndInit();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
