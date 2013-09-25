using System;
using System.Web.Mvc;
using Microsoft.Practices.Unity;

namespace Buche
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class LogOperationAttribute : ActionFilterAttribute
    {
        private static readonly ILogger Log = ContainerLocator.Container.Resolve<ILogger>(new ParameterOverride("callerMethod", System.Reflection.MethodBase.GetCurrentMethod()));

        private readonly string _operation;
        private LogOperation _logOperation;

        public LogOperationAttribute(string operation)
        {
            _operation = operation;
            Order = 99;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _logOperation = Log.CreateOperation(_operation);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            _logOperation.Dispose();
        }
    }
}
