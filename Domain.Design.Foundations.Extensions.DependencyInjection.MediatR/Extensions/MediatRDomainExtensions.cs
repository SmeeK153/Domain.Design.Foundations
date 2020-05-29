using System.Reflection;
using Domain.Design.Foundations.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Design.Foundations.Extensions
{
    public static partial class MediatRDomainExtensions
    {
        public static IServiceCollection AddDomainEvents(this IServiceCollection services, params Assembly[] assemblies)
        {
            // Add the domain event manager implementation
            services.AddScoped<IDomainEventManager, MediatRDomainEventManager>();

            // Register all applicable assemblies to handle generated events
            services.AddMediatR(assemblies);

            return services;
        }
    }
}