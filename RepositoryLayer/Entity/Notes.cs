using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    public class Notes
    {
        [Key]
        public int NoteId { get; set; }
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
        public string Colour { get; set; } = string.Empty;
        public bool IsArchived { get; set; } = false;
        public bool IsDeleted { get; set; } = false;

        [ForeignKey("Users")]
        public int UserId { get; set; }


    }

}
