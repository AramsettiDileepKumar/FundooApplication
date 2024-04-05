using BusinessLogicLayer.InterfaceBL.NotesInterface;
using CommonLayer.Model.RequestDTO;
using CommonLayer.Model.ResponseDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models;
using RepositoryLayer.Entity;
using System.Security.Claims;

namespace FundooApplication.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesBL noteServiceBL;

        public NotesController(INotesBL noteServiceBL)
        {
            this.noteServiceBL = noteServiceBL;
        }
      //  [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddNote(NotesRequest createNoteRequest)
        {
            try
            {
                return Ok( await noteServiceBL.CreateNote(createNoteRequest));
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<IEnumerable<NotesResponse>>
                {
                    Success = false,
                    Message = ex.Message,

                };
                return Ok(response);
                throw new Exception("");
            }
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetNotesByID(int Id)
        {
            try
            {
                return Ok(await noteServiceBL.GetNotesById(Id));
            }
            catch(Exception ex) 
            {
                return StatusCode(500, "Enter Valid User Details");
            }
        }

        [HttpPut("{NoteId}")]
        public async Task<IActionResult> UpdateNoteById(int NoteId,UpdateNotesRequest note )
        {
            try
            {
                return Ok(await noteServiceBL.UpdateNoteById(NoteId, note));
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Enter Valid User Details");
            }
        }
        [HttpDelete("{NoteId}")]
        public async Task<IActionResult> DeleteNoteById(int NoteId)
        {
            try
            {
                return Ok(await noteServiceBL.DeleteNoteById(NoteId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Enter Valid User Details");
            }
        }



    }
}
