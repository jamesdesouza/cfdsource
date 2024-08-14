using System;
using TCX.CFD.Controls;

namespace TCX.CFD.Classes.Components;

[Serializable]
public class ServerOutOfOfficeHoursDateTimeCondition : DateTimeCondition
{
	public override string GetConditionExpression()
	{
		return "!IsServerOfficeHourActive(myCall)";
	}

	public override DateTimeCondition Clone()
	{
		return new ServerOutOfOfficeHoursDateTimeCondition();
	}

	public override AbsDateTimeConditionEditorRowControl CreateDateTimeConditionEditorRowControl()
	{
		return new ServerOutOfOfficeHoursDateTimeConditionEditorRowControl();
	}
}
