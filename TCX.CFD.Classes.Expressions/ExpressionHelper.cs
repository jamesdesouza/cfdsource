using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Workflow.ComponentModel;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Expressions;

public static class ExpressionHelper
{
	public static string[] BooleanFunctionList = new string[11]
	{
		"AND", "OR", "NOT", "EQUAL", "NOT_EQUAL", "CONTAINS", "GREAT_THAN", "GREAT_THAN_OR_EQUAL", "LESS_THAN", "LESS_THAN_OR_EQUAL",
		"TO_BOOLEAN"
	};

	public static string[] StringFunctionList = new string[10] { "CONCATENATE", "TRIM", "LEFT", "MID", "RIGHT", "UPPER", "LOWER", "REPLACE", "REPLACE_REG_EXP", "TO_STRING" };

	public static string[] DateTimeFunctionList = new string[1] { "NOW" };

	public static string[] NumberFunctionList = new string[15]
	{
		"LEN", "SUM", "SUM_LONG", "NEGATIVE", "NEGATIVE_LONG", "MULTIPLY", "MULTIPLY_LONG", "DIVIDE", "DIVIDE_LONG", "ABS",
		"ABS_LONG", "GET_TABLE_ROW_COUNT", "GET_LIST_ITEM_COUNT", "TO_INTEGER", "TO_LONG"
	};

	public static string[] AnyFunctionList = new string[2] { "GET_TABLE_CELL_VALUE", "GET_LIST_ITEM" };

	private static void LoadVariables(List<string> variableList, string name, List<Variable> properties, bool onlyWritableVariables, bool onlyPublicVariables)
	{
		foreach (Variable property in properties)
		{
			if ((!onlyWritableVariables || property.Accessibility == VariableAccessibilities.ReadWrite) && (property.Scope == VariableScopes.Public || !onlyPublicVariables))
			{
				variableList.Add(name + "." + property.Name);
			}
		}
	}

	private static void LoadActivityVariables(List<string> variableList, Activity activity, bool onlyWritableVariables, bool onlyPublicVariables)
	{
		LoadVariables(variableList, activity.Name, (activity as IVadActivity).Properties, onlyWritableVariables, onlyPublicVariables);
	}

	private static void LoadActivitiesVariables(List<string> variableList, Activity currentComponent, ReadOnlyCollection<Activity> activityCollection, bool isInitialComponent, bool onlyWritableVariables, bool onlyPublicVariables)
	{
		if (currentComponent.Parent != null && currentComponent.Parent.Parent != null)
		{
			LoadActivitiesVariables(variableList, currentComponent.Parent, currentComponent.Parent.Parent.EnabledActivities, isInitialComponent: false, onlyWritableVariables, onlyPublicVariables);
		}
		foreach (Activity item in activityCollection)
		{
			if (isInitialComponent)
			{
				if (item == currentComponent)
				{
					break;
				}
				LoadActivityVariables(variableList, item, onlyWritableVariables, onlyPublicVariables);
			}
			else
			{
				LoadActivityVariables(variableList, item, onlyWritableVariables, onlyPublicVariables);
				if (item == currentComponent)
				{
					break;
				}
			}
		}
	}

	public static bool IsStringLiteral(string s)
	{
		string text = ((s == null) ? string.Empty : s.Trim());
		if (text.Length > 1 && text.StartsWith("\"") && text.EndsWith("\""))
		{
			bool flag = false;
			string text2 = text.Substring(1, text.Length - 2);
			foreach (char c in text2)
			{
				if (c == '"' && !flag)
				{
					return false;
				}
				flag = c == '\\' && !flag;
			}
			return !flag;
		}
		return false;
	}

	public static bool IsCharLiteral(string s)
	{
		string text = ((s == null) ? string.Empty : s.Trim());
		char result;
		if (text.Length > 1 && text.StartsWith("'") && text.EndsWith("'"))
		{
			return char.TryParse(text.Substring(1, text.Length - 2), out result);
		}
		return false;
	}

	public static bool IsBooleanLiteral(string s)
	{
		string text = ((s == null) ? string.Empty : s.Trim());
		if (!(text == "true"))
		{
			return text == "false";
		}
		return true;
	}

	public static bool IsIntegerLiteral(string s)
	{
		long result;
		return long.TryParse((s == null) ? string.Empty : s.Trim(), out result);
	}

	public static bool IsDoubleLiteral(string s)
	{
		double result;
		return double.TryParse((s == null) ? string.Empty : s.Trim(), out result);
	}

	public static string EscapeConstantString(string s)
	{
		StringBuilder stringBuilder = new StringBuilder(s.Length);
		foreach (char c in s)
		{
			switch (c)
			{
			case '"':
				stringBuilder.Append("\\\"");
				continue;
			case '\\':
				stringBuilder.Append("\\\\");
				continue;
			case '\0':
				stringBuilder.Append("\\0");
				continue;
			case '\a':
				stringBuilder.Append("\\a");
				continue;
			case '\b':
				stringBuilder.Append("\\b");
				continue;
			case '\f':
				stringBuilder.Append("\\f");
				continue;
			case '\n':
				stringBuilder.Append("\\n");
				continue;
			case '\r':
				stringBuilder.Append("\\r");
				continue;
			case '\t':
				stringBuilder.Append("\\t");
				continue;
			case '\v':
				stringBuilder.Append("\\v");
				continue;
			}
			if (char.GetUnicodeCategory(c) != UnicodeCategory.Control)
			{
				stringBuilder.Append(c);
				continue;
			}
			stringBuilder.Append("\\u");
			ushort num = c;
			stringBuilder.Append(num.ToString("x4"));
		}
		return stringBuilder.ToString();
	}

	public static string UnescapeConstantString(string s)
	{
		string text = s.Trim();
		string text2 = text.Substring(1, text.Length - 2);
		StringBuilder stringBuilder = new StringBuilder(text2.Length);
		bool flag = false;
		for (int i = 0; i < text2.Length; i++)
		{
			char c = text2[i];
			if (flag)
			{
				flag = false;
				switch (c)
				{
				case '"':
					stringBuilder.Append(c);
					break;
				case '\\':
					stringBuilder.Append(c);
					break;
				case '0':
					stringBuilder.Append("\0");
					break;
				case 'a':
					stringBuilder.Append("\a");
					break;
				case 'b':
					stringBuilder.Append("\b");
					break;
				case 'f':
					stringBuilder.Append("\f");
					break;
				case 'n':
					stringBuilder.Append("\n");
					break;
				case 'r':
					stringBuilder.Append("\r");
					break;
				case 't':
					stringBuilder.Append("\t");
					break;
				case 'v':
					stringBuilder.Append("\v");
					break;
				case 'u':
				{
					int num = int.Parse(text2.Substring(1 + i, 4), NumberStyles.HexNumber);
					i += 4;
					stringBuilder.Append((char)num);
					break;
				}
				default:
					stringBuilder.Append(c);
					break;
				}
			}
			else if (c == '\\')
			{
				flag = true;
			}
			else
			{
				stringBuilder.Append(c);
			}
		}
		return stringBuilder.ToString();
	}

	public static string MigrateConstantStringExpression(string expression)
	{
		string text = expression.Trim();
		if (text.Length >= 2 && text.StartsWith("'") && text.EndsWith("'"))
		{
			string text2 = text.Substring(1, text.Length - 2);
			bool flag = false;
			foreach (char c in text2)
			{
				if ((c == '"' || c == '\'') && !flag)
				{
					return expression;
				}
				flag = c == '\\' && !flag;
			}
			return "\"" + text2 + "\"";
		}
		return expression;
	}

	public static string MigrateConstantStringExpression(IVadActivity activity, string expression)
	{
		AbsArgument absArgument = AbsArgument.BuildArgument(GetValidVariables(activity), expression);
		absArgument.MigrateConstantStringExpression();
		return absArgument.GetString();
	}

	public static List<string> GetFunctionArguments(string expression)
	{
		List<string> list = new List<string>();
		int num = expression.IndexOf("(");
		int num2 = expression.LastIndexOf(")");
		if (num == -1 || num2 == -1 || num == num2 - 1)
		{
			return list;
		}
		string text = expression.Substring(num + 1, num2 - num - 1);
		int num3 = 0;
		int num4 = 0;
		bool flag = false;
		bool flag2 = false;
		for (int i = 0; i < text.Length; i++)
		{
			char c = text[i];
			if (c == '"')
			{
				if (flag)
				{
					if (!flag2)
					{
						flag = false;
					}
				}
				else
				{
					flag = true;
				}
			}
			else if (!flag && c == '(')
			{
				num3++;
			}
			else if (!flag && c == ')')
			{
				num3--;
			}
			else if (num3 == 0 && !flag && c == ',')
			{
				list.Add(text.Substring(num4, i - num4).Trim());
				num4 = i + 1;
			}
			flag2 = flag && c == '\\' && !flag2;
		}
		string item = text.Substring(num4).Trim();
		list.Add(item);
		return list;
	}

	public static bool IsBooleanFunction(string name)
	{
		string[] booleanFunctionList = BooleanFunctionList;
		for (int i = 0; i < booleanFunctionList.Length; i++)
		{
			if (booleanFunctionList[i] == name)
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsStringFunction(string name)
	{
		string[] stringFunctionList = StringFunctionList;
		for (int i = 0; i < stringFunctionList.Length; i++)
		{
			if (stringFunctionList[i] == name)
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsDateTimeFunction(string name)
	{
		string[] dateTimeFunctionList = DateTimeFunctionList;
		for (int i = 0; i < dateTimeFunctionList.Length; i++)
		{
			if (dateTimeFunctionList[i] == name)
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsNumberFunction(string name)
	{
		string[] numberFunctionList = NumberFunctionList;
		for (int i = 0; i < numberFunctionList.Length; i++)
		{
			if (numberFunctionList[i] == name)
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsAnyFunction(string name)
	{
		string[] anyFunctionList = AnyFunctionList;
		for (int i = 0; i < anyFunctionList.Length; i++)
		{
			if (anyFunctionList[i] == name)
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsFunction(string name)
	{
		if (!IsBooleanFunction(name) && !IsStringFunction(name) && !IsDateTimeFunction(name) && !IsNumberFunction(name))
		{
			return IsAnyFunction(name);
		}
		return true;
	}

	public static List<string> GetValidVariables(IVadActivity activity, bool onlyWritableVariables = false, bool includeConstants = true)
	{
		List<string> list = new List<string>();
		FileObject fileObject = activity.GetRootFlow().FileObject;
		LoadVariables(list, "callflow$", fileObject.Variables, onlyWritableVariables, onlyPublicVariables: false);
		ProjectObject projectObject = fileObject.GetProjectObject();
		LoadVariables(list, "project$", projectObject.Variables, onlyWritableVariables, onlyPublicVariables: false);
		if (!(fileObject is DialerFileObject))
		{
			if (includeConstants)
			{
				LoadVariables(list, "RecordResult", projectObject.RecordResultConstantList, onlyWritableVariables, onlyPublicVariables: false);
				LoadVariables(list, "MenuResult", projectObject.MenuResultConstantList, onlyWritableVariables, onlyPublicVariables: false);
				LoadVariables(list, "UserInputResult", projectObject.UserInputResultConstantList, onlyWritableVariables, onlyPublicVariables: false);
				LoadVariables(list, "VoiceInputResult", projectObject.VoiceInputResultConstantList, onlyWritableVariables, onlyPublicVariables: false);
			}
			if (!onlyWritableVariables)
			{
				list.Add("session.ani");
				list.Add("session.callid");
				list.Add("session.dnis");
				list.Add("session.did");
				list.Add("session.audioFolder");
				list.Add("session.transferingExtension");
			}
		}
		Activity activity2 = activity as Activity;
		LoadActivitiesVariables(list, activity2, activity2.Parent.EnabledActivities, isInitialComponent: true, onlyWritableVariables, onlyPublicVariables: true);
		return list;
	}

	public static Variable GetVariable(IVadActivity activity, string variableName)
	{
		string[] array = variableName?.Split('.');
		if (array.Length == 2)
		{
			string text = array[0];
			string text2 = array[1];
			RootFlow rootFlow = activity?.GetRootFlow();
			if (rootFlow != null)
			{
				if (text == "callflow$")
				{
					foreach (Variable variable in rootFlow.FileObject.Variables)
					{
						if (variable.Name == text2)
						{
							return variable;
						}
					}
				}
				else if (text == "project$")
				{
					foreach (Variable variable2 in rootFlow.FileObject.GetProjectObject().Variables)
					{
						if (variable2.Name == text2)
						{
							return variable2;
						}
					}
				}
				else
				{
					Activity activityByName = rootFlow.GetActivityByName(text);
					if (activityByName != null)
					{
						foreach (Variable property in (activityByName as IVadActivity).Properties)
						{
							if (property.Name == text2)
							{
								return property;
							}
						}
					}
				}
			}
		}
		return null;
	}

	public static string RenameComponent(IVadActivity activity, string expression, string oldValue, string newValue)
	{
		AbsArgument absArgument = AbsArgument.BuildArgument(GetValidVariables(activity), expression);
		absArgument.NotifyComponentRenamed(oldValue, newValue);
		return absArgument.GetString();
	}

	public static bool IsJSON(string text)
	{
		try
		{
			JToken.Parse(text);
			return true;
		}
		catch
		{
			return false;
		}
	}

	public static bool IsXML(string text)
	{
		try
		{
			XDocument.Parse(text);
			return true;
		}
		catch
		{
			return false;
		}
	}
}
