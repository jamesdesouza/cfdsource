using System.Drawing;
using System.Drawing.Drawing2D;
using System.Workflow.ComponentModel.Design;

namespace TCX.CFD.Classes.Components;

public class LoopComponentTheme : CompositeDesignerTheme
{
	public LoopComponentTheme(WorkflowTheme theme)
		: base(theme)
	{
		ShowDropShadow = true;
		ConnectorStartCap = LineAnchor.None;
		ConnectorEndCap = LineAnchor.None;
		BorderStyle = DashStyle.Dash;
		BackColorStart = Color.FromArgb(231, 231, 231);
		BackColorEnd = Color.FromArgb(231, 231, 231);
		BackgroundStyle = LinearGradientMode.Horizontal;
		base.ContainingTheme.AmbientTheme.CommentIndicatorColor = Color.Gray;
	}
}
