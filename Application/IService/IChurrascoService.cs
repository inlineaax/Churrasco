using Application.Models.Churrasco;
using Domain.Entities;

namespace Application.IService
{
    public interface IChurrascoService
    {
        RespostaChurrasco CadastrarChurrasco(DadosChurrasco churrasco);
        RespostaChurrasco RemoverChurrasco(int id);
        List<Churrasco> BuscarTodosChurrascos();
        DetalhesChurrasco BuscarDetalhesChurrasco(int idChurrasco);

    }
}
