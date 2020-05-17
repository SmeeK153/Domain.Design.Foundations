using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Domain.Design.Testing.Infrastructure.Email.Settings;

namespace Domain.Design.Testing.Infrastructure.Email.Clients
{
    public abstract class EmailClient
    {
        public EmailClientSettings ClientSettings { get; }
        public virtual string Host { get; }
        public virtual string EmailAddress { get; }
        protected int Port { get; }

        protected EmailClient(
            EmailClientSettings clientSettings, 
            Func<EmailClientSettings, string> hostSelector, 
            Func<EmailClientSettings, string> emailAddressSelector)
        {
            ClientSettings = clientSettings;
            
            Port = ClientSettings.ServerType switch
            {
                EmailServerType.POP3 => ClientSettings.IsAuthenticated ? 995 : 110,
                EmailServerType.IMAP => ClientSettings.IsAuthenticated ? 993 : 143,
                EmailServerType.API => ClientSettings.IsAuthenticated ? 443 : 8080,
                _ => throw new InvalidEnumArgumentException(
                    $"{ClientSettings.ServerType.ToString()} is not a supported {nameof(EmailServerType)}")
            };

            if (hostSelector != null)
            {
                Host = hostSelector.Invoke(clientSettings);
            }

            if (emailAddressSelector != null)
            {
                EmailAddress = emailAddressSelector.Invoke(clientSettings);
            }
        }

        protected EmailClient(EmailClientSettings clientSettings) : 
            this(clientSettings, null, null)
        {
        }

        public abstract Task<IEnumerable<EmailMessage>> GetEmailsAsync();

        public abstract Task ClearInboxAsync();
    }
}