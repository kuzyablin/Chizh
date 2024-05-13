using Chizh.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Chizh.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainController : ControllerBase
    {
        private User24Context _context;

        public TrainController(User24Context context)
        {
            _context = context;
        }

        [HttpGet] //Вывод тренировок
        public async Task<ActionResult<IEnumerable<TrainDTO>>> GetTrains()
        {
            if (_context.Trains == null)
            {
                return NotFound();
            }
            var trains = await _context.Trains.Include(s => s.IdPozes).ThenInclude(s=>s.IdMuscleNavigation).ToListAsync();
            // mega shit
            var trains2 = trains.Select(s => 
            new TrainDTO { 
                Id = s.Id, 
                TrDescription = s.TrDescription, 
                TrTime = s.TrTime, 
                TrTittle = s.TrTittle, 
                Pozes = s.IdPozes.Select(p=> 
                new PozeDTO { 
                    Id = p.Id, 
                    Description = p.Description, 
                    IdMuscle = p.IdMuscle, 
                    Image = p.Image, 
                    Time = p.Time, 
                    Tittle = p.Tittle }).ToList()});
            return trains2.ToList();
//            return trains.Select(s => new TrainDTO { Id = s.Id, IdMuscle = s.IdMuscle, IdPoze = s.IdPoze, MuTittle = s.IdMuscleNavigation.MuTittle, TrDescription = s.TrDescription, TrTime = s.TrTime, TrTittle = s.TrTittle }).ToList();
        }

        [HttpGet("{id}")] //Вывод трени по айди
        public async Task<ActionResult<Train>> GetTrains(int id)
        {
            if (_context.Trains == null)
            {
                return NotFound();
            }
            var train = await _context.Trains.FindAsync(id);
            if (train == null)
            {
                return NotFound(train);

            }
            return train;
        }

        [HttpPost("AddTrain")] //Добавление трени
        public async void AddTrain(TrainDTO train)
        {
            var t = new Train
            {
                TrTittle = train.TrTittle,
                TrDescription = train.TrDescription,
                TrTime = train.TrTime
            };
            t.IdPozes = train.Pozes.Select(p=>_context.Pozes.Find(p.Id)).ToList();
            _context.Add(t);
            _context.SaveChanges();
        }

        [HttpPut("{id}")] //Редактирование трени
        public async Task<ActionResult<Train>> EditTrain(int id, TrainDTO trainDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingTrain = await _context.Trains.Include(s=>s.IdPozes).FirstOrDefaultAsync(s => s.Id == id);
            if (existingTrain == null)
            {
                return NotFound();
            }

            existingTrain.TrTittle = trainDTO.TrTittle;
            existingTrain.TrDescription = trainDTO.TrDescription;
            existingTrain.TrTime = trainDTO.TrTime;

            existingTrain.IdPozes = trainDTO.Pozes.Select(p => _context.Pozes.Find(p.Id)).ToList();

            _context.Entry(existingTrain).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainExists(id))
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

        [HttpDelete("{id}")] //Удаление трени
        public async Task<IActionResult> DeleteTrain(int id)
        {
            if (_context.Trains == null)
            {
                return NotFound();
            }
            var train = await _context.Trains.FindAsync(id);
            if (train == null)
            {
                return NotFound(train);

            }
            _context.Trains.Remove(train);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool TrainExists(int id)
        {
            return (_context.Trains?.Any(e => e.Id == id)).GetValueOrDefault();


        }
    }
}
