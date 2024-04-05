using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.CustomExceptions
{
    public class PasswordMisMatchException:Exception
    {
        public PasswordMisMatchException() { }
        public PasswordMisMatchException(string message) : base(message) { }
    }
}
