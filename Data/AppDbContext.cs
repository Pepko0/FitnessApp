using Microsoft.EntityFrameworkCore;
using FitnessApp.Models;

namespace FitnessApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Tabela operatorów (pracowników)
        public DbSet<Operator> Operators { get; set; } = default!;

        // Tabela ról operatorów
        public DbSet<OperatorRole> OperatorRoles { get; set; } = default!;
        public DbSet<Client> Clients { get; set; }

        public DbSet<Post> Posts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Operator>()
                .HasIndex(o => o.Email)
                .IsUnique();

            modelBuilder.Entity<Operator>()
                .HasOne(o => o.Role)
                .WithMany()
                .HasForeignKey(o => o.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
