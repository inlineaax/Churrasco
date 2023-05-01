using Domain.Entities;
using Domain.Interfaces;
using Domain.IRepositories;

namespace Infra.Repositories
{
    public class PaticipanteRepository : IParticipanteRepository
    {
        private readonly IDBChurrascoContext _context;

        public PaticipanteRepository(IDBChurrascoContext context) => _context = context;

        public bool AdicionarParticipante(Participante participante)
        {
            _context.Participante.Add(participante);
            _context.SaveChanges();

            return true;
        }
        public bool RemoverParticipante(int idParticipante, int idChurrasco)
        {
            Participante? participante = _context.Participante.SingleOrDefault(p => p.Id == idParticipante && p.Id_Churrasco == idChurrasco);

            if (participante == null)
            {
                throw new ArgumentException($"Não foi possível encontrar o participante com o ID {idParticipante} no churrasco com o ID {idChurrasco}");
            }

            _context.Participante.Remove(participante);
            _context.SaveChanges();

            return true;
        }
    }
}
