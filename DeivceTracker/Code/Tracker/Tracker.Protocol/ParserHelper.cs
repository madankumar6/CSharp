using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tracker.Common;
using Tracker.Common.Model;
using Utils;

namespace Tracker.Parser
{
    public static class ParserHelper
    {
        public static String TSubstring(ref String SourceString, int startIndex, int length)
        {
            String tStr = SourceString.Substring(startIndex, length);
            SourceString = SourceString.Substring(length);
            return tStr;
        }

        public static void TSubstring(ref String SourceString, ref string DestinationString, int startIndex, int length)
        {
            DestinationString = SourceString.Substring(startIndex, length);
            SourceString = SourceString.Substring(length);
        }

        public static void TSkip(ref String SourceString, ref string DestinationString, int upToCharIndex)
        {
            DestinationString = SourceString.Substring(upToCharIndex, SourceString.Length - upToCharIndex);
        }
    }
}
