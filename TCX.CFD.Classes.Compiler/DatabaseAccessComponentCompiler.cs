using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class DatabaseAccessComponentCompiler : AbsComponentCompiler
{
	private readonly DatabaseAccessComponent databaseAccessComponent;

	public DatabaseAccessComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, DatabaseAccessComponent databaseAccessComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(databaseAccessComponent), databaseAccessComponent.GetRootFlow().FlowType, databaseAccessComponent)
	{
		this.databaseAccessComponent = databaseAccessComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (databaseAccessComponent.UseConnectionString && string.IsNullOrEmpty(databaseAccessComponent.ConnectionString))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.DatabaseAccessComponent.ConnectionStringIsEmpty"), databaseAccessComponent.Name), fileObject, flowType, databaseAccessComponent);
		}
		else if (!databaseAccessComponent.UseConnectionString && string.IsNullOrEmpty(databaseAccessComponent.Server))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.DatabaseAccessComponent.ServerIsEmpty"), databaseAccessComponent.Name), fileObject, flowType, databaseAccessComponent);
		}
		else if (!databaseAccessComponent.UseConnectionString && string.IsNullOrEmpty(databaseAccessComponent.Database))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.DatabaseAccessComponent.DatabaseIsEmpty"), databaseAccessComponent.Name), fileObject, flowType, databaseAccessComponent);
		}
		else if (!databaseAccessComponent.UseConnectionString && string.IsNullOrEmpty(databaseAccessComponent.UserName))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.DatabaseAccessComponent.UserNameIsEmpty"), databaseAccessComponent.Name), fileObject, flowType, databaseAccessComponent);
		}
		else if (!databaseAccessComponent.UseConnectionString && string.IsNullOrEmpty(databaseAccessComponent.Password))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.DatabaseAccessComponent.PasswordIsEmpty"), databaseAccessComponent.Name), fileObject, flowType, databaseAccessComponent);
		}
		else if (string.IsNullOrEmpty(databaseAccessComponent.SqlStatement))
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.DatabaseAccessComponent.SqlStatementIsEmpty"), databaseAccessComponent.Name), fileObject, flowType, databaseAccessComponent);
		}
		else
		{
			if (databaseAccessComponent.DatabaseType == DatabaseTypes.SqlServer)
			{
				componentsInitializationScriptSb.AppendFormat("SqlServerDatabaseAccessComponent {0} = new SqlServerDatabaseAccessComponent(\"{0}\", callflow, myCall, logHeader);", databaseAccessComponent.Name).AppendLine().Append("            ");
			}
			else if (databaseAccessComponent.DatabaseType == DatabaseTypes.PostgreSQL)
			{
				componentsInitializationScriptSb.AppendFormat("PostgresqlDatabaseAccessComponent {0} = new PostgresqlDatabaseAccessComponent(\"{0}\", callflow, myCall, logHeader);", databaseAccessComponent.Name).AppendLine().Append("            ");
			}
			else if (databaseAccessComponent.DatabaseType == DatabaseTypes.MySQL)
			{
				componentsInitializationScriptSb.AppendFormat("MySqlDatabaseAccessComponent {0} = new MySqlDatabaseAccessComponent(\"{0}\", callflow, myCall, logHeader);", databaseAccessComponent.Name).AppendLine().Append("            ");
			}
			else
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.DatabaseAccessComponent.InvalidDatabaseType"), databaseAccessComponent.Name), fileObject, flowType, databaseAccessComponent);
			}
			if (databaseAccessComponent.UseConnectionString)
			{
				if (!string.IsNullOrWhiteSpace(databaseAccessComponent.ConnectionString))
				{
					AbsArgument absArgument = AbsArgument.BuildArgument(validVariables, databaseAccessComponent.ConnectionString);
					if (!absArgument.IsSafeExpression())
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "ConnectionString", databaseAccessComponent.Name, databaseAccessComponent.ConnectionString), fileObject, flowType, databaseAccessComponent);
					}
					componentsInitializationScriptSb.AppendFormat("{0}.ConnectionStringHandler = () => {{ return Convert.ToString({1}); }};", databaseAccessComponent.Name, absArgument.GetCompilerString()).AppendLine().Append("            ");
				}
			}
			else
			{
				if (!string.IsNullOrWhiteSpace(databaseAccessComponent.Server))
				{
					AbsArgument absArgument2 = AbsArgument.BuildArgument(validVariables, databaseAccessComponent.Server);
					if (!absArgument2.IsSafeExpression())
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Server", databaseAccessComponent.Name, databaseAccessComponent.Server), fileObject, flowType, databaseAccessComponent);
					}
					componentsInitializationScriptSb.AppendFormat("{0}.ServerHandler = () => {{ return Convert.ToString({1}); }};", databaseAccessComponent.Name, absArgument2.GetCompilerString()).AppendLine().Append("            ");
				}
				if (!string.IsNullOrWhiteSpace(databaseAccessComponent.Port))
				{
					AbsArgument absArgument3 = AbsArgument.BuildArgument(validVariables, databaseAccessComponent.Port);
					if (!absArgument3.IsSafeExpression())
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Port", databaseAccessComponent.Name, databaseAccessComponent.Port), fileObject, flowType, databaseAccessComponent);
					}
					componentsInitializationScriptSb.AppendFormat("{0}.PortHandler = () => {{ return Convert.ToInt32({1}); }};", databaseAccessComponent.Name, absArgument3.GetCompilerString()).AppendLine().Append("            ");
				}
				if (!string.IsNullOrWhiteSpace(databaseAccessComponent.Database))
				{
					AbsArgument absArgument4 = AbsArgument.BuildArgument(validVariables, databaseAccessComponent.Database);
					if (!absArgument4.IsSafeExpression())
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Database", databaseAccessComponent.Name, databaseAccessComponent.Database), fileObject, flowType, databaseAccessComponent);
					}
					componentsInitializationScriptSb.AppendFormat("{0}.DatabaseHandler = () => {{ return Convert.ToString({1}); }};", databaseAccessComponent.Name, absArgument4.GetCompilerString()).AppendLine().Append("            ");
				}
				if (!string.IsNullOrWhiteSpace(databaseAccessComponent.UserName))
				{
					AbsArgument absArgument5 = AbsArgument.BuildArgument(validVariables, databaseAccessComponent.UserName);
					if (!absArgument5.IsSafeExpression())
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "UserName", databaseAccessComponent.Name, databaseAccessComponent.UserName), fileObject, flowType, databaseAccessComponent);
					}
					componentsInitializationScriptSb.AppendFormat("{0}.UserNameHandler = () => {{ return Convert.ToString({1}); }};", databaseAccessComponent.Name, absArgument5.GetCompilerString()).AppendLine().Append("            ");
				}
				if (!string.IsNullOrWhiteSpace(databaseAccessComponent.Password))
				{
					AbsArgument absArgument6 = AbsArgument.BuildArgument(validVariables, databaseAccessComponent.Password);
					if (!absArgument6.IsSafeExpression())
					{
						CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "Password", databaseAccessComponent.Name, databaseAccessComponent.Password), fileObject, flowType, databaseAccessComponent);
					}
					componentsInitializationScriptSb.AppendFormat("{0}.PasswordHandler = () => {{ return Convert.ToString({1}); }};", databaseAccessComponent.Name, absArgument6.GetCompilerString()).AppendLine().Append("            ");
				}
			}
			AbsArgument absArgument7 = AbsArgument.BuildArgument(validVariables, databaseAccessComponent.SqlStatement);
			if (!absArgument7.IsSafeExpression())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidExpression"), "SqlStatement", databaseAccessComponent.Name, databaseAccessComponent.SqlStatement), fileObject, flowType, databaseAccessComponent);
			}
			componentsInitializationScriptSb.AppendFormat("{0}.SqlStatementHandler = () => {{ return Convert.ToString({1}); }};", databaseAccessComponent.Name, absArgument7.GetCompilerString()).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.UseConnectionString = {1};", databaseAccessComponent.Name, databaseAccessComponent.UseConnectionString ? "true" : "false").AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.StatementType = {1};", databaseAccessComponent.Name, (databaseAccessComponent.StatementType == SqlStatementTypes.Query) ? "DatabaseAccessComponent.StatementTypes.Query" : ((databaseAccessComponent.StatementType == SqlStatementTypes.NonQuery) ? "DatabaseAccessComponent.StatementTypes.NonQuery" : "DatabaseAccessComponent.StatementTypes.Scalar")).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Timeout = {1};", databaseAccessComponent.Name, 1000 * databaseAccessComponent.Timeout).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, databaseAccessComponent.Name).AppendLine().Append("            ");
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}
