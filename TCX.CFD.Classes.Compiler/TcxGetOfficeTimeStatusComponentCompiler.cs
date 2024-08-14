using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class TcxGetOfficeTimeStatusComponentCompiler : AbsComponentCompiler
{
	private readonly TcxGetOfficeTimeStatusComponent tcxGetOfficeTimeStatusComponent;

	public TcxGetOfficeTimeStatusComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, TcxGetOfficeTimeStatusComponent tcxGetOfficeTimeStatusComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(tcxGetOfficeTimeStatusComponent), tcxGetOfficeTimeStatusComponent.GetRootFlow().FlowType, tcxGetOfficeTimeStatusComponent)
	{
		this.tcxGetOfficeTimeStatusComponent = tcxGetOfficeTimeStatusComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		componentsInitializationScriptSb.AppendFormat("TcxGetOfficeTimeStatusComponent {0} = new TcxGetOfficeTimeStatusComponent(\"{0}\", callflow, myCall, logHeader);", tcxGetOfficeTimeStatusComponent.Name).AppendLine().Append("            ");
		componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, tcxGetOfficeTimeStatusComponent.Name).AppendLine().Append("            ");
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}
