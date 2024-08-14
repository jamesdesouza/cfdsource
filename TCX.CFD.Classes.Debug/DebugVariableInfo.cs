namespace TCX.CFD.Classes.Debug;

public class DebugVariableInfo
{
	private string variableName;

	public string VariableName
	{
		get
		{
			return variableName;
		}
		set
		{
			variableName = value;
		}
	}

	public DebugVariableInfo(string variableName)
	{
		this.variableName = variableName;
	}
}
