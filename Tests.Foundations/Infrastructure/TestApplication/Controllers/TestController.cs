using Domain.Design.Foundations.Events;
using Microsoft.AspNetCore.Mvc;
using Tests.Foundations.Infrastructure.TestApplication.Events;

namespace Tests.Foundations.Infrastructure.TestApplication.Controllers
{
    [Route("test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private IDomainEventManager _manager { get; }
        private IDomainEventRepository _repository { get; }
        public TestController(IDomainEventManager manager, IDomainEventRepository repository)
        {
            _manager = manager;
            _repository = repository;
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