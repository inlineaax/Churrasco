using Domain.Entities;
using Domain.Interfaces;
using Domain.IRepositories;

namespace Infra.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IDBChurrascoContext _context;

        public UsuarioRepository(IDBChurrascoContext context) => _context = context;

        public Usuario ConsultaUsuario(string email)
        {
            var Consulta = _context.Usuario.Where(x => x.Email == email).FirstOrDefault();

            return Consulta;
        }
        public bool CadastrarUsuario(Usuario usuario)
        {
            _context.Usuario.Add(usuario);
            _context.SaveChanges();

            return true;
        }
    }
}
