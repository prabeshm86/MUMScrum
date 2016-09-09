using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUMScrum.Model
{
    public class Sprint
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Sprint Name")]
        [Required(ErrorMessage = "Please Enter the Sprint Name ")]
        [MinLength(3)]
        public string SprintName { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

       
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Release")]
        public int ReleaseBacklogId { get; set; }

        [Display(Name ="Sprint Running")]
        public bool SprintRunning { get; set; }

        [ForeignKey("ReleaseBacklogId")]
        [Display(Name = "Release")]
        public virtual ReleaseBacklog ReleaseBacklog { get; set; }

        public virtual ICollection<UserStory> UserStories { get; set; }

    }
}
