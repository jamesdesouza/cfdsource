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

public class CsvParserConfigurationForm : Form
{
	private readonly CsvParserComponent csvParserComponent;

	private readonly List<string> validVariables;

	private IContainer components;

	private Button cancelButton;

	private Button okButton;

	private Label lblText;

	private TextBox txtText;

	private Button textExpressionButton;

	private ErrorProvider errorProvider;

	public string TextToParse => txtText.Text;

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void TxtText_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtText.Text))
		{
			errorProvider.SetError(textExpressionButton, LocalizedResourceMgr.GetString("CsvParserConfigurationForm.Error.TextIsMandatory"));
			return;
		}
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtText.Text);
		errorProvider.SetError(textExpressionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("CsvParserConfigurationForm.Error.TextIsInvalid"));
	}

	private void TextExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(csvParserComponent);
		expressionEditorForm.Expression = txtText.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtText.Text = expressionEditorForm.Expression;
			TxtText_Validating(txtText, new CancelEventArgs());
		}
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.OK;
		Close();
	}

	public CsvParserConfigurationForm(CsvParserComponent csvParserComponent)
	{
		InitializeComponent();
		this.csvParserComponent = csvParserComponent;
		validVariables = ExpressionHelper.GetValidVariables(csvParserComponent);
		txtText.Text = csvParserComponent.Text;
		TxtText_Validating(txtText, new CancelEventArgs());
		Text = LocalizedResourceMgr.GetString("CsvParserConfigurationForm.Title");
		lblText.Text = LocalizedResourceMgr.GetString("CsvParserConfigurationForm.lblText.Text");
		okButton.Text = LocalizedResourceMgr.GetString("CsvParserConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("CsvParserConfigurationForm.cancelButton.Text");
	}

	private void CsvParserConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		csvParserComponent.ShowHelp();
	}

	private void CsvParserConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		csvParserComponent.ShowHelp();
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
		this.textExpressionButton = new System.Windows.Forms.Button();
		this.lblText = new System.Windows.Forms.Label();
		this.txtText = new System.Windows.Forms.TextBox();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(497, 58);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 4;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(389, 58);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 3;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.textExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.textExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.textExpressionButton.Location = new System.Drawing.Point(558, 11);
		this.textExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.textExpressionButton.Name = "textExpressionButton";
		this.textExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.textExpressionButton.TabIndex = 2;
		this.textExpressionButton.UseVisualStyleBackColor = true;
		this.textExpressionButton.Click += new System.EventHandler(TextExpressionButton_Click);
		this.lblText.AutoSize = true;
		this.lblText.Location = new System.Drawing.Point(15, 17);
		this.lblText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblText.Name = "lblText";
		this.lblText.Size = new System.Drawing.Size(35, 17);
		this.lblText.TabIndex = 0;
		this.lblText.Text = "Text";
		this.txtText.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtText.Location = new System.Drawing.Point(78, 13);
		this.txtText.Margin = new System.Windows.Forms.Padding(4);
		this.txtText.MaxLength = 8192;
		this.txtText.Name = "txtText";
		this.txtText.Size = new System.Drawing.Size(470, 22);
		this.txtText.TabIndex = 1;
		this.txtText.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtText.Validating += new System.ComponentModel.CancelEventHandler(TxtText_Validating);
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(619, 91);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.Controls.Add(this.lblText);
		base.Controls.Add(this.textExpressionButton);
		base.Controls.Add(this.txtText);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(1359, 138);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(634, 138);
		base.Name = "CsvParserConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "CSV Parser";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(CsvParserConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(CsvParserConfigurationForm_HelpRequested);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
