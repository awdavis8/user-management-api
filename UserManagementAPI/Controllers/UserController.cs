using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.DTOs;
using UserManagementAPI.Services;

namespace UserManagementAPI.Controllers
{
    /// <summary>
    /// API controller for managing users.
    /// </summary>
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of UserController.
        /// </summary>
        /// <param name="userService">The user service for handling business logic.</param>
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Retrieves pagenated users, or all users if pagination parameters are not provided. 
        /// Supports sorting by name, email, or age in ascending or descending order.
        /// </summary>
        /// <param name="page">The page number for pagination.</param>
        /// <param name="pageSize">The number of users per page for pagination.</param>
        /// <param name="sortBy">The property to sort the users by.</param>
        /// <param name="sortOrder">The order of sorting (ascending or descending).</param>
        /// <returns>A 200 OK response containing a list of users.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsers(
            int? page = null,
            int? pageSize = null,
            string? sortBy = null,
            string? sortOrder = null)
        {
            // Validate sortOrder
            if (!string.IsNullOrEmpty(sortOrder) && sortOrder.ToLower() != "asc" && sortOrder.ToLower() != "desc")
                return BadRequest(new ProblemDetails { Detail = "Invalid sortOrder. Must be 'asc' or 'desc'." });

            // Validate sortBy
            if (!string.IsNullOrEmpty(sortBy) &&
                sortBy.ToLower() != "email" &&
                sortBy.ToLower() != "name" &&
                sortBy.ToLower() != "age")
                return BadRequest(new ProblemDetails { Detail = "Invalid sortBy. Must be 'name', 'email', or 'age'." });

            var result = await _userService.GetUsersAsync(page, pageSize, sortBy, sortOrder);

            return Ok(result);
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="dto">The data for the new user.</param>
        /// <returns>A 201 Created response with the new user, or a 400 Bad Request with an error message.</returns>
        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> CreateUser(CreateUserDto dto)
        {
            var result = await _userService.CreateUserAsync(dto);
            if (result.IsFailure)
                return BadRequest(new ProblemDetails { Detail = result.Error });

            return CreatedAtAction(nameof(GetUsers), result.Value);
        }

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="id">The user's unique identifier.</param>
        /// <returns>A 200 OK response with the user if found, or a 404 Not Found with an error message.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUserById(int id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            if (result.IsFailure)
                return NotFound(new ProblemDetails { Detail = result.Error });

            return Ok(result.Value);
        }

        /// <summary>
        /// Deletes a user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <returns>A Result indicating success or failure with an error message.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (result.IsFailure)
                return NotFound(new ProblemDetails { Detail = result.Error });

            return NoContent();
        }

        /// <summary>
        /// Updates an existing user by their unique identifier.
        /// </summary>
        /// <param name="id">The user's unique identifier.</param>
        /// <param name="dto">The updated user data.</param>
        /// <returns>A 200 OK response with the updated user, or a 400/404 with an error message.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<UserResponseDto>> UpdateUser(int id, UpdateUserDto dto)
        {
            var result = await _userService.UpdateUserAsync(id, dto);
            if (result.IsFailure)
            {
                if (result.Error == "User not found.")
                    return NotFound(new ProblemDetails { Detail = result.Error });
                return BadRequest(new ProblemDetails { Detail = result.Error });
            }

            return Ok(result.Value);
        }
    }
}
