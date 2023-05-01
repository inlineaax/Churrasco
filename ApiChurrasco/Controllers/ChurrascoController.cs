using Application.IService;
using Application.Models.Churrasco;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiChurrasco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChurrascoController : ControllerBase
    {
        private readonly IChurrascoService _churrascoService;
        private readonly ILogger<ChurrascoController> _logger;

        public ChurrascoController(IChurrascoService churrascoService, ILogger<ChurrascoController> logger)
        {
            _churrascoService = churrascoService;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        public ActionResult CadastrarChurrasco([FromBody] DadosChurrasco churrasco)
        {
            try
            {
                _logger.LogInformation("Rastreio - Entrou no Post - CadastrarChurrasco");
                var ret = _churrascoService.CadastrarChurrasco(churrasco);
                return Ok(ret);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex.Message, "ERRO");
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete]
        public ActionResult RemoverChurrasco(int id)
        {
            try
            {
                _logger.LogInformation("Rastreio - Entrou no Delete - RemoverChurrasco");
                var ret = _churrascoService.RemoverChurrasco(id);
                return Ok(ret);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex.Message, "ERRO");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "ERRO");
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult BuscarTodosChurrascos()
        {
            try
            {
                _logger.LogInformation("Rastreio - Entrou no Get - BuscarTodosChurrascos");
                var ret = _churrascoService.BuscarTodosChurrascos();
                return Ok(ret);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "ERRO");
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("id")]
        public ActionResult BuscarDetalhesChurrasco(int idChurrasco)
        {
            try
            {
                _logger.LogInformation("Rastreio - Entrou no Get - BuscarDetalhesChurrasco");
                var ret = _churrascoService.BuscarDetalhesChurrasco(idChurrasco);
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
