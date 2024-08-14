using System;
using System.Collections;
using System.Collections.Generic;

namespace TCX.CFD.Classes.Expressions;

public class RightFunction : AbsFunction
{
	protected override string WrapArgument(int index, string argument)
	{
		if (index != 0)
		{
			return $"Convert.ToInt32({argument})";
		}
		return $"Convert.ToString({argument})";
	}

	public RightFunction(List<string> validVariables, string name, List<string> arguments)
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
		return LocalizedResourceMgr.GetString("ExpressionEditorForm.RightFunction.ExpressionHelpText");
	}

	public override object Evaluate(IList variableValues)
	{
		string text = Convert.ToString(base.ArgumentList[0].Evaluate(variableValues));
		int num = Convert.ToInt32(base.ArgumentList[1].Evaluate(variableValues));
		if (text != null)
		{
			if (text.Length >= num)
			{
				return text.Substring(text.Length - num, num);
			}
			return text;
		}
		return null;
	}
}
