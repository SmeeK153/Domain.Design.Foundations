namespace Domain.Design.Foundations.Testing.Email.Settings
{
    public class EmailClientSettings
    {
        public EmailServerType ServerType { get; }
        public string EmailAddress { get; }
        public bool IsAuthenticated => !string.IsNullOrEmpty(Password);
        public string Password { get; } = string.Empty;

        public EmailClientSettings(EmailServerType emailServerType, string emailAddress)
        {
            ServerType = emailServerType;
            EmailAddress = emailAddress;
        }

        public EmailClientSettings(EmailServerType emailServerType, string emailAddress, string password) : 
            this(emailServerType, emailAddress)
        {
            Password = password;
        }
    }
}