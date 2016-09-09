using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUMScrum.Model.Abstract
{
    public abstract class User
    {
        [Display(Name = "Username")]
        [Required]
        [MinLength(3)]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        [Required]
        [MinLength(3)]
        public string Password { get; set; }
        [Display(Name = "Deactivate")]
        public bool IsDeactivated { get; set; }
    }
}
