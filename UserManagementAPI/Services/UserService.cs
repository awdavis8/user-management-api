using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Data;
using UserManagementAPI.DTOs;
using UserManagementAPI.Mappings;

namespace UserManagementAPI.Services
{
    /// <inheritdoc />
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of UserService.
        /// </summary>
        /// <param name="context">The database context for user operations.</param>
        public UserService(AppDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return UserMapper.ToResponseDtos(users);
        }

        /// <inheritdoc />
        public async Task<UserResponseDto?> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user is null ? null : UserMapper.ToResponseDto(user);
        }

        /// <inheritdoc />
        public Task<UserResponseDto> CreateUserAsync(CreateUserDto dto)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<UserResponseDto?> UpdateUserAsync(int id, UpdateUserDto dto)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<bool> DeleteUserAsync(int id)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<bool> EmailExistsAsync(string email)
        {
            throw new NotImplementedException();
        }
    }
}
