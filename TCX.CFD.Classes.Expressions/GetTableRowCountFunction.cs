using System.Collections;
using System.Collections.Generic;

namespace TCX.CFD.Classes.Expressions;

public class GetTableRowCountFunction : AbsFunction
{
	protected override string WrapArgument(int index, string argument)
	{
		return argument;
	}

	public GetTableRowCountFunction(List<string> validVariables, string name, List<string> arguments)
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
		return LocalizedResourceMgr.GetString("ExpressionEditorForm.GetTableRowCountFunction.ExpressionHelpText");
	}

	public override object Evaluate(IList variableValues)
	{
		return (base.ArgumentList[0].Evaluate(variableValues) is List<object[]> list) ? list.Count : 0;
	}
}
