using Domain.Entities;

namespace Domain.IRepositories
{
    public interface IChurrascoRepository
    {
        bool CadastrarChurrasco(Churrasco churrasco);
        bool RemoverChurrasco(int id);
        List<Churrasco> BuscarTodosChurrascos();
        Churrasco ObterDetalhesChurrasco(int id);
    }
}
