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

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsers([FromQuery] PaginationParamsDto pagenationParams)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _userService.GetUsersAsync(
                    pagenationParams.Page,
                    pagenationParams.PageSize,
                    pagenationParams.SortBy,
                    pagenationParams.SortOrder
                );

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ProblemDetails
                {
                    Title = "An unexpected error occurred.",
                    Detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="dto">The data for the new user.</param>
        /// <returns>A 201 Created response with the new user, or a 400 Bad Request with an error message.</returns>
        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> CreateUser(CreateUserDto dto)
        {
            try
            {
                var result = await _userService.CreateUserAsync(dto);
                if (result.IsFailure)
                    return BadRequest(new ValidationProblemDetails(result.Errors));

                return CreatedAtAction(nameof(GetUsers), result.Value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ProblemDetails
                {
                    Title = "An unexpected error occurred.",
                    Detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="id">The user's unique identifier.</param>
        /// <returns>A 200 OK response with the user if found, or a 404 Not Found with an error message.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUserById(int id)
        {
            try
            {
                var result = await _userService.GetUserByIdAsync(id);
                if (result.IsFailure)
                    return NotFound(new ValidationProblemDetails(result.Errors));

                return Ok(result.Value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ProblemDetails
                {
                    Title = "An unexpected error occurred.",
                    Detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Deletes a user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <returns>A Result indicating success or failure with an error message.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(id);
                if (result.IsFailure)
                    return NotFound(new ValidationProblemDetails(result.Errors));

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ProblemDetails
                {
                    Title = "An unexpected error occurred.",
                    Detail = ex.Message
                });
            }
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
            try
            {
                var result = await _userService.UpdateUserAsync(id, dto);
                if (result.IsFailure)
                {
                    if (result.Errors != null && result.Errors.ContainsKey("Id"))
                        return NotFound(new ValidationProblemDetails(result.Errors));
                    return BadRequest(new ValidationProblemDetails(result.Errors));
                }

                return Ok(result.Value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ProblemDetails
                {
                    Title = "An unexpected error occurred.",
                    Detail = ex.Message
                });
            }
        }
    }
}
