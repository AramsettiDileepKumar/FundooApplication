using CommonLayer.Model.Label;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface.LabelRL
{
    public interface INotesLabelRL
    {
        public Task<bool> CreateLabel(LabelModel label, int UserId);
        public Task<bool> UpdateLabel(int LabelId,int UserId,LabelNameModel name);
        public Task<bool> DeleteLabel(int LabelId,int UserId);
        public Task<IEnumerable<LabelInfo>> getLabel(int LabelId);
    }
}
