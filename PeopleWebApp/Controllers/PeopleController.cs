using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PeopleWebApp.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace People.WebApp.Controllers
{
    public class PeopleController : Controller
    {
        private readonly PeopleContext _context;
        private ILogger<PeopleController> _logger;

        public PeopleController(PeopleContext context, ILogger<PeopleController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: PeopleDbMvc
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("[PeopleController] [Index]");

            return View(await _context.Persons.ToListAsync());
        }

        // GET: PeopleDbMvc/Details/5
        public async Task<IActionResult> Details(string id)
        {
            _logger.LogInformation($"[PeopleController] [Details: {id}]");

            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Persons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: PeopleDbMvc/Create
        public IActionResult Create()
        {
            _logger.LogInformation("[PeopleController] [Create]");

            return View();
        }

        // POST: PeopleDbMvc/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,BirthDate")] Person person)
        {
            _logger.LogInformation("[PeopleController] [Create]");

            if (ModelState.IsValid)
            {
                if(string.IsNullOrEmpty(person.Id))
                {
                    person.Id = Guid.NewGuid().ToString();
                }

                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        // GET: PeopleDbMvc/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            _logger.LogInformation($"[PeopleController] [Edit: {id}]");

            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Persons.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }

        // POST: PeopleDbMvc/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FirstName,LastName,BirthDate")] Person person)
        {
            _logger.LogInformation($"[PeopleController] [Edit: {id}]");

            if (id != person.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(person);
        }

        // GET: PeopleDbMvc/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            _logger.LogInformation($"[PeopleController] [Delete: {id}]");

            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Persons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: PeopleDbMvc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            _logger.LogInformation($"[PeopleController] [DeleteConfirmed: {id}]");

            var person = await _context.Persons.FindAsync(id);
            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(string id)
        {
            _logger.LogInformation($"[PeopleController] [PersonExists: {id}]");

            return _context.Persons.Any(e => e.Id == id);
        }
    }
}
