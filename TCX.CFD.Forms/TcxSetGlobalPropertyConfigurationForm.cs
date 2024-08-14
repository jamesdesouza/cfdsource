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

public class TcxSetGlobalPropertyConfigurationForm : Form
{
	private readonly TcxSetGlobalPropertyComponent tcxSetGlobalPropertyComponent;

	private readonly List<string> validVariables;

	private IContainer components;

	private TextBox txtPropertyName;

	private Label lblPropertyName;

	private Button cancelButton;

	private Button okButton;

	private Button editPropertyNameButton;

	private ErrorProvider errorProvider;

	private Button editPropertyValueButton;

	private Label lblPropertyValue;

	private TextBox txtPropertyValue;

	public string PropertyName => txtPropertyName.Text;

	public string PropertyValue => txtPropertyValue.Text;

	public TcxSetGlobalPropertyConfigurationForm(TcxSetGlobalPropertyComponent tcxSetGlobalPropertyComponent)
	{
		InitializeComponent();
		this.tcxSetGlobalPropertyComponent = tcxSetGlobalPropertyComponent;
		validVariables = ExpressionHelper.GetValidVariables(tcxSetGlobalPropertyComponent);
		txtPropertyName.Text = tcxSetGlobalPropertyComponent.PropertyName;
		txtPropertyValue.Text = tcxSetGlobalPropertyComponent.PropertyValue;
		TxtPropertyName_Validating(txtPropertyName, new CancelEventArgs());
		TxtPropertyValue_Validating(txtPropertyValue, new CancelEventArgs());
		Text = LocalizedResourceMgr.GetString("TcxSetGlobalPropertyConfigurationForm.Title");
		lblPropertyName.Text = LocalizedResourceMgr.GetString("TcxSetGlobalPropertyConfigurationForm.lblPropertyName.Text");
		lblPropertyValue.Text = LocalizedResourceMgr.GetString("TcxSetGlobalPropertyConfigurationForm.lblPropertyValue.Text");
		okButton.Text = LocalizedResourceMgr.GetString("TcxSetGlobalPropertyConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("TcxSetGlobalPropertyConfigurationForm.cancelButton.Text");
	}

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void TxtPropertyName_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtPropertyName.Text))
		{
			errorProvider.SetError(editPropertyNameButton, LocalizedResourceMgr.GetString("TcxSetGlobalPropertyConfigurationForm.Error.PropertyNameIsMandatory"));
			return;
		}
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtPropertyName.Text);
		errorProvider.SetError(editPropertyNameButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("TcxSetGlobalPropertyConfigurationForm.Error.PropertyNameIsInvalid"));
	}

	private void TxtPropertyValue_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtPropertyValue.Text))
		{
			errorProvider.SetError(editPropertyValueButton, LocalizedResourceMgr.GetString("TcxSetGlobalPropertyConfigurationForm.Error.PropertyValueIsMandatory"));
			return;
		}
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtPropertyValue.Text);
		errorProvider.SetError(editPropertyValueButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("TcxSetGlobalPropertyConfigurationForm.Error.PropertyValueIsInvalid"));
	}

	private void EditPropertyNameButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(tcxSetGlobalPropertyComponent);
		expressionEditorForm.Expression = txtPropertyName.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtPropertyName.Text = expressionEditorForm.Expression;
			TxtPropertyName_Validating(txtPropertyName, new CancelEventArgs());
		}
	}

	private void EditPropertyValueButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(tcxSetGlobalPropertyComponent);
		expressionEditorForm.Expression = txtPropertyValue.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtPropertyValue.Text = expressionEditorForm.Expression;
			TxtPropertyValue_Validating(txtPropertyValue, new CancelEventArgs());
		}
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void TcxSetGlobalPropertyConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		tcxSetGlobalPropertyComponent.ShowHelp();
	}

	private void TcxSetGlobalPropertyConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		tcxSetGlobalPropertyComponent.ShowHelp();
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
		this.txtPropertyName = new System.Windows.Forms.TextBox();
		this.lblPropertyName = new System.Windows.Forms.Label();
		this.cancelButton = new System.Windows.Forms.Button();
		this.okButton = new System.Windows.Forms.Button();
		this.editPropertyNameButton = new System.Windows.Forms.Button();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.editPropertyValueButton = new System.Windows.Forms.Button();
		this.lblPropertyValue = new System.Windows.Forms.Label();
		this.txtPropertyValue = new System.Windows.Forms.TextBox();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.txtPropertyName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtPropertyName.Location = new System.Drawing.Point(127, 15);
		this.txtPropertyName.Margin = new System.Windows.Forms.Padding(4);
		this.txtPropertyName.MaxLength = 8192;
		this.txtPropertyName.Name = "txtPropertyName";
		this.txtPropertyName.Size = new System.Drawing.Size(423, 22);
		this.txtPropertyName.TabIndex = 1;
		this.txtPropertyName.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtPropertyName.Validating += new System.ComponentModel.CancelEventHandler(TxtPropertyName_Validating);
		this.lblPropertyName.AutoSize = true;
		this.lblPropertyName.Location = new System.Drawing.Point(16, 18);
		this.lblPropertyName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblPropertyName.Name = "lblPropertyName";
		this.lblPropertyName.Size = new System.Drawing.Size(103, 17);
		this.lblPropertyName.TabIndex = 0;
		this.lblPropertyName.Text = "Property Name";
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
		this.editPropertyNameButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.editPropertyNameButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.editPropertyNameButton.Location = new System.Drawing.Point(559, 12);
		this.editPropertyNameButton.Margin = new System.Windows.Forms.Padding(4);
		this.editPropertyNameButton.Name = "editPropertyNameButton";
		this.editPropertyNameButton.Size = new System.Drawing.Size(39, 28);
		this.editPropertyNameButton.TabIndex = 2;
		this.editPropertyNameButton.UseVisualStyleBackColor = true;
		this.editPropertyNameButton.Click += new System.EventHandler(EditPropertyNameButton_Click);
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.editPropertyValueButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.editPropertyValueButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.editPropertyValueButton.Location = new System.Drawing.Point(559, 44);
		this.editPropertyValueButton.Margin = new System.Windows.Forms.Padding(4);
		this.editPropertyValueButton.Name = "editPropertyValueButton";
		this.editPropertyValueButton.Size = new System.Drawing.Size(39, 28);
		this.editPropertyValueButton.TabIndex = 5;
		this.editPropertyValueButton.UseVisualStyleBackColor = true;
		this.editPropertyValueButton.Click += new System.EventHandler(EditPropertyValueButton_Click);
		this.lblPropertyValue.AutoSize = true;
		this.lblPropertyValue.Location = new System.Drawing.Point(16, 50);
		this.lblPropertyValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblPropertyValue.Name = "lblPropertyValue";
		this.lblPropertyValue.Size = new System.Drawing.Size(102, 17);
		this.lblPropertyValue.TabIndex = 3;
		this.lblPropertyValue.Text = "Property Value";
		this.txtPropertyValue.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtPropertyValue.Location = new System.Drawing.Point(127, 47);
		this.txtPropertyValue.Margin = new System.Windows.Forms.Padding(4);
		this.txtPropertyValue.MaxLength = 8192;
		this.txtPropertyValue.Name = "txtPropertyValue";
		this.txtPropertyValue.Size = new System.Drawing.Size(423, 22);
		this.txtPropertyValue.TabIndex = 4;
		this.txtPropertyValue.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtPropertyValue.Validating += new System.ComponentModel.CancelEventHandler(TxtPropertyValue_Validating);
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(619, 114);
		base.Controls.Add(this.editPropertyValueButton);
		base.Controls.Add(this.lblPropertyValue);
		base.Controls.Add(this.txtPropertyValue);
		base.Controls.Add(this.editPropertyNameButton);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.Controls.Add(this.lblPropertyName);
		base.Controls.Add(this.txtPropertyName);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(1359, 161);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(634, 161);
		base.Name = "TcxSetGlobalPropertyConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Set Global Property";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(TcxSetGlobalPropertyConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(TcxSetGlobalPropertyConfigurationForm_HelpRequested);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
