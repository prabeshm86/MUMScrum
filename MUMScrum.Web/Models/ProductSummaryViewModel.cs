using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MUMScrum.Web.Models
{
    public class ProductSummaryViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Sprints { get; set; }
        public int Releases { get; set; }
        public int Developers { get; set; }
        public int Testers { get; set; }
        public int ScrumMasters { get; set; }
        public int AssignedUserStories { get; set; }
        public int BacklogUserStories { get; set; }
        public int PercentCompleted { get; set; }
    }
}