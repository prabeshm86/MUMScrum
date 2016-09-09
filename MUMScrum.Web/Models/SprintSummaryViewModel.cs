using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MUMScrum.Web.Models
{
    public class SprintSummaryViewModel
    {
        public int SprintId { get; set; }
        public string SprintName { get; set; }
        public string ProductName { get; set; }
        public string ReleaseName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Developers { get; set; }
        public int Testers { get; set; }
        public int PercentageCompleted { get; set; }
        public int UserStories { get; set; }
    }
}