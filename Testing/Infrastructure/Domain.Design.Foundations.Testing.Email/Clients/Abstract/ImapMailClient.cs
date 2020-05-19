using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Design.Testing.Infrastructure.Email.Settings;

namespace Domain.Design.Testing.Infrastructure.Email.Clients.Abstract
{
    public class ImapMailClient : EmailClient
    {
        public ImapMailClient(EmailClientSettings clientSettings, Func<EmailClientSettings, string> hostSelector, Func<EmailClientSettings, string> emailAddressSelector) : base(clientSettings, hostSelector, emailAddressSelector)
        {
        }

        public override Task<IEnumerable<EmailMessage>> GetEmailsAsync()
        {
            throw new NotImplementedException();
        }

        public override Task ClearInboxAsync()
        {
            throw new NotImplementedException();
        }
    }
}