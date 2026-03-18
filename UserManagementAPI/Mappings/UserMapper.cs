using UserManagementAPI.DTOs;
using UserManagementAPI.Models;

namespace UserManagementAPI.Mappings
{
    /// <summary>
    /// Provides static methods for mapping between User entities and DTOs.
    /// </summary>
    public static class UserMapper
    {
        /// <summary>
        /// Maps a User entity to a UserResponseDto.
        /// </summary>
        /// <param name="user">The user entity to map.</param>
        /// <returns>A UserResponseDto representing the user.</returns>
        public static UserResponseDto ToResponseDto(User user)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }

        /// <summary>
        /// Maps a collection of User entities to UserResponseDto objects.
        /// </summary>
        /// <param name="users">The user entities to map.</param>
        /// <returns>An enumerable of UserResponseDto objects.</returns>
        public static IEnumerable<UserResponseDto> ToResponseDtos(IEnumerable<User> users)
        {
            return users.Select(ToResponseDto);
        }

        /// <summary>
        /// Maps a CreateUserDto to a new User entity.
        /// </summary>
        /// <param name="dto">The DTO containing the new user's data.</param>
        /// <returns>A new User entity.</returns>
        public static User ToModel(CreateUserDto dto)
        {
            return new User(dto.Name, dto.Email, dto.DateOfBirth);
        }

        /// <summary>
        /// Applies the values from an UpdateUserDto to an existing User entity.
        /// Also updates the UpdatedAt timestamp to the current time.
        /// </summary>
        /// <param name="dto">The DTO containing the updated data.</param>
        /// <param name="user">The existing user entity to update.</param>
        public static void ApplyUpdate(UpdateUserDto dto, User user)
        {
            user.Name = dto.Name;
            user.Email = dto.Email;
            user.DateOfBirth = dto.DateOfBirth;
            user.UpdatedAt = DateTime.UtcNow;
        }
    }
}