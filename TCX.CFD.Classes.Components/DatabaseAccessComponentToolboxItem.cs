using System.ComponentModel;
using System.ComponentModel.Design;
using System.Workflow.ComponentModel.Design;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FlowDesigner;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.Components;

public class DatabaseAccessComponentToolboxItem : ActivityToolboxItem
{
	protected override IComponent[] CreateComponentsCore(IDesignerHost host)
	{
		DatabaseAccessComponent databaseAccessComponent = new DatabaseAccessComponent();
		databaseAccessComponent.UseConnectionString = Settings.Default.DatabaseAccessTemplateUseConnectionString;
		databaseAccessComponent.ConnectionString = (string.IsNullOrEmpty(Settings.Default.DatabaseAccessTemplateConnectionString) ? string.Empty : ("\"" + ExpressionHelper.EscapeConstantString(Settings.Default.DatabaseAccessTemplateConnectionString) + "\""));
		databaseAccessComponent.Server = (string.IsNullOrEmpty(Settings.Default.DatabaseAccessTemplateServer) ? string.Empty : ("\"" + ExpressionHelper.EscapeConstantString(Settings.Default.DatabaseAccessTemplateServer) + "\""));
		databaseAccessComponent.Port = ((Settings.Default.DatabaseAccessTemplatePort == -1) ? string.Empty : Settings.Default.DatabaseAccessTemplatePort.ToString());
		databaseAccessComponent.Database = (string.IsNullOrEmpty(Settings.Default.DatabaseAccessTemplateDatabase) ? string.Empty : ("\"" + ExpressionHelper.EscapeConstantString(Settings.Default.DatabaseAccessTemplateDatabase) + "\""));
		databaseAccessComponent.UserName = (string.IsNullOrEmpty(Settings.Default.DatabaseAccessTemplateUserName) ? string.Empty : ("\"" + ExpressionHelper.EscapeConstantString(Settings.Default.DatabaseAccessTemplateUserName) + "\""));
		databaseAccessComponent.Password = (string.IsNullOrEmpty(Settings.Default.DatabaseAccessTemplatePassword) ? string.Empty : ("\"" + ExpressionHelper.EscapeConstantString(Settings.Default.DatabaseAccessTemplatePassword) + "\""));
		databaseAccessComponent.DatabaseType = ((!(Settings.Default.DatabaseAccessTemplateDatabaseType == "SqlServer")) ? ((Settings.Default.DatabaseAccessTemplateDatabaseType == "PostgreSQL") ? DatabaseTypes.PostgreSQL : DatabaseTypes.MySQL) : DatabaseTypes.SqlServer);
		databaseAccessComponent.StatementType = ((!(Settings.Default.DatabaseAccessTemplateStatementType == "Query")) ? ((Settings.Default.DatabaseAccessTemplateStatementType == "NonQuery") ? SqlStatementTypes.NonQuery : SqlStatementTypes.Scalar) : SqlStatementTypes.Query);
		databaseAccessComponent.Timeout = Settings.Default.DatabaseAccessTemplateTimeout;
		FlowDesignerNameCreator.CreateName("DatabaseAccess", host.Container, databaseAccessComponent);
		return new IComponent[1] { databaseAccessComponent };
	}
}
