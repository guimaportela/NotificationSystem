using Microsoft.AspNetCore.Mvc;
using NotificationSystem.Common;
using NotificationSystem.Contracts;
using NotificationSystem.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Threading.RateLimiting;

namespace NotificationSystem.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RateLimitController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetRateLimits()
        {
            return Ok(NotificationRateLimits.GetRateLimits());
        }
    }
}
