using System;

namespace DevPlanTracker.Models
{
    public class DevTask
    {
        public int Id { get; set; }
        public string Area { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Goal { get; set; } = string.Empty;
        public string Status { get; set; } = "status-not-started";
        public string Notes { get; set; } = string.Empty;
        public DateTime? TargetDate { get; set; }
    }
}