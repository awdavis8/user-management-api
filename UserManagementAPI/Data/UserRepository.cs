using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Models;

namespace UserManagementAPI.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await Task.CompletedTask;
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeUserId = null)
        {
            if (excludeUserId.HasValue)
            {
                return await _context.Users.AnyAsync(u => u.Email == email && u.Id != excludeUserId.Value);
            }
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetUsersAsync(int? page, int? pageSize, string? sortBy, string? sortOrder)
        {
            var usersQuery = _context.Users.AsQueryable();

            // Sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                var order = sortOrder?.ToLower() ?? "asc";
                switch (sortBy.ToLower())
                {
                    case "email":
                        usersQuery = order == "desc"
                            ? usersQuery.OrderByDescending(u => u.Email)
                            : usersQuery.OrderBy(u => u.Email);
                        break;
                    case "name":
                        usersQuery = order == "desc"
                            ? usersQuery.OrderByDescending(u => u.Name)
                            : usersQuery.OrderBy(u => u.Name);
                        break;
                    case "age":
                        usersQuery = order == "desc"
                            ? usersQuery.OrderByDescending(u => u.DateOfBirth)
                            : usersQuery.OrderBy(u => u.DateOfBirth);
                        break;
                }
            }

            // Pagination
            if (page.HasValue && pageSize.HasValue)
            {
                usersQuery = usersQuery.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            return await usersQuery.ToListAsync();
        }

        public async Task RemoveAsync(User user)
        {
            _context.Users.Remove(user);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}