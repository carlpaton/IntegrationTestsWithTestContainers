using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerApi.Controllers
{
    [ApiController]
    public class PingController : ControllerBase
    {
        [HttpGet("/ping")]
        [AllowAnonymous]
        public IActionResult Get()
        {
            return Ok("ping ok");
        }
    }
}
