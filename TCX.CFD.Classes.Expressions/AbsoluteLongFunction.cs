using System;
using System.Collections;
using System.Collections.Generic;

namespace TCX.CFD.Classes.Expressions;

public class AbsoluteLongFunction : AbsFunction
{
	protected override string WrapArgument(int index, string argument)
	{
		return $"Convert.ToInt64({argument})";
	}

	public AbsoluteLongFunction(List<string> validVariables, string name, List<string> arguments)
		: base(validVariables, name, arguments)
	{
	}

	public override bool HasFixedArguments()
	{
		return true;
	}

	public override int GetFixedArgumentCount()
	{
		return 1;
	}

	public override int GetMinArgumentCount()
	{
		return 1;
	}

	public override string GetHelpText()
	{
		return LocalizedResourceMgr.GetString("ExpressionEditorForm.AbsoluteLongFunction.ExpressionHelpText");
	}

	public override object Evaluate(IList variableValues)
	{
		long num = Convert.ToInt64(base.ArgumentList[0].Evaluate(variableValues));
		return (num < 0) ? (-num) : num;
	}
}
