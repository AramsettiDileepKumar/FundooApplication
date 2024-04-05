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
        public Task<string> CreateNote(NotesRequest Request)
        {
            return NotesRepo.CreateNote(Request);
        }

        public Task<Notes> GetNotesById(int id)
        {
            return NotesRepo.GetNotesById(id); 
        }
        public Task<String> UpdateNoteById(int NoteId, UpdateNotesRequest note)
        {
            return NotesRepo.UpdateNoteById(NoteId,note);
        }
        public Task<String> DeleteNoteById(int NoteId)
        {
            return NotesRepo.DeleteNoteById(NoteId);
        }
    }
}
