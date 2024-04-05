using CommonLayer.Model;
using CommonLayer.Model.Note;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface.UserService
{
    public interface IRegistration
    {
        public Task<IEnumerable<Registration>> GetDetails();
        public Task<string> AddUser(Registration res);
        public Task<string> UpdateUser(Registration res, string FirstName);

        public Task<string> DeleteUser(string FirstName);
        public Task<string> UserLogin(Userlogin userLogin);
        public Task<int> UpdatePassword(string mailid, string password);
        public Task<Registration> GetByEmailAsync(string Email);
    }
}
