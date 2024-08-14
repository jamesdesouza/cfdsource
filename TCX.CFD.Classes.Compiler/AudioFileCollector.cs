using System.Collections.Generic;
using System.IO;
using TCX.CFD.Classes.FileSystem;
using TCX.CFD.Properties;

namespace TCX.CFD.Classes.Compiler;

public class AudioFileCollector
{
	private readonly List<string> audioFileList;

	public bool IncludeAllFiles { get; set; }

	public bool IncludeBeepFile { get; set; }

	private bool IsWavFileValid(FileInfo fileInfo, AbsCompilerResultCollector compilerResultCollector, int progress, CompilerErrorCounter errorCounter)
	{
		if (fileInfo.Length > 104857600)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidWavFileFormat"), fileInfo.Name, "Size " + fileInfo.Length + " bytes"));
			return false;
		}
		using FileStream wavFileStream = fileInfo.OpenRead();
		WavFileFormat wavFileFormat = WavFileFormatReader.GetWavFileFormat(wavFileStream);
		if (wavFileFormat.Channels != 1)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidWavFileFormat"), fileInfo.Name, wavFileFormat.Channels + " channels"));
			return false;
		}
		if (wavFileFormat.SampleRate != 8000)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidWavFileFormat"), fileInfo.Name, wavFileFormat.SampleRate + " samples per second"));
			return false;
		}
		if (wavFileFormat.BitsPerSample != 16)
		{
			CompilerHelper.ReportCompilerMessage(compilerResultCollector, progress, errorCounter, CompilerMessageTypes.Error, string.Format(LocalizedResourceMgr.GetString("Compiler.InvalidWavFileFormat"), fileInfo.Name, wavFileFormat.BitsPerSample + " bits per sample"));
			return false;
		}
		return true;
	}

	public AudioFileCollector()
	{
		IncludeAllFiles = false;
		IncludeBeepFile = false;
		audioFileList = new List<string>();
	}

	public void AddAudioFile(string fileName)
	{
		audioFileList.Add(fileName);
	}

	public void CopyRequiredAudioFiles(ProjectObject projectObject, AbsCompilerResultCollector compilerResultCollector, int progress, CompilerErrorCounter errorCounter, string outputDirectory)
	{
		string path = Path.Combine(projectObject.GetFolderPath(), "Audio");
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
		string text = Path.Combine(outputDirectory, "Audio");
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		if (IncludeBeepFile)
		{
			CompilerHelper.WriteOutput(Path.Combine(text, "beep.wav"), Resources.beep);
		}
		string[] files = Directory.GetFiles(path);
		for (int i = 0; i < files.Length; i++)
		{
			FileInfo fileInfo = new FileInfo(files[i]);
			if ((IncludeAllFiles || audioFileList.Contains(fileInfo.Name)) && IsWavFileValid(fileInfo, compilerResultCollector, progress, errorCounter))
			{
				fileInfo.CopyTo(Path.Combine(text, fileInfo.Name), overwrite: true);
			}
		}
	}
}
