using System;
using System.ComponentModel;
using System.Globalization;

namespace TCX.CFD.Classes.Editors;

public class OnlineServicesTypeConverter : ReferenceConverter
{
	public OnlineServicesTypeConverter(Type type)
		: base(type)
	{
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		return LocalizedResourceMgr.GetString("OnlineServicesTypeConverter.Configure.Text");
	}

	public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
	{
		return false;
	}
}
