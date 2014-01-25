using System;

namespace Otherworld.Utilities
{
    public static class StringHelper
    {
        public static String Format (this String format, params Object[] args)
        {
            return String.Format (format, args);
        }
    }
}
