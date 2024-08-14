using System;
using System.Collections;
using System.Collections.Generic;

namespace TCX.CFD.Classes.Expressions;

public class NowFunction : AbsFunction
{
	protected override string WrapArgument(int index, string argument)
	{
		return argument;
	}

	public NowFunction(List<string> validVariables, string name, List<string> arguments)
		: base(validVariables, name, arguments)
	{
	}

	public override bool HasFixedArguments()
	{
		return true;
	}

	public override int GetFixedArgumentCount()
	{
		return 0;
	}

	public override int GetMinArgumentCount()
	{
		return 0;
	}

	public override string GetHelpText()
	{
		return LocalizedResourceMgr.GetString("ExpressionEditorForm.NowFunction.ExpressionHelpText");
	}

	public override object Evaluate(IList variableValues)
	{
		return DateTime.Now;
	}
}
