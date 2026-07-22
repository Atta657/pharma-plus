using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaAPI.Models
{
    public class AIInsight
    {
        [Key]
        public int InsightId { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [MaxLength(100)]
        public string? InsightType { get; set; } // SalesPrediction, RepPerformance, VisitAnalysis

        [MaxLength(500)]
        public string? Description { get; set; }

        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    }
}
