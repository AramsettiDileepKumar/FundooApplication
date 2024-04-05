using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Model.RequestDTO
{
    public class NotesRequest
    {
        [Required]
        public string Title { get; set; }

        public string? Description { get; set; } = string.Empty;

        public string Colour { get; set; } = string.Empty;

        public string Email {  get; set; } = string.Empty;


    }
}
