using System;
using System.Collections;
using System.Collections.Generic;

namespace TCX.CFD.Classes.Expressions;

public class MidFunction : AbsFunction
{
	protected override string WrapArgument(int index, string argument)
	{
		if (index != 0)
		{
			return $"Convert.ToInt32({argument})";
		}
		return $"Convert.ToString({argument})";
	}

	public MidFunction(List<string> validVariables, string name, List<string> arguments)
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
		return LocalizedResourceMgr.GetString("ExpressionEditorForm.MidFunction.ExpressionHelpText");
	}

	public override object Evaluate(IList variableValues)
	{
		string text = Convert.ToString(base.ArgumentList[0].Evaluate(variableValues));
		int num = Convert.ToInt32(base.ArgumentList[1].Evaluate(variableValues));
		int num2 = Convert.ToInt32(base.ArgumentList[2].Evaluate(variableValues));
		if (text != null)
		{
			if (text.Length - num >= num2)
			{
				return text.Substring(num, num2);
			}
			return text.Substring(num);
		}
		return null;
	}
}
