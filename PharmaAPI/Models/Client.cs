using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaAPI.Models
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [Required]
        [MaxLength(150)]
        public string ClientName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? ClientType { get; set; } // Pharmacy, Clinic, Hospital

        [MaxLength(150)]
        public string? ContactPerson { get; set; }

        [MaxLength(50)]
        public string? Phone { get; set; }

        [MaxLength(300)]
        public string? Address { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
