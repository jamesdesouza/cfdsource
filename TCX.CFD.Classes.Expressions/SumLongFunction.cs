using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TCX.CFD.Classes.Expressions;

public class SumLongFunction : AbsFunction
{
	protected override string WrapArgument(int index, string argument)
	{
		return $"Convert.ToInt64({argument})";
	}

	public SumLongFunction(List<string> validVariables, string name, List<string> arguments)
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
		return LocalizedResourceMgr.GetString("ExpressionEditorForm.SumLongFunction.ExpressionHelpText");
	}

	public override object Evaluate(IList variableValues)
	{
		List<long> list = new List<long>(base.ArgumentList.Count);
		foreach (AbsArgument argument in base.ArgumentList)
		{
			list.Add(Convert.ToInt64(argument.Evaluate(variableValues)));
		}
		return list.Sum();
	}
}
