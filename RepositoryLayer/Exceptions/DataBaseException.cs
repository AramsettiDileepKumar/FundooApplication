using System.Runtime.Serialization;

namespace RepositoryLayer.Exceptions
{
    [Serializable]
    internal class DataBaseException : Exception
    {
        public DataBaseException()
        {
        }

        public DataBaseException(string? message) : base(message)
        {
        }

        public DataBaseException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DataBaseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}