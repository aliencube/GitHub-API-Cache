using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace Aliencube.GitHub.Cache.Services.Interfaces
{
    /// <summary>
    /// This provides interfaces to the EmailHelper class.
    /// </summary>
    public interface IEmailHelper : IDisposable
    {
        /// <summary>
        /// Sends an email.
        /// </summary>
        /// <param name="from">Email address that sends the email.</param>
        /// <param name="to">Email address that receives the email.</param>
        /// <param name="subject">Subject of the email.</param>
        /// <param name="body">Body message of the email.</param>
        /// <exception cref="ArgumentNullException">Throws when any parameter is <c>null</c> or empty.</exception>
        void Send(string from, string to, string subject, string body);

        /// <summary>
        /// Sends an email,
        /// </summary>
        /// <param name="from">Email address that sends the email.</param>
        /// <param name="to">Email address that receives the email.</param>
        /// <param name="subject">Subject of the email.</param>
        /// <param name="body">Body message of the email.</param>
        /// <param name="attachments">List of attachments.</param>
        /// <exception cref="ArgumentNullException">Throws when any parameter is <c>null</c> or empty.</exception>
        void Send(MailAddress from, MailAddress to, string subject, string body, IEnumerable<Attachment> attachments = null);
    }
}