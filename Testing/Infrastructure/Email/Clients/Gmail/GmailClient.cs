using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Design.Testing.Infrastructure.Email.Settings;
using MailKit;
using MailKit.Net.Imap;
using MimeKit;

namespace Domain.Design.Testing.Infrastructure.Email.Clients.Gmail
{
    public class GmailClient : EmailClient
    {
        private async Task<ImapClient> GetClientAsync()
        {
            var client = new ImapClient
            {
                ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true
            };
            await client.ConnectAsync(Host, Port, true);
            await client.AuthenticateAsync(ClientSettings.EmailAddress, ClientSettings.Password);
            await client.Inbox.OpenAsync(FolderAccess.ReadWrite);
            return client;
        }

        private static readonly Func<EmailClientSettings, string> HostSelector = (EmailClientSettings settings) =>
        {
            return settings.ServerType switch
            {
                EmailServerType.POP3 => "pop.gmail.com",
                EmailServerType.IMAP => "imap.gmail.com",
                _ => throw new InvalidOperationException(
                    $"{nameof(GmailClient)} does not support the {nameof(EmailServerType)} {settings.ServerType}")
            };
        };

        private static readonly Func<EmailClientSettings, string> EmailAddressSelector = (EmailClientSettings settings) => settings.EmailAddress;
        
        public GmailClient(EmailClientSettings clientSettings) : base(clientSettings, HostSelector, EmailAddressSelector)
        {
            if (!clientSettings.IsAuthenticated)
            {
                throw new InvalidOperationException($"{nameof(EmailClientSettings)} must provide the password for {nameof(GmailClient)}");
            }
        }

        private async Task<IEnumerable<EmailMessage>> GetEmailsViaPop3Async()
        {
            // TODO: Add implementation
            throw new NotImplementedException();
        }
        
        private async Task<IEnumerable<EmailMessage>> GetEmailsViaImapAsync()
        {
            using var client = await GetClientAsync();
            return client.Inbox
                .Cast<MimeMessage>()
                .OrderByDescending(message => message.Date)
                .Select(message => new EmailMessage
                {
                    // TODO: Map EmailMessage to include content
                });
        }

        public override async Task<IEnumerable<EmailMessage>> GetEmailsAsync()
        {
            Func<Task<IEnumerable<EmailMessage>>> action = ClientSettings.ServerType switch
            {
                EmailServerType.POP3 => GetEmailsViaPop3Async,
                EmailServerType.IMAP => GetEmailsViaImapAsync,
                _ => null
            };

            if (action is null)
            {
                return new List<EmailMessage>();
            }
            return await action.Invoke();
        }

        private async Task ClearInboxViaPop3Async()
        {
            // TODO: Add implementation
            throw new NotImplementedException();
        }
        
        private async Task ClearInboxViaImapAsync()
        {
            using var client = await GetClientAsync();
            var messages = await client.Inbox
                .FetchAsync(0, -1, MessageSummaryItems.UniqueId | MessageSummaryItems.Headers);
            var uniqueIds = messages.Select(message => message.UniqueId).ToList();
            await client.Inbox.AddFlagsAsync(uniqueIds, MessageFlags.Deleted, true);
        }

        public override async Task ClearInboxAsync()
        {
            Func<Task> action = ClientSettings.ServerType switch
            {
                EmailServerType.POP3 => ClearInboxViaPop3Async,
                EmailServerType.IMAP => ClearInboxViaImapAsync,
                _ => null
            };

            if (action != null)
            {
                await action.Invoke();
            }
        }
    }
}