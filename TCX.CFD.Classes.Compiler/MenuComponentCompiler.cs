using System.Text;
using TCX.CFD.Classes.Components;
using TCX.CFD.Classes.Expressions;
using TCX.CFD.Classes.FileSystem;

namespace TCX.CFD.Classes.Compiler;

public class MenuComponentCompiler : AbsComponentCompiler
{
	private readonly MenuComponent menuComponent;

	private void AppendOption(StringBuilder sb, string option)
	{
		if (sb.Length > 0)
		{
			sb.Append(", ");
		}
		sb.Append(option);
	}

	private bool IsBranchEnabled(MenuOptions menuOption)
	{
		for (int i = 0; i < menuComponent.EnabledActivities.Count; i++)
		{
			if ((menuComponent.EnabledActivities[i] as MenuComponentBranch).Option == menuOption)
			{
				return true;
			}
		}
		return false;
	}

	private string GetValidOptionList()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (menuComponent.IsValidOption_0 && IsBranchEnabled(MenuOptions.Option0))
		{
			AppendOption(stringBuilder, "'0'");
		}
		if (menuComponent.IsValidOption_1 && IsBranchEnabled(MenuOptions.Option1))
		{
			AppendOption(stringBuilder, "'1'");
		}
		if (menuComponent.IsValidOption_2 && IsBranchEnabled(MenuOptions.Option2))
		{
			AppendOption(stringBuilder, "'2'");
		}
		if (menuComponent.IsValidOption_3 && IsBranchEnabled(MenuOptions.Option3))
		{
			AppendOption(stringBuilder, "'3'");
		}
		if (menuComponent.IsValidOption_4 && IsBranchEnabled(MenuOptions.Option4))
		{
			AppendOption(stringBuilder, "'4'");
		}
		if (menuComponent.IsValidOption_5 && IsBranchEnabled(MenuOptions.Option5))
		{
			AppendOption(stringBuilder, "'5'");
		}
		if (menuComponent.IsValidOption_6 && IsBranchEnabled(MenuOptions.Option6))
		{
			AppendOption(stringBuilder, "'6'");
		}
		if (menuComponent.IsValidOption_7 && IsBranchEnabled(MenuOptions.Option7))
		{
			AppendOption(stringBuilder, "'7'");
		}
		if (menuComponent.IsValidOption_8 && IsBranchEnabled(MenuOptions.Option8))
		{
			AppendOption(stringBuilder, "'8'");
		}
		if (menuComponent.IsValidOption_9 && IsBranchEnabled(MenuOptions.Option9))
		{
			AppendOption(stringBuilder, "'9'");
		}
		if (menuComponent.IsValidOption_Star && IsBranchEnabled(MenuOptions.OptionStar))
		{
			AppendOption(stringBuilder, "'*'");
		}
		if (menuComponent.IsValidOption_Pound && IsBranchEnabled(MenuOptions.OptionPound))
		{
			AppendOption(stringBuilder, "'#'");
		}
		return stringBuilder.ToString();
	}

	private string GetMenuOptionAsString(MenuOptions menuOption)
	{
		return menuOption switch
		{
			MenuOptions.Option0 => "0", 
			MenuOptions.Option1 => "1", 
			MenuOptions.Option2 => "2", 
			MenuOptions.Option3 => "3", 
			MenuOptions.Option4 => "4", 
			MenuOptions.Option5 => "5", 
			MenuOptions.Option6 => "6", 
			MenuOptions.Option7 => "7", 
			MenuOptions.Option8 => "8", 
			MenuOptions.Option9 => "9", 
			MenuOptions.OptionStar => "*", 
			MenuOptions.OptionPound => "#", 
			_ => string.Empty, 
		};
	}

	private bool IsRepeatOptionUsed()
	{
		return menuComponent.RepeatOption switch
		{
			DtmfDigits.Digit0 => menuComponent.IsValidOption_0, 
			DtmfDigits.Digit1 => menuComponent.IsValidOption_1, 
			DtmfDigits.Digit2 => menuComponent.IsValidOption_2, 
			DtmfDigits.Digit3 => menuComponent.IsValidOption_3, 
			DtmfDigits.Digit4 => menuComponent.IsValidOption_4, 
			DtmfDigits.Digit5 => menuComponent.IsValidOption_5, 
			DtmfDigits.Digit6 => menuComponent.IsValidOption_6, 
			DtmfDigits.Digit7 => menuComponent.IsValidOption_7, 
			DtmfDigits.Digit8 => menuComponent.IsValidOption_8, 
			DtmfDigits.Digit9 => menuComponent.IsValidOption_9, 
			DtmfDigits.DigitStar => menuComponent.IsValidOption_Star, 
			DtmfDigits.DigitPound => menuComponent.IsValidOption_Pound, 
			_ => false, 
		};
	}

	private string GetRepeatOptionAsString()
	{
		return menuComponent.RepeatOption switch
		{
			DtmfDigits.Digit0 => "0", 
			DtmfDigits.Digit1 => "1", 
			DtmfDigits.Digit2 => "2", 
			DtmfDigits.Digit3 => "3", 
			DtmfDigits.Digit4 => "4", 
			DtmfDigits.Digit5 => "5", 
			DtmfDigits.Digit6 => "6", 
			DtmfDigits.Digit7 => "7", 
			DtmfDigits.Digit8 => "8", 
			DtmfDigits.Digit9 => "9", 
			DtmfDigits.DigitStar => "*", 
			DtmfDigits.DigitPound => "#", 
			_ => string.Empty, 
		};
	}

	public MenuComponentCompiler(AbsCompilerResultCollector compilerResultCollector, FileObject fileObject, int progress, CompilerErrorCounter errorCounter, MenuComponent menuComponent)
		: base(compilerResultCollector, fileObject, progress, errorCounter, ExpressionHelper.GetValidVariables(menuComponent), menuComponent.GetRootFlow().FlowType, menuComponent)
	{
		this.menuComponent = menuComponent;
	}

	public override CompilationResult Compile(bool isDebugBuild, string parentComponentListName, StringBuilder componentsInitializationScriptSb, StringBuilder externalCodeLauncherSb, AudioFileCollector audioFileCollector)
	{
		if (menuComponent.EnabledActivities.Count < 2)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.MenuComponent.FewOptions"), menuComponent.Name), fileObject, flowType, menuComponent);
		}
		else
		{
			if (menuComponent.InitialPrompts.Count == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.MenuComponent.InitialPromptIsEmpty"), menuComponent.Name), fileObject, flowType, menuComponent);
			}
			if (menuComponent.SubsequentPrompts.Count == 0 && menuComponent.MaxRetryCount > 1)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.MenuComponent.SubsequentPromptIsEmpty"), menuComponent.Name), fileObject, flowType, menuComponent);
			}
			if (menuComponent.TimeoutPrompts.Count == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Warning, string.Format(LocalizedResourceMgr.GetString("Compiler.MenuComponent.TimeoutPromptIsEmpty"), menuComponent.Name), fileObject, flowType, menuComponent);
			}
			if (menuComponent.InvalidDigitPrompts.Count == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Warning, string.Format(LocalizedResourceMgr.GetString("Compiler.MenuComponent.InvalidDigitPromptIsEmpty"), menuComponent.Name), fileObject, flowType, menuComponent);
			}
			if (menuComponent.Timeout == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.MenuComponent.TimeoutMustBeGreaterThanZero"), menuComponent.Name), fileObject, flowType, menuComponent);
			}
			if (menuComponent.MaxRetryCount == 0)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.MenuComponent.MaxRetryCountMustBeGreaterThanZero"), menuComponent.Name), fileObject, flowType, menuComponent);
			}
			if (IsRepeatOptionUsed())
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.MenuComponent.RepeatOptionIsValidOption"), menuComponent.Name), fileObject, flowType, menuComponent);
			}
			componentsInitializationScriptSb.AppendFormat("MenuComponent {0} = new MenuComponent(\"{0}\", callflow, myCall, logHeader);", menuComponent.Name).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.AllowDtmfInput = {1};", menuComponent.Name, menuComponent.AcceptDtmfInput ? "true" : "false").AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.MaxRetryCount = {1};", menuComponent.Name, menuComponent.MaxRetryCount - 1).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Timeout = {1};", menuComponent.Name, 1000 * menuComponent.Timeout).AppendLine().Append("            ");
			if (menuComponent.RepeatOption != DtmfDigits.None)
			{
				componentsInitializationScriptSb.AppendFormat("{0}.RepeatMenuOption = '{1}';", menuComponent.Name, GetRepeatOptionAsString()).AppendLine().Append("            ");
			}
			componentsInitializationScriptSb.AppendFormat("{0}.ValidOptionList.AddRange(new char[] {{ {1} }});", menuComponent.Name, GetValidOptionList()).AppendLine().Append("            ");
			foreach (Prompt ınitialPrompt in menuComponent.InitialPrompts)
			{
				ınitialPrompt.Accept(this, isDebugBuild, menuComponent.Name, "InitialPrompts", componentsInitializationScriptSb, audioFileCollector);
			}
			foreach (Prompt subsequentPrompt in menuComponent.SubsequentPrompts)
			{
				subsequentPrompt.Accept(this, isDebugBuild, menuComponent.Name, "SubsequentPrompts", componentsInitializationScriptSb, audioFileCollector);
			}
			foreach (Prompt ınvalidDigitPrompt in menuComponent.InvalidDigitPrompts)
			{
				ınvalidDigitPrompt.Accept(this, isDebugBuild, menuComponent.Name, "InvalidDigitPrompts", componentsInitializationScriptSb, audioFileCollector);
			}
			foreach (Prompt timeoutPrompt in menuComponent.TimeoutPrompts)
			{
				timeoutPrompt.Accept(this, isDebugBuild, menuComponent.Name, "TimeoutPrompts", componentsInitializationScriptSb, audioFileCollector);
			}
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, menuComponent.Name).AppendLine().Append("            ");
			string text = menuComponent.Name + "_Conditional";
			componentsInitializationScriptSb.AppendFormat("ConditionalComponent {0} = new ConditionalComponent(\"{0}\", callflow, myCall, logHeader);", text).AppendLine().Append("            ");
			componentsInitializationScriptSb.AppendFormat("{0}.Add({1});", parentComponentListName, text).AppendLine().Append("            ");
			bool flag = false;
			for (int i = 0; i < menuComponent.EnabledActivities.Count; i++)
			{
				MenuComponentBranch menuComponentBranch = menuComponent.EnabledActivities[i] as MenuComponentBranch;
				AbsArgument absArgument;
				if (menuComponentBranch.Option == MenuOptions.TimeoutOrInvalidOption)
				{
					flag = true;
					absArgument = AbsArgument.BuildArgument(validVariables, string.Format("{0}.Result == MenuComponent.MenuResults.InvalidOption || {0}.Result == MenuComponent.MenuResults.Timeout", menuComponent.Name));
				}
				else
				{
					absArgument = AbsArgument.BuildArgument(validVariables, string.Format("{0}.Result == MenuComponent.MenuResults.ValidOption && {0}.SelectedOption == '{1}'", menuComponent.Name, GetMenuOptionAsString(menuComponentBranch.Option)));
				}
				componentsInitializationScriptSb.AppendFormat("{0}.ConditionList.Add(() => {{ return {1}; }});", text, absArgument.GetCompilerString()).AppendLine().Append("            ");
				componentsInitializationScriptSb.AppendFormat("{0}.ContainerList.Add(new SequenceContainerComponent(\"{0}_{1}\", callflow, myCall, logHeader));", text, menuComponentBranch.Option).AppendLine().Append("            ");
				if (menuComponentBranch.GetCompiler(compilerResultCollector, fileObject, progress, errorCounter).Compile(isDebugBuild, $"{text}.ContainerList[{i}].ComponentList", componentsInitializationScriptSb, externalCodeLauncherSb, audioFileCollector) == CompilationResult.Cancelled)
				{
					return CompilationResult.Cancelled;
				}
			}
			if (!flag)
			{
				CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.MenuComponent.FewOptions"), menuComponent.Name), fileObject, flowType, menuComponent);
			}
		}
		if (!compilerResultCollector.CancellationPending)
		{
			return CompilationResult.Finished;
		}
		return CompilationResult.Cancelled;
	}
}
