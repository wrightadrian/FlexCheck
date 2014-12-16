using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Diagnostics;
//using Microsoft.Practices.EnterpriseLibrary.Logging;

using lib;

public partial class ShowResults : System.Web.UI.Page {
    protected void Page_Load(object sender, EventArgs e) {
        //LogEntry le;
        if (PreviousPage != null && PreviousPage.IsValid) {
            var tb = (TextBox)PreviousPage.FindControl("tbXmlUri");
            if (tb == null || string.IsNullOrEmpty(tb.Text)) {
                string error = "<h2>Unable to read XML URL</h2>";
                LiteralControl lc = new LiteralControl(error);
                phAnchors.Controls.Add(lc);
                return;
            }
            string xmlUrl = tb.Text;

            string[] permittedProtocols = new string[] { "http", "https" };
            if (!Util.UrlHasPermittedProtocol(xmlUrl, permittedProtocols)) {
                //le = new LogEntry();
                //le.Categories.Add(LogCategory.GENERAL);
                //le.Title = "Invalid protocol in XML URL";
                //le.Message = xmlUrl;
                //le.Severity = TraceEventType.Error;
                //le.ExtendedProperties.Add(new KeyValuePair<string, object>("Client IP", Request.UserHostAddress));
                //Logger.Write(le);

                string error = "<h2>Invalid protocol in URL.</h2>";
                LiteralControl lc = new LiteralControl(error);
                phAnchors.Controls.Add(lc);

                return;
            }

            if (Util.IsUrlReferencingThisMachine(xmlUrl,
                                                    Request.ServerVariables["SERVER_NAME"],
                                                    Request.ServerVariables["HTTP_HOST"],
                                                    permittedProtocols)) {
                //le = new LogEntry();
                //le.Categories.Add(LogCategory.GENERAL);
                //le.Title = "Invalid XML URL";
                //le.Message = string.Format("{0} references hosting machine", xmlUrl);
                //le.Severity = TraceEventType.Error;
                //le.ExtendedProperties.Add(new KeyValuePair<string, object>("Client IP", Request.UserHostAddress));
                //Logger.Write(le);

                string error = "<h2>Invalid URL.</h2>";
                LiteralControl lc = new LiteralControl(error);
                phAnchors.Controls.Add(lc);
                return;
            }

            XmlDocument xdoc = new XmlDocument();
            try {
                /*
                 * A bunch of exceptions can potentially occur here -
                 * 1) can't resolve hostname
                 * 2) unable to connect to remote server
                 * 3) file not found (404)
                 * 4) xml parse error
                 */
                xdoc.Load(xmlUrl);
            }
            catch (Exception ex) {
                string error = "<h2>Error loading XML</h2>";
                error += string.Format("<h3>{0}</h3>", ex.Message);
                LiteralControl lce = new LiteralControl(error);
                phAnchors.Controls.Add(lce);

                //LogEntry leEx = new LogEntry();
                //leEx.Categories.Add(LogCategory.GENERAL);
                //leEx.Title = "XmlDocument.Load() failed";
                //leEx.Message = ex.Message;
                //leEx.Severity = TraceEventType.Error;
                //leEx.ExtendedProperties.Add(new KeyValuePair<string, object>("Xml URL", xmlUrl));
                //leEx.ExtendedProperties.Add(new KeyValuePair<string, object>("Client IP", Request.UserHostAddress));
                //Logger.Write(leEx);

                return;
            }

            /*
             * Make a copy of Application["lstXpathExpressionInfo"] for this request
             * so that parallel requests don't overwrite XpathExpressionInfo.NumRequests
             */
            List<XpathExpressionInfo> lstXei = Application["lstXpathExpressionInfo"] as List<XpathExpressionInfo>;
            List<XpathExpressionInfo> lstXei2 = new List<XpathExpressionInfo>(lstXei.Count);
            foreach (var xei in lstXei) {
                lstXei2.Add(XpathExpressionInfo.Duplicate(xei));
            }

            var matcher = new Xpath();
            string matches = string.Empty;
            try {
                matches = matcher.Match(xdoc, lstXei2);
            }
            catch (Exception ex) {
                string error = "<h2>Error analyzing XML</h2>";
                error += string.Format("<h3>{0}</h3>", ex.Message);
                LiteralControl lce = new LiteralControl(error);
                phAnchors.Controls.Add(lce);

                //LogEntry leEx = new LogEntry();
                //leEx.Categories.Add(LogCategory.GENERAL);
                //leEx.Title = "Xpath.Match() failed";
                //leEx.Message = ex.Message;
                //leEx.Severity = TraceEventType.Error;
                //leEx.ExtendedProperties.Add(new KeyValuePair<string, object>("Xml URL", xmlUrl));
                //leEx.ExtendedProperties.Add(new KeyValuePair<string, object>("Client IP", Request.UserHostAddress));
                //for (int i = 0; i < lstXei.Count; i++) {
                //    var xei = lstXei[i];
                //    string name = "XPath" + (i + 1);
                //    leEx.ExtendedProperties.Add(new KeyValuePair<string, object>(name, xei.XpathExpression));
                //}
                //Logger.Write(leEx);

                return;
            }

            LiteralControl lcMatches = new LiteralControl(matches);
            phResult.Controls.Add(lcMatches);

            //le = new LogEntry();
            ////le.Categories.Add(LogCategory.REQUESTS);
            //le.Title = "Request Successful";
            //le.Message = xmlUrl;
            //le.Severity = TraceEventType.Information;
            //le.ExtendedProperties.Add(new KeyValuePair<string, object>("Client IP", Request.UserHostAddress));
            //Logger.Write(le);
        }
    }
}
