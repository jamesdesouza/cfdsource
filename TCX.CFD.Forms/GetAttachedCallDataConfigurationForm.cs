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

public class GetAttachedCallDataConfigurationForm : Form
{
	private readonly GetAttachedCallDataComponent getAttachedCallDataComponent;

	private readonly List<string> validVariables;

	private IContainer components;

	private TextBox txtDataName;

	private Label lblDataName;

	private Button cancelButton;

	private Button okButton;

	private Button dataNameExpressionButton;

	private ErrorProvider errorProvider;

	public string DataName => txtDataName.Text;

	public GetAttachedCallDataConfigurationForm(GetAttachedCallDataComponent getAttachedCallDataComponent)
	{
		InitializeComponent();
		this.getAttachedCallDataComponent = getAttachedCallDataComponent;
		validVariables = ExpressionHelper.GetValidVariables(getAttachedCallDataComponent);
		txtDataName.Text = getAttachedCallDataComponent.DataName;
		TxtDataName_Validating(txtDataName, new CancelEventArgs());
		Text = LocalizedResourceMgr.GetString("GetAttachedCallDataConfigurationForm.Title");
		lblDataName.Text = LocalizedResourceMgr.GetString("GetAttachedCallDataConfigurationForm.lblDataName.Text");
		okButton.Text = LocalizedResourceMgr.GetString("GetAttachedCallDataConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("GetAttachedCallDataConfigurationForm.cancelButton.Text");
	}

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void TxtDataName_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtDataName.Text))
		{
			errorProvider.SetError(dataNameExpressionButton, LocalizedResourceMgr.GetString("GetAttachedCallDataConfigurationForm.Error.DataNameIsMandatory"));
		}
		else if (!AbsArgument.BuildArgument(validVariables, txtDataName.Text).IsSafeExpression())
		{
			errorProvider.SetError(dataNameExpressionButton, LocalizedResourceMgr.GetString("GetAttachedCallDataConfigurationForm.Error.DataNameIsInvalid"));
		}
		else
		{
			errorProvider.SetError(dataNameExpressionButton, string.Empty);
		}
	}

	private void EditDataNameButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(getAttachedCallDataComponent)
		{
			Expression = txtDataName.Text
		};
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtDataName.Text = expressionEditorForm.Expression;
			TxtDataName_Validating(txtDataName, new CancelEventArgs());
		}
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void GetAttachedCallDataConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		getAttachedCallDataComponent.ShowHelp();
	}

	private void GetAttachedCallDataConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		getAttachedCallDataComponent.ShowHelp();
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
		this.cancelButton.Location = new System.Drawing.Point(451, 54);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 4;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(343, 54);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 3;
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
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(619, 87);
		base.Controls.Add(this.dataNameExpressionButton);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.Controls.Add(this.lblDataName);
		base.Controls.Add(this.txtDataName);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(1359, 134);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(634, 134);
		base.Name = "GetAttachedCallDataConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Get Attached Call Data";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(GetAttachedCallDataConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(GetAttachedCallDataConfigurationForm_HelpRequested);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
