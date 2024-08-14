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

public class AttachCallDataConfigurationForm : Form
{
	private readonly AttachCallDataComponent attachCallDataComponent;

	private readonly List<string> validVariables;

	private IContainer components;

	private TextBox txtDataName;

	private Label lblDataName;

	private Button cancelButton;

	private Button okButton;

	private Button dataNameExpressionButton;

	private ErrorProvider errorProvider;

	private Button dataValueExpressionButton;

	private Label lblDataValue;

	private TextBox txtDataValue;

	public string DataName => txtDataName.Text;

	public string DataValue => txtDataValue.Text;

	public AttachCallDataConfigurationForm(AttachCallDataComponent attachCallDataComponent)
	{
		InitializeComponent();
		this.attachCallDataComponent = attachCallDataComponent;
		validVariables = ExpressionHelper.GetValidVariables(attachCallDataComponent);
		txtDataName.Text = attachCallDataComponent.DataName;
		txtDataValue.Text = attachCallDataComponent.DataValue;
		TxtDataName_Validating(txtDataName, new CancelEventArgs());
		TxtDataValue_Validating(txtDataValue, new CancelEventArgs());
		Text = LocalizedResourceMgr.GetString("AttachCallDataConfigurationForm.Title");
		lblDataName.Text = LocalizedResourceMgr.GetString("AttachCallDataConfigurationForm.lblDataName.Text");
		lblDataValue.Text = LocalizedResourceMgr.GetString("AttachCallDataConfigurationForm.lblDataValue.Text");
		okButton.Text = LocalizedResourceMgr.GetString("AttachCallDataConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("AttachCallDataConfigurationForm.cancelButton.Text");
	}

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void TxtDataName_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtDataName.Text))
		{
			errorProvider.SetError(dataNameExpressionButton, LocalizedResourceMgr.GetString("AttachCallDataConfigurationForm.Error.DataNameIsMandatory"));
		}
		else if (!AbsArgument.BuildArgument(validVariables, txtDataName.Text).IsSafeExpression())
		{
			errorProvider.SetError(dataNameExpressionButton, LocalizedResourceMgr.GetString("AttachCallDataConfigurationForm.Error.DataNameIsInvalid"));
		}
		else
		{
			errorProvider.SetError(dataNameExpressionButton, string.Empty);
		}
	}

	private void TxtDataValue_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtDataValue.Text))
		{
			errorProvider.SetError(dataValueExpressionButton, LocalizedResourceMgr.GetString("AttachCallDataConfigurationForm.Error.DataValueIsMandatory"));
		}
		else if (!AbsArgument.BuildArgument(validVariables, txtDataValue.Text).IsSafeExpression())
		{
			errorProvider.SetError(dataValueExpressionButton, LocalizedResourceMgr.GetString("AttachCallDataConfigurationForm.Error.DataValueIsInvalid"));
		}
		else
		{
			errorProvider.SetError(dataValueExpressionButton, string.Empty);
		}
	}

	private void EditDataNameButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(attachCallDataComponent)
		{
			Expression = txtDataName.Text
		};
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtDataName.Text = expressionEditorForm.Expression;
			TxtDataName_Validating(txtDataName, new CancelEventArgs());
		}
	}

	private void EditDataValueButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(attachCallDataComponent)
		{
			Expression = txtDataValue.Text
		};
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtDataValue.Text = expressionEditorForm.Expression;
			TxtDataValue_Validating(txtDataValue, new CancelEventArgs());
		}
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void AttachCallDataConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		attachCallDataComponent.ShowHelp();
	}

	private void AttachCallDataConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		attachCallDataComponent.ShowHelp();
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
		this.txtDataName = new System.Windows.Forms.TextBox();
		this.lblDataName = new System.Windows.Forms.Label();
		this.cancelButton = new System.Windows.Forms.Button();
		this.okButton = new System.Windows.Forms.Button();
		this.dataNameExpressionButton = new System.Windows.Forms.Button();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.dataValueExpressionButton = new System.Windows.Forms.Button();
		this.lblDataValue = new System.Windows.Forms.Label();
		this.txtDataValue = new System.Windows.Forms.TextBox();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.txtDataName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtDataName.Location = new System.Drawing.Point(69, 15);
		this.txtDataName.Margin = new System.Windows.Forms.Padding(4);
		this.txtDataName.MaxLength = 8192;
		this.txtDataName.Name = "txtDataName";
		this.txtDataName.Size = new System.Drawing.Size(481, 22);
		this.txtDataName.TabIndex = 1;
		this.txtDataName.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtDataName.Validating += new System.ComponentModel.CancelEventHandler(TxtDataName_Validating);
		this.lblDataName.AutoSize = true;
		this.lblDataName.Location = new System.Drawing.Point(16, 18);
		this.lblDataName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblDataName.Name = "lblDataName";
		this.lblDataName.Size = new System.Drawing.Size(45, 17);
		this.lblDataName.TabIndex = 0;
		this.lblDataName.Text = "Name";
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
		this.dataNameExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.dataNameExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.dataNameExpressionButton.Location = new System.Drawing.Point(559, 12);
		this.dataNameExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.dataNameExpressionButton.Name = "editDataNameButton";
		this.dataNameExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.dataNameExpressionButton.TabIndex = 2;
		this.dataNameExpressionButton.UseVisualStyleBackColor = true;
		this.dataNameExpressionButton.Click += new System.EventHandler(EditDataNameButton_Click);
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.dataValueExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.dataValueExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.dataValueExpressionButton.Location = new System.Drawing.Point(559, 44);
		this.dataValueExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.dataValueExpressionButton.Name = "editDataValueButton";
		this.dataValueExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.dataValueExpressionButton.TabIndex = 5;
		this.dataValueExpressionButton.UseVisualStyleBackColor = true;
		this.dataValueExpressionButton.Click += new System.EventHandler(EditDataValueButton_Click);
		this.lblDataValue.AutoSize = true;
		this.lblDataValue.Location = new System.Drawing.Point(16, 50);
		this.lblDataValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblDataValue.Name = "lblDataValue";
		this.lblDataValue.Size = new System.Drawing.Size(44, 17);
		this.lblDataValue.TabIndex = 3;
		this.lblDataValue.Text = "Value";
		this.txtDataValue.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtDataValue.Location = new System.Drawing.Point(68, 47);
		this.txtDataValue.Margin = new System.Windows.Forms.Padding(4);
		this.txtDataValue.MaxLength = 8192;
		this.txtDataValue.Name = "txtDataValue";
		this.txtDataValue.Size = new System.Drawing.Size(482, 22);
		this.txtDataValue.TabIndex = 4;
		this.txtDataValue.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtDataValue.Validating += new System.ComponentModel.CancelEventHandler(TxtDataValue_Validating);
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(619, 114);
		base.Controls.Add(this.dataValueExpressionButton);
		base.Controls.Add(this.lblDataValue);
		base.Controls.Add(this.txtDataValue);
		base.Controls.Add(this.dataNameExpressionButton);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.Controls.Add(this.lblDataName);
		base.Controls.Add(this.txtDataName);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(1359, 161);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(634, 161);
		base.Name = "AttachCallDataConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Attach Call Data";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(AttachCallDataConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(AttachCallDataConfigurationForm_HelpRequested);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
