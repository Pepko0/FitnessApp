using Microsoft.EntityFrameworkCore;
using FitnessApp.Models;

namespace FitnessApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <summary>
        /// Tabela operatorów (pracowników)
        /// </summary>
        public DbSet<Operator> Operators { get; set; } = default!;

        /// <summary>
        /// Tabela ról operatorów (np. Admin, Trener personalny)
        /// </summary>
        public DbSet<OperatorRole> OperatorRoles { get; set; } = default!;
    }
}
