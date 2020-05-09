using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Middleware
{
    public sealed class DomainMiddleware
    {
        private RequestDelegate _next { get; }

        public DomainMiddleware(RequestDelegate next) => (_next) = (next);

        public async Task InvokeAsync(HttpContext context)
        {
            // Request processing
            
            
            // Continue the request pipeline
            await _next(context);

            // Response processing

        }
    }
}