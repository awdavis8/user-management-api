namespace UserManagementAPI.DTOs
{
    /// <summary>
    /// Data transfer object returned to clients representing a user.
    /// </summary>
    public class UserResponseDto
    {
        /// <summary>The unique identifier for the user.</summary>
        public int Id { get; set; }

        /// <summary>The user's full name.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>The user's email address. This must be unique.</summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>The user's date of birth.</summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>The UTC timestamp when the user was created.</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>The UTC timestamp when the user was last updated.</summary>
        public DateTime UpdatedAt { get; set; }
    }
}
