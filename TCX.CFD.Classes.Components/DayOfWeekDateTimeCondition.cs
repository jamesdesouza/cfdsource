using System;
using System.Collections.Generic;
using System.Text;
using TCX.CFD.Controls;

namespace TCX.CFD.Classes.Components;

[Serializable]
public class DayOfWeekDateTimeCondition : DateTimeCondition
{
	public bool SundayChecked { get; set; }

	public bool MondayChecked { get; set; } = true;


	public bool TuesdayChecked { get; set; } = true;


	public bool WednesdayChecked { get; set; } = true;


	public bool ThursdayChecked { get; set; } = true;


	public bool FridayChecked { get; set; } = true;


	public bool SaturdayChecked { get; set; }

	public int HourFrom { get; set; } = 8;


	public int MinuteFrom { get; set; }

	public int HourTo { get; set; } = 17;


	public int MinuteTo { get; set; }

	public DayOfWeekDateTimeCondition()
	{
	}

	public DayOfWeekDateTimeCondition(bool sundayChecked, bool mondayChecked, bool tuesdayChecked, bool wednesdayChecked, bool thursdayChecked, bool fridayChecked, bool saturdayChecked, int hourFrom, int minuteFrom, int hourTo, int minuteTo)
	{
		SetValues(sundayChecked, mondayChecked, tuesdayChecked, wednesdayChecked, thursdayChecked, fridayChecked, saturdayChecked, hourFrom, minuteFrom, hourTo, minuteTo);
	}

	public void SetValues(bool sundayChecked, bool mondayChecked, bool tuesdayChecked, bool wednesdayChecked, bool thursdayChecked, bool fridayChecked, bool saturdayChecked, int hourFrom, int minuteFrom, int hourTo, int minuteTo)
	{
		SundayChecked = sundayChecked;
		MondayChecked = mondayChecked;
		TuesdayChecked = tuesdayChecked;
		WednesdayChecked = wednesdayChecked;
		ThursdayChecked = thursdayChecked;
		FridayChecked = fridayChecked;
		SaturdayChecked = saturdayChecked;
		HourFrom = hourFrom;
		MinuteFrom = minuteFrom;
		HourTo = hourTo;
		MinuteTo = minuteTo;
	}

	public override bool IsValid()
	{
		if (SundayChecked || MondayChecked || TuesdayChecked || WednesdayChecked || ThursdayChecked || FridayChecked || SaturdayChecked)
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
		List<string> list = new List<string>();
		if (SundayChecked)
		{
			list.Add("DateTime.Now.DayOfWeek == DayOfWeek.Sunday");
		}
		if (MondayChecked)
		{
			list.Add("DateTime.Now.DayOfWeek == DayOfWeek.Monday");
		}
		if (TuesdayChecked)
		{
			list.Add("DateTime.Now.DayOfWeek == DayOfWeek.Tuesday");
		}
		if (WednesdayChecked)
		{
			list.Add("DateTime.Now.DayOfWeek == DayOfWeek.Wednesday");
		}
		if (ThursdayChecked)
		{
			list.Add("DateTime.Now.DayOfWeek == DayOfWeek.Thursday");
		}
		if (FridayChecked)
		{
			list.Add("DateTime.Now.DayOfWeek == DayOfWeek.Friday");
		}
		if (SaturdayChecked)
		{
			list.Add("DateTime.Now.DayOfWeek == DayOfWeek.Saturday");
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (list.Count > 0)
		{
			stringBuilder.Append("(");
			stringBuilder.Append(string.Join<string>(" || ", (IEnumerable<string>)list));
			stringBuilder.Append(")");
		}
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
		return new DayOfWeekDateTimeCondition(SundayChecked, MondayChecked, TuesdayChecked, WednesdayChecked, ThursdayChecked, FridayChecked, SaturdayChecked, HourFrom, MinuteFrom, HourTo, MinuteTo);
	}

	public override AbsDateTimeConditionEditorRowControl CreateDateTimeConditionEditorRowControl()
	{
		return new DayOfWeekDateTimeConditionEditorRowControl(this);
	}
}
