using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Xml.Serialization;
using TCX.CFD.Classes.Compiler;
using TCX.CFD.Classes.Editors;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Components;

[DesignerSerializer(typeof(ActivityMarkupSerializer), typeof(WorkflowMarkupSerializer))]
[Designer(typeof(UserComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(UserComponentToolboxItem))]
[ToolboxBitmap(typeof(UserComponent), "Resources.UserComponent.png")]
[ActivityValidator(typeof(ComponentValidator))]
public class UserComponent : AbsVadActivity, ICustomTypeDescriptor
{
	private class UserComponentPropertyDescriptor : PropertyDescriptor
	{
		private readonly Type propertyType;

		private readonly List<UserProperty> publicProperties;

		public override Type ComponentType => typeof(UserComponent);

		public override bool IsReadOnly => false;

		public override Type PropertyType => propertyType;

		private bool PropertyExists(string name)
		{
			foreach (UserProperty publicProperty in publicProperties)
			{
				if (publicProperty.Name == name)
				{
					return true;
				}
			}
			return false;
		}

		private object GetPropertyValue(string name)
		{
			foreach (UserProperty publicProperty in publicProperties)
			{
				if (publicProperty.Name == name)
				{
					return publicProperty.Value;
				}
			}
			return null;
		}

		private void SetPropertyValue(string name, object value)
		{
			foreach (UserProperty publicProperty in publicProperties)
			{
				if (publicProperty.Name == name)
				{
					publicProperty.Value = value;
					break;
				}
			}
		}

		public UserComponentPropertyDescriptor(Type propertyType, List<UserProperty> publicProperties, string name, Attribute[] attributes)
			: base(name, attributes)
		{
			this.propertyType = propertyType;
			this.publicProperties = publicProperties;
		}

		public override bool CanResetValue(object component)
		{
			return false;
		}

		public override object GetValue(object component)
		{
			if (!PropertyExists(Name))
			{
				publicProperties.Add(new UserProperty(Name, null));
			}
			return GetPropertyValue(Name);
		}

		public override void ResetValue(object component)
		{
		}

		public override void SetValue(object component, object value)
		{
			object propertyValue = GetPropertyValue(Name);
			if (propertyValue != value)
			{
				IDesignerHost designerHost = (component as DependencyObject).Site.Container as IDesignerHost;
				if (!(designerHost.GetService(typeof(IComponentChangeService)) is IComponentChangeService componentChangeService))
				{
					throw new InvalidOperationException(LocalizedResourceMgr.GetString("Components.Component.Error.ComponentChangeServiceNotFound"));
				}
				DesignerTransaction designerTransaction = designerHost.CreateTransaction("Change of property " + Name);
				componentChangeService.OnComponentChanging(component, this);
				SetPropertyValue(Name, value);
				componentChangeService.OnComponentChanged(component, this, propertyValue, value);
				designerTransaction.Commit();
			}
		}

		public override bool ShouldSerializeValue(object component)
		{
			return true;
		}
	}

	[Serializable]
	public class UserProperty
	{
		private string propertyName;

		private object propertyValue;

		public string Name
		{
			get
			{
				return propertyName;
			}
			set
			{
				propertyName = value;
			}
		}

		public object Value
		{
			get
			{
				return propertyValue;
			}
			set
			{
				propertyValue = value;
			}
		}

		public UserProperty()
		{
			propertyName = string.Empty;
			propertyValue = null;
		}

		public UserProperty(string propertyName, object propertyValue)
		{
			this.propertyName = propertyName;
			this.propertyValue = propertyValue;
		}
	}

	private readonly XmlSerializer publicPropertySerializer = new XmlSerializer(typeof(List<UserProperty>));

	private List<UserProperty> publicProperties = new List<UserProperty>();

	private ComponentFileObject fileObject;

	private string relativeFilePath;

	[Category("User Component")]
	[Description("The type of the User Component.")]
	[ReadOnly(true)]
	public string Type
	{
		get
		{
			if (fileObject != null)
			{
				return fileObject.GetNameWithoutExtension();
			}
			return string.Empty;
		}
	}

	[Category("User Component")]
	[Description("The relative path to the component file.")]
	[ReadOnly(true)]
	public string RelativeFilePath
	{
		get
		{
			if (fileObject != null)
			{
				return fileObject.RelativePath;
			}
			return string.Empty;
		}
		set
		{
			relativeFilePath = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public ComponentFileObject FileObject
	{
		get
		{
			return fileObject;
		}
		set
		{
			fileObject = value;
			VerifyPublicProperties();
		}
	}

	[Browsable(false)]
	public string PublicProperties
	{
		get
		{
			return SerializationHelper.Serialize(publicPropertySerializer, publicProperties);
		}
		set
		{
			publicProperties = SerializationHelper.Deserialize(publicPropertySerializer, value) as List<UserProperty>;
			VerifyPublicProperties();
		}
	}

	private void VerifyPublicProperties()
	{
		if (fileObject == null)
		{
			return;
		}
		for (int num = publicProperties.Count - 1; num >= 0; num--)
		{
			UserProperty userProperty = publicProperties[num];
			bool flag = false;
			foreach (Variable variable in fileObject.Variables)
			{
				if (variable.Name == userProperty.Name && variable.Scope == VariableScopes.Public)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				publicProperties.RemoveAt(num);
			}
		}
	}

	protected override void OnBeforeReadProperties()
	{
		if (fileObject != null)
		{
			properties.Clear();
			properties.AddRange(fileObject.Variables);
		}
	}

	public UserComponent()
	{
		InitializeComponent();
	}

	public override List<ComponentFileObject> GetComponentFileObjects()
	{
		List<ComponentFileObject> list = new List<ComponentFileObject>();
		if (fileObject != null)
		{
			list.Add(fileObject);
			list.AddRange(fileObject.GetComponentFileObjects());
		}
		return list;
	}

	public override bool DisableUserComponent(ComponentFileObject componentFileObject)
	{
		if (fileObject != null && fileObject.RelativePath == componentFileObject.RelativePath)
		{
			base.Enabled = false;
			return true;
		}
		return false;
	}

	public override bool IsUsingUserComponent(ComponentFileObject componentFileObject)
	{
		if (fileObject != null)
		{
			return fileObject.RelativePath == componentFileObject.RelativePath;
		}
		return false;
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		foreach (UserProperty publicProperty in publicProperties)
		{
			publicProperty.Value = ExpressionHelper.RenameComponent(this, Convert.ToString(publicProperty.Value), oldValue, newValue);
		}
	}

	public override bool IsCallRelated()
	{
		return false;
	}

	public override void MigrateConstantStringExpressions()
	{
		foreach (Variable property in properties)
		{
			property.InitialValue = ExpressionHelper.MigrateConstantStringExpression(property.InitialValue);
		}
		foreach (UserProperty publicProperty in publicProperties)
		{
			publicProperty.Value = ExpressionHelper.MigrateConstantStringExpression(this, Convert.ToString(publicProperty.Value));
		}
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new UserComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.2q88rbty3oxs");
	}

	public string GetRelativeFilePath()
	{
		return relativeFilePath;
	}

	public List<UserProperty> GetPublicProperties()
	{
		return publicProperties;
	}

	AttributeCollection ICustomTypeDescriptor.GetAttributes()
	{
		return TypeDescriptor.GetAttributes(this, noCustomTypeDesc: true);
	}

	string ICustomTypeDescriptor.GetClassName()
	{
		return TypeDescriptor.GetClassName(this, noCustomTypeDesc: true);
	}

	string ICustomTypeDescriptor.GetComponentName()
	{
		return TypeDescriptor.GetComponentName(this, noCustomTypeDesc: true);
	}

	TypeConverter ICustomTypeDescriptor.GetConverter()
	{
		return TypeDescriptor.GetConverter(this, noCustomTypeDesc: true);
	}

	EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
	{
		return TypeDescriptor.GetDefaultEvent(this, noCustomTypeDesc: true);
	}

	PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
	{
		return null;
	}

	object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
	{
		return TypeDescriptor.GetEditor(this, editorBaseType, noCustomTypeDesc: true);
	}

	EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
	{
		return TypeDescriptor.GetEvents(this, attributes, noCustomTypeDesc: true);
	}

	EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
	{
		return TypeDescriptor.GetEvents(this, noCustomTypeDesc: true);
	}

	PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
	{
		List<PropertyDescriptor> list = new List<PropertyDescriptor>();
		if (fileObject != null)
		{
			foreach (Variable variable in fileObject.Variables)
			{
				if (variable.Accessibility == VariableAccessibilities.ReadWrite && variable.Scope == VariableScopes.Public)
				{
					UserComponentPropertyDescriptor item = new UserComponentPropertyDescriptor(typeof(string), publicProperties, variable.Name, new Attribute[3]
					{
						new CategoryAttribute("User Component"),
						new DescriptionAttribute("User defined property."),
						new EditorAttribute(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))
					});
					list.Add(item);
				}
			}
		}
		foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(this, attributes, noCustomTypeDesc: true))
		{
			list.Add(property);
		}
		return new PropertyDescriptorCollection(list.ToArray());
	}

	PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
	{
		return ((ICustomTypeDescriptor)this).GetProperties(Array.Empty<Attribute>());
	}

	object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
	{
		return this;
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "Component";
	}
}
