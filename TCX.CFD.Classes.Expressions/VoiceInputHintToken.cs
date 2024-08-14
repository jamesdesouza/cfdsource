using System;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Controls;

namespace TCX.CFD.Classes.Expressions;

public class VoiceInputHintToken : AbsVoiceInputHint
{
	private readonly string token;

	private readonly string description;

	private readonly string spokenExample;

	private readonly string writtenExample;

	public VoiceInputHintToken(string token, string description, string spokenExample, string writtenExample)
	{
		this.token = token;
		this.description = description;
		this.spokenExample = spokenExample;
		this.writtenExample = writtenExample;
	}

	public override bool IsConstantValue()
	{
		return false;
	}

	public override bool IsToken()
	{
		return true;
	}

	public override string GetHelpText()
	{
		return token + Environment.NewLine + Environment.NewLine + description + Environment.NewLine + LocalizedResourceMgr.GetString("VoiceInputHintBuilderForm.TokenHint.SpokenExample") + " " + spokenExample + Environment.NewLine + LocalizedResourceMgr.GetString("VoiceInputHintBuilderForm.TokenHint.WrittenExample") + " " + writtenExample;
	}

	public override void SetHelpText(RichTextBox txtHintHelp)
	{
		txtHintHelp.SelectionFont = new Font(txtHintHelp.Font, FontStyle.Bold);
		txtHintHelp.AppendText(token + Environment.NewLine + Environment.NewLine);
		txtHintHelp.SelectionFont = new Font(txtHintHelp.Font, FontStyle.Regular);
		txtHintHelp.AppendText(description + Environment.NewLine);
		txtHintHelp.SelectionFont = new Font(txtHintHelp.Font, FontStyle.Bold);
		txtHintHelp.AppendText(LocalizedResourceMgr.GetString("VoiceInputHintBuilderForm.TokenHint.SpokenExample"));
		txtHintHelp.SelectionFont = new Font(txtHintHelp.Font, FontStyle.Regular);
		txtHintHelp.AppendText(" " + spokenExample + Environment.NewLine);
		txtHintHelp.SelectionFont = new Font(txtHintHelp.Font, FontStyle.Bold);
		txtHintHelp.AppendText(LocalizedResourceMgr.GetString("VoiceInputHintBuilderForm.TokenHint.WrittenExample"));
		txtHintHelp.SelectionFont = new Font(txtHintHelp.Font, FontStyle.Regular);
		txtHintHelp.AppendText(" " + writtenExample);
	}

	public override string GetText()
	{
		return token;
	}

	public override AbsVoiceInputHint Clone()
	{
		return new VoiceInputHintToken(token, description, spokenExample, writtenExample);
	}

	public override UserControl CreateEditorControl()
	{
		return new VoiceInputHintBuilderTokenControl
		{
			Token = this
		};
	}
}
