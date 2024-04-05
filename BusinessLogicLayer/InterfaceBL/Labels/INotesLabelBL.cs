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
        public Task<string> CreateLabel(LabelModel label);
        public Task<string> UpdateLabel(int LabelId,LabelNameModel name);
        public Task<string> DeleteLabel(int LabelId);
        public Task<IEnumerable<LabelInfo>> getLabel(int LabelId);
    }
}
