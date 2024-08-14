using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class FileManagementComponentCompiler : AbsComponentCompiler
{
	private readonly FileManagementComponent fileManagementComponent;

	private string GetFileModeText(FileManagementOpenModes openMode)
	{
		return openMode switch
		{
			FileManagementOpenModes.Append => "System.IO.FileMode.Append", 
			FileManagementOpenModes.Create => "System.IO.FileMode.Create", 
			FileManagementOpenModes.CreateNew => "System.IO.FileMode.CreateNew", 
			FileManagementOpenModes.Open => "System.IO.FileMode.Open", 
			FileManagementOpenModes.OpenOrCreate => "System.IO.FileMode.OpenOrCreate", 
			FileManagementOpenModes.Truncate => "System.IO.FileMode.Truncate", 
			_ => "System.IO.FileMode.Open", 
		};
	}

	public FileManagementComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, FileManagementComponent fileManagementComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(fileManagementComponent), fileManagementComponent.GetRootFlow().FlowType, fileManagementComponent)
	{
		this.fileManagementComponent = fileManagementComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (fileManagementComponent.Action == FileManagementActions.Read && (fileManagementComponent.OpenMode == FileManagementOpenModes.Append || fileManagementComponent.OpenMode == FileManagementOpenModes.Create))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.FileManagementComponent.InvalidOpenModeAction"), fileManagementComponent.Name), fileObject, flowType, fileManagementComponent);
		}
		else if (string.IsNullOrEmpty(fileManagementComponent.FileName))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.FileManagementComponent.FileNameIsEmpty"), fileManagementComponent.Name), fileObject, flowType, fileManagementComponent);
		}
		else if (fileManagementComponent.Action == FileManagementActions.Read && fileManagementComponent.ReadToEnd == "false" && string.IsNullOrEmpty(fileManagementComponent.LinesToRead))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.FileManagementComponent.LinesToReadIsEmpty"), fileManagementComponent.Name), fileObject, flowType, fileManagementComponent);
		}
		else if (fileManagementComponent.Action == FileManagementActions.Read && string.IsNullOrEmpty(fileManagementComponent.FirstLineToRead))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.FileManagementComponent.FirstLineToReadIsEmpty"), fileManagementComponent.Name), fileObject, flowType, fileManagementComponent);
		}
		else if (fileManagementComponent.Action == FileManagementActions.Write && string.IsNullOrEmpty(fileManagementComponent.Content))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.FileManagementComponent.ContentIsEmpty"), fileManagementComponent.Name), fileObject, flowType, fileManagementComponent);
		}
		else
		{
			AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, fileManagementComponent.FileName);
			if (!absArgument.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "FileName", fileManagementComponent.Name, fileManagementComponent.FileName), fileObject, flowType, fileManagementComponent);
			}
			componentsInitializationScriptSb.AppendFormat("FileManagementComponent {0} = new FileManagementComponent(\"{0}\", callflow, myCall, logHeader);", fileManagementComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Action = {1};", fileManagementComponent.Name, (fileManagementComponent.Action == FileManagementActions.Read) ? "FileManagementComponent.Actions.Read" : "FileManagementComponent.Actions.Write").AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.FileMode = {1};", fileManagementComponent.Name, GetFileModeText(fileManagementComponent.OpenMode)).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.FileNameHandler = () => {{ return Convert.ToString({1}); }};", fileManagementComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
			if (fileManagementComponent.Action == FileManagementActions.Read)
			{
				AbsArgument absArgument2 = AbsArgument.BuildArgument(validVariables, fileManagementComponent.FirstLineToRead);
				if (!absArgument2.IsSafeExpression())
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "FirstLineToRead", fileManagementComponent.Name, fileManagementComponent.FirstLineToRead), fileObject, flowType, fileManagementComponent);
				}
				AbsArgument absArgument3 = AbsArgument.BuildArgument(validVariables, fileManagementComponent.ReadToEnd);
				if (!absArgument3.IsSafeExpression())
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "ReadToEnd", fileManagementComponent.Name, fileManagementComponent.ReadToEnd), fileObject, flowType, fileManagementComponent);
				}
				componentsInitializationScriptSb.AppendFormat("{0}.FirstLineToReadHandler = () => {{ return Convert.ToInt32({1}); }};", fileManagementComponent.Name, absArgument2.GetCompilerString()).AppendLine().Append("            ");
				componentsInitializationScriptSb.AppendFormat("{0}.ReadToEndHandler = () => {{ return Convert.ToBoolean({1}); }};", fileManagementComponent.Name, absArgument3.GetCompilerString()).AppendLine().Append("            ");
				if (fileManagementComponent.ReadToEnd.Trim() != "true")
				{
					AbsArgument absArgument4 = AbsArgument.BuildArgument(validVariables, fileManagementComponent.LinesToRead);
					if (!absArgument4.IsSafeExpression())
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "LinesToRead", fileManagementComponent.Name, fileManagementComponent.LinesToRead), fileObject, flowType, fileManagementComponent);
					}
					componentsInitializationScriptSb.AppendFormat("{0}.LinesToReadHandler = () => {{ return Convert.ToInt32({1}); }};", fileManagementComponent.Name, absArgument4.GetCompilerString()).AppendLine().Append("            ");
				}
			}
			else
			{
				AbsArgument absArgument5 = AbsArgument.BuildArgument(validVariables, fileManagementComponent.Content);
				if (!absArgument5.IsSafeExpression())
				{
					CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Content", fileManagementComponent.Name, fileManagementComponent.Content), fileObject, flowType, fileManagementComponent);
				}
				componentsInitializationScriptSb.AppendFormat("{0}.AppendFinalCrLf = {1};", fileManagementComponent.Name, fileManagementComponent.AppendFinalCrLf ? "true" : "false").AppendLine().Append("            ");
				componentsInitializationScriptSb.AppendFormat("{0}.ContentHandler = () => {{ return Convert.ToString({1}); }};", fileManagementComponent.Name, absArgument5.GetCompilerString()).AppendLine().Append("            ");
			}
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, fileManagementComponent.Name).AppendLine().Append("            ");
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}
