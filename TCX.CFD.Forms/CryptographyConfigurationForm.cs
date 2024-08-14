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

public class CryptographyConfigurationForm : Form
{
	private readonly CryptographyComponent cryptographyComponent;

	private readonly List<string> validVariables;

	private IContainer components;

	private Label lblAlgorithm;

	private ComboBox comboAlgorithm;

	private Label lblFormat;

	private ComboBox comboFormat;

	private TextBox txtKey;

	private Label lblKey;

	private TextBox txtText;

	private Label lblText;

	private Button cancelButton;

	private Button okButton;

	private Button textExpressionButton;

	private ComboBox comboAction;

	private Label lblAction;

	private ErrorProvider errorProvider;

	public CryptographyAlgorithms Algorithm => (CryptographyAlgorithms)comboAlgorithm.SelectedItem;

	public CodificationFormats Format => (CodificationFormats)comboFormat.SelectedItem;

	public CryptographyActions Action => (CryptographyActions)comboAction.SelectedItem;

	public string Key => txtKey.Text;

	public string TextToProcess => txtText.Text;

	public CryptographyConfigurationForm(CryptographyComponent cryptographyComponent)
	{
		InitializeComponent();
		this.cryptographyComponent = cryptographyComponent;
		validVariables = ExpressionHelper.GetValidVariables(cryptographyComponent);
		comboAlgorithm.Items.AddRange(new object[2]
		{
			CryptographyAlgorithms.TripleDES,
			CryptographyAlgorithms.HashMD5
		});
		comboFormat.Items.AddRange(new object[2]
		{
			CodificationFormats.Hexadecimal,
			CodificationFormats.Base64
		});
		comboAction.Items.AddRange(new object[2]
		{
			CryptographyActions.Encrypt,
			CryptographyActions.Decrypt
		});
		comboAlgorithm.SelectedItem = cryptographyComponent.Algorithm;
		comboFormat.SelectedItem = cryptographyComponent.Format;
		comboAction.SelectedItem = cryptographyComponent.Action;
		txtKey.Text = cryptographyComponent.Key;
		txtText.Text = cryptographyComponent.Text;
		ComboAlgorithm_SelectionChangeCommitted(comboAlgorithm, EventArgs.Empty);
		TxtText_Validating(txtText, new CancelEventArgs());
		Text = LocalizedResourceMgr.GetString("CryptographyConfigurationForm.Title");
		lblAlgorithm.Text = LocalizedResourceMgr.GetString("CryptographyConfigurationForm.lblAlgorithm.Text");
		lblFormat.Text = LocalizedResourceMgr.GetString("CryptographyConfigurationForm.lblFormat.Text");
		lblAction.Text = LocalizedResourceMgr.GetString("CryptographyConfigurationForm.lblAction.Text");
		lblKey.Text = LocalizedResourceMgr.GetString("CryptographyConfigurationForm.lblKey.Text");
		lblText.Text = LocalizedResourceMgr.GetString("CryptographyConfigurationForm.lblText.Text");
		okButton.Text = LocalizedResourceMgr.GetString("CryptographyConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("CryptographyConfigurationForm.cancelButton.Text");
	}

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void TxtKey_Validating(object sender, CancelEventArgs e)
	{
		switch ((CryptographyAlgorithms)comboAlgorithm.SelectedItem)
		{
		case CryptographyAlgorithms.HashMD5:
			errorProvider.SetError(txtKey, string.Empty);
			break;
		case CryptographyAlgorithms.TripleDES:
			errorProvider.SetError(txtKey, (string.IsNullOrEmpty(txtKey.Text) || txtKey.Text.Length != 24) ? LocalizedResourceMgr.GetString("CryptographyConfigurationForm.Error.InvalidTripleDESKeyLength") : string.Empty);
			break;
		}
	}

	private void TxtText_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtText.Text))
		{
			errorProvider.SetError(textExpressionButton, LocalizedResourceMgr.GetString("CryptographyConfigurationForm.Error.TextIsMandatory"));
			return;
		}
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtText.Text);
		errorProvider.SetError(textExpressionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("CryptographyConfigurationForm.Error.TextIsInvalid"));
	}

	private void ComboAlgorithm_SelectionChangeCommitted(object sender, EventArgs e)
	{
		comboAction.Enabled = (CryptographyAlgorithms)comboAlgorithm.SelectedItem != CryptographyAlgorithms.HashMD5;
		txtKey.Enabled = (CryptographyAlgorithms)comboAlgorithm.SelectedItem != CryptographyAlgorithms.HashMD5;
		TxtKey_Validating(txtKey, new CancelEventArgs());
	}

	private void TextExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(cryptographyComponent)
		{
			Expression = txtText.Text
		};
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

	private void CryptographyConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		cryptographyComponent.ShowHelp();
	}

	private void CryptographyConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		cryptographyComponent.ShowHelp();
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
		this.lblAlgorithm = new System.Windows.Forms.Label();
		this.comboAlgorithm = new System.Windows.Forms.ComboBox();
		this.lblFormat = new System.Windows.Forms.Label();
		this.comboFormat = new System.Windows.Forms.ComboBox();
		this.txtKey = new System.Windows.Forms.TextBox();
		this.lblKey = new System.Windows.Forms.Label();
		this.txtText = new System.Windows.Forms.TextBox();
		this.lblText = new System.Windows.Forms.Label();
		this.cancelButton = new System.Windows.Forms.Button();
		this.okButton = new System.Windows.Forms.Button();
		this.textExpressionButton = new System.Windows.Forms.Button();
		this.comboAction = new System.Windows.Forms.ComboBox();
		this.lblAction = new System.Windows.Forms.Label();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.lblAlgorithm.AutoSize = true;
		this.lblAlgorithm.Location = new System.Drawing.Point(16, 11);
		this.lblAlgorithm.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblAlgorithm.Name = "lblAlgorithm";
		this.lblAlgorithm.Size = new System.Drawing.Size(67, 17);
		this.lblAlgorithm.TabIndex = 0;
		this.lblAlgorithm.Text = "Algorithm";
		this.comboAlgorithm.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboAlgorithm.FormattingEnabled = true;
		this.comboAlgorithm.Location = new System.Drawing.Point(91, 7);
		this.comboAlgorithm.Margin = new System.Windows.Forms.Padding(4);
		this.comboAlgorithm.Name = "comboAlgorithm";
		this.comboAlgorithm.Size = new System.Drawing.Size(459, 24);
		this.comboAlgorithm.TabIndex = 1;
		this.comboAlgorithm.SelectionChangeCommitted += new System.EventHandler(ComboAlgorithm_SelectionChangeCommitted);
		this.lblFormat.AutoSize = true;
		this.lblFormat.Location = new System.Drawing.Point(16, 46);
		this.lblFormat.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblFormat.Name = "lblFormat";
		this.lblFormat.Size = new System.Drawing.Size(52, 17);
		this.lblFormat.TabIndex = 2;
		this.lblFormat.Text = "Format";
		this.comboFormat.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboFormat.FormattingEnabled = true;
		this.comboFormat.Location = new System.Drawing.Point(91, 42);
		this.comboFormat.Margin = new System.Windows.Forms.Padding(4);
		this.comboFormat.Name = "comboFormat";
		this.comboFormat.Size = new System.Drawing.Size(459, 24);
		this.comboFormat.TabIndex = 3;
		this.txtKey.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtKey.Location = new System.Drawing.Point(91, 108);
		this.txtKey.Margin = new System.Windows.Forms.Padding(4);
		this.txtKey.MaxLength = 24;
		this.txtKey.Name = "txtKey";
		this.txtKey.Size = new System.Drawing.Size(459, 22);
		this.txtKey.TabIndex = 7;
		this.txtKey.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtKey.Validating += new System.ComponentModel.CancelEventHandler(TxtKey_Validating);
		this.lblKey.AutoSize = true;
		this.lblKey.Location = new System.Drawing.Point(16, 112);
		this.lblKey.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblKey.Name = "lblKey";
		this.lblKey.Size = new System.Drawing.Size(32, 17);
		this.lblKey.TabIndex = 6;
		this.lblKey.Text = "Key";
		this.txtText.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtText.Location = new System.Drawing.Point(91, 140);
		this.txtText.Margin = new System.Windows.Forms.Padding(4);
		this.txtText.MaxLength = 8192;
		this.txtText.Name = "txtText";
		this.txtText.Size = new System.Drawing.Size(459, 22);
		this.txtText.TabIndex = 9;
		this.txtText.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtText.Validating += new System.ComponentModel.CancelEventHandler(TxtText_Validating);
		this.lblText.AutoSize = true;
		this.lblText.Location = new System.Drawing.Point(16, 144);
		this.lblText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblText.Name = "lblText";
		this.lblText.Size = new System.Drawing.Size(35, 17);
		this.lblText.TabIndex = 8;
		this.lblText.Text = "Text";
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(450, 177);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 12;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(342, 177);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 11;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.textExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.textExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.textExpressionButton.Location = new System.Drawing.Point(559, 138);
		this.textExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.textExpressionButton.Name = "textExpressionButton";
		this.textExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.textExpressionButton.TabIndex = 10;
		this.textExpressionButton.UseVisualStyleBackColor = true;
		this.textExpressionButton.Click += new System.EventHandler(TextExpressionButton_Click);
		this.comboAction.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboAction.FormattingEnabled = true;
		this.comboAction.Location = new System.Drawing.Point(91, 75);
		this.comboAction.Margin = new System.Windows.Forms.Padding(4);
		this.comboAction.Name = "comboAction";
		this.comboAction.Size = new System.Drawing.Size(459, 24);
		this.comboAction.TabIndex = 5;
		this.lblAction.AutoSize = true;
		this.lblAction.Location = new System.Drawing.Point(16, 79);
		this.lblAction.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblAction.Name = "lblAction";
		this.lblAction.Size = new System.Drawing.Size(47, 17);
		this.lblAction.TabIndex = 4;
		this.lblAction.Text = "Action";
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(619, 218);
		base.Controls.Add(this.comboAction);
		base.Controls.Add(this.lblAction);
		base.Controls.Add(this.textExpressionButton);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.Controls.Add(this.lblText);
		base.Controls.Add(this.txtText);
		base.Controls.Add(this.lblKey);
		base.Controls.Add(this.txtKey);
		base.Controls.Add(this.comboFormat);
		base.Controls.Add(this.lblFormat);
		base.Controls.Add(this.comboAlgorithm);
		base.Controls.Add(this.lblAlgorithm);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(1359, 265);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(634, 265);
		base.Name = "CryptographyConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Encryption";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(CryptographyConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(CryptographyConfigurationForm_HelpRequested);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
