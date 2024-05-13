using Chizh.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chizh.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CheckController: ControllerBase
    {
        private User24Context _context;

        public CheckController(User24Context context)
        {
            _context = context;
        }

        [HttpGet("Check")] //Вывод чеков
        public async Task<ActionResult<IEnumerable<Check>>> GetCheck()
        {
            if (_context.Checks == null)
            {
                return NotFound();
            }
            return await _context.Checks.ToListAsync();
        }

        [HttpGet("{id}")] //Вывод чека по айдишнику
        public async Task<ActionResult<Check>> GetCheck(int id)
        {
            if (_context.Checks == null)
            {
                return NotFound();
            }
            var check = await _context.Checks.FindAsync(id);
            if (check == null)
            {
                return NotFound(check);

            }
            return check;
        }

        [HttpPost("AddCheck")] //Добавление чека
        public async void AddCheck(CheckDTO check)
        {
            _context.Add(new Check
            {
                Date = check.Date,
                Weight1 = check.Weight1,
                IdUser = check.IdUser
            });
            _context.SaveChanges();
        }


    }
}