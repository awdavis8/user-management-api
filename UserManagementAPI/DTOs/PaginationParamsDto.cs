using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.DTOs
{
    public class PaginationParamsDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page must be a positive integer.")]
        public int Page { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "PageSize must be between 1 and 100.")]
        public int PageSize { get; set; } = 10;

        [AllowedValues(["name", "email", "createdAt"], ErrorMessage = "sortBy must be 'name', 'email', or 'createdAt'.")]
        public string? SortBy { get; set; }

        [AllowedValues(["asc", "desc"], ErrorMessage = "sortOrder must be 'asc', 'desc'.")]
        public string? SortOrder { get; set; }
    }
}
