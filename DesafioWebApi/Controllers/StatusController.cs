using DesafioWebApi.Model;
using DesafioWebApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DesafioWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusController : ControllerBase
    {
        private readonly StatusRepository statusRepository;
        public StatusController(IConfiguration configuration)
        {
            statusRepository = new StatusRepository(configuration);
        }

        [HttpGet]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 500)]
        public IActionResult GetAll()
        {
            var statusList = statusRepository.GetAll();
            return Ok(statusList);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 500)]
        public IActionResult Get([FromRoute] int id)
        {
            var status = statusRepository.FindById(id);
            if (status == null)
            {
                return NotFound();
            }
            return Ok(status);
        }

        [HttpPost]
        [ProducesResponseType(statusCode: 201)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 500)]
        public IActionResult Create([FromBody]Status status)
        {
            if (ModelState.IsValid)
            {
                var result = statusRepository.Create(status);
                var lastResult = result ? statusRepository.GetLastInserted() : null;
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
            var status = statusRepository.FindById(id);
            if (status == null)
            {
                return NotFound();
            }
            statusRepository.Delete(id);
            return NoContent();
        }

        [HttpPut]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 401)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 500)]
        public IActionResult Update([Bind("Id,Name")]Status status)
        {
            if (ModelState.IsValid)
            {
                statusRepository.Update(status);
                return Ok();
            }
            return BadRequest();
        }
        
    }
}
