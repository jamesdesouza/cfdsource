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

public class ConditionConfigurationForm : Form
{
	private readonly LoopComponent loopComponent;

	private readonly ConditionalComponentBranch conditionalComponentBranch;

	private readonly bool isConditionMandatory = true;

	private readonly List<string> validVariables;

	private IContainer components;

	private TextBox txtCondition;

	private Label lblCondition;

	private Button cancelButton;

	private Button okButton;

	private Button editVariableExpressionButton;

	private ErrorProvider errorProvider;

	public string Condition => txtCondition.Text;

	private void Initialize(string title)
	{
		Text = title;
		lblCondition.Text = LocalizedResourceMgr.GetString("ConditionConfigurationForm.lblCondition.Text");
		okButton.Text = LocalizedResourceMgr.GetString("ConditionConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("ConditionConfigurationForm.cancelButton.Text");
	}

	public ConditionConfigurationForm(LoopComponent loopComponent)
	{
		InitializeComponent();
		this.loopComponent = loopComponent;
		validVariables = ExpressionHelper.GetValidVariables(loopComponent);
		txtCondition.Text = loopComponent.Condition;
		TxtCondition_Validating(txtCondition, new CancelEventArgs());
		Initialize(LocalizedResourceMgr.GetString("ConditionConfigurationForm.Title.Loop"));
	}

	public ConditionConfigurationForm(ConditionalComponentBranch conditionalComponentBranch)
	{
		InitializeComponent();
		this.conditionalComponentBranch = conditionalComponentBranch;
		validVariables = ExpressionHelper.GetValidVariables(conditionalComponentBranch);
		txtCondition.Text = conditionalComponentBranch.Condition;
		isConditionMandatory = conditionalComponentBranch.Parent == null || conditionalComponentBranch != conditionalComponentBranch.Parent.Activities[conditionalComponentBranch.Parent.Activities.Count - 1];
		TxtCondition_Validating(txtCondition, new CancelEventArgs());
		Initialize(LocalizedResourceMgr.GetString("ConditionConfigurationForm.Title.ConditionalBranch"));
	}

	private void TxtBox_Enter(object sender, EventArgs e)
	{
		(sender as TextBox).SelectAll();
	}

	private void TxtCondition_Validating(object sender, CancelEventArgs e)
	{
		if (isConditionMandatory && string.IsNullOrEmpty(txtCondition.Text))
		{
			errorProvider.SetError(editVariableExpressionButton, LocalizedResourceMgr.GetString("ConditionConfigurationForm.Error.ConditionIsMandatory"));
			return;
		}
		if (string.IsNullOrEmpty(txtCondition.Text))
		{
			errorProvider.SetError(editVariableExpressionButton, string.Empty);
			return;
		}
		AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, txtCondition.Text);
		errorProvider.SetError(editVariableExpressionButton, absArgument.IsSafeExpression() ? string.Empty : LocalizedResourceMgr.GetString("ConditionConfigurationForm.Error.ConditionIsInvalid"));
	}

	private void EditVariableExpressionButton_Click(object sender, EventArgs e)
	{
		IVadActivity vadActivity2;
		if (loopComponent != null)
		{
			IVadActivity vadActivity = loopComponent;
			vadActivity2 = vadActivity;
		}
		else
		{
			IVadActivity vadActivity = conditionalComponentBranch;
			vadActivity2 = vadActivity;
		}
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(vadActivity2)
		{
			Expression = txtCondition.Text
		};
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtCondition.Text = expressionEditorForm.Expression;
			TxtCondition_Validating(txtCondition, new CancelEventArgs());
		}
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void ConditionConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		if (loopComponent != null)
		{
			loopComponent.ShowHelp();
		}
		else
		{
			conditionalComponentBranch.ShowHelp();
		}
	}

	private void ConditionConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		if (loopComponent != null)
		{
			loopComponent.ShowHelp();
		}
		else
		{
			conditionalComponentBranch.ShowHelp();
		}
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
		this.txtCondition = new System.Windows.Forms.TextBox();
		this.lblCondition = new System.Windows.Forms.Label();
		this.cancelButton = new System.Windows.Forms.Button();
		this.okButton = new System.Windows.Forms.Button();
		this.editVariableExpressionButton = new System.Windows.Forms.Button();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.txtCondition.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtCondition.Location = new System.Drawing.Point(125, 15);
		this.txtCondition.Margin = new System.Windows.Forms.Padding(4);
		this.txtCondition.MaxLength = 8192;
		this.txtCondition.Name = "txtCondition";
		this.txtCondition.Size = new System.Drawing.Size(424, 22);
		this.txtCondition.TabIndex = 1;
		this.txtCondition.Enter += new System.EventHandler(TxtBox_Enter);
		this.txtCondition.Validating += new System.ComponentModel.CancelEventHandler(TxtCondition_Validating);
		this.lblCondition.AutoSize = true;
		this.lblCondition.Location = new System.Drawing.Point(16, 18);
		this.lblCondition.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblCondition.Name = "lblCondition";
		this.lblCondition.Size = new System.Drawing.Size(67, 17);
		this.lblCondition.TabIndex = 0;
		this.lblCondition.Text = "Condition";
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(451, 52);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 4;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(343, 52);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 3;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.editVariableExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.editVariableExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.editVariableExpressionButton.Location = new System.Drawing.Point(559, 12);
		this.editVariableExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.editVariableExpressionButton.Name = "editVariableExpressionButton";
		this.editVariableExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.editVariableExpressionButton.TabIndex = 2;
		this.editVariableExpressionButton.UseVisualStyleBackColor = true;
		this.editVariableExpressionButton.Click += new System.EventHandler(EditVariableExpressionButton_Click);
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(619, 85);
		base.Controls.Add(this.editVariableExpressionButton);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.Controls.Add(this.lblCondition);
		base.Controls.Add(this.txtCondition);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(1359, 132);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(634, 132);
		base.Name = "ConditionConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Condition";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(ConditionConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(ConditionConfigurationForm_HelpRequested);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
