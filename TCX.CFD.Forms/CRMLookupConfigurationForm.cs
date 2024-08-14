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

public class CRMLookupConfigurationForm : Form
{
	private readonly CRMLookupComponent crmLookupComponent;

	private readonly List<string> validVariables;

	private IContainer components;

	private Button cancelButton;

	private Button okButton;

	private Label lblResponseMappings;

	private DataGridView responseMappingsGrid;

	private BindingSource responseMappingsBindingSource;

	private Button lookupInputParameterExpressionButton;

	private TextBox txtLookupInputParameter;

	private Label lblLookupInputParameter;

	private ComboBox comboEntity;

	private Label lblEntity;

	private ErrorProvider errorProvider;

	private ComboBox comboLookupBy;

	private Label lblLookupBy;

	private DataGridViewTextBoxColumn pathDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn variableDataGridViewTextBoxColumn;

	private DataGridViewButtonColumn expressionBuilderColumn;

	public CRMEntities Entity => (CRMEntities)comboEntity.SelectedItem;

	public CRMLookupBy LookupBy
	{
		get
		{
			if (comboLookupBy.SelectedIndex != 0)
			{
				if (comboLookupBy.SelectedIndex != 1)
				{
					return CRMLookupBy.CustomQuery;
				}
				return CRMLookupBy.EntityID;
			}
			return CRMLookupBy.EntityNumber;
		}
	}

	public string LookupInputParameter => txtLookupInputParameter.Text;

	public List<ResponseMapping> ResponseMappings
	{
		get
		{
			List<ResponseMapping> list = new List<ResponseMapping>();
			foreach (ResponseMapping item in responseMappingsBindingSource.List)
			{
				list.Add(item);
			}
			return list;
		}
	}

	public CRMLookupConfigurationForm(CRMLookupComponent crmLookupComponent)
	{
		InitializeComponent();
		this.crmLookupComponent = crmLookupComponent;
		validVariables = ExpressionHelper.GetValidVariables(crmLookupComponent);
		comboEntity.Items.AddRange(new object[3]
		{
			CRMEntities.Contacts,
			CRMEntities.Leads,
			CRMEntities.Accounts
		});
		comboLookupBy.Items.AddRange(new object[3] { "Entity Number", "Entity ID", "Custom Query" });
		comboEntity.SelectedItem = crmLookupComponent.Entity;
		comboLookupBy.SelectedIndex = ((crmLookupComponent.LookupBy != 0) ? ((crmLookupComponent.LookupBy == CRMLookupBy.EntityID) ? 1 : 2) : 0);
		txtLookupInputParameter.Text = crmLookupComponent.LookupInputParameter;
		TxtLookupInputParameter_Validating(txtLookupInputParameter, new CancelEventArgs());
		foreach (ResponseMapping responseMapping in crmLookupComponent.ResponseMappings)
		{
			responseMappingsBindingSource.List.Add(responseMapping);
		}
		Text = LocalizedResourceMgr.GetString("CRMLookupConfigurationForm.Title");
		lblEntity.Text = LocalizedResourceMgr.GetString("CRMLookupConfigurationForm.lblEntity.Text");
		lblLookupBy.Text = LocalizedResourceMgr.GetString("CRMLookupConfigurationForm.lblLookupBy.Text");
		lblLookupInputParameter.Text = LocalizedResourceMgr.GetString("CRMLookupConfigurationForm.lblLookupInputParameter.Text");
		lblResponseMappings.Text = LocalizedResourceMgr.GetString("CRMLookupConfigurationForm.lblResponseMappings.Text");
		okButton.Text = LocalizedResourceMgr.GetString("CRMLookupConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("CRMLookupConfigurationForm.cancelButton.Text");
	}

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void TxtLookupInputParameter_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtLookupInputParameter.Text))
		{
			errorProvider.SetError(lookupInputParameterExpressionButton, LocalizedResourceMgr.GetString("CRMLookupConfigurationForm.Error.LookupInputParameterIsMandatory"));
			return;
		}
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtLookupInputParameter.Text);
		errorProvider.SetError(lookupInputParameterExpressionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("CRMLookupConfigurationForm.Error.LookupInputParameterIsInvalid"));
	}

	private void LookupInputParameterExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(crmLookupComponent)
		{
			Expression = txtLookupInputParameter.Text
		};
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtLookupInputParameter.Text = expressionEditorForm.Expression;
			TxtLookupInputParameter_Validating(txtLookupInputParameter, new CancelEventArgs());
		}
	}

	private void ResponseMappingsGrid_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		if (e.ColumnIndex == 2 && e.RowIndex >= 0)
		{
			LeftHandSideVariableSelectorForm leftHandSideVariableSelectorForm = new LeftHandSideVariableSelectorForm
			{
				Component = crmLookupComponent,
				VariableName = ((responseMappingsGrid[1, e.RowIndex].Value == null) ? string.Empty : responseMappingsGrid[1, e.RowIndex].Value.ToString())
			};
			if (leftHandSideVariableSelectorForm.ShowDialog() == DialogResult.OK)
			{
				responseMappingsGrid.CurrentCell = responseMappingsGrid[1, e.RowIndex];
				SendKeys.SendWait(" ");
				responseMappingsGrid.EndEdit();
				responseMappingsGrid.CurrentCell = null;
				responseMappingsGrid[1, e.RowIndex].Value = leftHandSideVariableSelectorForm.VariableName;
			}
		}
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		foreach (ResponseMapping item in responseMappingsBindingSource.List)
		{
			if ((!string.IsNullOrEmpty(item.Path) || !string.IsNullOrEmpty(item.Variable)) && (string.IsNullOrEmpty(item.Path) || string.IsNullOrEmpty(item.Variable)))
			{
				MessageBox.Show(LocalizedResourceMgr.GetString("CRMLookupConfigurationForm.MessageBox.Error.EmptyValuesNotAllowed"), LocalizedResourceMgr.GetString("CRMLookupConfigurationForm.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				responseMappingsBindingSource.Position = responseMappingsBindingSource.IndexOf(item);
				return;
			}
		}
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void CRMLookupConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		crmLookupComponent.ShowHelp();
	}

	private void CRMLookupConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		crmLookupComponent.ShowHelp();
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
		this.lblResponseMappings = new System.Windows.Forms.Label();
		this.responseMappingsGrid = new System.Windows.Forms.DataGridView();
		this.lookupInputParameterExpressionButton = new System.Windows.Forms.Button();
		this.txtLookupInputParameter = new System.Windows.Forms.TextBox();
		this.lblLookupInputParameter = new System.Windows.Forms.Label();
		this.comboEntity = new System.Windows.Forms.ComboBox();
		this.lblEntity = new System.Windows.Forms.Label();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.comboLookupBy = new System.Windows.Forms.ComboBox();
		this.lblLookupBy = new System.Windows.Forms.Label();
		this.responseMappingsBindingSource = new System.Windows.Forms.BindingSource(this.components);
		this.pathDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.variableDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.expressionBuilderColumn = new System.Windows.Forms.DataGridViewButtonColumn();
		((System.ComponentModel.ISupportInitialize)this.responseMappingsGrid).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.responseMappingsBindingSource).BeginInit();
		base.SuspendLayout();
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(495, 311);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 10;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(387, 311);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 9;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.lblResponseMappings.AutoSize = true;
		this.lblResponseMappings.Location = new System.Drawing.Point(13, 116);
		this.lblResponseMappings.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblResponseMappings.Name = "lblResponseMappings";
		this.lblResponseMappings.Size = new System.Drawing.Size(137, 17);
		this.lblResponseMappings.TabIndex = 7;
		this.lblResponseMappings.Text = "Response Mappings";
		this.responseMappingsGrid.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.responseMappingsGrid.AutoGenerateColumns = false;
		this.responseMappingsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
		this.responseMappingsGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
		this.responseMappingsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.responseMappingsGrid.Columns.AddRange(this.pathDataGridViewTextBoxColumn, this.variableDataGridViewTextBoxColumn, this.expressionBuilderColumn);
		this.responseMappingsGrid.DataSource = this.responseMappingsBindingSource;
		this.responseMappingsGrid.Location = new System.Drawing.Point(20, 137);
		this.responseMappingsGrid.Margin = new System.Windows.Forms.Padding(4);
		this.responseMappingsGrid.Name = "responseMappingsGrid";
		this.responseMappingsGrid.Size = new System.Drawing.Size(575, 166);
		this.responseMappingsGrid.TabIndex = 8;
		this.responseMappingsGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(ResponseMappingsGrid_CellClick);
		this.lookupInputParameterExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.lookupInputParameterExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.lookupInputParameterExpressionButton.Location = new System.Drawing.Point(556, 74);
		this.lookupInputParameterExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.lookupInputParameterExpressionButton.Name = "lookupInputParameterExpressionButton";
		this.lookupInputParameterExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.lookupInputParameterExpressionButton.TabIndex = 6;
		this.lookupInputParameterExpressionButton.UseVisualStyleBackColor = true;
		this.lookupInputParameterExpressionButton.Click += new System.EventHandler(LookupInputParameterExpressionButton_Click);
		this.txtLookupInputParameter.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtLookupInputParameter.Location = new System.Drawing.Point(185, 77);
		this.txtLookupInputParameter.Margin = new System.Windows.Forms.Padding(4);
		this.txtLookupInputParameter.Name = "txtLookupInputParameter";
		this.txtLookupInputParameter.Size = new System.Drawing.Size(363, 22);
		this.txtLookupInputParameter.TabIndex = 5;
		this.txtLookupInputParameter.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtLookupInputParameter.Validating += new System.ComponentModel.CancelEventHandler(TxtLookupInputParameter_Validating);
		this.lblLookupInputParameter.AutoSize = true;
		this.lblLookupInputParameter.Location = new System.Drawing.Point(13, 80);
		this.lblLookupInputParameter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblLookupInputParameter.Name = "lblLookupInputParameter";
		this.lblLookupInputParameter.Size = new System.Drawing.Size(164, 17);
		this.lblLookupInputParameter.TabIndex = 4;
		this.lblLookupInputParameter.Text = "Lookup Input  Parameter";
		this.comboEntity.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboEntity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboEntity.FormattingEnabled = true;
		this.comboEntity.Location = new System.Drawing.Point(185, 13);
		this.comboEntity.Margin = new System.Windows.Forms.Padding(4);
		this.comboEntity.Name = "comboEntity";
		this.comboEntity.Size = new System.Drawing.Size(363, 24);
		this.comboEntity.TabIndex = 1;
		this.lblEntity.AutoSize = true;
		this.lblEntity.Location = new System.Drawing.Point(13, 16);
		this.lblEntity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblEntity.Name = "lblEntity";
		this.lblEntity.Size = new System.Drawing.Size(43, 17);
		this.lblEntity.TabIndex = 0;
		this.lblEntity.Text = "Entity";
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.comboLookupBy.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboLookupBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboLookupBy.FormattingEnabled = true;
		this.comboLookupBy.Location = new System.Drawing.Point(185, 45);
		this.comboLookupBy.Margin = new System.Windows.Forms.Padding(4);
		this.comboLookupBy.Name = "comboLookupBy";
		this.comboLookupBy.Size = new System.Drawing.Size(363, 24);
		this.comboLookupBy.TabIndex = 3;
		this.lblLookupBy.AutoSize = true;
		this.lblLookupBy.Location = new System.Drawing.Point(13, 48);
		this.lblLookupBy.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblLookupBy.Name = "lblLookupBy";
		this.lblLookupBy.Size = new System.Drawing.Size(75, 17);
		this.lblLookupBy.TabIndex = 2;
		this.lblLookupBy.Text = "Lookup By";
		this.responseMappingsBindingSource.DataSource = typeof(TCX.CFD.Classes.Components.ResponseMapping);
		this.pathDataGridViewTextBoxColumn.DataPropertyName = "Path";
		this.pathDataGridViewTextBoxColumn.HeaderText = "Path";
		this.pathDataGridViewTextBoxColumn.Name = "pathDataGridViewTextBoxColumn";
		this.variableDataGridViewTextBoxColumn.DataPropertyName = "Variable";
		this.variableDataGridViewTextBoxColumn.HeaderText = "Variable";
		this.variableDataGridViewTextBoxColumn.Name = "variableDataGridViewTextBoxColumn";
		this.variableDataGridViewTextBoxColumn.ReadOnly = true;
		dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		dataGridViewCellStyle.NullValue = "...";
		this.expressionBuilderColumn.DefaultCellStyle = dataGridViewCellStyle;
		this.expressionBuilderColumn.FillWeight = 20f;
		this.expressionBuilderColumn.HeaderText = "";
		this.expressionBuilderColumn.Name = "expressionBuilderColumn";
		this.expressionBuilderColumn.Text = "";
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(616, 353);
		base.Controls.Add(this.comboLookupBy);
		base.Controls.Add(this.lblLookupBy);
		base.Controls.Add(this.comboEntity);
		base.Controls.Add(this.lblEntity);
		base.Controls.Add(this.lookupInputParameterExpressionButton);
		base.Controls.Add(this.txtLookupInputParameter);
		base.Controls.Add(this.lblLookupInputParameter);
		base.Controls.Add(this.responseMappingsGrid);
		base.Controls.Add(this.lblResponseMappings);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(1359, 875);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(634, 400);
		base.Name = "CRMLookupConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "CRM Lookup";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(CRMLookupConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(CRMLookupConfigurationForm_HelpRequested);
		((System.ComponentModel.ISupportInitialize)this.responseMappingsGrid).EndInit();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		((System.ComponentModel.ISupportInitialize)this.responseMappingsBindingSource).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
