using Chizh.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chizh.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private User24Context _context;

        public AdminController(User24Context context)
        {
            _context = context;
        }

        [HttpGet("GetAllAdmins")]
        public async Task<ActionResult<List<AdminDTO>>> GetAdmins()
        {
            List<AdminDTO> admins = _context.Admins.ToList().Select(s => new AdminDTO
            {
                Id = s.Id,
                AdmName = s.AdmName,
                AdmPassword = s.AdmPassword,
            }).ToList();
            return admins;
        }

        [HttpGet("GetAdmin")]
        public async Task<ActionResult<AdminDTO>> GetAdmin(int id)
        {
            var s = _context.Admins.FirstOrDefault(s => s.Id == id);
            if (s == null)
            {
                return NotFound();

            }
            return Ok(new AdminDTO
            {
                Id = s.Id,
                AdmName = s.AdmName,
                AdmPassword = s.AdmPassword,
            });
        }

        [HttpPost("AdminLogin")]
        public ActionResult<AdminDTO> AdminLogin(AdminDTO adminDTO)
        {

            Admin admin = _context.Admins.FirstOrDefault(a => a.AdmName == adminDTO.AdmName && a.AdmPassword == adminDTO.AdmPassword);
            if (admin != null)
            {
                return new AdminDTO
                {
                    Id = admin.Id,
                    AdmName = admin.AdmName,
                    AdmPassword = admin.AdmPassword,
                };
            }
            else
            {
                return BadRequest("нЕПРАВИЛЬНЫЙ лОгин или пароль");
            }
        }
    }
}
