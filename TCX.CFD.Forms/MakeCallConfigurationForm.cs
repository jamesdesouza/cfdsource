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

public class MakeCallConfigurationForm : Form
{
	private readonly MakeCallComponent makeCallComponent;

	private readonly List<string> validVariables;

	private IContainer components;

	private Label lblDestination;

	private TextBox txtDestination;

	private Button cancelButton;

	private Button okButton;

	private Button destinationExpressionButton;

	private ErrorProvider errorProvider;

	private Button originExpressionButton;

	private TextBox txtOrigin;

	private Label lblOrigin;

	private Label lblTimeout;

	private MaskedTextBox txtTimeout;

	public string Origin => txtOrigin.Text;

	public string Destination => txtDestination.Text;

	public uint Timeout
	{
		get
		{
			if (!string.IsNullOrEmpty(txtTimeout.Text))
			{
				return Convert.ToUInt32(txtTimeout.Text);
			}
			return 30u;
		}
	}

	public MakeCallConfigurationForm(MakeCallComponent makeCallComponent)
	{
		InitializeComponent();
		this.makeCallComponent = makeCallComponent;
		validVariables = ExpressionHelper.GetValidVariables(makeCallComponent);
		txtOrigin.Text = makeCallComponent.Origin;
		txtDestination.Text = makeCallComponent.Destination;
		TxtOrigin_Validating(txtOrigin, new CancelEventArgs());
		TxtDestination_Validating(txtDestination, new CancelEventArgs());
		txtTimeout.Text = makeCallComponent.Timeout.ToString();
		Text = LocalizedResourceMgr.GetString("MakeCallConfigurationForm.Title");
		lblOrigin.Text = LocalizedResourceMgr.GetString("MakeCallConfigurationForm.lblOrigin.Text");
		lblDestination.Text = LocalizedResourceMgr.GetString("MakeCallConfigurationForm.lblDestination.Text");
		lblTimeout.Text = LocalizedResourceMgr.GetString("MakeCallConfigurationForm.lblTimeout.Text");
		okButton.Text = LocalizedResourceMgr.GetString("MakeCallConfigurationForm.okButton.Text");
		cancelButton.Text = LocalizedResourceMgr.GetString("MakeCallConfigurationForm.cancelButton.Text");
	}

	private void TxtOrigin_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtOrigin.Text))
		{
			errorProvider.SetError(originExpressionButton, LocalizedResourceMgr.GetString("MakeCallConfigurationForm.Error.OriginIsMandatory"));
		}
		else if (!AbsArgument.BuildArgument(validVariables, txtOrigin.Text).IsSafeExpression())
		{
			errorProvider.SetError(originExpressionButton, LocalizedResourceMgr.GetString("MakeCallConfigurationForm.Error.OriginIsInvalid"));
		}
		else
		{
			errorProvider.SetError(originExpressionButton, string.Empty);
		}
	}

	private void TxtDestination_Validating(object sender, CancelEventArgs e)
	{
		if (string.IsNullOrEmpty(txtDestination.Text))
		{
			errorProvider.SetError(destinationExpressionButton, LocalizedResourceMgr.GetString("MakeCallConfigurationForm.Error.DestinationIsMandatory"));
		}
		else if (!AbsArgument.BuildArgument(validVariables, txtDestination.Text).IsSafeExpression())
		{
			errorProvider.SetError(destinationExpressionButton, LocalizedResourceMgr.GetString("MakeCallConfigurationForm.Error.DestinationIsInvalid"));
		}
		else
		{
			errorProvider.SetError(destinationExpressionButton, string.Empty);
		}
	}

	private void OriginExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(makeCallComponent);
		expressionEditorForm.Expression = txtOrigin.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtOrigin.Text = expressionEditorForm.Expression;
			TxtOrigin_Validating(txtOrigin, new CancelEventArgs());
		}
	}

	private void DestinationExpressionButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(makeCallComponent);
		expressionEditorForm.Expression = txtDestination.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtDestination.Text = expressionEditorForm.Expression;
			TxtDestination_Validating(txtDestination, new CancelEventArgs());
		}
	}

	private void OkButton_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void MakeCallConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		makeCallComponent.ShowHelp();
	}

	private void MakeCallConfigurationForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		makeCallComponent.ShowHelp();
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
		this.lblDestination = new System.Windows.Forms.Label();
		this.txtDestination = new System.Windows.Forms.TextBox();
		this.cancelButton = new System.Windows.Forms.Button();
		this.okButton = new System.Windows.Forms.Button();
		this.destinationExpressionButton = new System.Windows.Forms.Button();
		this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
		this.originExpressionButton = new System.Windows.Forms.Button();
		this.txtOrigin = new System.Windows.Forms.TextBox();
		this.lblOrigin = new System.Windows.Forms.Label();
		this.lblTimeout = new System.Windows.Forms.Label();
		this.txtTimeout = new System.Windows.Forms.MaskedTextBox();
		((System.ComponentModel.ISupportInitialize)this.errorProvider).BeginInit();
		base.SuspendLayout();
		this.lblDestination.AutoSize = true;
		this.lblDestination.Location = new System.Drawing.Point(16, 50);
		this.lblDestination.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblDestination.Name = "lblDestination";
		this.lblDestination.Size = new System.Drawing.Size(25, 17);
		this.lblDestination.TabIndex = 3;
		this.lblDestination.Text = "To";
		this.txtDestination.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtDestination.Location = new System.Drawing.Point(126, 47);
		this.txtDestination.Margin = new System.Windows.Forms.Padding(4);
		this.txtDestination.Name = "txtDestination";
		this.txtDestination.Size = new System.Drawing.Size(423, 22);
		this.txtDestination.TabIndex = 4;
		this.txtDestination.Validating += new System.ComponentModel.CancelEventHandler(TxtDestination_Validating);
		this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cancelButton.Location = new System.Drawing.Point(497, 119);
		this.cancelButton.Margin = new System.Windows.Forms.Padding(4);
		this.cancelButton.Name = "cancelButton";
		this.cancelButton.Size = new System.Drawing.Size(100, 28);
		this.cancelButton.TabIndex = 9;
		this.cancelButton.Text = "&Cancel";
		this.cancelButton.UseVisualStyleBackColor = true;
		this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.okButton.Location = new System.Drawing.Point(389, 119);
		this.okButton.Margin = new System.Windows.Forms.Padding(4);
		this.okButton.Name = "okButton";
		this.okButton.Size = new System.Drawing.Size(100, 28);
		this.okButton.TabIndex = 8;
		this.okButton.Text = "&OK";
		this.okButton.UseVisualStyleBackColor = true;
		this.okButton.Click += new System.EventHandler(OkButton_Click);
		this.destinationExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.destinationExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.destinationExpressionButton.Location = new System.Drawing.Point(559, 44);
		this.destinationExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.destinationExpressionButton.Name = "destinationExpressionButton";
		this.destinationExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.destinationExpressionButton.TabIndex = 5;
		this.destinationExpressionButton.UseVisualStyleBackColor = true;
		this.destinationExpressionButton.Click += new System.EventHandler(DestinationExpressionButton_Click);
		this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
		this.errorProvider.ContainerControl = this;
		this.originExpressionButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.originExpressionButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.originExpressionButton.Location = new System.Drawing.Point(559, 12);
		this.originExpressionButton.Margin = new System.Windows.Forms.Padding(4);
		this.originExpressionButton.Name = "originExpressionButton";
		this.originExpressionButton.Size = new System.Drawing.Size(39, 28);
		this.originExpressionButton.TabIndex = 2;
		this.originExpressionButton.UseVisualStyleBackColor = true;
		this.originExpressionButton.Click += new System.EventHandler(OriginExpressionButton_Click);
		this.txtOrigin.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtOrigin.Location = new System.Drawing.Point(126, 15);
		this.txtOrigin.Margin = new System.Windows.Forms.Padding(4);
		this.txtOrigin.Name = "txtOrigin";
		this.txtOrigin.Size = new System.Drawing.Size(423, 22);
		this.txtOrigin.TabIndex = 1;
		this.txtOrigin.Validating += new System.ComponentModel.CancelEventHandler(TxtOrigin_Validating);
		this.lblOrigin.AutoSize = true;
		this.lblOrigin.Location = new System.Drawing.Point(16, 18);
		this.lblOrigin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblOrigin.Name = "lblOrigin";
		this.lblOrigin.Size = new System.Drawing.Size(99, 17);
		this.lblOrigin.TabIndex = 0;
		this.lblOrigin.Text = "Make call from";
		this.lblTimeout.AutoSize = true;
		this.lblTimeout.Location = new System.Drawing.Point(16, 80);
		this.lblTimeout.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblTimeout.Name = "lblTimeout";
		this.lblTimeout.Size = new System.Drawing.Size(102, 17);
		this.lblTimeout.TabIndex = 6;
		this.lblTimeout.Text = "Timeout (secs)";
		this.txtTimeout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtTimeout.HidePromptOnLeave = true;
		this.txtTimeout.Location = new System.Drawing.Point(126, 77);
		this.txtTimeout.Margin = new System.Windows.Forms.Padding(4);
		this.txtTimeout.Mask = "999";
		this.txtTimeout.Name = "txtTimeout";
		this.txtTimeout.Size = new System.Drawing.Size(423, 22);
		this.txtTimeout.TabIndex = 7;
		base.AcceptButton = this.okButton;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.cancelButton;
		base.ClientSize = new System.Drawing.Size(619, 152);
		base.Controls.Add(this.lblTimeout);
		base.Controls.Add(this.txtTimeout);
		base.Controls.Add(this.originExpressionButton);
		base.Controls.Add(this.txtOrigin);
		base.Controls.Add(this.lblOrigin);
		base.Controls.Add(this.destinationExpressionButton);
		base.Controls.Add(this.cancelButton);
		base.Controls.Add(this.okButton);
		base.Controls.Add(this.txtDestination);
		base.Controls.Add(this.lblDestination);
		base.HelpButton = true;
		base.Icon = TCX.CFD.Properties.Resources.ApplicationTitleBar;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(637, 199);
		base.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(637, 199);
		base.Name = "MakeCallConfigurationForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Make Call";
		base.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(MakeCallConfigurationForm_HelpButtonClicked);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(MakeCallConfigurationForm_HelpRequested);
		((System.ComponentModel.ISupportInitialize)this.errorProvider).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
