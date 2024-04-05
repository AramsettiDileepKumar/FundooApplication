using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    public class Label
    {
        public int LabelId {  get; set; }
        public string LabelName { get; set; } = string.Empty;
        public int UserId { get; set; }
        public int NoteId { get; set; }
    }
}
