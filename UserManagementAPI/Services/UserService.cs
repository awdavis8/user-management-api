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
        public async Task<UserResponseDto?> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user is null ? null : UserMapper.ToResponseDto(user);
        }

        /// <inheritdoc />
        public async Task<Result<UserResponseDto>> CreateUserAsync(CreateUserDto dto)
        {
            var validation = ValidateCreateUser(dto);
            if (validation.IsFailure)
                return Result<UserResponseDto>.Failure(validation.Error);

            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return Result<UserResponseDto>.Failure("A user with this email address already exists.");

            var user = UserMapper.ToModel(dto);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Result<UserResponseDto>.Success(UserMapper.ToResponseDto(user));
        }

        /// <inheritdoc />
        public Task<Result<UserResponseDto>> UpdateUserAsync(int id, UpdateUserDto dto)
        {
            var validation = ValidateUpdateUser(dto);
            if (validation.IsFailure)
                return Task.FromResult(Result<UserResponseDto>.Failure(validation.Error));

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

        /// <summary>
        /// Validates the fields of a <see cref="CreateUserDto"/>.
        /// </summary>
        /// <param name="dto">The DTO to validate.</param>
        /// <returns>A successful Result if valid; otherwise, a failure Result with an error message.</returns>
        private static Result ValidateCreateUser(CreateUserDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return Result.Failure("Name is required.");

            if (dto.Name.Length > 100)
                return Result.Failure("Name must not exceed 100 characters.");

            if (string.IsNullOrWhiteSpace(dto.Email))
                return Result.Failure("Email is required.");

            if (!IsValidEmail(dto.Email))
                return Result.Failure("Email must be a valid email address.");

            if (dto.DateOfBirth == default || dto.DateOfBirth == DateTime.MinValue)
                return Result.Failure("Date of birth is required.");

            var age = GetAge(dto.DateOfBirth);
            if (age < 18)
                return Result.Failure("User must be at least 18 years of age.");

            return Result.Success();
        }

        /// <summary>
        /// Validates the fields of an <see cref="UpdateUserDto"/>.
        /// </summary>
        /// <param name="dto">The DTO to validate.</param>
        /// <returns>A successful Result if valid; otherwise, a failure Result with an error message.</returns>
        private static Result ValidateUpdateUser(UpdateUserDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return Result.Failure("Name is required.");

            if (dto.Name.Length > 100)
                return Result.Failure("Name must not exceed 100 characters.");

            if (string.IsNullOrWhiteSpace(dto.Email))
                return Result.Failure("Email is required.");

            if (!IsValidEmail(dto.Email))
                return Result.Failure("Email must be a valid email address.");

            if (dto.DateOfBirth == default || dto.DateOfBirth == DateTime.MinValue)
                return Result.Failure("Date of birth is required.");

            var age = GetAge(dto.DateOfBirth);
            if (age < 18)
                return Result.Failure("User must be at least 18 years of age.");

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

        /// <summary>
        /// Determines whether the specified string is a valid email address.
        /// </summary>
        /// <param name="email">The email address to validate.</param>
        /// <returns>true if the email is valid; otherwise, false.</returns>
        private static bool IsValidEmail(string email)
        {
            try
            {
                var address = new MailAddress(email);
                return address.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
