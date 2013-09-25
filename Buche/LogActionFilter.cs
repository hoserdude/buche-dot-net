using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;

namespace Buche
{

	/// <summary>
	/// LogActionFilter - added as a development tool and debugger.  
	/// Add as [LogActionFilter] to controller class.
	/// </summary>
	public class LogActionFilter : ActionFilterAttribute
	{
		public const string AdditionalValuesToLog = "AdditionalValuesToLog";
        public const string AdditionalParamValuesToLog = "AdditionalParamValuesToLog";
        public const string LogRequestModel = "LogRequestModel";

	    private static readonly ILogger Log =
			ContainerLocator.Container.Resolve<ILogger>(new ParameterOverride("callerMethod",
			                                                                  System.Reflection.MethodBase.GetCurrentMethod()));

		private Stopwatch _stopwatch;

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			_stopwatch = new Stopwatch();
			_stopwatch.Start();
            AppendActionParametersInfo(filterContext);
		}

	    public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
		}

		public override void OnResultExecuting(ResultExecutingContext filterContext)
		{
		}

		public override void OnResultExecuted(ResultExecutedContext filterContext)
		{
			if (_stopwatch == null) // can be null if we intercept a request in BaseWebController:OnActionExecuting
				return;

			_stopwatch.Stop();
			HttpResponseBase response = filterContext.HttpContext.Response;

		    var builder = new StringBuilder();

			if (filterContext.RouteData != null)
                builder.AppendFormat("Instrumentation; timeTracing={0}; time={1}",
				                     filterContext.RouteData.Values["controller"] + "." + filterContext.RouteData.Values["action"],_stopwatch.ElapsedMilliseconds);

			if (response.IsRequestBeingRedirected)
			{
				builder.AppendFormat("; redirect={0}", response.RedirectLocation);
			}

		    AppendAdditionalInfoToLog(builder, filterContext);

			Log.Info(builder.ToString());
		}

		private static void AppendActionParametersInfo(ActionExecutingContext filterContext)
		{
		    var builder = new StringBuilder();
			if (filterContext == null) return;

            foreach (var parameter in filterContext.ActionParameters)
            {
                if(parameter.Value==null)
                    continue;
                object value = null;
                if (parameter.Value.GetType().IsPrimitiveExtended())
                {
                    value = LogUtil.IsSensitiveKey(parameter.Key) ? "********" : parameter.Value;
                    if (value != null && !string.IsNullOrEmpty(value.ToString()))
                        builder.AppendFormat("param_{0}={1}; ", parameter.Key, value);
                }
                else
                {
                    var attributes = parameter.Value.GetType().GetCustomAttributes(true);
                    if (attributes.Any(x => x is Loggable))
                        value = ObjectDumper.Log(parameter.Value);
                    if (value != null && !string.IsNullOrEmpty(value.ToString()))
                        builder.Append(value);
                }
                
            }

            if(builder.Length>0)
            {
                builder = new StringBuilder(Regex.Replace(builder.ToString(), @";\s*$", ""));

                if (filterContext.HttpContext.Items.Contains(AdditionalParamValuesToLog))
                {
                    ((StringBuilder)filterContext.HttpContext.Items[AdditionalParamValuesToLog]).Append(builder);
                }
                else
                {
                    filterContext.HttpContext.Items.Add(AdditionalParamValuesToLog, builder);
                }
            }            
		}

        
		private static void AppendAdditionalInfoToLog(StringBuilder builder, ControllerContext filterContext)
		{
			if (builder == null || filterContext == null) return;

			if (filterContext.HttpContext.Items.Contains(AdditionalValuesToLog))
			{
				var additionalDataToLog = filterContext.HttpContext.Items[AdditionalValuesToLog] as Dictionary<string, object>;
				if (additionalDataToLog != null)
				{
                    builder.AppendFormat("; {0}", String.Join("; ", additionalDataToLog.Select(o => o.Key + "=" + o.Value).ToArray()));
                }
					
			}

            if (filterContext.HttpContext.Items.Contains(AdditionalParamValuesToLog))
            {
                builder.AppendFormat("; {0}",filterContext.HttpContext.Items[AdditionalParamValuesToLog] as StringBuilder);
            }
		}
	}
}