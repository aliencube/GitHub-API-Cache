using System;
using System.Net;
using System.Runtime.Serialization;

namespace Aliencube.GitHub.Cache.Services.Exceptions
{
    /// <summary>
    /// This represents an exception entity that is thrown when GitHub response value is <c>null</c>.
    /// </summary>
    public class GitHubResponseNullException : WebException
    {
        /// <summary>
        /// Iniitialises a new instance of the GitHubResponseNullException class.
        /// </summary>
        public GitHubResponseNullException()
            : base()
        {
        }

        /// <summary>
        /// Iniitialises a new instance of the GitHubResponseNullException class.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public GitHubResponseNullException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Iniitialises a new instance of the GitHubResponseNullException class.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="status">One of the System.Net.WebExceptionStatus values.</param>
        public GitHubResponseNullException(string message, WebExceptionStatus status)
        {
        }

        /// <summary>
        /// Iniitialises a new instance of the GitHubResponseNullException class.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public GitHubResponseNullException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Iniitialises a new instance of the GitHubResponseNullException class.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        /// <param name="status">One of the System.Net.WebExceptionStatus values.</param>
        /// <param name="response">A System.Net.WebResponse instance that contains the response from the remote host.</param>
        public GitHubResponseNullException(string message, Exception innerException, WebExceptionStatus status, WebResponse response)
        {
        }

        /// <summary>
        /// Iniitialises a new instance of the GitHubResponseNullException class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected GitHubResponseNullException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}