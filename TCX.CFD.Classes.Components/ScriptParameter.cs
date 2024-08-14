namespace TCX.CFD.Classes.Components;

public class ScriptParameter : Parameter
{
	public ScriptParameterTypes Type { get; set; }

	public ScriptParameter()
	{
	}

	public ScriptParameter(string parameterName, string parameterValue, ScriptParameterTypes parameterType)
		: base(parameterName, parameterValue)
	{
		Type = parameterType;
	}
}
