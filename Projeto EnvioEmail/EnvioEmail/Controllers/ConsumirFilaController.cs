using EnvioEmail.Services;
using Microsoft.AspNetCore.Mvc;

namespace EnvioEmail.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConsumirFilaController : ControllerBase
    {
        [HttpGet("consumir")]
        public ActionResult Consumir() {

            var rabbit = new RabbitMQService();
            rabbit.Consumir();

            return Ok();
        }
    }
}
