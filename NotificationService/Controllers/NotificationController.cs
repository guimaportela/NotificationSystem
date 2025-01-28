using Microsoft.AspNetCore.Mvc;
using NotificationSystem.Contracts;
using NotificationSystem.Contracts.Business;
using NotificationSystem.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace NotificationSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly ILogger<NotificationController> _logger;
        private readonly INotificationBO _notificationBO;

        public NotificationController(ILogger<NotificationController> logger, INotificationBO notificationBO)
        {
            _logger = logger;
            _notificationBO = notificationBO;
        }

        [HttpPost]
        [Route("send")]
        public async Task<IActionResult> Send([FromBody] [Required] NotificationDTO notificationDTO)
        {
            try
            {
                await _notificationBO.Send(notificationDTO);

                return Ok("Notification sent successfully.");
            }
            catch (RateLimitExceededException ex)
            {
                return StatusCode(429, ex.Message);
            }
            catch (GatewayInternalException)
            {
                return StatusCode(202);
            }
            catch (NotImplementedException ex)
            {
                return StatusCode(501, $"Not implemented - {ex.Message}");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
