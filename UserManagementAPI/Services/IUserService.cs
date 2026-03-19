using UserManagementAPI.DTOs;
using UserManagementAPI.Models;

namespace UserManagementAPI.Services
{
    /// <summary>
    /// Defines the contract for user-related operations.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>A collection of UserResponseDto objects.</returns>
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="id">The user's unique identifier.</param>
        /// <returns>A Result containing the UserResponseDto if found, or a failure message if not.</returns>
        Task<Result<UserResponseDto>> GetUserByIdAsync(int id);

        /// <summary>
        /// Validates and creates a new user.
        /// </summary>
        /// <param name="dto">The data for the new user.</param>
        /// <returns>A Result containing the created UserResponseDto, or a validation failure.</returns>
        Task<Result<UserResponseDto>> CreateUserAsync(CreateUserDto dto);

        /// <summary>
        /// Validates and updates an existing user.
        /// </summary>
        /// <param name="id">The unique identifier of the user to update.</param>
        /// <param name="dto">The updated user data.</param>
        /// <returns>A Result containing the updated UserResponseDto, or a validation/not-found failure.</returns>
        Task<Result<UserResponseDto>> UpdateUserAsync(int id, UpdateUserDto dto);

        /// <summary>
        /// Deletes a user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <returns>A Result to indicate success/failure in deletion.</returns>
        Task<Result> DeleteUserAsync(int id);
    }
}