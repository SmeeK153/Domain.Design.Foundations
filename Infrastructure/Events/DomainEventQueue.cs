using System.Collections.Generic;
using Domain.Design.Foundations.Events;

namespace Infrastructure.Events
{
    internal sealed class DomainEventQueue : Queue<DomainEvent>
    {
    }
}