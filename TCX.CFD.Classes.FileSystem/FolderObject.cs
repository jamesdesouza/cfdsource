using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using TCX.CFD.Controls;

namespace TCX.CFD.Classes.FileSystem;

public class FolderObject : AbsFileSystemObject, IFileSystemObjectContainer
{
	private DirectoryInfo directoryInfo;

	[Category("Folder")]
	[Description("The name of the folder.")]
	public override string Name
	{
		get
		{
			return directoryInfo.Name;
		}
		set
		{
			if (NameHelper.IsValidName(value))
			{
				if (directoryInfo.Exists)
				{
					string text = System.IO.Path.Combine(directoryInfo.Parent.FullName, value);
					directoryInfo.MoveTo(text);
					relativePath = System.IO.Path.Combine(parent.GetRelativeFolderPath(), value);
					directoryInfo = new DirectoryInfo(text);
					NotifyNameChanged();
					{
						foreach (AbsFileSystemObject children in ChildrenList)
						{
							children.OnParentPathChanged(this);
						}
						return;
					}
				}
				throw new InvalidOperationException(string.Format(LocalizedResourceMgr.GetString("FileSystemObjects.Error.MissingFolder"), base.Path));
			}
			throw new InvalidOperationException(string.Format(LocalizedResourceMgr.GetString("FileSystemObjects.Error.InvalidFolderName"), value));
		}
	}

	[Browsable(false)]
	public List<AbsFileSystemObject> ChildrenList { get; } = new List<AbsFileSystemObject>();


	public FolderObject(string projectDirectory, string relativePath, ProjectManagerControl projectManagerControl, IFileSystemObjectContainer parent)
		: base(projectDirectory, relativePath, projectManagerControl, parent)
	{
		directoryInfo = new DirectoryInfo(base.Path);
		if (!directoryInfo.Exists)
		{
			directoryInfo.Create();
			directoryInfo.Refresh();
		}
	}

	public FolderObject AddFolder(string folderName)
	{
		FolderObject folderObject = new FolderObject(projectDirectory, base.RelativePath + "\\" + folderName, projectManagerControl, this);
		ChildrenList.Add(folderObject);
		parent.NotifyChildChange();
		return folderObject;
	}

	public DialerFileObject AddDialer(string fileName)
	{
		DialerFileObject dialerFileObject = new DialerFileObject(projectDirectory, base.RelativePath + "\\" + fileName, projectManagerControl, this, create: true, DialerModes.PowerDialer, 30, 1, string.Empty, PredictiveDialerOptimizations.ForAgents);
		ChildrenList.Add(dialerFileObject);
		parent.NotifyChildChange();
		return dialerFileObject;
	}

	public CallflowFileObject AddCallflow(string fileName)
	{
		CallflowFileObject callflowFileObject = new CallflowFileObject(projectDirectory, base.RelativePath + "\\" + fileName, projectManagerControl, this, create: true);
		ChildrenList.Add(callflowFileObject);
		parent.NotifyChildChange();
		return callflowFileObject;
	}

	public ComponentFileObject AddComponent(string fileName)
	{
		ComponentFileObject componentFileObject = new ComponentFileObject(projectDirectory, base.RelativePath + "\\" + fileName, projectManagerControl, this, create: true);
		ChildrenList.Add(componentFileObject);
		parent.NotifyChildChange();
		return componentFileObject;
	}

	public void AddChild(AbsFileSystemObject fso)
	{
		ChildrenList.Add(fso);
		parent.NotifyChildChange();
	}

	public void RemoveChild(AbsFileSystemObject fso)
	{
		ChildrenList.Remove(fso);
		parent.NotifyChildChange();
	}

	public bool ChildExists(string fileName)
	{
		foreach (AbsFileSystemObject children in ChildrenList)
		{
			if (children.Name == fileName)
			{
				return true;
			}
		}
		return false;
	}

	public void NotifyChildChange()
	{
		parent.NotifyChildChange();
	}

	public string GetFolderPath()
	{
		return directoryInfo.FullName;
	}

	public string GetRelativeFolderPath()
	{
		return relativePath;
	}

	public override void OnParentPathChanged(AbsFileSystemObject parent)
	{
		relativePath = System.IO.Path.Combine(parent.RelativePath, directoryInfo.Name);
		directoryInfo = new DirectoryInfo(base.Path);
		foreach (AbsFileSystemObject children in ChildrenList)
		{
			children.OnParentPathChanged(this);
		}
	}

	public override bool Exists()
	{
		return directoryInfo.Exists;
	}

	public override void Open()
	{
	}

	public override void SaveTo(string newProjectDirectory)
	{
		Directory.CreateDirectory(System.IO.Path.Combine(newProjectDirectory, relativePath));
		projectDirectory = newProjectDirectory;
		directoryInfo = new DirectoryInfo(base.Path);
		foreach (AbsFileSystemObject children in ChildrenList)
		{
			children.SaveTo(newProjectDirectory);
		}
	}

	public override bool Close()
	{
		return true;
	}

	public override void Save()
	{
	}

	public override void Move(AbsFileSystemObject destination)
	{
		IFileSystemObjectContainer fileSystemObjectContainer = destination.GetFileSystemObjectContainer();
		string text = System.IO.Path.Combine(fileSystemObjectContainer.GetFolderPath(), directoryInfo.Name);
		directoryInfo.MoveTo(text);
		parent.RemoveChild(this);
		parent = fileSystemObjectContainer;
		parent.AddChild(this);
		relativePath = System.IO.Path.Combine(parent.GetRelativeFolderPath(), directoryInfo.Name);
		directoryInfo = new DirectoryInfo(text);
		NotifyNameChanged();
		foreach (AbsFileSystemObject children in ChildrenList)
		{
			children.OnParentPathChanged(this);
		}
	}

	public override void Delete()
	{
		while (ChildrenList.Count > 0)
		{
			ChildrenList[0].Delete();
		}
		if (directoryInfo.Exists)
		{
			directoryInfo.Delete(recursive: true);
		}
		parent.RemoveChild(this);
	}

	public override AbsFileSystemObject GetFileSystemObject(string relativePath)
	{
		if (base.relativePath == relativePath)
		{
			return this;
		}
		foreach (AbsFileSystemObject children in ChildrenList)
		{
			AbsFileSystemObject fileSystemObject = children.GetFileSystemObject(relativePath);
			if (fileSystemObject != null)
			{
				return fileSystemObject;
			}
		}
		return null;
	}

	public override ProjectObject GetProjectObject()
	{
		return parent.GetProjectObject();
	}

	public override List<FileObject> GetFileObjectList()
	{
		List<FileObject> list = new List<FileObject>();
		foreach (AbsFileSystemObject children in ChildrenList)
		{
			list.AddRange(children.GetFileObjectList());
		}
		return list;
	}

	public override List<DialerFileObject> GetDialerFileObjectList()
	{
		List<DialerFileObject> list = new List<DialerFileObject>();
		foreach (AbsFileSystemObject children in ChildrenList)
		{
			list.AddRange(children.GetDialerFileObjectList());
		}
		return list;
	}

	public override List<CallflowFileObject> GetCallflowFileObjectList()
	{
		List<CallflowFileObject> list = new List<CallflowFileObject>();
		foreach (AbsFileSystemObject children in ChildrenList)
		{
			list.AddRange(children.GetCallflowFileObjectList());
		}
		return list;
	}

	public override List<ComponentFileObject> GetComponentFileObjectList()
	{
		List<ComponentFileObject> list = new List<ComponentFileObject>();
		foreach (AbsFileSystemObject children in ChildrenList)
		{
			list.AddRange(children.GetComponentFileObjectList());
		}
		return list;
	}

	public override IFileSystemObjectContainer GetFileSystemObjectContainer()
	{
		return this;
	}

	public override XmlElement ToXmlElement(XmlDocument doc)
	{
		XmlElement xmlElement = doc.CreateElement("Folder");
		xmlElement.SetAttribute("path", base.RelativePath);
		ChildrenList.Sort(new FileSystemObjectComparer());
		foreach (AbsFileSystemObject children in ChildrenList)
		{
			XmlElement newChild = children.ToXmlElement(doc);
			xmlElement.AppendChild(newChild);
		}
		return xmlElement;
	}

	public override void CreateFlowLoaders()
	{
		foreach (AbsFileSystemObject children in ChildrenList)
		{
			children.CreateFlowLoaders();
		}
	}

	public override void ProcessComponentPathChanged(ComponentFileObject componentFileObject)
	{
		foreach (AbsFileSystemObject children in ChildrenList)
		{
			children.ProcessComponentPathChanged(componentFileObject);
		}
	}

	public override int GetExpandedImageIndex()
	{
		if (ChildrenList.Count != 0)
		{
			return 1;
		}
		return 2;
	}

	public override int GetCollapsedImageIndex()
	{
		return 2;
	}

	public override bool HasDialer()
	{
		foreach (AbsFileSystemObject children in ChildrenList)
		{
			if (children.HasDialer())
			{
				return true;
			}
		}
		return false;
	}

	public override bool HasCallflow()
	{
		foreach (AbsFileSystemObject children in ChildrenList)
		{
			if (children.HasCallflow())
			{
				return true;
			}
		}
		return false;
	}

	public override void ConfigureContextMenu(ToolStripMenuItem openDialerToolStripMenuItem, ToolStripMenuItem openCallflowToolStripMenuItem, ToolStripMenuItem openComponentToolStripMenuItem, ToolStripSeparator toolStripSeparator0, ToolStripMenuItem saveToolStripMenuItem, ToolStripMenuItem saveAsToolStripMenuItem, ToolStripSeparator toolStripSeparator1, ToolStripMenuItem renameToolStripMenuItem, ToolStripMenuItem removeDialerToolStripMenuItem, ToolStripMenuItem removeCallflowToolStripMenuItem, ToolStripMenuItem removeComponentToolStripMenuItem, ToolStripMenuItem removeFolderToolStripMenuItem, ToolStripMenuItem closeProjectToolStripMenuItem, ToolStripMenuItem closeDialerToolStripMenuItem, ToolStripMenuItem closeCallflowToolStripMenuItem, ToolStripMenuItem closeComponentToolStripMenuItem, ToolStripSeparator toolStripSeparator2, ToolStripMenuItem newFolderToolStripMenuItem, ToolStripMenuItem newDialerToolStripMenuItem, ToolStripMenuItem newCallflowToolStripMenuItem, ToolStripMenuItem newComponentToolStripMenuItem, ToolStripSeparator toolStripSeparator3, ToolStripMenuItem addExistingDialerToolStripMenuItem, ToolStripMenuItem addExistingCallflowToolStripMenuItem, ToolStripMenuItem addExistingComponentToolStripMenuItem, ToolStripSeparator toolStripSeparator4, ToolStripMenuItem debugBuildToolStripMenuItem, ToolStripMenuItem releaseBuildToolStripMenuItem, ToolStripMenuItem buildAllToolStripMenuItem)
	{
		bool flag = GetProjectObject().HasDialer();
		bool flag2 = GetProjectObject().HasCallflow();
		openDialerToolStripMenuItem.Visible = false;
		openCallflowToolStripMenuItem.Visible = false;
		openComponentToolStripMenuItem.Visible = false;
		toolStripSeparator0.Visible = false;
		saveToolStripMenuItem.Visible = false;
		saveAsToolStripMenuItem.Visible = false;
		toolStripSeparator1.Visible = false;
		renameToolStripMenuItem.Visible = true;
		removeDialerToolStripMenuItem.Visible = false;
		removeCallflowToolStripMenuItem.Visible = false;
		removeComponentToolStripMenuItem.Visible = false;
		removeFolderToolStripMenuItem.Visible = true;
		closeProjectToolStripMenuItem.Visible = false;
		closeDialerToolStripMenuItem.Visible = false;
		closeCallflowToolStripMenuItem.Visible = false;
		closeComponentToolStripMenuItem.Visible = false;
		toolStripSeparator2.Visible = true;
		newFolderToolStripMenuItem.Visible = true;
		newDialerToolStripMenuItem.Visible = !flag;
		newCallflowToolStripMenuItem.Visible = !flag2;
		newComponentToolStripMenuItem.Visible = true;
		toolStripSeparator3.Visible = true;
		addExistingDialerToolStripMenuItem.Visible = !flag;
		addExistingCallflowToolStripMenuItem.Visible = !flag2;
		addExistingComponentToolStripMenuItem.Visible = true;
		toolStripSeparator4.Visible = false;
		debugBuildToolStripMenuItem.Visible = false;
		releaseBuildToolStripMenuItem.Visible = false;
		buildAllToolStripMenuItem.Visible = false;
		renameToolStripMenuItem.Enabled = true;
		removeFolderToolStripMenuItem.Enabled = true;
		newFolderToolStripMenuItem.Enabled = true;
		newDialerToolStripMenuItem.Enabled = !flag;
		newCallflowToolStripMenuItem.Enabled = !flag2;
		newComponentToolStripMenuItem.Enabled = true;
		addExistingDialerToolStripMenuItem.Enabled = !flag;
		addExistingCallflowToolStripMenuItem.Enabled = !flag2;
		addExistingComponentToolStripMenuItem.Enabled = true;
	}

	public override void FillChildNodes(TreeNode parent)
	{
		TreeNode treeNode = projectManagerControl?.ProjectExplorer.AddNode(parent, this);
		foreach (AbsFileSystemObject children in ChildrenList)
		{
			children.FillChildNodes(treeNode);
		}
	}

	public override bool IsValidNewComponentName(string componentName)
	{
		foreach (AbsFileSystemObject children in ChildrenList)
		{
			if (!children.IsValidNewComponentName(componentName))
			{
				return false;
			}
		}
		return true;
	}

	public override void NotifyProjectVariableChanges(List<KeyValuePair<string, string>> renamedVariablesList)
	{
		foreach (AbsFileSystemObject children in ChildrenList)
		{
			children.NotifyProjectVariableChanges(renamedVariablesList);
		}
	}
}
