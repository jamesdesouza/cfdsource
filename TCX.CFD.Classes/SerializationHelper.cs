using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace TCX.CFD.Classes;

public class SerializationHelper
{
	public static string Serialize(XmlSerializer serializer, object o)
	{
		StringBuilder stringBuilder = new StringBuilder();
		using XmlWriter xmlWriter = XmlWriter.Create(stringBuilder);
		serializer.Serialize(xmlWriter, o);
		return stringBuilder.ToString();
	}

	public static object Deserialize(XmlSerializer serializer, string s)
	{
		using TextReader textReader = new StringReader(s);
		return serializer.Deserialize(textReader);
	}
}
