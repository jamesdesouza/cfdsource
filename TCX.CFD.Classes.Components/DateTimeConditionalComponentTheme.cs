using System.Drawing;
using System.Drawing.Drawing2D;
using System.Workflow.ComponentModel.Design;

namespace TCX.CFD.Classes.Components;

public class DateTimeConditionalComponentTheme : CompositeDesignerTheme
{
	public DateTimeConditionalComponentTheme(WorkflowTheme theme)
		: base(theme)
	{
		ShowDropShadow = true;
		ConnectorStartCap = LineAnchor.None;
		ConnectorEndCap = LineAnchor.None;
		BorderStyle = DashStyle.Dash;
		BackColorStart = Color.FromArgb(243, 243, 243);
		BackColorEnd = Color.FromArgb(243, 243, 243);
		BackgroundStyle = LinearGradientMode.Horizontal;
		base.ContainingTheme.AmbientTheme.CommentIndicatorColor = Color.Gray;
	}
}
