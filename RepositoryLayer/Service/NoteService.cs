using CommonLayer.Model.RequestDTO;
using Dapper;
using Nest;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface.NoteService;
using RepositoryLayer.Interface.UserService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class NoteService:INoteService
    {
        private readonly DapperContext _context;
        private readonly IRegistration objt;
        public NoteService(DapperContext context, IRegistration rs)
        {
            _context = context;
            objt= rs;
        }
        public async Task<string> CreateNote(NotesRequest Request)
        {
            var obj = await objt.GetByEmailAsync(Request.Email);
            var parameters = new DynamicParameters();
            parameters.Add("Description", Request.Description, DbType.String);
            parameters.Add("Title", Request.Title, DbType.String);
            parameters.Add("Colour", Request.Colour, DbType.String);
            parameters.Add("IsArchived", false, DbType.Boolean);
            parameters.Add("IsDeleted", false, DbType.Boolean);
            parameters.Add("UserId", obj.userId, DbType.Int64);
            var insertQuery = @"
                                INSERT INTO Notes(Description, [Title], Colour, IsArchived, IsDeleted,UserId)
                                VALUES (@Description, @Title, @Colour, @IsArchived, @IsDeleted,@UserId);
                               ";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(insertQuery, parameters);

                return "Notes Inserted Successfully";

            }
        }
        public async Task<Notes> GetNotesById(int Id)
        {
            var query = "select * from Notes where UserId=@Id and IsDeleted=0";
            using(var connection = _context.CreateConnection()) 
            {
                return await connection.QueryFirstOrDefaultAsync<Notes>(query,new {Id=Id});
            }
        }
        public async Task<String> UpdateNoteById(int NoteId, UpdateNotesRequest note)
        {
            var query = " UPDATE Notes SET Description = @Description, Title = @Title, Colour = @Colour  WHERE NoteId = @NoteId;";
            using(var connection = _context.CreateConnection()) 
            {
                await connection.ExecuteAsync(query, new {NoteId=NoteId,Description=note.Description,Title=note.Title,Colour=note.Colour});
                return " Note Updated Successfully";
            }
        }
       public async Task<string> DeleteNoteById(int NoteId)
        {
            var query = "update Notes set IsDeleted=1 where NoteId=@noteId";
            
            using( var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { noteId=NoteId});
                
                return "Note Deleted Successfully";
            }
        }
    }
}
