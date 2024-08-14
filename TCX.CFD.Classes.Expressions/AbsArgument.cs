using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using TCX.CFD.Controls;

namespace TCX.CFD.Classes.Expressions;

public abstract class AbsArgument
{
	public abstract bool IsFunction();

	public abstract bool IsVariableName();

	public abstract bool IsDotNetExpression();

	public abstract bool IsSafeExpression();

	public abstract string GetString();

	public abstract string GetCompilerString();

	public abstract object Evaluate(IList variableValues);

	public abstract void NotifyComponentRenamed(string oldValue, string newValue);

	public abstract void MigrateConstantStringExpression();

	public abstract UserControl GetExpressionEditorChildControl(List<string> validVariables);

	public abstract ExpressionEditorRowControl GetExpressionEditorRowControl(List<string> validVariables);

	public abstract List<DotNetExpressionArgument> GetLiteralExpressionList();

	public abstract List<VariableNameArgument> GetVariableNameList();

	protected static ExpressionEditorRowControl WrapRowControl(List<string> validVariables, UserControl userControl, string helpText)
	{
		return new ExpressionEditorRowControl
		{
			ValidVariables = validVariables,
			ChildControl = userControl,
			HelpText = helpText
		};
	}

	public static AbsArgument BuildArgument(List<string> validVariables, string expression)
	{
		if (string.IsNullOrEmpty(expression))
		{
			return new DotNetExpressionArgument(string.Empty);
		}
		int num = expression.IndexOf("(");
		if (num > 0)
		{
			string name = expression.Substring(0, num).Trim();
			if (ExpressionHelper.IsFunction(name))
			{
				return AbsFunction.BuildFunction(validVariables, name, ExpressionHelper.GetFunctionArguments(expression));
			}
		}
		if (validVariables.Contains(expression))
		{
			return new VariableNameArgument(expression);
		}
		return new DotNetExpressionArgument(expression);
	}
}
