<%@ Application Language="C#" %>
<%@ Import Namespace="DES" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.IO" %>

<script RunAt="server">
    //static Cache _cache = null;
    //static string _path = null;

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup

        /*
        Context.Cache.Insert("System.UseVirtualHosting",
            SystemSettings.GetSettings("System.UseVirtualHosting") == "1"
        );
        */
          
        //_cache = Context.Cache;
        //_path = Server.MapPath("Quotes.txt");

        //ArrayList quotes = ReadQuotes();

        /*
        _cache.Insert("Quotes", quotes, new CacheDependency(_path),
            Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration,
            CacheItemPriority.Default,
            new CacheItemRemovedCallback(RefreshQuotes));
        */
    }

    /*
    static void RefreshQuotes(String key, Object item,
        CacheItemRemovedReason reason)
    {
        ArrayList quotes = ReadQuotes();

        _cache.Insert("Quotes", quotes, new CacheDependency(_path),
            Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration,
            CacheItemPriority.Default,
            new CacheItemRemovedCallback(RefreshQuotes));
    }

    static ArrayList ReadQuotes()
    {
        ArrayList quotes = new ArrayList();
        StreamReader reader = null;

        try
        {
            reader = new StreamReader(_path);
            for (string line = reader.ReadLine(); line != null;
                line = reader.ReadLine())
                quotes.Add(line);
        }
        finally
        {
            if (reader != null)
                reader.Close();
        }
        return quotes;
    }
    */
    
    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }

    protected void Application_BeginRequest(object sender, EventArgs e)
    {
        //this.Context.IsPostNotification ###
        
        QSParser qs = new QSParser(this.Request.QueryString);

        // CHECK IF THERE IS S, SS, OR /SITE/
        // if ex: /Site/PNRC/Home.aspx?SS=45 then /Default.aspx?SS=45

        string sUrl = Request.Url.AbsolutePath.ToLower(); // "/site/newsroom.aspx"
        
        StringBuilder sbUrl = new StringBuilder();

        /*
        if (sUrl.Contains("/site/"))
        {
            // TRANSLATION STAGE (AFTER REWRITING)

            // GET SITE NAME
            string sSIdentityName;
            object obj;

            if (!string.IsNullOrEmpty(qs["S"]) && string.IsNullOrEmpty(qs["SS"]))
            {
                // ex: /Site/Aboitiz/Home.aspx
                
                // TRANSLATION STAGE (AFTER REWRITING)
                // HOME ONLY

                // Contains /Site/ and S is present (contains home.aspx)
            }
            else
            {
                bool isHome = false;
                // SITE IDENTITY
                int iIndexAfterSite = sUrl.IndexOf("/site/") + 6;
                int iIndexNextSlash = sUrl.IndexOf("/", iIndexAfterSite);
                
                // NEW: /Site/Aboitiz.aspx ----- for removing Home.asp / Default.aspx
                if (iIndexAfterSite > iIndexNextSlash)
                {
                    isHome = true;
                    iIndexNextSlash = sUrl.IndexOf(".aspx", iIndexAfterSite);
                }
                
                sSIdentityName = sUrl.Substring(iIndexAfterSite, iIndexNextSlash - iIndexAfterSite);

                obj = SqlHelper.ExecuteScalar(CommandType.Text, "SELECT SiteID FROM CMS.Sites WHERE IdentityName=@IdentityName",
                    new SqlParameter("@IdentityName", sSIdentityName)
                );

                if (obj != null)
                {
                    qs["S"] = obj.ToString();
                }
                else
                {
                    return;
                }

                if (!isHome)
                {
                    // SECTION IDENTITY
                    string sSSIdentityName = Path.GetFileNameWithoutExtension(sUrl);

                    if (sSSIdentityName != "default" && sSSIdentityName != "home")
                    {
                        // Section has friendly name
                        obj = SqlHelper.ExecuteScalar(CommandType.Text, "SELECT SiteSectionID FROM CMS.SiteSections WHERE IdentityName=@IdentityName AND SiteID=@SiteID",
                            new SqlParameter("@IdentityName", sSSIdentityName),
                            new SqlParameter("@SiteID", int.Parse(qs["S"]))
                        );

                        if (obj != null)
                        {
                            qs["SS"] = obj.ToString();
                        }
                    }
                }
            }
            
            Context.RewritePath("~/Default.aspx?" + qs);
        }
        else if (!string.IsNullOrEmpty(qs["SS"]))
        {
            int iSiteSectionID;
            if (int.TryParse(qs["SS"], out iSiteSectionID))
            {
                string sSIdentityName = string.Empty;
                string sSSIdentityName = string.Empty;

                using (SqlDataReader r = SqlHelper.ExecuteReader("CMS.SELECT_SiteSections_PATH",
                    new SqlParameter("@SiteSectionID", iSiteSectionID)
                ))
                {
                    if (r.Read())
                    {
                        sSIdentityName = r["SIdentityName"].ToString();
                        sSSIdentityName = r["SSIdentityName"].ToString();
                    }
                }

                // IF SS IS NOT A WORD
                qs.Remove("ID");

                if (!string.IsNullOrEmpty(sSIdentityName) && !string.IsNullOrEmpty(sSSIdentityName))
                {
                    // S AND SS HAS IDENTITY
                    qs.Remove("S");
                    qs.Remove("SS");

                    string sQS = qs.ToString();
                    if (!string.IsNullOrEmpty(sQS)) sQS = "?" + sQS;

                    Response.Redirect("~/Site/" + sSIdentityName + "/" + sSSIdentityName + ".aspx" + sQS, true);
                }
                else if (!string.IsNullOrEmpty(sSIdentityName))
                {
                    // ONLY S HAS IDENTITY
                    qs.Remove("S");

                    string sQS = qs.ToString();
                    if (!string.IsNullOrEmpty(sQS)) sQS = "?" + sQS;

                    Response.Redirect("~/Site/" + sSIdentityName + ".aspx" + sQS, true);
                }
            }
            else
            {
                Response.Write(string.Format("<h1>{0}</h1>", qs["SS"]));
                
                // TEST IF HAS PAGE (P)
                string sPage = (!string.IsNullOrEmpty(qs["P"])) ? qs["P"] : "-1";

                string sTI = null;
                string sSI = null;
                if (!string.IsNullOrEmpty(qs["ID"]))
                {
                    ControlInfo ci = new ControlInfo(qs["ID"]);
                    sTI = ci.Key(ControlInfoEnum.TemplateIdentity);
                    sSI = ci.Key(ControlInfoEnum.SectionIdentity);
                }

                object obj2 = SqlHelper.ExecuteScalar("CMS.SELECT_SitePage_SectionIsWord",
                    new SqlParameter("@SiteID", int.Parse(qs["S"])),
                    new SqlParameter("@SectionName", qs["SS"]),
                    new SqlParameter("@PageIdentity", int.Parse(sPage)),
                    new SqlParameter("@TemplateIdentity", sTI),
                    new SqlParameter("@SectionIdentity", sSI)
                );
                
                
                // SS IS WORD
                // CHECK S
                object obj = SqlHelper.ExecuteScalar(CommandType.Text, "SELECT IdentityName FROM CMS.Sites WHERE SiteID=@SiteID",
                    new SqlParameter("@SiteID", int.Parse(qs["S"]))
                );

                
                qs.Remove("S");
                qs.Remove("SS");
                qs.Remove("ID");
                //qs.Remove("P");

                string sQS = qs.ToString();
                if (!string.IsNullOrEmpty(sQS)) sQS = "?" + sQS;

                Response.Redirect("~/Site/" + obj + "/" + obj2 + ".aspx" + sQS, true);
            }
        }
        else if (!string.IsNullOrEmpty(qs["S"]))
        {
            object obj = SqlHelper.ExecuteScalar(CommandType.Text, "SELECT IdentityName FROM CMS.Sites WHERE SiteID=@SiteID",
                new SqlParameter("@SiteID", int.Parse(qs["S"]))
            );

            qs.Remove("S");

            string sQS = qs.ToString();
            if (!string.IsNullOrEmpty(sQS)) sQS = "?" + sQS;

            Response.Redirect("~/Site/" + obj + ".aspx" + sQS, true);
        }
        */
    }

</script>

