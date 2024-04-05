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
        Task<string> CreateNote(NotesRequest Request);
        Task<Notes> GetNotesById(int id);
        Task<String> UpdateNoteById(int NoteId, UpdateNotesRequest note);
        Task<string> DeleteNoteById(int NoteId);
    }
}
