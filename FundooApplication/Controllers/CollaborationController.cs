using BusinessLogicLayer.InterfaceBL;
using BusinessLogicLayer.InterfaceBL;
using BusinessLogicLayer.Mailsender;
using CommonLayer.Model.Collaboration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models;
using System.Security.Claims;

namespace FundooApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollaborationController : ControllerBase
    {
        public readonly ICollaborationBL collaborationBL;

        public CollaborationController(ICollaborationBL collaborationBL)
        {
            this.collaborationBL = collaborationBL;
        }
       // [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddCollaborator(int NoteId,int UserId, [FromBody] CollaborationRequestModel request)
        {
            var result = await collaborationBL.AddCollaborator(NoteId, UserId, request);
            MailSenderClass.sendCollabMail(request.Email);
            if (result)
                return Ok(result);
            else
                return BadRequest("Failed to add Collaboration");
        }
        [HttpGet("{CollaborationId}")]
        public async Task<IActionResult> GetCollaborator(int CollaborationId)
        {
            var result= await collaborationBL.GetCollaborationId(CollaborationId);
            return Ok(result);
        }
        [HttpDelete("{CollabId}")]
        public async Task<IActionResult> DeleteCollaborator(int CollabId)
        {
            return Ok(await collaborationBL.DeleteCollaborator(CollabId));
        }

    }
}
