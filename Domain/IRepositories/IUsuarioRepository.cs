using Domain.Entities;

namespace Domain.IRepositories
{
    public interface IUsuarioRepository
    {
        Usuario ConsultaUsuario(string email);
        bool CadastrarUsuario(Usuario usuario);
    }
}
