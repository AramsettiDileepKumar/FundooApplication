using BusinessLogicLayer.CustomExceptions;
using BusinessLogicLayer.InterfaceBL;
using BusinessLogicLayer.Mailsender;
using CommonLayer.Model;
using CommonLayer.Model.Note;
using CommonLayer.Model.RequestDTO;
using CommonLayer.Model.RequestDTO.ResponseDTO;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface.UserService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Dapper.SqlMapper;
using static System.Net.WebRequestMethods;

namespace BusinessLogicLayer.ServiceBL.UserService
{
    public class RegistrationServiceBL : IRegistrationBL
    {
        private readonly IRegistration register;

        private static string otp;
        private static string mailid;
        private static Registration entity;

        public RegistrationServiceBL(IRegistration register)
        {
            this.register = register;
        }
        public async Task<IEnumerable<UserResponse>> GetDetails()
        {
            var registrations = await register.GetDetails();
            var userResponses = new List<UserResponse>();

            foreach (var registration in registrations)
            {
                var userResponse = MapToResponse(registration);
                userResponses.Add(userResponse);
            }

            return userResponses;

        }
        private UserResponse MapToResponse(Registration response)
        {
            return new UserResponse
            {
                FirstName = response.Firstname,
                LastName = response.Lastname,
                Email = response.EmailID,
            };
        }

        public Task<string> AddUser(UserRequest res)
        {
            string EncrpPassword = Encrypt(res.UserPassword);
            res.UserPassword = EncrpPassword;
            Registration entity = MapToEntity(res);
            return register.AddUser(entity);
        }
        private Registration MapToEntity(UserRequest request)
        {
            return new Registration
            {
                Firstname = request.UserFirstName,
                Lastname = request.UserLastName,
                EmailID = request.UserEmail,
                password = Encrypt(request.UserPassword)

            };
        }
        public Task<string> UpdateUser(Registration res, string FirstName)
        {
            string EncrpPassword = Encrypt(res.password);
            res.password = EncrpPassword;
            return register.UpdateUser(res, FirstName);
        }
        public Task<string> DeleteUser(string FirstName)
        {
            return register.DeleteUser(FirstName);
        }
        public async Task<string> UserLogin(Userlogin userLogin)
        {
            if (userLogin == null)
            {
                throw new ArgumentNullException(nameof(userLogin), "Userlogin object cannot be null.");
            }

            string mail = userLogin.Email;
            string pass = userLogin.Password;

            // Retrieve user from the repository
            Registration user = await register.GetByEmailAsync(mail);

            if (user == null)
            {
                throw new UserNotFoundException($"User with email '{mail}' not found.");
            }

            // Decrypt the stored password
            string decryptedPass = Decrypt(user.password);

            if (pass.Equals(decryptedPass))
            {
                // Passwords match, proceed with login
                return await register.UserLogin(userLogin);
            }
            else
            {
                // Passwords do not match, throw an exception
                throw new PasswordMisMatchException("Incorrect password");
            }
        }
        public Task<string> ForgotPassword(string Email)
        {
            try
            {
                entity = register.GetByEmailAsync(Email).Result;
            }
            catch (Exception e)
            {
                throw new UserNotFoundException("UserNotFoundByEmailId" + e.Message);
            }

            Random r = new Random();
            otp = r.Next(100000, 999999).ToString();
            mailid = Email;
            MailSenderClass.sendMail(Email, otp);
            Console.WriteLine(otp);
            return Task.FromResult("OTP sent");

        }
        public Task<string> ResetPassword(string otp, string userpassword)
        {
            if (otp.Equals(null))
            {
                return Task.FromResult("Generate Otp First");
            }
            if (Decrypt(entity.password).Equals(userpassword))
            {

                throw new PasswordMisMatchException("Dont give the existing password");
            }
            if (Regex.IsMatch(userpassword, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])[a-zA-Z\d!@#$%^&*]{8,16}$"))
            {

                if (RegistrationServiceBL.otp.Equals(otp))
                {
                    if (register.UpdatePassword(mailid, Encrypt(userpassword)).Result == 1)
                    {
                        entity = null; otp = null; mailid = null;
                        return Task.FromResult("password changed successfully");
                    }
                    else
                    {
                        return Task.FromResult("password not changed");
                    }
                }

                else
                {
                    return Task.FromResult("otp miss matching");
                }
            }
            else
            {
                return Task.FromResult("regex is mismatching");
            }
        }
        public string Encrypt(string password)
        {
            byte[] refence = Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(refence);
        }
        public string Decrypt(string password)
        {
            byte[] passbyte = Convert.FromBase64String(password);
            string res = Encoding.UTF8.GetString(passbyte);
            return res;
        }
    }
}
