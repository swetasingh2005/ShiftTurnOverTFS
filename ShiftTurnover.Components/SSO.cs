using System;
using System.Web;
using System.Text.RegularExpressions;

namespace ShiftTurnover.Components
{
	/// <summary>
	/// Summary description for SSO.
	/// </summary>
	public class SSO
	{
		public SSO()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public bool SsoIsTurnedOn()
		{
            string servervar = (HttpContext.Current.Request.ServerVariables["ALL_RAW"]);
			
			//HttpContext.Current.Response.Write(servervar);
			bool retval = false;
			if ((servervar.IndexOf("SM_USER") > 0) || (servervar.IndexOf("SMUSER") > 0))//have to check for with underscore and without underscore because SSO changed the name starting with Windows Server 2003
			{
				retval = true;			
			}
			return retval;
		}

		public string GetSMUserName()
		{
			string servervar = (HttpContext.Current.Request.ServerVariables["ALL_RAW"]);
			//HttpContext.Current.Response.Write(servervar);
			
			string retval	= "";
			string sm		= "";

			if (servervar.IndexOf("SM_USER") > 0) //Siteminder running on < Windows 2003 OS
			{
				sm = "SM_USER:";			
			}
			else if (servervar.IndexOf("SMUSER") > 0) //Siteminder running on < Windows 2003 OS
			{				
				sm = "SMUSER:";			
			}

			string[] arInfo = Regex.Split(servervar, sm);

			foreach (String s in arInfo) 
			{				
				//HttpContext.Current.Response.Write("<br /><br />" + s.ToString());
				string[] arInfo2 = Regex.Split(s.Trim(),System.Environment.NewLine);
				retval = arInfo2[0];	
			}

			//HttpContext.Current.Response.Write("<br /><br />GetSMUserName is returning: " + retval);
			return retval;
		}

		public string GetSMDomain()
		{
			string servervar	= (HttpContext.Current.Request.ServerVariables["ALL_RAW"]);
			string retval		= "";			
			string sm			= "";

			if (servervar.IndexOf("SM_AUTHDIRNAME") > 0) //Siteminder running on < Windows 2003 OS
			{
				sm = "SM_AUTHDIRNAME:";			
			}
			else if (servervar.IndexOf("SMAUTHDIRNAME") > 0) //Siteminder running on >= Windows 2003 OS
			{				
				sm = "SMAUTHDIRNAME:";			
			}
			
			string[] arInfo = Regex.Split(servervar, sm);
			foreach (String s in arInfo) 
			{				
				//HttpContext.Current.Response.Write("<br /><br />" + s.ToString());
				string[] arInfo2 = Regex.Split(s.Trim(),System.Environment.NewLine);
				retval = arInfo2[0];	
			}
			//HttpContext.Current.Response.Write("<br /><br />GetSMDomain is returning: " + retval);
			return retval;
		}
	}
}
