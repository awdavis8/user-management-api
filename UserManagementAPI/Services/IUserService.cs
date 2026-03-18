using UserManagementAPI.DTOs;

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
        /// <returns>A UserResponseDto if found; otherwise, null.</returns>
        Task<UserResponseDto?> GetUserByIdAsync(int id);

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="dto">The data for the new user.</param>
        /// <returns>A UserResponseDto representing the created user.</returns>
        Task<UserResponseDto> CreateUserAsync(CreateUserDto dto);

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">The unique identifier of the user to update.</param>
        /// <param name="dto">The updated user data.</param>
        /// <returns>A UserResponseDto if found and updated; otherwise, null.</returns>
        Task<UserResponseDto?> UpdateUserAsync(int id, UpdateUserDto dto);

        /// <summary>
        /// Deletes a user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <returns>True if the user was deleted; false if not found.</returns>
        Task<bool> DeleteUserAsync(int id);

        /// <summary>
        /// Checks whether a user with the specified email address already exists.
        /// </summary>
        /// <param name="email">The email address to check.</param>
        /// <returns>True if the email exists; otherwise, false.</returns>
        Task<bool> EmailExistsAsync(string email);
    }
}