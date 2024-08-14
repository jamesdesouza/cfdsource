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

public class TcxGetLastCallConfigurationForm : Form
{
	private readonly TcxGetLastCallComponent tcxGetLastCallComponent;

	private readonly List<string> validVariables;

	private IContainer components;

	private Label lblNumber;

	private Button cancelButton;

	private Button okButton;

	private ErrorProvider errorProvider;

	private Button editNumberButton;

	private TextBox txtNumber;

	private Label lblCallType;

	private ComboBox comboCallType;

	public string Number => txtNumber.Text;

	public CallDirections CallType => ComboIndexToCallType(comboCallType.SelectedIndex);

	private int CallTypeToComboIndex(CallDirections callType)
	{
		return callType switch
		{
			CallDirections.Inbound => 0, 
			CallDirections.Outbound => 1, 
			_ => 2, 
		};
	}

	private CallDirections ComboIndexToCallType(int index)
	{
		return index switch
		{
			0 => CallDirections.Inbound, 
			1 => CallDirections.Outbound, 
			_ => CallDirections.Both, 
		};
	}

	public TcxGetLastCallConfigurationForm(TcxGetLastCallComponent tcxGetLastCallComponent)
	{
		InitializeComponent();
		this.tcxGetLastCallComponent = tcxGetLastCallComponent;
		validVariables = ExpressionHelper.GetValidVariables(tcxGetLastCallComponent);
		txtNumber.Text = tcxGetLastCallComponent.Number;
		comboCallType.SelectedIndex = CallTypeToComboIndex(tcxGetLastCallComponent.CallType);
		TxtNumber_Validating(txtNumber, new CancelEventArgs());
		Text = LocalizedResourceMgr.GetString("TcxGetLastCallConfigurationForm.Title");
		lblNumber.Text = LocalizedResourceMgr.GetString("TcxGetLastCallConfigurationForm.lblNumber.Text");
		lblCallType.Text = LocalizedResourceMgr.GetString("TcxGetLastCallConfigurationForm.lblCallType.Text");
		okButton.Text = LocalizedResourceMgr.GetString("TcxGetLastCallConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("TcxGetLastCallConfigurationForm.cancelButton.Text");
	}

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void TxtNumber_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtNumber.Text))
		{
			errorProvider.SetError(editNumberButton, LocalizedResourceMgr.GetString("TcxGetLastCallConfigurationForm.Error.NumberIsMandatory"));
			return;
		}
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtNumber.Text);
		errorProvider.SetError(editNumberButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("TcxGetLastCallConfigurationForm.Error.NumberIsInvalid"));
	}

	private void EditNumberButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(tcxGetLastCallComponent);
		expressionEditorForm.Expression = txtNumber.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtNumber.Text = expressionEditorForm.Expression;
			TxtNumber_Validating(txtNumber, new CancelEventArgs());
		}
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void TcxGetLastCallConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		tcxGetLastCallComponent.ShowHelp();
	}

	private void TcxGetLastCallConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		tcxGetLastCallComponent.ShowHelp();
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
		this.lblNumber = new System.Windows.Forms.Label();
		this.cancelButton = new System.Windows.Forms.Button();
		this.okButton = new System.Windows.Forms.Button();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.editNumberButton = new System.Windows.Forms.Button();
		this.txtNumber = new System.Windows.Forms.TextBox();
		this.lblCallType = new System.Windows.Forms.Label();
		this.comboCallType = new System.Windows.Forms.ComboBox();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.lblNumber.AutoSize = true;
		this.lblNumber.Location = new System.Drawing.Point(16, 11);
		this.lblNumber.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblNumber.Name = "lblNumber";
		this.lblNumber.Size = new System.Drawing.Size(58, 17);
		this.lblNumber.TabIndex = 0;
		this.lblNumber.Text = "Number";
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
		this.editNumberButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.editNumberButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.editNumberButton.Location = new System.Drawing.Point(559, 5);
		this.editNumberButton.Margin = new System.Windows.Forms.Padding(4);
		this.editNumberButton.Name = "editNumberButton";
		this.editNumberButton.Size = new System.Drawing.Size(39, 28);
		this.editNumberButton.TabIndex = 2;
		this.editNumberButton.UseVisualStyleBackColor = true;
		this.editNumberButton.Click += new System.EventHandler(EditNumberButton_Click);
		this.txtNumber.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtNumber.Location = new System.Drawing.Point(127, 7);
		this.txtNumber.Margin = new System.Windows.Forms.Padding(4);
		this.txtNumber.MaxLength = 8192;
		this.txtNumber.Name = "txtNumber";
		this.txtNumber.Size = new System.Drawing.Size(423, 22);
		this.txtNumber.TabIndex = 1;
		this.txtNumber.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtNumber.Validating += new System.ComponentModel.CancelEventHandler(TxtNumber_Validating);
		this.lblCallType.AutoSize = true;
		this.lblCallType.Location = new System.Drawing.Point(16, 40);
		this.lblCallType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblCallType.Name = "lblCallType";
		this.lblCallType.Size = new System.Drawing.Size(67, 17);
		this.lblCallType.TabIndex = 3;
		this.lblCallType.Text = "Call Type";
		this.comboCallType.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboCallType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboCallType.FormattingEnabled = true;
		this.comboCallType.Items.AddRange(new object[3] { "Inbound", "Outbound", "Both" });
		this.comboCallType.Location = new System.Drawing.Point(127, 37);
		this.comboCallType.Margin = new System.Windows.Forms.Padding(4);
		this.comboCallType.Name = "comboCallType";
		this.comboCallType.Size = new System.Drawing.Size(423, 24);
		this.comboCallType.TabIndex = 4;
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(619, 115);
		base.Controls.Add(this.comboCallType);
		base.Controls.Add(this.lblCallType);
		base.Controls.Add(this.editNumberButton);
		base.Controls.Add(this.txtNumber);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.Controls.Add(this.lblNumber);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(1359, 162);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(634, 162);
		base.Name = "TcxGetLastCallConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Get Last Call";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(TcxGetLastCallConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(TcxGetLastCallConfigurationForm_HelpRequested);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
