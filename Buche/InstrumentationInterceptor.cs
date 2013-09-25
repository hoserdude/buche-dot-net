using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Buche
{
    public class InstrumentationInterceptor : IInterceptionBehavior
    {
        private static readonly ILogger Log = ContainerLocator.Container.Resolve<ILogger>(new ParameterOverride("callerMethod", MethodBase.GetCurrentMethod()));

        public InstrumentationInterceptor()
        {
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        [DebuggerStepThrough]
        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            //Skip Property Accessors
            if (input.MethodBase.MemberType == MemberTypes.Property)
            {
                return getNext().Invoke(input, getNext);
            }

            //Time Everything else
            Stopwatch sw = new Stopwatch();
            sw.Start();

            IMethodReturn methodReturn = getNext().Invoke(input, getNext);

            sw.Stop();

            if (methodReturn.Exception == null)
			{
				//stop watch logging here
				var msg = string.Format("Instrumentation; timeTracing={0}.{1}, time={2}",
				                        input.MethodBase.DeclaringType, input.MethodBase.Name,
				                        sw.ElapsedMilliseconds);

				Log.Info(msg);
            }
			else if (methodReturn.Exception != null)
			{
				Log.InfoFormat("Instrumentation; timeTracing={0}.{1}, exceptionType={2}, exceptionMessage={3}, time={4}",
					input.MethodBase.DeclaringType, input.MethodBase.Name,
					methodReturn.Exception.GetType().Name,
					methodReturn.Exception.Message,
					sw.ElapsedMilliseconds);
			}
		
        	return methodReturn;
        }

        public bool WillExecute
        {
            get { return true; }
        }
    }
}
