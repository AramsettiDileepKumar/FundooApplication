using BusinessLogicLayer.InterfaceBL;
using CommonLayer.Model.Collaboration;
using ModelLayer.Model.Collaboration;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface.CollaborationRL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ServiceBL.CollaborationServiceBL
{
    public class CollaborationBL : ICollaborationBL
    {
        private readonly ICollaborationRL collaborationRL;

        public CollaborationBL(ICollaborationRL collaborationRL)
        {
            this.collaborationRL = collaborationRL;
        }

        public Task<bool> AddCollaborator(int NoteId, int UserId ,CollaborationRequestModel Request)
        {
            return collaborationRL.AddCollaborator(NoteId, UserId,Request);
        }
        public Task<IEnumerable<CollaborationInfoModel>> GetCollaborationId(int collaborationId, int UserId)
        {
            return collaborationRL.GetCollaborationId(collaborationId,UserId);
        }
        public Task<bool> DeleteCollaborator(int CollabId, int UserId)
        {
            return collaborationRL.DeleteCollaborator(CollabId,UserId);
        }
        public Task<IEnumerable<CollaborationInfoModel>> GetAllCollaborators(int UserId)
        {
            return collaborationRL.GetAllCollaborators(UserId);
        }
    }
}
