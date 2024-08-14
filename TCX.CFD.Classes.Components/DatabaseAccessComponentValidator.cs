using System.Collections.Generic;
using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class DatabaseAccessComponentValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		DatabaseAccessComponent databaseAccessComponent = obj as DatabaseAccessComponent;
		if (databaseAccessComponent.Parent != null)
		{
			List<string> validVariables = ExpressionHelper.GetValidVariables(databaseAccessComponent);
			if (databaseAccessComponent.UseConnectionString)
			{
				if (string.IsNullOrEmpty(databaseAccessComponent.ConnectionString))
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.DatabaseAccess.ConnectionStringRequired"), 0, isWarning: false, "ConnectionString"));
				}
				else if (!AbsArgument.BuildArgument(validVariables, databaseAccessComponent.ConnectionString).IsSafeExpression())
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.DatabaseAccess.InvalidConnectionString"), 0, isWarning: false, "ConnectionString"));
				}
			}
			else
			{
				if (string.IsNullOrEmpty(databaseAccessComponent.Server))
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.DatabaseAccess.ServerRequired"), 0, isWarning: false, "Server"));
				}
				else if (!AbsArgument.BuildArgument(validVariables, databaseAccessComponent.Server).IsSafeExpression())
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.DatabaseAccess.InvalidServer"), 0, isWarning: false, "Server"));
				}
				if (!string.IsNullOrEmpty(databaseAccessComponent.Port) && !AbsArgument.BuildArgument(validVariables, databaseAccessComponent.Port).IsSafeExpression())
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.DatabaseAccess.InvalidPort"), 0, isWarning: false, "Port"));
				}
				if (string.IsNullOrEmpty(databaseAccessComponent.Database))
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.DatabaseAccess.DatabaseRequired"), 0, isWarning: false, "Database"));
				}
				else if (!AbsArgument.BuildArgument(validVariables, databaseAccessComponent.Database).IsSafeExpression())
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.DatabaseAccess.InvalidDatabase"), 0, isWarning: false, "Database"));
				}
				if (string.IsNullOrEmpty(databaseAccessComponent.UserName))
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.DatabaseAccess.UserNameRequired"), 0, isWarning: false, "UserName"));
				}
				else if (!AbsArgument.BuildArgument(validVariables, databaseAccessComponent.UserName).IsSafeExpression())
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.DatabaseAccess.InvalidUserName"), 0, isWarning: false, "UserName"));
				}
				if (string.IsNullOrEmpty(databaseAccessComponent.Password))
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.DatabaseAccess.PasswordRequired"), 0, isWarning: false, "Password"));
				}
				else if (!AbsArgument.BuildArgument(validVariables, databaseAccessComponent.Password).IsSafeExpression())
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.DatabaseAccess.InvalidPassword"), 0, isWarning: false, "Password"));
				}
			}
			if (string.IsNullOrEmpty(databaseAccessComponent.SqlStatement))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.DatabaseAccess.SqlStatementRequired"), 0, isWarning: false, "SqlStatement"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, databaseAccessComponent.SqlStatement).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.DatabaseAccess.InvalidSqlStatement"), 0, isWarning: false, "SqlStatement"));
			}
		}
		return validationErrorCollection;
	}
}
