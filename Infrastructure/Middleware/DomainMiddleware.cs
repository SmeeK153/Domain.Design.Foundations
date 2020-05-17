using System.Threading.Tasks;
using Domain.Design.Foundations.Events;
using Microsoft.AspNetCore.Http;

namespace Domain.Design.Foundations.Middleware
{
    public class DomainMiddleware
    {
        private RequestDelegate _next { get; }

        public DomainMiddleware(RequestDelegate next) =>
            (_next) = (next);

        public async Task InvokeAsync(HttpContext context, IDomainEventManager manager)
        {
            // Request pre-processing

            // Continue the request pipeline
            await _next(context);
            
            // Request post-processing
            await manager.ExecuteEvents();
        }
    }
}