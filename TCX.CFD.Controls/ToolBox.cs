using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using System.Windows.Forms;
using TCX.CFD.Properties;

namespace TCX.CFD.Controls;

[ToolboxItem(false)]
public class ToolBox : TreeView, IToolboxService
{
	public class ToolboxNode : TreeNode
	{
		private Image nodeImage;

		private string toolTipCaption;

		private bool onEdit;

		private bool enabled;

		private Type componentType;

		private bool isNew;

		public Image NodeImage
		{
			get
			{
				return nodeImage;
			}
			set
			{
				nodeImage = value;
			}
		}

		public string ToolTipCaption
		{
			get
			{
				return toolTipCaption;
			}
			set
			{
				toolTipCaption = value;
			}
		}

		public bool OnEdit
		{
			get
			{
				return onEdit;
			}
			set
			{
				onEdit = value;
			}
		}

		public bool Enabled => enabled;

		public Type ComponentType
		{
			get
			{
				return componentType;
			}
			set
			{
				componentType = value;
			}
		}

		public bool IsNew
		{
			get
			{
				return isNew;
			}
			set
			{
				isNew = value;
			}
		}

		public bool SetEnabled(bool enabled)
		{
			if (this.enabled == enabled)
			{
				return false;
			}
			this.enabled = enabled;
			return true;
		}

		public ToolboxNode(string text)
		{
			toolTipCaption = string.Empty;
			onEdit = false;
			enabled = true;
			componentType = null;
			base.Text = text;
			isNew = false;
		}

		public ToolboxNode(string text, Image nodeImage, Type componentType, string toolTipCaption, string toolTipText, bool isNew)
		{
			base.Text = text;
			NodeImage = nodeImage;
			this.componentType = componentType;
			this.toolTipCaption = toolTipCaption;
			base.ToolTipText = toolTipText;
			onEdit = false;
			enabled = true;
			this.isNew = isNew;
		}
	}

	private const int NOTOOLTIPS = 128;

	private readonly Font groupHeaderFont;

	private ToolTip toolTip;

	private TreeNode previousNode;

	private readonly TextBox labelEditBox;

	private Hashtable customCreators;

	private IContainer components;

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams obj = base.CreateParams;
			obj.Style |= 128;
			return obj;
		}
	}

	public CategoryNameCollection CategoryNames => null;

	public string SelectedCategory
	{
		get
		{
			return string.Empty;
		}
		set
		{
		}
	}

	private void DrawRootItem(DrawTreeNodeEventArgs e)
	{
		Rectangle bounds = e.Bounds;
		bounds.Y++;
		bounds.Width--;
		bounds.Height -= 3;
		if ((e.State & TreeNodeStates.Marked) == TreeNodeStates.Marked || (e.State & TreeNodeStates.Selected) == TreeNodeStates.Selected)
		{
			using Brush brush = new SolidBrush(Color.FromArgb(225, 230, 232));
			e.Graphics.FillRectangle(brush, bounds);
		}
		else
		{
			using SolidBrush brush2 = new SolidBrush(Color.FromArgb(219, 219, 219));
			e.Graphics.FillRectangle(brush2, bounds);
		}
		if (e.Node.IsExpanded)
		{
			e.Graphics.DrawImage(Resources.Expression_Collapse, new Point(bounds.Left + 3, bounds.Top + 1));
		}
		else
		{
			e.Graphics.DrawImage(Resources.Expression_Expand, new Point(bounds.Left + 3, bounds.Top + 1));
		}
		bounds.Offset(16, 2);
		using SolidBrush brush3 = new SolidBrush(Color.FromArgb(61, 61, 61));
		e.Graphics.DrawString(e.Node.Text, groupHeaderFont, brush3, bounds.Location);
	}

	private void DrawItem(DrawTreeNodeEventArgs e)
	{
		Rectangle bounds = e.Bounds;
		bounds.Width--;
		bounds.Height--;
		ToolboxNode toolboxNode = e.Node as ToolboxNode;
		if (toolboxNode.OnEdit)
		{
			e.Graphics.FillRectangle(SystemBrushes.Window, bounds);
			using (new Pen(Color.FromArgb(49, 106, 197)))
			{
				e.Graphics.DrawRectangle(SystemPens.HotTrack, bounds);
			}
			if (toolboxNode.NodeImage != null)
			{
				e.Graphics.DrawImage(toolboxNode.NodeImage, new Point(e.Bounds.Left + 3, e.Bounds.Top + 2));
			}
			return;
		}
		if (toolboxNode.Enabled)
		{
			if ((e.State & TreeNodeStates.Hot) == TreeNodeStates.Hot)
			{
				using (Brush brush = new SolidBrush(Color.FromArgb(193, 210, 238)))
				{
					e.Graphics.FillRectangle(brush, bounds);
				}
				using (new Pen(Color.FromArgb(49, 106, 197)))
				{
					e.Graphics.DrawRectangle(SystemPens.HotTrack, bounds);
				}
			}
			else if ((e.State & TreeNodeStates.Selected) == TreeNodeStates.Selected)
			{
				using (Brush brush2 = new SolidBrush(Color.FromArgb(225, 230, 232)))
				{
					e.Graphics.FillRectangle(brush2, bounds);
				}
				e.Graphics.DrawRectangle(SystemPens.HotTrack, bounds);
			}
			else
			{
				using Brush brush3 = new SolidBrush(BackColor);
				e.Graphics.FillRectangle(brush3, e.Bounds);
			}
			if (toolboxNode.NodeImage != null)
			{
				e.Graphics.DrawImage(toolboxNode.NodeImage, new Point(e.Bounds.Left + 3, e.Bounds.Top + 2));
			}
			bounds.Offset(20, 3);
			e.Graphics.DrawString(e.Node.Text, Font, SystemBrushes.ControlText, bounds.Location);
		}
		else
		{
			using (Brush brush4 = new SolidBrush(BackColor))
			{
				e.Graphics.FillRectangle(brush4, e.Bounds);
			}
			if (toolboxNode.NodeImage != null)
			{
				ControlPaint.DrawImageDisabled(e.Graphics, toolboxNode.NodeImage, e.Bounds.Left + 3, e.Bounds.Top + 2, BackColor);
			}
			bounds.Offset(20, 3);
			ControlPaint.DrawStringDisabled(e.Graphics, e.Node.Text, Font, BackColor, bounds, new StringFormat());
		}
		if (toolboxNode.IsNew)
		{
			bounds.Offset(e.Graphics.MeasureString(e.Node.Text, Font).ToSize().Width, 0);
			Font font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold, GraphicsUnit.Point, 0);
			e.Graphics.DrawString("ᴺᴱᵂ", font, new SolidBrush(Color.FromArgb(5, 150, 212)), bounds.Location);
		}
	}

	private void EndLabelEdit(bool setNewValues)
	{
		if (setNewValues && labelEditBox.Tag != null)
		{
			(labelEditBox.Tag as ToolboxNode).Text = labelEditBox.Text;
		}
		labelEditBox.Visible = false;
		if (labelEditBox.Tag != null)
		{
			(labelEditBox.Tag as ToolboxNode).OnEdit = false;
			labelEditBox.Tag = null;
		}
		Invalidate();
	}

	private void LabelEditBox_LostFocus(object sender, EventArgs e)
	{
		EndLabelEdit(setNewValues: false);
	}

	private void LabelEditBox_KeyDown(object sender, KeyEventArgs e)
	{
		switch (e.KeyCode)
		{
		case Keys.Escape:
			EndLabelEdit(setNewValues: false);
			break;
		case Keys.Return:
			EndLabelEdit(setNewValues: true);
			break;
		}
	}

	private ToolboxItemCreatorCallback FindToolboxItemCreator(IDataObject dataObject, IDesignerHost host, out string foundFormat)
	{
		foundFormat = string.Empty;
		ToolboxItemCreatorCallback result = null;
		if (customCreators != null)
		{
			IEnumerator enumerator = customCreators.Keys.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string text = (string)enumerator.Current;
				if (dataObject.GetDataPresent(text))
				{
					result = (ToolboxItemCreatorCallback)customCreators[text];
					foundFormat = text;
					break;
				}
			}
		}
		return result;
	}

	private ToolboxItem GetToolboxItem(Type toolType)
	{
		if (toolType == null)
		{
			throw new ArgumentNullException("toolType");
		}
		ToolboxItem toolboxItem = null;
		if ((toolType.IsPublic || toolType.IsNestedPublic) && typeof(IComponent).IsAssignableFrom(toolType) && !toolType.IsAbstract)
		{
			ToolboxItemAttribute toolboxItemAttribute = (ToolboxItemAttribute)TypeDescriptor.GetAttributes(toolType)[typeof(ToolboxItemAttribute)];
			if (toolboxItemAttribute != null && !toolboxItemAttribute.IsDefaultAttribute())
			{
				Type toolboxItemType = toolboxItemAttribute.ToolboxItemType;
				if (toolboxItemType != null)
				{
					ConstructorInfo constructor = toolboxItemType.GetConstructor(new Type[1] { typeof(Type) });
					if (constructor != null)
					{
						toolboxItem = (ToolboxItem)constructor.Invoke(new object[1] { toolType });
					}
					else
					{
						constructor = toolboxItemType.GetConstructor(Array.Empty<Type>());
						if (constructor != null)
						{
							toolboxItem = (ToolboxItem)constructor.Invoke(Array.Empty<object>());
							toolboxItem.Initialize(toolType);
						}
					}
				}
			}
			else if (!toolboxItemAttribute.Equals(ToolboxItemAttribute.None))
			{
				toolboxItem = new ToolboxItem(toolType);
			}
		}
		else if (typeof(ToolboxItem).IsAssignableFrom(toolType))
		{
			try
			{
				toolboxItem = (ToolboxItem)Activator.CreateInstance(toolType, nonPublic: true);
			}
			catch
			{
			}
		}
		return toolboxItem;
	}

	protected override void OnDrawNode(DrawTreeNodeEventArgs e)
	{
		if (e.Node.Level == 0)
		{
			DrawRootItem(e);
		}
		else
		{
			DrawItem(e);
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		TreeViewHitTestInfo treeViewHitTestInfo = HitTest(e.X, e.Y);
		if (treeViewHitTestInfo.Node != null && treeViewHitTestInfo.Location != TreeViewHitTestLocations.PlusMinus && (treeViewHitTestInfo.Node as ToolboxNode).Enabled)
		{
			base.SelectedNode = treeViewHitTestInfo.Node;
			if (treeViewHitTestInfo.Node.Level == 0)
			{
				if (treeViewHitTestInfo.Node.IsExpanded)
				{
					treeViewHitTestInfo.Node.Collapse();
				}
				else
				{
					treeViewHitTestInfo.Node.Expand();
				}
			}
		}
		base.OnMouseDown(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);
		ToolboxNode toolboxNode = GetNodeAt(e.X, e.Y) as ToolboxNode;
		if (e.Button == MouseButtons.Left && toolboxNode != null && toolboxNode.Level > 0 && toolboxNode.Enabled)
		{
			ToolboxItem toolboxItem = GetToolboxItem(toolboxNode.ComponentType);
			IDataObject data = SerializeToolboxItem(toolboxItem) as IDataObject;
			DoDragDrop(data, DragDropEffects.Copy | DragDropEffects.Move);
		}
		else
		{
			if (toolboxNode == null || previousNode == toolboxNode)
			{
				return;
			}
			if (string.IsNullOrEmpty(toolboxNode.ToolTipText))
			{
				if (toolTip != null)
				{
					toolTip.Dispose();
					toolTip = null;
				}
				return;
			}
			previousNode = toolboxNode;
			string toolTipCaption = toolboxNode.ToolTipCaption;
			string toolTipText = toolboxNode.ToolTipText;
			if (toolTip != null && toolTip.Active)
			{
				toolTip.Dispose();
				toolTip = null;
			}
			toolTip = new ToolTip();
			toolTip.ToolTipTitle = toolTipCaption;
			toolTip.SetToolTip(this, toolTipText);
			toolTip.Active = true;
		}
	}

	protected override void OnBeforeLabelEdit(NodeLabelEditEventArgs e)
	{
		e.CancelEdit = true;
		if ((e.Node as ToolboxNode).Enabled)
		{
			(e.Node as ToolboxNode).OnEdit = true;
			labelEditBox.Bounds = new Rectangle(22, e.Node.Bounds.Top + 3, e.Node.Bounds.Width, e.Node.Bounds.Height + 4);
			labelEditBox.Text = e.Node.Text;
			labelEditBox.Visible = true;
			labelEditBox.Tag = e.Node;
			labelEditBox.Focus();
			labelEditBox.SelectAll();
		}
	}

	protected override void OnBeforeSelect(TreeViewCancelEventArgs e)
	{
		if (!(e.Node as ToolboxNode).Enabled)
		{
			e.Cancel = true;
		}
		base.OnBeforeSelect(e);
	}

	public ToolBox()
	{
		SetStyle(ControlStyles.OptimizedDoubleBuffer, value: true);
		SetStyle(ControlStyles.EnableNotifyMessage, value: true);
		groupHeaderFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
		base.ShowLines = false;
		base.HotTracking = true;
		base.FullRowSelect = true;
		base.DrawMode = TreeViewDrawMode.OwnerDrawAll;
		previousNode = null;
		toolTip = new ToolTip();
		labelEditBox = new TextBox();
		labelEditBox.BorderStyle = BorderStyle.None;
		labelEditBox.Visible = false;
		labelEditBox.LostFocus += LabelEditBox_LostFocus;
		labelEditBox.KeyDown += LabelEditBox_KeyDown;
		base.Controls.Add(labelEditBox);
	}

	public void AddCreator(ToolboxItemCreatorCallback creator, string format, IDesignerHost host)
	{
		if (creator == null || format == null)
		{
			throw new ArgumentNullException((creator == null) ? "creator" : "format");
		}
		if (customCreators == null)
		{
			customCreators = new Hashtable();
		}
		else if (customCreators.ContainsKey(format))
		{
			throw new Exception("There is already a creator registered for the format '" + format + "'.");
		}
		customCreators[format] = creator;
	}

	public void AddCreator(ToolboxItemCreatorCallback creator, string format)
	{
		AddCreator(creator, format, null);
	}

	public void AddLinkedToolboxItem(ToolboxItem toolboxItem, string category, IDesignerHost host)
	{
		throw new Exception("The method or operation is not implemented.");
	}

	public void AddLinkedToolboxItem(ToolboxItem toolboxItem, IDesignerHost host)
	{
		throw new Exception("The method or operation is not implemented.");
	}

	public void AddToolboxItem(ToolboxItem toolboxItem, string category)
	{
		throw new Exception("The method or operation is not implemented.");
	}

	public void AddToolboxItem(ToolboxItem toolboxItem)
	{
		throw new Exception("The method or operation is not implemented.");
	}

	public ToolboxItem DeserializeToolboxItem(object serializedObject, IDesignerHost host)
	{
		if (!(serializedObject is IDataObject dataObject))
		{
			return null;
		}
		ToolboxItem toolboxItem = (ToolboxItem)dataObject.GetData(typeof(ToolboxItem));
		if (toolboxItem == null)
		{
			string foundFormat;
			ToolboxItemCreatorCallback toolboxItemCreatorCallback = FindToolboxItemCreator(dataObject, host, out foundFormat);
			if (toolboxItemCreatorCallback != null)
			{
				return toolboxItemCreatorCallback(dataObject, foundFormat);
			}
		}
		return toolboxItem;
	}

	public ToolboxItem DeserializeToolboxItem(object serializedObject)
	{
		return DeserializeToolboxItem(serializedObject, null);
	}

	public ToolboxItem GetSelectedToolboxItem(IDesignerHost host)
	{
		return null;
	}

	public ToolboxItem GetSelectedToolboxItem()
	{
		return GetSelectedToolboxItem(null);
	}

	public ToolboxItemCollection GetToolboxItems(string category, IDesignerHost host)
	{
		return new ToolboxItemCollection(Array.Empty<ToolboxItem>());
	}

	public ToolboxItemCollection GetToolboxItems(string category)
	{
		return new ToolboxItemCollection(Array.Empty<ToolboxItem>());
	}

	public ToolboxItemCollection GetToolboxItems(IDesignerHost host)
	{
		return new ToolboxItemCollection(Array.Empty<ToolboxItem>());
	}

	public ToolboxItemCollection GetToolboxItems()
	{
		return new ToolboxItemCollection(Array.Empty<ToolboxItem>());
	}

	public bool IsSupported(object serializedObject, ICollection filterAttributes)
	{
		return true;
	}

	public bool IsSupported(object serializedObject, IDesignerHost host)
	{
		return true;
	}

	public bool IsToolboxItem(object serializedObject, IDesignerHost host)
	{
		if (!(serializedObject is IDataObject dataObject))
		{
			return false;
		}
		if (dataObject.GetDataPresent(typeof(ToolboxItem)))
		{
			return true;
		}
		if (FindToolboxItemCreator(dataObject, host, out var _) != null)
		{
			return true;
		}
		return false;
	}

	public bool IsToolboxItem(object serializedObject)
	{
		return IsToolboxItem(serializedObject, null);
	}

	public void RemoveCreator(string format, IDesignerHost host)
	{
		if (format == null)
		{
			throw new ArgumentNullException("format");
		}
		if (customCreators != null)
		{
			customCreators.Remove(format);
		}
	}

	public void RemoveCreator(string format)
	{
		RemoveCreator(format, null);
	}

	public void RemoveToolboxItem(ToolboxItem toolboxItem, string category)
	{
		throw new Exception("The method or operation is not implemented.");
	}

	public void RemoveToolboxItem(ToolboxItem toolboxItem)
	{
		throw new Exception("The method or operation is not implemented.");
	}

	public void SelectedToolboxItemUsed()
	{
		SetSelectedToolboxItem(null);
	}

	public object SerializeToolboxItem(ToolboxItem toolboxItem)
	{
		DataObject dataObject = new DataObject();
		dataObject.SetData(typeof(ToolboxItem), toolboxItem);
		return dataObject;
	}

	public bool SetCursor()
	{
		return false;
	}

	public void SetSelectedToolboxItem(ToolboxItem toolboxItem)
	{
		throw new Exception("The method or operation is not implemented.");
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
		base.SuspendLayout();
		base.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
		base.HideSelection = false;
		base.HotTracking = true;
		base.Name = "ToolBox1";
		base.ShowLines = false;
		base.ShowNodeToolTips = true;
		base.ShowPlusMinus = false;
		base.ShowRootLines = false;
		base.Size = new System.Drawing.Size(171, 256);
		base.ResumeLayout(false);
	}
}
