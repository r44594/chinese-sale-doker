namespace server_api.Interfaces
{
    
        public interface IEmailService
        {
            Task SendEmailAsync(string toEmail, string subject, string body);
        }
    
}
