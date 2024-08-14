using System;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.Compiler;

public class CompilerErrorCounter
{
	public uint ErrorCount { get; private set; }

	public CompilerErrorCounter()
	{
		ErrorCount = 0u;
	}

	public void IncrementErrorCount()
	{
		uint errorCount = ErrorCount + 1;
		ErrorCount = errorCount;
		if (ErrorCount >= Settings.Default.MaxErrorsBuildingProject)
		{
			throw new ApplicationException(LocalizedResourceMgr.GetString("Compiler.TooManyErrors"));
		}
	}
}
