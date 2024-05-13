using Chizh.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chizh.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PozeController : ControllerBase
    {
        private User24Context _context;

        public PozeController(User24Context context)
        {
            _context = context;
        }

        [HttpGet("Poze")] //Вывод поз
        public async Task<ActionResult<IEnumerable<Poze>>> GetPoze()
        {
            if (_context.Pozes == null)
            {
                return NotFound();
            }
            return await _context.Pozes.ToListAsync();
        }

        [HttpGet("{id}")] //Вывод позы по айдишнику
        public async Task<ActionResult<Poze>> GetPoze(int id)
        {
            if (_context.Pozes == null)
            {
                return NotFound();
            }
            var poze = await _context.Pozes.FindAsync(id);
            if (poze == null)
            {
                return NotFound(poze);

            }
            return poze;
        }

        [HttpGet("GetPozeByMuscle")]
        public async Task<ActionResult<List<PozeDTO>>> GetPoze1(int muscle)
        {
            var h = await _context.Pozes.Include(s => s.IdMuscleNavigation).Where(s => s.IdMuscle == muscle).OrderBy(s => s.Id).ToListAsync();

            var hz = h.Select(s => new PozeDTO
            {
                Muscle = s.IdMuscleNavigation.MuTittle,
                IdMuscle = s.IdMuscle,
                Tittle = s.Tittle,
                Description = s.Description,
                Image = s.Image,
                Time = s.Time,
                Id = s.Id,

            });
            return hz.ToList();
        }

        [HttpPost("AddPoze")] //Добавление позы
        public async void AddPoze(PozeDTO poze)
        {
            _context.Add(new Poze
            {
                Tittle = poze.Tittle,
                Description = poze.Description,
                Time = poze.Time,
                Image = poze.Image,
                IdMuscle = poze.IdMuscle
            });
            _context.SaveChanges();
        }

        [HttpPut("{id}")] //Редактирование позы
        public async Task<ActionResult<Poze>> EditPoze(int id, PozeDTO pozeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingPoze = await _context.Pozes.FirstOrDefaultAsync(s => s.Id == id);
            if (existingPoze == null)
            {
                return NotFound();
            }

            existingPoze.Tittle = pozeDTO.Tittle;
            existingPoze.Description = pozeDTO.Description;
            existingPoze.Time = pozeDTO.Time;
            existingPoze.Image = pozeDTO.Image;
            existingPoze.IdMuscle = pozeDTO.IdMuscle;

            _context.Entry(existingPoze).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PozeExists(id))
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

        [HttpDelete("{id}")] //Удаление позы
        public async Task<IActionResult> DeletePoze(int id)
        {
            if (_context.Pozes == null)
            {
                return NotFound();
            }
            var poze = await _context.Pozes.FindAsync(id);
            if (poze == null)
            {
                return NotFound(poze);

            }
            _context.Pozes.Remove(poze);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool PozeExists(int id)
        {
            return (_context.Pozes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
