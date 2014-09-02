using System;
using System.Diagnostics;
using System.Reflection;
using PostSharp.Aspects;

namespace Mono.Logging
{
    [Serializable]
    public sealed class TracingWithStopWatchAspect : OnMethodBoundaryAspect
    {
        private string _methodName;
        private string _className;
        private string[] _methodArguments;

        [NonSerialized]
        private Stopwatch _stopwatch;

        #region Build-Time Logic

        public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
        {
            _methodName = method.Name;
            _className = method.DeclaringType.FullName;

            ParameterInfo[] parameterInfos = method.GetParameters();
            if (parameterInfos.Length > 0)
            {
                _methodArguments = new string[parameterInfos.Length];
                for (int i = 0; i < parameterInfos.Length; i++)
                {
                    _methodArguments[i] = parameterInfos[i].Name;
                }
            }
        }

        #endregion

        public override void OnEntry(MethodExecutionArgs args)
        {
            Trace.WriteLine(string.Format("executing {0}.{1}", _className, _methodName));
            if (_methodArguments != null && _methodArguments.Length > 0)
            {
                for (int i = 0; i < _methodArguments.Length; i++)
                {
                    string temp = string.Format("[ {0}: {1} ]", _methodArguments[i], args.Arguments.GetArgument(i));
                    _methodArguments[i] = temp;
                }
                string @join = string.Join(",", _methodArguments);
                Trace.WriteLine(string.Format("params: {0}", @join));
            }

            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            _stopwatch.Stop();
            Trace.WriteLine(string.Format("elapsed: {0}", _stopwatch.Elapsed));
        }
    }
}
