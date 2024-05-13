using Chizh.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Chizh.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private User24Context _context;

        public UserController(User24Context context)
        {
            _context = context;
        }

        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<List<UserDTO>>> GetUsers()
        {
            List<UserDTO> users = _context.Users.ToList().Select(s => new UserDTO
            {
                Id = s.Id,
                Name = s.Name,
                Password = s.Password,
                Weight = s.Weight,
                Height = s.Height
            }).ToList();
            return users;
        }

        [HttpGet("GetUser")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var s = _context.Users.FirstOrDefault(s => s.Id == id);
            if (s == null)
            {
                return NotFound();

            }
            return Ok(new UserDTO
            {
                Id = s.Id,
                Name = s.Name,
                Password = s.Password,
                Weight = s.Weight,
                Height = s.Height
            });
        }

        [HttpPost("Register")]
        public async void GetRegister(UserDTO user)
        {
            var u = new User
            {
                Name = user.Name,
                Password = user.Password,
                Weight = user.Weight, 
                Height = user.Height
            };
            _context.Add(u);
            _context.SaveChanges();
        }

        [HttpPut("{id}")] //Редактирование юзера
        public async Task<ActionResult<User>> EditUser(int id, UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(s => s.Id == id);
            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.Name = userDTO.Name;
            existingUser.Password = userDTO.Password;
            existingUser.Weight = userDTO.Weight;
            existingUser.Height = userDTO.Height;

            _context.Entry(existingUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        [HttpPost("UserLogin")]
        public ActionResult<UserDTO> UserLogin(UserDTO userDTO)
        {

            User user = _context.Users.FirstOrDefault(a => a.Name == userDTO.Name && a.Password == userDTO.Password);
            if (user != null)
            {
                return new UserDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Password = user.Password,
                    Weight = user.Weight,
                    Height = user.Height
                };
            }
            else
            {
                return BadRequest("нЕПРАВИЛЬНЫЙ лОгин или пароль");
            }

        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(u => u.Id == id)).GetValueOrDefault();
        }
    }
}
