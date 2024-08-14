using System;
using System.IO;

namespace TCX.CFD.Classes;

public static class WavFileFormatReader
{
	public static WavFileFormat GetWavFileFormat(FileStream wavFileStream)
	{
		wavFileStream.Seek(22L, SeekOrigin.Begin);
		byte[] array = new byte[2];
		wavFileStream.Read(array, 0, 2);
		short channels = BitConverter.ToInt16(array, 0);
		byte[] array2 = new byte[4];
		wavFileStream.Read(array2, 0, 4);
		int sampleRate = BitConverter.ToInt32(array2, 0);
		wavFileStream.Seek(32L, SeekOrigin.Begin);
		byte[] array3 = new byte[2];
		wavFileStream.Read(array3, 0, 2);
		int bitsPerSample = BitConverter.ToInt16(array3, 0) * 8;
		WavFileFormat result = default(WavFileFormat);
		result.Channels = channels;
		result.SampleRate = sampleRate;
		result.BitsPerSample = bitsPerSample;
		return result;
	}
}
