using Application.Models.Participante;

namespace Application.IService
{
    public interface IParticipanteService
    {
        RespostaParticipante AdicionarParticipante(DadosParticipante participante);
        RespostaParticipante RemoverParticipante(int idParticipante, int idChurrasco);
    }
}
