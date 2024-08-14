using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using TCX.CFD.Classes.Components;
using TCX.CFD.Controls;

namespace TCX.CFD.Classes.Expressions;

public class VariableNameArgument : AbsArgument
{
	private string expression;

	public VariableNameArgument(string expression)
	{
		this.expression = expression;
	}

	public bool IsRecordResult()
	{
		return expression.StartsWith("RecordResult.");
	}

	public bool IsMenuResult()
	{
		return expression.StartsWith("MenuResult.");
	}

	public bool IsUserInputResult()
	{
		return expression.StartsWith("UserInputResult.");
	}

	public bool IsVoiceInputResult()
	{
		return expression.StartsWith("VoiceInputResult.");
	}

	public override bool IsFunction()
	{
		return false;
	}

	public override bool IsVariableName()
	{
		return true;
	}

	public override bool IsDotNetExpression()
	{
		return false;
	}

	public override bool IsSafeExpression()
	{
		return true;
	}

	public override string GetString()
	{
		return expression;
	}

	public override string GetCompilerString()
	{
		if (expression.StartsWith("project$.") || expression.StartsWith("callflow$.") || expression.StartsWith("session.") || expression.StartsWith("RecordResult.") || expression.StartsWith("MenuResult.") || expression.StartsWith("UserInputResult.") || expression.StartsWith("VoiceInputResult."))
		{
			return $"variableMap[\"{expression}\"].Value";
		}
		return expression;
	}

	public override object Evaluate(IList variableValues)
	{
		foreach (Parameter variableValue in variableValues)
		{
			if (variableValue.Name == expression)
			{
				return variableValue.Value;
			}
		}
		throw new Exception("Couldn't find value for variable '" + expression + "'");
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		if (expression == oldValue)
		{
			expression = newValue;
		}
	}

	public override void MigrateConstantStringExpression()
	{
	}

	public override UserControl GetExpressionEditorChildControl(List<string> validVariables)
	{
		return new ExpressionEditorVariableControl
		{
			Variable = this
		};
	}

	public override ExpressionEditorRowControl GetExpressionEditorRowControl(List<string> validVariables)
	{
		return AbsArgument.WrapRowControl(validVariables, GetExpressionEditorChildControl(validVariables), LocalizedResourceMgr.GetString("ExpressionEditorForm.VariableName.HelpText"));
	}

	public override List<DotNetExpressionArgument> GetLiteralExpressionList()
	{
		return new List<DotNetExpressionArgument>();
	}

	public override List<VariableNameArgument> GetVariableNameList()
	{
		return new List<VariableNameArgument> { this };
	}
}
