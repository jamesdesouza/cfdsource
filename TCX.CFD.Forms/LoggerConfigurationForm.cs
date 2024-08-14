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

public class LoggerConfigurationForm : Form
{
	private readonly LoggerComponent loggerComponent;

	private readonly List<string> validVariables;

	private IContainer components;

	private Label lblLevel;

	private TextBox txtText;

	private Label lblText;

	private Button cancelButton;

	private Button okButton;

	private Button editTextButton;

	private ErrorProvider errorProvider;

	private ComboBox comboLevel;

	public LogLevels Level => IndexToLevel(comboLevel.SelectedIndex);

	public string LogText => txtText.Text;

	private int LevelToIndex(LogLevels level)
	{
		return level switch
		{
			LogLevels.Critical => 0, 
			LogLevels.Error => 1, 
			LogLevels.Warn => 2, 
			LogLevels.Info => 3, 
			LogLevels.Debug => 4, 
			LogLevels.Trace => 5, 
			_ => -1, 
		};
	}

	private LogLevels IndexToLevel(int index)
	{
		return index switch
		{
			0 => LogLevels.Critical, 
			1 => LogLevels.Error, 
			2 => LogLevels.Warn, 
			3 => LogLevels.Info, 
			4 => LogLevels.Debug, 
			_ => LogLevels.Trace, 
		};
	}

	public LoggerConfigurationForm(LoggerComponent loggerComponent)
	{
		InitializeComponent();
		this.loggerComponent = loggerComponent;
		validVariables = ExpressionHelper.GetValidVariables(loggerComponent);
		comboLevel.SelectedIndex = LevelToIndex(loggerComponent.Level);
		txtText.Text = loggerComponent.Text;
		TxtText_Validating(txtText, new CancelEventArgs());
		Text = LocalizedResourceMgr.GetString("LoggerConfigurationForm.Title");
		lblLevel.Text = LocalizedResourceMgr.GetString("LoggerConfigurationForm.lblLevel.Text");
		lblText.Text = LocalizedResourceMgr.GetString("LoggerConfigurationForm.lblText.Text");
		okButton.Text = LocalizedResourceMgr.GetString("LoggerConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("LoggerConfigurationForm.cancelButton.Text");
	}

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void TxtText_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtText.Text))
		{
			errorProvider.SetError(editTextButton, LocalizedResourceMgr.GetString("LoggerConfigurationForm.Error.TextIsMandatory"));
			return;
		}
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtText.Text);
		errorProvider.SetError(editTextButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("LoggerConfigurationForm.Error.TextIsInvalid"));
	}

	private void EditTextButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(loggerComponent);
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

	private void LoggerConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		loggerComponent.ShowHelp();
	}

	private void LoggerConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		loggerComponent.ShowHelp();
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
		this.lblLevel = new System.Windows.Forms.Label();
		this.txtText = new System.Windows.Forms.TextBox();
		this.lblText = new System.Windows.Forms.Label();
		this.cancelButton = new System.Windows.Forms.Button();
		this.okButton = new System.Windows.Forms.Button();
		this.editTextButton = new System.Windows.Forms.Button();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.comboLevel = new System.Windows.Forms.ComboBox();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.lblLevel.AutoSize = true;
		this.lblLevel.Location = new System.Drawing.Point(16, 11);
		this.lblLevel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblLevel.Name = "lblLevel";
		this.lblLevel.Size = new System.Drawing.Size(42, 17);
		this.lblLevel.TabIndex = 0;
		this.lblLevel.Text = "Level";
		this.txtText.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtText.Location = new System.Drawing.Point(68, 43);
		this.txtText.Margin = new System.Windows.Forms.Padding(4);
		this.txtText.MaxLength = 8192;
		this.txtText.Name = "txtText";
		this.txtText.Size = new System.Drawing.Size(481, 22);
		this.txtText.TabIndex = 4;
		this.txtText.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtText.Validating += new System.ComponentModel.CancelEventHandler(TxtText_Validating);
		this.lblText.AutoSize = true;
		this.lblText.Location = new System.Drawing.Point(16, 47);
		this.lblText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblText.Name = "lblText";
		this.lblText.Size = new System.Drawing.Size(35, 17);
		this.lblText.TabIndex = 3;
		this.lblText.Text = "Text";
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
		this.editTextButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.editTextButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.editTextButton.Location = new System.Drawing.Point(559, 41);
		this.editTextButton.Margin = new System.Windows.Forms.Padding(4);
		this.editTextButton.Name = "editTextButton";
		this.editTextButton.Size = new System.Drawing.Size(39, 28);
		this.editTextButton.TabIndex = 5;
		this.editTextButton.UseVisualStyleBackColor = true;
		this.editTextButton.Click += new System.EventHandler(EditTextButton_Click);
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.comboLevel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboLevel.FormattingEnabled = true;
		this.comboLevel.Items.AddRange(new object[6] { "Critical", "Error", "Warn", "Info", "Debug", "Trace" });
		this.comboLevel.Location = new System.Drawing.Point(68, 10);
		this.comboLevel.Margin = new System.Windows.Forms.Padding(4);
		this.comboLevel.Name = "comboLevel";
		this.comboLevel.Size = new System.Drawing.Size(481, 24);
		this.comboLevel.TabIndex = 8;
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(619, 114);
		base.Controls.Add(this.comboLevel);
		base.Controls.Add(this.editTextButton);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.Controls.Add(this.lblText);
		base.Controls.Add(this.txtText);
		base.Controls.Add(this.lblLevel);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(1359, 161);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(634, 161);
		base.Name = "LoggerConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Logger";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(LoggerConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(LoggerConfigurationForm_HelpRequested);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
