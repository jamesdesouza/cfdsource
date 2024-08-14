using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Components;

namespace TCX.CFD.Controls;

public class SpecificDayDateTimeConditionEditorRowControl : AbsDateTimeConditionEditorRowControl
{
	private SpecificDayDateTimeCondition condition;

	private IContainer components;

	private Label lblFrom;

	private ComboBox comboFromHour;

	private ComboBox comboFromMinute;

	private ComboBox comboToMinute;

	private ComboBox comboToHour;

	private Label lblTo;

	private Label lblDate;

	private DateTimePicker dateTimePicker;

	public SpecificDayDateTimeConditionEditorRowControl(SpecificDayDateTimeCondition condition)
	{
		InitializeComponent();
		this.condition = condition;
		lblDate.Text = LocalizedResourceMgr.GetString("SpecificDayDateTimeConditionEditorRowControl.lblDate.Text");
		lblFrom.Text = LocalizedResourceMgr.GetString("SpecificDayDateTimeConditionEditorRowControl.lblFrom.Text");
		lblTo.Text = LocalizedResourceMgr.GetString("SpecificDayDateTimeConditionEditorRowControl.lblTo.Text");
		dateTimePicker.Value = condition.SpecificDay;
		comboFromHour.SelectedIndex = condition.HourFrom;
		comboFromMinute.SelectedIndex = condition.MinuteFrom;
		comboToHour.SelectedIndex = condition.HourTo;
		comboToMinute.SelectedIndex = condition.MinuteTo;
	}

	public override DateTimeCondition Save()
	{
		condition.SetValues(dateTimePicker.Value, comboFromHour.SelectedIndex, comboFromMinute.SelectedIndex, comboToHour.SelectedIndex, comboToMinute.SelectedIndex);
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
		this.lblFrom = new System.Windows.Forms.Label();
		this.comboFromHour = new System.Windows.Forms.ComboBox();
		this.comboFromMinute = new System.Windows.Forms.ComboBox();
		this.comboToMinute = new System.Windows.Forms.ComboBox();
		this.comboToHour = new System.Windows.Forms.ComboBox();
		this.lblTo = new System.Windows.Forms.Label();
		this.lblDate = new System.Windows.Forms.Label();
		this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
		base.SuspendLayout();
		this.lblFrom.AutoSize = true;
		this.lblFrom.Location = new System.Drawing.Point(211, 6);
		this.lblFrom.Name = "lblFrom";
		this.lblFrom.Size = new System.Drawing.Size(30, 13);
		this.lblFrom.TabIndex = 2;
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
		this.comboFromHour.TabIndex = 3;
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
		this.comboFromMinute.TabIndex = 4;
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
		this.comboToMinute.TabIndex = 7;
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
		this.comboToHour.TabIndex = 6;
		this.lblTo.AutoSize = true;
		this.lblTo.Location = new System.Drawing.Point(312, 6);
		this.lblTo.Name = "lblTo";
		this.lblTo.Size = new System.Drawing.Size(20, 13);
		this.lblTo.TabIndex = 5;
		this.lblTo.Text = "To";
		this.lblDate.AutoSize = true;
		this.lblDate.Location = new System.Drawing.Point(6, 6);
		this.lblDate.Name = "lblDate";
		this.lblDate.Size = new System.Drawing.Size(30, 13);
		this.lblDate.TabIndex = 0;
		this.lblDate.Text = "Date";
		this.dateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.dateTimePicker.Location = new System.Drawing.Point(9, 23);
		this.dateTimePicker.Name = "dateTimePicker";
		this.dateTimePicker.Size = new System.Drawing.Size(95, 20);
		this.dateTimePicker.TabIndex = 1;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.dateTimePicker);
		base.Controls.Add(this.lblDate);
		base.Controls.Add(this.comboToMinute);
		base.Controls.Add(this.comboToHour);
		base.Controls.Add(this.lblTo);
		base.Controls.Add(this.comboFromMinute);
		base.Controls.Add(this.comboFromHour);
		base.Controls.Add(this.lblFrom);
		base.Name = "SpecificDayDateTimeConditionEditorRowControl";
		base.Size = new System.Drawing.Size(409, 49);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
