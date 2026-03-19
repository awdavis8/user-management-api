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
        /// TODO: Add pagenation and filtering parameters to this endpoint.
        /// Retrieves all users.
        /// </summary>
        /// <returns>A 200 OK response containing a list of users.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
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
                return BadRequest(new { result.Error });

            return CreatedAtAction(nameof(GetUsers), result.Value);
        }
    }
}
