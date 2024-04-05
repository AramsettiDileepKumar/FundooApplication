using CommonLayer.Model.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Model.RequestDTO
{
    public class UserRequest
    {
        [Required]
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        [Required]
        [UserRequestValidation]
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }

    }
}
