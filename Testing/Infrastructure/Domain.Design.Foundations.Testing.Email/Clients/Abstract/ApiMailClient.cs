using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Domain.Design.Foundations.Testing.Email.Settings;

namespace Domain.Design.Foundations.Testing.Email.Clients.Abstract
{
    public class ApiMailClient : EmailClient
    {
        private IHttpClientFactory _httpClientFactory { get; }
        private Func<HttpClient, Task<IEnumerable<EmailMessage>>> _getEmailsAsyncHandler { get; }
        private Func<HttpClient, Task> _clearInboxAsyncHandler { get; }

        public ApiMailClient(
            EmailClientSettings clientSettings,
            Func<EmailClientSettings, string> hostSelector,
            Func<EmailClientSettings, string> emailAddressSelector,
            IHttpClientFactory httpClientFactory,
            Func<HttpClient, Task<IEnumerable<EmailMessage>>> getEmailsAsyncHandler,
            Func<HttpClient, Task> clearInboxAsyncHandler
        ) : base(
            clientSettings,
            hostSelector,
            emailAddressSelector
        ) =>
            (_httpClientFactory, _getEmailsAsyncHandler, _clearInboxAsyncHandler) =
            (httpClientFactory, getEmailsAsyncHandler, clearInboxAsyncHandler);

        public override async Task<IEnumerable<EmailMessage>> GetEmailsAsync() =>
            await _getEmailsAsyncHandler.Invoke(_httpClientFactory.CreateClient());

        public override async Task ClearInboxAsync() =>
            await _clearInboxAsyncHandler.Invoke(_httpClientFactory.CreateClient());
    }
}