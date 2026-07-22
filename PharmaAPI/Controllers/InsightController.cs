using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaAPI.Data;
using PharmaAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsightController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InsightController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Insight/company/5
        [HttpGet("company/{companyId}")]
        public async Task<ActionResult<IEnumerable<AIInsight>>> GetInsightsByCompany(int companyId)
        {
            return await _context.AIInsights.Where(i => i.CompanyId == companyId).OrderByDescending(i => i.GeneratedAt).ToListAsync();
        }
    }
}
