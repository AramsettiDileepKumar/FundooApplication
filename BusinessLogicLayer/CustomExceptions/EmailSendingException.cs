using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.CustomExceptions
{
    public class EmailSendingException:Exception
    {
        public EmailSendingException() { }
        public EmailSendingException(string message):base(message) { }
    }
}
