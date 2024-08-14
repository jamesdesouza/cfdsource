using System.Collections.Generic;
using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class FileManagementComponentValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		FileManagementComponent fileManagementComponent = obj as FileManagementComponent;
		if (fileManagementComponent.Parent != null)
		{
			List<string> validVariables = ExpressionHelper.GetValidVariables(fileManagementComponent);
			if (string.IsNullOrEmpty(fileManagementComponent.FileName))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.FileManagement.FileNameRequired"), 0, isWarning: false, "FileName"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, fileManagementComponent.FileName).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.FileManagement.InvalidFileName"), 0, isWarning: false, "FileName"));
			}
			if (fileManagementComponent.Action == FileManagementActions.Read && (fileManagementComponent.OpenMode == FileManagementOpenModes.Append || fileManagementComponent.OpenMode == FileManagementOpenModes.Create || fileManagementComponent.OpenMode == FileManagementOpenModes.CreateNew || fileManagementComponent.OpenMode == FileManagementOpenModes.Truncate))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.FileManagement.InvalidOpenModeAction"), 0, isWarning: false, "OpenMode"));
			}
			if (fileManagementComponent.Action == FileManagementActions.Read)
			{
				if (string.IsNullOrEmpty(fileManagementComponent.ReadToEnd))
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.FileManagement.ReadToEndRequired"), 0, isWarning: false, "ReadToEnd"));
				}
				else if (!AbsArgument.BuildArgument(validVariables, fileManagementComponent.ReadToEnd).IsSafeExpression())
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.FileManagement.InvalidReadToEnd"), 0, isWarning: false, "ReadToEnd"));
				}
				if (fileManagementComponent.ReadToEnd != "true")
				{
					if (string.IsNullOrEmpty(fileManagementComponent.LinesToRead))
					{
						validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.FileManagement.LinesToReadRequired"), 0, isWarning: false, "LinesToRead"));
					}
					else if (!AbsArgument.BuildArgument(validVariables, fileManagementComponent.LinesToRead).IsSafeExpression())
					{
						validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.FileManagement.InvalidLinesToRead"), 0, isWarning: false, "LinesToRead"));
					}
				}
				if (string.IsNullOrEmpty(fileManagementComponent.FirstLineToRead))
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.FileManagement.FirstLineToReadRequired"), 0, isWarning: false, "FirstLineToRead"));
				}
				else if (!AbsArgument.BuildArgument(validVariables, fileManagementComponent.FirstLineToRead).IsSafeExpression())
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.FileManagement.InvalidFirstLineToRead"), 0, isWarning: false, "FirstLineToRead"));
				}
			}
			else if (string.IsNullOrEmpty(fileManagementComponent.Content))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.FileManagement.ContentRequired"), 0, isWarning: false, "Content"));
			}
			else if (!AbsArgument.BuildArgument(validVariables, fileManagementComponent.Content).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.FileManagement.InvalidContent"), 0, isWarning: false, "Content"));
			}
		}
		return validationErrorCollection;
	}
}
