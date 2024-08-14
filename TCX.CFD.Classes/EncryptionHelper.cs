using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TCX.CFD.Classes;

public class EncryptionHelper
{
	private static void encryptFile(string inputFilePath, string outputFilePath, SymmetricAlgorithm symmetricAlgorithm, byte[] keyBytes, byte[] ivBytes)
	{
		using FileStream fileStream = new FileStream(inputFilePath, FileMode.Open);
		using FileStream stream = new FileStream(outputFilePath, FileMode.Create);
		using CryptoStream cryptoStream = new CryptoStream(stream, symmetricAlgorithm.CreateEncryptor(keyBytes, ivBytes), CryptoStreamMode.Write);
		byte[] array = new byte[1024];
		int num = 0;
		while ((num = fileStream.Read(array, 0, array.Length)) > 0)
		{
			cryptoStream.Write(array, 0, num);
		}
		cryptoStream.FlushFinalBlock();
	}

	private static string convertToHexadecimal(byte[] data)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < data.Length; i++)
		{
			stringBuilder.AppendFormat("{0,2:x2}", data[i]);
		}
		return stringBuilder.ToString();
	}

	private static string calculateKey(string fileName)
	{
		MD5 mD = MD5.Create();
		byte[] bytes = Encoding.UTF8.GetBytes(fileName);
		return convertToHexadecimal(mD.ComputeHash(bytes));
	}

	public static void EncryptFile(string inputFilePath, string outputFilePath)
	{
		Aes aes = Aes.Create();
		aes.Padding = PaddingMode.Zeros;
		byte[] bytes = Encoding.UTF8.GetBytes(calculateKey(new FileInfo(outputFilePath).Name));
		byte[] bytes2 = Encoding.UTF8.GetBytes("NvLlYKsiSSibSyHI");
		encryptFile(inputFilePath, outputFilePath, aes, bytes, bytes2);
	}
}
