using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NLog;

using Mono.Logging.Calculator;

namespace Mono.Logging
{
    class Program
    {
        static void Main(string[] args)
        {
            var doer = new Doer();
            doer.DoSomething();

            var addOperation = new AddOperation();
            addOperation.Add(1, 2);

            var otherdoer = new Otherdoer();
            otherdoer.MyMethod();
            otherdoer.MyMethod1();

            var splitOperation = new SplitOperation();
            splitOperation.Split(0, 1);
            splitOperation.Split(1, 0);

            // http://nlog-project.org/2011/10/30/using-nlog-with-mono.html
            LogManager.Configuration = null;

            Console.Read();
        }
    }
}
