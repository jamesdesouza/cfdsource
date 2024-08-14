using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using TCX.CFD.Classes;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

public class StartPageControl : UserControl
{
	public delegate void OpenProjectHandler();

	public delegate void OpenRecentProjectHandler(string path);

	public delegate void NewProjectHandler(NewProjectTypes projectType);

	private const int PanelMargin = 25;

	private const int MaxCollapsedFeedLength = 8;

	private const int MaxCollapsedRecentLength = 3;

	private const int MaxFeaturedComponents = 3;

	private const string feedUrl = "https://downloads.3cx.com/downloads/v180/CFD/startpage_feed.xml";

	private string feed;

	private bool feedExpanded;

	private bool recentExpanded;

	private readonly Random rnd = new Random();

	private IContainer components;

	private Panel topRightPanel;

	private Panel topLeftPanel;

	private Label lblGetStarted;

	private Label lblStart;

	private Label lblRecent;

	private FlowLayoutPanel recentProjectsFlowLayoutPanel;

	private Panel mainPanel;

	private FlowLayoutPanel getStartedFlowLayoutPanel;

	private FlowLayoutPanel startFlowLayoutPanel;

	private BackgroundWorker updateFeedBackgroundWorker;

	private StartPageGetStartedProgressControl getStartedProgressControl;

	private Panel bottomLeftPanel;

	private Panel bottomRightPanel;

	private Label lblFeaturedComponents;

	private FlowLayoutPanel featuredComponentsFlowLayoutPanel;

	private CircularProgressBar feedProgressBar;

	private Panel topPanel;

	private PictureBox logoPictureBox;

	private Label lblMainTitle;

	public event OpenProjectHandler OnOpenProject;

	public event OpenRecentProjectHandler OnOpenRecentProject;

	public event NewProjectHandler OnNewProject;

	public StartPageControl()
	{
		InitializeComponent();
		base.Name = LocalizedResourceMgr.GetString("StartPageControl.TabName");
		base.AccessibleDescription = LocalizedResourceMgr.GetString("StartPageControl.TabDescription");
		base.Tag = Resources.View_StartPage;
		lblGetStarted.Text = LocalizedResourceMgr.GetString("StartPageControl.lblGetStarted.Text");
		lblStart.Text = LocalizedResourceMgr.GetString("StartPageControl.lblStart.Text");
		lblRecent.Text = LocalizedResourceMgr.GetString("StartPageControl.lblRecent.Text");
		lblFeaturedComponents.Text = LocalizedResourceMgr.GetString("StartPageControl.lblFeaturedComponents.Text");
		feed = Settings.Default.StartPageFeed;
		AddFeedProgressIndicator();
		updateFeedBackgroundWorker.RunWorkerAsync();
		AddStartItems();
		AddRecentProjects();
	}

	private void AddStartItems()
	{
		AddNewOrOpenProjectRow(LocalizedResourceMgr.GetString("StartPageControl.OpenProject.Title"), LocalizedResourceMgr.GetString("StartPageControl.OpenProject.Subtitle"), Resources.StartPage_OpenProject, OpenProject, startFlowLayoutPanel);
		AddNewOrOpenProjectRow(LocalizedResourceMgr.GetString("StartPageControl.NewCallflow.Title"), LocalizedResourceMgr.GetString("StartPageControl.NewCallflow.Subtitle"), Resources.StartPage_NewProject, CreateNewCallflow, startFlowLayoutPanel);
		AddNewOrOpenProjectRow(LocalizedResourceMgr.GetString("StartPageControl.NewDialer.Title"), LocalizedResourceMgr.GetString("StartPageControl.NewDialer.Subtitle"), Resources.StartPage_NewDialer, CreateNewDialer, startFlowLayoutPanel);
	}

	private void AddNewOrOpenProjectRow(string title, string description, Image picture, StartPageNewOrOpenProjectRowControl.RowClickedHandler rowClickedCallback, FlowLayoutPanel panel)
	{
		StartPageNewOrOpenProjectRowControl startPageNewOrOpenProjectRowControl = new StartPageNewOrOpenProjectRowControl
		{
			RowTitle = title,
			RowDescription = description,
			RowPicture = picture,
			Width = startFlowLayoutPanel.Width - 25
		};
		startPageNewOrOpenProjectRowControl.OnRowClicked += rowClickedCallback;
		panel.Controls.Add(startPageNewOrOpenProjectRowControl);
	}

	private void OpenProject()
	{
		this.OnOpenProject?.Invoke();
	}

	private void CreateNewCallflow()
	{
		this.OnNewProject?.Invoke(NewProjectTypes.Callflow);
	}

	private void CreateNewDialer()
	{
		this.OnNewProject?.Invoke(NewProjectTypes.Dialer);
	}

	private void CreateNewTemplate()
	{
		this.OnNewProject?.Invoke(NewProjectTypes.Template);
	}

	public void ReloadRecentProjects()
	{
		recentProjectsFlowLayoutPanel.Controls.Clear();
		AddRecentProjects();
	}

	private void AddRecentProjects()
	{
		List<string> recentProjects = RecentProjectsHelper.GetRecentProjects();
		int num = (recentExpanded ? recentProjects.Count : Math.Min(recentProjects.Count, 3));
		for (int i = 0; i < num; i++)
		{
			AddRecentProject(recentProjects[i]);
		}
		if (!recentExpanded && recentProjects.Count > 3)
		{
			StartPageGetStartedRowControl startPageGetStartedRowControl = new StartPageGetStartedRowControl
			{
				Title = LocalizedResourceMgr.GetString("StartPageControl.loadMore.Text"),
				Width = recentProjectsFlowLayoutPanel.Width - 25
			};
			startPageGetStartedRowControl.OnRowClicked += LoadMoreRecentProjectsRowControl_OnRowClicked;
			recentProjectsFlowLayoutPanel.Controls.Add(startPageGetStartedRowControl);
		}
		if (recentProjectsFlowLayoutPanel.Controls.Count == 0)
		{
			Label value = new Label
			{
				AutoSize = true,
				Font = new Font("Verdana", 9f, FontStyle.Regular, GraphicsUnit.Point, 0),
				Location = new Point(0, 0),
				TextAlign = ContentAlignment.TopLeft,
				Text = LocalizedResourceMgr.GetString("StartPageControl.Recent.Empty")
			};
			recentProjectsFlowLayoutPanel.Controls.Add(value);
		}
	}

	private void LoadMoreRecentProjectsRowControl_OnRowClicked(StartPageGetStartedRowControl rowControl)
	{
		recentExpanded = true;
		recentProjectsFlowLayoutPanel.Controls.RemoveAt(recentProjectsFlowLayoutPanel.Controls.Count - 1);
		List<string> recentProjects = RecentProjectsHelper.GetRecentProjects();
		for (int i = recentProjectsFlowLayoutPanel.Controls.Count; i < recentProjects.Count; i++)
		{
			AddRecentProject(recentProjects[i]);
		}
	}

	private void AddRecentProject(string recentProject)
	{
		try
		{
			StartPageRecentProjectRowControl startPageRecentProjectRowControl = new StartPageRecentProjectRowControl
			{
				ProjectPath = recentProject
			};
			startPageRecentProjectRowControl.OnRecentProjectClicked = (StartPageRecentProjectRowControl.RecentProjectClickedHandler)Delegate.Combine(startPageRecentProjectRowControl.OnRecentProjectClicked, new StartPageRecentProjectRowControl.RecentProjectClickedHandler(OpenRecentProject));
			recentProjectsFlowLayoutPanel.Controls.Add(startPageRecentProjectRowControl);
		}
		catch (Exception ex)
		{
			MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("StartPageControl.MessageBox.Error.AddingRecentProject"), recentProject, ex.Message), LocalizedResourceMgr.GetString("StartPageControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void OpenRecentProject(string recentProject)
	{
		this.OnOpenRecentProject?.Invoke(recentProject);
	}

	private void FlowLayoutPanel_SizeChanged(object sender, EventArgs e)
	{
		FlowLayoutPanel flowLayoutPanel = sender as FlowLayoutPanel;
		foreach (Control control in flowLayoutPanel.Controls)
		{
			control.Width = flowLayoutPanel.Width - 25;
		}
	}

	private void AddFeedProgressIndicator()
	{
		feedProgressBar.Visible = true;
		feedProgressBar.StartAnimation();
		if (string.IsNullOrEmpty(feed))
		{
			getStartedProgressControl.Width = getStartedFlowLayoutPanel.Width - 25;
			getStartedProgressControl.Visible = true;
			getStartedProgressControl.BringToFront();
		}
		else
		{
			AddFeedItems();
		}
	}

	private void UpdateFeedBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
	{
		using Stream stream = WebRequest.Create("https://downloads.3cx.com/downloads/v180/CFD/startpage_feed.xml").GetResponse().GetResponseStream();
		using StreamReader streamReader = new StreamReader(stream);
		e.Result = streamReader.ReadToEnd();
	}

	private List<Tuple<string, string>> ProcessGetStartedFeed()
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(XmlHelper.SanitizeXmlString(feed));
		List<Tuple<string, string>> list = new List<Tuple<string, string>>();
		XPathNavigator xPathNavigator = xmlDocument.CreateNavigator().SelectSingleNode("/cfd_feed/get_started");
		if (xPathNavigator != null)
		{
			XPathNodeIterator xPathNodeIterator = xPathNavigator.SelectChildren(XPathNodeType.Element);
			while (xPathNodeIterator.MoveNext())
			{
				string attribute = xPathNodeIterator.Current.GetAttribute("title", "");
				string attribute2 = xPathNodeIterator.Current.GetAttribute("link", "");
				list.Add(new Tuple<string, string>(attribute, attribute2));
			}
		}
		return list;
	}

	private void AddFeedItems()
	{
		try
		{
			AddGetStarted();
			AddFeaturedComponents();
		}
		catch (Exception ex)
		{
			MessageBox.Show(LocalizedResourceMgr.GetString("StartPageControl.MessageBox.Error.AddingFeedItems") + ex.Message, LocalizedResourceMgr.GetString("StartPageControl.MessageBox.Title"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void AddGetStarted()
	{
		List<Tuple<string, string>> list = ProcessGetStartedFeed();
		int num = (feedExpanded ? list.Count : Math.Min(list.Count, 8));
		if (num < 8)
		{
			while (getStartedFlowLayoutPanel.Controls.Count > num)
			{
				getStartedFlowLayoutPanel.Controls.RemoveAt(getStartedFlowLayoutPanel.Controls.Count - 1);
			}
		}
		for (int i = 0; i < num; i++)
		{
			if (i < getStartedFlowLayoutPanel.Controls.Count)
			{
				StartPageGetStartedRowControl obj = getStartedFlowLayoutPanel.Controls[i] as StartPageGetStartedRowControl;
				obj.Title = list[i].Item1;
				obj.Tag = list[i].Item2;
				continue;
			}
			StartPageGetStartedRowControl startPageGetStartedRowControl = new StartPageGetStartedRowControl
			{
				Title = list[i].Item1,
				Tag = list[i].Item2,
				Width = getStartedFlowLayoutPanel.Width - 25
			};
			startPageGetStartedRowControl.OnRowClicked += GetStartedRowControl_OnRowClicked;
			getStartedFlowLayoutPanel.Controls.Add(startPageGetStartedRowControl);
		}
		if (!feedExpanded && list.Count > 8 && getStartedFlowLayoutPanel.Controls.Count == 8)
		{
			StartPageGetStartedRowControl startPageGetStartedRowControl2 = new StartPageGetStartedRowControl
			{
				Title = LocalizedResourceMgr.GetString("StartPageControl.loadMore.Text"),
				Width = getStartedFlowLayoutPanel.Width - 25
			};
			startPageGetStartedRowControl2.OnRowClicked += LoadMoreFeedItemsRowControl_OnRowClicked;
			getStartedFlowLayoutPanel.Controls.Add(startPageGetStartedRowControl2);
		}
	}

	private void AddFeaturedComponents()
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(XmlHelper.SanitizeXmlString(feed));
		List<StartPageFeaturedComponent> list = new List<StartPageFeaturedComponent>();
		XPathNavigator xPathNavigator = xmlDocument.CreateNavigator().SelectSingleNode("/cfd_feed/featured_components");
		if (xPathNavigator != null)
		{
			XPathNodeIterator xPathNodeIterator = xPathNavigator.SelectChildren(XPathNodeType.Element);
			while (xPathNodeIterator.MoveNext())
			{
				string attribute = xPathNodeIterator.Current.GetAttribute("id", "");
				string attribute2 = xPathNodeIterator.Current.GetAttribute("title", "");
				string attribute3 = xPathNodeIterator.Current.GetAttribute("description", "");
				string attribute4 = xPathNodeIterator.Current.GetAttribute("link", "");
				list.Add(new StartPageFeaturedComponent(attribute, attribute2, attribute3, attribute4));
			}
		}
		for (int i = 0; i < 3; i++)
		{
			int index = rnd.Next(list.Count);
			StartPageFeaturedComponent featuredComponent = list[index];
			list.RemoveAt(index);
			if (i < featuredComponentsFlowLayoutPanel.Controls.Count)
			{
				(featuredComponentsFlowLayoutPanel.Controls[i] as StartPageFeaturedComponentRowControl).FeaturedComponent = featuredComponent;
				continue;
			}
			StartPageFeaturedComponentRowControl value = new StartPageFeaturedComponentRowControl
			{
				FeaturedComponent = featuredComponent,
				Width = featuredComponentsFlowLayoutPanel.Width - 25
			};
			featuredComponentsFlowLayoutPanel.Controls.Add(value);
		}
	}

	private void GetStartedRowControl_OnRowClicked(StartPageGetStartedRowControl rowControl)
	{
		Process.Start(rowControl.Tag as string);
	}

	private void LoadMoreFeedItemsRowControl_OnRowClicked(StartPageGetStartedRowControl rowControl)
	{
		feedExpanded = true;
		getStartedFlowLayoutPanel.Controls.RemoveAt(getStartedFlowLayoutPanel.Controls.Count - 1);
		List<Tuple<string, string>> list = ProcessGetStartedFeed();
		for (int i = getStartedFlowLayoutPanel.Controls.Count; i < list.Count; i++)
		{
			StartPageGetStartedRowControl startPageGetStartedRowControl = new StartPageGetStartedRowControl
			{
				Title = list[i].Item1,
				Tag = list[i].Item2,
				Width = getStartedFlowLayoutPanel.Width - 25
			};
			startPageGetStartedRowControl.OnRowClicked += GetStartedRowControl_OnRowClicked;
			getStartedFlowLayoutPanel.Controls.Add(startPageGetStartedRowControl);
		}
	}

	private void UpdateFeedBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		feedProgressBar.Visible = false;
		feedProgressBar.StopAnimation();
		getStartedProgressControl.Visible = false;
		getStartedProgressControl.SendToBack();
		if (e.Error == null)
		{
			feed = e.Result as string;
			AddFeedItems();
			Settings.Default.StartPageFeed = feed;
			Settings.Default.Save();
		}
		else if (string.IsNullOrEmpty(feed))
		{
			CreateRetryPanel(getStartedFlowLayoutPanel, LocalizedResourceMgr.GetString("StartPageControl.Error.DownloadingArticleVideos"));
			CreateRetryPanel(featuredComponentsFlowLayoutPanel, LocalizedResourceMgr.GetString("StartPageControl.Error.DownloadingFeaturedComponents"));
		}
	}

	private void CreateRetryPanel(FlowLayoutPanel parentPanel, string errorText)
	{
		Label value = new Label
		{
			AutoSize = true,
			Font = new Font("Verdana", 9f, FontStyle.Regular, GraphicsUnit.Point, 0),
			Location = new Point(0, 0),
			Margin = new Padding(3, 0, 0, 7),
			TextAlign = ContentAlignment.TopLeft,
			Text = errorText
		};
		LinkLabel linkLabel = new LinkLabel
		{
			AutoSize = true,
			Font = new Font("Verdana", 9f, FontStyle.Regular, GraphicsUnit.Point, 0),
			LinkColor = Color.FromArgb(5, 151, 212),
			VisitedLinkColor = Color.FromArgb(5, 151, 212),
			Location = new Point(0, 0),
			TextAlign = ContentAlignment.TopLeft,
			Text = LocalizedResourceMgr.GetString("StartPageControl.Error.Retry")
		};
		linkLabel.LinkClicked += RetryLink_Click;
		parentPanel.Controls.Add(value);
		parentPanel.Controls.Add(linkLabel);
	}

	private void RetryLink_Click(object sender, EventArgs e)
	{
		getStartedFlowLayoutPanel.Controls.Clear();
		featuredComponentsFlowLayoutPanel.Controls.Clear();
		AddFeedProgressIndicator();
		updateFeedBackgroundWorker.RunWorkerAsync();
	}

	private void StartPageControl_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-workspace/");
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TCX.CFD.Controls.StartPageControl));
		this.topRightPanel = new System.Windows.Forms.Panel();
		this.startFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
		this.lblStart = new System.Windows.Forms.Label();
		this.topLeftPanel = new System.Windows.Forms.Panel();
		this.feedProgressBar = new TCX.CFD.Controls.CircularProgressBar();
		this.getStartedFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
		this.lblGetStarted = new System.Windows.Forms.Label();
		this.getStartedProgressControl = new TCX.CFD.Controls.StartPageGetStartedProgressControl();
		this.recentProjectsFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
		this.lblRecent = new System.Windows.Forms.Label();
		this.mainPanel = new System.Windows.Forms.Panel();
		this.topPanel = new System.Windows.Forms.Panel();
		this.lblMainTitle = new System.Windows.Forms.Label();
		this.logoPictureBox = new System.Windows.Forms.PictureBox();
		this.bottomLeftPanel = new System.Windows.Forms.Panel();
		this.bottomRightPanel = new System.Windows.Forms.Panel();
		this.featuredComponentsFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
		this.lblFeaturedComponents = new System.Windows.Forms.Label();
		this.updateFeedBackgroundWorker = new System.ComponentModel.BackgroundWorker();
		this.topRightPanel.SuspendLayout();
		this.topLeftPanel.SuspendLayout();
		this.mainPanel.SuspendLayout();
		this.topPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.logoPictureBox).BeginInit();
		this.bottomLeftPanel.SuspendLayout();
		this.bottomRightPanel.SuspendLayout();
		base.SuspendLayout();
		this.topRightPanel.Controls.Add(this.startFlowLayoutPanel);
		this.topRightPanel.Controls.Add(this.lblStart);
		this.topRightPanel.Location = new System.Drawing.Point(630, 97);
		this.topRightPanel.Name = "topRightPanel";
		this.topRightPanel.Size = new System.Drawing.Size(378, 365);
		this.topRightPanel.TabIndex = 2;
		this.startFlowLayoutPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.startFlowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
		this.startFlowLayoutPanel.Location = new System.Drawing.Point(26, 50);
		this.startFlowLayoutPanel.Name = "startFlowLayoutPanel";
		this.startFlowLayoutPanel.Size = new System.Drawing.Size(349, 312);
		this.startFlowLayoutPanel.TabIndex = 3;
		this.startFlowLayoutPanel.WrapContents = false;
		this.startFlowLayoutPanel.SizeChanged += new System.EventHandler(FlowLayoutPanel_SizeChanged);
		this.lblStart.AutoSize = true;
		this.lblStart.Font = new System.Drawing.Font("Verdana", 14f);
		this.lblStart.ForeColor = System.Drawing.Color.FromArgb(5, 151, 212);
		this.lblStart.Location = new System.Drawing.Point(25, 10);
		this.lblStart.Name = "lblStart";
		this.lblStart.Size = new System.Drawing.Size(71, 29);
		this.lblStart.TabIndex = 0;
		this.lblStart.Text = "Start";
		this.topLeftPanel.Controls.Add(this.feedProgressBar);
		this.topLeftPanel.Controls.Add(this.getStartedFlowLayoutPanel);
		this.topLeftPanel.Controls.Add(this.lblGetStarted);
		this.topLeftPanel.Controls.Add(this.getStartedProgressControl);
		this.topLeftPanel.Location = new System.Drawing.Point(3, 97);
		this.topLeftPanel.Name = "topLeftPanel";
		this.topLeftPanel.Size = new System.Drawing.Size(621, 365);
		this.topLeftPanel.TabIndex = 0;
		this.feedProgressBar.Location = new System.Drawing.Point(202, 10);
		this.feedProgressBar.Name = "feedProgressBar";
		this.feedProgressBar.Size = new System.Drawing.Size(30, 30);
		this.feedProgressBar.TabIndex = 6;
		this.feedProgressBar.Visible = false;
		this.getStartedFlowLayoutPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.getStartedFlowLayoutPanel.AutoScroll = true;
		this.getStartedFlowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
		this.getStartedFlowLayoutPanel.Location = new System.Drawing.Point(46, 50);
		this.getStartedFlowLayoutPanel.Name = "getStartedFlowLayoutPanel";
		this.getStartedFlowLayoutPanel.Size = new System.Drawing.Size(572, 312);
		this.getStartedFlowLayoutPanel.TabIndex = 1;
		this.getStartedFlowLayoutPanel.WrapContents = false;
		this.getStartedFlowLayoutPanel.SizeChanged += new System.EventHandler(FlowLayoutPanel_SizeChanged);
		this.lblGetStarted.AutoSize = true;
		this.lblGetStarted.Font = new System.Drawing.Font("Verdana", 14f);
		this.lblGetStarted.ForeColor = System.Drawing.Color.FromArgb(5, 151, 212);
		this.lblGetStarted.Location = new System.Drawing.Point(46, 10);
		this.lblGetStarted.Name = "lblGetStarted";
		this.lblGetStarted.Size = new System.Drawing.Size(150, 29);
		this.lblGetStarted.TabIndex = 0;
		this.lblGetStarted.Text = "Get Started";
		this.getStartedProgressControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.getStartedProgressControl.Location = new System.Drawing.Point(26, 50);
		this.getStartedProgressControl.Name = "getStartedProgressControl";
		this.getStartedProgressControl.Size = new System.Drawing.Size(592, 312);
		this.getStartedProgressControl.TabIndex = 5;
		this.recentProjectsFlowLayoutPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.recentProjectsFlowLayoutPanel.AutoScroll = true;
		this.recentProjectsFlowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
		this.recentProjectsFlowLayoutPanel.Location = new System.Drawing.Point(46, 50);
		this.recentProjectsFlowLayoutPanel.Name = "recentProjectsFlowLayoutPanel";
		this.recentProjectsFlowLayoutPanel.Size = new System.Drawing.Size(572, 231);
		this.recentProjectsFlowLayoutPanel.TabIndex = 1;
		this.recentProjectsFlowLayoutPanel.WrapContents = false;
		this.recentProjectsFlowLayoutPanel.SizeChanged += new System.EventHandler(FlowLayoutPanel_SizeChanged);
		this.lblRecent.AutoSize = true;
		this.lblRecent.Font = new System.Drawing.Font("Verdana", 14f);
		this.lblRecent.ForeColor = System.Drawing.Color.FromArgb(5, 151, 212);
		this.lblRecent.Location = new System.Drawing.Point(46, 10);
		this.lblRecent.Name = "lblRecent";
		this.lblRecent.Size = new System.Drawing.Size(94, 29);
		this.lblRecent.TabIndex = 0;
		this.lblRecent.Text = "Recent";
		this.mainPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.mainPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.mainPanel.Controls.Add(this.topPanel);
		this.mainPanel.Controls.Add(this.bottomLeftPanel);
		this.mainPanel.Controls.Add(this.bottomRightPanel);
		this.mainPanel.Controls.Add(this.topLeftPanel);
		this.mainPanel.Controls.Add(this.topRightPanel);
		this.mainPanel.Location = new System.Drawing.Point(3, 3);
		this.mainPanel.Name = "mainPanel";
		this.mainPanel.Size = new System.Drawing.Size(1013, 772);
		this.mainPanel.TabIndex = 13;
		this.topPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.topPanel.BackColor = System.Drawing.Color.FromArgb(51, 51, 51);
		this.topPanel.Controls.Add(this.lblMainTitle);
		this.topPanel.Controls.Add(this.logoPictureBox);
		this.topPanel.Location = new System.Drawing.Point(0, 0);
		this.topPanel.Name = "topPanel";
		this.topPanel.Size = new System.Drawing.Size(1012, 72);
		this.topPanel.TabIndex = 4;
		this.lblMainTitle.AutoSize = true;
		this.lblMainTitle.Font = new System.Drawing.Font("Verdana", 16.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblMainTitle.ForeColor = System.Drawing.Color.White;
		this.lblMainTitle.Location = new System.Drawing.Point(130, 24);
		this.lblMainTitle.Name = "lblMainTitle";
		this.lblMainTitle.Size = new System.Drawing.Size(282, 34);
		this.lblMainTitle.TabIndex = 1;
		this.lblMainTitle.Text = "Call Flow Designer";
		this.logoPictureBox.Image = (System.Drawing.Image)resources.GetObject("logoPictureBox.Image");
		this.logoPictureBox.Location = new System.Drawing.Point(28, 11);
		this.logoPictureBox.Name = "logoPictureBox";
		this.logoPictureBox.Size = new System.Drawing.Size(112, 50);
		this.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.logoPictureBox.TabIndex = 0;
		this.logoPictureBox.TabStop = false;
		this.bottomLeftPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.bottomLeftPanel.Controls.Add(this.lblRecent);
		this.bottomLeftPanel.Controls.Add(this.recentProjectsFlowLayoutPanel);
		this.bottomLeftPanel.Location = new System.Drawing.Point(3, 468);
		this.bottomLeftPanel.Name = "bottomLeftPanel";
		this.bottomLeftPanel.Size = new System.Drawing.Size(621, 299);
		this.bottomLeftPanel.TabIndex = 1;
		this.bottomRightPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.bottomRightPanel.Controls.Add(this.featuredComponentsFlowLayoutPanel);
		this.bottomRightPanel.Controls.Add(this.lblFeaturedComponents);
		this.bottomRightPanel.Location = new System.Drawing.Point(630, 468);
		this.bottomRightPanel.Name = "bottomRightPanel";
		this.bottomRightPanel.Size = new System.Drawing.Size(378, 299);
		this.bottomRightPanel.TabIndex = 3;
		this.featuredComponentsFlowLayoutPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.featuredComponentsFlowLayoutPanel.AutoScroll = true;
		this.featuredComponentsFlowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
		this.featuredComponentsFlowLayoutPanel.Location = new System.Drawing.Point(26, 50);
		this.featuredComponentsFlowLayoutPanel.Name = "featuredComponentsFlowLayoutPanel";
		this.featuredComponentsFlowLayoutPanel.Size = new System.Drawing.Size(349, 231);
		this.featuredComponentsFlowLayoutPanel.TabIndex = 4;
		this.featuredComponentsFlowLayoutPanel.WrapContents = false;
		this.lblFeaturedComponents.AutoSize = true;
		this.lblFeaturedComponents.Font = new System.Drawing.Font("Verdana", 14f);
		this.lblFeaturedComponents.ForeColor = System.Drawing.Color.FromArgb(5, 151, 212);
		this.lblFeaturedComponents.Location = new System.Drawing.Point(28, 10);
		this.lblFeaturedComponents.Name = "lblFeaturedComponents";
		this.lblFeaturedComponents.Size = new System.Drawing.Size(276, 29);
		this.lblFeaturedComponents.TabIndex = 0;
		this.lblFeaturedComponents.Text = "Featured Components";
		this.updateFeedBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(UpdateFeedBackgroundWorker_DoWork);
		this.updateFeedBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(UpdateFeedBackgroundWorker_RunWorkerCompleted);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.Controls.Add(this.mainPanel);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "StartPageControl";
		base.Size = new System.Drawing.Size(1019, 778);
		base.HelpRequested += new System.Windows.Forms.HelpEventHandler(StartPageControl_HelpRequested);
		this.topRightPanel.ResumeLayout(false);
		this.topRightPanel.PerformLayout();
		this.topLeftPanel.ResumeLayout(false);
		this.topLeftPanel.PerformLayout();
		this.mainPanel.ResumeLayout(false);
		this.topPanel.ResumeLayout(false);
		this.topPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.logoPictureBox).EndInit();
		this.bottomLeftPanel.ResumeLayout(false);
		this.bottomLeftPanel.PerformLayout();
		this.bottomRightPanel.ResumeLayout(false);
		this.bottomRightPanel.PerformLayout();
		base.ResumeLayout(false);
	}
}
