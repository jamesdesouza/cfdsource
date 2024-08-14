using System;
using TCX.CFD.Controls;

namespace TCX.CFD.Classes.Components;

[Serializable]
public class ServerOfficeHoursDateTimeCondition : DateTimeCondition
{
	public override string GetConditionExpression()
	{
		return "IsServerOfficeHourActive(myCall)";
	}

	public override DateTimeCondition Clone()
	{
		return new ServerOfficeHoursDateTimeCondition();
	}

	public override AbsDateTimeConditionEditorRowControl CreateDateTimeConditionEditorRowControl()
	{
		return new ServerOfficeHoursDateTimeConditionEditorRowControl();
	}
}
