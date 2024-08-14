using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Forms;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.Components;

[ActivityDesignerTheme(typeof(LoopComponentTheme))]
public class LoopComponentDesigner : SequenceComponentDesigner
{
	protected override void Initialize(Activity activity)
	{
		ActivityDesignerVerb activityDesignerVerb = new ActivityDesignerVerb(this, DesignerVerbGroup.General, LocalizedResourceMgr.GetString("ComponentDesigners.LoopComponent.ConfigureVerb"), OnConfigure);
		activityDesignerVerb.Properties.Add("Image", Resources.EditConfiguration);
		verbs.Add(activityDesignerVerb);
		base.Initialize(activity);
	}

	private void OnConfigure(object sender, EventArgs e)
	{
		if (!(base.Activity is LoopComponent loopComponent))
		{
			return;
		}
		ConditionConfigurationForm conditionConfigurationForm = new ConditionConfigurationForm(loopComponent);
		if (conditionConfigurationForm.ShowDialog() == DialogResult.OK)
		{
			using (DesignerTransaction designerTransaction = (GetService(typeof(IDesignerHost)) as IDesignerHost).CreateTransaction("Configuration form changes for component " + base.Activity.Name))
			{
				TypeDescriptor.GetProperties(base.Activity)["Condition"].SetValue(base.Activity, conditionConfigurationForm.Condition);
				designerTransaction.Commit();
			}
		}
	}

	protected override void DoDefaultAction()
	{
		base.DoDefaultAction();
		OnConfigure(null, EventArgs.Empty);
	}

	protected override void PostFilterProperties(IDictionary properties)
	{
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Name", "Loop", "Please specify the name of the component. It has to be unique in the entire flow.");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Enabled", "General");
		ComponentDesignerHelper.UpdateExistingProperty(properties, "Description", "General");
		base.PostFilterProperties(properties);
	}

	protected override void OnPaint(ActivityDesignerPaintEventArgs e)
	{
		base.OnPaint(e);
		if (Expanded && e.DesignerTheme is CompositeDesignerTheme compositeDesignerTheme)
		{
			Rectangle bounds = base.Bounds;
			Rectangle textRectangle = TextRectangle;
			Rectangle ımageRectangle = ImageRectangle;
			Point point = ((!ımageRectangle.IsEmpty) ? new Point(ımageRectangle.Right + e.AmbientTheme.Margin.Width / 2, ımageRectangle.Top + ımageRectangle.Height / 2) : (textRectangle.IsEmpty ? new Point(bounds.Right - bounds.Width / 2 - e.AmbientTheme.Margin.Width / 2, bounds.Top + e.AmbientTheme.Margin.Height / 2) : new Point(textRectangle.Right + e.AmbientTheme.Margin.Width / 2, textRectangle.Top + textRectangle.Height / 2)));
			Point[] array = new Point[4];
			array[0].X = bounds.Right - bounds.Width / 2;
			array[0].Y = bounds.Bottom - compositeDesignerTheme.ConnectorSize.Height / 3;
			array[1].X = bounds.Right - compositeDesignerTheme.ConnectorSize.Width / 3;
			array[1].Y = bounds.Bottom - compositeDesignerTheme.ConnectorSize.Height / 3;
			array[2].X = bounds.Right - compositeDesignerTheme.ConnectorSize.Width / 3;
			array[2].Y = point.Y;
			array[3].X = point.X;
			array[3].Y = point.Y;
			DrawConnectors(e.Graphics, compositeDesignerTheme.ForegroundPen, array, LineAnchor.None, LineAnchor.ArrowAnchor);
			DrawConnectors(e.Graphics, compositeDesignerTheme.ForegroundPen, new Point[2]
			{
				array[0],
				new Point(bounds.Left + bounds.Width / 2, bounds.Bottom)
			}, LineAnchor.None, LineAnchor.None);
		}
	}
}
