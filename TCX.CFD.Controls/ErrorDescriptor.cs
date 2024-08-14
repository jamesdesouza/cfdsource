using System.ComponentModel;
using System.Windows.Forms;
using System.Workflow.ComponentModel;
using TCX.CFD.Classes;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Controls;

public class ErrorDescriptor
{
	private static int currentNumber;

	private int number;

	public CompilerMessageTypes ErrorType { get; private set; }

	public string Description { get; private set; }

	public FileObject FileObject { get; private set; }

	public FlowTypes FlowType { get; private set; }

	public Activity ErrorActivity { get; private set; }

	private string ErrorTypeAsString()
	{
		return ErrorType switch
		{
			CompilerMessageTypes.Error => LocalizedResourceMgr.GetString("ErrorListControl.ErrorTypes.Error"), 
			CompilerMessageTypes.Warning => LocalizedResourceMgr.GetString("ErrorListControl.ErrorTypes.Warning"), 
			CompilerMessageTypes.Message => LocalizedResourceMgr.GetString("ErrorListControl.ErrorTypes.Message"), 
			_ => throw new InvalidEnumArgumentException(string.Format(LocalizedResourceMgr.GetString("ErrorListControl.Error.InvalidErrorType"), ErrorType)), 
		};
	}

	private void Initialize(CompilerMessageTypes errorType, string description, FileObject fileObject, FlowTypes flowType, Activity errorActivity)
	{
		number = currentNumber++;
		ErrorType = errorType;
		Description = description;
		FileObject = fileObject;
		FlowType = flowType;
		ErrorActivity = errorActivity;
	}

	public ErrorDescriptor(CompilerMessageTypes errorType, string description)
	{
		Initialize(errorType, description, null, FlowTypes.MainFlow, null);
	}

	public ErrorDescriptor(CompilerMessageTypes errorType, string description, FileObject fileObject, FlowTypes flowType, Activity errorActivity)
	{
		Initialize(errorType, description, fileObject, flowType, errorActivity);
	}

	public static void Reset()
	{
		currentNumber = 0;
	}

	public ListViewItem ToListViewItem()
	{
		return new ListViewItem(new string[4]
		{
			number.ToString(),
			ErrorTypeAsString(),
			Description,
			(FileObject == null) ? string.Empty : FileObject.Name
		}, (ErrorType != 0) ? ((ErrorType == CompilerMessageTypes.Warning) ? 1 : 2) : 0)
		{
			Tag = this
		};
	}

	public string ToCopiedText()
	{
		return string.Format("{0}: {1}{2}", ErrorTypeAsString(), Description, (FileObject == null) ? string.Empty : (" (file " + FileObject.Name + ")"));
	}
}
