using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.FileSystem;
using TCX.CFD.Forms;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class RecordedAudioPromptEditorRowControl : AbsPromptEditorRowControl
{
	private IVadActivity activity;

	private RecordedAudioPrompt prompt;

	private ProjectObject projectObject;

	private IContainer components;

	private Label lblAudioIdExpression;

	private TextBox txtExpression;

	private Button editButton;

	public RecordedAudioPromptEditorRowControl(IVadActivity activity, RecordedAudioPrompt prompt)
	{
		InitializeComponent();
		this.activity = activity;
		this.prompt = prompt;
		projectObject = activity.GetRootFlow().FileObject.GetProjectObject();
		lblAudioIdExpression.Text = LocalizedResourceMgr.GetString("RecordedAudioPromptEditorRowControl.lblAudioIdExpression.Text");
		txtExpression.Text = prompt.AudioId;
	}

	private void txtExpression_Enter(object sender, EventArgs e)
	{
		txtExpression.SelectAll();
	}

	private void editButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(activity);
		expressionEditorForm.Expression = txtExpression.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtExpression.Text = expressionEditorForm.Expression;
		}
	}

	public override Prompt Save()
	{
		prompt.AudioId = txtExpression.Text;
		return prompt;
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
		this.editButton = new System.Windows.Forms.Button();
		this.txtExpression = new System.Windows.Forms.TextBox();
		this.lblAudioIdExpression = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.editButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.editButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.editButton.Location = new System.Drawing.Point(386, 21);
		this.editButton.Name = "editButton";
		this.editButton.Size = new System.Drawing.Size(29, 23);
		this.editButton.TabIndex = 2;
		this.editButton.UseVisualStyleBackColor = true;
		this.editButton.Click += new System.EventHandler(editButton_Click);
		this.txtExpression.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtExpression.Location = new System.Drawing.Point(6, 23);
		this.txtExpression.Name = "txtExpression";
		this.txtExpression.Size = new System.Drawing.Size(374, 20);
		this.txtExpression.TabIndex = 1;
		this.lblAudioIdExpression.AutoSize = true;
		this.lblAudioIdExpression.Location = new System.Drawing.Point(3, 7);
		this.lblAudioIdExpression.Name = "lblAudioIdExpression";
		this.lblAudioIdExpression.Size = new System.Drawing.Size(102, 13);
		this.lblAudioIdExpression.TabIndex = 0;
		this.lblAudioIdExpression.Text = "Audio ID Expression";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.editButton);
		base.Controls.Add(this.txtExpression);
		base.Controls.Add(this.lblAudioIdExpression);
		base.Name = "RecordedAudioPromptEditorRowControl";
		base.Size = new System.Drawing.Size(418, 49);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
