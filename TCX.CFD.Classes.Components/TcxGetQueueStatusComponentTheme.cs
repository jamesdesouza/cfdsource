using System.Drawing;
using System.Drawing.Drawing2D;
using System.Workflow.ComponentModel.Design;

namespace TCX.CFD.Classes.Components;

public class TcxGetQueueStatusComponentTheme : ActivityDesignerTheme
{
	public TcxGetQueueStatusComponentTheme(WorkflowTheme theme)
		: base(theme)
	{
		BackColorStart = Color.FromArgb(243, 243, 243);
		BackColorEnd = Color.FromArgb(243, 243, 243);
		BackgroundStyle = LinearGradientMode.Horizontal;
		base.ContainingTheme.AmbientTheme.CommentIndicatorColor = Color.Gray;
	}
}
