using System;

namespace TCX.CFD.Classes;

public class ErrorHelper
{
	public static string GetErrorDescription(Exception exc)
	{
		if (exc.InnerException != null)
		{
			return exc.Message + " --> " + GetErrorDescription(exc.InnerException);
		}
		return exc.Message;
	}
}
