using System.Configuration;
using Microsoft.AspNet.SignalR.Hubs;
using SharpRaven;

namespace Extentions.SignalR.Sentry
{
    public class SentryErrorHandlingPipelineModule : HubPipelineModule
    {
        protected override void OnIncomingError(ExceptionContext exceptionContext, IHubIncomingInvokerContext invokerContext)
        {
            var ravenClient = new RavenClient(GetSentryDns());

            ravenClient.CaptureException(exceptionContext.Error);

            base.OnIncomingError(exceptionContext, invokerContext);
        }

        private static string GetSentryDns()
        {
            return ConfigurationManager.AppSettings["sentryDns"];
        }
    }
}
