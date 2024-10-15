using System.Security.Claims;
using BudgetMate.Application.DTO.Stats;
using BudgetMate.Application.Interfaces.Stats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BudgetMate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly IStatsService _service;

        public StatsController(IStatsService service)
        {
            _service = service;
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<StatsDto>> GetStatsAsync()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            return await _service.GetStatsAsync(userId);
        }
    }
}
