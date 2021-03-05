namespace Domain.Design.Foundations.Events
{
    /// <summary>
    /// <see cref="DomainEvent"/> processing strategy after being published by the respective <see cref="Entity{T}"/>.
    /// </summary>
    public enum EObserverBehavior
    {
        /// <summary>
        /// <see cref="DomainEvent"/>s are executed immediately upon receipt. This is useful for atomic changes that
        /// do not require any additional complexity for managing synchronization.
        /// </summary>
        Immediate,

        /// <summary>
        /// <see cref="DomainEvent"/>s are queued, in order, until <see cref="IDomainEventManager.ExecuteEvents()"/>
        /// is called. This is useful for ensuring that all events are handled at the same time and in a more
        /// predictable manner.
        /// </summary>
        Deferred,
    }
}