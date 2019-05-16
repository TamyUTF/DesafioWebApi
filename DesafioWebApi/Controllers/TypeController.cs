using DesafioWebApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using DesafioWebApi.Model;

namespace DesafioWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TypeController : ControllerBase
    {
        private readonly TypeRepository typeRepository;
        public TypeController(IConfiguration configuration)
        {
            typeRepository = new TypeRepository(configuration);
        }

        [HttpGet]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 500)]
        public IActionResult GetAll()
        {
            var typeList = typeRepository.GetAll();
            return Ok(typeList);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 500)]
        public IActionResult Get([FromRoute] int id)
        {
            var type = typeRepository.FindById(id);
            if(type == null)
            {
                return NotFound();
            }
            return Ok(type);
        }

        [HttpPost]
        [ProducesResponseType(statusCode: 201)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 500)]
        public IActionResult Create([FromBody]TypePlan type)
        {
            if (ModelState.IsValid)
            {
                var result = typeRepository.Create(type);
                var lastResult = result ? typeRepository.GetLastInserted() : null;
                var uri = Url.Action("Get", new { id = lastResult.Id });
                return Created(uri, lastResult);
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 500)]
        public IActionResult Delete([FromRoute]int id)
        {
            var type = typeRepository.FindById(id);
            if(type == null)
            {
                return NotFound();
            }
            typeRepository.Delete(id);
            return NoContent();
        }

        [HttpPut]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 401)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 500)]
        public IActionResult Update([Bind("Id,Name")]TypePlan type)
        {
            if (ModelState.IsValid)
            {
                typeRepository.Update(type);
                return Ok();
            }
            return BadRequest();
        }
    }
}
