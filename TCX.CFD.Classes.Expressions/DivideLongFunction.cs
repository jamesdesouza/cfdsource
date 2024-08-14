using System;
using System.Collections;
using System.Collections.Generic;

namespace TCX.CFD.Classes.Expressions;

public class DivideLongFunction : AbsFunction
{
	protected override string WrapArgument(int index, string argument)
	{
		return $"Convert.ToInt64({argument})";
	}

	public DivideLongFunction(List<string> validVariables, string name, List<string> arguments)
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
		return LocalizedResourceMgr.GetString("ExpressionEditorForm.DivideLongFunction.ExpressionHelpText");
	}

	public override object Evaluate(IList variableValues)
	{
		long num = Convert.ToInt64(base.ArgumentList[0].Evaluate(variableValues));
		long num2 = Convert.ToInt64(base.ArgumentList[1].Evaluate(variableValues));
		return num / num2;
	}
}
