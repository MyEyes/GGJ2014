using System;

namespace Otherworld
{
    public static class Assert
    {
        public static void IsLessThanOrEqual<T> (T less, T greater)
            where T : IComparable
        {
            if (less.CompareTo (greater) > 0)
                throw new Exception ("Less than cannot be greater than the upper value");
        }

        public static void IsNotNull<T> (T value)
            where T : class
        {
            if (value == null)
                throw new AssertException ("Value can not be null");
        }
    }

    public class AssertException : Exception
    {
        public AssertException (string message)
            : base (message)
        {
        }

        public AssertException (string message, Exception innerException)
            : base (message, innerException)
        {
        }
    }
}
