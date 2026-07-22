using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaAPI.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int MedicineId { get; set; }

        [Required]
        public int Quantity { get; set; }

        public decimal? UnitPrice { get; set; }
    }
}
