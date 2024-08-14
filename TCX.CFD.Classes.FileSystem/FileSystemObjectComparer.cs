using System.Collections.Generic;

namespace TCX.CFD.Classes.FileSystem;

public class FileSystemObjectComparer : IComparer<AbsFileSystemObject>
{
	public int Compare(AbsFileSystemObject x, AbsFileSystemObject y)
	{
		return x.Name.CompareTo(y.Name);
	}
}
