using System;
using System.Collections;
using System.Collections.Generic;

namespace TCX.CFD.Classes.Expressions;

public class LessThanOrEqualFunction : AbsFunction
{
	protected override string WrapArgument(int index, string argument)
	{
		return $"(IComparable){argument}";
	}

	public LessThanOrEqualFunction(List<string> validVariables, string name, List<string> arguments)
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
		return LocalizedResourceMgr.GetString("ExpressionEditorForm.LessThanOrEqualFunction.ExpressionHelpText");
	}

	public override object Evaluate(IList variableValues)
	{
		IComparable comparable = base.ArgumentList[0].Evaluate(variableValues) as IComparable;
		IComparable comparable2 = base.ArgumentList[1].Evaluate(variableValues) as IComparable;
		return (comparable == null) ? (comparable2 == null) : (comparable.CompareTo(comparable2) <= 0);
	}
}
