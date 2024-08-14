using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using TCX.CFD.Controls;

namespace TCX.CFD.Classes.Expressions;

public class DotNetExpressionArgument : AbsArgument
{
	private readonly ConstantValueTypes forcedType;

	private string expression;

	public DotNetExpressionArgument(string expression)
	{
		this.expression = expression;
		forcedType = ConstantValueTypes.None;
	}

	public DotNetExpressionArgument(string expression, ConstantValueTypes forcedType)
	{
		this.expression = expression;
		this.forcedType = forcedType;
	}

	public override bool IsFunction()
	{
		return false;
	}

	public override bool IsVariableName()
	{
		return false;
	}

	public override bool IsDotNetExpression()
	{
		return true;
	}

	public bool IsStringLiteral()
	{
		return ExpressionHelper.IsStringLiteral(expression);
	}

	public bool IsCharLiteral()
	{
		return ExpressionHelper.IsCharLiteral(expression);
	}

	public bool IsBooleanLiteral()
	{
		return ExpressionHelper.IsBooleanLiteral(expression);
	}

	public bool IsIntegerLiteral()
	{
		return ExpressionHelper.IsIntegerLiteral(expression);
	}

	public bool IsDoubleLiteral()
	{
		return ExpressionHelper.IsDoubleLiteral(expression);
	}

	public override bool IsSafeExpression()
	{
		if (!ExpressionHelper.IsStringLiteral(expression) && !ExpressionHelper.IsCharLiteral(expression) && !ExpressionHelper.IsBooleanLiteral(expression) && !ExpressionHelper.IsIntegerLiteral(expression))
		{
			return ExpressionHelper.IsDoubleLiteral(expression);
		}
		return true;
	}

	public override string GetString()
	{
		return expression;
	}

	public override string GetCompilerString()
	{
		return expression;
	}

	public override object Evaluate(IList variableValues)
	{
		if (IsStringLiteral())
		{
			return ExpressionHelper.UnescapeConstantString(expression);
		}
		if (IsCharLiteral())
		{
			return expression.Substring(1, expression.Length - 2);
		}
		return expression;
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		if (expression.StartsWith(oldValue + "."))
		{
			expression = newValue + expression.Substring(oldValue.Length);
		}
		else if (expression == oldValue)
		{
			expression = newValue;
		}
	}

	public override void MigrateConstantStringExpression()
	{
		expression = ExpressionHelper.MigrateConstantStringExpression(expression);
	}

	public override UserControl GetExpressionEditorChildControl(List<string> validVariables)
	{
		if (string.IsNullOrEmpty(expression))
		{
			return new ExpressionEditorEmptyControl();
		}
		return new ExpressionEditorConstantValueControl
		{
			ConstantValue = this,
			ForcedType = forcedType
		};
	}

	public override ExpressionEditorRowControl GetExpressionEditorRowControl(List<string> validVariables)
	{
		return AbsArgument.WrapRowControl(validVariables, GetExpressionEditorChildControl(validVariables), string.IsNullOrEmpty(expression) ? LocalizedResourceMgr.GetString("ExpressionEditorForm.EmptyExpression.HelpText") : LocalizedResourceMgr.GetString("ExpressionEditorForm.ConstantValue.HelpText"));
	}

	public override List<DotNetExpressionArgument> GetLiteralExpressionList()
	{
		return new List<DotNetExpressionArgument> { this };
	}

	public override List<VariableNameArgument> GetVariableNameList()
	{
		return new List<VariableNameArgument>();
	}
}
