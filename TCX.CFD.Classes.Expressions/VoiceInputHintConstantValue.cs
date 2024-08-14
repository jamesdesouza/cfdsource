using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Controls;

namespace TCX.CFD.Classes.Expressions;

public class VoiceInputHintConstantValue : AbsVoiceInputHint
{
	private readonly string text;

	public VoiceInputHintConstantValue(string text)
	{
		this.text = text;
	}

	public override bool IsConstantValue()
	{
		return true;
	}

	public override bool IsToken()
	{
		return false;
	}

	public override string GetHelpText()
	{
		return LocalizedResourceMgr.GetString("VoiceInputHintBuilderForm.ConstantHint.HelpText");
	}

	public override void SetHelpText(RichTextBox txtHintHelp)
	{
		txtHintHelp.SelectionFont = new Font(txtHintHelp.Font, FontStyle.Regular);
		txtHintHelp.AppendText(LocalizedResourceMgr.GetString("VoiceInputHintBuilderForm.ConstantHint.HelpText"));
	}

	public override string GetText()
	{
		return text;
	}

	public override AbsVoiceInputHint Clone()
	{
		return new VoiceInputHintConstantValue(text);
	}

	public override UserControl CreateEditorControl()
	{
		return new VoiceInputHintBuilderConstantValueControl
		{
			ConstantValue = this
		};
	}
}
