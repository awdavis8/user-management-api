using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.DTOs
{
    /// <summary>
    /// Data transfer object for updating an existing user.
    /// </summary>
    public class UpdateUserDto
    {
        [Required]
        [MaxLength(100)]
        /// <summary>The user's full name. Maximum 100 characters.</summary>
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        /// <summary>The user's email address. Must be a valid email format.</summary>
        public string Email { get; set; } = string.Empty;

        [Required]
        /// <summary>The user's date of birth. The user must be at least 18 years of age.</summary>
        public DateTime? DateOfBirth { get; set; }
    }
}
