using System;

namespace TCX.CFD.Classes.Debug;

public class VariableChangeDebugInfo
{
	private readonly string name = string.Empty;

	private readonly string value = string.Empty;

	public string Name => name;

	public string Value => value;

	public VariableChangeDebugInfo(string line)
	{
		int num = line.IndexOf('=');
		if (num > 0)
		{
			name = line.Substring(0, num);
			if (num < line.Length - 1)
			{
				value = line.Substring(num + 1);
			}
			return;
		}
		throw new ArgumentException(string.Format(LocalizedResourceMgr.GetString("DebugInformationValidation.MessageBox.Error.InvalidVariableChangeLine"), line));
	}
}
