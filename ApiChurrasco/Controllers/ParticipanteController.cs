using Application.IService;
using Application.Models.Participante;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiChurrasco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipanteController : ControllerBase
    {
        private readonly IParticipanteService _participanteService;
        private readonly ILogger<ParticipanteController> _logger;

        public ParticipanteController(IParticipanteService participanteService, ILogger<ParticipanteController> logger)
        {
            _participanteService = participanteService;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        public ActionResult AdicionarParticipante([FromBody] DadosParticipante participante)
        {
            try
            {
                _logger.LogInformation("Rastreio - Entrou no Post - AdicionarParticipante");
                var ret = _participanteService.AdicionarParticipante(participante);
                return Ok(ret);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex.Message, "ERRO");
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex.Message, "ERRO");
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete]
        public ActionResult RemoverParticipante(int idParticipante, int idChurrasco)
        {
            try
            {
                _logger.LogInformation("Rastreio - Entrou no Delete - RemoverParticipante");
                var ret = _participanteService.RemoverParticipante(idParticipante, idChurrasco);
                return Ok(ret);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex.Message, "ERRO");
                return BadRequest(ex.Message);
            }
        }
    }
}
