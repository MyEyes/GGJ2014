using System;

namespace Vest
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (G game = new G())
            {
                game.Run();
            }
        }
    }
#endif
}

