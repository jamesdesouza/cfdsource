using System;
using System.Collections;
using System.Collections.Generic;

namespace TCX.CFD.Classes.Expressions;

public class ReplaceFunction : AbsFunction
{
	protected override string WrapArgument(int index, string argument)
	{
		return $"Convert.ToString({argument})";
	}

	public ReplaceFunction(List<string> validVariables, string name, List<string> arguments)
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
		return LocalizedResourceMgr.GetString("ExpressionEditorForm.ReplaceFunction.ExpressionHelpText");
	}

	public override object Evaluate(IList variableValues)
	{
		string text = Convert.ToString(base.ArgumentList[0].Evaluate(variableValues));
		string oldValue = Convert.ToString(base.ArgumentList[1].Evaluate(variableValues));
		string newValue = Convert.ToString(base.ArgumentList[2].Evaluate(variableValues));
		return text?.Replace(oldValue, newValue);
	}
}
