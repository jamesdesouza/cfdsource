using System;
using System.Net.Mail;

namespace TCX.CFD.Classes.Expressions;

public static class EMailValidator
{
	public static bool IsEmail(string email)
	{
		try
		{
			new MailMessage().To.Add(email);
			return true;
		}
		catch (FormatException)
		{
			return false;
		}
	}

	public static bool IsEmailList(string emailList)
	{
		try
		{
			new MailMessage().To.Add(emailList);
			return true;
		}
		catch (FormatException)
		{
			return false;
		}
	}
}
