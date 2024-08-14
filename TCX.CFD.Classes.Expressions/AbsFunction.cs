using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using TCX.CFD.Controls;

namespace TCX.CFD.Classes.Expressions;

public abstract class AbsFunction : AbsArgument
{
	public string Name { get; }

	public List<AbsArgument> ArgumentList { get; }

	protected abstract string WrapArgument(int index, string argument);

	private string GetString(bool fromCompiler)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (fromCompiler)
		{
			stringBuilder.Append("CFDFunctions.");
		}
		stringBuilder.Append(Name);
		stringBuilder.Append("(");
		for (int i = 0; i < ArgumentList.Count; i++)
		{
			stringBuilder.Append(fromCompiler ? WrapArgument(i, ArgumentList[i].GetCompilerString()) : ArgumentList[i].GetString());
			if (i < ArgumentList.Count - 1)
			{
				stringBuilder.Append(",");
			}
		}
		stringBuilder.Append(")");
		return stringBuilder.ToString();
	}

	protected static bool AreEqual(object arg1, object arg2)
	{
		if (arg1 == null)
		{
			return arg2 == null;
		}
		if (arg2 == null)
		{
			return false;
		}
		if ((arg1.GetType() == typeof(short) || arg1.GetType() == typeof(int) || arg1.GetType() == typeof(long)) && (arg2.GetType() == typeof(short) || arg2.GetType() == typeof(int) || arg2.GetType() == typeof(long)))
		{
			long num = ((arg1.GetType() == typeof(short)) ? ((short)arg1) : ((arg1.GetType() == typeof(int)) ? ((int)arg1) : ((long)arg1)));
			long num2 = ((arg2.GetType() == typeof(short)) ? ((short)arg2) : ((arg2.GetType() == typeof(int)) ? ((int)arg2) : ((long)arg2)));
			return num == num2;
		}
		return arg1.Equals(arg2);
	}

	protected AbsFunction(List<string> validVariables, string name, List<string> arguments)
	{
		Name = name;
		ArgumentList = new List<AbsArgument>();
		foreach (string argument in arguments)
		{
			ArgumentList.Add(AbsArgument.BuildArgument(validVariables, argument));
		}
	}

	public override bool IsFunction()
	{
		return true;
	}

	public override bool IsVariableName()
	{
		return false;
	}

	public override bool IsDotNetExpression()
	{
		return false;
	}

	public override bool IsSafeExpression()
	{
		foreach (AbsArgument argument in ArgumentList)
		{
			if (!argument.IsSafeExpression())
			{
				return false;
			}
		}
		return true;
	}

	public override string GetString()
	{
		return GetString(fromCompiler: false);
	}

	public override string GetCompilerString()
	{
		return GetString(fromCompiler: true);
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		foreach (AbsArgument argument in ArgumentList)
		{
			argument.NotifyComponentRenamed(oldValue, newValue);
		}
	}

	public override void MigrateConstantStringExpression()
	{
		foreach (AbsArgument argument in ArgumentList)
		{
			argument.MigrateConstantStringExpression();
		}
	}

	public override UserControl GetExpressionEditorChildControl(List<string> validVariables)
	{
		return new ExpressionEditorInbuiltFunctionControl
		{
			ValidVariables = validVariables,
			Function = this
		};
	}

	public override ExpressionEditorRowControl GetExpressionEditorRowControl(List<string> validVariables)
	{
		return AbsArgument.WrapRowControl(validVariables, GetExpressionEditorChildControl(validVariables), GetHelpText());
	}

	public override List<DotNetExpressionArgument> GetLiteralExpressionList()
	{
		List<DotNetExpressionArgument> list = new List<DotNetExpressionArgument>();
		foreach (AbsArgument argument in ArgumentList)
		{
			list.AddRange(argument.GetLiteralExpressionList());
		}
		return list;
	}

	public override List<VariableNameArgument> GetVariableNameList()
	{
		List<VariableNameArgument> list = new List<VariableNameArgument>();
		foreach (AbsArgument argument in ArgumentList)
		{
			list.AddRange(argument.GetVariableNameList());
		}
		return list;
	}

	public abstract bool HasFixedArguments();

	public abstract int GetFixedArgumentCount();

	public abstract int GetMinArgumentCount();

	public abstract string GetHelpText();

	public static AbsFunction BuildFunction(List<string> validVariables, string name, List<string> arguments)
	{
		return name switch
		{
			"AND" => new AndFunction(validVariables, name, arguments), 
			"OR" => new OrFunction(validVariables, name, arguments), 
			"NOT" => new NotFunction(validVariables, name, arguments), 
			"EQUAL" => new EqualFunction(validVariables, name, arguments), 
			"NOT_EQUAL" => new NotEqualFunction(validVariables, name, arguments), 
			"GREAT_THAN" => new GreatThanFunction(validVariables, name, arguments), 
			"GREAT_THAN_OR_EQUAL" => new GreatThanOrEqualFunction(validVariables, name, arguments), 
			"LESS_THAN" => new LessThanFunction(validVariables, name, arguments), 
			"LESS_THAN_OR_EQUAL" => new LessThanOrEqualFunction(validVariables, name, arguments), 
			"CONCATENATE" => new ConcatenateFunction(validVariables, name, arguments), 
			"CONTAINS" => new ContainsFunction(validVariables, name, arguments), 
			"TRIM" => new TrimFunction(validVariables, name, arguments), 
			"LEFT" => new LeftFunction(validVariables, name, arguments), 
			"MID" => new MidFunction(validVariables, name, arguments), 
			"RIGHT" => new RightFunction(validVariables, name, arguments), 
			"REPLACE" => new ReplaceFunction(validVariables, name, arguments), 
			"REPLACE_REG_EXP" => new ReplaceRegExpFunction(validVariables, name, arguments), 
			"UPPER" => new UpperFunction(validVariables, name, arguments), 
			"LOWER" => new LowerFunction(validVariables, name, arguments), 
			"NOW" => new NowFunction(validVariables, name, arguments), 
			"LEN" => new LenFunction(validVariables, name, arguments), 
			"SUM" => new SumFunction(validVariables, name, arguments), 
			"NEGATIVE" => new NegativeFunction(validVariables, name, arguments), 
			"MULTIPLY" => new MultiplyFunction(validVariables, name, arguments), 
			"DIVIDE" => new DivideFunction(validVariables, name, arguments), 
			"ABS" => new AbsoluteFunction(validVariables, name, arguments), 
			"SUM_LONG" => new SumLongFunction(validVariables, name, arguments), 
			"NEGATIVE_LONG" => new NegativeLongFunction(validVariables, name, arguments), 
			"MULTIPLY_LONG" => new MultiplyLongFunction(validVariables, name, arguments), 
			"DIVIDE_LONG" => new DivideLongFunction(validVariables, name, arguments), 
			"ABS_LONG" => new AbsoluteLongFunction(validVariables, name, arguments), 
			"GET_TABLE_ROW_COUNT" => new GetTableRowCountFunction(validVariables, name, arguments), 
			"GET_TABLE_CELL_VALUE" => new GetTableCellValueFunction(validVariables, name, arguments), 
			"GET_LIST_ITEM_COUNT" => new GetListItemCountFunction(validVariables, name, arguments), 
			"GET_LIST_ITEM" => new GetListItemFunction(validVariables, name, arguments), 
			"TO_BOOLEAN" => new ToBooleanFunction(validVariables, name, arguments), 
			"TO_INTEGER" => new ToIntegerFunction(validVariables, name, arguments), 
			"TO_LONG" => new ToLongFunction(validVariables, name, arguments), 
			"TO_STRING" => new ToStringFunction(validVariables, name, arguments), 
			_ => null, 
		};
	}
}
