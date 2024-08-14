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

public class TextToSpeechAudioPromptEditorRowControl : AbsPromptEditorRowControl
{
	private readonly IVadActivity activity;

	private readonly TextToSpeechAudioPrompt prompt;

	private readonly ProjectObject projectObject;

	private IContainer components;

	private ComboBox comboVoices;

	private Label lblVoice;

	private Label lblType;

	private ComboBox comboTypes;

	private Label lblExpression;

	private Button editButton;

	private TextBox txtExpression;

	private void FillTypesCombo(TextToSpeechFormats format)
	{
		comboTypes.Items.Clear();
		ComboBox.ObjectCollection ıtems = comboTypes.Items;
		object[] items = new string[2] { "Text", "SSML" };
		ıtems.AddRange(items);
		comboTypes.SelectedIndex = ((format != 0) ? 1 : 0);
	}

	public TextToSpeechAudioPromptEditorRowControl(IVadActivity activity, TextToSpeechAudioPrompt prompt)
	{
		InitializeComponent();
		this.activity = activity;
		this.prompt = prompt;
		projectObject = activity.GetRootFlow().FileObject.GetProjectObject();
		lblVoice.Text = LocalizedResourceMgr.GetString("TextToSpeechAudioPromptEditorRowControl.lblVoice.Text");
		lblType.Text = LocalizedResourceMgr.GetString("TextToSpeechAudioPromptEditorRowControl.lblType.Text");
		lblExpression.Text = LocalizedResourceMgr.GetString("TextToSpeechAudioPromptEditorRowControl.lblExpression.Text");
		if (projectObject.OnlineServices.TextToSpeechEngine == TextToSpeechEngines.None)
		{
			comboVoices.Items.Add(LocalizedResourceMgr.GetString("TextToSpeechAudioPromptEditorRowControl.ConfigureOnlineServices.Text"));
		}
		else
		{
			TextToSpeechHelper.FillVoicesCombo(comboVoices, prompt.VoiceName, prompt.VoiceType, projectObject.OnlineServices.TextToSpeechEngine);
		}
		FillTypesCombo(prompt.Format);
		txtExpression.Text = prompt.Text;
	}

	private void ComboVoices_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (projectObject.OnlineServices.TextToSpeechEngine != 0 || comboVoices.SelectedIndex != 0)
		{
			return;
		}
		OnlineServicesConfigurationForm onlineServicesConfigurationForm = new OnlineServicesConfigurationForm
		{
			OnlineServices = projectObject.OnlineServices
		};
		if (onlineServicesConfigurationForm.ShowDialog() == DialogResult.OK)
		{
			projectObject.OnlineServices = onlineServicesConfigurationForm.OnlineServices;
			if (projectObject.OnlineServices.TextToSpeechEngine == TextToSpeechEngines.None)
			{
				comboVoices.SelectedIndex = -1;
			}
			else
			{
				TextToSpeechHelper.FillVoicesCombo(comboVoices, prompt.VoiceName, prompt.VoiceType, projectObject.OnlineServices.TextToSpeechEngine);
			}
		}
		else
		{
			comboVoices.SelectedIndex = -1;
		}
	}

	private void TxtExpression_Enter(object sender, EventArgs e)
	{
		txtExpression.SelectAll();
	}

	private void EditButton_Click(object sender, EventArgs e)
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
		string text = ((comboVoices.SelectedIndex == -1) ? comboVoices.Text : (comboVoices.SelectedItem as string));
		TextToSpeechVoiceTypes voiceType = (text.Contains(" - Neural") ? TextToSpeechVoiceTypes.Neural : (text.Contains("Wavenet") ? TextToSpeechVoiceTypes.Wavenet : TextToSpeechVoiceTypes.Standard));
		int num = text.IndexOf('(');
		if (num != -1)
		{
			text = text.Substring(0, num).Trim();
		}
		prompt.VoiceName = text;
		prompt.VoiceType = voiceType;
		prompt.Format = ((comboTypes.SelectedIndex != 0) ? TextToSpeechFormats.SSML : TextToSpeechFormats.Text);
		prompt.Text = txtExpression.Text;
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
		this.comboVoices = new System.Windows.Forms.ComboBox();
		this.lblVoice = new System.Windows.Forms.Label();
		this.lblType = new System.Windows.Forms.Label();
		this.comboTypes = new System.Windows.Forms.ComboBox();
		this.lblExpression = new System.Windows.Forms.Label();
		this.editButton = new System.Windows.Forms.Button();
		this.txtExpression = new System.Windows.Forms.TextBox();
		base.SuspendLayout();
		this.comboVoices.DropDownWidth = 300;
		this.comboVoices.FormattingEnabled = true;
		this.comboVoices.Location = new System.Drawing.Point(4, 28);
		this.comboVoices.Margin = new System.Windows.Forms.Padding(4);
		this.comboVoices.Name = "comboVoices";
		this.comboVoices.Size = new System.Drawing.Size(143, 24);
		this.comboVoices.TabIndex = 1;
		this.comboVoices.SelectedIndexChanged += new System.EventHandler(ComboVoices_SelectedIndexChanged);
		this.lblVoice.AutoSize = true;
		this.lblVoice.Location = new System.Drawing.Point(4, 9);
		this.lblVoice.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblVoice.Name = "lblVoice";
		this.lblVoice.Size = new System.Drawing.Size(43, 17);
		this.lblVoice.TabIndex = 0;
		this.lblVoice.Text = "Voice";
		this.lblType.AutoSize = true;
		this.lblType.Location = new System.Drawing.Point(156, 9);
		this.lblType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblType.Name = "lblType";
		this.lblType.Size = new System.Drawing.Size(40, 17);
		this.lblType.TabIndex = 2;
		this.lblType.Text = "Type";
		this.comboTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboTypes.FormattingEnabled = true;
		this.comboTypes.Location = new System.Drawing.Point(156, 28);
		this.comboTypes.Margin = new System.Windows.Forms.Padding(4);
		this.comboTypes.Name = "comboTypes";
		this.comboTypes.Size = new System.Drawing.Size(88, 24);
		this.comboTypes.TabIndex = 3;
		this.lblExpression.AutoSize = true;
		this.lblExpression.Location = new System.Drawing.Point(253, 9);
		this.lblExpression.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblExpression.Name = "lblExpression";
		this.lblExpression.Size = new System.Drawing.Size(35, 17);
		this.lblExpression.TabIndex = 4;
		this.lblExpression.Text = "Text";
		this.editButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.editButton.Image = TCX.CFD.Properties.Resources.ExpressionEditor;
		this.editButton.Location = new System.Drawing.Point(515, 27);
		this.editButton.Margin = new System.Windows.Forms.Padding(4);
		this.editButton.Name = "editButton";
		this.editButton.Size = new System.Drawing.Size(39, 28);
		this.editButton.TabIndex = 6;
		this.editButton.UseVisualStyleBackColor = true;
		this.editButton.Click += new System.EventHandler(EditButton_Click);
		this.txtExpression.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtExpression.Location = new System.Drawing.Point(253, 30);
		this.txtExpression.Margin = new System.Windows.Forms.Padding(4);
		this.txtExpression.Name = "txtExpression";
		this.txtExpression.Size = new System.Drawing.Size(252, 22);
		this.txtExpression.TabIndex = 5;
		this.txtExpression.Enter += new System.EventHandler(TxtExpression_Enter);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.editButton);
		base.Controls.Add(this.txtExpression);
		base.Controls.Add(this.lblExpression);
		base.Controls.Add(this.comboTypes);
		base.Controls.Add(this.lblType);
		base.Controls.Add(this.lblVoice);
		base.Controls.Add(this.comboVoices);
		base.Margin = new System.Windows.Forms.Padding(5);
		base.Name = "TextToSpeechAudioPromptEditorRowControl";
		base.Size = new System.Drawing.Size(557, 60);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
