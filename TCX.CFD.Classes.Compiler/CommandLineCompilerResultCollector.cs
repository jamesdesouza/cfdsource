using System;
using System.IO;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class CommandLineCompilerResultCollector : AbsCompilerResultCollector
{
	private readonly string buildLogFile;

	public override bool CancellationPending => false;

	public int ErrorCount { get; private set; }

	public CommandLineCompilerResultCollector(string buildLogFile)
	{
		this.buildLogFile = buildLogFile;
	}

	public override void ReportProgress(int progress, CompilerEvent compilerEvent)
	{
		switch (compilerEvent.Type)
		{
		case CompilerEventTypes.OutputWindowText:
			File.AppendAllText(buildLogFile, compilerEvent.Message);
			if (compilerEvent.AddNewLineEnding)
			{
				File.AppendAllText(buildLogFile, Environment.NewLine);
			}
			break;
		case CompilerEventTypes.OutputWindowLink:
		{
			string contents = compilerEvent.Message + " (" + compilerEvent.Link + ")";
			File.AppendAllText(buildLogFile, contents);
			if (compilerEvent.AddNewLineEnding)
			{
				File.AppendAllText(buildLogFile, Environment.NewLine);
			}
			break;
		}
		case CompilerEventTypes.ErrorDescriptor:
			File.AppendAllText(buildLogFile, compilerEvent.ErrorDescriptor.ToCopiedText());
			if (compilerEvent.AddNewLineEnding)
			{
				File.AppendAllText(buildLogFile, Environment.NewLine);
			}
			if (compilerEvent.ErrorDescriptor.ErrorType == CompilerMessageTypes.Error)
			{
				int errorCount = ErrorCount + 1;
				ErrorCount = errorCount;
			}
			break;
		}
	}

	public override bool AskForExtension(ProjectObject projectObject)
	{
		return true;
	}
}
