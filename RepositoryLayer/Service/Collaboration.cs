using CommonLayer.Model.Collaboration;
using Dapper;
using ModelLayer.Model.Collaboration;
using Org.BouncyCastle.Asn1.Ocsp;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Exceptions;
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
            try
            {
                var isexists = "select count(*) From Notes where UserId=@userid and NoteId=@noteid";
                using (var connection = _context.CreateConnection())
                {
                    var noteExists = await connection.ExecuteScalarAsync<int>(isexists, new { userid = userID, noteid = NoteId });
                    if (noteExists == 0)
                    {
                        throw new Exception("Note Id doesn't Exists");
                    }
                    var query = @"INSERT INTO NoteCollaborators (NoteId, UserId, collabEmail) VALUES (@noteid, @userid, @collabemail)";
                    var register = await connection.ExecuteAsync(query, new { noteId = NoteId, userid = userID, CollabEmail = Request.Email });
                    return true;
                }
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<IEnumerable<CollaborationInfoModel>> GetCollaborationId(int collabId, int UserId)
        {
            try
            {
                var query = "SELECT * FROM NoteCollaborators WHERE CollabId = @CollabId and UserId=@userId";
                using (var connection = _context.CreateConnection())
                {
                    var collaborations = await connection.QueryAsync<CollaborationInfoModel>(query, new { CollabId = collabId, userId = UserId });
                    return collaborations;
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> DeleteCollaborator(int CollabId, int UserId)
        {
            try
            {
                var query = @" DELETE FROM NoteCollaborators WHERE UserId = @userId AND CollabId=@collabid";
                using (var connection = _context.CreateConnection())
                {
                    int rowsAffected = await connection.ExecuteAsync(query,new { userId = UserId,collabid=CollabId });
                    return rowsAffected > 0;
                }
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<IEnumerable<CollaborationInfoModel>> GetAllCollaborators(int UserId)
        {
            try
            {
                var query = "SELECT * FROM NoteCollaborators where UserId=@userid";
                using (var connection = _context.CreateConnection())
                {
                    var collaborators = await connection.QueryAsync<CollaborationInfoModel>(query, new { userid = UserId });
                    return collaborators;

                }
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
