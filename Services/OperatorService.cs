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
        /// Pobiera operatora po ID
        /// </summary>
        public async Task<Operator?> GetOperatorByIdAsync(int id)
        {
            return await _db.Operators
                .Include(o => o.Role)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        /// <summary>
        /// Aktualizuje dane operatora
        /// </summary>
        public async Task<(bool Success, string Message)> UpdateOperatorAsync(int id, Operator updated)
        {
            var existing = await _db.Operators.FirstOrDefaultAsync(o => o.Id == id);

            if (existing == null)
                return (false, "Nie znaleziono operatora.");

            // aktualizacja pól
            existing.FirstName = updated.FirstName;
            existing.LastName  = updated.LastName;
            existing.Email     = updated.Email;
            existing.RoleId    = updated.RoleId;
            existing.UpdatedAt = DateTime.UtcNow;

            // hasła nie zmieniamy tutaj — to będzie osobna funkcja (bezpieczeństwo)

            await _db.SaveChangesAsync();

            return (true, "Pomyślnie zaktualizowano operatora.");
        }

        /// <summary>
        /// Usuwa operatora z bazy
        /// </summary>F
        public async Task<(bool Success, string Message)> DeleteOperatorAsync(int id)
        {
            var existing = await _db.Operators.FirstOrDefaultAsync(o => o.Id == id);

            if (existing == null)
                return (false, "Nie znaleziono operatora do usunięcia.");

            _db.Operators.Remove(existing);
            await _db.SaveChangesAsync();

            return (true, "Pomyślnie usunięto operatora.");
        }

        public async Task<List<OperatorRole>> GetAllRolesAsync()
        {
            return await _db.OperatorRoles
                .OrderBy(r => r.Name)
                .ToListAsync();
        }

        public async Task<(bool success, string message)> AddOperatorAsync(Operator op)
        {
            if (string.IsNullOrWhiteSpace(op.Email))
                return (false, "Email jest wymagany.");

            var exists = await _db.Operators.AnyAsync(o => o.Email == op.Email);
            if (exists)
                return (false, $"Operator z adresem {op.Email} już istnieje.");

            if (string.IsNullOrWhiteSpace(op.Password))
                return (false, "Hasło jest wymagane.");

            op.Password = BCrypt.Net.BCrypt.HashPassword(op.Password);

            op.ChangePassword = false;
            op.PasswordResetToken = null;
            op.PasswordResetTokenExpires = null;
            op.CreatedAt = DateTime.UtcNow;
            op.UpdatedAt = DateTime.UtcNow;

            _db.Operators.Add(op);
            await _db.SaveChangesAsync();

            return (true, $"Dodano nowego operatora: {op.FirstName} {op.LastName}");
        }

    }
}
