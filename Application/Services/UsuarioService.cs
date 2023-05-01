using Application.IService;
using Application.Models.Usuario;
using Domain.Entities;
using Domain.IRepositories;

namespace Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenService _tokenService;

        public UsuarioService(IUsuarioRepository usuarioRepository, ITokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
        }
        public RetornaMensagemLogin FazerLogin(DadosLogin req)
        {
            Usuario usuario = _usuarioRepository.ConsultaUsuario(req.Email);
            if (usuario != null && usuario.Senha == req.Senha)
            {
                var token = _tokenService.GerarToken(usuario);
                return new RetornaMensagemLogin()
                {
                    Mensagem = "Login feito com sucesso",
                    Token = token
                };
            }
            if (usuario != null && usuario.Senha != req.Senha)
            {
                throw new Exception("Senha incorreta");
            }

            else
            {
                throw new Exception("Usuário não encontrado");
            }
        }
        public RetornaMensagemCadastro CadastrarUsuario(DadosLogin req)
        {
            var ret = new RetornaMensagemCadastro();

            Usuario usuario = new Usuario()
            {
                Email = req.Email,
                Senha = req.Senha,
            };

            if (_usuarioRepository.ConsultaUsuario(usuario.Email) != null)
            {
                throw new Exception("Usuário já cadastrado");
            }

            ret.Mensagem = "Usuário cadastrado com sucesso";
            return ret;

        }
    }
}
