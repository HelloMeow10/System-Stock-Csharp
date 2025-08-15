using System;

namespace Presentation.Exceptions
{
    public class UILayerException : Exception
    {
        public UILayerException(string message) : base(message)
        {
        }

        public UILayerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
