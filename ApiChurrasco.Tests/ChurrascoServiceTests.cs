using Application.Models.Churrasco;
using Application.Services;
using Domain.Entities;
using Domain.IRepositories;
using FluentAssertions;
using Moq;


namespace ApiChurrasco.Tests
{
    public class ChurrascoServiceTests
    {
        private readonly ChurrascoService _churrascoService;
        private readonly Mock<IChurrascoRepository> _churrascoRepositoryMock = new Mock<IChurrascoRepository>();

        public ChurrascoServiceTests() => _churrascoService = new ChurrascoService(_churrascoRepositoryMock.Object);

        #region CadastrarChurrasco
        [Fact]
        public void CadastrarChurrasco_Deve_Cadastrar_Com_Sucesso()
        {
            // Arrange
            var dadosChurrasco = new DadosChurrasco()
            {
                Data = DateTime.Today.AddDays(7),
                Descricao = "Churrasco de Aniversário",
                ValorComBebida = 50,
                ValorSemBebida = 30
            };

            // Act
            var resultado = _churrascoService.CadastrarChurrasco(dadosChurrasco);

            // Assert
            _churrascoRepositoryMock.Verify(r => r.CadastrarChurrasco(It.IsAny<Churrasco>()), Times.Once);
            resultado.Resposta.Should().Be("Churrasco cadastrado com sucesso");
        }

        [Fact]
        public void CadastrarChurrasco_DeveLancarExcecaoQuandoDescricaoNulaOuVazia()
        {
            // Arrange
            var dadosChurrasco = new DadosChurrasco()
            {
                Data = DateTime.Today.AddDays(7),
                Descricao = "",
                ValorComBebida = 50,
                ValorSemBebida = 30
            };

            // Act
            Action act = () => _churrascoService.CadastrarChurrasco(dadosChurrasco);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("A descrição do churrasco é obrigatória");
        }

        [Fact]
        public void CadastrarChurrasco_DeveLancarExcecaoQuandoDataForAnteriorADataAtual()
        {
            // Arrange
            var dadosChurrasco = new DadosChurrasco()
            {
                Data = DateTime.Today.AddDays(-1),
                Descricao = "Churrasco de Aniversário",
                ValorComBebida = 50,
                ValorSemBebida = 30
            };

            // Act
            Action act = () => _churrascoService.CadastrarChurrasco(dadosChurrasco);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("A data do churrasco não pode ser anterior à data atual");
        }

        [Fact]
        public void CadastrarChurrasco_DeveLancarExcecaoQuandoValorSugeridoForMenorOuIgualAZero()
        {
            // Arrange
            var dadosChurrasco = new DadosChurrasco()
            {
                Data = DateTime.Today.AddDays(7),
                Descricao = "Churrasco de Aniversário",
                ValorComBebida = 50,
                ValorSemBebida = 0
            };

            // Act
            Action act = () => _churrascoService.CadastrarChurrasco(dadosChurrasco);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("O valor sugerido do churrasco deve ser maior que zero");
        }

        #endregion

        #region RemoverChurrasco
        [Fact]
        public void RemoverChurrasco_DeveRetornarMensagemDeSucesso_QuandoChurrascoExiste()
        {
            // Arrange
            int id = 1;
            var churrasco = new Churrasco() { Id = id };
            _churrascoRepositoryMock.Setup(x => x.ObterDetalhesChurrasco(id)).Returns(churrasco);

            // Act
            var resultado = _churrascoService.RemoverChurrasco(id);

            // Assert
            resultado.Resposta.Should().Be("Churrasco excluido com sucesso");
            _churrascoRepositoryMock.Verify(x => x.RemoverChurrasco(id), Times.Once);
        }

        [Fact]
        public void RemoverChurrasco_DeveLancarException_QuandoOcorrerErroAoDeletarChurrasco()
        {
            // Arrange
            int id = 1;
            var churrasco = new Churrasco() { Id = id };
            _churrascoRepositoryMock.Setup(x => x.ObterDetalhesChurrasco(id)).Returns(churrasco);
            _churrascoRepositoryMock.Setup(x => x.RemoverChurrasco(id)).Throws<Exception>();

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => _churrascoService.RemoverChurrasco(id));
            exception.Message.Should().Be(exception.Message);

            _churrascoRepositoryMock.Verify(x => x.RemoverChurrasco(id), Times.Once);
        }

        #endregion

        #region BuscarTodosChurrascos
        [Fact]
        public void BuscarTodosChurrascos_DeveRetornarListaDeChurrascos_QuandoExistiremChurrascosCadastrados()
        {
            // Arrange
            var churrascos = new List<Churrasco>
        {
            new Churrasco { Id = 1, Descricao = "Churrasco de carnaval", Data = DateTime.Now.AddDays(1), Valor_Sugerido_Com_Bebida = 30.00m, Valor_Sugerido_Sem_Bebida = 20.00m },
            new Churrasco { Id = 2, Descricao = "Churrasco de Natal", Data = DateTime.Now.AddDays(2), Valor_Sugerido_Com_Bebida = 50.00m, Valor_Sugerido_Sem_Bebida = 40.00m }
        };
            _churrascoRepositoryMock.Setup(repo => repo.BuscarTodosChurrascos()).Returns(churrascos);

            var churrascoService = new ChurrascoService(_churrascoRepositoryMock.Object);

            // Act
            var resultado = churrascoService.BuscarTodosChurrascos();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeOfType<List<Churrasco>>();
            resultado.Should().HaveCount(2);
            resultado.Should().BeEquivalentTo(churrascos);
        }

        #endregion

        #region BuscarDetalhesChurrasco
        [Fact]
        public void BuscarDetalhesChurrasco_DeveRetornarDetalhesChurrasco()
        {
            // Arrange
            var idChurrasco = 1;
            var churrasco = new Churrasco
            {
                Id = idChurrasco,
                Data = DateTime.Now,
                Descricao = "Churrasco Teste",
                Participantes = new List<Participante>
                {
                    new Participante
                    {
                        Id = 1,
                        Nome = "Ana",
                        Valor_Pago = 25
                    },
                    new Participante
                    {
                        Id = 2,
                        Nome = "João",
                        Valor_Pago = 30
                    }
                }
            };

            _churrascoRepositoryMock.Setup(c => c.ObterDetalhesChurrasco(idChurrasco)).Returns(churrasco);

            // Act
            var detalhesChurrasco = _churrascoService.BuscarDetalhesChurrasco(idChurrasco);

            // Assert
            detalhesChurrasco.Should().NotBeNull();
            detalhesChurrasco.Data.Should().Be(churrasco.Data);
            detalhesChurrasco.Descricao.Should().Be(churrasco.Descricao);
            detalhesChurrasco.Total_Participantes.Should().Be(churrasco.Participantes.Count);

            detalhesChurrasco.Valor_Total_Arrecadado.Should().Be(churrasco.Participantes.Sum(p => p.Valor_Pago));

            detalhesChurrasco.Lista_Participantes.Should().NotBeNullOrEmpty();
            detalhesChurrasco.Lista_Participantes.Should().BeEquivalentTo(churrasco.Participantes.Select(p => new DetalhesChurrasco.Participantes
            {
                Id = p.Id,
                Nome = p.Nome,
                ValorPago = p.Valor_Pago
            }));
        }

        #endregion
    }
}
