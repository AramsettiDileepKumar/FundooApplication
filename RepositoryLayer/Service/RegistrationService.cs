using CommonLayer.Model;
using CommonLayer.Model.Note;
using CommonLayer.Model.RequestDTO;
using CommonLayer.Model.RequestDTO.ResponseDTO;
using Confluent.Kafka;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Nest;
using NLog;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Exceptions;
using RepositoryLayer.Helper;
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
using ILogger = NLog.ILogger;

namespace RepositoryLayer.Service
{
    public class RegistrationService : IRegistration
    {
        private readonly DapperContext _context;
        private readonly IAuthonticationService _authService;
        private readonly ILogger _logger;
        public RegistrationService(DapperContext context, IAuthonticationService authService)
        {
            _context = context;
            _authService = authService;
            _logger =LogManager.GetCurrentClassLogger();
        }
        public async Task<IEnumerable<Registration>> GetDetails()
        {
            try
            {
                var query = "Select * from Registration";
                using (var connection = _context.CreateConnection())
                {
                    _logger.Info("Get Details Executed");
                    return await connection.QueryAsync<Registration>(query);
                }
            }
            catch (Exception ex) 
            {
                _logger.Error(ex, "User Not Found");
                throw;
            }
        }
        public async Task<bool> AddUser(Registration res)
        {
            try
            {
                var query = $"insert into Registration values(@FirstName,@LastName,@EmailId,@Password)";
                using (var connection = _context.CreateConnection())
                {
                     var result = await connection.ExecuteAsync(query, res);
                     _logger.Info("New User Added");
                      helper h = new helper();
                      h.producer(res);
                      return result > 0;
                }
            }
            catch(Exception ex) 
            {
                _logger.Error(ex, "Error Occurred While Creating User");
                throw new Exception(ex.Message);    
            }
        }
        public async Task<Registration> GetByEmail(string email)
        {
            try
            {
                var query = "Select * from Registration where EmailID=@email";
                var connection = _context.CreateConnection();
                _logger.Info("GetByEmail Executed");
                return await connection.QueryFirstAsync<Registration>(query, new { email = email });
            }
            catch(Exception ex)
            {
                _logger.Error(ex, "Error Occurred while Getting details By Email");
                throw new Exception(ex.Message);
            }
            
        }
        public async Task<bool> UpdateUser(UserRequest res, string firstName)
        {
            try
            {
                var query = "UPDATE Registration SET Firstname = @FirstName, Lastname =@LastName, EmailId = @EmailId ,Password=@Password WHERE Firstname=@name";
                var connection = _context.CreateConnection();
                var result = await connection.ExecuteAsync(query, new { FirstName = res.UserFirstName, LastName = res.UserLastName, EmailId = res.UserEmail, Password = res.UserPassword, name = firstName });
                _logger.Info("Update User Executed");
                return result > 0;
            }
            catch(Exception ex)
            {
                _logger.Error(ex, "Error Occurred While Executing the Update User ");
                throw new Exception(ex.Message);
            }    
        }
        public async Task<bool> DeleteUser(string FirstName)
        {
            try
            {
                var query = "DELETE NC FROM NoteCollaborators AS NC INNER JOIN Notes AS N ON NC.NoteId = N.NoteId INNER JOIN Registration AS R ON N.UserId = R.UserId WHERE R.FirstName = @name; DELETE N FROM Notes AS N INNER JOIN Registration AS R ON N.UserId = R.UserId WHERE R.FirstName = @name;DELETE FROM Registration WHERE FirstName = @name;";
                var connection = _context.CreateConnection();
                var result=await connection.ExecuteAsync(query,new { name = FirstName });
                _logger.Info("Delete User Executed");
                return result > 0;
            }
            catch(Exception ex)
            {
                _logger.Error(ex, "Error Occurred While Executing Delete User");
                throw new Exception(ex.Message);
            }
            
        }
        public async Task<string> UserLogin(Userlogin userslogin)
        {
            try
            {
                string query = @"SELECT * FROM Registration WHERE EmailID = @Email;";
                var connection = _context.CreateConnection();
                var user = await connection.QueryFirstOrDefaultAsync<Registration>(query, new { Email = userslogin.Email });
                if (user == null)
                {
                    throw new UserNotFoundException($"User with email '{userslogin.Email}' not found.");
                }
                _logger.Info("Login Executed");
                return _authService.GenerateJwtToken(user);
            }
            catch(Exception ex)
            {
                _logger.Error(ex, "Error Occurred while Login");
                throw new Exception(ex.Message);
            }
            
        }
        public async Task<int> UpdatePassword(string mailid, string password)
        {
            try
            {
                String Query = "update Registration set Password = @Password where EmailID = @mail";
                IDbConnection connection = _context.CreateConnection();
                return await connection.ExecuteAsync(Query, new { mail = mailid, Password = password });
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
