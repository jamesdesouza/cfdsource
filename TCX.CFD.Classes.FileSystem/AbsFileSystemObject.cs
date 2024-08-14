using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using TCX.CFD.Controls;

namespace TCX.CFD.Classes.FileSystem;

public abstract class AbsFileSystemObject
{
	protected const string DialerExtension = ".dialer";

	protected const string CallflowExtension = ".flow";

	protected const string ComponentExtension = ".comp";

	private bool hasChanges;

	protected string projectDirectory;

	protected string relativePath;

	protected ProjectManagerControl projectManagerControl;

	protected IFileSystemObjectContainer parent;

	protected FileSystemObjectVersions fsoVersion;

	[Browsable(false)]
	public virtual bool HasChanges
	{
		get
		{
			return hasChanges;
		}
		set
		{
			hasChanges = value;
			if (hasChanges)
			{
				this.Changed?.Invoke(this, EventArgs.Empty);
			}
			if (hasChanges)
			{
				parent?.NotifyChildChange();
			}
		}
	}

	public abstract string Name { get; set; }

	[Description("The path to the file system object.")]
	public string Path => System.IO.Path.Combine(projectDirectory, relativePath);

	[Browsable(false)]
	public string RelativePath => relativePath;

	[Browsable(false)]
	public FileSystemObjectVersions FsoVersion => fsoVersion;

	public event EventHandler NameChanged;

	public event EventHandler Changed;

	protected void NotifyNameChanged()
	{
		parent?.NotifyChildChange();
		this.NameChanged?.Invoke(this, EventArgs.Empty);
	}

	public static AbsFileSystemObject CreateFromXml(string projectDirectory, string xml, ProjectManagerControl projectManagerControl, IFileSystemObjectContainer parent)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(XmlHelper.SanitizeXmlString(xml));
		string name = xmlDocument.FirstChild.Name;
		if (!(name == "Folder"))
		{
			if (name == "File")
			{
				return xmlDocument.FirstChild.Attributes["type"].Value switch
				{
					"dialer" => new DialerFileObject(projectDirectory, xmlDocument.FirstChild.Attributes["path"].Value, projectManagerControl, parent, create: false, (xmlDocument.FirstChild.Attributes["dialer_mode"] != null) ? EnumHelper.StringToDialerMode(xmlDocument.FirstChild.Attributes["dialer_mode"].Value) : DialerModes.PowerDialer, (xmlDocument.FirstChild.Attributes["pause_between_dialer_execution"] == null) ? 30 : Convert.ToInt32(xmlDocument.FirstChild.Attributes["pause_between_dialer_execution"].Value), (xmlDocument.FirstChild.Attributes["parallel_dialers"] == null) ? 1 : Convert.ToInt32(xmlDocument.FirstChild.Attributes["parallel_dialers"].Value), (xmlDocument.FirstChild.Attributes["predictive_dialer_queue"] == null) ? string.Empty : xmlDocument.FirstChild.Attributes["predictive_dialer_queue"].Value, (xmlDocument.FirstChild.Attributes["predictive_dialer_optimization"] != null) ? EnumHelper.StringToPredictiveDialerOptimization(xmlDocument.FirstChild.Attributes["predictive_dialer_optimization"].Value) : PredictiveDialerOptimizations.ForAgents), 
					"callflow" => new CallflowFileObject(projectDirectory, xmlDocument.FirstChild.Attributes["path"].Value, projectManagerControl, parent, create: false), 
					"component" => new ComponentFileObject(projectDirectory, xmlDocument.FirstChild.Attributes["path"].Value, projectManagerControl, parent, create: false), 
					_ => throw new ArgumentException(string.Format(LocalizedResourceMgr.GetString("FileSystemObjects.Error.InvalidProjectFileFormat"), string.Format(LocalizedResourceMgr.GetString("FileSystemObjects.Error.InvalidXmlAttributeValue"), xmlDocument.FirstChild.Name, "type", xmlDocument.FirstChild.Attributes["type"].Value))), 
				};
			}
			throw new ArgumentException(string.Format(LocalizedResourceMgr.GetString("FileSystemObjects.Error.InvalidProjectFileFormat"), string.Format(LocalizedResourceMgr.GetString("FileSystemObjects.Error.InvalidXmlElement"), xmlDocument.FirstChild.Name)));
		}
		FolderObject folderObject = new FolderObject(projectDirectory, xmlDocument.FirstChild.Attributes["path"].Value, projectManagerControl, parent);
		XPathNodeIterator xPathNodeIterator = xmlDocument.FirstChild.CreateNavigator().SelectChildren(XPathNodeType.Element);
		while (xPathNodeIterator.MoveNext())
		{
			folderObject.ChildrenList.Add(CreateFromXml(projectDirectory, xPathNodeIterator.Current.OuterXml, projectManagerControl, folderObject));
		}
		return folderObject;
	}

	protected AbsFileSystemObject(string projectDirectory, string relativePath, ProjectManagerControl projectManagerControl, IFileSystemObjectContainer parent)
	{
		this.projectDirectory = projectDirectory;
		this.relativePath = relativePath;
		this.projectManagerControl = projectManagerControl;
		this.parent = parent;
	}

	public IFileSystemObjectContainer GetParent()
	{
		return parent;
	}

	public abstract void OnParentPathChanged(AbsFileSystemObject parent);

	public abstract bool Exists();

	public abstract void Open();

	public abstract void SaveTo(string newProjectDirectory);

	public abstract bool Close();

	public abstract void Save();

	public abstract void Move(AbsFileSystemObject destination);

	public abstract void Delete();

	public abstract AbsFileSystemObject GetFileSystemObject(string relativePath);

	public abstract ProjectObject GetProjectObject();

	public abstract List<FileObject> GetFileObjectList();

	public abstract List<DialerFileObject> GetDialerFileObjectList();

	public abstract List<CallflowFileObject> GetCallflowFileObjectList();

	public abstract List<ComponentFileObject> GetComponentFileObjectList();

	public abstract IFileSystemObjectContainer GetFileSystemObjectContainer();

	public abstract XmlElement ToXmlElement(XmlDocument doc);

	public abstract void CreateFlowLoaders();

	public abstract void ProcessComponentPathChanged(ComponentFileObject componentFileObject);

	public abstract int GetExpandedImageIndex();

	public abstract int GetCollapsedImageIndex();

	public abstract bool HasDialer();

	public abstract bool HasCallflow();

	public abstract void ConfigureContextMenu(ToolStripMenuItem openDialerToolStripMenuItem, ToolStripMenuItem openCallflowToolStripMenuItem, ToolStripMenuItem openComponentToolStripMenuItem, ToolStripSeparator toolStripSeparator0, ToolStripMenuItem saveToolStripMenuItem, ToolStripMenuItem saveAsToolStripMenuItem, ToolStripSeparator toolStripSeparator1, ToolStripMenuItem renameToolStripMenuItem, ToolStripMenuItem removeDialerToolStripMenuItem, ToolStripMenuItem removeCallflowToolStripMenuItem, ToolStripMenuItem removeComponentToolStripMenuItem, ToolStripMenuItem removeFolderToolStripMenuItem, ToolStripMenuItem closeProjectToolStripMenuItem, ToolStripMenuItem closeDialerToolStripMenuItem, ToolStripMenuItem closeCallflowToolStripMenuItem, ToolStripMenuItem closeComponentToolStripMenuItem, ToolStripSeparator toolStripSeparator2, ToolStripMenuItem newFolderToolStripMenuItem, ToolStripMenuItem newDialerToolStripMenuItem, ToolStripMenuItem newCallflowToolStripMenuItem, ToolStripMenuItem newComponentToolStripMenuItem, ToolStripSeparator toolStripSeparator3, ToolStripMenuItem addExistingDialerToolStripMenuItem, ToolStripMenuItem addExistingCallflowToolStripMenuItem, ToolStripMenuItem addExistingComponentToolStripMenuItem, ToolStripSeparator toolStripSeparator4, ToolStripMenuItem debugBuildToolStripMenuItem, ToolStripMenuItem releaseBuildToolStripMenuItem, ToolStripMenuItem buildAllToolStripMenuItem);

	public abstract void FillChildNodes(TreeNode parent);

	public abstract bool IsValidNewComponentName(string componentName);

	public abstract void NotifyProjectVariableChanges(List<KeyValuePair<string, string>> renamedVariablesList);
}
