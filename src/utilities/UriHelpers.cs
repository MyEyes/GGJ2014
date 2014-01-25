using System;
using System.IO;

namespace Otherworld.Utilities
{
    public static class UriHelpers
    {
        public static string FilePath (this Uri uri)
        {
            if (!uri.IsFile)
                throw new ArgumentException ("Uri must refer to a path", "uri");

            return uri.AbsolutePath.Replace ('/', Path.DirectorySeparatorChar);
        }
    }
}
