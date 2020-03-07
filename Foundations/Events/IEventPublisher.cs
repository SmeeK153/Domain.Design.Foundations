using System;
using System.Threading.Tasks;

namespace Foundations.Events
{
    public interface IEventPublisher
    {
        Task Notify(Notification notification);

        Task Request<TResponse>(Request<TResponse> request);
    }
}
