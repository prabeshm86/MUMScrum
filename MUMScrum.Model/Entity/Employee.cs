using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MUMScrum.Model.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MUMScrum.Model
{
    public class Employee : User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [MinLength(3)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Role")]
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }      
        
    }
}
