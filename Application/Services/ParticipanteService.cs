using Application.IService;
using Application.Models.Participante;
using Domain.Entities;
using Domain.IRepositories;

namespace Application.Services
{
    public class ParticipanteService : IParticipanteService
    {
        private readonly IParticipanteRepository _participanteRepository;
        private readonly IChurrascoService _churrascoService;

        public ParticipanteService(IParticipanteRepository participanteRepository, IChurrascoService churrascoService)
        {
            _participanteRepository = participanteRepository;
            _churrascoService   = churrascoService; 
        }

        public RespostaParticipante AdicionarParticipante(DadosParticipante participante)
        {
            if (participante == null)
            {
                throw new ArgumentNullException(nameof(participante), "O objeto participante não pode ser nulo.");
            }

            if (string.IsNullOrWhiteSpace(participante.Nome))
            {
                throw new ArgumentException("O nome do participante não pode ser vazio ou composto somente por espaços.", nameof(participante.Nome));
            }

            if (participante.Valor_Pago < 0)
            {
                throw new ArgumentException("O valor pago pelo participante não pode ser negativo.", nameof(participante.Valor_Pago));
            }

            var Churrasco = _churrascoService.BuscarDetalhesChurrasco(participante.Id_Churrasco);  

            if (Churrasco == null)
            {
                throw new ArgumentException("Id do Churrasco inexistente");
            }         

            Participante novoParticipante = new Participante()
            {
                Id_Churrasco = participante.Id_Churrasco,
                Nome = participante.Nome,
                Valor_Pago = participante.Valor_Pago
            };

            _participanteRepository.AdicionarParticipante(novoParticipante);

            return new RespostaParticipante { Resposta = "Participante cadastrado com sucesso" };
        }

        public RespostaParticipante RemoverParticipante(int idParticipante, int idChurrasco)
        {
            try
            {
                _participanteRepository.RemoverParticipante(idParticipante, idChurrasco);
                return new RespostaParticipante { Resposta = "Participante excluído com sucesso" };
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}
