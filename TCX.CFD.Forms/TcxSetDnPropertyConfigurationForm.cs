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

public class TcxSetDnPropertyConfigurationForm : Form
{
	private readonly TcxSetDnPropertyComponent tcxSetDnPropertyComponent;

	private readonly List<string> validVariables;

	private IContainer components;

	private Label lblExtension;

	private TextBox txtPropertyName;

	private Label lblPropertyName;

	private Button cancelButton;

	private Button okButton;

	private Button editPropertyNameButton;

	private ErrorProvider errorProvider;

	private Button editExtensionButton;

	private TextBox txtExtension;

	private Button editPropertyValueButton;

	private Label lblPropertyValue;

	private TextBox txtPropertyValue;

	public string Extension => txtExtension.Text;

	public string PropertyName => txtPropertyName.Text;

	public string PropertyValue => txtPropertyValue.Text;

	public TcxSetDnPropertyConfigurationForm(TcxSetDnPropertyComponent tcxSetDnPropertyComponent)
	{
		InitializeComponent();
		this.tcxSetDnPropertyComponent = tcxSetDnPropertyComponent;
		validVariables = ExpressionHelper.GetValidVariables(tcxSetDnPropertyComponent);
		txtExtension.Text = tcxSetDnPropertyComponent.Extension;
		txtPropertyName.Text = tcxSetDnPropertyComponent.PropertyName;
		txtPropertyValue.Text = tcxSetDnPropertyComponent.PropertyValue;
		TxtExtension_Validating(txtExtension, new CancelEventArgs());
		TxtPropertyName_Validating(txtPropertyName, new CancelEventArgs());
		TxtPropertyValue_Validating(txtPropertyValue, new CancelEventArgs());
		Text = LocalizedResourceMgr.GetString("TcxSetDnPropertyConfigurationForm.Title");
		lblExtension.Text = LocalizedResourceMgr.GetString("TcxSetDnPropertyConfigurationForm.lblExtension.Text");
		lblPropertyName.Text = LocalizedResourceMgr.GetString("TcxSetDnPropertyConfigurationForm.lblPropertyName.Text");
		lblPropertyValue.Text = LocalizedResourceMgr.GetString("TcxSetDnPropertyConfigurationForm.lblPropertyValue.Text");
		okButton.Text = LocalizedResourceMgr.GetString("TcxSetDnPropertyConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("TcxSetDnPropertyConfigurationForm.cancelButton.Text");
	}

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void TxtExtension_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtExtension.Text))
		{
			errorProvider.SetError(editExtensionButton, LocalizedResourceMgr.GetString("TcxSetDnPropertyConfigurationForm.Error.ExtensionIsMandatory"));
			return;
		}
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtExtension.Text);
		errorProvider.SetError(editExtensionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("TcxSetDnPropertyConfigurationForm.Error.ExtensionIsInvalid"));
	}

	private void TxtPropertyName_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtPropertyName.Text))
		{
			errorProvider.SetError(editPropertyNameButton, LocalizedResourceMgr.GetString("TcxSetDnPropertyConfigurationForm.Error.PropertyNameIsMandatory"));
			return;
		}
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtPropertyName.Text);
		errorProvider.SetError(editPropertyNameButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("TcxSetDnPropertyConfigurationForm.Error.PropertyNameIsInvalid"));
	}

	private void TxtPropertyValue_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtPropertyValue.Text))
		{
			errorProvider.SetError(editPropertyValueButton, LocalizedResourceMgr.GetString("TcxSetDnPropertyConfigurationForm.Error.PropertyValueIsMandatory"));
			return;
		}
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtPropertyValue.Text);
		errorProvider.SetError(editPropertyValueButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("TcxSetDnPropertyConfigurationForm.Error.PropertyValueIsInvalid"));
	}

	private void EditExtensionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(tcxSetDnPropertyComponent);
		expressionEditorForm.Expression = txtExtension.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtExtension.Text = expressionEditorForm.Expression;
			TxtExtension_Validating(txtExtension, new CancelEventArgs());
		}
	}

	private void EditPropertyNameButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(tcxSetDnPropertyComponent);
		expressionEditorForm.Expression = txtPropertyName.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtPropertyName.Text = expressionEditorForm.Expression;
			TxtPropertyName_Validating(txtPropertyName, new CancelEventArgs());
		}
	}

	private void EditPropertyValueButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(tcxSetDnPropertyComponent);
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

	private void TcxSetDnPropertyConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		tcxSetDnPropertyComponent.ShowHelp();
	}

	private void TcxSetDnPropertyConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		tcxSetDnPropertyComponent.ShowHelp();
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
		this.lblExtension = new System.Windows.Forms.Label();
		this.txtPropertyName = new System.Windows.Forms.TextBox();
		this.lblPropertyName = new System.Windows.Forms.Label();
		this.cancelButton = new System.Windows.Forms.Button();
		this.okButton = new System.Windows.Forms.Button();
		this.editPropertyNameButton = new System.Windows.Forms.Button();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.editExtensionButton = new System.Windows.Forms.Button();
		this.txtExtension = new System.Windows.Forms.TextBox();
		this.editPropertyValueButton = new System.Windows.Forms.Button();
		this.lblPropertyValue = new System.Windows.Forms.Label();
		this.txtPropertyValue = new System.Windows.Forms.TextBox();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.lblExtension.AutoSize = true;
		this.lblExtension.Location = new System.Drawing.Point(16, 11);
		this.lblExtension.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblExtension.Name = "lblExtension";
		this.lblExtension.Size = new System.Drawing.Size(69, 17);
		this.lblExtension.TabIndex = 0;
		this.lblExtension.Text = "Extension";
		this.txtPropertyName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtPropertyName.Location = new System.Drawing.Point(127, 43);
		this.txtPropertyName.Margin = new System.Windows.Forms.Padding(4);
		this.txtPropertyName.MaxLength = 8192;
		this.txtPropertyName.Name = "txtPropertyName";
		this.txtPropertyName.Size = new System.Drawing.Size(423, 22);
		this.txtPropertyName.TabIndex = 4;
		this.txtPropertyName.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtPropertyName.Validating += new System.ComponentModel.CancelEventHandler(TxtPropertyName_Validating);
		this.lblPropertyName.AutoSize = true;
		this.lblPropertyName.Location = new System.Drawing.Point(16, 47);
		this.lblPropertyName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblPropertyName.Name = "lblPropertyName";
		this.lblPropertyName.Size = new System.Drawing.Size(103, 17);
		this.lblPropertyName.TabIndex = 3;
		this.lblPropertyName.Text = "Property Name";
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(451, 114);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 10;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(343, 114);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 9;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.editPropertyNameButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.editPropertyNameButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.editPropertyNameButton.Location = new System.Drawing.Point(559, 41);
		this.editPropertyNameButton.Margin = new System.Windows.Forms.Padding(4);
		this.editPropertyNameButton.Name = "editPropertyNameButton";
		this.editPropertyNameButton.Size = new System.Drawing.Size(39, 28);
		this.editPropertyNameButton.TabIndex = 5;
		this.editPropertyNameButton.UseVisualStyleBackColor = true;
		this.editPropertyNameButton.Click += new System.EventHandler(EditPropertyNameButton_Click);
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.editExtensionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.editExtensionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.editExtensionButton.Location = new System.Drawing.Point(559, 5);
		this.editExtensionButton.Margin = new System.Windows.Forms.Padding(4);
		this.editExtensionButton.Name = "editExtensionButton";
		this.editExtensionButton.Size = new System.Drawing.Size(39, 28);
		this.editExtensionButton.TabIndex = 2;
		this.editExtensionButton.UseVisualStyleBackColor = true;
		this.editExtensionButton.Click += new System.EventHandler(EditExtensionButton_Click);
		this.txtExtension.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtExtension.Location = new System.Drawing.Point(127, 7);
		this.txtExtension.Margin = new System.Windows.Forms.Padding(4);
		this.txtExtension.MaxLength = 8192;
		this.txtExtension.Name = "txtExtension";
		this.txtExtension.Size = new System.Drawing.Size(423, 22);
		this.txtExtension.TabIndex = 1;
		this.txtExtension.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtExtension.Validating += new System.ComponentModel.CancelEventHandler(TxtExtension_Validating);
		this.editPropertyValueButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.editPropertyValueButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.editPropertyValueButton.Location = new System.Drawing.Point(559, 73);
		this.editPropertyValueButton.Margin = new System.Windows.Forms.Padding(4);
		this.editPropertyValueButton.Name = "editPropertyValueButton";
		this.editPropertyValueButton.Size = new System.Drawing.Size(39, 28);
		this.editPropertyValueButton.TabIndex = 8;
		this.editPropertyValueButton.UseVisualStyleBackColor = true;
		this.editPropertyValueButton.Click += new System.EventHandler(EditPropertyValueButton_Click);
		this.lblPropertyValue.AutoSize = true;
		this.lblPropertyValue.Location = new System.Drawing.Point(16, 79);
		this.lblPropertyValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblPropertyValue.Name = "lblPropertyValue";
		this.lblPropertyValue.Size = new System.Drawing.Size(102, 17);
		this.lblPropertyValue.TabIndex = 6;
		this.lblPropertyValue.Text = "Property Value";
		this.txtPropertyValue.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtPropertyValue.Location = new System.Drawing.Point(127, 75);
		this.txtPropertyValue.Margin = new System.Windows.Forms.Padding(4);
		this.txtPropertyValue.MaxLength = 8192;
		this.txtPropertyValue.Name = "txtPropertyValue";
		this.txtPropertyValue.Size = new System.Drawing.Size(423, 22);
		this.txtPropertyValue.TabIndex = 7;
		this.txtPropertyValue.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtPropertyValue.Validating += new System.ComponentModel.CancelEventHandler(TxtPropertyValue_Validating);
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(619, 148);
		base.Controls.Add(this.editPropertyValueButton);
		base.Controls.Add(this.lblPropertyValue);
		base.Controls.Add(this.txtPropertyValue);
		base.Controls.Add(this.editExtensionButton);
		base.Controls.Add(this.txtExtension);
		base.Controls.Add(this.editPropertyNameButton);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.Controls.Add(this.lblPropertyName);
		base.Controls.Add(this.txtPropertyName);
		base.Controls.Add(this.lblExtension);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(1359, 195);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(634, 195);
		base.Name = "TcxSetDnPropertyConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Set DN Property";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(TcxSetDnPropertyConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(TcxSetDnPropertyConfigurationForm_HelpRequested);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
