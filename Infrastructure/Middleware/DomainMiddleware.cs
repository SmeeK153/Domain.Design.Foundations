using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Design.Foundations.Events;
using Microsoft.AspNetCore.Http;

namespace Domain.Design.Foundations.Middleware
{
    public sealed class DomainMiddleware
    {
        private IEnumerable<DomainEvent> _queuedDomainEvents { get; }
        private RequestDelegate _next { get; }

        public DomainMiddleware(RequestDelegate next, IEnumerable<DomainEvent> queuedDomainEvents) =>
            (_next, _queuedDomainEvents) = (next, queuedDomainEvents);

        public async Task InvokeAsync(HttpContext context)
        {
            // Request pre-processing

            // Continue the request pipeline
            await _next(context);
            
            // Request post-processing
            
            // Execute each of the domain events now that the request is returning
        }
    }
}