using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using backend.Models;
using backend.Services;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager _userManager = new UserManager();

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Users user)
        {
            var validUser = await _userManager.LoginUserAsync(user.UserName, user.UserPassword);

            if (validUser == null)
                return Unauthorized(new { message = "Usuario o contraseña incorrectos" });

            return Ok(new { message = "Login exitoso", UserID = validUser.UserID });
        }
    }
}