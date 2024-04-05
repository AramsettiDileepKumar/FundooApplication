using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    public class Collaboration
    {
       
        public int CollabId { get; set; }

      
        public int UserId { get; set; }

      
        public int NoteId { get; set; }

   
        public string collabEmail { get; set; }

    }

}
