using Microsoft.AspNetCore.Http;
using Xunit;

namespace Tests.Foundations
{
    public class DomainMiddlewareTests
    {
        [Fact]
        public void DomainMiddlewareExecutesQueuedDomainEvents()
        {
            var context = new DefaultHttpContext();
        }
    }
}