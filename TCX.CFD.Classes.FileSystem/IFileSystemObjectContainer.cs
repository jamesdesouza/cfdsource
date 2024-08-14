namespace TCX.CFD.Classes.FileSystem;

public interface IFileSystemObjectContainer
{
	FolderObject AddFolder(string folderName);

	DialerFileObject AddDialer(string fileName);

	CallflowFileObject AddCallflow(string fileName);

	ComponentFileObject AddComponent(string fileName);

	void AddChild(AbsFileSystemObject fso);

	void RemoveChild(AbsFileSystemObject fso);

	bool ChildExists(string fileName);

	void NotifyChildChange();

	string GetFolderPath();

	string GetRelativeFolderPath();

	ProjectObject GetProjectObject();
}
