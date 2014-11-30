using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace lib {
    public class Namespace {

        /// <summary>
        /// Extract all the namespaces from an XML document.
        /// </summary>
        /// <param name="xdoc">The XML document from which the namespaces will be extracted.</param>
        /// <returns>A list of generic pairs.  
        /// Each pair represents one namespace.  
        /// pair.Left = the namespace prefix, 
        /// pair.Right = the namespace</returns>
        public static List<GenericPair<string, string>> GetAllNamespaces(XmlDocument xdoc) {
            List<GenericPair<string, string>> lstNamespace = new List<GenericPair<string, string>>();
            XPathNavigator xpnav = xdoc.CreateNavigator();
            traverseNodes(xpnav.SelectChildren(XPathNodeType.All), lstNamespace);
            return lstNamespace;
        }

        private static void traverseNodes(XPathNodeIterator nodeSet, List<GenericPair<string, string>> lstNs) {
            while (nodeSet.MoveNext()) {
                var node = nodeSet.Current;

                switch (node.NodeType) {
                    case XPathNodeType.Element:
                        bool hasNamespace = node.MoveToFirstNamespace(XPathNamespaceScope.Local);
                        if (hasNamespace) {
                            do {
                                GenericPair<string, string> ns = new GenericPair<string, string>(node.Name, node.Value);
                                lstNs.Add(ns);
                            } while (node.MoveToNextNamespace(XPathNamespaceScope.Local));
                            node.MoveToParent();
                        }
                        if (node.HasChildren)
                            traverseNodes(node.SelectChildren(XPathNodeType.Element), lstNs);
                        break;

                    default:
                        break;
                }
            } // while
        }
    }
}
