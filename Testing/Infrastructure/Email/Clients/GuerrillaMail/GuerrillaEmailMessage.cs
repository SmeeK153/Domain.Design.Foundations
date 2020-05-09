using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Domain.Design.Testing.Infrastructure.Email.Clients.GuerrillaMail
{
    public class GuerrillaEmailMessage
    {
        [JsonProperty("mail_from")]
        public string From { get; set; }
        
        [JsonProperty("mail_subject")]
        public string Subject { get; set; }
        
        [JsonProperty("mail_timestamp")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime Received { get; set; }
        
        [JsonProperty("mail_body")]
        public string Body { get; set; }
        
        [JsonProperty("mail_recipient")]
        public string To { get; set; }
    }
}