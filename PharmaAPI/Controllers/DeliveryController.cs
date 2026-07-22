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
    public class DeliveryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DeliveryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Delivery/order/5
        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<IEnumerable<DeliveryTracking>>> GetDeliveryTrackingsByOrder(int orderId)
        {
            return await _context.DeliveryTrackings.Where(dt => dt.OrderId == orderId).ToListAsync();
        }

        // GET: api/Delivery/latest-locations/1
        [HttpGet("latest-locations/{companyId}")]
        public async Task<IActionResult> GetLatestLocations(int companyId)
        {
            var repIds = await _context.Users
                .Where(u => u.CompanyId == companyId && u.Role == "Rep")
                .Select(u => u.UserId)
                .ToListAsync();

            var latestLocations = await _context.DeliveryTrackings
                .Where(dt => repIds.Contains(dt.UserId))
                .GroupBy(dt => dt.UserId)
                .Select(g => g.OrderByDescending(dt => dt.UpdatedAt).FirstOrDefault())
                .ToListAsync();
            
            // Join with user info
            var result = from loc in latestLocations
                         join user in _context.Users on loc.UserId equals user.UserId
                         select new
                         {
                             user.FullName,
                             user.UserId,
                             loc.Latitude,
                             loc.Longitude,
                             loc.UpdatedAt,
                             loc.Status
                         };

            return Ok(result);
        }

        // POST: api/Delivery
        [HttpPost]
        public async Task<ActionResult<DeliveryTracking>> PostTracking(DeliveryTracking tracking)
        {
            // Upsert: If tracking exists for this UserId (without specific order associated or same order), update it
            var existing = await _context.DeliveryTrackings
                .FirstOrDefaultAsync(dt => dt.UserId == tracking.UserId && dt.OrderId == tracking.OrderId);

            if (existing != null)
            {
                existing.Latitude = tracking.Latitude;
                existing.Longitude = tracking.Longitude;
                existing.Status = tracking.Status;
                existing.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return Ok(existing);
            }

            _context.DeliveryTrackings.Add(tracking);
            await _context.SaveChangesAsync();
            return Ok(tracking);
        }
    }
}
