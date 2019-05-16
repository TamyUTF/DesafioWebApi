using DesafioWebApi.Filters;
using DesafioWebApi.Model;
using DesafioWebApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Swagger;

namespace DesafioWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlannerController : ControllerBase
    {
        private readonly PlannerRepository plannerRepository;
        public PlannerController(IConfiguration configuration)
        {
            plannerRepository = new PlannerRepository(configuration);
        }

        [HttpGet]
        [ProducesResponseType(statusCode:200, Type = typeof(Planner))]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 500)]
        public IActionResult GetAll([FromQuery]PlannerFilter search)
        {
            var plannerList = plannerRepository.GetAll().Filter(search);
            return Ok(plannerList);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 500)]
        public IActionResult Get([FromRoute]int id)
        {
            var planner = plannerRepository.FindById(id);
            if (planner == null)
            {
                return NotFound();
            }
            return Ok(planner);
        }
        [HttpPost]
        [ProducesResponseType(statusCode: 201)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 500)]
        public IActionResult Create([FromBody]Planner planner)
        {
            if (ModelState.IsValid)
            {
                var result = plannerRepository.Create(planner);
                var lastResult = result ? plannerRepository.GetLastInserted() : null;
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
            var planner = plannerRepository.FindById(id);
            if (planner == null)
            {
                return NotFound();
            }
            plannerRepository.Delete(id);
            return Ok();
        }

        [HttpPut]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 400)]
        [ProducesResponseType(statusCode: 404)]
        [ProducesResponseType(statusCode: 500)]
        public IActionResult Update([FromBody]Planner planner)
        {
            if (ModelState.IsValid)
            {
                plannerRepository.Update(planner);
                return Ok();
            }
            return BadRequest();
        }

    }
}
