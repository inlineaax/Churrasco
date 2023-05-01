using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;

namespace Infra.Context
{
    public class DBChurrascoContext : DbContext , IDBChurrascoContext
    {
        protected readonly IConfiguration Configuration;
        public DBChurrascoContext(DbContextOptions<DBChurrascoContext> options, IConfiguration configuration
            ) : base(options)
        {
            Configuration = configuration;
        }

        public DBChurrascoContext() { }

        public DbSet<Churrasco> Churrasco { get; set; }
        public DbSet<Participante> Participante { get; set; }
        public DbSet<Usuario> Usuario { get; set; }

        public IDbContextTransaction? Transaction { get; private set; }

        public IDbContextTransaction BeginTransaction()
        {
            if (Transaction == null)
                Transaction = this.Database.BeginTransaction();

            return Transaction;
        }
        public async Task<int> SaveChangesAsync()
        {
            var save = await base.SaveChangesAsync();
            await CommitAsync();
            return save;
        }

        public override int SaveChanges()
        {
            var save = base.SaveChanges();
            Commit();
            return save;
        }

        internal void RollBack()
        {
            if (Transaction != null)
            {
                Transaction.Rollback();
            }
        }
        private void Save()
        {
            try
            {
                ChangeTracker.DetectChanges();
                SaveChanges();
            }
            catch
            {
                RollBack();
                throw;
            }
        }
        private async Task CommitAsync()
        {
            if (Transaction != null)
            {
                await Transaction.CommitAsync();
                await Transaction.DisposeAsync();
                Transaction = null;
            }
        }
        private void Commit()
        {
            if (Transaction != null)
            {
                Transaction.Commit();
                Transaction.Dispose();
                Transaction = null;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Migrations
            modelBuilder.Entity<Churrasco>().HasKey(x => x.Id);
            modelBuilder.Entity<Participante>().HasKey(x => x.Id);
            modelBuilder.Entity<Usuario>().HasKey(x => x.Id);
            modelBuilder.Entity<Participante>().HasOne(x => x.Churrasco).WithMany(x => x.Participantes).HasForeignKey(x => x.Id_Churrasco);
        }
    }
}
