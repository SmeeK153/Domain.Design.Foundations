using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Domain.Design.Foundations.Testing.Email.Clients.Gmail;
using Domain.Design.Foundations.Testing.Email.Settings;
using Newtonsoft.Json;

namespace Domain.Design.Foundations.Testing.Email.Clients.GuerrillaMail
{
    public class GuerrillaMailClient : EmailClient
    {
        public override string Host { get; }
        public override string EmailAddress => $"{_emailAlias}@sharklasers.com";
        private string _emailAlias { get; set; }
        private IHttpClientFactory _httpClientFactory { get; }   
        public GuerrillaMailClient(
            EmailClientSettings clientSettings, 
            IHttpClientFactory httpClientFactory
        ) : base(clientSettings)
        {
            Host = ClientSettings.ServerType switch
            {
                EmailServerType.API => "https://api.guerrillamail.com/ajax.php",
                _ => throw new InvalidOperationException(
                    $"{nameof(GmailClient)} does not support the {nameof(EmailServerType)} {ClientSettings.ServerType}")
            };

            if (clientSettings.EmailAddress is null)
            {
                _emailAlias = Guid.NewGuid().ToString();
            }
            else
            {
                if (clientSettings.EmailAddress.Contains('@'))
                {
                    _emailAlias = clientSettings.EmailAddress.Split('@').FirstOrDefault();
                }
                else
                {
                    _emailAlias = clientSettings.EmailAddress;
                }
            }

            _httpClientFactory = httpClientFactory;
        }

        private async Task<HttpResponseMessage> ExecuteAsync(string command, [AllowNull] Dictionary<string, string>? arguments = null)
        {
            var endpoint = $"{Host}?f={command}&ip=127.0.0.1&agent=foo_bar";
            if (arguments != null)
            {
                foreach (var argument in arguments)
                {
                    endpoint += $"&{argument.Key}={argument.Value}";
                }
            }
            var response = await _httpClientFactory.CreateClient().GetAsync(endpoint);
            return response;
        }

        private async Task GetEmailAddressAsync()
        {
            await ExecuteAsync("get_email_address");
        }

        private async Task UpdateEmailAddressAsync()
        {
            await ExecuteAsync("set_email_user", new Dictionary<string, string>
            {
                {"email_user", _emailAlias}
            });
        }

        public override async Task<IEnumerable<EmailMessage>> GetEmailsAsync()
        {
            var emailListResponse = await ExecuteAsync("get_email_list", new Dictionary<string, string>
            {
                {"offset", "0"}
            });

            var inbox = JsonConvert.DeserializeObject<GuerrillaMailInbox>(await emailListResponse.Content.ReadAsStringAsync());
                
            var guerrillaEmailMessages = await Task.WhenAll(inbox.Emails.Select(async email =>
            {
                var response = await ExecuteAsync("fetch_email", new Dictionary<string, string>
                {
                    {"email_id", email.Id},
                    {"in", _emailAlias}
                });
                return JsonConvert.DeserializeObject<GuerrillaEmailMessage>(await response.Content.ReadAsStringAsync());
            }));
            
            // The email inbox is initialized with a welcome email that has a very old timestamp on it, filter that one out
            var startOfEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToLocalTime();
            var applicableEmails = guerrillaEmailMessages.Where(email => email.Received > startOfEpoch);

            var emails = applicableEmails.Select(email => new EmailMessage
            {
                // TODO: Map EmailMessage to include content
            });
            return emails;
        }
        
        public override Task ClearInboxAsync()
        {
            _emailAlias = Guid.NewGuid().ToString();
            return Task.CompletedTask;
        }
    }
}