using CommonLayer.Model.Collaboration;
using ModelLayer.Model.Collaboration;
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
        Task<IEnumerable<CollaborationInfoModel>> GetCollaborationId(int collaborationId, int UserId);
        Task<bool> DeleteCollaborator(int Collabid, int UserId);
        Task<IEnumerable<CollaborationInfoModel>> GetAllCollaborators(int UserId);
    }
}
