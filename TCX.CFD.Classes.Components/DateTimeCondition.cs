using System;
using System.Xml.Serialization;
using TCX.CFD.Controls;

namespace TCX.CFD.Classes.Components;

[Serializable]
[XmlInclude(typeof(DayOfWeekDateTimeCondition))]
[XmlInclude(typeof(SpecificDayDateTimeCondition))]
[XmlInclude(typeof(DayRangeDateTimeCondition))]
[XmlInclude(typeof(ServerOfficeHoursDateTimeCondition))]
[XmlInclude(typeof(ServerOutOfOfficeHoursDateTimeCondition))]
[XmlInclude(typeof(ServerHolidaysDateTimeCondition))]
public abstract class DateTimeCondition
{
	public virtual bool IsValid()
	{
		return true;
	}

	public abstract string GetConditionExpression();

	public abstract DateTimeCondition Clone();

	public abstract AbsDateTimeConditionEditorRowControl CreateDateTimeConditionEditorRowControl();
}
