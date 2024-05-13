using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chizh.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MuscleController : ControllerBase
    {
        private User24Context _context;

        public MuscleController(User24Context context)
        {
            _context = context;
        }

        [HttpGet("Muscle")] //Вывод мускул
        public async Task<ActionResult<IEnumerable<Muscle>>> GetMuscle()
        {
            if (_context.Muscles == null)
            {
                return NotFound();
            }
            return await _context.Muscles.ToListAsync();
        }

        [HttpGet("{id}")] //Вывод мускул по айдишнику
        public async Task<ActionResult<Muscle>> GetMuscle(int id)
        {
            if (_context.Muscles == null)
            {
                return NotFound();
            }
            var muscle = await _context.Muscles.FindAsync(id);
            if (muscle == null)
            {
                return NotFound(muscle);

            }
            return muscle;
        }
    }
}
