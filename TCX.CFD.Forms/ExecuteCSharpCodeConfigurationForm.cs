using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;
using TCX.CFD.Properties;

namespace TCX.CFD.Forms;

public class ExecuteCSharpCodeConfigurationForm : Form
{
	private readonly ExecuteCSharpCodeComponent executeCSharpCodeComponent;

	private bool isCodeSet;

	private IContainer components;

	private Button cancelButton;

	private Button okButton;

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

	public string Code
	{
		get
		{
			if (!isCodeSet)
			{
				return string.Empty;
			}
			return txtCode.Text;
		}
	}

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

	public ExecuteCSharpCodeConfigurationForm(ExecuteCSharpCodeComponent executeCSharpCodeComponent)
	{
		InitializeComponent();
		typeDataGridViewComboBoxColumn.ValueType = typeof(ScriptParameterTypes);
		typeDataGridViewComboBoxColumn.DataSource = Enum.GetValues(typeof(ScriptParameterTypes));
		this.executeCSharpCodeComponent = executeCSharpCodeComponent;
		txtMethodName.Text = executeCSharpCodeComponent.MethodName;
		TxtMethodName_Validating(txtMethodName, new CancelEventArgs());
		chkReturnsValue.Checked = executeCSharpCodeComponent.ReturnsValue;
		SetCode(executeCSharpCodeComponent.Code);
		TxtCode_Validating(txtCode, new CancelEventArgs());
		foreach (ScriptParameter parameter in executeCSharpCodeComponent.Parameters)
		{
			parameterBindingSource.List.Add(parameter);
		}
		Text = LocalizedResourceMgr.GetString("ExecuteCSharpCodeConfigurationForm.Title");
		lblMethodName.Text = LocalizedResourceMgr.GetString("ExecuteCSharpCodeConfigurationForm.lblMethodName.Text");
		chkReturnsValue.Text = LocalizedResourceMgr.GetString("ExecuteCSharpCodeConfigurationForm.chkReturnsValue.Text");
		lblCode.Text = LocalizedResourceMgr.GetString("ExecuteCSharpCodeConfigurationForm.lblCode.Text");
		lblParameters.Text = LocalizedResourceMgr.GetString("ExecuteCSharpCodeConfigurationForm.lblParameters.Text");
		okButton.Text = LocalizedResourceMgr.GetString("ExecuteCSharpCodeConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("ExecuteCSharpCodeConfigurationForm.cancelButton.Text");
	}

	private void SetCode(string code)
	{
		if (string.IsNullOrEmpty(code))
		{
			txtCode.Text = "if (1 + 1 == 2) return true;";
			txtCode.ForeColor = SystemColors.ControlDarkDark;
			isCodeSet = false;
		}
		else
		{
			txtCode.Text = code;
			txtCode.ForeColor = SystemColors.WindowText;
			isCodeSet = true;
		}
	}

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void TxtCode_Enter(object sender, EventArgs e)
	{
		if (isCodeSet)
		{
			(sender as TextBox).SelectAll();
			return;
		}
		txtCode.Text = string.Empty;
		txtCode.ForeColor = SystemColors.WindowText;
	}

	private void TxtCode_Leave(object sender, EventArgs e)
	{
		SetCode(txtCode.Text);
	}

	private void TxtMethodName_Validating(object sender, CancelEventArgs e)
	{
		errorProvider.SetError(txtMethodName, string.IsNullOrEmpty(txtMethodName.Text) ? LocalizedResourceMgr.GetString("ExecuteCSharpCodeConfigurationForm.Error.MethodNameIsMandatory") : string.Empty);
	}

	private void TxtCode_Validating(object sender, CancelEventArgs e)
	{
		errorProvider.SetError(txtCode, isCodeSet ? string.Empty : LocalizedResourceMgr.GetString("ExecuteCSharpCodeConfigurationForm.Error.CodeIsMandatory"));
	}

	private void ParametersGrid_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		if (e == null || e.ColumnIndex != 3 || e.RowIndex < 0)
		{
			return;
		}
		if (parametersGrid == null)
		{
			MessageBox.Show("parametersGrid is null, something is wrong, can't show dialog...", LocalizedResourceMgr.GetString("ExecuteCSharpCodeConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			return;
		}
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(executeCSharpCodeComponent)
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
				MessageBox.Show(LocalizedResourceMgr.GetString("ExecuteCSharpCodeConfigurationForm.MessageBox.Error.EmptyValuesNotAllowed"), LocalizedResourceMgr.GetString("ExecuteCSharpCodeConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				parameterBindingSource.Position = parameterBindingSource.IndexOf(item);
				return;
			}
			if (list.Contains(item.Name))
			{
				MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("ExecuteCSharpCodeConfigurationForm.MessageBox.Error.DuplicatedNamesNotAllowed"), item.Name), LocalizedResourceMgr.GetString("ExecuteCSharpCodeConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				parameterBindingSource.Position = parameterBindingSource.IndexOf(item);
				return;
			}
			list.Add(item.Name);
		}
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void ExecuteCSharpCodeConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		executeCSharpCodeComponent.ShowHelp();
	}

	private void ExecuteCSharpCodeConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		executeCSharpCodeComponent.ShowHelp();
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
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(490, 590);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 8;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(382, 590);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 7;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.lblMethodName.AutoSize = true;
		this.lblMethodName.Location = new System.Drawing.Point(13, 16);
		this.lblMethodName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblMethodName.Name = "lblMethodName";
		this.lblMethodName.Size = new System.Drawing.Size(96, 17);
		this.lblMethodName.TabIndex = 0;
		this.lblMethodName.Text = "Method Name";
		this.txtMethodName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtMethodName.Location = new System.Drawing.Point(117, 13);
		this.txtMethodName.Margin = new System.Windows.Forms.Padding(4);
		this.txtMethodName.MaxLength = 1024;
		this.txtMethodName.Name = "txtMethodName";
		this.txtMethodName.Size = new System.Drawing.Size(473, 22);
		this.txtMethodName.TabIndex = 1;
		this.txtMethodName.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtMethodName.Validating += new System.ComponentModel.CancelEventHandler(TxtMethodName_Validating);
		this.lblParameters.AutoSize = true;
		this.lblParameters.Location = new System.Drawing.Point(13, 80);
		this.lblParameters.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblParameters.Name = "lblParameters";
		this.lblParameters.Size = new System.Drawing.Size(120, 17);
		this.lblParameters.TabIndex = 3;
		this.lblParameters.Text = "Input Parameters:";
		this.parametersGrid.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.parametersGrid.AutoGenerateColumns = false;
		this.parametersGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
		this.parametersGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
		this.parametersGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.parametersGrid.Columns.AddRange(this.nameDataGridViewTextBoxColumn, this.typeDataGridViewComboBoxColumn, this.valueDataGridViewTextBoxColumn, this.expressionColumn);
		this.parametersGrid.DataSource = this.parameterBindingSource;
		this.parametersGrid.Location = new System.Drawing.Point(16, 101);
		this.parametersGrid.Margin = new System.Windows.Forms.Padding(4);
		this.parametersGrid.Name = "parametersGrid";
		this.parametersGrid.Size = new System.Drawing.Size(574, 204);
		this.parametersGrid.TabIndex = 4;
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
		this.chkReturnsValue.AutoSize = true;
		this.chkReturnsValue.Location = new System.Drawing.Point(16, 45);
		this.chkReturnsValue.Name = "chkReturnsValue";
		this.chkReturnsValue.Size = new System.Drawing.Size(176, 21);
		this.chkReturnsValue.TabIndex = 2;
		this.chkReturnsValue.Text = "Method returns a value";
		this.chkReturnsValue.UseVisualStyleBackColor = true;
		this.txtCode.AcceptsReturn = true;
		this.txtCode.AcceptsTab = true;
		this.txtCode.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtCode.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
		this.txtCode.Location = new System.Drawing.Point(117, 312);
		this.txtCode.Multiline = true;
		this.txtCode.Name = "txtCode";
		this.txtCode.ScrollBars = System.Windows.Forms.ScrollBars.Both;
		this.txtCode.Size = new System.Drawing.Size(473, 271);
		this.txtCode.TabIndex = 6;
		this.txtCode.Text = "return a + b;";
		this.txtCode.WordWrap = false;
		this.txtCode.Enter += new System.EventHandler(TxtCode_Enter);
		this.txtCode.Leave += new System.EventHandler(TxtCode_Leave);
		this.txtCode.Validating += new System.ComponentModel.CancelEventHandler(TxtCode_Validating);
		this.lblCode.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.lblCode.AutoSize = true;
		this.lblCode.Location = new System.Drawing.Point(13, 315);
		this.lblCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblCode.Name = "lblCode";
		this.lblCode.Size = new System.Drawing.Size(62, 17);
		this.lblCode.TabIndex = 5;
		this.lblCode.Text = "C# Code";
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(616, 633);
		base.Controls.Add(this.lblCode);
		base.Controls.Add(this.txtCode);
		base.Controls.Add(this.chkReturnsValue);
		base.Controls.Add(this.parametersGrid);
		base.Controls.Add(this.lblParameters);
		base.Controls.Add(this.lblMethodName);
		base.Controls.Add(this.txtMethodName);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(1359, 875);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(634, 680);
		base.Name = "ExecuteCSharpCodeConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Execute C# Code";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(ExecuteCSharpCodeConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(ExecuteCSharpCodeConfigurationForm_HelpRequested);
		((System.ComponentModel.ISupportInitialize)this.parametersGrid).EndInit();
		((System.ComponentModel.ISupportInitialize)this.parameterBindingSource).EndInit();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
