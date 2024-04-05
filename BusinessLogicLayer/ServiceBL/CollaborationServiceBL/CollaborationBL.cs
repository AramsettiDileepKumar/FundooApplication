using BusinessLogicLayer.InterfaceBL;
using CommonLayer.Model.Collaboration;
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
        public Task<IEnumerable<Collaboration>> GetCollaborationId(int collaborationId)
        {
            return collaborationRL.GetCollaborationId(collaborationId);
        }
        public Task<string> DeleteCollaborator(int CollabId)
        {
            return collaborationRL.DeleteCollaborator(CollabId);
        }
            

    }
}
