using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using TCX.CFD.Classes.Components;

namespace TCX.CFD.Classes.Editors;

public abstract class AbsFileTypeConverter : StringConverter
{
	protected abstract IVadActivity getActivity(ITypeDescriptorContext context);

	protected abstract string getFileFolder();

	protected abstract string[] getFileSearchPatterns();

	public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
	{
		return true;
	}

	public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
	{
		return false;
	}

	public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
	{
		List<string> list = new List<string>();
		IVadActivity activity = getActivity(context);
		if (activity != null)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(activity.GetRootFlow().FileObject.GetProjectObject().GetFolderPath(), getFileFolder()));
			if (directoryInfo.Exists)
			{
				string[] fileSearchPatterns = getFileSearchPatterns();
				foreach (string searchPattern in fileSearchPatterns)
				{
					FileInfo[] files = directoryInfo.GetFiles(searchPattern, SearchOption.TopDirectoryOnly);
					foreach (FileInfo fileInfo in files)
					{
						list.Add(fileInfo.Name);
					}
				}
			}
		}
		list.Sort();
		list.Add(LocalizedResourceMgr.GetString("FileTypeConverters.Browse.Text"));
		return new StandardValuesCollection(list);
	}
}
