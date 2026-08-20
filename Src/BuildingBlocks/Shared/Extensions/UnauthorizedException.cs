using Shared.Contract.Exeption;
using System.Net;

namespace Shared.Extensions
{
    public class UnauthorizedException : CustomException
    {
        public UnauthorizedException()
       : base("Unauthorized access.", Array.Empty<string>(), HttpStatusCode.Unauthorized)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForbiddenException"/> class with specified message.
        /// </summary>
        /// <param name="message">The error message describing the forbidden action.</param>
        public UnauthorizedException(string message)
            : base(message, Array.Empty<string>(), HttpStatusCode.Unauthorized)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForbiddenException"/> class with message and error details.
        /// </summary>
        /// <param name="message">The main error message.</param>
        /// <param name="errors">Collection of detailed error messages.</param>
        public UnauthorizedException(string message, IEnumerable<string> errors)
            : base(message, errors.ToList(), HttpStatusCode.Unauthorized)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorizedException"/> class with message and inner exception.
        /// </summary>
        /// <param name="message">The error message describing the unauthorized action.</param>
        /// <param name="innerException">The inner exception that caused this exception.</param>
        public UnauthorizedException(string message, Exception innerException)
            : base(message, innerException, HttpStatusCode.Unauthorized)
        {
        }
    }
}
