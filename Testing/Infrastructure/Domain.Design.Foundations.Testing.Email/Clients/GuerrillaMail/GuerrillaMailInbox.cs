using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.Design.Testing.Infrastructure.Email.Clients.GuerrillaMail
{
    public class GuerrillaMailInbox
    {
        [JsonProperty("list")]
        public List<GuerrillaEmailSummary> Emails { get; set; } = new List<GuerrillaEmailSummary>();
    }
}