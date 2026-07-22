using System;
using System.ComponentModel.DataAnnotations;

namespace PharmaAPI.Models
{
    public class Medicine
    {
        [Key]
        public int MedicineId { get; set; }

        public int CompanyId { get; set; }

        [Required]
        public string MedicineName { get; set; } = string.Empty;

        public string? BatchNumber { get; set; }

        public DateTime ExpiryDate { get; set; }

        public decimal Price { get; set; }
    }
}