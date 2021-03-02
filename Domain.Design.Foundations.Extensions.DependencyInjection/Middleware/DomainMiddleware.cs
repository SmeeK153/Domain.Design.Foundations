using System.Threading.Tasks;
using Domain.Design.Foundations.Events;
using Microsoft.AspNetCore.Http;

namespace Domain.Design.Foundations.Middleware
{
    /// <summary>
    /// Manages the <see cref="IDomainEventManager"/> lifecycle during the course of a request and response lifecycle.
    /// </summary>
    public class DomainMiddleware
    {
        /// <summary>
        /// Creates a new instance of the middleware that controls the <see cref="IDomainEventManager"/> lifecycle.
        /// </summary>
        /// <param name="next"></param>
        public DomainMiddleware(RequestDelegate next) => (_next) = (next);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="manager"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, IDomainEventManager manager)
        {
            #region Request Pre-Processing

            #endregion

            #region Request Processing

            // Continue the request pipeline so that any batched/deferred transactions may be queued
            await _next(context);

            #endregion

            #region Request Post-Processing

            // Dispose the manager in order to process any queued batched/deferred transactions
            manager.Dispose();

            #endregion
        }
        
        /// <summary>
        /// The next, consecutive step in the request processing pipeline, if any.
        /// </summary>
        private RequestDelegate _next { get; }
    }
}