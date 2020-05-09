using System;
using System.Collections.Generic;
using Domain.Design.Foundations.Events;
using Infrastructure.Events;
using Infrastructure.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static partial class DomainExtensions
    {
        public static IServiceCollection AddDomainEvents(this IServiceCollection services)
        {
            // Add a new queue of domain events for each new request
            services.AddScoped<IEnumerable<DomainEvent>, DomainEventQueue>();
            
            // Add a new publisher for each new request
            services.AddScoped<IObservable<DomainEvent>, DomainEventPublisher>();
            
            return services;
        }

        public static IApplicationBuilder UseDomainEvents(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<DomainMiddleware>();
            return builder;
        }
    }
}