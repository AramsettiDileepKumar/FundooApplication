using CommonLayer.Model.Collaboration;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.InterfaceBL
{
    public interface ICollaborationBL
    {
        Task<bool> AddCollaborator(int NoteId,int UserId, CollaborationRequestModel Request);
        Task<IEnumerable<Collaboration>> GetCollaborationId(int collaborationId);
        Task<string> DeleteCollaborator(int CollabId);
    }
}
