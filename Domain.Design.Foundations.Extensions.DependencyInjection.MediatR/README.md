## Domain.Design.Foundations.Extensions.DependencyInjection.MediatR

The MediatR dependency injection project is designed to further supplement the
Foundations project by providing [MediatR](https://github.com/jbogard/MediatR) specific implementations defined
in the `Domain.Design.Foundations.Extensions.DependencyInjection` project.

### Usage
This project provides a MediatR specific domain event manager and augments the
available extension methods for registering domain events into the .NET Core framework.

#### Configuration
The `Configure` setup does not change, but the `ConfigureServices` setup is simplified
because the implementation does no longer need to be specified as it is using the
MediatR specific implementation of the domain event manager. Another change is the each
of the applicable assemblies that could create MediatR-related events must also be registered
since the MediatR registration is handled within this setup call as well.

```csharp

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDomainEvents(Assembly.GetExecutingAssembly(), ...);
    }

    public void Configure(IApplicationBuilder app, IWebhostEnvironment env)
    {
        app.UseDomainEvents();
    }

```