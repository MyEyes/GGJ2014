namespace Otherworld.Utilities
{
    public static class MathHelpers
    {
        public static float Lerp (float x, float fromMin, float fromMax, float toMin, float toMax)
        {
            return (x - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
        }

        public static float Lerp (float x, float toMin, float toMax)
        {
            return Lerp(x, 0.0f, 1.0f, toMin, toMax);
        }

        public static float Unlerp (float x, float fromMin, float fromMax)
        {
            return Lerp(x, fromMin, fromMax, 0.0f, 1.0f);
        }
    }
}
