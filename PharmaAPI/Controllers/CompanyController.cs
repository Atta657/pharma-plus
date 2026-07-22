using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaAPI.Data;
using PharmaAPI.Models;
using System.Threading.Tasks;

namespace PharmaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CompanyController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterCompany(Company company)
        {
            if (string.IsNullOrWhiteSpace(company.CompanyName))
                return BadRequest("Company Name is required");

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            return Ok(company);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCompanies()
        {
            var companies = await _context.Companies.ToListAsync();
            return Ok(companies);
        }
    }
}
