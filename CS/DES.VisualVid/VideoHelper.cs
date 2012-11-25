using System;
using System.Collections.Generic;
using System.Text;

namespace DES.VisualVid
{
    public abstract class VideoHelper
    {
        public static string FormatTags(string sTags)
        {
            string sResults = string.Empty;

            string[] sTagsCollection = sTags.Split(' ');
            foreach (string s in sTagsCollection)
            {
                string sTag = s.TrimEnd(',').TrimEnd(';');

                sResults += "<a class=\"tags\" href=\"/Results.aspx?Search=" + sTag + "\">" + sTag + "</a>&nbsp;";
            }

            return sResults;
        }
    }
}
