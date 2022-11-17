using Microsoft.AspNetCore.Mvc;
using Versioning.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Versioning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntitiesController : ControllerBase
    {
        private readonly IEntitiesService _service;

        public EntitiesController(IEntitiesService service)
        {
            _service = service;
        }
        
        [HttpPut]
        public bool Put(Update update)
        {
            return _service.Update(update.OldFile, update.NewFile);
        }

        [HttpPost]
        public int? Post(Entity entity)
        {
            return _service.Add(entity);
        }

        [HttpGet("{id}")]
        public ActionResult<Entity> Get(int id)
        {
            Entity? entity = _service.Get(id);
            return entity == null ? NotFound() : Ok(entity);
        }
    }
}
