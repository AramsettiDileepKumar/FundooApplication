using CommonLayer.Model.Label;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.InterfaceBL.Labels
{
    public interface INotesLabelBL
    {
        public Task<bool> CreateLabel(LabelModel label,int UserId);
        public Task<bool> UpdateLabel(int LabelId,int UserId,LabelNameModel name);
        public Task<bool> DeleteLabel(int LabelId,int UsedId);
        public Task<IEnumerable<LabelInfo>> getLabel(int LabelId);
    }
}
