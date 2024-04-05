using CommonLayer.Model;
using CommonLayer.Model.Note;
using CommonLayer.Model.RequestDTO;
using CommonLayer.Model.RequestDTO.ResponseDTO;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.InterfaceBL
{
    public interface IRegistrationBL
    {
        public Task<IEnumerable<UserResponse>> GetDetails();
        public Task<string> AddUser(UserRequest res);
        public Task<string> UpdateUser(Registration res,string FirstName);
        public Task<string> DeleteUser(string FirstName);
        public Task<string> UserLogin(Userlogin userLogin);
        public Task<String> ForgotPassword(String Email);
        public Task<string> ResetPassword(string otp, String password);


    }
}
