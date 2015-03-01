using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using SharpRaven;

namespace Extentions.WebApi.Sentry
{
    public class SentryApiErrorAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var ravenClient = new RavenClient(GetSentryDns());

            var request = context.ActionContext.Request;

            if (context.Exception == null)
            {
                throw new InvalidOperationException("Exception cannot be null..");
            }

            ravenClient.CaptureException(context.Exception);

            var response = new
            {
                Message = "An internal system error has occured.",
                AddtionalMessage = context.Exception.Message
            };

            context.Response = request.CreateResponse(HttpStatusCode.InternalServerError, response);
        }

        private static string GetSentryDns()
        {
            return ConfigurationManager.AppSettings["sentryDns"];    
        }
    }
}