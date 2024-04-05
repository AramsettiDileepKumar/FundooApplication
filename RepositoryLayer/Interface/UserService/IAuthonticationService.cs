using CommonLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface.UserService
{
    public interface IAuthonticationService
    {
        public string GenerateJwtToken(Registration user);
    }
}
