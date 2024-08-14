using System;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using TCX.CFD.Classes;
using TCX.CFD.Classes.Compiler;
using TCX.CFD.Classes.FileSystem;
using TCX.CFD.Forms;

namespace TCX.CFD;

internal static class Program
{
	[STAThread]
	private static void Main(params string[] args)
	{
		if (args.Length == 3)
		{
			if (args[1] == "--build")
			{
				string text = args[0];
				FileInfo fileInfo = new FileInfo(text);
				if (!fileInfo.Exists)
				{
					Console.WriteLine("The project file '{0}' was not found.", text);
					Environment.Exit(2);
					return;
				}
				ProjectObject projectObject = new ProjectObject(fileInfo.DirectoryName, fileInfo.Name, null);
				projectObject.Open();
				CommandLineCompilerResultCollector commandLineCompilerResultCollector = new CommandLineCompilerResultCollector(args[2]);
				new ProjectCompiler(commandLineCompilerResultCollector, projectObject).Compile(isDebugBuild: false);
				Environment.Exit((commandLineCompilerResultCollector.ErrorCount != 0) ? 3 : 0);
			}
			else
			{
				Console.WriteLine("Usage: 3CX Call Flow Designer.exe PROJECT_PATH --build BUILD_LOG_FILE");
				Environment.Exit(1);
			}
			return;
		}
		Application.ThreadException += Application_ThreadException;
		try
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(defaultValue: false);
			Application.Run(new MainForm(args));
		}
		catch (MissingManifestResourceException ex)
		{
			MessageBox.Show($"Resource file for culture '{CultureInfo.CurrentCulture.Name}' could not be found.\n\n{ex.Message}\n\nThe application will now exit.", "3CX Call Flow Designer", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			Application.Exit();
		}
		catch (Exception ex2)
		{
			if (MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("Main.MessageBox.Error.FatalError"), ex2.ToString()), LocalizedResourceMgr.GetString("Main.MessageBox.Title"), MessageBoxButtons.YesNo, MessageBoxIcon.Hand) == DialogResult.Yes)
			{
				Application.Exit();
			}
		}
	}

	private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
	{
		if (e.Exception is MissingManifestResourceException)
		{
			if (MessageBox.Show($"Resource file for culture '{CultureInfo.CurrentCulture.Name}' could not be found.\n\n{e.Exception.Message}\n\nPlease, save your changes and restart the application.\n\nDo you want to restart the application now?", "3CX Call Flow Designer", MessageBoxButtons.YesNo, MessageBoxIcon.Hand) == DialogResult.Yes)
			{
				Application.Exit();
			}
		}
		else if (MessageBox.Show(string.Format(LocalizedResourceMgr.GetString("Main.MessageBox.Error.FatalError"), e.Exception.ToString()), LocalizedResourceMgr.GetString("Main.MessageBox.Title"), MessageBoxButtons.YesNo, MessageBoxIcon.Hand) == DialogResult.Yes)
		{
			Application.Exit();
		}
	}
}
