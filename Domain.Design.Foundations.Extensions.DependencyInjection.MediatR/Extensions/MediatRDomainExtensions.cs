using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Design.Foundations.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class MediatRDomainExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IServiceCollection AddMediatRDomainEvents(this IServiceCollection services, params Assembly[] assemblies)
        {
            // Add the domain event manager implementation
            services.AddDomainEvents<MediatRDomainEventManager>();

            // Register all applicable assemblies to handle generated events
            services.AddMediatR(assemblies);

            return services;
        }
    }
}