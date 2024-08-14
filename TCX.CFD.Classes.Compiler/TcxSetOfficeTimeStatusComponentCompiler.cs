using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class TcxSetOfficeTimeStatusComponentCompiler : AbsComponentCompiler
{
	private readonly TcxSetOfficeTimeStatusComponent tcxSetOfficeTimeStatusComponent;

	public TcxSetOfficeTimeStatusComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, TcxSetOfficeTimeStatusComponent tcxSetOfficeTimeStatusComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(tcxSetOfficeTimeStatusComponent), tcxSetOfficeTimeStatusComponent.GetRootFlow().FlowType, tcxSetOfficeTimeStatusComponent)
	{
		this.tcxSetOfficeTimeStatusComponent = tcxSetOfficeTimeStatusComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		componentsInitializationScriptSb.AppendFormat("TcxSetOfficeTimeStatusComponent {0} = new TcxSetOfficeTimeStatusComponent(\"{0}\", callflow, myCall, logHeader);", tcxSetOfficeTimeStatusComponent.Name).AppendLine().Append("            ");
		componentsInitializationScriptSb.AppendFormat("{0}.Status = {1}; }};", tcxSetOfficeTimeStatusComponent.Name, EnumHelper.OfficeTimeStatusToCompilerString(tcxSetOfficeTimeStatusComponent.Status)).AppendLine().Append("            ");
		componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, tcxSetOfficeTimeStatusComponent.Name).AppendLine().Append("            ");
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}
