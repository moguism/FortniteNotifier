using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefreshController : ControllerBase
    {
        [HttpGet]
        public string RefreshServer()
        {
            return "Refreshed";
        }
    }
}
