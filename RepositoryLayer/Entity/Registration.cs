using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    public class Registration
    {
        public int userId { get; set; }
        public string Firstname {  get; set; }

        public string Lastname { get; set; }

        public string EmailID { get; set; }

        public string password { get; set; }
    }
}
