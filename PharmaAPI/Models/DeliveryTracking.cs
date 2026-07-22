using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaAPI.Models
{
    public class DeliveryTracking
    {
        [Key]
        public int TrackingId { get; set; }

        public int? OrderId { get; set; } // Made optional for general tracking

        [Required]
        public int UserId { get; set; } // Changed from DeliveryRepId to UserId

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
