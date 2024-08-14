using System;
using System.Text;

namespace TCX.CFD.Classes;

public static class NameHelper
{
	public static string SanitizeName(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentException("Name is invalid", "name");
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (char.IsDigit(name[0]))
		{
			stringBuilder.Append('_');
		}
		foreach (char c in name)
		{
			stringBuilder.Append(char.IsLetterOrDigit(c) ? c : '_');
		}
		return stringBuilder.ToString();
	}

	public static bool IsValidName(string name)
	{
		if (name.Length > 50)
		{
			return false;
		}
		foreach (char c in name)
		{
			if (c != ' ' && c != '_' && c != '-' && !char.IsLetterOrDigit(c))
			{
				return false;
			}
		}
		return true;
	}
}
