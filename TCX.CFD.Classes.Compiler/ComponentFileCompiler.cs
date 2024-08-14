using System.Text;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class ComponentFileCompiler : FileCompiler
{
	private readonly StringBuilder variablesHandlersInitializationScriptSb;

	private readonly StringBuilder variablesHandlersInvocationInitializationScriptSb;

	private readonly StringBuilder variablesHandlersSettersInitializationScriptSb;

	public string VariablesHandlersInitializationScript => variablesHandlersInitializationScriptSb.ToString();

	public string VariablesHandlersInvocationInitializationScript => variablesHandlersInvocationInitializationScriptSb.ToString();

	public string VariablesHandlersSettersInitializationScript => variablesHandlersSettersInitializationScriptSb.ToString();

	public ComponentFileCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
		: base(compilerResultCollector, fileObject, progress, errorCounter)
	{
		variablesHandlersInitializationScriptSb = new StringBuilder();
		variablesHandlersInvocationInitializationScriptSb = new StringBuilder();
		variablesHandlersSettersInitializationScriptSb = new StringBuilder();
		foreach (Variable variable in fileObject.Variables)
		{
			AddVariable("componentVariableMap", "callflow$", variable, systemVariable: false, extraTab: true);
			variablesHandlersInitializationScriptSb.AppendFormat("private ObjectExpressionHandler _{0}Handler = null;", variable.Name).AppendLine().Append("            ");
			variablesHandlersInvocationInitializationScriptSb.AppendFormat("if (_{0}Handler != null) componentVariableMap[\"callflow$.{0}\"].Set(_{0}Handler());", variable.Name).AppendLine().Append("                ");
			variablesHandlersSettersInitializationScriptSb.AppendFormat("public ObjectExpressionHandler {0}Setter {{ set {{ _{0}Handler = value; }} }}", variable.Name).AppendLine().Append("            ");
			variablesHandlersSettersInitializationScriptSb.AppendFormat("public object {0} {{ get {{ return componentVariableMap[\"callflow$.{0}\"].Value; }} }}", variable.Name).AppendLine().Append("            ");
		}
	}
}
