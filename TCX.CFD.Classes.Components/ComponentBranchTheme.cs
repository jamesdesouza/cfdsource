using System.Drawing;
using System.Drawing.Drawing2D;
using System.Workflow.ComponentModel.Design;

namespace TCX.CFD.Classes.Components;

public class ComponentBranchTheme : CompositeDesignerTheme
{
	public ComponentBranchTheme(WorkflowTheme theme)
		: base(theme)
	{
		ShowDropShadow = true;
		ConnectorStartCap = LineAnchor.None;
		ConnectorEndCap = LineAnchor.None;
		BorderStyle = DashStyle.Dash;
		BackColorStart = Color.FromArgb(219, 219, 219);
		BackColorEnd = Color.FromArgb(219, 219, 219);
		BackgroundStyle = LinearGradientMode.Horizontal;
		base.ContainingTheme.AmbientTheme.CommentIndicatorColor = Color.Gray;
	}
}
