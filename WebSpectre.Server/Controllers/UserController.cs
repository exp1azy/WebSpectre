using Microsoft.AspNetCore.Mvc;
using WebSpectre.Server.Exceptions;
using WebSpectre.Server.Services.Interfaces;

namespace WebSpectre.Server.Controllers
{
    [ApiController]
    [Route("api.webspectre/user")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromQuery] string u, [FromQuery] string p, CancellationToken cancellationToken)
        {
            try
            {
                await _userService.AddUserAsync(u, p, cancellationToken);
            }
            catch (UserAlreadyExistException)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login([FromQuery] string u, [FromQuery] string p, CancellationToken cancellationToken)
        {
            var loggedIn = await _userService.LoginUserAsync(u, p, cancellationToken);
            if (loggedIn == null)
            {
                return BadRequest();
            }

            return Ok(loggedIn);
        }
    }
}
