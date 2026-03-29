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
        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return UserMapper.ToResponseDtos(users);
        }

        /// <inheritdoc />
        public async Task<Result<UserResponseDto>> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
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

            if (await _userRepository.EmailExistsAsync(dto.Email))
                return Result<UserResponseDto>.Failure("A user with this email address already exists.");

            var user = UserMapper.ToModel(dto);
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return Result<UserResponseDto>.Success(UserMapper.ToResponseDto(user));
        }

        /// <inheritdoc />
        public async Task<Result<UserResponseDto>> UpdateUserAsync(int id, UpdateUserDto dto)
        {
            var age = GetAge(dto.DateOfBirth!.Value);
            if (age < 18)
                return Result<UserResponseDto>.Failure("User must be at least 18 years of age.");

            var user = await _userRepository.GetByIdAsync(id);
            if (user is null)
                return Result<UserResponseDto>.Failure("User not found.");

            // Check for email uniqueness (excluding current user)
            if (await _userRepository.EmailExistsAsync(dto.Email, id))
                return Result<UserResponseDto>.Failure("A user with this email address already exists.");

            UserMapper.ApplyUpdate(dto, user);

            await _userRepository.SaveChangesAsync();

            return Result<UserResponseDto>.Success(UserMapper.ToResponseDto(user));
        }

        /// <inheritdoc />
        public async Task<Result> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user is null)
                return Result.Failure("User not found.");

            // Assuming you add a Remove method to the repository:
            await _userRepository.RemoveAsync(user);
            await _userRepository.SaveChangesAsync();
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
