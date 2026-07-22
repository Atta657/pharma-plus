using Microsoft.AspNetCore.Mvc;
using PharmaAPI.Data;
using PharmaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace PharmaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StockController(AppDbContext context)
        {
            _context = context;
        }

        // GET STOCK FOR A USER (DISTRIBUTOR OR REP)
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserStock(int userId)
        {
            var stock = await _context.Inventories
                .Where(i => i.DistributorId == userId) // Reusing DistributorId for any User holding stock
                .Join(_context.Medicines,
                    inv => inv.MedicineId,
                    med => med.MedicineId,
                    (inv, med) => new {
                        inv.InventoryId,
                        inv.MedicineId,
                        med.MedicineName,
                        inv.Quantity,
                        med.BatchNumber,
                        med.ExpiryDate
                    })
                .ToListAsync();

            return Ok(stock);
        }

        // ASSIGN STOCK FROM DISTRIBUTOR TO REP
        [HttpPost("assign")]
        public async Task<IActionResult> AssignStock(StockAssignmentDto dto)
        {
            // 1. Check Distributor Stock
            var distInventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.DistributorId == dto.FromDistributorId && i.MedicineId == dto.MedicineId);

            if (distInventory == null || distInventory.Quantity < dto.Quantity)
            {
                return BadRequest("Insufficient stock with distributor");
            }

            // 2. Deduct from Distributor
            distInventory.Quantity -= dto.Quantity;

            // 3. Add to Rep Stock
            var repInventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.DistributorId == dto.ToRepId && i.MedicineId == dto.MedicineId);

            if (repInventory == null)
            {
                repInventory = new Inventory
                {
                    DistributorId = dto.ToRepId,
                    MedicineId = dto.MedicineId,
                    Quantity = dto.Quantity
                };
                _context.Inventories.Add(repInventory);
            }
            else
            {
                repInventory.Quantity += dto.Quantity;
            }

            // 4. Record Transaction
            var transaction = new StockTransaction
            {
                MedicineId = dto.MedicineId,
                CompanyId = dto.CompanyId,
                Quantity = dto.Quantity,
                TransactionType = "Transfer",
                UserId = dto.FromDistributorId,
                Remarks = $"Assigned to Rep ID: {dto.ToRepId}"
            };

            _context.StockTransactions.Add(transaction);

            await _context.SaveChangesAsync();

            return Ok(new { message = "Stock assigned successfully" });
        }

        // ADD STOCK TO DISTRIBUTOR (FROM COMPANY ADMIN)
        [HttpPost("add-to-distributor")]
        public async Task<IActionResult> AddStockToDistributor(StockAssignmentDto dto)
        {
            var inventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.DistributorId == dto.ToRepId && i.MedicineId == dto.MedicineId);

            if (inventory == null)
            {
                inventory = new Inventory
                {
                    DistributorId = dto.ToRepId, // Using ToRepId parameter as recipient
                    MedicineId = dto.MedicineId,
                    Quantity = dto.Quantity
                };
                _context.Inventories.Add(inventory);
            }
            else
            {
                inventory.Quantity += dto.Quantity;
            }

            // Record Transaction
            var transaction = new StockTransaction
            {
                MedicineId = dto.MedicineId,
                CompanyId = dto.CompanyId,
                Quantity = dto.Quantity,
                TransactionType = "Add",
                UserId = dto.FromDistributorId, // Admin who added
                Remarks = "Stock added by Admin"
            };

            _context.StockTransactions.Add(transaction);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Stock added successfully" });
        }
    }

    public class StockAssignmentDto
    {
        public int FromDistributorId { get; set; }
        public int ToRepId { get; set; }
        public int MedicineId { get; set; }
        public int Quantity { get; set; }
        public int CompanyId { get; set; }
    }
}
