using System;
using System.Text;
using TCX.CFD.Controls;

namespace TCX.CFD.Classes.Components;

[Serializable]
public class SpecificDayDateTimeCondition : DateTimeCondition
{
	public DateTime SpecificDay { get; set; } = DateTime.Now;


	public int HourFrom { get; set; } = 8;


	public int MinuteFrom { get; set; }

	public int HourTo { get; set; } = 17;


	public int MinuteTo { get; set; }

	public SpecificDayDateTimeCondition()
	{
	}

	public SpecificDayDateTimeCondition(DateTime specificDay, int hourFrom, int minuteFrom, int hourTo, int minuteTo)
	{
		SetValues(specificDay, hourFrom, minuteFrom, hourTo, minuteTo);
	}

	public void SetValues(DateTime specificDay, int hourFrom, int minuteFrom, int hourTo, int minuteTo)
	{
		SpecificDay = specificDay;
		HourFrom = hourFrom;
		MinuteFrom = minuteFrom;
		HourTo = hourTo;
		MinuteTo = minuteTo;
	}

	public override bool IsValid()
	{
		if (HourFrom >= HourTo)
		{
			if (HourFrom == HourTo)
			{
				return MinuteFrom < MinuteTo;
			}
			return false;
		}
		return true;
	}

	public override string GetConditionExpression()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("DateTime.Now.Year == ").Append(SpecificDay.Year);
		stringBuilder.Append(" && DateTime.Now.Month == ").Append(SpecificDay.Month);
		stringBuilder.Append(" && DateTime.Now.Day == ").Append(SpecificDay.Day);
		stringBuilder.Append(" && (DateTime.Now.Hour > ").Append(HourFrom).Append(" || DateTime.Now.Hour == ")
			.Append(HourFrom)
			.Append(" && DateTime.Now.Minute >= ")
			.Append(MinuteFrom)
			.Append(")");
		stringBuilder.Append(" && (DateTime.Now.Hour < ").Append(HourTo).Append(" || DateTime.Now.Hour == ")
			.Append(HourTo)
			.Append(" && DateTime.Now.Minute <= ")
			.Append(MinuteTo)
			.Append(")");
		return stringBuilder.ToString();
	}

	public override DateTimeCondition Clone()
	{
		return new SpecificDayDateTimeCondition(SpecificDay, HourFrom, MinuteFrom, HourTo, MinuteTo);
	}

	public override AbsDateTimeConditionEditorRowControl CreateDateTimeConditionEditorRowControl()
	{
		return new SpecificDayDateTimeConditionEditorRowControl(this);
	}
}
