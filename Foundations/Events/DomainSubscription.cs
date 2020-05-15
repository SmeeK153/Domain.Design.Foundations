using System;

namespace Domain.Design.Foundations.Events
{
    internal class DomainSubscription : IDisposable
    {
        internal DomainSubscription(Action disposal)
        {
            Disposal = disposal;
        }

        private Action Disposal { get; }


        public void Dispose() => Disposal.Invoke();
    }
}