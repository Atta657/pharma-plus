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
    public class AlertController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AlertController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Alert/company/5
        [HttpGet("company/{companyId}")]
        public async Task<ActionResult<IEnumerable<Alert>>> GetAlertsByCompany(int companyId)
        {
            return await _context.Alerts.Where(a => a.CompanyId == companyId).OrderByDescending(a => a.CreatedAt).ToListAsync();
        }
    }
}
