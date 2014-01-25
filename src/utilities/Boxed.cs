namespace Vest.utilities
{
    public class Boxed<T> where T : struct
    {
        private readonly T value;

        public Boxed (T value)
        {
            this.value = value;
        }

        public T Value
        {
            get { return value; }
        }
    }

    public static class BoxedHelper
    {
        public static Boxed<T> Box<T> (this T unboxed) where T : struct
        {
            return new Boxed<T> (unboxed);
        }

        public static Boxed<T> Box<T> (this T? unboxed) where T : struct
        {
            return unboxed.HasValue ? unboxed.Value.Box() : null;
        }

        public static T? Unbox<T> (this Boxed<T> boxed) where T : struct
        {
            if (boxed == null)
                return null;
            return boxed.Value;
        }
    }
}