using System.Collections.Generic;
using System.Reflection;
using Domain.Design.Foundations.Events;
using Domain.Design.Foundations.Middleware;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Design.Foundations.Extensions
{
    /// <summary>
    /// Registers the specified <see cref="MediatRDomainEventManager"/> with the request pipeline and configures the
    /// <see cref="DomainMiddleware"/> to manage <see cref="DomainEvent"/>s during the request lifecycle.
    /// </summary>
    public static partial class MediatRDomainExtensions
    {
        /// <summary>
        /// Registers the specific <see cref="MediatRDomainEventManager"/> as the event manager for all requests.
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="assemblies"><see cref="Assembly"/> references to send to MediatR for registering any pertinent
        /// implementations for defining events or handling events</param>
        /// <returns></returns>
        public static IServiceCollection AddMediatRDomainEvents(
            this IServiceCollection services,
            params Assembly[] assemblies
        )
        {
            // Add the domain event manager implementation
            services.AddDomainEvents<MediatRDomainEventManager>();

            // Register all applicable assemblies to handle generated events
            var registeredAssemblies = new List<Assembly>(assemblies);
            registeredAssemblies.Add(Assembly.GetAssembly(typeof(MediatRDomainExtensions)));
            services.AddMediatR(registeredAssemblies.ToArray());

            return services;
        }
    }
}