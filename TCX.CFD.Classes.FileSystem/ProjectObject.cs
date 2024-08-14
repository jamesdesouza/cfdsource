using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using TCX.CFD.Classes.Editors;
using TCX.CFD.Controls;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.FileSystem;

public class ProjectObject : AbsFileSystemObject, IFileSystemObjectContainer
{
	public delegate void ReportProgressEventHandler(int progress);

	public const FileSystemObjectVersions CurrentVersion = FileSystemObjectVersions.V2_1;

	private bool isCreating;

	private FileInfo fileInfo;

	private readonly List<AbsFileSystemObject> childrenList = new List<AbsFileSystemObject>();

	private readonly XmlSerializer variableListSerializer = new XmlSerializer(typeof(List<Variable>));

	private List<Variable> variableList = new List<Variable>();

	private bool debugBuildSuccessful;

	private bool releaseBuildSuccessful;

	private uint debugBuildNumber;

	private uint releaseBuildNumber;

	private bool changedSinceLastDebugBuild;

	private bool doNotAskForExtension;

	private string extension = string.Empty;

	private OnlineServices onlineServices = new OnlineServices();

	[Browsable(false)]
	public override bool HasChanges
	{
		get
		{
			return base.HasChanges;
		}
		set
		{
			if (value)
			{
				changedSinceLastDebugBuild = true;
			}
			base.HasChanges = value;
		}
	}

	[Category("Project")]
	[Description("The name of the project.")]
	public override string Name
	{
		get
		{
			return fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length);
		}
		set
		{
			if (NameHelper.IsValidName(value))
			{
				relativePath = value + fileInfo.Extension;
				fileInfo.MoveTo(base.Path);
				NotifyNameChanged();
				return;
			}
			throw new InvalidOperationException(string.Format(LocalizedResourceMgr.GetString("FileSystemObjects.Error.InvalidProjectName"), value));
		}
	}

	[Browsable(false)]
	public bool DebugBuildSuccessful
	{
		get
		{
			return debugBuildSuccessful;
		}
		set
		{
			if (debugBuildSuccessful != value)
			{
				debugBuildSuccessful = value;
				HasChanges = true;
			}
		}
	}

	[Browsable(false)]
	public bool ReleaseBuildSuccessful
	{
		get
		{
			return releaseBuildSuccessful;
		}
		set
		{
			if (releaseBuildSuccessful != value)
			{
				releaseBuildSuccessful = value;
				HasChanges = true;
			}
		}
	}

	[Browsable(false)]
	public uint DebugBuildNumber
	{
		get
		{
			return debugBuildNumber;
		}
		set
		{
			if (debugBuildNumber != value)
			{
				debugBuildNumber = value;
				HasChanges = true;
			}
		}
	}

	[Browsable(false)]
	public uint ReleaseBuildNumber
	{
		get
		{
			return releaseBuildNumber;
		}
		set
		{
			if (releaseBuildNumber != value)
			{
				releaseBuildNumber = value;
				HasChanges = true;
			}
		}
	}

	[Browsable(false)]
	public bool ChangedSinceLastDebugBuild
	{
		get
		{
			return changedSinceLastDebugBuild;
		}
		set
		{
			changedSinceLastDebugBuild = value;
		}
	}

	[Editor(typeof(VariableCollectionEditor), typeof(UITypeEditor))]
	[Category("Project")]
	[Description("The variables of the project (can be accessed from any callflow or component within the project).")]
	public List<Variable> Variables
	{
		get
		{
			List<Variable> list = new List<Variable>();
			foreach (Variable variable in variableList)
			{
				list.Add(new Variable(variable.Name, variable.Scope, variable.Accessibility, variable.InitialValue, showScopeProperty: false, variable.HelpText));
			}
			return list;
		}
		set
		{
			variableList = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Variable> RecordResultConstantList { get; } = new List<Variable>();


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Variable> MenuResultConstantList { get; } = new List<Variable>();


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Variable> UserInputResultConstantList { get; } = new List<Variable>();


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Variable> VoiceInputResultConstantList { get; } = new List<Variable>();


	[Browsable(false)]
	public bool DoNotAskForExtension
	{
		get
		{
			return doNotAskForExtension;
		}
		set
		{
			doNotAskForExtension = value;
		}
	}

	[Category("Project")]
	[Description("The extension where this project will be deployed.")]
	public string Extension
	{
		get
		{
			return extension;
		}
		set
		{
			if (extension != value)
			{
				extension = value;
				HasChanges = true;
			}
		}
	}

	[Editor(typeof(OnlineServicesEditor), typeof(UITypeEditor))]
	[TypeConverter(typeof(OnlineServicesTypeConverter))]
	[Category("Project")]
	[Description("The online services configuration, required when using text to speech or speech to text in this project.")]
	public OnlineServices OnlineServices
	{
		get
		{
			return onlineServices;
		}
		set
		{
			onlineServices = value;
			HasChanges = true;
		}
	}

	public event CancelEventHandler Closing;

	public event EventHandler Closed;

	public event EventHandler Loading;

	public event EventHandler Loaded;

	public event EventHandler NotLoaded;

	public event ReportProgressEventHandler LoadProgress;

	private void ReportLoadingProgress(int progress)
	{
		this.LoadProgress?.Invoke(progress);
	}

	private void Initialize()
	{
		fileInfo = new FileInfo(base.Path);
		projectManagerControl?.RegisterProjectObject(this);
		RecordResultConstantList.Add(new Variable("NothingRecorded", VariableScopes.Public, VariableAccessibilities.ReadOnly, "RecordComponent.RecordResults.NothingRecorded"));
		RecordResultConstantList.Add(new Variable("StopDigit", VariableScopes.Public, VariableAccessibilities.ReadOnly, "RecordComponent.RecordResults.StopDigit"));
		RecordResultConstantList.Add(new Variable("Completed", VariableScopes.Public, VariableAccessibilities.ReadOnly, "RecordComponent.RecordResults.Completed"));
		MenuResultConstantList.Add(new Variable("Timeout", VariableScopes.Public, VariableAccessibilities.ReadOnly, "MenuComponent.MenuResults.Timeout"));
		MenuResultConstantList.Add(new Variable("InvalidOption", VariableScopes.Public, VariableAccessibilities.ReadOnly, "MenuComponent.MenuResults.InvalidOption"));
		MenuResultConstantList.Add(new Variable("ValidOption", VariableScopes.Public, VariableAccessibilities.ReadOnly, "MenuComponent.MenuResults.ValidOption"));
		UserInputResultConstantList.Add(new Variable("Timeout", VariableScopes.Public, VariableAccessibilities.ReadOnly, "UserInputComponent.UserInputResults.Timeout"));
		UserInputResultConstantList.Add(new Variable("InvalidDigits", VariableScopes.Public, VariableAccessibilities.ReadOnly, "UserInputComponent.UserInputResults.InvalidDigits"));
		UserInputResultConstantList.Add(new Variable("ValidDigits", VariableScopes.Public, VariableAccessibilities.ReadOnly, "UserInputComponent.UserInputResults.ValidDigits"));
		VoiceInputResultConstantList.Add(new Variable("Timeout", VariableScopes.Public, VariableAccessibilities.ReadOnly, "VoiceInputComponent.VoiceInputResults.Timeout"));
		VoiceInputResultConstantList.Add(new Variable("InvalidInput", VariableScopes.Public, VariableAccessibilities.ReadOnly, "VoiceInputComponent.VoiceInputResults.InvalidInput"));
		VoiceInputResultConstantList.Add(new Variable("ValidInput", VariableScopes.Public, VariableAccessibilities.ReadOnly, "VoiceInputComponent.VoiceInputResults.ValidInput"));
		VoiceInputResultConstantList.Add(new Variable("ValidDtmfInput", VariableScopes.Public, VariableAccessibilities.ReadOnly, "VoiceInputComponent.VoiceInputResults.ValidDtmfInput"));
	}

	public ProjectObject(string projectDirectory, string relativePath, ProjectManagerControl projectManagerControl)
		: base(projectDirectory, relativePath, projectManagerControl, null)
	{
		Initialize();
		isCreating = false;
	}

	public ProjectObject(string projectDirectory, string relativePath, ProjectManagerControl projectManagerControl, NewProjectTypes projectType)
		: base(projectDirectory, relativePath, projectManagerControl, null)
	{
		Initialize();
		isCreating = true;
		if (projectType == NewProjectTypes.Callflow)
		{
			AddCallflow("Main.flow");
		}
		else
		{
			AddDialer("Dialer.dialer");
		}
		onlineServices.TextToSpeechEngine = EnumHelper.StringToTextToSpeechEngines(Settings.Default.OnlineServicesTextToSpeechEngine);
		onlineServices.SpeechToTextEngine = EnumHelper.StringToSpeechToTextEngines(Settings.Default.OnlineServicesSpeechToTextEngine);
		onlineServices.AmazonPollySettings.ClientID = Settings.Default.AmazonPollyClientID;
		onlineServices.AmazonPollySettings.ClientSecret = Settings.Default.AmazonPollyClientSecret;
		onlineServices.AmazonPollySettings.Region = EnumHelper.StringToTextToSpeechAmazonRegion(Settings.Default.AmazonRegion);
		onlineServices.AmazonPollySettings.Lexicons = new List<string>(Settings.Default.AmazonPollyLexicons.Split(new string[1] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
		onlineServices.GoogleCloudSettings.ServiceAccountKeyFileName = Settings.Default.GoogleCloudServiceAccountKeyFileName;
		onlineServices.GoogleCloudSettings.ServiceAccountKeyJSON = Settings.Default.GoogleCloudServiceAccountKeyJSON;
		Save();
		Directory.CreateDirectory(System.IO.Path.Combine(projectDirectory, "Output", "Release"));
		Directory.CreateDirectory(System.IO.Path.Combine(projectDirectory, "Audio"));
		Directory.CreateDirectory(System.IO.Path.Combine(projectDirectory, "Libraries"));
	}

	public void Export()
	{
		string text = System.IO.Path.Combine(new DirectoryInfo(projectDirectory).Parent.FullName, Name + ".zip");
		if (File.Exists(text))
		{
			File.Delete(text);
		}
		ZipFile.CreateFromDirectory(projectDirectory, text, CompressionLevel.Optimal, includeBaseDirectory: false);
		Process.Start("explorer.exe", "/select,\"" + text + "\"");
	}

	public override void OnParentPathChanged(AbsFileSystemObject parent)
	{
		throw new InvalidOperationException(LocalizedResourceMgr.GetString("FileSystemObjects.Error.ProjectHasNoParent"));
	}

	public override bool Exists()
	{
		return fileInfo.Exists;
	}

	public override void Open()
	{
		try
		{
			this.Loading?.Invoke(this, EventArgs.Empty);
			ReportLoadingProgress(0);
			using (StreamReader streamReader = fileInfo.OpenText())
			{
				ReportLoadingProgress(10);
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(XmlHelper.SanitizeXmlString(streamReader.ReadToEnd()));
				XPathNavigator xPathNavigator = xmlDocument.CreateNavigator();
				ReportLoadingProgress(20);
				if (!isCreating)
				{
					XPathNavigator xPathNavigator2 = xPathNavigator.SelectSingleNode("/Graphical_Application_Designer_Project/Files");
					if (xPathNavigator2 != null)
					{
						XPathNodeIterator xPathNodeIterator = xPathNavigator2.SelectChildren(XPathNodeType.Element);
						while (xPathNodeIterator.MoveNext())
						{
							ReportLoadingProgress(20 + 30 * xPathNodeIterator.CurrentPosition / xPathNodeIterator.Count);
							childrenList.Add(AbsFileSystemObject.CreateFromXml(projectDirectory, xPathNodeIterator.Current.OuterXml, projectManagerControl, this));
						}
					}
				}
				else
				{
					isCreating = false;
				}
				XPathNavigator xPathNavigator3 = xPathNavigator.SelectSingleNode("/Graphical_Application_Designer_Project/Version");
				if (xPathNavigator3 == null || !FileSystemObjectVersionHelper.IsValidVersion(xPathNavigator3.Value))
				{
					throw new ApplicationException(LocalizedResourceMgr.GetString("FileSystemObjects.Error.InvalidProjectVersion"));
				}
				bool flag = false;
				fsoVersion = FileSystemObjectVersionHelper.FromString(xPathNavigator3.Value);
				if (fsoVersion == FileSystemObjectVersions.V1_0 || fsoVersion == FileSystemObjectVersions.V1_1)
				{
					string text = fileInfo.FullName.Substring(0, fileInfo.FullName.Length - fileInfo.Extension.Length) + ".cfdproj";
					fileInfo.CopyTo(text);
					fileInfo = new FileInfo(text);
					flag = true;
				}
				ReportLoadingProgress(60);
				XPathNavigator xPathNavigator4 = xPathNavigator.SelectSingleNode("/Graphical_Application_Designer_Project/Variables");
				if (xPathNavigator4 != null && !string.IsNullOrEmpty(xPathNavigator4.InnerXml))
				{
					variableList = SerializationHelper.Deserialize(variableListSerializer, xPathNavigator4.InnerXml) as List<Variable>;
				}
				XPathNavigator xPathNavigator5 = xPathNavigator.SelectSingleNode("/Graphical_Application_Designer_Project/DebugBuildSuccessful");
				debugBuildSuccessful = xPathNavigator5 != null && Convert.ToBoolean(xPathNavigator5.Value);
				XPathNavigator xPathNavigator6 = xPathNavigator.SelectSingleNode("/Graphical_Application_Designer_Project/ReleaseBuildSuccessful");
				releaseBuildSuccessful = xPathNavigator6 != null && Convert.ToBoolean(xPathNavigator6.Value);
				XPathNavigator xPathNavigator7 = xPathNavigator.SelectSingleNode("/Graphical_Application_Designer_Project/DebugBuildNumber");
				debugBuildNumber = ((xPathNavigator7 != null) ? Convert.ToUInt32(xPathNavigator7.Value) : 0u);
				XPathNavigator xPathNavigator8 = xPathNavigator.SelectSingleNode("/Graphical_Application_Designer_Project/ReleaseBuildNumber");
				releaseBuildNumber = ((xPathNavigator8 != null) ? Convert.ToUInt32(xPathNavigator8.Value) : 0u);
				XPathNavigator xPathNavigator9 = xPathNavigator.SelectSingleNode("/Graphical_Application_Designer_Project/ChangedSinceLastDebugBuild");
				changedSinceLastDebugBuild = xPathNavigator9 != null && Convert.ToBoolean(xPathNavigator9.Value);
				XPathNavigator xPathNavigator10 = xPathNavigator.SelectSingleNode("/Graphical_Application_Designer_Project/DoNotAskForExtension");
				doNotAskForExtension = xPathNavigator10 != null && Convert.ToBoolean(xPathNavigator10.Value);
				XPathNavigator xPathNavigator11 = xPathNavigator.SelectSingleNode("/Graphical_Application_Designer_Project/Extension");
				extension = ((xPathNavigator11 == null) ? string.Empty : xPathNavigator11.Value);
				XPathNavigator xPathNavigator12 = xPathNavigator.SelectSingleNode("/Graphical_Application_Designer_Project/OnlineServicesTextToSpeechEngine");
				onlineServices.TextToSpeechEngine = ((xPathNavigator12 != null) ? EnumHelper.StringToTextToSpeechEngines(xPathNavigator12.Value) : TextToSpeechEngines.None);
				XPathNavigator xPathNavigator13 = xPathNavigator.SelectSingleNode("/Graphical_Application_Designer_Project/OnlineServicesSpeechToTextEngine");
				onlineServices.SpeechToTextEngine = ((xPathNavigator13 != null) ? EnumHelper.StringToSpeechToTextEngines(xPathNavigator13.Value) : SpeechToTextEngines.None);
				XPathNavigator xPathNavigator14 = xPathNavigator.SelectSingleNode("/Graphical_Application_Designer_Project/AmazonClientID");
				onlineServices.AmazonPollySettings.ClientID = ((xPathNavigator14 == null) ? string.Empty : xPathNavigator14.Value);
				XPathNavigator xPathNavigator15 = xPathNavigator.SelectSingleNode("/Graphical_Application_Designer_Project/AmazonClientSecret");
				onlineServices.AmazonPollySettings.ClientSecret = ((xPathNavigator15 == null) ? string.Empty : xPathNavigator15.Value);
				XPathNavigator xPathNavigator16 = xPathNavigator.SelectSingleNode("/Graphical_Application_Designer_Project/AmazonRegion");
				onlineServices.AmazonPollySettings.Region = ((xPathNavigator16 == null) ? TextToSpeechAmazonRegions.USEastOhio : EnumHelper.StringToTextToSpeechAmazonRegion(xPathNavigator16.Value));
				XPathNavigator xPathNavigator17 = xPathNavigator.SelectSingleNode("/Graphical_Application_Designer_Project/AmazonLexicons");
				onlineServices.AmazonPollySettings.Lexicons = ((xPathNavigator17 == null || xPathNavigator17.Value == null) ? new List<string>() : new List<string>(xPathNavigator17.Value.Split(new string[1] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)));
				XPathNavigator xPathNavigator18 = xPathNavigator.SelectSingleNode("/Graphical_Application_Designer_Project/GoogleCloudServiceAccountKeyFileName");
				onlineServices.GoogleCloudSettings.ServiceAccountKeyFileName = ((xPathNavigator18 == null) ? string.Empty : xPathNavigator18.Value);
				XPathNavigator xPathNavigator19 = xPathNavigator.SelectSingleNode("/Graphical_Application_Designer_Project/GoogleCloudServiceAccountKeyJSON");
				onlineServices.GoogleCloudSettings.ServiceAccountKeyJSON = ((xPathNavigator19 == null) ? string.Empty : xPathNavigator19.Value);
				if (flag)
				{
					Save();
				}
			}
			ReportLoadingProgress(70);
			projectManagerControl?.ProjectExplorer.LoadProject(this);
			ReportLoadingProgress(80);
			CreateFlowLoaders();
			ReportLoadingProgress(100);
			this.Loaded?.Invoke(this, EventArgs.Empty);
		}
		catch (Exception)
		{
			this.NotLoaded?.Invoke(this, EventArgs.Empty);
			throw;
		}
	}

	public override void SaveTo(string newProjectDirectory)
	{
		this.fileInfo.CopyTo(System.IO.Path.Combine(newProjectDirectory, relativePath));
		Directory.CreateDirectory(System.IO.Path.Combine(newProjectDirectory, "Output", "Release"));
		Directory.CreateDirectory(System.IO.Path.Combine(newProjectDirectory, "Audio"));
		Directory.CreateDirectory(System.IO.Path.Combine(newProjectDirectory, "Libraries"));
		FileInfo[] files = new DirectoryInfo(System.IO.Path.Combine(projectDirectory, "Audio")).GetFiles();
		foreach (FileInfo fileInfo in files)
		{
			fileInfo.CopyTo(System.IO.Path.Combine(newProjectDirectory, "Audio", fileInfo.Name));
		}
		files = new DirectoryInfo(System.IO.Path.Combine(projectDirectory, "Libraries")).GetFiles();
		foreach (FileInfo fileInfo2 in files)
		{
			fileInfo2.CopyTo(System.IO.Path.Combine(newProjectDirectory, "Libraries", fileInfo2.Name));
		}
		projectDirectory = newProjectDirectory;
		this.fileInfo = new FileInfo(base.Path);
		foreach (AbsFileSystemObject children in childrenList)
		{
			children.SaveTo(newProjectDirectory);
		}
	}

	public override bool Close()
	{
		if (HasChanges)
		{
			Save();
		}
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		this.Closing(this, cancelEventArgs);
		if (cancelEventArgs.Cancel)
		{
			return false;
		}
		projectManagerControl?.ProjectExplorer.UnloadProject();
		this.Closed(this, EventArgs.Empty);
		return true;
	}

	public override void Save()
	{
		childrenList.Sort(new FileSystemObjectComparer());
		using StreamWriter writer = fileInfo.CreateText();
		XmlDocument xmlDocument = new XmlDocument();
		XmlElement newChild = ToXmlElement(xmlDocument);
		xmlDocument.AppendChild(newChild);
		xmlDocument.Save(writer);
		HasChanges = false;
	}

	public override void Move(AbsFileSystemObject destination)
	{
		throw new InvalidOperationException(LocalizedResourceMgr.GetString("FileSystemObjects.Error.CantMoveProject"));
	}

	public override void Delete()
	{
		throw new InvalidOperationException(LocalizedResourceMgr.GetString("FileSystemObjects.Error.CantDeleteProject"));
	}

	public override AbsFileSystemObject GetFileSystemObject(string relativePath)
	{
		if (base.relativePath == relativePath)
		{
			return this;
		}
		foreach (AbsFileSystemObject children in childrenList)
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
		return this;
	}

	public override List<FileObject> GetFileObjectList()
	{
		List<FileObject> list = new List<FileObject>();
		foreach (AbsFileSystemObject children in childrenList)
		{
			list.AddRange(children.GetFileObjectList());
		}
		return list;
	}

	public override List<DialerFileObject> GetDialerFileObjectList()
	{
		List<DialerFileObject> list = new List<DialerFileObject>();
		foreach (AbsFileSystemObject children in childrenList)
		{
			list.AddRange(children.GetDialerFileObjectList());
		}
		return list;
	}

	public override List<CallflowFileObject> GetCallflowFileObjectList()
	{
		List<CallflowFileObject> list = new List<CallflowFileObject>();
		foreach (AbsFileSystemObject children in childrenList)
		{
			list.AddRange(children.GetCallflowFileObjectList());
		}
		return list;
	}

	public override List<ComponentFileObject> GetComponentFileObjectList()
	{
		List<ComponentFileObject> list = new List<ComponentFileObject>();
		foreach (AbsFileSystemObject children in childrenList)
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
		XmlElement xmlElement = doc.CreateElement("Graphical_Application_Designer_Project");
		XmlElement xmlElement2 = doc.CreateElement("Files");
		foreach (AbsFileSystemObject children in childrenList)
		{
			XmlElement newChild = children.ToXmlElement(doc);
			xmlElement2.AppendChild(newChild);
		}
		XmlElement xmlElement3 = doc.CreateElement("Version");
		xmlElement3.InnerText = FileSystemObjectVersionHelper.ToString(FileSystemObjectVersions.V2_1);
		XmlElement newChild2 = doc.CreateElement("Variables");
		XmlElement xmlElement4 = doc.CreateElement("DebugBuildSuccessful");
		xmlElement4.InnerText = debugBuildSuccessful.ToString();
		XmlElement xmlElement5 = doc.CreateElement("ReleaseBuildSuccessful");
		xmlElement5.InnerText = releaseBuildSuccessful.ToString();
		XmlElement xmlElement6 = doc.CreateElement("DebugBuildNumber");
		xmlElement6.InnerText = debugBuildNumber.ToString();
		XmlElement xmlElement7 = doc.CreateElement("ReleaseBuildNumber");
		xmlElement7.InnerText = releaseBuildNumber.ToString();
		XmlElement xmlElement8 = doc.CreateElement("ChangedSinceLastDebugBuild");
		xmlElement8.InnerText = changedSinceLastDebugBuild.ToString();
		XmlElement xmlElement9 = doc.CreateElement("DoNotAskForExtension");
		xmlElement9.InnerText = doNotAskForExtension.ToString();
		XmlElement xmlElement10 = doc.CreateElement("Extension");
		xmlElement10.InnerText = extension;
		XmlElement xmlElement11 = doc.CreateElement("OnlineServicesTextToSpeechEngine");
		xmlElement11.InnerText = EnumHelper.TextToSpeechEnginesToString(onlineServices.TextToSpeechEngine);
		XmlElement xmlElement12 = doc.CreateElement("OnlineServicesSpeechToTextEngine");
		xmlElement12.InnerText = EnumHelper.SpeechToTextEnginesToString(onlineServices.SpeechToTextEngine);
		XmlElement xmlElement13 = doc.CreateElement("AmazonClientID");
		xmlElement13.InnerText = onlineServices.AmazonPollySettings.ClientID;
		XmlElement xmlElement14 = doc.CreateElement("AmazonClientSecret");
		xmlElement14.InnerText = onlineServices.AmazonPollySettings.ClientSecret;
		XmlElement xmlElement15 = doc.CreateElement("AmazonRegion");
		xmlElement15.InnerText = EnumHelper.TextToSpeechAmazonRegionToString(onlineServices.AmazonPollySettings.Region);
		XmlElement xmlElement16 = doc.CreateElement("AmazonLexicons");
		xmlElement16.InnerText = string.Join(Environment.NewLine, onlineServices.AmazonPollySettings.Lexicons);
		XmlElement xmlElement17 = doc.CreateElement("GoogleCloudServiceAccountKeyFileName");
		xmlElement17.InnerText = onlineServices.GoogleCloudSettings.ServiceAccountKeyFileName;
		XmlElement xmlElement18 = doc.CreateElement("GoogleCloudServiceAccountKeyJSON");
		xmlElement18.InnerText = onlineServices.GoogleCloudSettings.ServiceAccountKeyJSON;
		xmlElement.AppendChild(xmlElement3);
		xmlElement.AppendChild(xmlElement2);
		xmlElement.AppendChild(newChild2);
		xmlElement.AppendChild(xmlElement4);
		xmlElement.AppendChild(xmlElement5);
		xmlElement.AppendChild(xmlElement6);
		xmlElement.AppendChild(xmlElement7);
		xmlElement.AppendChild(xmlElement8);
		xmlElement.AppendChild(xmlElement9);
		xmlElement.AppendChild(xmlElement10);
		xmlElement.AppendChild(xmlElement11);
		xmlElement.AppendChild(xmlElement12);
		xmlElement.AppendChild(xmlElement13);
		xmlElement.AppendChild(xmlElement14);
		xmlElement.AppendChild(xmlElement15);
		xmlElement.AppendChild(xmlElement16);
		xmlElement.AppendChild(xmlElement17);
		xmlElement.AppendChild(xmlElement18);
		xmlElement.CreateNavigator().SelectSingleNode("/Variables").InnerXml = SerializationHelper.Serialize(variableListSerializer, variableList);
		return xmlElement;
	}

	public override void CreateFlowLoaders()
	{
		foreach (AbsFileSystemObject children in childrenList)
		{
			children.CreateFlowLoaders();
		}
	}

	public override void ProcessComponentPathChanged(ComponentFileObject componentFileObject)
	{
		foreach (AbsFileSystemObject children in childrenList)
		{
			children.ProcessComponentPathChanged(componentFileObject);
		}
	}

	public FolderObject AddFolder(string folderName)
	{
		FolderObject folderObject = new FolderObject(projectDirectory, folderName, projectManagerControl, this);
		HasChanges = true;
		childrenList.Add(folderObject);
		return folderObject;
	}

	public DialerFileObject AddDialer(string fileName)
	{
		DialerFileObject dialerFileObject = new DialerFileObject(projectDirectory, fileName, projectManagerControl, this, create: true, DialerModes.PowerDialer, 30, 1, string.Empty, PredictiveDialerOptimizations.ForAgents);
		HasChanges = true;
		childrenList.Add(dialerFileObject);
		return dialerFileObject;
	}

	public CallflowFileObject AddCallflow(string fileName)
	{
		CallflowFileObject callflowFileObject = new CallflowFileObject(projectDirectory, fileName, projectManagerControl, this, create: true);
		HasChanges = true;
		childrenList.Add(callflowFileObject);
		return callflowFileObject;
	}

	public ComponentFileObject AddComponent(string fileName)
	{
		ComponentFileObject componentFileObject = new ComponentFileObject(projectDirectory, fileName, projectManagerControl, this, create: true);
		HasChanges = true;
		childrenList.Add(componentFileObject);
		return componentFileObject;
	}

	public void AddChild(AbsFileSystemObject fso)
	{
		childrenList.Add(fso);
		HasChanges = true;
	}

	public void RemoveChild(AbsFileSystemObject fso)
	{
		childrenList.Remove(fso);
		HasChanges = true;
	}

	public bool ChildExists(string fileName)
	{
		foreach (AbsFileSystemObject children in childrenList)
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
		HasChanges = true;
	}

	public string GetFolderPath()
	{
		return projectDirectory;
	}

	public string GetRelativeFolderPath()
	{
		return string.Empty;
	}

	public override int GetExpandedImageIndex()
	{
		return 0;
	}

	public override int GetCollapsedImageIndex()
	{
		return 0;
	}

	public override bool HasDialer()
	{
		foreach (AbsFileSystemObject children in childrenList)
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
		foreach (AbsFileSystemObject children in childrenList)
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
		bool flag = HasDialer();
		bool flag2 = HasCallflow();
		openDialerToolStripMenuItem.Visible = false;
		openCallflowToolStripMenuItem.Visible = false;
		openComponentToolStripMenuItem.Visible = false;
		toolStripSeparator0.Visible = false;
		saveToolStripMenuItem.Visible = true;
		saveAsToolStripMenuItem.Visible = true;
		toolStripSeparator1.Visible = true;
		renameToolStripMenuItem.Visible = true;
		removeDialerToolStripMenuItem.Visible = false;
		removeCallflowToolStripMenuItem.Visible = false;
		removeComponentToolStripMenuItem.Visible = false;
		removeFolderToolStripMenuItem.Visible = false;
		closeProjectToolStripMenuItem.Visible = true;
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
		toolStripSeparator4.Visible = true;
		debugBuildToolStripMenuItem.Visible = false;
		releaseBuildToolStripMenuItem.Visible = false;
		buildAllToolStripMenuItem.Visible = true;
		saveToolStripMenuItem.Enabled = HasChanges;
		renameToolStripMenuItem.Enabled = true;
		closeProjectToolStripMenuItem.Enabled = true;
		newFolderToolStripMenuItem.Enabled = true;
		newDialerToolStripMenuItem.Enabled = !flag;
		newCallflowToolStripMenuItem.Enabled = !flag2;
		newComponentToolStripMenuItem.Enabled = true;
		addExistingDialerToolStripMenuItem.Enabled = !flag;
		addExistingCallflowToolStripMenuItem.Enabled = !flag2;
		addExistingComponentToolStripMenuItem.Enabled = true;
		debugBuildToolStripMenuItem.Enabled = false;
		releaseBuildToolStripMenuItem.Enabled = false;
		buildAllToolStripMenuItem.Enabled = true;
	}

	public override void FillChildNodes(TreeNode parent)
	{
		foreach (AbsFileSystemObject children in childrenList)
		{
			children.FillChildNodes(parent);
		}
	}

	public override bool IsValidNewComponentName(string componentName)
	{
		string componentName2 = (componentName.EndsWith(".comp") ? componentName : (componentName + ".comp"));
		foreach (AbsFileSystemObject children in childrenList)
		{
			if (!children.IsValidNewComponentName(componentName2))
			{
				return false;
			}
		}
		return true;
	}

	public override void NotifyProjectVariableChanges(List<KeyValuePair<string, string>> renamedVariablesList)
	{
		HasChanges = true;
		foreach (AbsFileSystemObject children in childrenList)
		{
			children.NotifyProjectVariableChanges(renamedVariablesList);
		}
	}
}
