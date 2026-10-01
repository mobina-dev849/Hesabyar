using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using Service.Interfaces;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var persons = await _personService.GetAll(ct);
            return Ok(persons);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id, CancellationToken ct)
        {
            var person = await _personService.GetById(id, ct);

            return Ok(person);
        }

        [HttpPost]
        public async Task<IActionResult> Add(
            CreatePersonDto dto,
            CancellationToken ct)
        {
            await _personService.Add(dto, ct);

            return Ok();
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(
            long id,
            PersonDto dto,
            CancellationToken ct)
        {
            if (id != dto.Id)
                return BadRequest("شناسه شخص صحیح نیست.");

            await _personService.Update(dto, ct);

            return Ok();
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(
            long id,
            CancellationToken ct)
        {
            await _personService.Delete(id, ct);

            return Ok();
        }
    }
}