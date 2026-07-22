using Microsoft.AspNetCore.Mvc;
using PharmaAPI.Data;
using PharmaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace PharmaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // REGISTER USER
        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            if (user.CompanyId <= 0)
                return BadRequest("Invalid Company ID. Please provide a valid company ID for registration.");

            var companyExists = await _context.Companies.AnyAsync(c => c.CompanyId == user.CompanyId);
            if (!companyExists)
                return BadRequest("Company not found. Please register your company first or provide a valid ID.");

            var exists = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == user.Email);

            if (exists != null)
                return BadRequest("User already exists");

            // HASH PASSWORD
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            // Auto-approve logic:
            // 1. First user of the company
            // 2. First Admin of the company (if no approved Admin exists)
            bool companyHasUsers = await _context.Users.AnyAsync(u => u.CompanyId == user.CompanyId);
            bool companyHasApprovedAdmin = await _context.Users.AnyAsync(u => u.CompanyId == user.CompanyId && u.Role == "Admin" && u.IsApproved);

            if (!companyHasUsers || (user.Role == "Admin" && !companyHasApprovedAdmin))
            {
                user.IsApproved = true;
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        // LOGIN
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
                return Unauthorized("Invalid credentials");

            bool validPassword =
                BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);

            if (!validPassword)
                return Unauthorized("Invalid credentials");

            if (!user.IsApproved)
                return Unauthorized("Your account is pending approval from your company admin.");

            return Ok(new
            {
                user.UserId,
                user.CompanyId,
                user.FullName,
                user.Email,
                user.Role
            });
        }

        // GET PENDING USERS (FOR ADMIN)
        [HttpGet("pending-users/{companyId}")]
        public async Task<IActionResult> GetPendingUsers(int companyId)
        {
            var users = await _context.Users
                .Where(u => u.CompanyId == companyId && !u.IsApproved)
                .ToListAsync();
            return Ok(users);
        }

        // GET USERS BY ROLE (FOR DROPDOWNS)
        [HttpGet("users/{companyId}/role/{role}")]
        public async Task<IActionResult> GetUsersByRole(int companyId, string role)
        {
            var users = await _context.Users
                .Where(u => u.CompanyId == companyId && u.Role == role && u.IsApproved)
                .Select(u => new { u.UserId, u.FullName, u.Email })
                .ToListAsync();
            return Ok(users);
        }

        // APPROVE USER
        [HttpPost("approve-user/{userId}")]
        public async Task<IActionResult> ApproveUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound("User not found");

            user.IsApproved = true;
            await _context.SaveChangesAsync();
            return Ok(new { message = "User approved successfully" });
        }
    }

    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}