using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PostSharp.Patterns.Diagnostics;

namespace Mono.Logging.Calculator
{
    public class AddOperation
    {
        [Log]
        public double Add(double a, double b)
        {
            return a + b;
        }
    }
}
