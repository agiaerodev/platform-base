using Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Icomments.Controllers
{
    [Route("api/icomments/v1/service-status")]
    [ApiController]
    public class ServiceStatusController : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetStatus([FromQuery] UrlRequestBase? urlRequestBase)
        {
            int status = 200;
            object? response = null;

            return StatusCode(status, await ResponseBase.Response(response));

        }
    }
}
