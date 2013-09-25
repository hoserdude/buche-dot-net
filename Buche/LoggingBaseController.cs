using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.Unity;

namespace Buche
{
    [LogActionFilter]
    public abstract class LoggingBaseController : Controller
    {
        private static readonly ILogger Log = ContainerLocator.Container.Resolve<ILogger>(new ParameterOverride("callerMethod", System.Reflection.MethodBase.GetCurrentMethod()));

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            InitializeLogContext();
            StringBuilder builder = new StringBuilder();

            // In case of a child action, we shouldn't log the url, ip, etc again.
            if (ControllerContext.IsChildAction)
            {
                // only log the routing info because it will be different
                AppendRoutingInfo(builder, requestContext);
            }
            else
            {
                //InitializeLogContext();

                // log the request here. DO NOT move this to LogActionFilter or any other attribute.
                // LogActionFilter is not executed if any AuthorizationFilter returns a unauthorized result.
                // So if a user is not logged in, LogActionFilter is not called.
                AppendServerInfo(builder, requestContext);
                AppendClientInfo(builder, requestContext);
                AppendRoutingInfo(builder, requestContext);
            }

            Log.Info("RequestInfo; " + builder);
        }

        private void InitializeLogContext()
        {
            Log.ClearContext();
            // Always set a RID, API is stateless and won't have a session.
            var sessionId = (Session != null) ? Session.SessionID : RequestInfo.UninitializedSessionId;
            var requestId = (Request != null) ? Request.GetHashCode().ToString() : RequestInfo.UninitializedRequestId;
            Log.SetRequestId(sessionId, requestId);

            var ipAddress = HttpContext.Request.GetOriginationIp();
            Log.SetProperty(LogContext.PropertyKey.IpAddress, ipAddress);

            var userAgent = HttpContext.Request.UserAgent;
            Log.SetProperty(LogContext.PropertyKey.UserAgent, userAgent);
        }

        private static void AppendClientInfo(StringBuilder builder, RequestContext requestContext)
        {
            if (builder == null || requestContext == null)
            {
                return;
            }

            builder.AppendFormat("method={0}; ", requestContext.HttpContext.Request.HttpMethod);

            if (requestContext.HttpContext.Request.UserLanguages != null && requestContext.HttpContext.Request.UserLanguages.Length > 0)
            {
                builder.AppendFormat("languages={0}; ", string.Join(",", requestContext.HttpContext.Request.UserLanguages));
            }

            if (requestContext.HttpContext.Request.UrlReferrer != null)
            {
                builder.AppendFormat("referrer={0}; ", requestContext.HttpContext.Request.UrlReferrer);
            }

            if (!string.IsNullOrWhiteSpace(requestContext.HttpContext.Request.UserAgent))
            {
                builder.AppendFormat("userAgent={0}; ", requestContext.HttpContext.Request.UserAgent);
            }
        }

        private static void AppendServerInfo(StringBuilder builder, RequestContext requestContext)
        {
            if (builder == null || requestContext == null)
            {
                return;
            }

            builder.AppendFormat("server={0}; url={1};", requestContext.HttpContext.Server.MachineName, requestContext.HttpContext.Request.Url);
        }

        private static void AppendRoutingInfo(StringBuilder builder, RequestContext requestContext)
        {
            if (builder == null || requestContext == null)
            {
                return;
            }

            // Only append route data containing non empty values. There is always data for /controller/action,
            // but anything further depends on the action. So, ignore empty values to only log useful info.
            foreach (var entry in requestContext.RouteData.Values.Where(entry => entry.Value != null && !string.IsNullOrEmpty(entry.Value.ToString())))
            {
                builder.AppendFormat("{0}={1};", entry.Key, entry.Value);
            }
        }

        protected void AddAdditionalDataToLog(string key, object value)
        {
            if (HttpContext.Items == null)
            {
                return;
            }

            var additionalDataToLog = HttpContext.Items[LogActionFilter.AdditionalValuesToLog] as Dictionary<string, object>;
            if (additionalDataToLog == null)
            {
                additionalDataToLog = new Dictionary<string, object>();
                HttpContext.Items.Add(LogActionFilter.AdditionalValuesToLog, additionalDataToLog);
            }
            additionalDataToLog.Add(key, value);
        }

    }
}
