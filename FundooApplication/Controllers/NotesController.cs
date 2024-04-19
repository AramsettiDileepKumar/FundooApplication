using BusinessLogicLayer.CustomExceptions;
using BusinessLogicLayer.InterfaceBL;
using BusinessLogicLayer.InterfaceBL.NotesInterface;
using CommonLayer.Model.RequestDTO;
using CommonLayer.Model.ResponseDTO;
using Elasticsearch.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using ModelLayer.Models;
using Nest;
using Newtonsoft.Json;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface.UserService;
using StackExchange.Redis;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Text.Json;

namespace FundooApplication.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly INotesBL noteServiceBL;
        private readonly ILogger _logger;
        private readonly IDatabase cache;
        public NotesController(INotesBL noteServiceBL,ConnectionMultiplexer redisConnectionString)
        {
            this.noteServiceBL = noteServiceBL;
            cache=redisConnectionString.GetDatabase();
        }
      
        [HttpPost]
        public async Task<IActionResult> AddNote(NotesRequest createNoteRequest)
        {
            try
            {
                var value = User.FindFirstValue("userId");
                int UserId = int.Parse(value);
                var Note =await noteServiceBL.CreateNote(createNoteRequest,UserId);
                if (Note != null)
                {
                    var response = new ResponseModel<string>
                    {
                        StatusCode = 200,
                        Message = "Note Created Successfully",
                        Data = Note
                    };
                    return Ok(response);
                }
                var respons = new ResponseModel<string>
                {
                    StatusCode = 400,
                    Success=false,
                    Message = "User Not Found",
                    Data = Note
                };
                return NotFound(respons);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<IEnumerable<NotesResponse>>
                {
                    StatusCode = 500,
                    Message = ex.Message,

                };
                return Ok(response);
            }
        }
      
        [HttpGet]
        public async Task<IActionResult> GetNotesByID()
        {
            try
            {
                var value = User.FindFirstValue("userId");
                int UserId = int.Parse(value);
                var cachekey = $"Notes_{UserId}";
                var cachedLabels = await cache.StringGetAsync(cachekey);
                if (!string.IsNullOrEmpty(cachedLabels))
                {
                    var notesList = JsonConvert.DeserializeObject<List<Notes>>(cachedLabels);
                    var response = new ResponseModel<IEnumerable<Notes>>
                    {
                        StatusCode = 200,
                        Message = "Note Fetched Successfully From Cache",
                        Data = notesList
                    };
                  return Ok(response);
                }
                var notes = await noteServiceBL.GetAllNote(UserId);
                if (notes != null)
                {
                    await cache.StringSetAsync(cachekey, JsonConvert.SerializeObject(notes), TimeSpan.FromMinutes(10));
                    var response = new ResponseModel<IEnumerable<NotesResponse>>
                    {
                        StatusCode = 200,
                        Message = "Note Fetched Successfully From Data Base",
                        Data = notes
                    };
                    return Ok(response);
                }
                var respons = new ResponseModel<IEnumerable<NotesResponse>>
                {
                    StatusCode = 400,
                    Success=false,
                    Message = "User Not Found",
                    Data = notes
                };
                return NotFound(respons);

            }
            catch (UserNotFoundException ex)
            {
                var response = new ResponseModel<string>
                {
                    Success = false,
                    Message = $"User Not Found: {ex.Message}",
                    Data = null
                };
                return StatusCode(500, response);
            }
            catch (SqlException ex)
            {
                return StatusCode(500, new ResponseModel<string>
                {
                    Success = false,
                    Message = "An error occurred while retrieving note from the database.",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string>
                {
                    Success = false,
                    Message = $"An error occurred {ex.Message}",
                    Data = null
                });
            }
        }
        [HttpPut("{NoteId}")]
   
        public async Task<IActionResult> UpdateNoteById(int NoteId,UpdateNotesRequest note )
        {
            try
            {
               
                var value = User.FindFirstValue("userId");
                int UserId = int.Parse(value);
                var NoteUpdate =await noteServiceBL.UpdateNoteById(NoteId,UserId, note);
                if (NoteUpdate != null)
                {
                    var response = new ResponseModel<NotesResponse>
                    {
                        Message = "Note updated successfully",
                        Data = NoteUpdate
                    };
                    return Ok(response);
                }
                var respons = new ResponseModel<NotesResponse>
                {
                    Message = "User Not found",
                    Data = NoteUpdate
                };
                return NotFound(respons);
            }
            catch (UserNotFoundException ex)
            {
                var response = new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message
                };
                return NotFound(response);
            }
            catch (DataBaseException ex)
            {
                var response = new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message
                };
                return StatusCode(500, response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<string>
                {
                    Success = false,
                    Message = "An unexpected error occurred: " + ex.Message
                };
                return StatusCode(500, response);
            }
        }
        [HttpDelete("{NoteId}")]
        public async Task<IActionResult> DeleteNoteById(int NoteId)
        {
            try
            {

                var value = User.FindFirstValue("userId");
                int UserId = int.Parse(value);
                bool result = await noteServiceBL.DeleteNoteById(NoteId, UserId);
                if (result)
                {
                    return Ok(new ResponseModel<string>
                    {
                        Message = "Note Deleted successfully",
                        Data = null,
                    });
                }
                else
                {
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Note not found",
                        Data = null
                    });
                }
            }
            catch (DataBaseException ex)
            {
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string>
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}",
                    Data = null
                });
            }
        }
        [HttpGet("GetNoteById")]
        public async Task<IActionResult> GetNoteById(int NoteId)
        {
            try
            {
                var value = User.FindFirstValue("userId");
                int UserId = int.Parse(value);
                var cachekey = $"Notes_{UserId}";
                var cachedLabels = await cache.StringGetAsync(cachekey);
                if (!string.IsNullOrEmpty(cachedLabels))
                {
                    var notesList = JsonConvert.DeserializeObject<List<Notes>>(cachedLabels);
                    var response = new ResponseModel<IEnumerable<Notes>>
                    {
                        StatusCode = 200,
                        Message = "Note Fetched Successfully From Cache",
                        Data = notesList
                    };
                    return Ok(response);
                }
                var notes = await noteServiceBL.GetNoteByNoteId(NoteId, UserId);
                if (notes != null)
                {
                    await cache.StringSetAsync(cachekey, JsonConvert.SerializeObject(notes), TimeSpan.FromMinutes(10));
                    var response = new ResponseModel<IEnumerable<NotesResponse>>
                    {
                        StatusCode = 200,
                        Message = "Note Fetched Successfully From Data Base",
                        Data = notes
                    };
                    return Ok(response);
                }
                else
                {
                    return NotFound(new ResponseModel<NotesResponse>
                    {
                        Success = false,
                        Message = "No note found",
                        Data = null
                    });
                }
            }
            catch (UserNotFoundException ex)
            {
                var response = new ResponseModel<string>
                {
                    Success = false,
                    Message = $"Error sending email: {ex.Message}",
                    Data = null
                };
                return StatusCode(500, response);
            }
            catch (SqlException ex)
            {
                return StatusCode(500, new ResponseModel<string>
                {
                    Success = false,
                    Message = "An error occurred while retrieving note from the database.",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string>
                {

                    Success = false,
                    Message = $"An error occurred.{ex.Message}",
                    Data = null
                });
            }
        }
    }
}
