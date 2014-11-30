using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace lib {
    public class Util {
        public static bool IsUrlReferencingThisMachine(string url, string serverName, string http_host, string[] validProtocols) {
            string hostName = http_host.Split(new Char[] { ':' })[0];  // using Split() to discard the port number if it exists

            List<string> lstSelfReferences = new List<string>(20);
            lstSelfReferences.Add("localhost");
            lstSelfReferences.Add("127.0.0.1");
            lstSelfReferences.Add("::1"); // localhost in IP6
            lstSelfReferences.Add(serverName);
            lstSelfReferences.Add(hostName);

            // resolve the server name to IP addresses then add the IP addresses to our list of InvalidHostsOrIps (the naughty list)
            IPAddress[] arrIpAddr;
            arrIpAddr = Dns.GetHostAddresses(serverName);
            foreach (var ipAddr in arrIpAddr) {
                lstSelfReferences.Add(ipAddr.ToString());
            }

            // resolve the host name to IP addresses then add the IP addresses to our list of InvalidHostsOrIps (the naughty list)
            arrIpAddr = Dns.GetHostAddresses(hostName);
            foreach (var ipAddr in arrIpAddr) {
                lstSelfReferences.Add(ipAddr.ToString());
            }

            StringBuilder sb = new StringBuilder(512);
            // if the URL start with *any* members of lstSelfReferences[] then it's self referencing
            foreach (var selfReference in lstSelfReferences) {
                foreach (var validProtocol in validProtocols) {
                    sb.Length = 0;
                    sb.AppendFormat("{0}://{1}", validProtocol, selfReference);
                    if (url.StartsWith(sb.ToString())) {
                        return true; // it's self referencing
                    }
                }
            }

            return false; // it's not self referencing
        }

        public static bool UrlHasPermittedProtocol(string url, string[] permittedProtocols) {
            foreach (var permittedProtocol in permittedProtocols) {
                if (url.StartsWith(string.Format("{0}://", permittedProtocol)))
                    return true;
            }
            return false;
        }
    }
}
