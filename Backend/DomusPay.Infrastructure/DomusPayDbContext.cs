using DomusPay.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DomusPay.Infrastructure;

public class DomusPayDbContext(DbContextOptions<DomusPayDbContext> options) : DbContext(options)
{
    public DbSet<Pessoa> Pessoas { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Transacao> Transacoes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Pessoa>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Idade).IsRequired();
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Descricao).IsRequired().HasMaxLength(400);
            entity.Property(e => e.Finalidade).IsRequired();
        });

        modelBuilder.Entity<Transacao>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Descricao).IsRequired().HasMaxLength(400);
            entity.Property(e => e.Valor).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.Tipo).IsRequired();
            entity.HasOne(e => e.Categoria).WithMany(e => e.Transacoes).HasForeignKey(e => e.CategoriaId).OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(e => e.Pessoa).WithMany(e => e.Transacoes).HasForeignKey(e => e.PessoaId).OnDelete(DeleteBehavior.Cascade);
        });
    }
}
