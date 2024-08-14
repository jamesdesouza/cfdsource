using System.Collections.Generic;
using System.Windows.Forms;

namespace TCX.CFD.Classes.Expressions;

public abstract class AbsVoiceInputHint
{
	public abstract bool IsConstantValue();

	public abstract bool IsToken();

	public abstract string GetHelpText();

	public abstract void SetHelpText(RichTextBox txtHintHelp);

	public abstract string GetText();

	public abstract AbsVoiceInputHint Clone();

	public abstract UserControl CreateEditorControl();

	private static List<AbsVoiceInputHint> ProcessPartialResults(List<AbsVoiceInputHint> partialResults, VoiceInputHintToken languageToken)
	{
		List<AbsVoiceInputHint> list = new List<AbsVoiceInputHint>();
		foreach (AbsVoiceInputHint partialResult in partialResults)
		{
			if (partialResult.IsToken())
			{
				list.Add(partialResult);
				continue;
			}
			string text = ((VoiceInputHintConstantValue)partialResult).GetText();
			string text2 = languageToken.GetText();
			int num = 0;
			int num2;
			while ((num2 = text.IndexOf(text2, num)) != -1)
			{
				if (num2 > num)
				{
					string text3 = text.Substring(num, num2 - num).Trim();
					if (!string.IsNullOrEmpty(text3))
					{
						list.Add(new VoiceInputHintConstantValue(text3));
					}
				}
				list.Add(languageToken.Clone());
				num = num2 + text2.Length;
			}
			if (num < text.Length)
			{
				string text4 = text.Substring(num).Trim();
				if (!string.IsNullOrEmpty(text4))
				{
					list.Add(new VoiceInputHintConstantValue(text4));
				}
			}
		}
		return list;
	}

	public static List<AbsVoiceInputHint> BuildHints(string languageCode, string hint)
	{
		List<AbsVoiceInputHint> list = new List<AbsVoiceInputHint>
		{
			new VoiceInputHintConstantValue(hint)
		};
		foreach (VoiceInputHintToken item in VoiceInputHintsHelper.GetTokensForLanguage(languageCode))
		{
			list = ProcessPartialResults(list, item);
		}
		return list;
	}
}
