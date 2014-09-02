using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using PostSharp.Aspects;

namespace Mono.Logging
{
    [Serializable]
    public sealed class ExceptionWrapper : OnMethodBoundaryAspect
    {
        private string _methodName;
        private string _className;
        private string[] _methodArguments;
        private bool _hasToReturnSomething;
        private string _join;

        private Type _typeToReturn;

        public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
        {
            // fetch the method's name and class full-name where the exception was trown
            _methodName = method.Name;
            _className = method.DeclaringType.FullName;

            // get the method's parameters' name
            ParameterInfo[] parameterInfos = method.GetParameters();
            if (parameterInfos.Length <= 0) return;
            _methodArguments = new string[parameterInfos.Length];
            for (int i = 0; i < parameterInfos.Length; i++)
            {
                _methodArguments[i] = parameterInfos[i].Name;
            }

            // figuring out if the method must return a value or not
            MethodInfo methodInfo = method as MethodInfo;
            if (methodInfo == null) return;
            _hasToReturnSomething = !methodInfo.ReturnType.FullName.Equals("System.Void");
            if (_hasToReturnSomething)
            {
                _typeToReturn = methodInfo.ReturnType;
            }
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            if (_methodArguments == null || _methodArguments.Length <= 0) return;
            var methodArgumentsTemp = new string[_methodArguments.Length];
            for (int i = 0; i < _methodArguments.Length; i++)
            {
                string temp = string.Format("[ {0}: {1} ]", _methodArguments[i], args.Arguments.GetArgument(i));
                methodArgumentsTemp[i] = temp;
            }
            _join = string.Join(",", methodArgumentsTemp);
        }
        
        public override void OnException(MethodExecutionArgs args)
        {
            // building the log message
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(string.Format("Exception in: {0}.{1}({2})", _className, _methodName, _join));
            Exception exception = args.Exception;
            builder.Append(string.Format("Exception: {0} \r Message: {1}", exception.GetType(), exception.Message));
            while (exception.InnerException != null)
            {
                Exception innerException = exception.InnerException;
                builder.Append(string.Format("Exception: {0} \r Message: {1}", innerException.GetType(),innerException.Message));
                exception = innerException;
            }

            // this is like our log, send the message to the log
            Trace.WriteLine(builder.ToString());

            // figuring out if the method's return value
            if(_hasToReturnSomething)
            {
                // if the method has a empty constructor
                // we build a default instance of it and 
                // set that instance as return value
                ConstructorInfo constructorInfo = _typeToReturn.GetConstructor(Type.EmptyTypes);
                if (constructorInfo != null)
                {
                    args.ReturnValue = Activator.CreateInstance(_typeToReturn);
                }
                    // if the method's return type has no empty constructor
                    // we find out if we can assing null to the type, is the type is Nullable
                    // so we set null as return value
                else if (!_typeToReturn.IsValueType || Nullable.GetUnderlyingType(_typeToReturn) != null)
                {
                    args.ReturnValue = null;
                }
            }

            // and this is like the user notification
            // NOTE: this kind of actions must be implemented in the upper layer of the system
            // the layer where the notification will be presented to the user
            Console.WriteLine("Sorry, something went wrong! :-(");

            args.FlowBehavior = FlowBehavior.Return;
        }
    }
}
