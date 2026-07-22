using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaAPI.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [Required]
        public int ClientId { get; set; }

        [Required]
        public int BookingRepId { get; set; }

        public int? AssignedDeliveryRepId { get; set; }

        [Required]
        public int DistributorId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [MaxLength(50)]
        public string Status { get; set; } = "Pending";

        public decimal TotalAmount { get; set; } = 0;

        [MaxLength(300)]
        public string? Notes { get; set; }
        
        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}
