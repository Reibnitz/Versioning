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
        public Response Put(Update update)
        {
            try
            {
                bool result = _service.Update(update.OldFile, update.NewFile);

                return new Response { Success = result, Message = "Ok" };
            }
            catch (Exception ex)
            {
                return new Response { Success = false, Message = ex.Message };                
            }
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
