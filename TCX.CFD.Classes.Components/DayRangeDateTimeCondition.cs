using System;
using System.Text;
using TCX.CFD.Controls;

namespace TCX.CFD.Classes.Components;

[Serializable]
public class DayRangeDateTimeCondition : DateTimeCondition
{
	public DateTime DayFrom { get; set; } = DateTime.Now;


	public DateTime DayTo { get; set; } = DateTime.Now.AddDays(1.0);


	public int HourFrom { get; set; } = 8;


	public int MinuteFrom { get; set; }

	public int HourTo { get; set; } = 17;


	public int MinuteTo { get; set; }

	public DayRangeDateTimeCondition()
	{
	}

	public DayRangeDateTimeCondition(DateTime dayFrom, DateTime dayTo, int hourFrom, int minuteFrom, int hourTo, int minuteTo)
	{
		SetValues(dayFrom, dayTo, hourFrom, minuteFrom, hourTo, minuteTo);
	}

	public void SetValues(DateTime dayFrom, DateTime dayTo, int hourFrom, int minuteFrom, int hourTo, int minuteTo)
	{
		DayFrom = dayFrom;
		DayTo = dayTo;
		HourFrom = hourFrom;
		MinuteFrom = minuteFrom;
		HourTo = hourTo;
		MinuteTo = minuteTo;
	}

	public override bool IsValid()
	{
		if (DayFrom < DayTo)
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
		return false;
	}

	public override string GetConditionExpression()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("DateTime.Now >= new DateTime(").Append(DayFrom.Year).Append(", ")
			.Append(DayFrom.Month)
			.Append(", ")
			.Append(DayFrom.Day)
			.Append(", ")
			.Append(HourFrom)
			.Append(", ")
			.Append(MinuteFrom)
			.Append(", 0)");
		stringBuilder.Append(" && DateTime.Now <= new DateTime(").Append(DayTo.Year).Append(", ")
			.Append(DayTo.Month)
			.Append(", ")
			.Append(DayTo.Day)
			.Append(", ")
			.Append(HourTo)
			.Append(", ")
			.Append(MinuteTo)
			.Append(", 59)");
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
		return new DayRangeDateTimeCondition(DayFrom, DayTo, HourFrom, MinuteFrom, HourTo, MinuteTo);
	}

	public override AbsDateTimeConditionEditorRowControl CreateDateTimeConditionEditorRowControl()
	{
		return new DayRangeDateTimeConditionEditorRowControl(this);
	}
}
