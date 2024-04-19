using System.Runtime.Serialization;

namespace RepositoryLayer.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string v)
        {
        }

        public UserNotFoundException(string? message, string v) : base(message)
        {
        }

        public UserNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}