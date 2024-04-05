using CommonLayer.Model.RequestDTO;
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
        public Task<string> CreateNote(NotesRequest Request);
        public Task<Notes> GetNotesById(int id);
       public Task<String> UpdateNoteById(int NoteId, UpdateNotesRequest note);
       public Task<string> DeleteNoteById(int NoteId);
    }
}
