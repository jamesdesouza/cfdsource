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

public class TcxGetGlobalPropertyConfigurationForm : Form
{
	private readonly TcxGetGlobalPropertyComponent tcxGetGlobalPropertyComponent;

	private readonly List<string> validVariables;

	private IContainer components;

	private TextBox txtPropertyName;

	private Label lblPropertyName;

	private Button cancelButton;

	private Button okButton;

	private Button editPropertyNameButton;

	private ErrorProvider errorProvider;

	public string PropertyName => txtPropertyName.Text;

	public TcxGetGlobalPropertyConfigurationForm(TcxGetGlobalPropertyComponent tcxGetGlobalPropertyComponent)
	{
		InitializeComponent();
		this.tcxGetGlobalPropertyComponent = tcxGetGlobalPropertyComponent;
		validVariables = ExpressionHelper.GetValidVariables(tcxGetGlobalPropertyComponent);
		txtPropertyName.Text = tcxGetGlobalPropertyComponent.PropertyName;
		TxtPropertyName_Validating(txtPropertyName, new CancelEventArgs());
		Text = LocalizedResourceMgr.GetString("TcxGetGlobalPropertyConfigurationForm.Title");
		lblPropertyName.Text = LocalizedResourceMgr.GetString("TcxGetGlobalPropertyConfigurationForm.lblPropertyName.Text");
		okButton.Text = LocalizedResourceMgr.GetString("TcxGetGlobalPropertyConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("TcxGetGlobalPropertyConfigurationForm.cancelButton.Text");
	}

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void TxtPropertyName_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtPropertyName.Text))
		{
			errorProvider.SetError(editPropertyNameButton, LocalizedResourceMgr.GetString("TcxGetGlobalPropertyConfigurationForm.Error.PropertyNameIsMandatory"));
			return;
		}
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtPropertyName.Text);
		errorProvider.SetError(editPropertyNameButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("TcxGetGlobalPropertyConfigurationForm.Error.PropertyNameIsInvalid"));
	}

	private void EditPropertyNameButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(tcxGetGlobalPropertyComponent);
		expressionEditorForm.Expression = txtPropertyName.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtPropertyName.Text = expressionEditorForm.Expression;
			TxtPropertyName_Validating(txtPropertyName, new CancelEventArgs());
		}
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void TcxGetGlobalPropertyConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		tcxGetGlobalPropertyComponent.ShowHelp();
	}

	private void TcxGetGlobalPropertyConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		tcxGetGlobalPropertyComponent.ShowHelp();
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
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(619, 87);
		base.Controls.Add(this.editPropertyNameButton);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.Controls.Add(this.lblPropertyName);
		base.Controls.Add(this.txtPropertyName);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(1359, 134);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(634, 134);
		base.Name = "TcxGetGlobalPropertyConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Get Global Property";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(TcxGetGlobalPropertyConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(TcxGetGlobalPropertyConfigurationForm_HelpRequested);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
