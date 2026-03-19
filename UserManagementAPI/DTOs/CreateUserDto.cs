using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.DTOs
{
    /// <summary>
    /// Data transfer object for creating a new user.
    /// </summary>
    public class CreateUserDto
    {
        /// <summary>The user's full name. Maximum of 100 characters.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>The user's email address. Must be a valid email format.</summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>The user's date of birth. The user must be at least 18 years of age.</summary>
        public DateTime DateOfBirth { get; set; }
    }
}
