using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;

namespace lib {
    public class XpathExpressionInfo {
        /// <summary>
        /// The uncompiled XPath expression
        /// </summary>
        public string XpathExpression { get; set; }
        /// <summary>
        /// The compiled XPath expression
        /// </summary>
        public XPathExpression CompiledExpression { get; set; }
        /// <summary>
        /// The name of a CSS class selector used to render a matching node
        /// </summary>
        public string CssClassSelector { get; set; }
        /// <summary>
        /// The base name to use when generating an HTML anchor for a matchng node
        /// </summary>
        public string RootAnchorName { get; set; }
        /// <summary>
        /// The number of matches found for this XPath expression
        /// </summary>
        public int NumMatches { get; set; }

        public XpathExpressionInfo() {
        }

        public XpathExpressionInfo(XPathExpression compiled, string xpathExpression, string cssClasSelector) {
            this.CompiledExpression = compiled;
            this.XpathExpression = xpathExpression;
            this.CssClassSelector = cssClasSelector;
        }

        public static XpathExpressionInfo Duplicate(XpathExpressionInfo xei) {
            //XpathExpressionInfo xei2 = new XpathExpressionInfo(xei.CompiledExpression, xei.XpathExpression, xei.CssClassSelector);
            //xei2.NumMatches = xei.NumMatches;
            //xei2.RootAnchorName = xei.RootAnchorName;
            return xei.MemberwiseClone() as XpathExpressionInfo;
        }
    }
}
