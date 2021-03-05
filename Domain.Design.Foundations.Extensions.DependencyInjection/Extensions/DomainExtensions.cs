using Domain.Design.Foundations.Events;
using Domain.Design.Foundations.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Design.Foundations.Extensions
{
    /// <summary>
    /// Registers the specified <see cref="IDomainEventManager"/> with the request pipeline and configures the
    /// <see cref="DomainMiddleware"/> to manage <see cref="DomainEvent"/>s during the request lifecycle.
    /// </summary>
    public static partial class DomainExtensions
    {
        /// <summary>
        /// Registers the specific <see cref="IDomainEventManager"/> as the event manager for all requests.
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <typeparam name="TEventManager"><see cref="IDomainEventManager"/> implementation to use for managing the
        /// lifecycle of <see cref="DomainEvent"/>s</typeparam>
        /// <returns></returns>
        public static IServiceCollection AddDomainEvents<TEventManager>(this IServiceCollection services) where TEventManager : IDomainEventManager
        {
            // Add the domain event manager implementation
            services.AddScoped(typeof(IDomainEventManager), typeof(TEventManager));
            return services;
        }

        /// <summary>
        /// Configures the request pipeline with domain events.
        /// </summary>
        /// <param name="builder"><see cref="IApplicationBuilder"/> instance to configure the request pipeline</param>
        /// <returns></returns>
        public static IApplicationBuilder UseDomainEvents(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<DomainMiddleware>();
            return builder;
        }
    }
}