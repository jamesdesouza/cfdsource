using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TCX.CFD.Classes.FileSystem;

[Serializable]
[DefaultProperty("Name")]
public class Variable : ICustomTypeDescriptor
{
	public delegate void NameChangingEventHandler(object sender, NameChangingEventArgs e);

	public delegate void NameChangedEventHandler(object sender, NameChangedEventArgs e);

	private string name;

	private VariableScopes scope;

	private VariableAccessibilities accessibility;

	private string initialValue;

	private bool showScopeProperty;

	private bool debuggerVisible = true;

	private string helpText;

	[ParenthesizePropertyName(true)]
	[DefaultValue("")]
	[Category("Variable")]
	public string Name
	{
		get
		{
			return name;
		}
		set
		{
			if (value != name)
			{
				NameChangingEventArgs nameChangingEventArgs = new NameChangingEventArgs(name, value);
				this.NameChanging?.Invoke(this, nameChangingEventArgs);
				if (!nameChangingEventArgs.Cancel)
				{
					string oldValue = name;
					name = value;
					this.NameChanged?.Invoke(this, new NameChangedEventArgs(oldValue, name));
				}
			}
		}
	}

	[DefaultValue("")]
	[Category("Variable")]
	public string InitialValue
	{
		get
		{
			return initialValue;
		}
		set
		{
			initialValue = value;
		}
	}

	[DefaultValue(VariableScopes.Public)]
	[Category("Variable")]
	public VariableScopes Scope
	{
		get
		{
			return scope;
		}
		set
		{
			scope = value;
		}
	}

	[DefaultValue(VariableAccessibilities.ReadWrite)]
	[Category("Variable")]
	public VariableAccessibilities Accessibility
	{
		get
		{
			return accessibility;
		}
		set
		{
			accessibility = value;
		}
	}

	[Browsable(false)]
	public bool ShowScopeProperty
	{
		get
		{
			return showScopeProperty;
		}
		set
		{
			showScopeProperty = value;
		}
	}

	[Browsable(false)]
	public bool DebuggerVisible
	{
		get
		{
			return debuggerVisible;
		}
		set
		{
			debuggerVisible = value;
		}
	}

	[Category("Variable")]
	public string HelpText
	{
		get
		{
			return helpText;
		}
		set
		{
			helpText = value;
		}
	}

	public event NameChangingEventHandler NameChanging;

	public event NameChangedEventHandler NameChanged;

	public Variable(string name, VariableScopes scope, VariableAccessibilities accessibility, string initialValue, bool showScopeProperty, string helpText)
	{
		this.name = name;
		this.initialValue = initialValue;
		this.scope = scope;
		this.accessibility = accessibility;
		this.showScopeProperty = showScopeProperty;
		this.helpText = helpText;
	}

	public Variable(string name, VariableScopes scope, VariableAccessibilities accessibility, string initialValue)
	{
		this.name = name;
		this.scope = scope;
		this.accessibility = accessibility;
		this.initialValue = initialValue;
		showScopeProperty = true;
		helpText = "";
	}

	public Variable(string name, VariableScopes scope, VariableAccessibilities accessibility)
	{
		this.name = name;
		this.scope = scope;
		this.accessibility = accessibility;
		initialValue = string.Empty;
		showScopeProperty = true;
		helpText = "";
	}

	public Variable(bool showScopeProperty)
	{
		name = string.Empty;
		scope = VariableScopes.Public;
		accessibility = VariableAccessibilities.ReadWrite;
		initialValue = string.Empty;
		this.showScopeProperty = showScopeProperty;
		helpText = "";
	}

	public Variable()
	{
		name = string.Empty;
		scope = VariableScopes.Public;
		accessibility = VariableAccessibilities.ReadWrite;
		initialValue = string.Empty;
		showScopeProperty = true;
		helpText = "";
	}

	public bool IsNameChangingHandled()
	{
		return this.NameChanging != null;
	}

	public bool IsNameChangedHandled()
	{
		return this.NameChanged != null;
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
		PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this, attributes, noCustomTypeDesc: true);
		if (showScopeProperty)
		{
			return properties;
		}
		List<PropertyDescriptor> list = new List<PropertyDescriptor>();
		foreach (PropertyDescriptor item in properties)
		{
			if (item.Name != "Scope")
			{
				list.Add(item);
			}
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
}
