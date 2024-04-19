using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.CustomExceptions
{
    public class DataBaseException:Exception
    {
        public DataBaseException() { }
        public DataBaseException(string message) : base(message) { }
    }
}
