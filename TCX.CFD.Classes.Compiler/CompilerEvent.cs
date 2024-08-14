using TCX.CFD.Controls;

namespace TCX.CFD.Classes.Compiler;

public class CompilerEvent
{
	public CompilerEventTypes Type { get; }

	public bool AddNewLineEnding { get; }

	public string Message { get; }

	public string Link { get; }

	public ErrorDescriptor ErrorDescriptor { get; }

	public CompilerEvent(string message, bool addNewLineEnding = true)
	{
		Type = CompilerEventTypes.OutputWindowText;
		AddNewLineEnding = addNewLineEnding;
		Message = message;
		Link = string.Empty;
		ErrorDescriptor = null;
	}

	public CompilerEvent(string message, string link, bool addNewLineEnding = true)
	{
		Type = CompilerEventTypes.OutputWindowLink;
		AddNewLineEnding = addNewLineEnding;
		Message = message;
		Link = link;
		ErrorDescriptor = null;
	}

	public CompilerEvent(ErrorDescriptor errorDescriptor, bool addNewLineEnding = true)
	{
		Type = CompilerEventTypes.ErrorDescriptor;
		AddNewLineEnding = addNewLineEnding;
		Message = string.Empty;
		Link = string.Empty;
		ErrorDescriptor = errorDescriptor;
	}
}
