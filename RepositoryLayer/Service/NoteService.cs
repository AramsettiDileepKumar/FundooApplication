using CommonLayer.Model.RequestDTO;
using CommonLayer.Model.ResponseDTO;
using Dapper;
using Microsoft.Win32;
using Nest;
using NLog;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Exceptions;
using RepositoryLayer.Interface.NoteService;
using RepositoryLayer.Interface.UserService;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class NoteService:INoteService
    {
        private readonly DapperContext _context;
        private readonly ILogger _logger;
        private readonly IDatabase cache;
        public NoteService(DapperContext context, ConnectionMultiplexer redisConnectionString)
        {
            _context = context;
            cache = redisConnectionString.GetDatabase();

        }
        public async Task<string> CreateNote(NotesRequest createNoteRequest, int userId)
        {
            var key = $"Notes_{userId}";
            await cache.KeyDeleteAsync(key);
            var parameters = new DynamicParameters();
            parameters.Add("Description", createNoteRequest.Description, DbType.String);
            parameters.Add("Title", createNoteRequest.Title, DbType.String);
            parameters.Add("Colour", createNoteRequest.Colour, DbType.String);
            parameters.Add("IsArchived", false, DbType.Boolean);
            parameters.Add("IsDeleted", false, DbType.Boolean);
            parameters.Add("UserId", userId, DbType.Int32);
            var insertQuery = @"
                                INSERT INTO Notes (Description, [Title], Colour, IsArchived, IsDeleted, UserId)
                                VALUES (@Description, @Title, @Colour, @IsArchived, @IsDeleted, @UserId);
                               ";
            using (var connection = _context.CreateConnection())
            { 
                bool tableExists = await connection.QueryFirstOrDefaultAsync<bool>(

                                 @"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Notes';");     
                if (!tableExists)
                {
                    await connection.ExecuteAsync(@"
                                                    CREATE TABLE Notes (
                                                             NoteId int PRIMARY KEY IDENTITY,
                                                             Description nvarchar(max),
                                                             [Title] nvarchar(max) NOT NULL,
                                                             Colour nvarchar(max),
                                                             IsArchived bit DEFAULT (0),
                                                             IsDeleted bit DEFAULT (0),
                                                             UserId int FOREIGN KEY REFERENCES Users(UserId));");
                }
                try
                {
                    var result=await connection.ExecuteAsync(insertQuery, parameters);
                    return "1 Row Affected";
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }
            }
        }
        public async Task<IEnumerable<NotesResponse>> GetAllNote(int userId)
        {
            var selectQuery = "SELECT * FROM Notes WHERE UserId = @UserId AND IsDeleted = 0 AND IsArchived = 0";
            using (var connection = _context.CreateConnection())
            {
                try
                {
                    var notes = await connection.QueryAsync<NotesResponse>(selectQuery, new { UserId = userId });
                    return notes;
                }
                catch (SqlException ex)
                {
                    throw new Exception("An error occurred while retrieving notes from the database.", ex);
                }
            }
        }
        public async Task<NotesResponse> UpdateNoteById(int noteId, int userId, UpdateNotesRequest updatedNote)
        {
             var key = $"Notes_{userId}";
             await cache.KeyDeleteAsync(key);
            var selectQuery = "SELECT NoteId, Description, Title, Colour FROM Notes WHERE UserId = @UserId AND NoteId = @NoteId";
            var updateQuery = @"UPDATE Notes SET Description = @Description,Title = @Title,Colour = @Colour WHERE UserId = @UserId AND NoteId = @NoteId;";
            string prevTitle, prevDescription, prevColour;
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var currentnote = await connection.QueryFirstOrDefaultAsync<NotesResponse>(selectQuery, new { UserId = userId, NoteId = noteId });
                    if (currentnote == null)
                    {
                        throw new UserNotFoundException("Note not found");
                    }
                    prevTitle = currentnote.Title;
                    prevDescription = currentnote.Description;
                    prevColour = currentnote.Colour;
                    await connection.ExecuteAsync(updateQuery, new
                    {
                        Description = CheckInput(updatedNote.Description, prevDescription),
                        Title = CheckInput(updatedNote.Title, prevTitle),
                        Colour = CheckInput(updatedNote.Colour, prevColour),
                        UserId = userId,
                        NoteId = noteId
                    });
                    var updatedNoteResponse = await connection.QueryFirstOrDefaultAsync<NotesResponse>(selectQuery, new { UserId = userId, NoteId = noteId });

                    if (updatedNoteResponse == null)
                    {
                        throw new DataBaseException("Failed to retrieve the updated note");
                    }

                    return updatedNoteResponse;
                }
            }
            catch (SqlException ex)
            {
                throw new DataBaseException("An error occurred while updating the note in the database", ex);
            }
            catch (UserNotFoundException ex)
            {
                throw new UserNotFoundException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("An error occurred in the repository layer", ex);
            }
         }
         public string CheckInput(string newValue, string previousValue)
         {
         return string.IsNullOrEmpty(newValue) ? previousValue : newValue;
         }
         public async Task<IEnumerable<NotesResponse>> GetAllNotes(int userId)
         {
            var selectQuery = "SELECT * FROM Notes WHERE UserId = @UserId AND IsDeleted = 0 AND IsArchived = 0";
            using (var connection = _context.CreateConnection())
            {
                try
                {
                    var notes = await connection.QueryAsync<NotesResponse>(selectQuery, new { UserId = userId });
                    return notes.Reverse().ToList();
                }
                catch (SqlException ex)
                {
                    throw new Exception("An error occurred while retrieving notes from the database.", ex);
                }
            }
         }
        public async Task<bool> DeleteNoteById(int noteId, int userId)
        {
            var deleteQuery = "DELETE FROM Notes WHERE NoteId = @NoteId AND UserId = @UserId";
            try
            {
                var key = $"Notes_{userId}";
                await cache.KeyDeleteAsync(key);
                using (var connection = _context.CreateConnection())
                {
                    var rowsAffected = await connection.ExecuteAsync(deleteQuery, new { NoteId = noteId, UserId = userId });
                    return rowsAffected > 0;
                }
            }
            catch (SqlException ex)
            {

                throw new DataBaseException("An error occurred while deleting the note from the database", ex);
            }
            catch (Exception ex)
            {

                throw new RepositoryException("An error occurred in the repository layer", ex);
            }
        }
        public async Task<IEnumerable<NotesResponse>> GetNoteByNoteId(int NoteId, int UserId)
        {
            var selectQuery = "SELECT * FROM Notes WHERE NoteId = @noteId AND UserId = @userId";

            using (var connection = _context.CreateConnection())
            {
                try
                {
                    var note = await connection.QueryAsync<NotesResponse>(selectQuery, new { userId = UserId, noteId = NoteId });

                    if (note == null)
                    {
                        throw new NotFoundException($"Note with NoteId '{NoteId}' does not exist for User with UserId '{UserId}'.");
                    }

                    return note;
                }
                catch (SqlException ex)
                {
                    throw new Exception("An error occurred while retriving note to from database.", ex);
                }

            }
        }

    }

   
}
