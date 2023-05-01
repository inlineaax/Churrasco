using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Interfaces
{
    public interface IDBChurrascoContext
    {
        DbSet<Churrasco> Churrasco { get; set; }
        DbSet<Participante> Participante { get; set; }
        DbSet<Usuario> Usuario { get; set; }
        Task<int> SaveChangesAsync();
        int SaveChanges();
    }
}
