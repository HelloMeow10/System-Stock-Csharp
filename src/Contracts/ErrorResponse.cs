using System.Collections.Generic;

namespace Contracts
{
    /// <summary>
    /// Represents a standardized error response from the API.
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// The primary, user-friendly error message.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// A list of specific validation errors, if any.
        /// </summary>
        public IEnumerable<string>? Errors { get; set; }
    }
}
