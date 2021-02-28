using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.Design.Foundations.Testing.Email.Clients.GuerrillaMail
{
    public class GuerrillaMailInbox
    {
        [JsonProperty("list")]
        public List<GuerrillaEmailSummary> Emails { get; set; } = new List<GuerrillaEmailSummary>();
    }
}