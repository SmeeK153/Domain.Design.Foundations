using System;
using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Tests.Foundations.Infrastructure.TestApplication
{
    public class ExceptionFilter : IExceptionFilter
    {
        private ILogger<ExceptionFilter> _logger { get; }

        public ExceptionFilter(ILogger<ExceptionFilter> logger) => (_logger) = (logger);
        
        public void OnException(ExceptionContext context)
        {
            var response = context.HttpContext.Response;
            var reference = Guid.NewGuid().ToString();
            response.StatusCode = (int) HttpStatusCode.InternalServerError;
            response.ContentType = MediaTypeNames.Text.Plain;
            response.WriteAsync(
                $"An unexpected error has occurred. You can use the following reference id to help us diagnose your problem: {reference}");
            _logger.LogError(context.Exception, $"Unhandled exception of type '{context.Exception.GetType().ToString()}' encountered (Reference: {reference})");
        }
    }
}