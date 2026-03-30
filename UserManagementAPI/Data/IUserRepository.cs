using UserManagementAPI.Models;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<IEnumerable<User>> GetAllAsync();
    Task AddAsync(User user);
    Task<bool> EmailExistsAsync(string email, int? excludeUserId = null);
    Task SaveChangesAsync();
    Task RemoveAsync(User user);
    Task<IEnumerable<User>> GetUsersAsync(int? page, int? pageSize, string? sortBy, string? sortOrder);
}