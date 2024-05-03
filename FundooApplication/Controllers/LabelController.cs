using BusinessLogicLayer.InterfaceBL.Labels;
using CommonLayer.Model.Label;
using CommonLayer.Model.ResponseDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models;
using Nest;
using RepositoryLayer.Entity;
using RepositoryLayer.Service;
using System.Security.Claims;

namespace FundooApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LabelController : ControllerBase
    {
        private readonly INotesLabelBL notesLabelBL;
        public LabelController(INotesLabelBL notesLabelBL)
        {
            this.notesLabelBL = notesLabelBL;
        }
        [HttpPost]
        public async Task<IActionResult> CreateLabel(LabelModel label)
        {
            try
            {
                var value = User.FindFirstValue("userId");
                int UserId = int.Parse(value);
                var result = await notesLabelBL.CreateLabel(label, UserId);
                if (result)
                {
                    var response = new ResponseModel<bool>
                    {
                        Success = true,
                        Message = "Label Created Successfully",
                        Data = result
                    };
                    return Ok(response);
                }
                var respons = new ResponseModel<bool>
                {
                    Success = false,
                    Message = "Label Not Found",
                    Data = result
                };
                return NotFound(respons);

            }
            catch (Exception ex)
            {
                return Ok( new ResponseModel<string>
                {
                    Success = false,
                    Message = $"An error occurred {ex.Message}",
                    Data = null
                });
            }
        }
        [HttpPut("{LabelId}")]
        public async Task<IActionResult> UpdateLabel(int LabelId, LabelNameModel name)
        {
            try
            {
                var value = User.FindFirstValue("userId");
                int UserId = int.Parse(value);
                var result = await notesLabelBL.UpdateLabel(LabelId, UserId, name);
                if (result)
                {
                    var response = new ResponseModel<bool>
                    {
                        Success = true,
                        Message = "Label Updated Successfully",
                        Data = result
                    };
                    return Ok(response);
                }
                var respons = new ResponseModel<bool>
                {
                    Success =false,
                    Message = "Label Not found",
                    Data = result
                };
                return NotFound(respons);

            }
            catch (Exception ex)
            {
                return Ok(new ResponseModel<string>
                {
                    Success = false,
                    Message = $"An error occurred {ex.Message}",
                    Data = null
                });
            }
        }
        [HttpDelete("{LabelId}")]
        public async Task<IActionResult> DeleteLabel(int LabelId)
        {
            try
            {
                var value = User.FindFirstValue("userId");
                int UserId = int.Parse(value);
                var result = await notesLabelBL.DeleteLabel(LabelId,UserId);
                if (result)
                {
                    var response = new ResponseModel<bool>
                    {
                        Success = true,
                        Message = "Label Deleted Successfully",
                        Data = result
                    };
                    return Ok(response);
                }
                var respons = new ResponseModel<bool>
                {
                    Success=false,
                    Message = "Label Not Found",
                    Data = result
                };
                return NotFound(respons);
            }
            catch (Exception ex)
            {
                return Ok( new ResponseModel<string>
                {
                    Success = false,
                    Message = $"An error occurred {ex.Message}",
                    Data = null
                });
            }
        }
        [HttpGet("{LabelId}")]
        public async Task<IActionResult> GetLabel(int LabelId)
        {
            try
            {
                var result=await notesLabelBL.getLabel(LabelId);
                if (result != null)
                {
                    var response = new ResponseModel<IEnumerable<LabelInfo>>
                    {
                        Success=true,
                        Message = "Label Details Fetched Successfully",
                        Data = result
                    };
                    return Ok(response);
                }
                var respons = new ResponseModel<IEnumerable<LabelInfo>>
                {
                    Success=true,
                    Message = "Label Not Found",
                    Data = result
                };
                return NotFound(respons);

            }
            catch (Exception ex)
            {
                return Ok(new ResponseModel<Notes> {Success=false,Message=ex.Message});
            }
        }

    }
}
