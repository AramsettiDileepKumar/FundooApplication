using CommonLayer.Model.RequestDTO;
using CommonLayer.Model.ResponseDTO;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.InterfaceBL.NotesInterface
{
    public interface INotesBL
    {
        Task<string> CreateNote(NotesRequest Request, int userid);
        Task<IEnumerable<NotesResponse>> GetAllNote(int userId);
        Task<NotesResponse> UpdateNoteById(int NoteId,int userId, UpdateNotesRequest note);
        Task<bool> DeleteNoteById(int NoteId,int userId);
        public Task<IEnumerable<NotesResponse>> GetNoteByNoteId(int NoteId, int UserId);


    }
}
