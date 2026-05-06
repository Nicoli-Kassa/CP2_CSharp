using CP2_CSharp.Models;
using Microsoft.EntityFrameworkCore;

namespace CP2_CSharp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<PessoaFisica> PessoasFisicas { get; set; }
        public DbSet<PessoaJuridica> PessoasJuridicas { get; set; }
        public DbSet<Agencia> Agencias { get; set; }
        public DbSet<Contratacao> Contratacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            modelBuilder.Entity<Cliente>().ToTable("Clientes");
            modelBuilder.Entity<PessoaFisica>().ToTable("PessoasFisicas");
            modelBuilder.Entity<PessoaJuridica>().ToTable("PessoasJuridicas");

            modelBuilder.Entity<PessoaFisica>()
                .HasIndex(p => p.Cpf)
                .IsUnique();

            modelBuilder.Entity<PessoaJuridica>()
                .HasIndex(p => p.Cnpj)
                .IsUnique();

            modelBuilder.Entity<Contratacao>()
                .Property(c => c.ValorSolicitado)
                .HasPrecision(18, 2);

            
        }
    }
}