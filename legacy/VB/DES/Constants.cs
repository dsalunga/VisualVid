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
using System.Globalization;

namespace DES
{
    public enum DateType : int
    {
        Day = 1,
        Month = 2,
        Year = 3
    }
    public abstract class Constants
    {
        public static ListItem[] GetDropDownDate(DateType dtpType)
        {
            ListItem[] liDate;
            switch(dtpType)
            {
                case DateType.Day:
                    liDate = new ListItem[31];
                    for (int x = 1; x < 32; x++)
                    {
                        ListItem lstItem = new ListItem(x.ToString(), x.ToString());
                        if (DateTime.Now.Day == x)
                        {
                            lstItem.Selected = true;
                        }
                        liDate[x - 1] = lstItem;
                    }
                    break;
                case DateType.Month:
                    liDate = new ListItem[12];
                    for (int x = 1; x < 13; x++)
                    {
                        ListItem lstItem = new ListItem(DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(x), x.ToString());
                        if (DateTime.Now.Month == x)
                        {
                            lstItem.Selected = true;
                        }
                        liDate[x - 1] = lstItem;
                    }
                    break;
                case DateType.Year:
                    liDate = new ListItem[(DateTime.Now.Year-1990)+2];
                    for (int x = 1990; x < DateTime.Now.Year + 2; x++)
                    {
                        ListItem lstItem = new ListItem(x.ToString(), x.ToString());
                        if (DateTime.Now.Year == x)
                        {
                            lstItem.Selected = true;
                        }
                        liDate[x-1990] = lstItem;
                    }
                    break;
                default :
                    liDate = new ListItem[1];
                    break;
            }
            return liDate;
        }
    }
}