using System;
using System.Collections.Specialized;
using System.Web.UI;

namespace DES
{
	/// <summary>
	/// Summary description for QueryStringParser.
	/// </summary>
	public class QSParser : NameValueCollection
	{
		public QSParser()
		{
			 
		}

		public QSParser(Page p)
		{
			base.Add(p.Request.QueryString);
		}

		public QSParser(UserControl c)
		{
			base.Add(c.Request.QueryString);
		}

		public QSParser(NameValueCollection c)
		{
			base.Add(c);
		}

		public QSParser(string str)
		{
			string[] sKeys = str.Split('&');
			foreach(string s in sKeys)
			{
				string[] sKey = s.Split('=');
				string sName = string.Empty;
				string sValue = string.Empty;

				try
				{
					sName = sKey[0];
					sValue = sKey[1];
				}
				catch{}

				base.Add(sName, sValue);
			}
		}

		public override string ToString()
		{
			string s = string.Empty;

			foreach(string s_key in base.AllKeys)
			{
				s += s_key + "=" + base[s_key] + "&";
			}

			return s.TrimEnd('&');
		}

	}
}
