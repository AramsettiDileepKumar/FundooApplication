using CommonLayer.Model.Collaboration;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface.CollaborationRL
{
    public interface ICollaborationRL
    {
        public Task<bool> AddCollaborator(int NoteId,int userId, CollaborationRequestModel Request);
        Task<IEnumerable<Collaboration>> GetCollaborationId(int collaborationId);
        Task<string> DeleteCollaborator(int CollabId);
    }
}
