using BusinessLogicLayer.CustomExceptions;
using BusinessLogicLayer.InterfaceBL;
using BusinessLogicLayer.InterfaceBL.NotesInterface;
using CommonLayer.Model.RequestDTO;
using CommonLayer.Model.ResponseDTO;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface.NoteService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ServiceBL.NotesService
{
    public class NotesServiceBL:INotesBL
    {
        private readonly INoteService NotesRepo;
        public NotesServiceBL(INoteService notesRepo)
        {
            NotesRepo = notesRepo;
        }
        public Task<string> CreateNote(NotesRequest Request, int userid)
        {
            return NotesRepo.CreateNote(Request,userid);
        }
        public Task<IEnumerable<NotesResponse>> GetAllNote(int userId)
        {
            return NotesRepo.GetAllNote(userId);
        }
        public Task<NotesResponse> UpdateNoteById(int NoteId,int userId, UpdateNotesRequest note)
        {
            return NotesRepo.UpdateNoteById(NoteId,userId,note);
        }
        public Task<bool> DeleteNoteById(int NoteId, int userId)
        {
            return NotesRepo.DeleteNoteById(NoteId,userId);
        }
        public  Task<IEnumerable<NotesResponse>> GetNoteByNoteId(int NoteId, int UserId)
        {
            return NotesRepo.GetNoteByNoteId(NoteId,UserId);
        }



    }
}
