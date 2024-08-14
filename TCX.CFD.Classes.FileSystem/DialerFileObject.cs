using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using TCX.CFD.Controls;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.FileSystem;

public class DialerFileObject : FileObject
{
	private DialerModes dialerMode;

	private int pauseBetweenDialerExecution;

	private int parallelDialers;

	private string predictiveDialerQueue;

	private PredictiveDialerOptimizations optimization;

	[Category("Dialer")]
	[Description("Specify the mode for this dialer. Use PowerDialer to make calls at a steady pace. Use PredictiveDialer to adjust the make call pace to the number of agents available in a queue.")]
	public DialerModes DialerMode
	{
		get
		{
			return dialerMode;
		}
		set
		{
			dialerMode = value;
		}
	}

	[Category("Power Dialer")]
	[Description("3CX Phone System will execute this dialer flow once, then wait the number of seconds specified here, and then execute this dialer flow again. This process will repeat indefinitely. Only valid when the DialerMode is set to PowerDialer.")]
	public int PauseBetweenDialerExecution
	{
		get
		{
			return pauseBetweenDialerExecution;
		}
		set
		{
			if (value <= 0)
			{
				throw new ArgumentException(LocalizedResourceMgr.GetString("FileSystemObjects.Error.InvalidPauseBetweenDialerExecution"));
			}
			pauseBetweenDialerExecution = value;
		}
	}

	[Category("Power Dialer")]
	[Description("3CX Phone System will execute this number of dialer flows in parallel. Only valid when the DialerMode is set to PowerDialer.")]
	public int ParallelDialers
	{
		get
		{
			return parallelDialers;
		}
		set
		{
			if (value <= 0)
			{
				throw new ArgumentException(LocalizedResourceMgr.GetString("FileSystemObjects.Error.InvalidParallelDialers"));
			}
			parallelDialers = value;
		}
	}

	[Category("Predictive Dialer")]
	[Description("The dialer will monitor the agents of this queue, and the number of calls waiting to be served, and will adapt the make call pace to these variables. Only valid when the DialerMode is set to PredictiveDialer.")]
	public string Queue
	{
		get
		{
			return predictiveDialerQueue;
		}
		set
		{
			predictiveDialerQueue = ((value.Length > 2 && value.StartsWith("\"") && value.EndsWith("\"")) ? value.Substring(1, value.Length - 2) : value);
		}
	}

	[Category("Predictive Dialer")]
	[Description("Controls the predictive dialer behavior. Optimize for agents when you prefer to keep your agents as full as possible, causing that a few callees wait a short time in the queue. Optimize for callees when you want to minimize as much as possible callees waiting on the queue, possibly increasing the agents not talking time. Only valid when the DialerMode is set to PredictiveDialer.")]
	public PredictiveDialerOptimizations Optimization
	{
		get
		{
			return optimization;
		}
		set
		{
			optimization = value;
		}
	}

	public DialerFileObject(string projectDirectory, string relativePath, ProjectManagerControl projectManagerControl, IFileSystemObjectContainer parent, bool create, DialerModes dialerMode, int pauseBetweenDialerExecution, int parallelDialers, string predictiveDialerQueue, PredictiveDialerOptimizations optimization)
		: base(projectDirectory, relativePath, projectManagerControl, parent, create, ".dialer", needsDisconnectHandlerFlow: false)
	{
		this.dialerMode = dialerMode;
		this.pauseBetweenDialerExecution = pauseBetweenDialerExecution;
		this.parallelDialers = parallelDialers;
		this.predictiveDialerQueue = predictiveDialerQueue;
		this.optimization = optimization;
		projectManagerControl?.RegisterDialerFileObject(this);
	}

	protected override void NotifyComponentPathChanged()
	{
	}

	public override List<DialerFileObject> GetDialerFileObjectList()
	{
		return new List<DialerFileObject> { this };
	}

	public override List<CallflowFileObject> GetCallflowFileObjectList()
	{
		return new List<CallflowFileObject>();
	}

	public override List<ComponentFileObject> GetComponentFileObjectList()
	{
		return new List<ComponentFileObject>();
	}

	public override XmlElement ToXmlElement(XmlDocument doc)
	{
		XmlElement xmlElement = doc.CreateElement("File");
		xmlElement.SetAttribute("path", base.RelativePath);
		xmlElement.SetAttribute("type", "dialer");
		xmlElement.SetAttribute("dialer_mode", EnumHelper.DialerModeToString(dialerMode));
		xmlElement.SetAttribute("pause_between_dialer_execution", pauseBetweenDialerExecution.ToString());
		xmlElement.SetAttribute("parallel_dialers", parallelDialers.ToString());
		xmlElement.SetAttribute("predictive_dialer_queue", predictiveDialerQueue);
		xmlElement.SetAttribute("predictive_dialer_optimization", EnumHelper.PredictiveDialerOptimizationToString(optimization));
		return xmlElement;
	}

	public override int GetExpandedImageIndex()
	{
		return 3;
	}

	public override int GetCollapsedImageIndex()
	{
		return 3;
	}

	public override void FillChildNodes(TreeNode parent)
	{
		projectManagerControl?.ProjectExplorer.AddNode(parent, this);
	}

	public override bool IsValidNewComponentName(string componentName)
	{
		return true;
	}

	public override bool HasDialer()
	{
		return true;
	}

	public override bool HasCallflow()
	{
		return false;
	}

	public override void ConfigureContextMenu(ToolStripMenuItem openDialerToolStripMenuItem, ToolStripMenuItem openCallflowToolStripMenuItem, ToolStripMenuItem openComponentToolStripMenuItem, ToolStripSeparator toolStripSeparator0, ToolStripMenuItem saveToolStripMenuItem, ToolStripMenuItem saveAsToolStripMenuItem, ToolStripSeparator toolStripSeparator1, ToolStripMenuItem renameToolStripMenuItem, ToolStripMenuItem removeDialerToolStripMenuItem, ToolStripMenuItem removeCallflowToolStripMenuItem, ToolStripMenuItem removeComponentToolStripMenuItem, ToolStripMenuItem removeFolderToolStripMenuItem, ToolStripMenuItem closeProjectToolStripMenuItem, ToolStripMenuItem closeDialerToolStripMenuItem, ToolStripMenuItem closeCallflowToolStripMenuItem, ToolStripMenuItem closeComponentToolStripMenuItem, ToolStripSeparator toolStripSeparator2, ToolStripMenuItem newFolderToolStripMenuItem, ToolStripMenuItem newDialerToolStripMenuItem, ToolStripMenuItem newCallflowToolStripMenuItem, ToolStripMenuItem newComponentToolStripMenuItem, ToolStripSeparator toolStripSeparator3, ToolStripMenuItem addExistingDialerToolStripMenuItem, ToolStripMenuItem addExistingCallflowToolStripMenuItem, ToolStripMenuItem addExistingComponentToolStripMenuItem, ToolStripSeparator toolStripSeparator4, ToolStripMenuItem debugBuildToolStripMenuItem, ToolStripMenuItem releaseBuildToolStripMenuItem, ToolStripMenuItem buildAllToolStripMenuItem)
	{
		openDialerToolStripMenuItem.Visible = true;
		openCallflowToolStripMenuItem.Visible = false;
		openComponentToolStripMenuItem.Visible = false;
		toolStripSeparator0.Visible = true;
		saveToolStripMenuItem.Visible = true;
		saveAsToolStripMenuItem.Visible = false;
		toolStripSeparator1.Visible = true;
		renameToolStripMenuItem.Visible = true;
		removeDialerToolStripMenuItem.Visible = true;
		removeCallflowToolStripMenuItem.Visible = false;
		removeComponentToolStripMenuItem.Visible = false;
		removeFolderToolStripMenuItem.Visible = false;
		closeProjectToolStripMenuItem.Visible = false;
		closeDialerToolStripMenuItem.Visible = true;
		closeCallflowToolStripMenuItem.Visible = false;
		closeComponentToolStripMenuItem.Visible = false;
		toolStripSeparator2.Visible = false;
		newFolderToolStripMenuItem.Visible = false;
		newDialerToolStripMenuItem.Visible = false;
		newCallflowToolStripMenuItem.Visible = false;
		newComponentToolStripMenuItem.Visible = false;
		toolStripSeparator3.Visible = false;
		addExistingDialerToolStripMenuItem.Visible = false;
		addExistingCallflowToolStripMenuItem.Visible = false;
		addExistingComponentToolStripMenuItem.Visible = false;
		toolStripSeparator4.Visible = false;
		debugBuildToolStripMenuItem.Visible = false;
		releaseBuildToolStripMenuItem.Visible = false;
		buildAllToolStripMenuItem.Visible = false;
		openDialerToolStripMenuItem.Enabled = true;
		saveToolStripMenuItem.Enabled = HasChanges;
		renameToolStripMenuItem.Enabled = true;
		removeDialerToolStripMenuItem.Enabled = true;
		closeDialerToolStripMenuItem.Enabled = isEditing;
	}

	public override Image GetImage()
	{
		return Resources.Dialer;
	}
}
