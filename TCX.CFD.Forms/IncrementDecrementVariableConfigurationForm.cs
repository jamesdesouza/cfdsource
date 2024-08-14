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

public class IncrementDecrementVariableConfigurationForm : Form
{
	private readonly IncrementVariableComponent incrementVariableComponent;

	private readonly DecrementVariableComponent decrementVariableComponent;

	private readonly List<string> validVariables;

	private IContainer components;

	private TextBox txtVariableName;

	private Label lblVariableName;

	private Button cancelButton;

	private Button okButton;

	private Button selectVariableNameButton;

	private ErrorProvider errorProvider;

	public string VariableName => txtVariableName.Text;

	private void Initialize(string title)
	{
		TxtVariableName_Validating(txtVariableName, new CancelEventArgs());
		Text = title;
		lblVariableName.Text = LocalizedResourceMgr.GetString("IncrementDecrementVariableConfigurationForm.lblVariableName.Text");
		okButton.Text = LocalizedResourceMgr.GetString("IncrementDecrementVariableConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("IncrementDecrementVariableConfigurationForm.cancelButton.Text");
	}

	public IncrementDecrementVariableConfigurationForm(IncrementVariableComponent incrementVariableComponent)
	{
		InitializeComponent();
		this.incrementVariableComponent = incrementVariableComponent;
		validVariables = ExpressionHelper.GetValidVariables(incrementVariableComponent);
		txtVariableName.Text = incrementVariableComponent.VariableName;
		Initialize(LocalizedResourceMgr.GetString("IncrementDecrementVariableConfigurationForm.Title.Increment"));
	}

	public IncrementDecrementVariableConfigurationForm(DecrementVariableComponent decrementVariableComponent)
	{
		InitializeComponent();
		this.decrementVariableComponent = decrementVariableComponent;
		validVariables = ExpressionHelper.GetValidVariables(decrementVariableComponent);
		txtVariableName.Text = decrementVariableComponent.VariableName;
		Initialize(LocalizedResourceMgr.GetString("IncrementDecrementVariableConfigurationForm.Title.Decrement"));
	}

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void TxtVariableName_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtVariableName.Text))
		{
			errorProvider.SetError(selectVariableNameButton, LocalizedResourceMgr.GetString("IncrementDecrementVariableConfigurationForm.Error.VariableNameIsMandatory"));
			return;
		}
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtVariableName.Text);
		errorProvider.SetError(selectVariableNameButton, absArgument.IsVariableName() ? string.Empty : LocalizedResourceMgr.GetString("IncrementDecrementVariableConfigurationForm.Error.UnknownVariableName"));
	}

	private void SelectVariableNameButton_Click(object sender, EventArgs e)
	{
		IVadActivity vadActivity2;
		if (incrementVariableComponent != null)
		{
			IVadActivity vadActivity = incrementVariableComponent;
			vadActivity2 = vadActivity;
		}
		else
		{
			IVadActivity vadActivity = decrementVariableComponent;
			vadActivity2 = vadActivity;
		}
		IVadActivity component = vadActivity2;
		LeftHandSideVariableSelectorForm leftHandSideVariableSelectorForm = new LeftHandSideVariableSelectorForm
		{
			Component = component,
			VariableName = txtVariableName.Text
		};
		if (leftHandSideVariableSelectorForm.ShowDialog() == DialogResult.OK)
		{
			txtVariableName.Text = leftHandSideVariableSelectorForm.VariableName;
			TxtVariableName_Validating(txtVariableName, new CancelEventArgs());
		}
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void IncrementDecrementVariableConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		IVadActivity vadActivity2;
		if (incrementVariableComponent != null)
		{
			IVadActivity vadActivity = incrementVariableComponent;
			vadActivity2 = vadActivity;
		}
		else
		{
			IVadActivity vadActivity = decrementVariableComponent;
			vadActivity2 = vadActivity;
		}
		vadActivity2.ShowHelp();
	}

	private void IncrementDecrementVariableConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		IVadActivity vadActivity2;
		if (incrementVariableComponent != null)
		{
			IVadActivity vadActivity = incrementVariableComponent;
			vadActivity2 = vadActivity;
		}
		else
		{
			IVadActivity vadActivity = decrementVariableComponent;
			vadActivity2 = vadActivity;
		}
		vadActivity2.ShowHelp();
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
		this.txtVariableName = new System.Windows.Forms.TextBox();
		this.lblVariableName = new System.Windows.Forms.Label();
		this.cancelButton = new System.Windows.Forms.Button();
		this.okButton = new System.Windows.Forms.Button();
		this.selectVariableNameButton = new System.Windows.Forms.Button();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.txtVariableName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtVariableName.Location = new System.Drawing.Point(125, 11);
		this.txtVariableName.Margin = new System.Windows.Forms.Padding(4);
		this.txtVariableName.MaxLength = 8192;
		this.txtVariableName.Name = "txtVariableName";
		this.txtVariableName.Size = new System.Drawing.Size(424, 22);
		this.txtVariableName.TabIndex = 1;
		this.txtVariableName.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtVariableName.Validating += new System.ComponentModel.CancelEventHandler(TxtVariableName_Validating);
		this.lblVariableName.AutoSize = true;
		this.lblVariableName.Location = new System.Drawing.Point(16, 11);
		this.lblVariableName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblVariableName.Name = "lblVariableName";
		this.lblVariableName.Size = new System.Drawing.Size(101, 17);
		this.lblVariableName.TabIndex = 0;
		this.lblVariableName.Text = "Variable Name";
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(451, 55);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 4;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(343, 55);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 3;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.selectVariableNameButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.selectVariableNameButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.selectVariableNameButton.Location = new System.Drawing.Point(559, 9);
		this.selectVariableNameButton.Margin = new System.Windows.Forms.Padding(4);
		this.selectVariableNameButton.Name = "selectVariableNameButton";
		this.selectVariableNameButton.Size = new System.Drawing.Size(39, 28);
		this.selectVariableNameButton.TabIndex = 2;
		this.selectVariableNameButton.UseVisualStyleBackColor = true;
		this.selectVariableNameButton.Click += new System.EventHandler(SelectVariableNameButton_Click);
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(619, 89);
		base.Controls.Add(this.selectVariableNameButton);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.Controls.Add(this.lblVariableName);
		base.Controls.Add(this.txtVariableName);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(1359, 136);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(634, 136);
		base.Name = "IncrementDecrementVariableConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Increment / Decrement Variable";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(IncrementDecrementVariableConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(IncrementDecrementVariableConfigurationForm_HelpRequested);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
