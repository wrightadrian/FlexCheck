<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Configuration" %>
<%@ Import Namespace="System.Xml" %>
<%@ Import Namespace="System.Xml.XPath" %>
<%@ Import Namespace="System.Diagnostics" %>
<%@ Import Namespace="lib" %>
<%-- @ Import Namespace="Microsoft.Practices.EnterpriseLibrary.Logging" --%>

<script runat="server">
    
    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup
        //LogEntry le = new LogEntry();
        //le.Title = "Lifecycle Event";
        //le.Message = "Application_Start()";
        ////le.Categories.Add(LogCategory.LIFE_CYCLE);
        //le.Severity = TraceEventType.Information;
        //Logger.Write(le);

        var appSettings = WebConfigurationManager.AppSettings;
        var appKeys = appSettings.AllKeys;
        List<Tuple<string,string>> lstNamespace = GetNamespaces(appKeys);
        XmlNamespaceManager nsm = MakeNameSpaceManager(lstNamespace);
        List<XpathExpressionInfo> lstXei = MakeXpathExpressionInfoList(appKeys);
        // attempt to compile each xpath expression
        foreach (var xei in lstXei) {
            try {
                xei.CompiledExpression = XPathExpression.Compile(xei.XpathExpression, nsm);
            }
            catch (Exception ex) {
                //LogEntry le2 = new LogEntry();
                //le2.Title = "XPath compilation failed";
                //le2.Message = string.Format("expression={0}; error={1}", xei.XpathExpression, ex.Message);
                //le2.Severity = TraceEventType.Error;
                //le2.Categories.Add("General");
                //Logger.Write(le2);

                xei.CompiledExpression = null;
            }
        }
        // remove XPath expressions which wouldn't compile
        for (int i = 0; i < lstXei.Count; i++) {
            if (lstXei[i].CompiledExpression == null)
                lstXei.RemoveAt(i);
        }
        Application["lstXpathExpressionInfo"] = lstXei;

        //le.Title = "XPath to Css Class Selector Mapping";
        //le.Message = string.Format("found {0} mappings", lstXei.Count);
        ////le.Categories.Add(LogCategory.GENERAL);
        //le.Severity = TraceEventType.Information;
        //foreach (var xei in lstXei)
        //    le.ExtendedProperties.Add(new KeyValuePair<string, object>(xei.XpathExpression, xei.CssClassSelector));
        //Logger.Write(le);
    }

    void Application_End(object sender, EventArgs e) 
    {
        ////  Code that runs on application shutdown
        //LogEntry le = new LogEntry();
        //le.Title = "Lifecycle Event";
        //le.Message = "Application_End()";
        //le.Severity = TraceEventType.Information;
        ////le.Categories.Add(LogCategory.LIFE_CYCLE);
        //Logger.Write(le);
    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs
        Exception ex = Server.GetLastError().GetBaseException();
        //LogEntry le = new LogEntry();
        //le.Title = "Unhandled Exception";
        //le.Message = ex.StackTrace;
        //le.Severity = TraceEventType.Error;
        ////le.Categories.Add(LogCategory.UNHANDLED_EXCEPTION);
        //Logger.Write(le);
        Server.ClearError();
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

    private List<Tuple<string, string>> GetNamespaces(string[] keys) {
        List<Tuple<string, string>> lstNamespace = new List<Tuple<string, string>>();
        foreach (var key in keys) {
            if (key.StartsWith("namespace")) {
                var ns = WebConfigurationManager.AppSettings[key];
                var nsParts = ns.Split(new char[] { ':' });
                int colonIdx = ns.IndexOf(':');
                if (nsParts.Length > 1) {
                    var gp = new Tuple<string, string>(nsParts[0], ns.Substring(colonIdx + 1));
                    lstNamespace.Add(gp);
                }
            }
        }
        return lstNamespace;
    }

    private XmlNamespaceManager MakeNameSpaceManager(List<Tuple<string, string>> lstNamespace)
    {
        XmlNamespaceManager nsm = new XmlNamespaceManager(new XmlDocument().NameTable); // kludge!
        foreach (var ns in lstNamespace) {
            nsm.AddNamespace(ns.Item1, ns.Item2);
        }
        return nsm;
    }

    private List<XpathExpressionInfo> MakeXpathExpressionInfoList(string[] keys) {
        List<XpathExpressionInfo> lstXei = new List<XpathExpressionInfo>();
        int i = 1;
        foreach (var key in keys) {
            if (key.StartsWith("xpath")) {
                string strIdx = StringUtil.TrimFromStart(key, "xpath");
                // get the corresponding CSS class selector
                string cssClassSelectorName = "cssClassSelector" + strIdx;
                string cssClassSelector = WebConfigurationManager.AppSettings[cssClassSelectorName];
                string xpathExpression = WebConfigurationManager.AppSettings[key];
                if (cssClassSelector != null && xpathExpression != null) { // if both values found . . .
                    var xei = new XpathExpressionInfo();
                    xei.CssClassSelector = cssClassSelector;
                    xei.XpathExpression = xpathExpression;
                    xei.RootAnchorName = "a" + i.ToString("X" /* hex */);
                    lstXei.Add(xei);
                    i++;
                }
                else {
                    //LogEntry le2 = new LogEntry();
                    //le2.Message = string.Format("no match between AppSettings[{0}] and AppSettings[{1}]", key, cssClassSelectorName);
                    ////le2.Categories.Add(LogCategory.GENERAL);
                    //le2.Title = "No match between XPath expression and CSS class selector";
                    //le2.Severity = TraceEventType.Warning;
                    //Logger.Write(le2);
                }
            }
        }
        return lstXei;
    }
        
</script>
