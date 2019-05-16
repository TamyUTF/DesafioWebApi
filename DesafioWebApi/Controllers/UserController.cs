using DesafioWebApi.Model;
using DesafioWebApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DesafioWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserRepository userRepository;
        public UserController(IConfiguration configuration)
        {
            userRepository = new UserRepository(configuration);
        }

        [HttpGet]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 500)]
        public IActionResult GetAll()
        {
            var userList = userRepository.GetAll();
            return Ok(userList);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 500)]
        public IActionResult Get([FromRoute] int id)
        {
            var user = userRepository.FindById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        [ProducesResponseType(statusCode: 201)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 500)]
        public IActionResult Create([FromBody]User user)
        {
            if (ModelState.IsValid)
            {
                var result = userRepository.Create(user);
                var lastResult = result ? userRepository.GetLastInserted() : null;
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
            var user = userRepository.FindById(id);
            if (user == null)
            {
                return NotFound();
            }
            userRepository.Delete(id);
            return NoContent();
        }

        [HttpPut]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 400)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 500)]
        public IActionResult Update([Bind("Id,Name")]User user)
        {
            if (ModelState.IsValid)
            {
                userRepository.Update(user);
                return Ok();
            }
            return BadRequest();
        }


    }
}
