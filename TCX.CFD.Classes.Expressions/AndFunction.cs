using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TCX.CFD.Classes.Expressions;

public class AndFunction : AbsFunction
{
	protected override string WrapArgument(int index, string argument)
	{
		return $"Convert.ToBoolean({argument})";
	}

	public AndFunction(List<string> validVariables, string name, List<string> arguments)
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
		return LocalizedResourceMgr.GetString("ExpressionEditorForm.AndFunction.ExpressionHelpText");
	}

	public override object Evaluate(IList variableValues)
	{
		List<bool> list = new List<bool>(base.ArgumentList.Count);
		foreach (AbsArgument argument in base.ArgumentList)
		{
			list.Add(Convert.ToBoolean(argument.Evaluate(variableValues)));
		}
		return list.All((bool x) => x);
	}
}
