using System.Collections;
using System.Collections.Generic;

namespace TCX.CFD.Classes.Expressions;

public class GetListItemCountFunction : AbsFunction
{
	protected override string WrapArgument(int index, string argument)
	{
		if (argument.StartsWith("variableMap"))
		{
			return $"new List<object>(((System.Collections.IList){argument}).Cast<object>())";
		}
		return $"new List<object>((object[]){argument}.ToArray())";
	}

	public GetListItemCountFunction(List<string> validVariables, string name, List<string> arguments)
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
		return LocalizedResourceMgr.GetString("ExpressionEditorForm.GetListItemCountFunction.ExpressionHelpText");
	}

	public override object Evaluate(IList variableValues)
	{
		return (base.ArgumentList[0].Evaluate(variableValues) is List<object> list) ? list.Count : 0;
	}
}
