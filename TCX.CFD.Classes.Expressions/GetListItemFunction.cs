using System;
using System.Collections;
using System.Collections.Generic;

namespace TCX.CFD.Classes.Expressions;

public class GetListItemFunction : AbsFunction
{
	protected override string WrapArgument(int index, string argument)
	{
		if (index == 0)
		{
			if (argument.StartsWith("variableMap"))
			{
				return $"new List<object>(((System.Collections.IList){argument}).Cast<object>())";
			}
			return $"new List<object>((object[]){argument}.ToArray())";
		}
		return $"Convert.ToInt32({argument})";
	}

	public GetListItemFunction(List<string> validVariables, string name, List<string> arguments)
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
		return LocalizedResourceMgr.GetString("ExpressionEditorForm.GetListItemFunction.ExpressionHelpText");
	}

	public override object Evaluate(IList variableValues)
	{
		List<object> obj = base.ArgumentList[0].Evaluate(variableValues) as List<object>;
		int index = Convert.ToInt32(base.ArgumentList[1].Evaluate(variableValues));
		return obj[index];
	}
}
