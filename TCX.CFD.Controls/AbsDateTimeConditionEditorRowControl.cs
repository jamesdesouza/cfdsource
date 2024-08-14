using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TCX.CFD.Classes.Components;

namespace TCX.CFD.Controls;

public class AbsDateTimeConditionEditorRowControl : UserControl
{
	private IContainer components;

	public AbsDateTimeConditionEditorRowControl()
	{
		InitializeComponent();
	}

	public virtual DateTimeCondition Save()
	{
		throw new NotImplementedException("Save method not implemented");
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
		base.Name = "AbsDateTimeConditionEditorRowControl";
		base.Size = new System.Drawing.Size(418, 27);
		base.ResumeLayout(false);
	}
}
