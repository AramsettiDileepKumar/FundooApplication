using BusinessLogicLayer.CustomExceptions;
using BusinessLogicLayer.InterfaceBL;
using CommonLayer.Model;
using CommonLayer.Model.Note;
using CommonLayer.Model.RequestDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models;
using RepositoryLayer.Entity;

namespace FundooApplication.Controllers
{
   //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationBL registrationBL;
        public RegistrationController(IRegistrationBL registrationBL)
        {
            this.registrationBL = registrationBL;
        }
        [Route("api/getAllUsers")]
        [HttpGet]
        public async Task<IActionResult> GetDetails()
        {
            try
            {
                return Ok(await registrationBL.GetDetails());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
       // [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> AddUser(UserRequest res)
        {
            try
            {
                return Ok(await registrationBL.AddUser(res));
            }
            catch(Exception ex) 
            {
                return StatusCode(400, ex.StackTrace);
            }
        }
        
        [HttpPut("{FirstName}")]
        public async Task<ActionResult> UpdateUser(Registration res,string FirstName)
        {
            try
            {
                return Ok(await registrationBL.UpdateUser(res, FirstName));
            }
            catch(UserNotFoundException ex) 
            {
                return StatusCode(500,"Enter Valid User Details");
            }
        }
      
        [HttpDelete("{FirstName}")]
        public async Task<ActionResult> DeleteUser(string FirstName)
        {
            try
            {
                return Ok(await registrationBL.DeleteUser(FirstName));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
       [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(Userlogin userLogin)
        {
            try
            {
                

                var token = await registrationBL.UserLogin(userLogin);

                var response = new ResponseModel<string>
                {

                    Message = "Login Sucessfull",
                    Data = token

                };
                return Ok(token);

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
                else if (ex is PasswordMisMatchException)
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
                    return StatusCode(500, $"An error occurred while processing the login request: {ex.Message}");
                }
            }

        }
        [HttpPut("ForgotPassword/{Email}")]
        public async Task<IActionResult> ForgotPassword(String Email)
        {
            return Ok(await registrationBL.ForgotPassword(Email));
        }
        [HttpPut("ResetPassword/{otp}/{Newpassword}")]
        public async Task<IActionResult> ResetPassword(String otp, String Newpassword)
        {
            return Ok(await registrationBL.ResetPassword(otp, Newpassword));
        }

    }
}
