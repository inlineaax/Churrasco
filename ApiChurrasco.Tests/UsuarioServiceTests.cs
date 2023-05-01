using Application.IService;
using Application.Models.Usuario;
using Application.Services;
using Domain.Entities;
using Domain.IRepositories;
using FluentAssertions;
using Moq;

namespace ApiChurrasco.Tests
{
    public class UsuarioServiceTests
    {
        private readonly UsuarioService _usuarioService;
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private readonly Mock<ITokenService> _tokenServiceMock;

        public UsuarioServiceTests()
        {
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _tokenServiceMock = new Mock<ITokenService>();
            _usuarioService = new UsuarioService(_usuarioRepositoryMock.Object, _tokenServiceMock.Object);
        }

        #region FazerLogin
        [Fact]
        public void FazerLogin_DeveRetornarMensagemComToken_QuandoEmailESenhaEstaoCorretos()
        {
            // Arrange
            var email = "usuario@teste.com";
            var senha = "senha123";
            var req = new DadosLogin { Email = email, Senha = senha };
            var usuario = new Usuario { Email = email, Senha = senha };
            var token = "token123";
            _usuarioRepositoryMock.Setup(r => r.ConsultaUsuario(email)).Returns(usuario);
            _tokenServiceMock.Setup(t => t.GerarToken(usuario)).Returns(token);

            // Act
            var result = _usuarioService.FazerLogin(req);

            // Assert
            result.Mensagem.Should().Be("Login feito com sucesso");
            result.Token.Should().Be(token);
        }

        [Fact]
        public void FazerLogin_DeveLancarExcecao_QuandoSenhaEstaIncorreta()
        {
            // Arrange
            var email = "usuario@teste.com";
            var senha = "senha123";
            var req = new DadosLogin { Email = email, Senha = "senha456" };
            var usuario = new Usuario { Email = email, Senha = senha };
            _usuarioRepositoryMock.Setup(r => r.ConsultaUsuario(email)).Returns(usuario);

            // Act
            Action act = () => _usuarioService.FazerLogin(req);

            // Assert
            act.Should().Throw<Exception>().WithMessage("Senha incorreta");
        }

        [Fact]
        public void FazerLogin_DeveLancarExcecao_QuandoUsuarioNaoFoiEncontrado()
        {
            // Arrange
            var email = "usuario@teste.com";
            var senha = "senha123";
            var req = new DadosLogin { Email = email, Senha = senha };
            _usuarioRepositoryMock.Setup(r => r.ConsultaUsuario(email)).Returns((Usuario)null);

            // Act
            Action act = () => _usuarioService.FazerLogin(req);

            // Assert
            act.Should().Throw<Exception>().WithMessage("Usuário não encontrado");
        }

        #endregion

        #region CadastrarUsuario
        [Fact]
        public void CadastrarUsuario_DeveRetornarMensagemDeSucesso_QuandoUsuarioNaoCadastrado()
        {
            // Arrange
            var dadosLogin = new DadosLogin
            {
                Email = "usuario@teste.com",
                Senha = "123456"
            };
            _usuarioRepositoryMock.Setup(r => r.ConsultaUsuario(dadosLogin.Email)).Returns((Usuario)null);

            // Act
            var resultado = _usuarioService.CadastrarUsuario(dadosLogin);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Mensagem.Should().Be("Usuário cadastrado com sucesso");
        }

        [Fact]
        public void CadastrarUsuario_DeveLancarExcecao_QuandoUsuarioJaCadastrado()
        {
            // Arrange
            var dadosLogin = new DadosLogin
            {
                Email = "usuario@teste.com",
                Senha = "123456"
            };
            _usuarioRepositoryMock.Setup(r => r.ConsultaUsuario(dadosLogin.Email)).Returns(new Usuario());

            // Act & Assert
            _usuarioService.Invoking(s => s.CadastrarUsuario(dadosLogin)).Should().Throw<Exception>()
                .WithMessage("Usuário já cadastrado");
            _usuarioRepositoryMock.Verify(r => r.ConsultaUsuario(dadosLogin.Email), Times.Once);
            _usuarioRepositoryMock.Verify(r => r.CadastrarUsuario(It.IsAny<Usuario>()), Times.Never);
        }

        #endregion
    }
}
