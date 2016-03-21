using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.IO;

/// <summary>
/// Summary description for LogHelper
/// </summary>
public class LogHelper
{
	public LogHelper()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public void ErrorLogging(Exception ex)
    {
        String FilePath = ConfigurationSettings.AppSettings["ErrorFile"].ToString();
        using (StreamWriter writer = new StreamWriter(FilePath, true))
        {
            writer.WriteLine("Message :" + ex.Message + "<br/>" + Environment.NewLine + "StackTrace :" + ex.StackTrace +
               "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
            writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
        }

    }
}