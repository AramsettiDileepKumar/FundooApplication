using CommonLayer.Model;
using CommonLayer.Model.Note;
using CommonLayer.Model.RequestDTO;
using Dapper;
using Microsoft.IdentityModel.Tokens;
using Nest;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface.UserService;
using RepositoryLayer.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class RegistrationService : IRegistration
    {
        private readonly DapperContext _context;
        private readonly IAuthonticationService _authService;
        public RegistrationService(DapperContext context, IAuthonticationService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<IEnumerable<Registration>> GetDetails()
        {
            var query = "Select * from Registration";
            using (var connection = _context.CreateConnection())
            {
                var register = await connection.QueryAsync<Registration>(query);
                return register.ToList();
            }
        }
        public async Task<string> AddUser(Registration res)
        {
            var query = $"insert into Registration values(@FirstName,@LastName,@EmailId,@Password)";
            using (var connection = _context.CreateConnection())
            {
                var employee = await connection.ExecuteAsync(query, res);
                return "User Added Successfully";
            }
        }
        public async Task<Registration> GetByEmailAsync(string email)
        {
            var query = "Select * from Registration where EmailID=@email";
            using (var connection = _context.CreateConnection())
            {

                return await connection.QueryFirstAsync<Registration>(query, new { email = email });
            }
        }
        public async Task<string> UpdateUser(Registration res, string firstName)
        {
            var query = "UPDATE Registration SET Firstname = @FirstName, Lastname =@LastName, EmailId = @EmailId ,Password=@Password WHERE Firstname=@name";
            using (var connection = _context.CreateConnection())
            {
                var affectedRows = await connection.ExecuteAsync(query, new { FirstName = res.Firstname, LastName = res.Lastname, EmailId = res.EmailID, Password = res.password, name = firstName });
                return "User Updated Successfully";

            }
        }
        public async Task<string> DeleteUser(string FirstName)
        {
            var query = "Delete from Registration where Firstname=@name";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { name = FirstName });
                return "User Deleted Successfully";
            }
        }
        public async Task<string> UserLogin(Userlogin userslogin)
        {

            var parameters = new DynamicParameters();
            parameters.Add("Email", userslogin.Email);


            string query = @"
                            SELECT *
                            FROM Registration 
                            WHERE EmailID = @Email;
                            ";

            using (var connection = _context.CreateConnection())
            {
                var user = await connection.QueryFirstOrDefaultAsync<Registration>(query, parameters);
                
                if (user == null)
                {
                    throw new InvalidDataException($"User with email '{userslogin.Email}' not found.");
                }
                string token = _authService.GenerateJwtToken(user);
                return token;
            }
        }
        public async Task<int> UpdatePassword(string mailid, string password)
        {
            String Query = "update Registration set Password = @Password where EmailID = @mail";
            IDbConnection connection = _context.CreateConnection();
            return await connection.ExecuteAsync(Query, new { mail = mailid, Password = password });
        }
    }
}
