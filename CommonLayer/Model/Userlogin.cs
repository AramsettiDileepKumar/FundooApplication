using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Model
{
    public class Userlogin
    {
        [Required(ErrorMessage ="Email is Required")]
        [EmailAddress(ErrorMessage ="Invalid Email Format")]
        public string Email { get; set; }
        [Required (ErrorMessage ="Password is required")]
        public string Password { get; set; }
    }
}
