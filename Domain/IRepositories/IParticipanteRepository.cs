using Domain.Entities;

namespace Domain.IRepositories
{
    public interface IParticipanteRepository
    {
        bool AdicionarParticipante(Participante participante);
        bool RemoverParticipante(int idParticipante, int idChurrasco);
    }
}
