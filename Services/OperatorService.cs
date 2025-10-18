using FitnessApp.Data;
using FitnessApp.Models;
using FitnessApp.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Services
{
    /// <summary>
    /// Serwis do zarządzania operatorami (pracownikami systemu)
    /// </summary>
    public class OperatorService
    {
        private readonly AppDbContext _db;

        public OperatorService(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Pobiera dane do widoku operatorów (lista operatorów i ról)
        /// </summary>
        public async Task<OperatorsViewModel> GetOperatorsViewModelAsync()
        {
            var operators = await _db.Operators
                .Include(o => o.Role)
                .AsNoTracking()
                .OrderBy(o => o.LastName)
                .ToListAsync();

            var roles = await _db.OperatorRoles
                .AsNoTracking()
                .OrderBy(r => r.Name)
                .ToListAsync();

            return new OperatorsViewModel
            {
                Operators = operators,
                Roles = roles
            };
        }

        /// <summary>
        /// Dodaje nowego operatora do bazy
        /// </summary>
        public async Task<(bool Success, string Message)> AddOperatorAsync(Operator op)
        {
            // walidacja modelu
            if (string.IsNullOrWhiteSpace(op.Email))
                return (false, "Email jest wymagany.");

            var exists = await _db.Operators.AnyAsync(o => o.Email == op.Email);
            if (exists)
                return (false, $"Operator z adresem {op.Email} już istnieje.");

            _db.Operators.Add(op);
            await _db.SaveChangesAsync();

            return (true, $"Dodano nowego operatora: {op.FirstName} {op.LastName}");
        }
    }
}
