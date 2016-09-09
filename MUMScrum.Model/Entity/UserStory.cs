using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUMScrum.Model
{
    public class UserStory
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "User story should have title!", AllowEmptyStrings = false)]
        [MinLength(3)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Display(Name = "Developer")]
        public int? DevelopedId { get; set; }
        [ForeignKey("DevelopedId")]
        public virtual Employee Developer { get; set; }
        [Display(Name = "Tester")]
        public int? TesterId { get; set; }
        [ForeignKey("TesterId")]
        public virtual Employee Tester { get; set; }

        [Required]
        [Display(Name = "Product BackLog")]
        public int ProductBackLogId { get; set; }
        [ForeignKey("ProductBackLogId")]
        public virtual ProductBacklog ProductBackLog { get; set; }

        [Display(Name = "Release")]
        public int? ReleaseId { get; set; }
        [ForeignKey("ReleaseId")]
        public virtual ReleaseBacklog ReleaseBackLog { get; set; }

        [Display(Name = "Sprint")]
        public int? SprintId { get; set; }
        [ForeignKey("SprintId")]
        public virtual Sprint Sprint { get; set; }

        [Display(Name = "Developer Estimate [Hrs]")]
        public int? DevEstimate { get; set; }

        [Display(Name = "Tester Estimate [Hrs]")]
        public int? TestEstiamte { get; set; }

        [Display(Name = "Developer Actual [Hrs]")]
        public int? DevActual { get; set; }

        [Display(Name = "Tester Actual [Hrs]")]
        public int? TestActual { get; set; }

        public virtual ICollection<UserStoryLog> UserStoryLogs { get; set; }
        public bool IsAssigned()
        {
            return ReleaseId.HasValue;
        }
        public int WorkRemaining()
        {
            return (DevEstimate.HasValue ? DevEstimate.Value : 0) + (TestEstiamte.HasValue ? TestEstiamte.Value : 0)
                - (DevActual.HasValue ? DevActual.Value : 0) - (TestActual.HasValue ? TestActual.Value : 0);
        }
        public int PercentageCompleted()
        {
            var totalEstimate = (DevEstimate.HasValue ? DevEstimate.Value : 0) + (TestEstiamte.HasValue ? TestEstiamte.Value : 0);
            var totalWork = (TestActual.HasValue ? TestActual.Value : 0) + (DevActual.HasValue ? DevActual.Value : 0);
            var percent = totalEstimate == 0 ? 0 : (totalWork * 100) / totalEstimate;
            return percent;
        }
    }
}
