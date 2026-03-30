using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Data;
using UserManagementAPI.DTOs;
using UserManagementAPI.Mappings;
using UserManagementAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.Services
{
    /// <inheritdoc />
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of UserService.
        /// </summary>
        /// <param name="userRepository">The repository for user operations.</param>
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <inheritdoc />
        public async Task<Result<UserResponseDto>> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user is null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { "Id", ["User not found."] }
                };
                return Result<UserResponseDto>.Failure(errors);
            }

            return Result<UserResponseDto>.Success(UserMapper.ToResponseDto(user));
        }

        /// <inheritdoc />
        public async Task<Result<UserResponseDto>> CreateUserAsync(CreateUserDto dto)
        {
            var errors = new Dictionary<string, string[]>();

            // Email uniqueness
            if (await _userRepository.EmailExistsAsync(dto.Email))
                errors["Email"] = ["A user with this email address already exists."];

            // Age validation
            var age = GetAge(dto.DateOfBirth!.Value);
            if (age < 18)
                errors["DateOfBirth"] = ["User must be at least 18 years of age."];

            if (errors.Count > 0)
                return Result<UserResponseDto>.Failure(errors);

            var user = UserMapper.ToModel(dto);
            await _userRepository.AddAsync(user);

            return Result<UserResponseDto>.Success(UserMapper.ToResponseDto(user));
        }

        /// <inheritdoc />
        public async Task<Result<UserResponseDto>> UpdateUserAsync(int id, UpdateUserDto dto)
        {
            var errors = new Dictionary<string, string[]>();

            // Email uniqueness
            if (await _userRepository.EmailExistsAsync(dto.Email, id))
                errors["Email"] = ["A user with this email address already exists."];

            // Age validation
            var age = GetAge(dto.DateOfBirth!.Value);
            if (age < 18)
                errors["DateOfBirth"] = ["User must be at least 18 years of age."];

            var user = await _userRepository.GetByIdAsync(id);
            if (user is null)
                errors["Id"] = ["User not found."];

            if (errors.Count > 0)
                return Result<UserResponseDto>.Failure(errors);

            UserMapper.ApplyUpdate(dto, user!);

            return Result<UserResponseDto>.Success(UserMapper.ToResponseDto(user!));
        }

        /// <inheritdoc />
        public async Task<Result> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user is null)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { "Id", ["User not found."] }
                };
                return Result.Failure(errors);
            }

            await _userRepository.RemoveAsync(user);
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
            var users = await _userRepository.GetUsersAsync(page, pageSize, sortBy, sortOrder);
            return UserMapper.ToResponseDtos(users);
        }
    }
}
