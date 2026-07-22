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
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Order/company/5
        [HttpGet("company/{companyId}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByCompany(int companyId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.CompanyId == companyId)
                .ToListAsync();
        }

        // PATCH: api/Order/5/assign-delivery
        [HttpPatch("{id}/assign-delivery")]
        public async Task<IActionResult> AssignDelivery(int id, [FromBody] int deliveryRepId)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            order.AssignedDeliveryRepId = deliveryRepId;
            order.Status = "Assigned";
            
            await _context.SaveChangesAsync();
            return Ok(order);
        }
    }
}
