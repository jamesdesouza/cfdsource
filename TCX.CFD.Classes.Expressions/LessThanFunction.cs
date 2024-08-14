using System;
using System.Collections;
using System.Collections.Generic;

namespace TCX.CFD.Classes.Expressions;

public class LessThanFunction : AbsFunction
{
	protected override string WrapArgument(int index, string argument)
	{
		return $"(IComparable){argument}";
	}

	public LessThanFunction(List<string> validVariables, string name, List<string> arguments)
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
		return LocalizedResourceMgr.GetString("ExpressionEditorForm.LessThanFunction.ExpressionHelpText");
	}

	public override object Evaluate(IList variableValues)
	{
		IComparable comparable = base.ArgumentList[0].Evaluate(variableValues) as IComparable;
		IComparable obj = base.ArgumentList[1].Evaluate(variableValues) as IComparable;
		return comparable != null && comparable.CompareTo(obj) < 0;
	}
}
