using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Model.Collaboration
{
    public class CollaborationInfoModel
    {
        public int CollabId { get; set; }
        public int UserId { get; set; }
        public int NoteId { get; set; }
        public string collabEmail { get; set; }

    }
}
