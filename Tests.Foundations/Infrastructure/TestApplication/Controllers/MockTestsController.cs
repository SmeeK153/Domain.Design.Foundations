using Domain.Design.Foundations.Events;
using Microsoft.AspNetCore.Mvc;

namespace Tests.Foundations.Infrastructure.TestApplication.Controllers
{
    [Route("mock/test")]
    [ApiController]
    public class MockTestsController : ControllerBase
    {
        private IDomainEventManager _manager { get; }
        public MockTestsController(IDomainEventManager manager)
        {
            _manager = manager;
        }
        
        [HttpGet("success")]
        public ActionResult SuccessfulResult()
        {
            var entity = new TestEntity();
            _manager.StartListening(entity, EObserverBehavior.Deferred);
            entity.PublishTestEvent();
            entity.PublishTestEvent();
            entity.PublishTestEvent();
            
            return Ok();
        }
        
        [HttpGet("fail")]
        public ActionResult NonSuccessfulResult()
        {
            var entity = new TestEntity();
            _manager.StartListening(entity, EObserverBehavior.Deferred);
            entity.PublishTestEvent();
            entity.PublishTestEvent();
            entity.PublishTestEvent();
            entity.RaiseTestException();

            return Ok();
        }
    }
}