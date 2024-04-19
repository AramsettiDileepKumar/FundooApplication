using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.CustomExceptions
{
    public class InvalidEmailFormatException:Exception
    {
        public InvalidEmailFormatException() { }
        public InvalidEmailFormatException(string message):base(message) { }
    }
}
