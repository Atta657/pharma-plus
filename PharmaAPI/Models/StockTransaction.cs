using System;
using System.ComponentModel.DataAnnotations;

namespace PharmaAPI.Models
{
    public class StockTransaction
    {
        [Key]
        public int TransactionId { get; set; }

        [Required]
        public int MedicineId { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [MaxLength(20)]
        public string TransactionType { get; set; } = "Add"; // Add, Remove, Order

        public int? UserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string? Remarks { get; set; }
    }
}
