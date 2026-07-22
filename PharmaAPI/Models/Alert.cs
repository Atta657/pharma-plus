using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaAPI.Models
{
    public class Alert
    {
        [Key]
        public int AlertId { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [MaxLength(50)]
        public string? AlertType { get; set; } // Expiry, LowStock, OrderDelay, FakeVisit

        public int? ReferenceId { get; set; } // MedicineId / OrderId

        [MaxLength(300)]
        public string? Message { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
