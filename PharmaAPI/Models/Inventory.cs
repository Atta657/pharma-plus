using System.ComponentModel.DataAnnotations;

namespace PharmaAPI.Models
{
    public class Inventory
    {
        [Key]
        public int InventoryId { get; set; }

        public int DistributorId { get; set; }

        public int MedicineId { get; set; }

        public int Quantity { get; set; }
    }
}