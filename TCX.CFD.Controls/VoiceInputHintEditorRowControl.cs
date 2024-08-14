using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Forms;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class VoiceInputHintEditorRowControl : UserControl
{
	private string languageCode;

	private IContainer components;

	private CheckBox chkSelection;

	private Label lblValue;

	private TextBox txtValue;

	private Button editButton;

	public bool IsChecked
	{
		get
		{
			return chkSelection.Checked;
		}
		set
		{
			chkSelection.Checked = value;
		}
	}

	public string Hint => txtValue.Text;

	public event EventHandler CheckedChanged;

	public VoiceInputHintEditorRowControl(string languageCode, string hint)
	{
		InitializeComponent();
		lblValue.Text = LocalizedResourceMgr.GetString("VoiceInputHintEditorRowControl.lblValue.Text");
		this.languageCode = languageCode;
		txtValue.Text = hint;
	}

	private void ChkSelection_CheckedChanged(object sender, EventArgs e)
	{
		this.CheckedChanged?.Invoke(this, e);
	}

	private void TxtDictionaryEntry_Enter(object sender, EventArgs e)
	{
		txtValue.SelectAll();
	}

	private void EditButton_Click(object sender, EventArgs e)
	{
		VoiceInputHintBuilderForm voiceInputHintBuilderForm = new VoiceInputHintBuilderForm(languageCode);
		voiceInputHintBuilderForm.Hint = txtValue.Text;
		if (voiceInputHintBuilderForm.ShowDialog() == DialogResult.OK)
		{
			txtValue.Text = voiceInputHintBuilderForm.Hint;
		}
	}

	public void UpdateLanguage(string languageCode)
	{
		this.languageCode = languageCode;
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
		this.chkSelection = new System.Windows.Forms.CheckBox();
		this.lblValue = new System.Windows.Forms.Label();
		this.txtValue = new System.Windows.Forms.TextBox();
		this.editButton = new System.Windows.Forms.Button();
		base.SuspendLayout();
		this.chkSelection.AutoSize = true;
		this.chkSelection.Location = new System.Drawing.Point(4, 22);
		this.chkSelection.Margin = new System.Windows.Forms.Padding(4);
		this.chkSelection.Name = "chkSelection";
		this.chkSelection.Size = new System.Drawing.Size(18, 17);
		this.chkSelection.TabIndex = 0;
		this.chkSelection.UseVisualStyleBackColor = true;
		this.chkSelection.CheckedChanged += new System.EventHandler(ChkSelection_CheckedChanged);
		this.lblValue.AutoSize = true;
		this.lblValue.Location = new System.Drawing.Point(34, 9);
		this.lblValue.Name = "lblValue";
		this.lblValue.Size = new System.Drawing.Size(44, 17);
		this.lblValue.TabIndex = 2;
		this.lblValue.Text = "Value";
		this.txtValue.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtValue.Location = new System.Drawing.Point(37, 29);
		this.txtValue.MaxLength = 150;
		this.txtValue.Name = "txtValue";
		this.txtValue.Size = new System.Drawing.Size(693, 22);
		this.txtValue.TabIndex = 3;
		this.txtValue.Enter += new System.EventHandler(TxtDictionaryEntry_Enter);
		this.editButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.editButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.editButton.Location = new System.Drawing.Point(737, 26);
		this.editButton.Margin = new System.Windows.Forms.Padding(4);
		this.editButton.Name = "editButton";
		this.editButton.Size = new System.Drawing.Size(39, 28);
		this.editButton.TabIndex = 7;
		this.editButton.UseVisualStyleBackColor = true;
		this.editButton.Click += new System.EventHandler(EditButton_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.editButton);
		base.Controls.Add(this.txtValue);
		base.Controls.Add(this.lblValue);
		base.Controls.Add(this.chkSelection);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "VoiceInputHintEditorRowControl";
		base.Size = new System.Drawing.Size(780, 61);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
