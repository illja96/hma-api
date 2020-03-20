using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HMA.API.Controllers
{
    [Authorize]
    [ApiController]
    public class HouseController : ControllerBase
    {
        [HttpGet("houses")]
        public IActionResult GetHouses()
        {
            return new OkResult();
        }
    }
}
