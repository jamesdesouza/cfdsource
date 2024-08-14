using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;
using TCX.CFD.Forms;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class NumberPromptEditorRowControl : AbsPromptEditorRowControl
{
	private readonly IVadActivity activity;

	private readonly NumberPrompt prompt;

	private IContainer components;

	private ComboBox comboFormat;

	private Label lblNumber;

	private Button editButton;

	private TextBox txtNumber;

	private Label lblFormat;

	private void FillFormatCombo(NumberPromptFormats format)
	{
		comboFormat.Items.Clear();
		ComboBox.ObjectCollection ıtems = comboFormat.Items;
		object[] items = new string[3] { "Full Number", "Grouped by Two", "One by One" };
		ıtems.AddRange(items);
		comboFormat.SelectedIndex = format switch
		{
			NumberPromptFormats.GroupedByTwo => 1, 
			NumberPromptFormats.FullNumber => 0, 
			_ => 2, 
		};
	}

	public NumberPromptEditorRowControl(IVadActivity activity, NumberPrompt prompt)
	{
		InitializeComponent();
		this.activity = activity;
		this.prompt = prompt;
		lblFormat.Text = LocalizedResourceMgr.GetString("NumberPromptEditorRowControl.lblFormat.Text");
		lblNumber.Text = LocalizedResourceMgr.GetString("NumberPromptEditorRowControl.lblNumber.Text");
		FillFormatCombo(prompt.Format);
		txtNumber.Text = prompt.Number;
	}

	private void TxtNumber_Enter(object sender, EventArgs e)
	{
		txtNumber.SelectAll();
	}

	private void EditButton_Click(object sender, EventArgs e)
	{
		ExpressionEditorForm expressionEditorForm = new ExpressionEditorForm(activity);
		expressionEditorForm.Expression = txtNumber.Text;
		if (expressionEditorForm.ShowDialog() == DialogResult.OK)
		{
			txtNumber.Text = expressionEditorForm.Expression;
		}
	}

	public override Prompt Save()
	{
		prompt.Format = ((comboFormat.SelectedIndex != 0) ? ((comboFormat.SelectedIndex == 1) ? NumberPromptFormats.GroupedByTwo : NumberPromptFormats.OneByOne) : NumberPromptFormats.FullNumber);
		prompt.Number = txtNumber.Text;
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
		this.comboFormat = new System.Windows.Forms.ComboBox();
		this.lblNumber = new System.Windows.Forms.Label();
		this.editButton = new System.Windows.Forms.Button();
		this.txtNumber = new System.Windows.Forms.TextBox();
		this.lblFormat = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.comboFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboFormat.FormattingEnabled = true;
		this.comboFormat.Location = new System.Drawing.Point(4, 28);
		this.comboFormat.Margin = new System.Windows.Forms.Padding(4);
		this.comboFormat.Name = "comboFormat";
		this.comboFormat.Size = new System.Drawing.Size(155, 24);
		this.comboFormat.TabIndex = 1;
		this.lblNumber.AutoSize = true;
		this.lblNumber.Location = new System.Drawing.Point(164, 9);
		this.lblNumber.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblNumber.Name = "lblNumber";
		this.lblNumber.Size = new System.Drawing.Size(58, 17);
		this.lblNumber.TabIndex = 2;
		this.lblNumber.Text = "Number";
		this.editButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.editButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.editButton.Location = new System.Drawing.Point(515, 27);
		this.editButton.Margin = new System.Windows.Forms.Padding(4);
		this.editButton.Name = "editButton";
		this.editButton.Size = new System.Drawing.Size(39, 28);
		this.editButton.TabIndex = 4;
		this.editButton.UseVisualStyleBackColor = true;
		this.editButton.Click += new System.EventHandler(EditButton_Click);
		this.txtNumber.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtNumber.Location = new System.Drawing.Point(167, 30);
		this.txtNumber.Margin = new System.Windows.Forms.Padding(4);
		this.txtNumber.Name = "txtNumber";
		this.txtNumber.Size = new System.Drawing.Size(340, 22);
		this.txtNumber.TabIndex = 3;
		this.txtNumber.Enter += new System.EventHandler(TxtNumber_Enter);
		this.lblFormat.AutoSize = true;
		this.lblFormat.Location = new System.Drawing.Point(4, 9);
		this.lblFormat.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblFormat.Name = "lblFormat";
		this.lblFormat.Size = new System.Drawing.Size(52, 17);
		this.lblFormat.TabIndex = 0;
		this.lblFormat.Text = "Format";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.editButton);
		base.Controls.Add(this.txtNumber);
		base.Controls.Add(this.lblNumber);
		base.Controls.Add(this.comboFormat);
		base.Controls.Add(this.lblFormat);
		base.Margin = new System.Windows.Forms.Padding(5);
		base.Name = "NumberPromptEditorRowControl";
		base.Size = new System.Drawing.Size(557, 60);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
