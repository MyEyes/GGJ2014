using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Otherworld.Utilities
{
    public class TaskHelper
    {
        public static Action SetDelay (int delayInMilliseconds, Action action)
        {
            Timer timer = new Timer (delayInMilliseconds);
            timer.AutoReset = false;
            timer.Elapsed += (sender, args) => action();
            timer.Start();

            return timer.Close;
        }
    }
}
