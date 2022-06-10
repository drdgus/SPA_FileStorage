using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SPA_Sibintek_NET6.API.v1
{
#if !DEBUG
    [Authorize]
#endif
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(User.Identity.Name);
        }
    }
}
