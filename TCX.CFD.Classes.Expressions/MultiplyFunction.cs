using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TCX.CFD.Classes.Expressions;

public class MultiplyFunction : AbsFunction
{
	protected override string WrapArgument(int index, string argument)
	{
		return $"Convert.ToInt32({argument})";
	}

	public MultiplyFunction(List<string> validVariables, string name, List<string> arguments)
		: base(validVariables, name, arguments)
	{
	}

	public override bool HasFixedArguments()
	{
		return false;
	}

	public override int GetFixedArgumentCount()
	{
		return 0;
	}

	public override int GetMinArgumentCount()
	{
		return 2;
	}

	public override string GetHelpText()
	{
		return LocalizedResourceMgr.GetString("ExpressionEditorForm.MultiplyFunction.ExpressionHelpText");
	}

	public override object Evaluate(IList variableValues)
	{
		List<int> list = new List<int>(base.ArgumentList.Count);
		foreach (AbsArgument argument in base.ArgumentList)
		{
			list.Add(Convert.ToInt32(argument.Evaluate(variableValues)));
		}
		return list.Aggregate((int a, int b) => a * b);
	}
}
