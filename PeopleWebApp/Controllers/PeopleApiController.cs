using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PeopleWebApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeopleWebApp.Controllers
{
    [Route("api/people")]
    [ApiController]
    public class PeopleApiController : ControllerBase
    {
        private readonly PeopleContext _context;
        private ILogger<PeopleApiController> _logger;

        public PeopleApiController(PeopleContext context, ILogger<PeopleApiController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/PeopleDb
        [HttpGet]
        public IEnumerable<Person> GetPersons()
        {
            _logger.LogInformation("[PeopleApiController] [GetPersons]");

            return _context.Persons;
        }

        // GET: api/PeopleDb/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPerson([FromRoute] string id)
        {
            _logger.LogInformation($"[PeopleApiController] [GetPerson: {id}]");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var person = await _context.Persons.FindAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            return Ok(person);
        }

        // PUT: api/PeopleDb/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson([FromRoute] string id, [FromBody] Person person)
        {
            _logger.LogInformation($"[PeopleApiController] [PutPerson: {id}]");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != person.Id)
            {
                return BadRequest();
            }

            _context.Entry(person).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PeopleDb
        [HttpPost]
        public async Task<IActionResult> PostPerson([FromBody] Person person)
        {
            _logger.LogInformation($"[PeopleApiController] [PostPerson: {person.Id}]");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (string.IsNullOrEmpty(person.Id))
            {
                person.Id = Guid.NewGuid().ToString();
            }

            _context.Persons.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPerson", new { id = person.Id }, person);
        }

        // DELETE: api/PeopleDb/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson([FromRoute] string id)
        {
            _logger.LogInformation($"[PeopleApiController] [DeletePerson: {id}]");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var person = await _context.Persons.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();

            return Ok(person);
        }

        private bool PersonExists(string id)
        {
            _logger.LogInformation($"[PeopleApiController] [PersonExists: {id}]");

            return _context.Persons.Any(e => e.Id == id);
        }
    }
}
