using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lib {
    public class StringUtil {

        public static string TrimFromStart(string value, string stuffToTrim) {
            int trimLen = stuffToTrim.Length;
            if (trimLen >= value.Length)
                return string.Empty;
            return value.Substring(trimLen);
        }
    }
}
