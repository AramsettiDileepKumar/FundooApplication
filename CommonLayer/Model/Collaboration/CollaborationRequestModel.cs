using CommonLayer.Model.Validation;
using ModelLayer.Model.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Model.Collaboration
{
    public class CollaborationRequestModel
    {
        [EmailValidation]
        public string Email { get; set; }

    }
}
