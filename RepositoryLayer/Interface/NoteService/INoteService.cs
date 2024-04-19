using CommonLayer.Model.RequestDTO;
using CommonLayer.Model.ResponseDTO;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface.NoteService
{
    public interface INoteService
    {
       public Task<string> CreateNote(NotesRequest Request, int userid);
       public Task<NotesResponse> UpdateNoteById(int NoteId,int userid, UpdateNotesRequest note);
       public Task<bool> DeleteNoteById(int NoteId,int userid);
       public Task<IEnumerable<NotesResponse>> GetAllNote(int userId);
        public Task<IEnumerable<NotesResponse>> GetNoteByNoteId(int NoteId, int UserId);

    }
}
