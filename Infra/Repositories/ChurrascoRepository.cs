using Domain.Entities;
using Domain.Interfaces;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories
{
    public class ChurrascoRepository : IChurrascoRepository
    {
        private readonly IDBChurrascoContext _context;

        public ChurrascoRepository(IDBChurrascoContext context) => _context = context;

        public bool CadastrarChurrasco(Churrasco churrasco)
        {
            _context.Churrasco.Add(churrasco);
            _context.SaveChanges();

            return true;
        }

        public bool RemoverChurrasco(int id)
        {
            var churrasco = _context.Churrasco.Find(id);

            if (churrasco == null)
            {
                throw new ArgumentException($"O Churrasco com o Id {id} não foi encontrado");
            }

            _context.Churrasco.Remove(churrasco);
            _context.SaveChanges();

            return true;
        }

        public List<Churrasco> BuscarTodosChurrascos()
        {
            var ret = _context.Churrasco.Include(x => x.Participantes).ToList();

            if (ret == null)
            {
                throw new ArgumentException("Não foram encontrados churrascos cadastrados.");
            }

            return ret;
        }

        public Churrasco ObterDetalhesChurrasco(int id)
        {
            var ret = _context.Churrasco.Include(x => x.Participantes).FirstOrDefault(x => x.Id == id);

            if (ret == null)
            {
                throw new ArgumentException($"O Churrasco com o Id {id} não foi encontrado");
            }
            if (ret.Participantes == null || ret.Participantes.Count == 0)
            {
                throw new ArgumentException($"O Churrasco com o Id {id} não possui participantes");
            }

            return ret;
        }
    }
}
