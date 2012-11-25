using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for ControlHelper
/// </summary>
namespace DES
{
    public enum LinkType : int
    {
        FullLink = 1,
        Page = 2
    }
    public class ControlHelper
    {
        private string _PageType;
        private int _ItemID;
        private int _SiteID;
        private bool _IsValid;
        private NameValueCollection _nvcSettings;

        public ControlHelper()
        {
        
        }

        public ControlHelper(NameValueCollection nvcQueryString)
        {
            _nvcSettings = nvcQueryString;
        }

        public ControlHelper(string strSettings)
        {
            CHProcess(strSettings);
        }

        public ControlHelper(Control ctl)
        {
            CHProcess(((Literal)ctl.FindControl("___LiteralID")).Text);
        }

        public void CHProcess(string strSettings)
        {
            _nvcSettings = new NameValueCollection();
            string[] strKeys = strSettings.Split('&');
            foreach (string strKey in strKeys)
            {
                string[] str = strKey.Split('=');
                try
                {
                    _nvcSettings.Add(str[0], str[1]);
                }
                catch { }
            }

            try
            {
                if (string.IsNullOrEmpty(_nvcSettings["ID"]))
                {
                    _SiteID = int.Parse(_nvcSettings["SiteID"]);
                    _PageType = _nvcSettings["PageType"];
                    _ItemID = int.Parse(_nvcSettings["SitePageItemID"]);
                }
                else
                {
                    _SiteID = int.Parse(_nvcSettings["S"]);
                    string[] strElement = _nvcSettings["ID"].Split(',');
                    _PageType = strElement[0];
                    _ItemID = int.Parse(strElement[1]);
                }
                _IsValid = true;
            }
            catch
            {
                _IsValid = false;
            }
        }

        public string ToLink(string strValue, LinkType lnkType)
        {
            if (strValue != null)
            {
                switch (lnkType)
                {
                    case LinkType.FullLink:
                        _nvcSettings["SS"] = strValue;
                        break;
                    case LinkType.Page:
                        _nvcSettings["ShowP"] = strValue;
                        break;
                    default:
                        _nvcSettings["SS"] = strValue;
                        break;
                }
            }
            string str = string.Empty;
            foreach (string strKey in _nvcSettings.AllKeys)
            {
                str += strKey + "=" + _nvcSettings[strKey] + "&";
            }
            return str.TrimEnd('&');
            
        }
        public string ToLink()
        {
            string str = string.Empty;
            foreach (string strKey in _nvcSettings.AllKeys)
            {
                str += strKey + "=" + _nvcSettings[strKey] + "&";
            }
            return str.TrimEnd('&');
        }

        public NameValueCollection Settings
        {
            set
            {
                _nvcSettings = value;
            }
            get
            {
                return _nvcSettings;
            }
        }             

        public bool IsValid
        {
            get
            {
                return _IsValid;
            }
        }

        public string PageType
        {
            get
            {
                return _PageType;
            }
        }

        public int ItemID
        {
            get
            {
                return _ItemID;
            }
        }

        public int SiteID
        {
            get
            {
                return _SiteID;
            }
        }
    }
}
