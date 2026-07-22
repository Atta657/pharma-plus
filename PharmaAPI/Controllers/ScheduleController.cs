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
    public class ScheduleController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ScheduleController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Schedule/rep/10
        [HttpGet("rep/{repId}")]
        public async Task<ActionResult<IEnumerable<Schedule>>> GetSchedulesByRep(int repId)
        {
            return await _context.Schedules.Where(s => s.RepId == repId).OrderBy(s => s.ScheduledDate).ToListAsync();
        }

        // GET: api/Schedule/company/1
        [HttpGet("company/{companyId}")]
        public async Task<ActionResult<IEnumerable<Schedule>>> GetSchedulesByCompany(int companyId)
        {
            return await _context.Schedules.Where(s => s.CompanyId == companyId).OrderByDescending(s => s.ScheduledDate).ToListAsync();
        }
        
        [HttpPost]
        public async Task<ActionResult<Schedule>> PostSchedule(Schedule schedule)
        {
            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();
            return Ok(schedule);
        }
    }
}
