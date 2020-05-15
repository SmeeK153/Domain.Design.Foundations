using System.Collections.Generic;
using Domain.Design.Foundations.Events;
using Domain.Design.Foundations.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Design.Foundations.Extensions
{
    public static partial class DomainExtensions
    {
        public static IServiceCollection AddDomainEvents(this IServiceCollection services)
        {
            // Add a new queue of domain events for each new request
            services.AddScoped<IEnumerable<DomainEvent>, DomainEventQueue>();
            
            return services;
        }

        public static IApplicationBuilder UseDomainEvents(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<DomainMiddleware>();
            return builder;
        }
    }
}