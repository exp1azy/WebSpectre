using Microsoft.AspNetCore.Mvc;
using WebSpectre.Server.Exceptions;
using WebSpectre.Server.Services.Interfaces;
using WebSpectre.Shared.Models;

namespace WebSpectre.Server.Controllers
{
    [ApiController]
    [Route("api.webspectre/agent")]
    public class AgentController(IAgentService agentService) : Controller
    {
        private readonly IAgentService _agentService = agentService;

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromQuery] string host, [FromQuery] string url, CancellationToken cancellationToken)
        {
            try
            {
                await _agentService.AddAgentAsync(new AgentModel { Hostname = host.ToLower(), Url = url.ToLower() }, cancellationToken);
                return Ok();
            }
            catch (EntityAlreadyExistException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> Remove([FromQuery] string host, CancellationToken cancellationToken)
        {
            try
            {
                await _agentService.RemoveAgentAsync(host, cancellationToken);
                return Ok();
            }
            catch (NoSuchAgentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
