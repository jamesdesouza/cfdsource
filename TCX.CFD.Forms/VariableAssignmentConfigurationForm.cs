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

public class VariableAssignmentConfigurationForm : Form
{
	private readonly VariableAssignmentComponent variableAssignmentComponent;

	private readonly List<string> validVariables;

	private IContainer components;

	private TextBox txtVariableName;

	private Label lblVariableName;

	private TextBox txtExpression;

	private Label lblExpression;

	private Button cancelButton;

	private Button okButton;

	private Button editVariableExpressionButton;

	private Button selectVariableNameButton;

	private ErrorProvider errorProvider;

	public string VariableName => txtVariableName.Text;

	public string Expression => txtExpression.Text;

	public VariableAssignmentConfigurationForm(VariableAssignmentComponent variableAssignmentComponent)
	{
		InitializeComponent();
		this.variableAssignmentComponent = variableAssignmentComponent;
		validVariables = ExpressionHelper.GetValidVariables(variableAssignmentComponent);
		txtVariableName.Text = variableAssignmentComponent.VariableName;
		txtExpression.Text = variableAssignmentComponent.Expression;
		TxtVariableName_Validating(txtVariableName, new CancelEventArgs());
		TxtExpression_Validating(txtExpression, new CancelEventArgs());
		Text = LocalizedResourceMgr.GetString("VariableAssignmentConfigurationForm.Title");
		lblVariableName.Text = LocalizedResourceMgr.GetString("VariableAssignmentConfigurationForm.lblVariableName.Text");
		lblExpression.Text = LocalizedResourceMgr.GetString("VariableAssignmentConfigurationForm.lblExpression.Text");
		okButton.Text = LocalizedResourceMgr.GetString("VariableAssignmentConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("VariableAssignmentConfigurationForm.cancelButton.Text");
	}

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void TxtVariableName_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtVariableName.Text))
		{
			errorProvider.SetError(selectVariableNameButton, LocalizedResourceMgr.GetString("VariableAssignmentConfigurationForm.Error.VariableNameIsMandatory"));
			return;
		}
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtVariableName.Text);
		errorProvider.SetError(selectVariableNameButton, absArgument.IsVariableName() ? string.Empty : LocalizedResourceMgr.GetString("VariableAssignmentConfigurationForm.Error.UnknownVariableName"));
	}

	private void TxtExpression_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtExpression.Text))
		{
			errorProvider.SetError(editVariableExpressionButton, LocalizedResourceMgr.GetString("VariableAssignmentConfigurationForm.Error.ExpressionIsMandatory"));
			return;
		}
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtExpression.Text);
		errorProvider.SetError(editVariableExpressionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("VariableAssignmentConfigurationForm.Error.ExpressionIsInvalid"));
	}

	private void SelectVariableNameButton_Click(object sender, EventArgs e)
	{
		LeftHandSideVariableSelectorForm leftHandSideVariableSelectorForm = new LeftHandSideVariableSelectorForm();
		leftHandSideVariableSelectorForm.Component = variableAssignmentComponent;
		leftHandSideVariableSelectorForm.VariableName = txtVariableName.Text;
		if (leftHandSideVariableSelectorForm.ShowDialog() == DialogResult.OK)
		{
			txtVariableName.Text = leftHandSideVariableSelectorForm.VariableName;
			TxtVariableName_Validating(txtVariableName, new CancelEventArgs());
		}
	}

	private void EditVariableExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(variableAssignmentComponent);
		expressionEditorForm.Expression = txtExpression.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtExpression.Text = expressionEditorForm.Expression;
			TxtExpression_Validating(txtExpression, new CancelEventArgs());
		}
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void VariableAssignmentConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		variableAssignmentComponent.ShowHelp();
	}

	private void VariableAssignmentConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		variableAssignmentComponent.ShowHelp();
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
		this.txtVariableName = new System.Windows.Forms.TextBox();
		this.lblVariableName = new System.Windows.Forms.Label();
		this.txtExpression = new System.Windows.Forms.TextBox();
		this.lblExpression = new System.Windows.Forms.Label();
		this.cancelButton = new System.Windows.Forms.Button();
		this.okButton = new System.Windows.Forms.Button();
		this.editVariableExpressionButton = new System.Windows.Forms.Button();
		this.selectVariableNameButton = new System.Windows.Forms.Button();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.txtVariableName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtVariableName.Location = new System.Drawing.Point(125, 11);
		this.txtVariableName.Margin = new System.Windows.Forms.Padding(4);
		this.txtVariableName.MaxLength = 8192;
		this.txtVariableName.Name = "txtVariableName";
		this.txtVariableName.Size = new System.Drawing.Size(424, 22);
		this.txtVariableName.TabIndex = 1;
		this.txtVariableName.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtVariableName.Validating += new System.ComponentModel.CancelEventHandler(TxtVariableName_Validating);
		this.lblVariableName.AutoSize = true;
		this.lblVariableName.Location = new System.Drawing.Point(16, 11);
		this.lblVariableName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblVariableName.Name = "lblVariableName";
		this.lblVariableName.Size = new System.Drawing.Size(101, 17);
		this.lblVariableName.TabIndex = 0;
		this.lblVariableName.Text = "Variable Name";
		this.txtExpression.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtExpression.Location = new System.Drawing.Point(125, 43);
		this.txtExpression.Margin = new System.Windows.Forms.Padding(4);
		this.txtExpression.MaxLength = 8192;
		this.txtExpression.Name = "txtExpression";
		this.txtExpression.Size = new System.Drawing.Size(424, 22);
		this.txtExpression.TabIndex = 4;
		this.txtExpression.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtExpression.Validating += new System.ComponentModel.CancelEventHandler(TxtExpression_Validating);
		this.lblExpression.AutoSize = true;
		this.lblExpression.Location = new System.Drawing.Point(16, 47);
		this.lblExpression.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblExpression.Name = "lblExpression";
		this.lblExpression.Size = new System.Drawing.Size(77, 17);
		this.lblExpression.TabIndex = 3;
		this.lblExpression.Text = "Expression";
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(451, 81);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 7;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(343, 81);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 6;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.editVariableExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.editVariableExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.editVariableExpressionButton.Location = new System.Drawing.Point(559, 41);
		this.editVariableExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.editVariableExpressionButton.Name = "editVariableExpressionButton";
		this.editVariableExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.editVariableExpressionButton.TabIndex = 5;
		this.editVariableExpressionButton.UseVisualStyleBackColor = true;
		this.editVariableExpressionButton.Click += new System.EventHandler(EditVariableExpressionButton_Click);
		this.selectVariableNameButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.selectVariableNameButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.selectVariableNameButton.Location = new System.Drawing.Point(559, 9);
		this.selectVariableNameButton.Margin = new System.Windows.Forms.Padding(4);
		this.selectVariableNameButton.Name = "selectVariableNameButton";
		this.selectVariableNameButton.Size = new System.Drawing.Size(39, 28);
		this.selectVariableNameButton.TabIndex = 2;
		this.selectVariableNameButton.UseVisualStyleBackColor = true;
		this.selectVariableNameButton.Click += new System.EventHandler(SelectVariableNameButton_Click);
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(619, 114);
		base.Controls.Add(this.selectVariableNameButton);
		base.Controls.Add(this.editVariableExpressionButton);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.Controls.Add(this.lblExpression);
		base.Controls.Add(this.txtExpression);
		base.Controls.Add(this.lblVariableName);
		base.Controls.Add(this.txtVariableName);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(1359, 161);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(634, 161);
		base.Name = "VariableAssignmentConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Assign a Variable";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(VariableAssignmentConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(VariableAssignmentConfigurationForm_HelpRequested);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
