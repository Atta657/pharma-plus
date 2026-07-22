using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaAPI.Models
{
    public class Schedule
    {
        [Key]
        public int ScheduleId { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [Required]
        public int RepId { get; set; }

        [Required]
        public int ClientId { get; set; }

        [Required]
        public DateTime ScheduledDate { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "Pending";
    }
}
