using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.Compiler;

public static class ScriptCodeSnipetLoader
{
	public static string Load(List<string> extraCodeSnipets)
	{
		List<string> list = new List<string>();
		StringBuilder stringBuilder = new StringBuilder();
		foreach (string extraCodeSnipet in extraCodeSnipets)
		{
			ScriptCodeSnipet scriptCodeSnipet = new ScriptCodeSnipet(extraCodeSnipet);
			list.AddRange(scriptCodeSnipet.UsingStatements.Except(list));
			stringBuilder.Append(scriptCodeSnipet.CodeSnipet);
		}
		ScriptCodeSnipet scriptCodeSnipet2 = new ScriptCodeSnipet(Resources.Callflow_cs);
		list.AddRange(scriptCodeSnipet2.UsingStatements.Except(list));
		list.Sort();
		StringBuilder stringBuilder2 = new StringBuilder();
		foreach (string item in list)
		{
			stringBuilder2.AppendLine(item);
		}
		stringBuilder2.Append(scriptCodeSnipet2.CodeSnipet.Replace("[CHILDREN_CLASSES_PLACEHOLDER]", stringBuilder.ToString()));
		return stringBuilder2.ToString();
	}
}
