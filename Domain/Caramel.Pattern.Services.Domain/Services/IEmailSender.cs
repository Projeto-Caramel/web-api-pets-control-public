namespace Caramel.Pattern.Services.Domain.Services
{
    public interface IEmailSender
    {
        Task SendConfirmationEmailAsync(string receiver, string code);
    }
}
