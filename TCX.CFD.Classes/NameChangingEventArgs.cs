using System.ComponentModel;

namespace TCX.CFD.Classes;

public class NameChangingEventArgs : CancelEventArgs
{
	private string oldValue;

	private string newValue;

	public string OldValue => oldValue;

	public string NewValue => newValue;

	public NameChangingEventArgs(string oldValue, string newValue)
	{
		this.oldValue = oldValue;
		this.newValue = newValue;
	}
}
