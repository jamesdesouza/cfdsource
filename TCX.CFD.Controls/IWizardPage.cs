namespace TCX.CFD.Controls;

internal interface IWizardPage
{
	void FocusFirstControl();

	bool ValidateBeforeMovingToNext();
}
