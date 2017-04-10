using System.Collections.Generic;
using System.Diagnostics;
using Autofac.log4net.Extensions;

namespace Autofac.log4net.Mapping
{
    public class NamespaceLengthComparer : IComparer<string>
    {
        public int Compare(string str1, string str2)
        {
            Debug.Assert(str1 != null, "str1 != null");
            Debug.Assert(str2 != null, "str2 != null");
            var str1Len = str1.Length;
            var str2Len = str2.Length;

            return str1Len.CompareTo(str2Len).Opposite();
        }
    }
}
