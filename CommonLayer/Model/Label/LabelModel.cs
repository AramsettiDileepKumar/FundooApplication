using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Model.Label
{
    public class LabelModel
    {
        public string LabelName { get; set; } = string.Empty;
        public int UserId { get; set; }
        public int NoteId { get; set; }
    }
}
