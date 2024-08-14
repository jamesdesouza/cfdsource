using System;
using System.Collections;
using System.Collections.Generic;

namespace TCX.CFD.Classes.Expressions;

public class ToBooleanFunction : AbsFunction
{
	protected override string WrapArgument(int index, string argument)
	{
		return argument;
	}

	public ToBooleanFunction(List<string> validVariables, string name, List<string> arguments)
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
		return LocalizedResourceMgr.GetString("ExpressionEditorForm.ToBooleanFunction.ExpressionHelpText");
	}

	public override object Evaluate(IList variableValues)
	{
		return Convert.ToBoolean(base.ArgumentList[0].Evaluate(variableValues));
	}
}
