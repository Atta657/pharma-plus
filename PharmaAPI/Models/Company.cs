using System.ComponentModel.DataAnnotations;

namespace PharmaAPI.Models
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }

        [Required]
        public string CompanyName { get; set; } = string.Empty;

        public string? CompanyEmail { get; set; }
        public string? CompanyPhone { get; set; }
        public string? Address { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
