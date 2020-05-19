using System;

namespace Domain.Design.Foundations.Events
{
    public class DomainSubscription : IDisposable
    {
        internal DomainSubscription(Action disposal)
        {
            Disposal = disposal;
        }

        private Action Disposal { get; }


        public void Dispose()
        {
            Disposal.Invoke();
            IsDisposed = true;
        }

        public bool IsDisposed { get; private set; } = false;
    }
}