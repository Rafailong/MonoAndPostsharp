using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mono.Logging
{
    public class Doer
    {
        [TracingWithStopWatchAspect]
        public void DoSomething()
        {
            Console.WriteLine("doing something!");
        }
    }
}
