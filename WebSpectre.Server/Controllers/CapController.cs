using Microsoft.AspNetCore.Mvc;
using WebSpectre.Server.Exceptions;
using WebSpectre.Server.Services.Interfaces;

namespace WebSpectre.Server.Controllers
{
    [ApiController]
    [Route("api.webspectre/cap")]
    public class CapController : Controller
    {
        private readonly IMonitoringService _monitoringService;

        public CapController(IMonitoringService monitoringService)
        {
            _monitoringService = monitoringService;
        }

        [HttpGet("agents")]
        public async Task<IActionResult> GetAgents(CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _monitoringService.GetAgentsAsync(cancellationToken));
            }
            catch (ReadingConfigurationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (NoAgentsException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("start-required")]
        public async Task<IActionResult> StartMonitorFromRequired([FromQuery] string agent, [FromQuery] int count, CancellationToken cancellationToken)
        {
            try
            {
                _ = _monitoringService.SendNetworkFromStreamAsync(agent, count, cancellationToken);
                return Ok();
            }
            catch (ReadingConfigurationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (WebSpectreRedisConnectionException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ApplicationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpGet("start")]
        public async Task<IActionResult> StartMonitor([FromQuery] int count, CancellationToken cancellationToken)
        {
            try
            {
                _ = _monitoringService.SendNetworkFromStreamAsync(count, cancellationToken);
                return Ok();
            }
            catch (ReadingConfigurationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (NoAgentsException ex)
            {
                return NotFound(ex.Message);
            }
            catch (WebSpectreRedisConnectionException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ApplicationException ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}
