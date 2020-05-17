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
            if (builder.ApplicationServices.GetService<IDomainEventManager>() is null)
            {
                throw new InvalidOperationException(
                    $"{nameof(IDomainEventManager)} must have a provided implementation via " +
                    $"IServiceCollection.AddDomainEvents<T>() if using IApplicationBuilder.UseDomainEvents()");
            }
            
            builder.UseMiddleware<DomainMiddleware>();
            return builder;
        }
    }
}