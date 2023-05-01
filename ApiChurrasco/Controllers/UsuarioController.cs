using Application.IService;
using Application.Models.Usuario;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace ApiChurrasco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(IUsuarioService usuarioService, ILogger<UsuarioController> logger)
        {
            _usuarioService = usuarioService;
            _logger = logger;
        }
        [HttpPost("login")]
        public ActionResult FazerLogin([FromBody]DadosLogin login)
        {
            try
            {
                _logger.LogInformation("Rastreio - Entrou no Post - Login");
                var ret = _usuarioService.FazerLogin(login);
                return Ok(ret);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "ERRO");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("cadastrar")]
        public ActionResult CadastrarUsuario([FromBody] DadosLogin login)
        {
            if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Senha))
            {
                _logger.LogError("Campos inválidos, ERRO");
                return BadRequest("Campos inválidos");
            }
            try
            {
                _logger.LogInformation("Rastreio - Entrou no Post - CadastrarUsuario");
                MailAddress mailAddress = new MailAddress(login.Email);
                var ret = _usuarioService.CadastrarUsuario(login);
                return Ok(ret);
            }
            catch (FormatException)
            {
                _logger.LogError("Email inválido, ERRO");
                return BadRequest("Email inválido");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "ERRO");
                return BadRequest(ex.Message);
            }
        }
    }
}
