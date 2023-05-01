using Application.Models.Usuario;

namespace Application.IService
{
    public interface IUsuarioService
    {
        RetornaMensagemLogin FazerLogin(DadosLogin req);
        RetornaMensagemCadastro CadastrarUsuario(DadosLogin req);
    }
}
