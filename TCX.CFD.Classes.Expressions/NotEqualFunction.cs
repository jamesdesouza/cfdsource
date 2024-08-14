using System.Collections;
using System.Collections.Generic;

namespace TCX.CFD.Classes.Expressions;

public class NotEqualFunction : AbsFunction
{
	protected override string WrapArgument(int index, string argument)
	{
		return argument;
	}

	public NotEqualFunction(List<string> validVariables, string name, List<string> arguments)
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
		return LocalizedResourceMgr.GetString("ExpressionEditorForm.NotEqualFunction.ExpressionHelpText");
	}

	public override object Evaluate(IList variableValues)
	{
		object arg = base.ArgumentList[0].Evaluate(variableValues);
		object arg2 = base.ArgumentList[1].Evaluate(variableValues);
		return !AbsFunction.AreEqual(arg, arg2);
	}
}
