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
        public Task<bool> AddUser(UserRequest res);
        public Task<bool> UpdateUser(UserRequest res,string FirstName);
        public Task<bool> DeleteUser(string FirstName);
        public Task<string> UserLogin(Userlogin userLogin);
        public Task<string> ForgotPassword(string Email);
        public Task<string> ResetPassword(string otp, string password);


    }
}
