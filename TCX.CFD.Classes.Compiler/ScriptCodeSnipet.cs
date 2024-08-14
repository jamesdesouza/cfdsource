using System.Collections.Generic;

namespace TCX.CFD.Classes.Compiler;

public class ScriptCodeSnipet
{
	public List<string> UsingStatements { get; private set; } = new List<string>();


	public string CodeSnipet { get; private set; }

	public ScriptCodeSnipet(string script)
	{
		string[] array = script.Split('\n');
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			string text = array2[i].Trim();
			if (text.StartsWith("using "))
			{
				UsingStatements.Add(text);
				continue;
			}
			CodeSnipet = string.Join("\n", array, UsingStatements.Count, array.Length - UsingStatements.Count);
			break;
		}
	}
}
