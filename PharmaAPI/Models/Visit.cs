using System;
using System.ComponentModel.DataAnnotations;

namespace PharmaAPI.Models
{
    public class Visit
    {
        [Key]
        public int VisitId { get; set; }

        public int RepId { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public DateTime VisitTime { get; set; } = DateTime.Now;

        public bool IsVerified { get; set; } = false;
    }
}