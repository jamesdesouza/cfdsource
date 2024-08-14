using TCX.CFD.Classes.Expressions;

namespace TCX.CFD.Controls;

public interface IExpressionEditorControl
{
	AbsArgument GetArgument();

	void UpdateConstantValues();
}
