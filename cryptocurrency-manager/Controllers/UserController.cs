using cryptocurrency_manager.DataContext.Dtos;
using cryptocurrency_manager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace cryptocurrency_manager.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("POST/api/users/register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            if (userRegisterDto == null)
            {
                return BadRequest("Invalid user data.");
            }
            var result = await _userService.RegisterAsync(userRegisterDto);
            return Ok(result);
        }

        [HttpPost]
        [Route("POST/api/users/login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            if (userLoginDto == null)
            {
                return BadRequest("Invalid login data.");
            }
            var token = await _userService.LoginAsync(userLoginDto);
            if (token == null)
            {
                return Unauthorized();
            }
            return Ok(new { Token = token });
        }

        [HttpGet]
        [Route("GET/api/users/{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet]
        [Route("GET/api/myuser")]
        [Authorize(Roles = "User")]

        public async Task<IActionResult> GetMyUser()
        {
            var userId = int.Parse(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet]
        [Route("GET/api/profit/{userId}")]
        public async Task<IActionResult> GetUserProfit(int userId)
        {
            var profit = await _userService.GetUserProfitAsync(userId);
            if (profit == null)
            {
                return NotFound();
            }
            return Ok(profit);
        }

        [HttpGet]
        [Route("GET/api/myprofit")]
        [Authorize(Roles = "User")]

        public async Task<IActionResult> GetMyProfit()
        {
            var userId = int.Parse(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var profit = await _userService.GetUserProfitAsync(userId);
            if (profit == null)
            {
                return NotFound();
            }
            return Ok(profit);
        }

        [HttpGet]
        [Route("GET/api/profit/details/{userId}")]
        public async Task<IActionResult> GetUserDetailedProfit(int userId)
        {
            var profitDetails = await _userService.GetUserDetailedProfitAsync(userId);
            if (profitDetails == null)
            {
                return NotFound();
            }
            return Ok(profitDetails);
        }

        [HttpGet]
        [Route("GET/api/myprofit/details")]
        [Authorize(Roles = "User")]

        public async Task<IActionResult> GetMyDetailedProfit()
        {
            var userId = int.Parse(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var profitDetails = await _userService.GetUserDetailedProfitAsync(userId);
            if (profitDetails == null)
            {
                return NotFound();
            }
            return Ok(profitDetails);
        }

        [HttpPut]
        [Route("PUT/api/users/{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UserUpdateDto userUpdateDto)
        {
            if (userUpdateDto == null)
            {
                return BadRequest("Invalid user data.");
            }
            var updatedUser = await _userService.UpdateUserAsync(userId, userUpdateDto);
            if (updatedUser == null)
            {
                return NotFound();
            }
            return Ok(updatedUser);
        }

        [HttpPut]
        [Route("PUT/api/user")]
        [Authorize(Roles = "User")]

        public async Task<IActionResult> UpdateMyUser([FromBody] UserUpdateDto userUpdateDto)
        {
            if (userUpdateDto == null)
            {
                return BadRequest("Invalid user data.");
            }
            var userId = int.Parse(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

            var updatedUser = await _userService.UpdateUserAsync(userId, userUpdateDto);
            if (updatedUser == null)
            {
                return NotFound();
            }
            return Ok(updatedUser);
        }

        [HttpDelete]
        [Route("DELETE/api/users/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var result = await _userService.DeleteUserAsync(userId);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete]
        [Route("DELETE/api/user")]
        [Authorize(Roles = "User")]

        public async Task<IActionResult> DeleteMyUser()
        {
            var userId = int.Parse(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var result = await _userService.DeleteUserAsync(userId);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }


    }
}
