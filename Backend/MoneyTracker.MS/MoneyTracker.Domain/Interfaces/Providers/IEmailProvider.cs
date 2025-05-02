namespace MoneyTracker.Domain.Interfaces;

using MoneyTracker.Domain.Entities;

public interface IEmailProvider
{
    /// <summary>
    /// Send an email using the provided payload
    /// </summary>
    /// <param name="payload"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    bool SendEmail(EmailPayload payload, out string status);
}