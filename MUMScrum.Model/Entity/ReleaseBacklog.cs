using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUMScrum.Model
{
    public class ReleaseBacklog
    {
        [Key]
        public int Id { get; set; }
          [Display(Name = "Release Name")]
        [Required(ErrorMessage = "Please Enter the Release Name ")]
        [MinLength(3)]
        public string ReleaseName { get; set; }
        
        public string Description { get; set; }

      
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }
        [Required]
        [Display(Name = "Product")]
        public int ProductBacklogId { get; set; }
        [ForeignKey("ProductBacklogId")]
        [Display(Name = "Product")]
        public virtual ProductBacklog ProductBacklog { get; set; }    
        public virtual ICollection<Sprint> Sprints { get; set; }
        public virtual ICollection<UserStory> UserStories { get; set; }
        public int? ScrumMasterId { get; set; }
        [ForeignKey("ScrumMasterId")]
        [Display(Name = "Scrum Master")]
        public virtual Employee ScrumMaster { get; set; }

    }
}
