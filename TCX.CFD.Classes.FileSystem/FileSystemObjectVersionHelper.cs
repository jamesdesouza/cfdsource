using System;

namespace TCX.CFD.Classes.FileSystem;

public class FileSystemObjectVersionHelper
{
	private FileSystemObjectVersionHelper()
	{
	}

	public static bool IsValidVersion(string s)
	{
		switch (s)
		{
		case "1.0":
		case "1.1":
		case "2.0":
		case "2.1":
			return true;
		default:
			return false;
		}
	}

	public static FileSystemObjectVersions FromString(string s)
	{
		return s switch
		{
			"1.0" => FileSystemObjectVersions.V1_0, 
			"1.1" => FileSystemObjectVersions.V1_1, 
			"2.0" => FileSystemObjectVersions.V2_0, 
			"2.1" => FileSystemObjectVersions.V2_1, 
			_ => throw new ArgumentException("Invalid FileSystemObjectVersions representation: '" + s + "' - it must be '1.0', '1.1', '2.0' or '2.1'."), 
		};
	}

	public static string ToString(FileSystemObjectVersions e)
	{
		return e switch
		{
			FileSystemObjectVersions.V1_0 => "1.0", 
			FileSystemObjectVersions.V1_1 => "1.1", 
			FileSystemObjectVersions.V2_0 => "2.0", 
			FileSystemObjectVersions.V2_1 => "2.1", 
			_ => throw new ArgumentException("Invalid FileSystemObjectVersions: " + e), 
		};
	}
}
