using System.Collections.Generic;

namespace Domain.Design.Foundations.Events
{
    public sealed class DomainEventQueue : Queue<DomainEvent>
    {
    }
}