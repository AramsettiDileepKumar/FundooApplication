using CommonLayer.Model.Collaboration;
using Dapper;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface.CollaborationRL;
using System.Data;

namespace RepositoryLayer.Service
{
    public class CollaborationRL : ICollaborationRL
    { 
        private readonly DapperContext _context;
        public CollaborationRL(DapperContext context)
        {
            _context = context;
        }
        public async Task<bool> AddCollaborator(int NoteId, int userID, CollaborationRequestModel Request)
        {
            var query = @"
        INSERT INTO NoteCollaborators (NoteId, UserId, collabEmail)
        VALUES (@noteid, @userid, @collabemail)";
            using (var connection = _context.CreateConnection())
            {
                var register = await connection.ExecuteAsync(query, new {noteId = NoteId, userID = userID, CollabEmail = Request.Email });
                return true;
            }
        }
        public async Task<IEnumerable<Collaboration>> GetCollaborationId(int collabId)
        {
            var query = "SELECT CollabId, UserId, NoteId, collabEmail FROM NoteCollaborators WHERE CollabId = @CollabId";

            var parameters = new DynamicParameters();
            parameters.Add("@CollabId", collabId, DbType.Int64);

            using (var connection = _context.CreateConnection())
            {
                var collaborations = await connection.QueryAsync<Collaboration>(query, parameters);
                return collaborations;
            }
        }
        public async Task<string> DeleteCollaborator(int CollabId)
        {
            var query = $"Delete from NoteCollaborators where CollabId={CollabId};";
            using (var connection = _context.CreateConnection())
            {
                var register = await connection.ExecuteAsync(query);

                return "Collaborator Deleted Successfully";
            }
        }

    }
}
