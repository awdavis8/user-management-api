using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Data;
using UserManagementAPI.DTOs;
using UserManagementAPI.Mappings;
using UserManagementAPI.Models;

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
        public async Task<Result<UserResponseDto>> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null)
                return Result<UserResponseDto>.Failure("User not found.");

            return Result<UserResponseDto>.Success(UserMapper.ToResponseDto(user));
        }

        /// <inheritdoc />
        public async Task<Result<UserResponseDto>> CreateUserAsync(CreateUserDto dto)
        {
            var age = GetAge(dto.DateOfBirth!.Value);
            if (age < 18)
                return Result<UserResponseDto>.Failure("User must be at least 18 years of age.");

            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return Result<UserResponseDto>.Failure("A user with this email address already exists.");

            var user = UserMapper.ToModel(dto);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Result<UserResponseDto>.Success(UserMapper.ToResponseDto(user));
        }

        /// <inheritdoc />
        public async Task<Result<UserResponseDto>> UpdateUserAsync(int id, UpdateUserDto dto)
        {
            var age = GetAge(dto.DateOfBirth!.Value);
            if (age < 18)
                return Result<UserResponseDto>.Failure("User must be at least 18 years of age.");

            var user = await _context.Users.FindAsync(id);
            if (user is null)
                return Result<UserResponseDto>.Failure("User not found.");

            // Check for email uniqueness (excluding current user)
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email && u.Id != id))
                return Result<UserResponseDto>.Failure("A user with this email address already exists.");

            UserMapper.ApplyUpdate(dto, user);

            await _context.SaveChangesAsync();

            return Result<UserResponseDto>.Success(UserMapper.ToResponseDto(user));
        }

        /// <inheritdoc />
        public async Task<Result> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null)
                return Result.Failure("User not found.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Result.Success();
        }

        /// <summary>
        /// Calculates a person's age in whole years from their date of birth.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <returns>The age in whole years.</returns>
        private static int GetAge(DateTime dateOfBirth)
        {
            var today = DateTime.UtcNow;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age).Date)
                age--;
            return age;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<UserResponseDto>> GetUsersAsync(
            int? page, int? pageSize, string? sortBy, string? sortOrder)
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

            var users = await usersQuery.ToListAsync();
            return UserMapper.ToResponseDtos(users);
        }
    }
}
