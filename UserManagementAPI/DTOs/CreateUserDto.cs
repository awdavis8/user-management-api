using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.DTOs
{
    /// <summary>
    /// Data transfer object for creating a new user.
    /// </summary>
    public class CreateUserDto
    {
        /// <summary>The user's full name. Maximum of 100 characters.</summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>The user's email address. Must be a valid email format.</summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>The user's date of birth.</summary>
        [Required]
        public DateTime DateOfBirth { get; set; }
    }
}
