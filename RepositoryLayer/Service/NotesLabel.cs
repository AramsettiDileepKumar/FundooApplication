using CommonLayer.Model.Label;
using Dapper;
using Nest;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface.LabelRL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class NotesLabelRL:INotesLabelRL
    {
        private readonly DapperContext _context;
        public NotesLabelRL(DapperContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateLabel(LabelModel label, int UserId)
        {
            try
            {
                var query = "INSERT INTO Label (LabelName, UserId, NoteId) VALUES (@Name, @Userid, @noteid)";
                using (var connection = _context.CreateConnection())
                {
                    var result = await connection.ExecuteAsync(query, new { Name = label.LabelName, Userid = UserId, noteid = label.NoteId });
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> UpdateLabel(int LabelId,int UserId, LabelNameModel model)
        {
            try
            {
                var query = "update Label set LabelName=@labelname where LabelId=@id and UserId=@userid";
                using (var connection = _context.CreateConnection())
                {
                    var result = await connection.ExecuteAsync(query, new { labelname = model.LabelName,id = LabelId, userid = UserId });
                    return result > 0;
                }
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> DeleteLabel(int LabelId, int UserId)
        {
            try
            {
                var query = "Delete from Label where LabelId=@id and UserId=@userid";
                using (var connections = _context.CreateConnection())
                {
                    var result = await connections.ExecuteAsync(query, new { id = LabelId, userid = UserId });
                    return result > 0;
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<IEnumerable<LabelInfo>> getLabel(int LabelId)
        {
            try
            {
                var query = "select l.LabelName,n.Title,n.Description,n.Colour " +
                          "from Label l join " +
                          "Notes n on l.NoteId=n.NoteId where l.LabelId=@id";
                using (var connections = _context.CreateConnection())
                {
                    return await connections.QueryAsync<LabelInfo>(query, new { id = LabelId });
                }
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message); 
            }
        }


    }
}
