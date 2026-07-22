using Microsoft.AspNetCore.Mvc;
using PharmaAPI.Data;
using PharmaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace PharmaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicineController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MedicineController(AppDbContext context)
        {
            _context = context;
        }

        // Add Medicine and optionally assign initial stock
        [HttpPost("add")]
        public async Task<IActionResult> AddMedicine(MedicineWithStockDto dto)
        {
            var medicine = new Medicine
            {
                CompanyId = dto.CompanyId,
                MedicineName = dto.MedicineName,
                BatchNumber = dto.BatchNumber,
                ExpiryDate = (DateTime)dto.ExpiryDate,
                Price = dto.Price
            };

            _context.Medicines.Add(medicine);
            await _context.SaveChangesAsync();

            // If a distributor is specified, add into their inventory
            if (dto.InitialDistributorId.HasValue && dto.InitialQuantity > 0)
            {
                var inventory = new Inventory
                {
                    DistributorId = dto.InitialDistributorId.Value,
                    MedicineId = medicine.MedicineId,
                    Quantity = dto.InitialQuantity
                };
                _context.Inventories.Add(inventory);

                // Record Transaction
                _context.StockTransactions.Add(new StockTransaction
                {
                    MedicineId = medicine.MedicineId,
                    CompanyId = dto.CompanyId,
                    Quantity = dto.InitialQuantity,
                    TransactionType = "Add",
                    UserId = dto.InitialDistributorId,
                    Remarks = "Initial stock added by Admin"
                });

                await _context.SaveChangesAsync();
            }

            return Ok(medicine);
        }

        public class MedicineWithStockDto
        {
            public int CompanyId { get; set; }
            public string MedicineName { get; set; } = string.Empty;
            public string? BatchNumber { get; set; }
            public DateTime? ExpiryDate { get; set; }
            public decimal Price { get; set; }
            public int? InitialDistributorId { get; set; }
            public int InitialQuantity { get; set; }
        }

        // Get All Medicines (Company-wise SaaS isolation)
        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetByCompany(int companyId)
        {
            var medicines = await _context.Medicines
                .Where(m => m.CompanyId == companyId)
                .ToListAsync();

            return Ok(medicines);
        }

        // 🔥 Expiry Alert (Key Pharma Feature)
        [HttpGet("expiring/{days}")]
        public async Task<IActionResult> GetExpiring(int days)
        {
            var targetDate = DateTime.Now.AddDays(days);

            var expiring = await _context.Medicines
                .Where(m => m.ExpiryDate <= targetDate)
                .ToListAsync();

            return Ok(expiring);
        }
    }
}