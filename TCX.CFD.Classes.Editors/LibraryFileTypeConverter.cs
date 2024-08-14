using System.ComponentModel;
using TCX.CFD.Classes.Components;

namespace TCX.CFD.Classes.Editors;

public class LibraryFileTypeConverter : AbsFileTypeConverter
{
	protected override IVadActivity getActivity(ITypeDescriptorContext context)
	{
		return context.Instance as IVadActivity;
	}

	protected override string getFileFolder()
	{
		return "Libraries";
	}

	protected override string[] getFileSearchPatterns()
	{
		return new string[1] { "*.cs" };
	}
}
