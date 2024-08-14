using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;

namespace TCX.CFD.Controls;

public class DayOfWeekDateTimeConditionEditorRowControl : AbsDateTimeConditionEditorRowControl
{
	private DayOfWeekDateTimeCondition condition;

	private IContainer components;

	private CheckBox chkSunday;

	private CheckBox chkMonday;

	private CheckBox chkTuesday;

	private CheckBox chkWednesday;

	private CheckBox chkSaturday;

	private CheckBox chkFriday;

	private CheckBox chkThursday;

	private Label lblSunday;

	private Label lblMonday;

	private Label lblTuesday;

	private Label lblWednesday;

	private Label lblThursday;

	private Label lblFriday;

	private Label lblSaturday;

	private Label lblFrom;

	private ComboBox comboFromHour;

	private ComboBox comboFromMinute;

	private ComboBox comboToMinute;

	private ComboBox comboToHour;

	private Label lblTo;

	public DayOfWeekDateTimeConditionEditorRowControl(DayOfWeekDateTimeCondition condition)
	{
		InitializeComponent();
		this.condition = condition;
		lblSunday.Text = LocalizedResourceMgr.GetString("DayOfWeekDateTimeConditionEditorRowControl.lblSunday.Text");
		lblMonday.Text = LocalizedResourceMgr.GetString("DayOfWeekDateTimeConditionEditorRowControl.lblMonday.Text");
		lblTuesday.Text = LocalizedResourceMgr.GetString("DayOfWeekDateTimeConditionEditorRowControl.lblTuesday.Text");
		lblWednesday.Text = LocalizedResourceMgr.GetString("DayOfWeekDateTimeConditionEditorRowControl.lblWednesday.Text");
		lblThursday.Text = LocalizedResourceMgr.GetString("DayOfWeekDateTimeConditionEditorRowControl.lblThursday.Text");
		lblFriday.Text = LocalizedResourceMgr.GetString("DayOfWeekDateTimeConditionEditorRowControl.lblFriday.Text");
		lblSaturday.Text = LocalizedResourceMgr.GetString("DayOfWeekDateTimeConditionEditorRowControl.lblSaturday.Text");
		lblFrom.Text = LocalizedResourceMgr.GetString("DayOfWeekDateTimeConditionEditorRowControl.lblFrom.Text");
		lblTo.Text = LocalizedResourceMgr.GetString("DayOfWeekDateTimeConditionEditorRowControl.lblTo.Text");
		chkSunday.Checked = condition.SundayChecked;
		chkMonday.Checked = condition.MondayChecked;
		chkTuesday.Checked = condition.TuesdayChecked;
		chkWednesday.Checked = condition.WednesdayChecked;
		chkThursday.Checked = condition.ThursdayChecked;
		chkFriday.Checked = condition.FridayChecked;
		chkSaturday.Checked = condition.SaturdayChecked;
		comboFromHour.SelectedIndex = condition.HourFrom;
		comboFromMinute.SelectedIndex = condition.MinuteFrom;
		comboToHour.SelectedIndex = condition.HourTo;
		comboToMinute.SelectedIndex = condition.MinuteTo;
	}

	public override DateTimeCondition Save()
	{
		condition.SetValues(chkSunday.Checked, chkMonday.Checked, chkTuesday.Checked, chkWednesday.Checked, chkThursday.Checked, chkFriday.Checked, chkSaturday.Checked, comboFromHour.SelectedIndex, comboFromMinute.SelectedIndex, comboToHour.SelectedIndex, comboToMinute.SelectedIndex);
		return condition;
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
		this.chkSunday = new System.Windows.Forms.CheckBox();
		this.chkMonday = new System.Windows.Forms.CheckBox();
		this.chkTuesday = new System.Windows.Forms.CheckBox();
		this.chkWednesday = new System.Windows.Forms.CheckBox();
		this.chkSaturday = new System.Windows.Forms.CheckBox();
		this.chkFriday = new System.Windows.Forms.CheckBox();
		this.chkThursday = new System.Windows.Forms.CheckBox();
		this.lblSunday = new System.Windows.Forms.Label();
		this.lblMonday = new System.Windows.Forms.Label();
		this.lblTuesday = new System.Windows.Forms.Label();
		this.lblWednesday = new System.Windows.Forms.Label();
		this.lblThursday = new System.Windows.Forms.Label();
		this.lblFriday = new System.Windows.Forms.Label();
		this.lblSaturday = new System.Windows.Forms.Label();
		this.lblFrom = new System.Windows.Forms.Label();
		this.comboFromHour = new System.Windows.Forms.ComboBox();
		this.comboFromMinute = new System.Windows.Forms.ComboBox();
		this.comboToMinute = new System.Windows.Forms.ComboBox();
		this.comboToHour = new System.Windows.Forms.ComboBox();
		this.lblTo = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.chkSunday.AutoSize = true;
		this.chkSunday.Location = new System.Drawing.Point(6, 22);
		this.chkSunday.Name = "chkSunday";
		this.chkSunday.Size = new System.Drawing.Size(15, 14);
		this.chkSunday.TabIndex = 2;
		this.chkSunday.UseVisualStyleBackColor = true;
		this.chkMonday.AutoSize = true;
		this.chkMonday.Location = new System.Drawing.Point(30, 22);
		this.chkMonday.Name = "chkMonday";
		this.chkMonday.Size = new System.Drawing.Size(15, 14);
		this.chkMonday.TabIndex = 4;
		this.chkMonday.UseVisualStyleBackColor = true;
		this.chkTuesday.AutoSize = true;
		this.chkTuesday.Location = new System.Drawing.Point(54, 22);
		this.chkTuesday.Name = "chkTuesday";
		this.chkTuesday.Size = new System.Drawing.Size(15, 14);
		this.chkTuesday.TabIndex = 6;
		this.chkTuesday.UseVisualStyleBackColor = true;
		this.chkWednesday.AutoSize = true;
		this.chkWednesday.Location = new System.Drawing.Point(78, 22);
		this.chkWednesday.Name = "chkWednesday";
		this.chkWednesday.Size = new System.Drawing.Size(15, 14);
		this.chkWednesday.TabIndex = 8;
		this.chkWednesday.UseVisualStyleBackColor = true;
		this.chkSaturday.AutoSize = true;
		this.chkSaturday.Location = new System.Drawing.Point(150, 22);
		this.chkSaturday.Name = "chkSaturday";
		this.chkSaturday.Size = new System.Drawing.Size(15, 14);
		this.chkSaturday.TabIndex = 14;
		this.chkSaturday.UseVisualStyleBackColor = true;
		this.chkFriday.AutoSize = true;
		this.chkFriday.Location = new System.Drawing.Point(126, 22);
		this.chkFriday.Name = "chkFriday";
		this.chkFriday.Size = new System.Drawing.Size(15, 14);
		this.chkFriday.TabIndex = 12;
		this.chkFriday.UseVisualStyleBackColor = true;
		this.chkThursday.AutoSize = true;
		this.chkThursday.Location = new System.Drawing.Point(102, 22);
		this.chkThursday.Name = "chkThursday";
		this.chkThursday.Size = new System.Drawing.Size(15, 14);
		this.chkThursday.TabIndex = 10;
		this.chkThursday.UseVisualStyleBackColor = true;
		this.lblSunday.AutoSize = true;
		this.lblSunday.Location = new System.Drawing.Point(6, 6);
		this.lblSunday.Name = "lblSunday";
		this.lblSunday.Size = new System.Drawing.Size(14, 13);
		this.lblSunday.TabIndex = 1;
		this.lblSunday.Text = "S";
		this.lblMonday.AutoSize = true;
		this.lblMonday.Location = new System.Drawing.Point(30, 6);
		this.lblMonday.Name = "lblMonday";
		this.lblMonday.Size = new System.Drawing.Size(16, 13);
		this.lblMonday.TabIndex = 3;
		this.lblMonday.Text = "M";
		this.lblTuesday.AutoSize = true;
		this.lblTuesday.Location = new System.Drawing.Point(54, 6);
		this.lblTuesday.Name = "lblTuesday";
		this.lblTuesday.Size = new System.Drawing.Size(14, 13);
		this.lblTuesday.TabIndex = 5;
		this.lblTuesday.Text = "T";
		this.lblWednesday.AutoSize = true;
		this.lblWednesday.Location = new System.Drawing.Point(78, 6);
		this.lblWednesday.Name = "lblWednesday";
		this.lblWednesday.Size = new System.Drawing.Size(18, 13);
		this.lblWednesday.TabIndex = 7;
		this.lblWednesday.Text = "W";
		this.lblThursday.AutoSize = true;
		this.lblThursday.Location = new System.Drawing.Point(102, 6);
		this.lblThursday.Name = "lblThursday";
		this.lblThursday.Size = new System.Drawing.Size(14, 13);
		this.lblThursday.TabIndex = 9;
		this.lblThursday.Text = "T";
		this.lblFriday.AutoSize = true;
		this.lblFriday.Location = new System.Drawing.Point(126, 6);
		this.lblFriday.Name = "lblFriday";
		this.lblFriday.Size = new System.Drawing.Size(13, 13);
		this.lblFriday.TabIndex = 11;
		this.lblFriday.Text = "F";
		this.lblSaturday.AutoSize = true;
		this.lblSaturday.Location = new System.Drawing.Point(150, 6);
		this.lblSaturday.Name = "lblSaturday";
		this.lblSaturday.Size = new System.Drawing.Size(14, 13);
		this.lblSaturday.TabIndex = 13;
		this.lblSaturday.Text = "S";
		this.lblFrom.AutoSize = true;
		this.lblFrom.Location = new System.Drawing.Point(211, 6);
		this.lblFrom.Name = "lblFrom";
		this.lblFrom.Size = new System.Drawing.Size(30, 13);
		this.lblFrom.TabIndex = 15;
		this.lblFrom.Text = "From";
		this.comboFromHour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboFromHour.FormattingEnabled = true;
		this.comboFromHour.Items.AddRange(new object[24]
		{
			"00", "01", "02", "03", "04", "05", "06", "07", "08", "09",
			"10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
			"20", "21", "22", "23"
		});
		this.comboFromHour.Location = new System.Drawing.Point(214, 22);
		this.comboFromHour.Name = "comboFromHour";
		this.comboFromHour.Size = new System.Drawing.Size(38, 21);
		this.comboFromHour.TabIndex = 16;
		this.comboFromMinute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboFromMinute.FormattingEnabled = true;
		this.comboFromMinute.Items.AddRange(new object[60]
		{
			"00", "01", "02", "03", "04", "05", "06", "07", "08", "09",
			"10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
			"20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
			"30", "31", "32", "33", "34", "35", "36", "37", "38", "39",
			"40", "41", "42", "43", "44", "45", "46", "47", "48", "49",
			"50", "51", "52", "53", "54", "55", "56", "57", "58", "59"
		});
		this.comboFromMinute.Location = new System.Drawing.Point(258, 22);
		this.comboFromMinute.Name = "comboFromMinute";
		this.comboFromMinute.Size = new System.Drawing.Size(38, 21);
		this.comboFromMinute.TabIndex = 17;
		this.comboToMinute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboToMinute.FormattingEnabled = true;
		this.comboToMinute.Items.AddRange(new object[60]
		{
			"00", "01", "02", "03", "04", "05", "06", "07", "08", "09",
			"10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
			"20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
			"30", "31", "32", "33", "34", "35", "36", "37", "38", "39",
			"40", "41", "42", "43", "44", "45", "46", "47", "48", "49",
			"50", "51", "52", "53", "54", "55", "56", "57", "58", "59"
		});
		this.comboToMinute.Location = new System.Drawing.Point(359, 22);
		this.comboToMinute.Name = "comboToMinute";
		this.comboToMinute.Size = new System.Drawing.Size(38, 21);
		this.comboToMinute.TabIndex = 20;
		this.comboToHour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboToHour.FormattingEnabled = true;
		this.comboToHour.Items.AddRange(new object[24]
		{
			"00", "01", "02", "03", "04", "05", "06", "07", "08", "09",
			"10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
			"20", "21", "22", "23"
		});
		this.comboToHour.Location = new System.Drawing.Point(315, 22);
		this.comboToHour.Name = "comboToHour";
		this.comboToHour.Size = new System.Drawing.Size(38, 21);
		this.comboToHour.TabIndex = 19;
		this.lblTo.AutoSize = true;
		this.lblTo.Location = new System.Drawing.Point(312, 6);
		this.lblTo.Name = "lblTo";
		this.lblTo.Size = new System.Drawing.Size(20, 13);
		this.lblTo.TabIndex = 18;
		this.lblTo.Text = "To";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.comboToMinute);
		base.Controls.Add(this.comboToHour);
		base.Controls.Add(this.lblTo);
		base.Controls.Add(this.comboFromMinute);
		base.Controls.Add(this.comboFromHour);
		base.Controls.Add(this.lblFrom);
		base.Controls.Add(this.lblSaturday);
		base.Controls.Add(this.lblFriday);
		base.Controls.Add(this.lblThursday);
		base.Controls.Add(this.lblWednesday);
		base.Controls.Add(this.lblTuesday);
		base.Controls.Add(this.lblMonday);
		base.Controls.Add(this.lblSunday);
		base.Controls.Add(this.chkSaturday);
		base.Controls.Add(this.chkFriday);
		base.Controls.Add(this.chkThursday);
		base.Controls.Add(this.chkWednesday);
		base.Controls.Add(this.chkTuesday);
		base.Controls.Add(this.chkMonday);
		base.Controls.Add(this.chkSunday);
		base.Name = "DayOfWeekDateTimeConditionEditorRowControl";
		base.Size = new System.Drawing.Size(409, 49);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
