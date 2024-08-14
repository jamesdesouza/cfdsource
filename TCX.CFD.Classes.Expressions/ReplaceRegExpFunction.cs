using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TCX.CFD.Classes.Expressions;

public class ReplaceRegExpFunction : AbsFunction
{
	protected override string WrapArgument(int index, string argument)
	{
		return $"Convert.ToString({argument})";
	}

	public ReplaceRegExpFunction(List<string> validVariables, string name, List<string> arguments)
		: base(validVariables, name, arguments)
	{
	}

	public override bool HasFixedArguments()
	{
		return true;
	}

	public override int GetFixedArgumentCount()
	{
		return 3;
	}

	public override int GetMinArgumentCount()
	{
		return 3;
	}

	public override string GetHelpText()
	{
		return LocalizedResourceMgr.GetString("ExpressionEditorForm.ReplaceRegExpFunction.ExpressionHelpText");
	}

	public override object Evaluate(IList variableValues)
	{
		string input = Convert.ToString(base.ArgumentList[0].Evaluate(variableValues));
		string pattern = Convert.ToString(base.ArgumentList[1].Evaluate(variableValues));
		string replacement = Convert.ToString(base.ArgumentList[2].Evaluate(variableValues));
		return new Regex(pattern).Replace(input, replacement);
	}
}
