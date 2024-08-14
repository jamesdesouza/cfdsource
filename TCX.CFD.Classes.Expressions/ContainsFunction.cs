using System;
using System.Collections;
using System.Collections.Generic;

namespace TCX.CFD.Classes.Expressions;

public class ContainsFunction : AbsFunction
{
	protected override string WrapArgument(int index, string argument)
	{
		return $"Convert.ToString({argument})";
	}

	public ContainsFunction(List<string> validVariables, string name, List<string> arguments)
		: base(validVariables, name, arguments)
	{
	}

	public override bool HasFixedArguments()
	{
		return true;
	}

	public override int GetFixedArgumentCount()
	{
		return 2;
	}

	public override int GetMinArgumentCount()
	{
		return 2;
	}

	public override string GetHelpText()
	{
		return LocalizedResourceMgr.GetString("ExpressionEditorForm.ContainsFunction.ExpressionHelpText");
	}

	public override object Evaluate(IList variableValues)
	{
		string text = Convert.ToString(base.ArgumentList[0].Evaluate(variableValues));
		string value = Convert.ToString(base.ArgumentList[1].Evaluate(variableValues));
		return text.Contains(value);
	}
}
