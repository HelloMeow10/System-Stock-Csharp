using System;

namespace UserManagementSystem.DataAccess.Exceptions
{
    public class DataAccessLayerException : Exception
    {
        public DataAccessLayerException(string message) : base(message)
        {
        }

        public DataAccessLayerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
