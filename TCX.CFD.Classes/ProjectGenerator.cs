using System;
using System.IO;

namespace TCX.CFD.Classes;

public class ProjectGenerator
{
	private enum GenerationStates
	{
		LookingForProject,
		WritingTextFile,
		WritingAudioFile
	}

	private readonly string scriptCode;

	private readonly string outputFolder;

	private string projectFolder;

	private string projectAudioFolder;

	private string projectLibrariesFolder;

	private string projectOutputFolder;

	private string projectFilePath;

	private string currentFilePath;

	private GenerationStates state;

	private const string projectFilePrefix = "// ---Project File: ";

	private const string projectFileExtension = ".cfdproj";

	private const string dialerFilePrefix = "// ---Dialer File: ";

	private const string callflowFilePrefix = "// ---Callflow File: ";

	private const string componentFilePrefix = "// ---Component File: ";

	private const string libraryFilePrefix = "// ---Library File: ";

	private const string audioFilePrefix = "// ---Audio File: ";

	private const string commentPrefix = "// ";

	private void ProcessUserFile(string filePath)
	{
		currentFilePath = Path.Combine(projectFolder, filePath);
		FileInfo fileInfo = new FileInfo(currentFilePath);
		fileInfo.Directory.Create();
		if (fileInfo.Exists)
		{
			fileInfo.Delete();
		}
	}

	private void ProcessScriptCodeLine(string line)
	{
		if (state == GenerationStates.LookingForProject)
		{
			if (line.StartsWith("// ---Project File: "))
			{
				string text = line.Substring("// ---Project File: ".Length);
				string path = text.Substring(0, text.Length - ".cfdproj".Length);
				projectFolder = Path.Combine(outputFolder, path);
				projectAudioFolder = Path.Combine(projectFolder, "Audio");
				projectLibrariesFolder = Path.Combine(projectFolder, "Libraries");
				projectOutputFolder = Path.Combine(projectFolder, "Output", "Release");
				Directory.CreateDirectory(projectFolder);
				Directory.CreateDirectory(projectAudioFolder);
				Directory.CreateDirectory(projectLibrariesFolder);
				Directory.CreateDirectory(projectOutputFolder);
				projectFilePath = (currentFilePath = Path.Combine(projectFolder, text));
				if (File.Exists(currentFilePath))
				{
					File.Delete(currentFilePath);
				}
				state = GenerationStates.WritingTextFile;
			}
		}
		else if (state == GenerationStates.WritingTextFile)
		{
			if (line.StartsWith("// ---Dialer File: "))
			{
				string filePath = line.Substring("// ---Dialer File: ".Length);
				ProcessUserFile(filePath);
			}
			else if (line.StartsWith("// ---Callflow File: "))
			{
				string filePath2 = line.Substring("// ---Callflow File: ".Length);
				ProcessUserFile(filePath2);
			}
			else if (line.StartsWith("// ---Component File: "))
			{
				string filePath3 = line.Substring("// ---Component File: ".Length);
				ProcessUserFile(filePath3);
			}
			else if (line.StartsWith("// ---Library File: "))
			{
				string path2 = line.Substring("// ---Library File: ".Length);
				currentFilePath = Path.Combine(projectLibrariesFolder, path2);
				if (File.Exists(currentFilePath))
				{
					File.Delete(currentFilePath);
				}
			}
			else if (line.StartsWith("// ---Audio File: "))
			{
				string path3 = line.Substring("// ---Audio File: ".Length);
				currentFilePath = Path.Combine(projectAudioFolder, path3);
				state = GenerationStates.WritingAudioFile;
			}
			else if (line.StartsWith("// "))
			{
				File.AppendAllText(currentFilePath, line.Substring("// ".Length) + Environment.NewLine);
			}
		}
		else if (state == GenerationStates.WritingAudioFile)
		{
			byte[] bytes = Convert.FromBase64String(line.Substring("// ".Length));
			File.WriteAllBytes(currentFilePath, bytes);
			state = GenerationStates.WritingTextFile;
		}
	}

	public ProjectGenerator(string scriptCode, string outputFolder)
	{
		this.scriptCode = scriptCode;
		this.outputFolder = outputFolder;
		state = GenerationStates.LookingForProject;
	}

	public string Generate()
	{
		string[] array = scriptCode.Split('\n');
		foreach (string text in array)
		{
			ProcessScriptCodeLine(text.Trim());
		}
		return projectFilePath;
	}
}
