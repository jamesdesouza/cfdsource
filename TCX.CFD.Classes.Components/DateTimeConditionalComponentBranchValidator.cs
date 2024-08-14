using System.Workflow.ComponentModel.Compiler;

namespace TCX.CFD.Classes.Components;

public class DateTimeConditionalComponentBranchValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		DateTimeConditionalComponentBranch dateTimeConditionalComponentBranch = obj as DateTimeConditionalComponentBranch;
		if (dateTimeConditionalComponentBranch.Parent != null)
		{
			if (dateTimeConditionalComponentBranch.DIDFilter != 0 && string.IsNullOrEmpty(dateTimeConditionalComponentBranch.DIDFilterList))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.DateTimeConditionalBranch.DIDFilterListRequired"), 0, isWarning: false, "DIDFilterList"));
			}
			if (dateTimeConditionalComponentBranch.DateTimeConditions.Count == 0)
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.DateTimeConditionalBranch.NoDateTimeConditions"), 0, isWarning: false, "DateTimeConditions"));
			}
			foreach (DateTimeCondition dateTimeCondition in dateTimeConditionalComponentBranch.DateTimeConditions)
			{
				if (!dateTimeCondition.IsValid())
				{
					validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.DateTimeConditionalBranch.InvalidCondition"), 0, isWarning: false, "DateTimeConditions"));
				}
			}
		}
		return validationErrorCollection;
	}
}
