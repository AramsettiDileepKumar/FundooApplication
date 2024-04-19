using BusinessLogicLayer.CustomExceptions;
using BusinessLogicLayer.InterfaceBL;
using BusinessLogicLayer.InterfaceBL;
using BusinessLogicLayer.Mailsender;
using CommonLayer.Model.Collaboration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model.Collaboration;
using ModelLayer.Models;
using Nest;
using System.Security.Claims;

namespace FundooApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CollaborationController : ControllerBase
    {
        public readonly ICollaborationBL collaborationBL;

        public CollaborationController(ICollaborationBL collaborationBL)
        {
            this.collaborationBL = collaborationBL;
        }
        [HttpPost("{NoteId}")]
        public async Task<IActionResult> AddCollaborator(int NoteId, [FromBody] CollaborationRequestModel request)
        {
            try
            {
                var value = User.FindFirstValue("userId");
                int UserId = int.Parse(value);
                var result = await collaborationBL.AddCollaborator(NoteId, UserId, request);
                MailSenderClass.sendCollabMail(request.Email);
                if (result)
                {
                    var response = new ResponseModel<bool>
                    {
                        StatusCode=200,
                        Message = "Collaboration Successfull",
                        Data=result

                    };
                    return Ok(response);
                }
                var respons = new ResponseModel<bool>
                {
                    StatusCode = 200,
                    Success=false,
                    Message = "Invalid User",
                    Data = result

                };
                return NotFound(respons);

            }
            catch (NotFoundException ex)
            {
                var response = new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
                return NotFound(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message,

                };
                return Ok(response);

            }
        }
        [HttpGet("{CollaboratorId}")]
        public async Task<IActionResult> GetCollaborator(int CollaboratorId)
        {
            try
            {
                var value = User.FindFirstValue("userId");
                int UserId = int.Parse(value);
                var result = await collaborationBL.GetCollaborationId(CollaboratorId,UserId);
                if (result != null)
                {
                    var response = new ResponseModel<IEnumerable<CollaborationInfoModel>>
                    {
                        Message = "Collaborator Fetched Successfully",
                        Data = result
                    };
                    return Ok(response);
                }
                var respons = new ResponseModel<IEnumerable<CollaborationInfoModel>>
                {   Success = false,
                    Message = "User Not Found",
                    Data = result
                };
                return NotFound(respons);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message,

                };
                return Ok(response);
            }
        }


       
        [HttpGet]
        public async Task<IActionResult> GetCollaborator()
        {
            try
            {
                var value = User.FindFirstValue("userId");
                int UserId = int.Parse(value);

                var collaborators = await collaborationBL.GetAllCollaborators(UserId);
                if (collaborators != null)
                {
                    var response = new ResponseModel<IEnumerable<CollaborationInfoModel>>
                    {
                        Message = "Collaborators Fetched Successfully",
                        Data = collaborators
                    };
                    return Ok(response);
                }
                var respons = new ResponseModel<IEnumerable<CollaborationInfoModel>>
                {
                    Success=false,
                    Message = "User Not Found",
                    Data = collaborators
                };
                return NotFound(respons);

            }
            catch (Exception ex)
            {
                var response = new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message,

                };
                return Ok(response);

            }

        }
        [HttpDelete("{CollabId}")]
        public async Task<IActionResult> DeleteCollaborator(int CollabId)
        {
            try
            {
                var value = User.FindFirstValue("userId");
                int UserId = int.Parse(value);
                var result=await collaborationBL.DeleteCollaborator(CollabId,UserId);
                if (result)
                {
                    var response = new ResponseModel<bool>
                    {
                        
                        Message = "Collaborator Deleted successfully",
                        Data = result
                    };
                    return Ok(response);
                }
                var respons = new ResponseModel<bool>
                {
                    Success=false,
                    Message = "User Not Found",
                    Data = result
                };
                return NotFound(respons);

            }
            catch (NotFoundException ex)
            {
                var response = new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
                return NotFound(response);
            }
            catch (Exception)
            {
                var response = new ResponseModel<string>
                {
                    Success = false,
                    Message = "An error occurred while removing the collaborator",
                    Data = null
                };
                return StatusCode(500, response);
            }
        }

    }
}
