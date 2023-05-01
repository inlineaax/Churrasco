using Domain.Entities;

namespace Application.IService
{
    public interface ITokenService
    {
        object GerarToken(Usuario usuario);
    }
}
