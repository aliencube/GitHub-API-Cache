using Aliencube.GitHub.Cache.Services.Interfaces;
using Aliencube.GitHub.Cache.Services.Properties;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Aliencube.GitHub.Cache.Services.Helpers
{
    /// <summary>
    /// This represents the email helper entity.
    /// </summary>
    public class EmailHelper : IEmailHelper
    {
        private readonly Settings _settings;

        /// <summary>
        /// Initialises a new instance of the EmailHelper class.
        /// </summary>
        public EmailHelper()
        {
            this._settings = Settings.Default;
        }

        /// <summary>
        /// Sends an email.
        /// </summary>
        /// <param name="from">Email address that sends the email.</param>
        /// <param name="to">Email address that receives the email.</param>
        /// <param name="subject">Subject of the email.</param>
        /// <param name="body">Body message of the email.</param>
        /// <exception cref="ArgumentNullException">Throws when any parameter is <c>null</c> or empty.</exception>
        public void Send(string from, string to, string subject, string body)
        {
            if (String.IsNullOrWhiteSpace(from))
                throw new ArgumentNullException("from");

            if (String.IsNullOrWhiteSpace(to))
                throw new ArgumentNullException("to");

            if (String.IsNullOrWhiteSpace(subject))
                throw new ArgumentNullException("subject");

            if (String.IsNullOrWhiteSpace(body))
                throw new ArgumentNullException("body");

            this.Send(new MailAddress(from), new MailAddress(to), subject, body);
        }

        /// <summary>
        /// Sends an email,
        /// </summary>
        /// <param name="from">Email address that sends the email.</param>
        /// <param name="to">Email address that receives the email.</param>
        /// <param name="subject">Subject of the email.</param>
        /// <param name="body">Body message of the email.</param>
        /// <param name="attachments">List of attachments.</param>
        /// <exception cref="ArgumentNullException">Throws when any parameter is <c>null</c> or empty.</exception>
        public void Send(MailAddress from, MailAddress to, string subject, string body, IEnumerable<Attachment> attachments = null)
        {
            if (from == null)
                throw new ArgumentNullException("from");

            if (to == null)
                throw new ArgumentNullException("to");

            if (String.IsNullOrWhiteSpace(subject))
                throw new ArgumentNullException("subject");

            if (String.IsNullOrWhiteSpace(body))
                throw new ArgumentNullException("body");

            using (var smtp = new SmtpClient())
            using (var message = new MailMessage(from, to))
            {
                message.Subject = subject;
                message.SubjectEncoding = Encoding.UTF8;
                message.Body = body;
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;

                if (attachments != null)
                {
                    foreach (var attachment in attachments)
                    {
                        message.Attachments.Add(attachment);
                    }
                }

                smtp.Send(message);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }
    }
}