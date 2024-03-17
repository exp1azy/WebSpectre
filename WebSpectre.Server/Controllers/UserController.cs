using Microsoft.AspNetCore.Mvc;
using WebSpectre.Server.Exceptions;
using WebSpectre.Server.Services.Interfaces;

namespace WebSpectre.Server.Controllers
{
    [ApiController]
    [Route("api.webspectre/user")]
    public class UserController(IUserService userService) : Controller
    {
        private readonly IUserService _userService = userService;

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromQuery] string user, [FromQuery] string pass, CancellationToken cancellationToken)
        {
            try
            {
                await _userService.AddUserAsync(user, pass, cancellationToken);
            }
            catch (EntityAlreadyExistException)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login([FromQuery] string user, [FromQuery] string pass, CancellationToken cancellationToken)
        {
            var loggedIn = await _userService.LoginUserAsync(user, pass, cancellationToken);
            if (loggedIn == null)
            {
                return BadRequest();
            }

            return Ok(loggedIn);
        }
    }
}
