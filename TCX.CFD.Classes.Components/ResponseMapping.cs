using System.ComponentModel;
using System.Drawing.Design;
using TCX.CFD.Classes.Editors;

namespace TCX.CFD.Classes.Components;

public class ResponseMapping
{
	private IVadActivity containerActivity;

	public string Path { get; set; }

	[Editor(typeof(LeftHandSideVariableEditor), typeof(UITypeEditor))]
	public string Variable { get; set; }

	public ResponseMapping()
	{
		Path = string.Empty;
		Variable = string.Empty;
	}

	public ResponseMapping(string path, string variable)
	{
		Path = path;
		Variable = variable;
	}

	public IVadActivity GetContainerActivity()
	{
		return containerActivity;
	}

	public void SetContainerActivity(IVadActivity containerActivity)
	{
		this.containerActivity = containerActivity;
	}
}
