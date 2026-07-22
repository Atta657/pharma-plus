using Microsoft.AspNetCore.Mvc;
using PharmaAPI.Data;
using PharmaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace PharmaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InventoryController(AppDbContext context)
        {
            _context = context;
        }

        // Add Stock
        [HttpPost("add")]
        public async Task<IActionResult> AddStock(Inventory inventory)
        {
            _context.Inventories.Add(inventory);
            await _context.SaveChangesAsync();
            return Ok(inventory);
        }

        // Get Distributor Inventory
        [HttpGet("distributor/{distributorId}")]
        public async Task<IActionResult> GetInventory(int distributorId)
        {
            var stock = await _context.Inventories
                .Where(i => i.DistributorId == distributorId)
                .ToListAsync();

            return Ok(stock);
        }
    }
}