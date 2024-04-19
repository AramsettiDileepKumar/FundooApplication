using BusinessLogicLayer.InterfaceBL.Labels;
using CommonLayer.Model.Label;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface.LabelRL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ServiceBL.LabelService
{
    public class NotesLabelBL:INotesLabelBL
    {
        private readonly INotesLabelRL notesLabelRL;
        public NotesLabelBL(INotesLabelRL notesLabelRL)
        {
            this.notesLabelRL = notesLabelRL;
        }
        public Task<bool> CreateLabel(LabelModel label, int UserId)
        {
            return notesLabelRL.CreateLabel(label,UserId);
        }
        public Task<bool> UpdateLabel(int LabelId,int UserId, LabelNameModel name)
        {
            return notesLabelRL.UpdateLabel(LabelId,UserId,name);
        }
        public Task<bool> DeleteLabel(int LabelId, int UserId)
        {
            return notesLabelRL.DeleteLabel(LabelId,UserId);   
        }
        public Task<IEnumerable<LabelInfo>> getLabel(int LabelId)
        {
            return notesLabelRL.getLabel(LabelId);
        }
    }
}
