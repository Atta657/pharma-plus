using System.ComponentModel.DataAnnotations;

namespace PharmaAPI.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [Required]
        [MaxLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsApproved { get; set; } = false; // New field for admin approval

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}