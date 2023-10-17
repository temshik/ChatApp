using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;

namespace ChatApp.Bll.Exceptions
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        string _environmentName;
        public ExceptionHandlerMiddleware(RequestDelegate next, string environmentName)
        {
            _next = next;
            _environmentName = environmentName;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionMessageAsync(context, ex, _environmentName).ConfigureAwait(false);
            }
        }

        private static Task HandleExceptionMessageAsync(HttpContext context, Exception exception, string environmentName)
        {
            HttpStatusCode status;
            string message;
            var stackTrace = String.Empty;

            var exceptionType = exception.GetType();
            if (exceptionType == typeof(AlreadyExistException))
            {
                message = exception.Message;
                status = HttpStatusCode.Conflict;
            }
            else if (exceptionType == typeof(NotFoundException))
            {
                message = exception.Message;
                status = HttpStatusCode.NotFound;
            }
            else
            {
                status = HttpStatusCode.InternalServerError;
                message = exception.Message;
                if (environmentName == "Development")
                    stackTrace = exception.StackTrace;
                else
                    stackTrace = String.Empty;
            }

            var result = JsonConvert.SerializeObject(new
            {
                status = (int)status,
                errors = message,
                stackTrace = stackTrace

            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            return context.Response.WriteAsync(result);
        }
    }
}
