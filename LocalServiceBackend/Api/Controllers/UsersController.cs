using Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Trial;

namespace Api.Controllers
{
    public class RegisterDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class UserResponseDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public string IsActive { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public UsersController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Role = dto.Role,
                CreatedAt = DateTime.UtcNow,
                IsActive = "yes"
            };
            try
            {
                var createdUser = await _userService.InsertUserAsync(user, dto.Password);
                var response = new UserResponseDto {UserId=createdUser.UserId,FullName= createdUser.FullName,Email=createdUser.Email,PhoneNumber=createdUser.PhoneNumber,Role=createdUser.Role };
                var token = GenerateJwtToken(user);
                return Ok(new { token, user = new UserResponseDto { UserId = createdUser.UserId, FullName = createdUser.FullName, Email = createdUser.Email, PhoneNumber = createdUser.PhoneNumber, Role = createdUser.Role } });
            }
            catch (Exception e)
            {
                return BadRequest("User creation failed because: " + e.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _userService.LoginAsync(dto.Email, dto.Password);
            if (user == null)
                return Unauthorized("Invalid email or password");
            var token = GenerateJwtToken(user);

            return Ok(new
            {
                token,
                user = new UserResponseDto
                {
                    UserId = user.UserId,
                    FullName = user.FullName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = user.Role,
                    IsActive = user.IsActive
                }
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            var result = users.Select(u => new UserResponseDto
            {
                UserId = u.UserId,
                FullName = u.FullName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Role = u.Role,
                IsActive = u.IsActive
            });

            return Ok(result);
        }

        [HttpGet("Role/{role}")]
        public async Task<IActionResult> GetByRole(string role)
        {
            if (role!="Customer"&&role!="Service Provider")
                return BadRequest("Role does not exist");
            var users = await _userService.GetUsersByRoleAsync(role);
            var result = users.Select(u => new UserResponseDto
            {
                UserId = u.UserId,
                FullName = u.FullName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Role = u.Role,
                IsActive = u.IsActive
            });
            return Ok(result);

        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(new UserResponseDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                IsActive = user.IsActive
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("activate/{id}")]
        public async Task<IActionResult> Activate(int id)
        {
            var result = await _userService.ActivateUserAsync(id);
            if (!result)
                return NotFound();

            return Ok("User activated");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("deactivate/{id}")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var result = await _userService.DeactivateUserAsync(id);

            if (!result)
                return NotFound();

            return Ok("User deactivated");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.DeleteUserAsync(id);

            if (!result)
                return NotFound();

            return Ok("User deleted");
        }

        // JWT GENERATION
        // ===========================
        private string GenerateJwtToken(User user)
        {
            var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };
            var key = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes("THIS_IS_VERY_SECRET_KEY_123456789"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "LocalServicesAPI",
                audience: "LocalServicesUser",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        
        [HttpGet("Customer/{customerId}")]
        public async Task<IActionResult> GetUserByCustomerId(int customerId)
        {
            try
            {
                var customerService = new CustomerService();
                var customer = await customerService.GetCustomerByIdAsync(customerId);

                var user = await _userService.GetUserByIdAsync(customer.UserId);

                return Ok(new { user.UserId, user.FullName, user.PhoneNumber });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
          
        }
    }
}
