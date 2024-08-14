using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class CryptographyComponentCompiler : AbsComponentCompiler
{
	private readonly CryptographyComponent cryptographyComponent;

	public CryptographyComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, CryptographyComponent cryptographyComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(cryptographyComponent), cryptographyComponent.GetRootFlow().FlowType, cryptographyComponent)
	{
		this.cryptographyComponent = cryptographyComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (string.IsNullOrEmpty(cryptographyComponent.Text))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CryptographyComponent.TextIsEmpty"), cryptographyComponent.Name), fileObject, flowType, cryptographyComponent);
		}
		else if (cryptographyComponent.Algorithm == CryptographyAlgorithms.TripleDES && cryptographyComponent.Key.Length != 24)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.CryptographyComponent.InvalidKeyLength"), cryptographyComponent.Name), fileObject, flowType, cryptographyComponent);
		}
		else
		{
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, cryptographyComponent.Text);
			if (!absArgument.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Text", cryptographyComponent.Name, cryptographyComponent.Text), fileObject, flowType, cryptographyComponent);
			}
			componentsInitializationScriptSb.AppendFormat("CryptographyComponent {0} = new CryptographyComponent(\"{0}\", callflow, myCall, logHeader);", cryptographyComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Action = {1};", cryptographyComponent.Name, (cryptographyComponent.Action == CryptographyActions.Encrypt) ? "CryptographyComponent.CryptographyActions.Encrypt" : "CryptographyComponent.CryptographyActions.Decrypt").AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Algorithm = {1};", cryptographyComponent.Name, (cryptographyComponent.Algorithm == CryptographyAlgorithms.TripleDES) ? "CryptographyComponent.CryptographyAlgorithms.TripleDES" : "CryptographyComponent.CryptographyAlgorithms.HashMD5").AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Format = {1};", cryptographyComponent.Name, (cryptographyComponent.Format == CodificationFormats.Hexadecimal) ? "CryptographyComponent.CodificationFormats.Hexadecimal" : "CryptographyComponent.CodificationFormats.Base64").AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Key = \"{1}\";", cryptographyComponent.Name, cryptographyComponent.Key).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.TextHandler = () => {{ return Convert.ToString({1}); }};", cryptographyComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, cryptographyComponent.Name).AppendLine().Append("            ");
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}
