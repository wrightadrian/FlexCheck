using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
//using Microsoft.Practices.EnterpriseLibrary.Logging;

public partial class _Default : System.Web.UI.Page {
    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            var appSettings = WebConfigurationManager.AppSettings;

            string defaultXmlUrl = appSettings["defaultXmlUrl"];
            if (!string.IsNullOrEmpty(defaultXmlUrl))
                tbXmlUri.Text = defaultXmlUrl;

            string flexDocURI;
            flexDocURI = appSettings["flex3DocURI"];
            if (!string.IsNullOrEmpty(flexDocURI))
                urlFlex3Doc.NavigateUrl = flexDocURI;

            flexDocURI = appSettings["flex4DocURI"];
            if (!string.IsNullOrEmpty(flexDocURI))
                urlFlex4Doc.NavigateUrl = flexDocURI;
        }
    }
}
