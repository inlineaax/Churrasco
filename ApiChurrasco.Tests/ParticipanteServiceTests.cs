using Application.IService;
using Application.Models.Churrasco;
using Application.Models.Participante;
using Application.Services;
using Domain.Entities;
using Domain.IRepositories;
using FluentAssertions;
using Moq;


namespace ApiChurrasco.Tests
{
    public class ParticipanteServiceTests
    {
        private readonly Mock<IParticipanteRepository> _participanteRepositoryMock;
        private readonly Mock<IChurrascoService> _churrascoServiceMock;
        private readonly ParticipanteService _participanteService;

        public ParticipanteServiceTests()
        {
            _participanteRepositoryMock = new Mock<IParticipanteRepository>();
            _churrascoServiceMock = new Mock<IChurrascoService>();
            _participanteService = new ParticipanteService(_participanteRepositoryMock.Object, _churrascoServiceMock.Object);
        }

        #region AdicionarParticipante
        [Fact]
        public void AdicionarParticipante_DeveCadastrarParticipante_QuandoDadosSaoValidos()
        {
            // Arrange
            var participante = new DadosParticipante
            {
                Id_Churrasco = 1,
                Nome = "Fulano",
                Valor_Pago = 25.00m
            };

            var churrasco = new Churrasco
            {
                Id = participante.Id_Churrasco,
                Descricao = "Churrasco de Aniversário",
                Data = new DateTime(2022, 05, 01),
                Valor_Sugerido_Com_Bebida = 50,
                Valor_Sugerido_Sem_Bebida = 25
            };

            _churrascoServiceMock.Setup(c => c.BuscarDetalhesChurrasco(participante.Id_Churrasco)).Returns(new DetalhesChurrasco
            {
                Data = churrasco.Data,
                Descricao = churrasco.Descricao,
                Total_Participantes = 0,
                Lista_Participantes = new List<DetalhesChurrasco.Participantes>(),
                Valor_Total_Arrecadado = 0
            });

            // Act
            var response = _participanteService.AdicionarParticipante(participante);

            // Assert
            response.Resposta.Should().Be("Participante cadastrado com sucesso");
            _participanteRepositoryMock.Verify(pr => pr.AdicionarParticipante(It.Is<Participante>(p =>
                p.Id_Churrasco == participante.Id_Churrasco &&
                p.Nome == participante.Nome &&
                p.Valor_Pago == participante.Valor_Pago)), Times.Once);
        }

        [Fact]
        public void AdicionarParticipante_DeveLancarArgumentNullExceptionQuandoParticipanteForNulo()
        {
            // Arrange
            DadosParticipante participante = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _participanteService.AdicionarParticipante(participante));
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void AdicionarParticipante_DeveLancarArgumentExceptionQuandoNomeForInvalido(string nomeInvalido)
        {
            // Arrange
            var participante = new DadosParticipante
            {
                Id_Churrasco = 1,
                Nome = nomeInvalido,
                Valor_Pago = 25.00m
            };

            // Act
            Action act = () => _participanteService.AdicionarParticipante(participante);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("O nome do participante não pode ser vazio ou composto somente por espaços. (Parameter 'Nome')");
            _participanteRepositoryMock.Verify(pr => pr.AdicionarParticipante(It.IsAny<Participante>()), Times.Never);
        }

        [Fact]
        public void AdicionarParticipante_DeveLancarArgumentExceptionQuandoValorPagoForNegativo()
        {
            // Arrange
            var participante = new DadosParticipante
            {
                Id_Churrasco = 1,
                Nome = "Fulano",
                Valor_Pago = -1.00m
            };

            // Act
            Action act = () => _participanteService.AdicionarParticipante(participante);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("O valor pago pelo participante não pode ser negativo. (Parameter 'Valor_Pago')");
            _participanteRepositoryMock.Verify(pr => pr.AdicionarParticipante(It.IsAny<Participante>()), Times.Never);
        }

        [Fact]
        public void AdicionarParticipante_DeveLancarArgumentExceptionQuandoIdChurrascoForInvalido()
        {
            // Arrange
            var participante = new DadosParticipante
            {
                Id_Churrasco = -1,
                Nome = "Fulano",
                Valor_Pago = 25.00m
            };

            _churrascoServiceMock.Setup(c => c.BuscarDetalhesChurrasco(participante.Id_Churrasco));

            // Act
            Action act = () => _participanteService.AdicionarParticipante(participante);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("Id do Churrasco inexistente");
            _participanteRepositoryMock.Verify(pr => pr.AdicionarParticipante(It.IsAny<Participante>()), Times.Never);
        }

        #endregion

        #region RemoverParticipante
        [Fact]
        public void RemoverParticipante_DeveChamarMetodoRemoverParticipanteDoRepositorio()
        {
            //Arrange
            int idParticipante = 1;
            int idChurrasco = 1;

            //Act
            _participanteService.RemoverParticipante(idParticipante, idChurrasco);

            //Assert
            _participanteRepositoryMock.Verify(r => r.RemoverParticipante(idParticipante, idChurrasco), Times.Once);
        }

        [Fact]
        public void RemoverParticipante_DeveRetornarMensagemDeSucesso()
        {
            //Arrange
            int idParticipante = 1;
            int idChurrasco = 1;

            //Act
            RespostaParticipante mensagem = _participanteService.RemoverParticipante(idParticipante, idChurrasco);

            //Assert
            mensagem.Resposta.Should().Be("Participante excluído com sucesso");
        }

        [Fact]
        public void RemoverParticipante_DeveLancarExcecaoQuandoRepositorioLancaExcecao()
        {
            //Arrange
            int idParticipante = 1;
            int idChurrasco = 1;
            string mensagemErro = "Erro ao excluir participante";

            _participanteRepositoryMock.Setup(r => r.RemoverParticipante(idParticipante, idChurrasco))
                .Throws(new ArgumentException(mensagemErro));

            //Act
            Action act = () => _participanteService.RemoverParticipante(idParticipante, idChurrasco);

            //Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage(mensagemErro);
        }

        #endregion
    }
}