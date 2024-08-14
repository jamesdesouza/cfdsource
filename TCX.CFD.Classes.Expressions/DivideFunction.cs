using System;
using System.Collections;
using System.Collections.Generic;

namespace TCX.CFD.Classes.Expressions;

public class DivideFunction : AbsFunction
{
	protected override string WrapArgument(int index, string argument)
	{
		return $"Convert.ToInt32({argument})";
	}

	public DivideFunction(List<string> validVariables, string name, List<string> arguments)
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
		return LocalizedResourceMgr.GetString("ExpressionEditorForm.DivideFunction.ExpressionHelpText");
	}

	public override object Evaluate(IList variableValues)
	{
		int num = Convert.ToInt32(base.ArgumentList[0].Evaluate(variableValues));
		int num2 = Convert.ToInt32(base.ArgumentList[1].Evaluate(variableValues));
		return num / num2;
	}
}
