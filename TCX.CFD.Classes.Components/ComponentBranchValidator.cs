using System.Workflow.ComponentModel.Compiler;

namespace TCX.CFD.Classes.Components;

public class ComponentBranchValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		return new ValidationErrorCollection();
	}
}
