using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for Utilities
/// </summary>
/// 

namespace DES
{
    public abstract class WebHelper
    {
        public static string SetStateImage(object obj)
        {
            bool b = false;

            try { b = (bool)obj; }
            catch { }

            return (b) ? "/_CMS/Images/ico_check.gif" : "/_CMS/Images/ico_x.gif";
        }

        public static string SetStateImageNull(object obj)
        {
            bool b = false;

            try { b = (bool)obj; }
            catch { }

            return (b) ? "/_CMS/Images/ico_check.gif" : "/_CMS/Images/px.gif";
        }

        public static string GetQueryString(Control c)
        {
            return ((Literal)c.FindControl("___LiteralID")).Text;
        }

        public static bool SetQueryString(Control c, string sQS)
        {
            Literal l = (Literal)c.FindControl("___LiteralID"); //new Literal();
            if (l == null)
            {
                l = new Literal();
                l.ID = "___LiteralID";
                l.Text = sQS;
                l.Visible = false;
                c.Controls.Add(l);
            }
            else
            {
                l.Text = sQS;
            }

            return true;
        }
    }
}