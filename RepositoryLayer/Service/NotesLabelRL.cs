using CommonLayer.Model.Label;
using Dapper;
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
        public async Task<string> CreateLabel(LabelModel label)
        {
            var query = "Insert into Label values(@Name,@Userid,@noteid)";
            using(var connection=_context.CreateConnection()) 
            {
                await connection.ExecuteAsync(query, new {Name=label.LabelName,Userid=label.UserId,noteid=label.NoteId});
                return "Label Created Successfully";
            }
        }
        public async Task<string> UpdateLabel(int LabelId,LabelNameModel name)
        {
            var query = "update Label set LabelName=@labelname where LabelId=@id";
            using (var connection=_context.CreateConnection()) 
            {
                await connection.ExecuteAsync(query, new {labelname=name.LabelName,id=LabelId});
                return "Label Updated Successfully";
            }
        }
        public async Task<string> DeleteLabel(int LabelId)
        {
            var query = "Delete from Label where LabelId=@id";
            using(var  connections=_context.CreateConnection()) 
            {
                await connections.ExecuteAsync(query, new {id=LabelId});
                return "Label Deleted Successfully";
            }
        }
        public async Task<IEnumerable<LabelInfo>> getLabel(int LabelId)
        {
            var query = "select l.LabelName,n.Title,n.Description,n.Colour " +
                      "from Label l join " +
                      "Notes n on l.NoteId=n.NoteId where l.LabelId=@id";
            using(var connections=_context.CreateConnection())
            {
                return  await connections.QueryAsync<LabelInfo>(query,new { id = LabelId });
            }
        }


    }
}
