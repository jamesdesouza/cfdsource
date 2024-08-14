using System;
using TCX.CFD.Controls;

namespace TCX.CFD.Classes.Components;

[Serializable]
public class ServerHolidaysDateTimeCondition : DateTimeCondition
{
	public override string GetConditionExpression()
	{
		return "IsServerInHoliday(myCall)";
	}

	public override DateTimeCondition Clone()
	{
		return new ServerHolidaysDateTimeCondition();
	}

	public override AbsDateTimeConditionEditorRowControl CreateDateTimeConditionEditorRowControl()
	{
		return new ServerHolidaysDateTimeConditionEditorRowControl();
	}
}
