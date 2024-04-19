using CommonLayer.Model.Collaboration;
using ModelLayer.Model.Collaboration;
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
        Task<IEnumerable<CollaborationInfoModel>> GetCollaborationId(int collaborationId,int UserId);
        Task<IEnumerable<CollaborationInfoModel>> GetAllCollaborators(int UserId);
        Task<bool> DeleteCollaborator(int collabId,int UserId);
    }
}
