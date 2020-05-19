using System.ComponentModel;
using System.Net.Http;
using Domain.Design.Testing.Infrastructure.Email.Clients.Gmail;
using Domain.Design.Testing.Infrastructure.Email.Clients.GuerrillaMail;
using Domain.Design.Testing.Infrastructure.Email.Settings;

namespace Domain.Design.Testing.Infrastructure.Email.Clients
{
    public class EmailClientFactory
    {
        private IHttpClientFactory _httpClientFactory { get; }

        public EmailClientFactory(IHttpClientFactory httpClientFactory) => (_httpClientFactory) = (httpClientFactory);

        public EmailClient BuildClient(EmailClientProvider provider, EmailClientSettings emailClientSettings)
        {
            return provider switch
            {
                EmailClientProvider.Gmail => new GmailClient(emailClientSettings),
                EmailClientProvider.GuerrillaMail => new GuerrillaMailClient(emailClientSettings, _httpClientFactory),
                _ => throw new InvalidEnumArgumentException(
                    $"{provider} is not a supported {nameof(EmailClientProvider)}")
            };
        }

        private EmailClient BuildDynamicClient(EmailClientSettings emailClientSettings)
        {
            return emailClientSettings.ServerType switch
            {
                // EmailServerType.API => new ApiMailClient(),
                // EmailServerType.POP3 => new Pop3MailClient(),
                // EmailServerType.IMAP => new ImapMailClient(),
                _ => throw new InvalidEnumArgumentException(
                    $"{emailClientSettings.ServerType} is not a supported {nameof(EmailServerType)}")
            };
        }
    }
}