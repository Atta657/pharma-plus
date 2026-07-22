using Microsoft.AspNetCore.Mvc;
using PharmaAPI.Data;
using PharmaAPI.Models;

namespace PharmaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VisitController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VisitController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("track")]
        public async Task<IActionResult> TrackVisit(Visit visit)
        {
            // Future: Geo-Fence AI verification
            visit.IsVerified = true;

            _context.Visits.Add(visit);
            await _context.SaveChangesAsync();

            return Ok("Visit Recorded Successfully");
        }
    }
}