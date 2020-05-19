## Domain.Design.Foundations.Extensions.DependencyInjection

The dependency injection project is designed to supplement the
Foundations project to provide the ability to handle events
in the .NET Core framework. The project exposes a simple
middleware component that caches domain effects to ensure
none are executed until the completion of the request to ensure
that in the case of an error, none of the events can corrupt
down-stream systems (i.e. database, cache, other services, etc.)

### Usage
In order to use the middleware, a domain event manager must be provided
and added to the .NET Core DI container within `ConfigureServices`.

#### Manager
```csharp
    using System.Threading.Tasks;
    using Domain.Design.Foundations.Events;
    
    namespace Domain.Design.Foundations
    {
        public class MyDomainEventManager : DomainEventObserverManager
        {
            public MyDomainEventManager()
            {
            }
            
            protected override async Task ExecuteEvent(DomainEvent domainEvent)
            {
                ... send event to subsequent system(s)
            }
        }
    }
```

#### Configuration
```csharp

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDomainEvents<MyDomainEventManager>();
    }

    public void Configure(IApplicationBuilder app, IWebhostEnvironment env)
    {
        app.UseDomainEvents();
    }

```