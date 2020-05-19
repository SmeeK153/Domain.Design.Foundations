using Newtonsoft.Json;

namespace Domain.Design.Testing.Infrastructure.Email.Clients.GuerrillaMail
{
    public class GuerrillaEmailSummary
    {
        [JsonProperty("mail_id")]
        public string Id { get; set; }
    }
}