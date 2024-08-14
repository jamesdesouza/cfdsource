using System.ComponentModel;
using System.Drawing.Design;
using TCX.CFD.Classes.Editors;

namespace TCX.CFD.Classes.Components;

public class Parameter
{
	public delegate void NameChangingEventHandler(object sender, NameChangingEventArgs e);

	public delegate void NameChangedEventHandler(object sender, NameChangedEventArgs e);

	private IVadActivity containerActivity;

	private string parameterName;

	private string parameterValue;

	public string Name
	{
		get
		{
			return parameterName;
		}
		set
		{
			if (value != parameterName)
			{
				NameChangingEventArgs nameChangingEventArgs = new NameChangingEventArgs(parameterName, value);
				this.NameChanging?.Invoke(this, nameChangingEventArgs);
				if (!nameChangingEventArgs.Cancel)
				{
					string oldValue = parameterName;
					parameterName = value;
					this.NameChanged?.Invoke(this, new NameChangedEventArgs(oldValue, parameterName));
				}
			}
		}
	}

	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string Value
	{
		get
		{
			return parameterValue;
		}
		set
		{
			parameterValue = value;
		}
	}

	public event NameChangingEventHandler NameChanging;

	public event NameChangedEventHandler NameChanged;

	public Parameter()
	{
		parameterName = string.Empty;
		parameterValue = string.Empty;
	}

	public Parameter(string parameterName, string parameterValue)
	{
		this.parameterName = parameterName;
		this.parameterValue = parameterValue;
	}

	public IVadActivity GetContainerActivity()
	{
		return containerActivity;
	}

	public void SetContainerActivity(IVadActivity containerActivity)
	{
		this.containerActivity = containerActivity;
	}

	public bool IsNameChangingHandled()
	{
		return this.NameChanging != null;
	}

	public bool IsNameChangedHandled()
	{
		return this.NameChanged != null;
	}
}
