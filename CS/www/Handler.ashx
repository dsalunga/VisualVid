<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;

using DES;

public class Handler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        string sType = context.Request.QueryString["Type"];
        string sID = context.Request.QueryString["ID"];
        
        context.Response.Cache.SetCacheability(HttpCacheability.Public);
        context.Response.BufferOutput = false;
        //context.Response.Write("Hello World");

        object obj;

        switch (sType)
        {
            case "CSS":
                context.Response.ContentType = "text/css";
                
                obj = SqlHelper.ExecuteScalar(CommandType.Text,
                    "SELECT Content FROM CMS.CSSTemplates WHERE CSSTemplateID=@CSSTemplateID",
                    new SqlParameter("@CSSTemplateID", int.Parse(sID))
                );

                context.Response.Write(obj);
                break;
            case "Image":
                break;
            case "Script":
                context.Response.ContentType = "text/javascript";

                obj = SqlHelper.ExecuteScalar(CommandType.Text,
                    "SELECT Content FROM CMS.Scripts WHERE ScriptID=@ScriptID",
                    new SqlParameter("@ScriptID", int.Parse(sID))
                );

                context.Response.Write(obj);
                break;
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}