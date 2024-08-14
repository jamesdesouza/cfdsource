using System.Collections.Generic;
using System.ComponentModel;

namespace TCX.CFD.Classes.Editors;

public class ContentTypesTypeConverter : StringConverter
{
	public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
	{
		return true;
	}

	public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
	{
		return false;
	}

	public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
	{
		return new StandardValuesCollection(new List<string>
		{
			"application/javascript", "application/json", "application/x-www-form-urlencoded", "application/pdf", "application/xml", "application/zip", "multipart/form-data", "text/css", "text/html", "text/plain",
			"image/png", "image/jpeg", "image/gif"
		});
	}
}
