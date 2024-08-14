using System.Collections.Generic;
using TCX.CFD.Classes.Compiler;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Components;

public interface IVadActivity
{
	bool DebugModeActive { get; set; }

	List<Variable> Properties { get; }

	List<ComponentFileObject> GetComponentFileObjects();

	void NotifyComponentRenamed(string oldValue, string newValue);

	bool DisableUserComponent(ComponentFileObject componentFileObject);

	bool IsUsingUserComponent(ComponentFileObject componentFileObject);

	RootFlow GetRootFlow();

	bool IsCallRelated();

	void MigrateConstantStringExpressions();

	AbsComponentCompiler GetCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter);

	void ShowHelp();
}
