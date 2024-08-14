using System;
using System.IO;
using System.Workflow.ComponentModel;
using TCX.CFD.Classes.FileSystem;
using TCX.CFD.Controls;

namespace TCX.CFD.Classes.Compiler;

public static class CompilerHelper
{
	private static readonly Random random = new Random();

	public static int GetRandomId()
	{
		return random.Next();
	}

	public static void ReportCompilerMessage(AbsCompilerResultCollector compilerResultCollector, int progress, CompilerErrorCounter errorCounter, CompilerMessageTypes compilerMessageType, string description)
	{
		ReportCompilerMessage(compilerResultCollector, progress, errorCounter, compilerMessageType, description, null, FlowTypes.MainFlow, null);
	}

	public static void ReportCompilerMessage(AbsCompilerResultCollector compilerResultCollector, int progress, CompilerErrorCounter errorCounter, CompilerMessageTypes compilerMessageType, string description, FileObject fileObject, FlowTypes flowType, Activity errorActivity)
	{
		if (compilerMessageType == CompilerMessageTypes.Error)
		{
			errorCounter.IncrementErrorCount();
		}
		compilerResultCollector.ReportProgress(progress, new CompilerEvent(new ErrorDescriptor(compilerMessageType, description, fileObject, flowType, errorActivity)));
	}

	public static void WriteOutput(string fileName, string content)
	{
		if (File.Exists(fileName))
		{
			throw new InvalidOperationException("FATAL - Compiler needs to write the output to an already existing file: " + fileName);
		}
		File.WriteAllText(fileName, content);
	}

	public static void WriteOutput(string fileName, UnmanagedMemoryStream inputStream)
	{
		using Stream stream = File.Create(fileName);
		byte[] array = new byte[8192];
		int count;
		while ((count = inputStream.Read(array, 0, array.Length)) > 0)
		{
			stream.Write(array, 0, count);
		}
	}
}
