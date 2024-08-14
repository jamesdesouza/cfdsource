using System.ComponentModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.FlowDesigner;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.Components;

public class CryptographyComponentToolboxItem : ActivityToolboxItem
{
	private CryptographyAlgorithms GetCryptographyAlgorithm(string str)
	{
		if (str == "3DES")
		{
			return CryptographyAlgorithms.TripleDES;
		}
		return CryptographyAlgorithms.HashMD5;
	}

	protected override IComponent[] CreateComponentsCore(IDesignerHost host)
	{
		CryptographyComponent cryptographyComponent = new CryptographyComponent
		{
			Algorithm = GetCryptographyAlgorithm(Settings.Default.CryptographyTemplateAlgorithm),
			Format = ((!(Settings.Default.CryptographyTemplateFormat == "Hexadecimal")) ? CodificationFormats.Base64 : CodificationFormats.Hexadecimal),
			Key = Settings.Default.CryptographyTemplateKey
		};
		FlowDesignerNameCreator.CreateName("Encryption", host.Container, cryptographyComponent);
		return new IComponent[1] { cryptographyComponent };
	}
}
