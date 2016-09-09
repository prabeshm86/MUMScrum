using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUMScrum.Model
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }
}
