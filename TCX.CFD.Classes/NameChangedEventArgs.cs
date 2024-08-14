using System;

namespace TCX.CFD.Classes;

public class NameChangedEventArgs : EventArgs
{
	private string oldValue;

	private string newValue;

	public string OldValue => oldValue;

	public string NewValue => newValue;

	public NameChangedEventArgs(string oldValue, string newValue)
	{
		this.oldValue = oldValue;
		this.newValue = newValue;
	}
}
