using System;
using Domain.Design.Foundations.Events;
using Domain.Design.Foundations.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Design.Foundations.Extensions
{
    public static partial class DomainExtensions
    {
        public static IServiceCollection AddDomainEvents<TEventManager>(this IServiceCollection services) where TEventManager : IDomainEventManager
        {
            // Add the domain event manager implementation
            services.AddScoped(typeof(IDomainEventManager), typeof(TEventManager));
            return services;
        }

        public static IApplicationBuilder UseDomainEvents(this IApplicationBuilder builder)
        {
            try
            {
                var service = builder.ApplicationServices.GetRequiredService<IDomainEventManager>();
                builder.UseMiddleware<DomainMiddleware>();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("IDomainEventManager must have a provided implementation via IServiceCollection.AddDomainEvents<T>() if using IApplicationBuilder.UseDomainEvents()");
            }
            return builder;
        }
    }
}