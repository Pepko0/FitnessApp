using FitnessApp.Data;
using FitnessApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Services
{
    /// <summary>
    /// Klasa odpowiedzialna za logikę biznesową związaną z rolami operatorów.
    /// </summary>
    public class OperatorRoleService
    {
        private readonly AppDbContext _db;

        public OperatorRoleService(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Dodaje nową rolę operatora do bazy danych.
        /// </summary>
        /// <param name="name">Nazwa roli do dodania.</param>
        /// <returns>Krotka z informacją o sukcesie i komunikatem.</returns>
        public async Task<(bool Success, string Message)> AddRoleAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return (false, "Nazwa roli nie może być pusta.");

            var exists = await _db.OperatorRoles
                                  .AnyAsync(r => r.Name.ToLower() == name.Trim().ToLower());

            if (exists)
                return (false, $"Rola „{name}” już istnieje.");

            _db.OperatorRoles.Add(new OperatorRole { Name = name.Trim() });
            await _db.SaveChangesAsync();

            return (true, $"Dodano rolę „{name}”.");
        }

        /// <summary>
        /// Pobiera wszystkie role operatorów z bazy danych.
        /// </summary>
        public async Task<List<OperatorRole>> GetAllRolesAsync()
        {
            return await _db.OperatorRoles
                            .AsNoTracking()
                            .OrderBy(r => r.Name)
                            .ToListAsync();
        }

        public async Task<(bool Success, string Message)> DeleteRoleAsync(int id)
        {
            var role = await _db.OperatorRoles.FirstOrDefaultAsync(r => r.Id == id);
        
            if (role == null)
                return (false, "Nie znaleziono roli.");
        
            // sprawdź czy ktoś używa tej roli
            bool isUsed = await _db.Operators.AnyAsync(o => o.RoleId == id);
        
            if (isUsed)
                return (false, "Nie można usunąć roli, ponieważ jest przypisana do operatorów.");
        
            _db.OperatorRoles.Remove(role);
            await _db.SaveChangesAsync();
        
            return (true, "Rola została usunięta.");
        }
    }
}
