using System.Workflow.ComponentModel.Compiler;
using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Classes.Components;

public class CryptographyComponentValidator : ActivityValidator
{
	public override ValidationErrorCollection Validate(ValidationManager manager, object obj)
	{
		ValidationErrorCollection validationErrorCollection = new ValidationErrorCollection();
		CryptographyComponent cryptographyComponent = obj as CryptographyComponent;
		if (cryptographyComponent.Parent != null)
		{
			if (cryptographyComponent.Algorithm == CryptographyAlgorithms.TripleDES && cryptographyComponent.Key.Length != 24)
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.Cryptography.InvalidKeyLength"), 0, isWarning: false, "Key"));
			}
			if (string.IsNullOrEmpty(cryptographyComponent.Text))
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.Cryptography.TextRequired"), 0, isWarning: false, "Text"));
			}
			else if (!AbsArgument.BuildArgument(ExpressionHelper.GetValidVariables(cryptographyComponent), cryptographyComponent.Text).IsSafeExpression())
			{
				validationErrorCollection.Add(new ValidationError(LocalizedResourceMgr.GetString("ComponentValidators.Cryptography.InvalidText"), 0, isWarning: false, "Text"));
			}
		}
		return validationErrorCollection;
	}
}
