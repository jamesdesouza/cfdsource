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

public class TcxSetExtensionStatusConfigurationForm : Form
{
	private readonly TcxSetExtensionStatusComponent tcxSetExtensionStatusComponent;

	private readonly List<string> validVariables;

	private IContainer components;

	private Label lblExtension;

	private Button cancelButton;

	private Button okButton;

	private ErrorProvider errorProvider;

	private Button editExtensionButton;

	private TextBox txtExtension;

	private Label lblStatus;

	private ComboBox comboStatus;

	public string Extension => txtExtension.Text;

	public ExtensionStatus Status => ComboIndexToExtensionStatus(comboStatus.SelectedIndex);

	private int ExtensionStatusToComboIndex(ExtensionStatus status)
	{
		return status switch
		{
			ExtensionStatus.Available => 0, 
			ExtensionStatus.Away => 1, 
			ExtensionStatus.DoNotDisturb => 2, 
			ExtensionStatus.Lunch => 3, 
			_ => 4, 
		};
	}

	private ExtensionStatus ComboIndexToExtensionStatus(int index)
	{
		return index switch
		{
			0 => ExtensionStatus.Available, 
			1 => ExtensionStatus.Away, 
			2 => ExtensionStatus.DoNotDisturb, 
			3 => ExtensionStatus.Lunch, 
			_ => ExtensionStatus.BusinessTrip, 
		};
	}

	public TcxSetExtensionStatusConfigurationForm(TcxSetExtensionStatusComponent tcxSetExtensionStatusComponent)
	{
		InitializeComponent();
		this.tcxSetExtensionStatusComponent = tcxSetExtensionStatusComponent;
		validVariables = ExpressionHelper.GetValidVariables(tcxSetExtensionStatusComponent);
		txtExtension.Text = tcxSetExtensionStatusComponent.Extension;
		comboStatus.SelectedIndex = ExtensionStatusToComboIndex(tcxSetExtensionStatusComponent.Status);
		TxtExtension_Validating(txtExtension, new CancelEventArgs());
		Text = LocalizedResourceMgr.GetString("TcxSetExtensionStatusConfigurationForm.Title");
		lblExtension.Text = LocalizedResourceMgr.GetString("TcxSetExtensionStatusConfigurationForm.lblExtension.Text");
		lblStatus.Text = LocalizedResourceMgr.GetString("TcxSetExtensionStatusConfigurationForm.lblStatus.Text");
		okButton.Text = LocalizedResourceMgr.GetString("TcxSetExtensionStatusConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("TcxSetExtensionStatusConfigurationForm.cancelButton.Text");
	}

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void TxtExtension_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtExtension.Text))
		{
			errorProvider.SetError(editExtensionButton, LocalizedResourceMgr.GetString("TcxSetExtensionStatusConfigurationForm.Error.ExtensionIsMandatory"));
			return;
		}
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtExtension.Text);
		errorProvider.SetError(editExtensionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("TcxSetExtensionStatusConfigurationForm.Error.ExtensionIsInvalid"));
	}

	private void EditExtensionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(tcxSetExtensionStatusComponent);
		expressionEditorForm.Expression = txtExtension.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtExtension.Text = expressionEditorForm.Expression;
			TxtExtension_Validating(txtExtension, new CancelEventArgs());
		}
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void TcxSetExtensionStatusConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		tcxSetExtensionStatusComponent.ShowHelp();
	}

	private void TcxSetExtensionStatusConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		tcxSetExtensionStatusComponent.ShowHelp();
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
		this.cancelButton = new System.Windows.Forms.Button();
		this.okButton = new System.Windows.Forms.Button();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.editExtensionButton = new System.Windows.Forms.Button();
		this.txtExtension = new System.Windows.Forms.TextBox();
		this.lblStatus = new System.Windows.Forms.Label();
		this.comboStatus = new System.Windows.Forms.ComboBox();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.lblExtension.AutoSize = true;
		this.lblExtension.Location = new System.Drawing.Point(16, 11);
		this.lblExtension.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblExtension.Name = "lblExtension";
		this.lblExtension.Size = new System.Drawing.Size(69, 17);
		this.lblExtension.TabIndex = 0;
		this.lblExtension.Text = "Extension";
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(451, 81);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 6;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(343, 81);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 5;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
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
		this.lblStatus.AutoSize = true;
		this.lblStatus.Location = new System.Drawing.Point(16, 40);
		this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblStatus.Name = "lblStatus";
		this.lblStatus.Size = new System.Drawing.Size(48, 17);
		this.lblStatus.TabIndex = 3;
		this.lblStatus.Text = "Status";
		this.comboStatus.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboStatus.FormattingEnabled = true;
		this.comboStatus.Items.AddRange(new object[5] { "Available", "Away", "Do Not Disturb", "Lunch", "Business Trip" });
		this.comboStatus.Location = new System.Drawing.Point(127, 37);
		this.comboStatus.Margin = new System.Windows.Forms.Padding(4);
		this.comboStatus.Name = "comboStatus";
		this.comboStatus.Size = new System.Drawing.Size(423, 24);
		this.comboStatus.TabIndex = 4;
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(619, 115);
		base.Controls.Add(this.comboStatus);
		base.Controls.Add(this.lblStatus);
		base.Controls.Add(this.editExtensionButton);
		base.Controls.Add(this.txtExtension);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.Controls.Add(this.lblExtension);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(1359, 162);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(634, 162);
		base.Name = "TcxSetExtensionStatusConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Set Extension Status";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(TcxSetExtensionStatusConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(TcxSetExtensionStatusConfigurationForm_HelpRequested);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
