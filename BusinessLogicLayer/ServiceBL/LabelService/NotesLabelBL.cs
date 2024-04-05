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
        public Task<string> CreateLabel(LabelModel label)
        {
            return notesLabelRL.CreateLabel(label);
        }
        public Task<string> UpdateLabel(int LabelId,LabelNameModel name)
        {
            return notesLabelRL.UpdateLabel(LabelId,name);
        }
        public Task<string> DeleteLabel(int LabelId)
        {
            return notesLabelRL.DeleteLabel(LabelId);   
        }
        public Task<IEnumerable<LabelInfo>> getLabel(int LabelId)
        {
            return notesLabelRL.getLabel(LabelId);
        }
    }
}
