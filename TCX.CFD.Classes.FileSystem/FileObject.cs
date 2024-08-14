using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Editors;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Controls;

namespace TCX.CFD.Classes.FileSystem;

public abstract class FileObject : AbsFileSystemObject
{
	public delegate void FlowTypeChangedHandler(FileObject sender, FlowTypes flowType);

	public delegate void ActivitySelectedHandler(FileObject sender, string activityName, FlowTypes flowType);

	protected bool isEditing;

	private readonly string fileExtension;

	private readonly bool needsDisconnectHandlerFlow;

	private FileInfo fileInfo;

	private List<Variable> variableList = new List<Variable>();

	private readonly XmlSerializer variableListSerializer = new XmlSerializer(typeof(List<Variable>));

	private FlowLoader flowLoader;

	[Category("File")]
	[Description("The name of the file.")]
	public override string Name
	{
		get
		{
			return fileInfo.Name;
		}
		set
		{
			string text = (value.EndsWith(fileExtension, StringComparison.OrdinalIgnoreCase) ? value : (value + fileExtension));
			string text2 = text.Substring(0, text.Length - fileExtension.Length);
			if (NameHelper.IsValidName(text2))
			{
				if (fileInfo.Exists)
				{
					string text3 = fileInfo.DirectoryName + "\\" + value;
					if (!text3.EndsWith(fileExtension, StringComparison.OrdinalIgnoreCase))
					{
						text3 += fileExtension;
					}
					fileInfo.MoveTo(text3);
					fileInfo = new FileInfo(text3);
					relativePath = System.IO.Path.Combine(parent.GetRelativeFolderPath(), fileInfo.Name);
					NotifyNameChanged();
					NotifyComponentPathChanged();
					return;
				}
				throw new FileNotFoundException(string.Format(LocalizedResourceMgr.GetString("FileSystemObjects.Error.MissingFile"), base.Path));
			}
			throw new InvalidOperationException(string.Format(LocalizedResourceMgr.GetString("FileSystemObjects.Error.InvalidFileName"), text2));
		}
	}

	[Editor(typeof(VariableCollectionEditor), typeof(UITypeEditor))]
	[Category("File")]
	[Description("The variables of the file.")]
	public List<Variable> Variables
	{
		get
		{
			List<Variable> list = new List<Variable>();
			foreach (Variable variable in variableList)
			{
				list.Add(new Variable(variable.Name, variable.Scope, variable.Accessibility, variable.InitialValue, variable.ShowScopeProperty, variable.HelpText));
			}
			return list;
		}
		set
		{
			variableList = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public FlowLoader FlowLoader
	{
		get
		{
			if (flowLoader == null)
			{
				flowLoader = new FlowLoader(this);
			}
			return flowLoader;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool NeedsDisconnectHandlerFlow => needsDisconnectHandlerFlow;

	public event EventHandler Loading;

	public event EventHandler Selected;

	public event CancelEventHandler Saving;

	public event EventHandler Saved;

	public event CancelEventHandler Closing;

	public event EventHandler Closed;

	public event EventHandler Deleted;

	public event FlowTypeChangedHandler FlowTypeChanged;

	public event ActivitySelectedHandler ActivitySelected;

	protected abstract void NotifyComponentPathChanged();

	private void CreateDefaultContent()
	{
		XmlDocument xmlDocument = new XmlDocument();
		XmlElement xmlElement = xmlDocument.CreateElement("File");
		XmlElement xmlElement2 = xmlDocument.CreateElement("Version");
		xmlElement2.InnerText = FileSystemObjectVersionHelper.ToString(FileSystemObjectVersions.V2_1);
		XmlElement newChild = xmlDocument.CreateElement("Variables");
		XmlElement xmlElement3 = xmlDocument.CreateElement("Flows");
		XmlElement newChild2 = xmlDocument.CreateElement("MainFlow");
		xmlElement3.AppendChild(newChild2);
		XmlElement newChild3 = xmlDocument.CreateElement("ErrorHandlerFlow");
		xmlElement3.AppendChild(newChild3);
		if (needsDisconnectHandlerFlow)
		{
			XmlElement newChild4 = xmlDocument.CreateElement("DisconnectHandlerFlow");
			xmlElement3.AppendChild(newChild4);
		}
		xmlElement.AppendChild(xmlElement2);
		xmlElement.AppendChild(newChild);
		xmlElement.AppendChild(xmlElement3);
		xmlDocument.AppendChild(xmlElement);
		using StreamWriter streamWriter = fileInfo.CreateText();
		streamWriter.Write(xmlDocument.OuterXml);
	}

	private XmlDocument LoadToXmlDocument()
	{
		if (!fileInfo.Exists || fileInfo.Length == 0L)
		{
			CreateDefaultContent();
			fileInfo.Refresh();
		}
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(fileInfo.FullName);
		return xmlDocument;
	}

	private void WriteFromXmlDocument(XmlDocument xmlDocument)
	{
		using StreamWriter writer = fileInfo.CreateText();
		xmlDocument.Save(writer);
	}

	private void SaveVariables()
	{
		XmlDocument xmlDocument = LoadToXmlDocument();
		xmlDocument.CreateNavigator().SelectSingleNode("/File/Variables").InnerXml = SerializationHelper.Serialize(variableListSerializer, variableList);
		WriteFromXmlDocument(xmlDocument);
	}

	private void LoadVariables()
	{
		XPathNavigator xPathNavigator = LoadToXmlDocument().CreateNavigator().SelectSingleNode("/File/Variables");
		if (!string.IsNullOrEmpty(xPathNavigator.InnerXml))
		{
			variableList = SerializationHelper.Deserialize(variableListSerializer, xPathNavigator.InnerXml) as List<Variable>;
		}
	}

	private void VerifyFileVersion()
	{
		XPathNavigator xPathNavigator = LoadToXmlDocument().CreateNavigator().SelectSingleNode("/File/Version");
		if (xPathNavigator == null || !FileSystemObjectVersionHelper.IsValidVersion(xPathNavigator.Value))
		{
			throw new ApplicationException(string.Format(LocalizedResourceMgr.GetString("FileSystemObjects.Error.InvalidFileVersion"), base.RelativePath));
		}
		fsoVersion = FileSystemObjectVersionHelper.FromString(xPathNavigator.Value);
	}

	protected FileObject(string projectDirectory, string relativePath, ProjectManagerControl projectManagerControl, IFileSystemObjectContainer parent, bool create, string fileExtension, bool needsDisconnectHandlerFlow)
		: base(projectDirectory, relativePath, projectManagerControl, parent)
	{
		this.fileExtension = fileExtension;
		this.needsDisconnectHandlerFlow = needsDisconnectHandlerFlow;
		fileInfo = new FileInfo(base.Path);
		if (!fileInfo.Exists && create)
		{
			CreateDefaultContent();
			fileInfo.Refresh();
		}
		if (fileInfo.Exists)
		{
			LoadVariables();
			VerifyFileVersion();
		}
	}

	public override void OnParentPathChanged(AbsFileSystemObject parent)
	{
		relativePath = System.IO.Path.Combine(parent.RelativePath, fileInfo.Name);
		fileInfo = new FileInfo(base.Path);
		NotifyComponentPathChanged();
	}

	public override bool Exists()
	{
		return fileInfo.Exists;
	}

	public override void Open()
	{
		if (isEditing)
		{
			this.Selected(this, EventArgs.Empty);
			return;
		}
		fileInfo.Refresh();
		if (fileInfo.Exists)
		{
			HasChanges = false;
			this.Loading(this, EventArgs.Empty);
			isEditing = true;
			return;
		}
		throw new FileNotFoundException(string.Format(LocalizedResourceMgr.GetString("FileSystemObjects.Error.MissingFile"), base.Path));
	}

	public override void SaveTo(string newProjectDirectory)
	{
		fileInfo.CopyTo(System.IO.Path.Combine(newProjectDirectory, relativePath));
		projectDirectory = newProjectDirectory;
		fileInfo = new FileInfo(base.Path);
	}

	public override bool Close()
	{
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		this.Closing(this, cancelEventArgs);
		if (cancelEventArgs.Cancel)
		{
			return false;
		}
		if (HasChanges)
		{
			LoadVariables();
		}
		isEditing = false;
		HasChanges = false;
		this.Closed(this, EventArgs.Empty);
		flowLoader = new FlowLoader(this);
		return true;
	}

	public override void Save()
	{
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		this.Saving(this, cancelEventArgs);
		if (!cancelEventArgs.Cancel)
		{
			HasChanges = false;
			SaveVariables();
			this.Saved(this, EventArgs.Empty);
		}
	}

	public override void Move(AbsFileSystemObject destination)
	{
		IFileSystemObjectContainer fileSystemObjectContainer = destination.GetFileSystemObjectContainer();
		string text = System.IO.Path.Combine(fileSystemObjectContainer.GetFolderPath(), fileInfo.Name);
		fileInfo.MoveTo(text);
		parent.RemoveChild(this);
		parent = fileSystemObjectContainer;
		parent.AddChild(this);
		relativePath = System.IO.Path.Combine(parent.GetRelativeFolderPath(), fileInfo.Name);
		fileInfo = new FileInfo(text);
		NotifyNameChanged();
		NotifyComponentPathChanged();
	}

	public override void Delete()
	{
		if (fileInfo.Exists)
		{
			fileInfo.Delete();
		}
		parent.RemoveChild(this);
		this.Deleted(this, EventArgs.Empty);
	}

	public override AbsFileSystemObject GetFileSystemObject(string relativePath)
	{
		if (base.relativePath == relativePath)
		{
			return this;
		}
		return null;
	}

	public override ProjectObject GetProjectObject()
	{
		return parent.GetProjectObject();
	}

	public override List<FileObject> GetFileObjectList()
	{
		return new List<FileObject> { this };
	}

	public List<ComponentFileObject> GetComponentFileObjects()
	{
		List<ComponentFileObject> list = new List<ComponentFileObject>();
		RootFlow rootFlow = FlowLoader.GetRootFlow(FlowTypes.MainFlow);
		list.AddRange(rootFlow.GetComponentFileObjects());
		RootFlow rootFlow2 = FlowLoader.GetRootFlow(FlowTypes.ErrorHandler);
		list.AddRange(rootFlow2.GetComponentFileObjects());
		if (needsDisconnectHandlerFlow)
		{
			RootFlow rootFlow3 = FlowLoader.GetRootFlow(FlowTypes.DisconnectHandler);
			list.AddRange(rootFlow3.GetComponentFileObjects());
		}
		return list;
	}

	public override IFileSystemObjectContainer GetFileSystemObjectContainer()
	{
		return parent;
	}

	public override void CreateFlowLoaders()
	{
		try
		{
			if (fileInfo.Exists && (fsoVersion == FileSystemObjectVersions.V1_0 || fsoVersion == FileSystemObjectVersions.V1_1))
			{
				string text = fileInfo.FullName + ".bak";
				if (!File.Exists(text))
				{
					fileInfo.CopyTo(text);
				}
				File.WriteAllText(fileInfo.FullName, File.ReadAllText(fileInfo.FullName).Replace("Innovatip.VAD", "TCX.CFD").Replace("TCX.VAD", "TCX.CFD")
					.Replace("3CX Voice Application Designer", "3CX Call Flow Designer"));
				XmlDocument xmlDocument = LoadToXmlDocument();
				XPathNavigator xPathNavigator = xmlDocument.CreateNavigator();
				XPathNavigator xPathNavigator2 = xPathNavigator.SelectSingleNode("/File/Flows/ErrorHandlerFlow");
				XPathNodeIterator xPathNodeIterator = xPathNavigator2.SelectChildren(XPathNodeType.Element);
				if (xPathNodeIterator.MoveNext())
				{
					XPathNavigator current = xPathNodeIterator.Current;
					XPathNodeIterator xPathNodeIterator2 = current.SelectChildren(XPathNodeType.Element);
					if (xPathNodeIterator2.MoveNext())
					{
						XPathNodeIterator xPathNodeIterator3 = xPathNodeIterator2.Current.SelectChildren(XPathNodeType.Element);
						bool flag = false;
						bool flag2 = false;
						while (xPathNodeIterator3.MoveNext())
						{
							if (flag)
							{
								flag2 = true;
								continue;
							}
							string ınnerXml = xPathNodeIterator3.Current.InnerXml;
							current.InnerXml = ınnerXml;
							flag = true;
						}
						if (flag2)
						{
							MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("FileSystemObjects.MessageBox.Error.ErrorHandlerWithManyBranches"), base.Path), LocalizedResourceMgr.GetString("FileSystemObjects.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
						}
					}
				}
				XmlHelper.MigratePromptCollections(xPathNavigator.SelectSingleNode("/File/Flows/MainFlow").SelectDescendants(XPathNodeType.Element, matchSelf: false));
				XmlHelper.MigratePromptCollections(xPathNavigator2.SelectDescendants(XPathNodeType.Element, matchSelf: false));
				foreach (Variable variable in variableList)
				{
					variable.InitialValue = ExpressionHelper.MigrateConstantStringExpression(variable.InitialValue);
				}
				WriteFromXmlDocument(xmlDocument);
			}
			if (flowLoader == null)
			{
				flowLoader = new FlowLoader(this);
			}
		}
		catch (Exception exc)
		{
			MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("FileSystemObjects.MessageBox.Error.FileCanNotBeLoaded"), base.Path, ErrorHelper.GetErrorDescription(exc)), LocalizedResourceMgr.GetString("FileSystemObjects.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public override void ProcessComponentPathChanged(ComponentFileObject componentFileObject)
	{
		if (flowLoader != null && flowLoader.IsUsingUserComponent(componentFileObject))
		{
			if (isEditing)
			{
				HasChanges = true;
			}
			else
			{
				flowLoader.Save();
			}
		}
	}

	public abstract Image GetImage();

	public string GetNameWithoutExtension()
	{
		return fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length);
	}

	public string GetFileContent(FlowTypes flowType)
	{
		if (fileInfo.Exists)
		{
			try
			{
				using StreamReader streamReader = fileInfo.OpenText();
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(XmlHelper.SanitizeXmlString(streamReader.ReadToEnd()));
				XPathNavigator xPathNavigator = xmlDocument.CreateNavigator();
				XPathNavigator xPathNavigator2 = null;
				switch (flowType)
				{
				case FlowTypes.MainFlow:
					xPathNavigator2 = xPathNavigator.SelectSingleNode("/File/Flows/MainFlow");
					break;
				case FlowTypes.ErrorHandler:
					xPathNavigator2 = xPathNavigator.SelectSingleNode("/File/Flows/ErrorHandlerFlow");
					break;
				case FlowTypes.DisconnectHandler:
					xPathNavigator2 = xPathNavigator.SelectSingleNode("/File/Flows/DisconnectHandlerFlow");
					break;
				}
				return xPathNavigator2.InnerXml;
			}
			catch (Exception)
			{
			}
		}
		return string.Empty;
	}

	public void SetFileContent(FlowTypes flowType, string flowContent)
	{
		XmlDocument xmlDocument = LoadToXmlDocument();
		XPathNavigator xPathNavigator = xmlDocument.CreateNavigator();
		switch (flowType)
		{
		case FlowTypes.MainFlow:
			xPathNavigator.SelectSingleNode("/File/Flows/MainFlow").InnerXml = flowContent;
			break;
		case FlowTypes.ErrorHandler:
			xPathNavigator.SelectSingleNode("/File/Flows/ErrorHandlerFlow").InnerXml = flowContent;
			break;
		case FlowTypes.DisconnectHandler:
			if (needsDisconnectHandlerFlow)
			{
				xPathNavigator.SelectSingleNode("/File/Flows/DisconnectHandlerFlow").InnerXml = flowContent;
			}
			break;
		}
		XPathNavigator xPathNavigator2 = xPathNavigator.SelectSingleNode("/File/Version");
		if (xPathNavigator2 != null)
		{
			xPathNavigator2.InnerXml = FileSystemObjectVersionHelper.ToString(FileSystemObjectVersions.V2_1);
		}
		WriteFromXmlDocument(xmlDocument);
	}

	public void NotifyVariableChanges(List<KeyValuePair<string, string>> renamedVariablesList)
	{
		if (isEditing)
		{
			HasChanges = true;
		}
		else
		{
			SaveVariables();
		}
		if (flowLoader == null)
		{
			return;
		}
		foreach (KeyValuePair<string, string> renamedVariables in renamedVariablesList)
		{
			string oldValue = "callflow$." + renamedVariables.Key;
			string newValue = "callflow$." + renamedVariables.Value;
			flowLoader.GetRootFlow(FlowTypes.MainFlow)?.NotifyComponentRenamed(oldValue, newValue);
			flowLoader.GetRootFlow(FlowTypes.ErrorHandler)?.NotifyComponentRenamed(oldValue, newValue);
			if (needsDisconnectHandlerFlow)
			{
				flowLoader.GetRootFlow(FlowTypes.DisconnectHandler)?.NotifyComponentRenamed(oldValue, newValue);
			}
		}
	}

	public override void NotifyProjectVariableChanges(List<KeyValuePair<string, string>> renamedVariablesList)
	{
		if (flowLoader == null)
		{
			return;
		}
		foreach (KeyValuePair<string, string> renamedVariables in renamedVariablesList)
		{
			string oldValue = "project$." + renamedVariables.Key;
			string newValue = "project$." + renamedVariables.Value;
			flowLoader.GetRootFlow(FlowTypes.MainFlow)?.NotifyComponentRenamed(oldValue, newValue);
			flowLoader.GetRootFlow(FlowTypes.ErrorHandler)?.NotifyComponentRenamed(oldValue, newValue);
			if (needsDisconnectHandlerFlow)
			{
				flowLoader.GetRootFlow(FlowTypes.DisconnectHandler)?.NotifyComponentRenamed(oldValue, newValue);
			}
		}
	}

	public void ChangeFlowType(FlowTypes flowType)
	{
		this.FlowTypeChanged?.Invoke(this, flowType);
	}

	public void SelectActivity(string activityName, FlowTypes flowType)
	{
		this.ActivitySelected?.Invoke(this, activityName, flowType);
	}
}
