using BiometriaValidacaoApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BiometriaValidacaoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificacaoController : ControllerBase
    {
        private readonly INotificacaoService _notificacaoService;

        public NotificacaoController(INotificacaoService notificacaoService)
        {
            _notificacaoService = notificacaoService;
        }

        [HttpPost("fraude")]
        public async Task<IActionResult> NotificarFraude([FromBody] NotificacaoFraudeRequest request)
        {
            if (request == null)
            {
                return BadRequest("Requisição inválida.");
            }

            var sucesso = await _notificacaoService.ProcessarNotificacaoFraudeAsync(request);
            if (sucesso)
            {
                return Ok(new { Message = "Notificação de fraude enviada com sucesso." });
            }
            return StatusCode(500, "Falha ao processar a notificação de fraude.");
        }
    }
}
