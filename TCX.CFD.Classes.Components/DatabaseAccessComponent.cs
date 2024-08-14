using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Xml.Serialization;
using TCX.CFD.Classes.Compiler;
using TCX.CFD.Classes.Editors;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Components;

[DesignerSerializer(typeof(ActivityMarkupSerializer), typeof(WorkflowMarkupSerializer))]
[Designer(typeof(DatabaseAccessComponentDesigner), typeof(IDesigner))]
[ToolboxItem(typeof(DatabaseAccessComponentToolboxItem))]
[ToolboxBitmap(typeof(DatabaseAccessComponent), "Resources.DatabaseAccess.png")]
[ActivityValidator(typeof(DatabaseAccessComponentValidator))]
public class DatabaseAccessComponent : AbsVadActivity
{
	private uint timeout;

	private readonly XmlSerializer parameterSerializer = new XmlSerializer(typeof(List<Parameter>));

	[Category("Database Access")]
	[Description("True to set a connection string, False to set each property separately.")]
	public bool UseConnectionString { get; set; }

	[Category("Database Access")]
	[Description("The database connection string. Only valid when UseConnectionString is set to True.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string ConnectionString { get; set; } = string.Empty;


	[Category("Database Access")]
	[Description("The database server name or IP address. Only valid when UseConnectionString is set to False.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string Server { get; set; } = string.Empty;


	[Category("Database Access")]
	[Description("The database server port. Only valid when UseConnectionString is set to False.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string Port { get; set; } = string.Empty;


	[Category("Database Access")]
	[Description("The database to use when connecting to the Server. Only valid when UseConnectionString is set to False.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string Database { get; set; } = string.Empty;


	[Category("Database Access")]
	[Description("The username to use when connecting to the database. Only valid when UseConnectionString is set to False.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string UserName { get; set; } = string.Empty;


	[Category("Database Access")]
	[Description("The password to use when connecting to the database. Only valid when UseConnectionString is set to False.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string Password { get; set; } = string.Empty;


	[Category("Database Access")]
	[Description("The SQL statement to execute.")]
	[Editor(typeof(RightHandSideExpressionEditor), typeof(UITypeEditor))]
	public string SqlStatement { get; set; } = string.Empty;


	[Category("Database Access")]
	[Description("The type of the database.")]
	public DatabaseTypes DatabaseType { get; set; }

	[Category("Database Access")]
	[Description("The type of the statement. Use Query to execute an SQL statement that returns rows. Use NonQuery to execute an SQL statement that doesn't return rows (for example, INSERT, DELETE, UPDATE, etc.). Use Scalar to execute an SQL statement that returns a single value (for example, SUM, COUNT, etc.).")]
	public SqlStatementTypes StatementType { get; set; }

	[Category("Database Access")]
	[Description("The wait time in seconds before terminating the attempt to execute an SQL statement and generating an error. A value of 0 indicates no limit, and should be avoided because an attempt to execute an SQL statement could wait indefinitely.")]
	public uint Timeout
	{
		get
		{
			return timeout;
		}
		set
		{
			if (value > 999)
			{
				throw new ArgumentException(string.Format(LocalizedResourceMgr.GetString("ComponentValidators.General.NumericPropertyOutOfRange"), 0, 999));
			}
			timeout = value;
		}
	}

	[Browsable(false)]
	public string ParameterList
	{
		get
		{
			return string.Empty;
		}
		set
		{
			if (!string.IsNullOrEmpty(value))
			{
				Parameters = SerializationHelper.Deserialize(parameterSerializer, value) as List<Parameter>;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<Parameter> Parameters { get; private set; } = new List<Parameter>();


	protected override void OnBeforeReadProperties()
	{
		properties.Clear();
		AddProperties();
	}

	private void AddProperties()
	{
		if (StatementType == SqlStatementTypes.Scalar)
		{
			properties.Add(new Variable("ScalarResult", VariableScopes.Public, VariableAccessibilities.ReadOnly)
			{
				HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.DatabaseAccess.ScalarResultHelpText")
			});
		}
		else if (StatementType == SqlStatementTypes.Query)
		{
			properties.Add(new Variable("QueryResult", VariableScopes.Public, VariableAccessibilities.ReadOnly)
			{
				HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.DatabaseAccess.QueryResultHelpText")
			});
		}
		else
		{
			properties.Add(new Variable("NonQueryResult", VariableScopes.Public, VariableAccessibilities.ReadOnly)
			{
				HelpText = LocalizedResourceMgr.GetString("ComponentDesigners.DatabaseAccess.NonQueryResultHelpText")
			});
		}
	}

	public DatabaseAccessComponent()
	{
		InitializeComponent();
		AddProperties();
	}

	public override void NotifyComponentRenamed(string oldValue, string newValue)
	{
		ConnectionString = ExpressionHelper.RenameComponent(this, ConnectionString, oldValue, newValue);
		Server = ExpressionHelper.RenameComponent(this, Server, oldValue, newValue);
		Port = ExpressionHelper.RenameComponent(this, Port, oldValue, newValue);
		Database = ExpressionHelper.RenameComponent(this, Database, oldValue, newValue);
		UserName = ExpressionHelper.RenameComponent(this, UserName, oldValue, newValue);
		Password = ExpressionHelper.RenameComponent(this, Password, oldValue, newValue);
		foreach (Parameter parameter in Parameters)
		{
			parameter.Value = ExpressionHelper.RenameComponent(this, parameter.Value, oldValue, newValue);
		}
	}

	public override bool IsCallRelated()
	{
		return false;
	}

	public override void MigrateConstantStringExpressions()
	{
		ConnectionString = ExpressionHelper.MigrateConstantStringExpression(this, ConnectionString);
		Server = ExpressionHelper.MigrateConstantStringExpression(this, Server);
		Port = ExpressionHelper.MigrateConstantStringExpression(this, Port);
		Database = ExpressionHelper.MigrateConstantStringExpression(this, Database);
		UserName = ExpressionHelper.MigrateConstantStringExpression(this, UserName);
		Password = ExpressionHelper.MigrateConstantStringExpression(this, Password);
		foreach (Parameter parameter in Parameters)
		{
			parameter.Value = ExpressionHelper.MigrateConstantStringExpression(this, parameter.Value);
		}
	}

	public override AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter)
	{
		return new DatabaseAccessComponentCompiler(compilerResultCollector, fileObject, progress, errorCounter, this);
	}

	public override void ShowHelp()
	{
		Process.Start("https://www.3cx.com/docs/manual/cfd-components/#h.44lwg0nrv5st");
	}

	[DebuggerNonUserCode]
	private void InitializeComponent()
	{
		base.Name = "DatabaseAccessComponent";
	}
}
