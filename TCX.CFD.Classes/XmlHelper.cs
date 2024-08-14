using System.Text;
using System.Web;
using System.Xml;
using System.Xml.XPath;

namespace TCX.CFD.Classes;

public class XmlHelper
{
	private static bool isLegalXmlChar(int character)
	{
		if (character != 9 && character != 10 && character != 13 && (character < 32 || character > 55295) && (character < 57344 || character > 65533))
		{
			if (character >= 65536)
			{
				return character <= 1114111;
			}
			return false;
		}
		return true;
	}

	private static string migratePromptCollection(string xml)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xml);
		XPathNavigator xPathNavigator = xmlDocument.CreateNavigator().SelectSingleNode("/ArrayOfPromptCollection/PromptCollection/PromptList");
		if (xPathNavigator != null)
		{
			return HttpUtility.HtmlDecode(xPathNavigator.InnerXml);
		}
		return "<?xml version=\"1.0\" encoding=\"utf-16\"?><ArrayOfPrompt xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"></ArrayOfPrompt>";
	}

	private XmlHelper()
	{
	}

	public static string SanitizeXmlString(string xml)
	{
		if (xml == null)
		{
			return string.Empty;
		}
		StringBuilder stringBuilder = new StringBuilder(xml.Length);
		foreach (char c in xml)
		{
			if (isLegalXmlChar(c))
			{
				stringBuilder.Append(c);
			}
		}
		return stringBuilder.ToString();
	}

	public static void MigratePromptCollections(XPathNodeIterator nodesIterator)
	{
		while (nodesIterator.MoveNext())
		{
			string localName = nodesIterator.Current.LocalName;
			if (localName == "MenuComponent" || localName == "UserInputComponent")
			{
				if (nodesIterator.Current.MoveToAttribute("InvalidDigitPromptList", string.Empty))
				{
					nodesIterator.Current.SetValue(migratePromptCollection(nodesIterator.Current.Value));
					nodesIterator.Current.MoveToParent();
				}
				if (nodesIterator.Current.MoveToAttribute("TimeoutPromptList", string.Empty))
				{
					nodesIterator.Current.SetValue(migratePromptCollection(nodesIterator.Current.Value));
					nodesIterator.Current.MoveToParent();
				}
				if (nodesIterator.Current.MoveToAttribute("SubsequentPromptList", string.Empty))
				{
					nodesIterator.Current.SetValue(migratePromptCollection(nodesIterator.Current.Value));
					nodesIterator.Current.MoveToParent();
				}
			}
		}
	}
}
