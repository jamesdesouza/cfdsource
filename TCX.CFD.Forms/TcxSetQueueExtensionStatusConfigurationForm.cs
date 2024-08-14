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

public class TcxSetQueueExtensionStatusConfigurationForm : Form
{
	private readonly TcxSetQueueExtensionStatusComponent tcxSetQueueExtensionStatusComponent;

	private readonly List<string> validVariables;

	private IContainer components;

	private Label lblExtension;

	private Button cancelButton;

	private Button okButton;

	private ErrorProvider errorProvider;

	private Button editExtensionButton;

	private TextBox txtExtension;

	private Label lblQueueStatus;

	private ComboBox comboQueueStatus;

	private Button editQueueExtensionButton;

	private TextBox txtQueueExtension;

	private Label lblQueueExtension;

	private ComboBox comboQueueMode;

	private Label lblQueueMode;

	public string Extension => txtExtension.Text;

	public string QueueExtension => txtQueueExtension.Text;

	public QueueStatusOperationModes QueueMode => (QueueStatusOperationModes)comboQueueMode.SelectedItem;

	public QueueStatusTypes QueueStatus => (QueueStatusTypes)comboQueueStatus.SelectedItem;

	public TcxSetQueueExtensionStatusConfigurationForm(TcxSetQueueExtensionStatusComponent tcxSetQueueExtensionStatusComponent)
	{
		InitializeComponent();
		this.tcxSetQueueExtensionStatusComponent = tcxSetQueueExtensionStatusComponent;
		validVariables = ExpressionHelper.GetValidVariables(tcxSetQueueExtensionStatusComponent);
		comboQueueMode.Items.AddRange(new object[2]
		{
			QueueStatusOperationModes.SpecificQueue,
			QueueStatusOperationModes.Global
		});
		comboQueueStatus.Items.AddRange(new object[2]
		{
			QueueStatusTypes.LoggedIn,
			QueueStatusTypes.LoggedOut
		});
		comboQueueMode.SelectedItem = tcxSetQueueExtensionStatusComponent.QueueMode;
		comboQueueStatus.SelectedItem = tcxSetQueueExtensionStatusComponent.QueueStatus;
		txtExtension.Text = tcxSetQueueExtensionStatusComponent.Extension;
		txtQueueExtension.Text = tcxSetQueueExtensionStatusComponent.QueueExtension;
		TxtExtension_Validating(txtExtension, new CancelEventArgs());
		TxtQueueExtension_Validating(txtQueueExtension, new CancelEventArgs());
		Text = LocalizedResourceMgr.GetString("TcxSetQueueExtensionStatusConfigurationForm.Title");
		lblQueueMode.Text = LocalizedResourceMgr.GetString("TcxSetQueueExtensionStatusConfigurationForm.lblQueueMode.Text");
		lblExtension.Text = LocalizedResourceMgr.GetString("TcxSetQueueExtensionStatusConfigurationForm.lblExtension.Text");
		lblQueueExtension.Text = LocalizedResourceMgr.GetString("TcxSetQueueExtensionStatusConfigurationForm.lblQueueExtension.Text");
		lblQueueStatus.Text = LocalizedResourceMgr.GetString("TcxSetQueueExtensionStatusConfigurationForm.lblQueueStatus.Text");
		okButton.Text = LocalizedResourceMgr.GetString("TcxSetQueueExtensionStatusConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("TcxSetQueueExtensionStatusConfigurationForm.cancelButton.Text");
	}

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void ComboQueueMode_SelectedIndexChanged(object sender, EventArgs e)
	{
		txtQueueExtension.Enabled = QueueMode == QueueStatusOperationModes.SpecificQueue;
		editQueueExtensionButton.Enabled = QueueMode == QueueStatusOperationModes.SpecificQueue;
		TxtQueueExtension_Validating(txtQueueExtension, new CancelEventArgs());
	}

	private void TxtExtension_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtExtension.Text))
		{
			errorProvider.SetError(editExtensionButton, LocalizedResourceMgr.GetString("TcxSetQueueExtensionStatusConfigurationForm.Error.ExtensionIsMandatory"));
			return;
		}
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtExtension.Text);
		errorProvider.SetError(editExtensionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("TcxSetQueueExtensionStatusConfigurationForm.Error.ExtensionIsInvalid"));
	}

	private void EditExtensionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(tcxSetQueueExtensionStatusComponent);
		expressionEditorForm.Expression = txtExtension.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtExtension.Text = expressionEditorForm.Expression;
			TxtExtension_Validating(txtExtension, new CancelEventArgs());
		}
	}

	private void TxtQueueExtension_Validating(object sender, CancelEventArgs e)
	{
		if (QueueMode == QueueStatusOperationModes.SpecificQueue)
		{
			if (string.IsNullOrEmpty(txtQueueExtension.Text))
			{
				errorProvider.SetError(editQueueExtensionButton, LocalizedResourceMgr.GetString("TcxSetQueueExtensionStatusConfigurationForm.Error.QueueExtensionIsMandatory"));
				return;
			}
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtQueueExtension.Text);
			errorProvider.SetError(editQueueExtensionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("TcxSetQueueExtensionStatusConfigurationForm.Error.QueueExtensionIsInvalid"));
		}
		else
		{
			errorProvider.SetError(editQueueExtensionButton, string.Empty);
		}
	}

	private void EditQueueExtensionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(tcxSetQueueExtensionStatusComponent);
		expressionEditorForm.Expression = txtQueueExtension.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtQueueExtension.Text = expressionEditorForm.Expression;
			TxtQueueExtension_Validating(txtQueueExtension, new CancelEventArgs());
		}
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void TcxSetQueueExtensionStatusConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		tcxSetQueueExtensionStatusComponent.ShowHelp();
	}

	private void TcxSetQueueExtensionStatusConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		tcxSetQueueExtensionStatusComponent.ShowHelp();
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
		this.lblQueueStatus = new System.Windows.Forms.Label();
		this.comboQueueStatus = new System.Windows.Forms.ComboBox();
		this.comboQueueMode = new System.Windows.Forms.ComboBox();
		this.lblQueueMode = new System.Windows.Forms.Label();
		this.editQueueExtensionButton = new System.Windows.Forms.Button();
		this.txtQueueExtension = new System.Windows.Forms.TextBox();
		this.lblQueueExtension = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.lblExtension.AutoSize = true;
		this.lblExtension.Location = new System.Drawing.Point(17, 49);
		this.lblExtension.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblExtension.Name = "lblExtension";
		this.lblExtension.Size = new System.Drawing.Size(69, 17);
		this.lblExtension.TabIndex = 2;
		this.lblExtension.Text = "Extension";
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(451, 146);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 11;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(343, 146);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 10;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.editExtensionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.editExtensionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.editExtensionButton.Location = new System.Drawing.Point(560, 43);
		this.editExtensionButton.Margin = new System.Windows.Forms.Padding(4);
		this.editExtensionButton.Name = "editExtensionButton";
		this.editExtensionButton.Size = new System.Drawing.Size(39, 28);
		this.editExtensionButton.TabIndex = 4;
		this.editExtensionButton.UseVisualStyleBackColor = true;
		this.editExtensionButton.Click += new System.EventHandler(EditExtensionButton_Click);
		this.txtExtension.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtExtension.Location = new System.Drawing.Point(141, 45);
		this.txtExtension.Margin = new System.Windows.Forms.Padding(4);
		this.txtExtension.MaxLength = 8192;
		this.txtExtension.Name = "txtExtension";
		this.txtExtension.Size = new System.Drawing.Size(410, 22);
		this.txtExtension.TabIndex = 3;
		this.txtExtension.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtExtension.Validating += new System.ComponentModel.CancelEventHandler(TxtExtension_Validating);
		this.lblQueueStatus.AutoSize = true;
		this.lblQueueStatus.Location = new System.Drawing.Point(17, 108);
		this.lblQueueStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblQueueStatus.Name = "lblQueueStatus";
		this.lblQueueStatus.Size = new System.Drawing.Size(48, 17);
		this.lblQueueStatus.TabIndex = 8;
		this.lblQueueStatus.Text = "Status";
		this.comboQueueStatus.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboQueueStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboQueueStatus.FormattingEnabled = true;
		this.comboQueueStatus.Location = new System.Drawing.Point(141, 105);
		this.comboQueueStatus.Margin = new System.Windows.Forms.Padding(4);
		this.comboQueueStatus.Name = "comboQueueStatus";
		this.comboQueueStatus.Size = new System.Drawing.Size(410, 24);
		this.comboQueueStatus.TabIndex = 9;
		this.comboQueueMode.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboQueueMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboQueueMode.FormattingEnabled = true;
		this.comboQueueMode.Location = new System.Drawing.Point(141, 13);
		this.comboQueueMode.Margin = new System.Windows.Forms.Padding(4);
		this.comboQueueMode.Name = "comboQueueMode";
		this.comboQueueMode.Size = new System.Drawing.Size(410, 24);
		this.comboQueueMode.TabIndex = 1;
		this.comboQueueMode.SelectedIndexChanged += new System.EventHandler(ComboQueueMode_SelectedIndexChanged);
		this.lblQueueMode.AutoSize = true;
		this.lblQueueMode.Location = new System.Drawing.Point(17, 16);
		this.lblQueueMode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblQueueMode.Name = "lblQueueMode";
		this.lblQueueMode.Size = new System.Drawing.Size(43, 17);
		this.lblQueueMode.TabIndex = 0;
		this.lblQueueMode.Text = "Mode";
		this.editQueueExtensionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.editQueueExtensionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.editQueueExtensionButton.Location = new System.Drawing.Point(560, 73);
		this.editQueueExtensionButton.Margin = new System.Windows.Forms.Padding(4);
		this.editQueueExtensionButton.Name = "editQueueExtensionButton";
		this.editQueueExtensionButton.Size = new System.Drawing.Size(39, 28);
		this.editQueueExtensionButton.TabIndex = 7;
		this.editQueueExtensionButton.UseVisualStyleBackColor = true;
		this.editQueueExtensionButton.Click += new System.EventHandler(EditQueueExtensionButton_Click);
		this.txtQueueExtension.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtQueueExtension.Location = new System.Drawing.Point(141, 75);
		this.txtQueueExtension.Margin = new System.Windows.Forms.Padding(4);
		this.txtQueueExtension.MaxLength = 8192;
		this.txtQueueExtension.Name = "txtQueueExtension";
		this.txtQueueExtension.Size = new System.Drawing.Size(410, 22);
		this.txtQueueExtension.TabIndex = 6;
		this.txtQueueExtension.Validating += new System.ComponentModel.CancelEventHandler(TxtQueueExtension_Validating);
		this.lblQueueExtension.AutoSize = true;
		this.lblQueueExtension.Location = new System.Drawing.Point(17, 79);
		this.lblQueueExtension.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblQueueExtension.Name = "lblQueueExtension";
		this.lblQueueExtension.Size = new System.Drawing.Size(116, 17);
		this.lblQueueExtension.TabIndex = 5;
		this.lblQueueExtension.Text = "Queue Extension";
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(619, 180);
		base.Controls.Add(this.editQueueExtensionButton);
		base.Controls.Add(this.txtQueueExtension);
		base.Controls.Add(this.lblQueueExtension);
		base.Controls.Add(this.comboQueueMode);
		base.Controls.Add(this.lblQueueMode);
		base.Controls.Add(this.comboQueueStatus);
		base.Controls.Add(this.lblQueueStatus);
		base.Controls.Add(this.editExtensionButton);
		base.Controls.Add(this.txtExtension);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.Controls.Add(this.lblExtension);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(634, 162);
		base.Name = "TcxSetQueueExtensionStatusConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Set Queue Extension Status";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(TcxSetQueueExtensionStatusConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(TcxSetQueueExtensionStatusConfigurationForm_HelpRequested);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
