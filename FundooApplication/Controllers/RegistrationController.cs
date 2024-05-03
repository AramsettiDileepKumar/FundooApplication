using BusinessLogicLayer.CustomExceptions;
using BusinessLogicLayer.InterfaceBL;
using CommonLayer.Model;
using CommonLayer.Model.Note;
using CommonLayer.Model.RequestDTO;
using CommonLayer.Model.RequestDTO.ResponseDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models;
using Nest;
using RepositoryLayer.Entity;

namespace FundooApplication.Controllers
{
  
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [EnableCors]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationBL registrationBL;
        public RegistrationController(IRegistrationBL registrationBL)
        {
            this.registrationBL = registrationBL;
        }
        [HttpGet]
        public async Task<IActionResult> GetDetails()
        {
            try
            {
                var result=await registrationBL.GetDetails();
                if (result != null)
                {
                    var response = new ResponseModel<IEnumerable<UserResponse>>
                    {
                        Success = true,
                        Message = "Users  Details Fetched Successfully",
                        Data=result
                        
                    };
                    return Ok(response);
                }
                return BadRequest(new ResponseModel<Registration>
                {
                    Success = false,
                    Message = "User Not Found"
                });
            }
            catch (Exception ex)
            {
                return Ok(new ResponseModel<Registration> { Success = false, Message = ex.Message });            }
            }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> AddUser(UserRequest res)
        {
            try
            {
                var result=await registrationBL.AddUser(res);
                if (result)
                {
                    var response = new ResponseModel<Registration>
                    {
                        Success = true,
                        Message = "User Registration Successful",
                    };
                    return Ok(response);
                }
                else
                {
                    return BadRequest("invalid input");
                }
            }
            catch (Exception ex)
            {
                return Ok(new ResponseModel<Registration> { Success = false, Message = ex.Message });
            }

        }

        [HttpPut("{FirstName}")]
        public async Task<ActionResult> UpdateUser(UserRequest res,string FirstName)
        {
            try
            {
               var result= await registrationBL.UpdateUser(res, FirstName);
                var response = new ResponseModel<bool>
                {
                    Success = true,
                    Message = "User Updated Successfully",
                };
                return Ok(response);

            }
            catch(UserNotFoundException ex) 
            {
                return Ok(new ResponseModel<Registration>{ Success = false, Message = ex.Message });
            }
        }
      
        [HttpDelete("{FirstName}")]
        public async Task<ActionResult> DeleteUser(string FirstName)
        {
            try
            {
                var result=await registrationBL.DeleteUser(FirstName);
                var response = new ResponseModel<bool>
                {
                    Success = true,
                    Message = "User Deleted Successfully",
                    Data = result
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new ResponseModel<Registration> { Success = false, Message = ex.Message });
            }
        }
        
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(Userlogin userLogin)
        {
            try
            { 
                var token=await registrationBL.UserLogin(userLogin);
                var response = new ResponseModel<string>
                {
                    Message = "Login Sucessful",
                    Data = token
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                if (ex is UserNotFoundException)
                {
                    var response = new ResponseModel<Userlogin>
                    {

                        Success = false,
                        Message = ex.Message

                    };
                    return Conflict(response);
                }
                else if (ex is InvalidPasswordException)
                {
                    var response = new ResponseModel<Userlogin>
                    {

                        Success = false,
                        Message = ex.Message

                    };
                    return BadRequest(response);
                }
                else
                {
                    return Ok(new ResponseModel<Registration> { Success = false, Message = ex.Message });

                }
            }


        }
        [HttpPut("ForgotPassword/{Email}")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            try
            {
                var result=await registrationBL.ForgotPassword(Email);
                if (result != null)
                {
                    var response = new ResponseModel<string>
                    {
                        Success = true,
                        Message = "Email sent successfully.",
                        Data = result

                    };
                    return Ok(response);
                }
                else
                {
                    var response = new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Failed to send email.",
                        Data = null
                    };
                    return BadRequest(response);
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
                return Ok(response);
            }
            catch (EmailSendingException ex)
            {
                var response = new ResponseModel<string>
                {

                    Success = false,
                    Message = $"Error sending email: {ex.Message}",
                    Data = null
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<string>
                {
                    Success = false,
                    Message = $"An unexpected error occurred: {ex.Message}",
                };
                return Ok(response);
            }
        }
           
        [HttpPut("ResetPassword/{otp}/{Newpassword}")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string otp, string Newpassword)
        {
            try
            {
                var result=await registrationBL.ResetPassword(otp, Newpassword);
                if (result != null)
                {
                    var response = new ResponseModel<string>
                    {
                        Success = true,
                        Message = "Reset Password  successful.",
                        Data = result
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Failed to Reset",
                        Data = null
                    };
                    return BadRequest(response);
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
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<string>
                {
                    Success = false,
                    Message = $"An unexpected error occurred: {ex.StackTrace}",
                    Data = null
                };
                return Ok(response);
            }
        }
           
        
    }
}
