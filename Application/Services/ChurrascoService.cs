using Application.IService;
using Application.Models.Churrasco;
using Domain.Entities;
using Domain.IRepositories;

namespace Application.Services
{
    public class ChurrascoService : IChurrascoService
    {
        private readonly IChurrascoRepository _churrascoRepository;

        public ChurrascoService(IChurrascoRepository churrascoRepository) => _churrascoRepository = churrascoRepository;

        public RespostaChurrasco CadastrarChurrasco(DadosChurrasco churrasco)
        {
            if (string.IsNullOrWhiteSpace(churrasco.Descricao))
            {
                throw new ArgumentException("A descrição do churrasco é obrigatória");
            }

            if (churrasco.Data < DateTime.Today)
            {
                throw new ArgumentException("A data do churrasco não pode ser anterior à data atual");
            }

            if (churrasco.ValorComBebida <= 0 || churrasco.ValorSemBebida <= 0)
            {
                throw new ArgumentException("O valor sugerido do churrasco deve ser maior que zero");
            }

            Churrasco novoChurrasco = new Churrasco()
            {
                Data = churrasco.Data,
                Descricao = churrasco.Descricao,
                Valor_Sugerido_Com_Bebida = churrasco.ValorComBebida,
                Valor_Sugerido_Sem_Bebida = churrasco.ValorSemBebida
            };

            _churrascoRepository.CadastrarChurrasco(novoChurrasco);

            return new RespostaChurrasco { Resposta = "Churrasco cadastrado com sucesso" };
        }                                   
        public RespostaChurrasco RemoverChurrasco(int id)
        {
                _churrascoRepository.RemoverChurrasco(id);
                return new RespostaChurrasco { Resposta = "Churrasco excluido com sucesso" };            
        }
        public List<Churrasco> BuscarTodosChurrascos()
        {
            try
            {
                var listaChurrascos = _churrascoRepository.BuscarTodosChurrascos().ToList();

                return listaChurrascos;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar churrascos: " + ex.Message);
            }
        }
        public DetalhesChurrasco BuscarDetalhesChurrasco(int idChurrasco)
        {
            var Churrasco = _churrascoRepository.ObterDetalhesChurrasco(idChurrasco);

            var participantes = Churrasco.Participantes.Select(p => new DetalhesChurrasco.Participantes
            {
                Id = p.Id,
                Nome = p.Nome,
                ValorPago = p.Valor_Pago
            }).ToList();

            var valorTotal = participantes.Sum(p => p.ValorPago);

            var detalhesChurrasco = new DetalhesChurrasco
            {
                Data = Churrasco.Data,
                Descricao = Churrasco.Descricao,
                Total_Participantes = Churrasco.Participantes.Count(),
                Lista_Participantes = participantes,
                Valor_Total_Arrecadado = valorTotal
            };

            return detalhesChurrasco;
        }
    }
}
