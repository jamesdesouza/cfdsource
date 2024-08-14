using System;
using System.Collections;
using System.Collections.Generic;

namespace TCX.CFD.Classes.Expressions;

public class GetTableCellValueFunction : AbsFunction
{
	protected override string WrapArgument(int index, string argument)
	{
		if (index != 0)
		{
			return $"Convert.ToInt32({argument})";
		}
		return argument;
	}

	public GetTableCellValueFunction(List<string> validVariables, string name, List<string> arguments)
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
		return LocalizedResourceMgr.GetString("ExpressionEditorForm.GetTableCellValueFunction.ExpressionHelpText");
	}

	public override object Evaluate(IList variableValues)
	{
		List<object[]> obj = base.ArgumentList[0].Evaluate(variableValues) as List<object[]>;
		int index = Convert.ToInt32(base.ArgumentList[1].Evaluate(variableValues));
		int num = Convert.ToInt32(base.ArgumentList[2].Evaluate(variableValues));
		return obj[index][num];
	}
}
