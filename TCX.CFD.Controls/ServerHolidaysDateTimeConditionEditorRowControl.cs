using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes.Components;

namespace TCX.CFD.Controls;

public class ServerHolidaysDateTimeConditionEditorRowControl : AbsDateTimeConditionEditorRowControl
{
	private IContainer components;

	public ServerHolidaysDateTimeConditionEditorRowControl()
	{
		InitializeComponent();
	}

	public override DateTimeCondition Save()
	{
		return new ServerHolidaysDateTimeCondition();
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
		base.SuspendLayout();
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "ServerHolidaysDateTimeConditionEditorRowControl";
		base.Size = new System.Drawing.Size(409, 49);
		base.ResumeLayout(false);
	}
}
