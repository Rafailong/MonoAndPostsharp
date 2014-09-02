using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mono.Logging.Calculator
{
    public class SplitOperation
    {
        private double TOLERANCE = 0d;

        [ExceptionWrapper]
        public double Split(double a, double b)
        {
            if (System.Math.Abs(b) == TOLERANCE)
            {
                throw new NotSupportedException("split by 0 is Infinity");
            }

            return a/b;
        }
    }
}
