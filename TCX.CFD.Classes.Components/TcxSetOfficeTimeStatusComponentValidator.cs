using System.Workflow.ComponentModel.Compiler;

namespace TCX.CFD.Classes.Components;

public class TcxSetOfficeTimeStatusComponentValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		return new ValidationErrorCollection();
	}
}
