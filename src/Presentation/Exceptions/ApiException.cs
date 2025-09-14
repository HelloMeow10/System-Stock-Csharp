using System;
using System.Collections.Generic;
using System.Net;
using Contracts;

namespace Presentation.Exceptions
{
    /// <summary>
    /// Represents an exception thrown when an API call fails.
    /// </summary>
    public class ApiException : Exception
    {
        /// <summary>
        /// The HTTP status code returned by the API.
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// A collection of specific error messages from the API response.
        /// </summary>
        public IEnumerable<string> Errors { get; }

        public ApiException(string message, HttpStatusCode statusCode, IEnumerable<string>? errors = null)
            : base(message)
        {
            StatusCode = statusCode;
            Errors = errors ?? new List<string>();
        }
    }
}
