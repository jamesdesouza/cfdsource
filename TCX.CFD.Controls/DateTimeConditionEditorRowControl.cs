using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;

namespace TCX.CFD.Controls;

public class DateTimeConditionEditorRowControl : UserControl
{
	private DateTimeCondition dateTimeCondition;

	private readonly DayOfWeekDateTimeCondition dayOfWeekDateTimeCondition = new DayOfWeekDateTimeCondition();

	private readonly SpecificDayDateTimeCondition specificDayDateTimeCondition = new SpecificDayDateTimeCondition();

	private readonly DayRangeDateTimeCondition dayRangeDateTimeCondition = new DayRangeDateTimeCondition();

	private AbsDateTimeConditionEditorRowControl dateTimeConditionEditorRowControl;

	private IContainer components;

	private ComboBox comboConditionType;

	private CheckBox chkSelection;

	public bool IsChecked
	{
		get
		{
			return chkSelection.Checked;
		}
		set
		{
			chkSelection.Checked = value;
		}
	}

	public event EventHandler CheckedChanged;

	private void CreateDateTimeConditionEditorRowControl()
	{
		if (dateTimeConditionEditorRowControl != null)
		{
			base.Controls.Remove(dateTimeConditionEditorRowControl);
		}
		dateTimeConditionEditorRowControl = dateTimeCondition.CreateDateTimeConditionEditorRowControl();
		dateTimeConditionEditorRowControl.Location = new Point(comboConditionType.Location.X + comboConditionType.Width + 6, 0);
		dateTimeConditionEditorRowControl.Size = new Size(base.Width - dateTimeConditionEditorRowControl.Location.X - 6, base.Height);
		dateTimeConditionEditorRowControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		base.Controls.Add(dateTimeConditionEditorRowControl);
	}

	public DateTimeConditionEditorRowControl(DateTimeCondition dateTimeCondition)
	{
		InitializeComponent();
		this.dateTimeCondition = dateTimeCondition;
		dateTimeConditionEditorRowControl = null;
		comboConditionType.Items.AddRange(new object[6]
		{
			LocalizedResourceMgr.GetString("DateTimeConditionEditorRowControl.comboConditionType.DayOfWeekDateTimeCondition"),
			LocalizedResourceMgr.GetString("DateTimeConditionEditorRowControl.comboConditionType.SpecificDayDateTimeCondition"),
			LocalizedResourceMgr.GetString("DateTimeConditionEditorRowControl.comboConditionType.DayRangeDateTimeCondition"),
			LocalizedResourceMgr.GetString("DateTimeConditionEditorRowControl.comboConditionType.ServerOfficeHoursDateTimeCondition"),
			LocalizedResourceMgr.GetString("DateTimeConditionEditorRowControl.comboConditionType.ServerOutOfOfficeHoursDateTimeCondition"),
			LocalizedResourceMgr.GetString("DateTimeConditionEditorRowControl.comboConditionType.ServerHolidayDateTimeCondition")
		});
		if (dateTimeCondition is DayOfWeekDateTimeCondition)
		{
			dayOfWeekDateTimeCondition = dateTimeCondition as DayOfWeekDateTimeCondition;
			comboConditionType.SelectedIndex = 0;
		}
		else if (dateTimeCondition is SpecificDayDateTimeCondition)
		{
			specificDayDateTimeCondition = dateTimeCondition as SpecificDayDateTimeCondition;
			comboConditionType.SelectedIndex = 1;
		}
		else if (dateTimeCondition is DayRangeDateTimeCondition)
		{
			dayRangeDateTimeCondition = dateTimeCondition as DayRangeDateTimeCondition;
			comboConditionType.SelectedIndex = 2;
		}
		else if (dateTimeCondition is ServerOfficeHoursDateTimeCondition)
		{
			comboConditionType.SelectedIndex = 3;
		}
		else if (dateTimeCondition is ServerOutOfOfficeHoursDateTimeCondition)
		{
			comboConditionType.SelectedIndex = 4;
		}
		else
		{
			comboConditionType.SelectedIndex = 5;
		}
	}

	private void ChkSelection_CheckedChanged(object sender, EventArgs e)
	{
		this.CheckedChanged?.Invoke(this, e);
	}

	private void ComboConditionType_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (dateTimeConditionEditorRowControl != null)
		{
			dateTimeConditionEditorRowControl.Save();
		}
		switch (comboConditionType.SelectedIndex)
		{
		case 0:
			dateTimeCondition = dayOfWeekDateTimeCondition;
			break;
		case 1:
			dateTimeCondition = specificDayDateTimeCondition;
			break;
		case 2:
			dateTimeCondition = dayRangeDateTimeCondition;
			break;
		case 3:
			dateTimeCondition = new ServerOfficeHoursDateTimeCondition();
			break;
		case 4:
			dateTimeCondition = new ServerOutOfOfficeHoursDateTimeCondition();
			break;
		case 5:
			dateTimeCondition = new ServerHolidaysDateTimeCondition();
			break;
		}
		CreateDateTimeConditionEditorRowControl();
	}

	public DateTimeCondition Save()
	{
		return dateTimeConditionEditorRowControl.Save();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.comboConditionType = new System.Windows.Forms.ComboBox();
		this.chkSelection = new System.Windows.Forms.CheckBox();
		base.SuspendLayout();
		this.comboConditionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboConditionType.FormattingEnabled = true;
		this.comboConditionType.Location = new System.Drawing.Point(24, 14);
		this.comboConditionType.Name = "comboConditionType";
		this.comboConditionType.Size = new System.Drawing.Size(150, 21);
		this.comboConditionType.TabIndex = 0;
		this.comboConditionType.SelectedIndexChanged += new System.EventHandler(ComboConditionType_SelectedIndexChanged);
		this.chkSelection.AutoSize = true;
		this.chkSelection.Location = new System.Drawing.Point(3, 17);
		this.chkSelection.Name = "chkSelection";
		this.chkSelection.Size = new System.Drawing.Size(15, 14);
		this.chkSelection.TabIndex = 1;
		this.chkSelection.UseVisualStyleBackColor = true;
		this.chkSelection.CheckedChanged += new System.EventHandler(ChkSelection_CheckedChanged);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.chkSelection);
		base.Controls.Add(this.comboConditionType);
		base.Name = "DateTimeConditionEditorRowControl";
		base.Size = new System.Drawing.Size(585, 49);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
