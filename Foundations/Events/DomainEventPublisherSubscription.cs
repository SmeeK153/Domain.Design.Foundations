using System;

namespace Domain.Design.Foundations.Events
{
    internal class DomainEventPublisherSubscription : IDisposable
    {
        internal DomainEventPublisherSubscription(Action disposal)
        {
            Disposal = disposal;
        }

        private Action Disposal { get; }


        public void Dispose() => Disposal.Invoke();
    }
}