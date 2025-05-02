namespace MoneyTracker.Infrastructure.Providers;

using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Mail;
using Ardalis.GuardClauses;
using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Entities.Config;
using MoneyTracker.Domain.Interfaces;

[ExcludeFromCodeCoverage]
public class EmailProvider(
    MoneyTrackerSettings appSettings
) : IEmailProvider
{
    private readonly string smtpServer = Guard.Against.Null(appSettings.Smtp.Server);
    private readonly int smtpPort = Guard.Against.Null(appSettings.Smtp.Port);
    private readonly string smtpUser = Guard.Against.Null(appSettings.Smtp.Username);
    private readonly string smtpPassword = Guard.Against.Null(appSettings.Smtp.Password);

    private MailMessage GetMailMessage(EmailPayload payload)
    {
        MailMessage mail = new()
        {
            From = new MailAddress(smtpUser),
            Subject = payload.Subject,
            Body = payload.Body,
            IsBodyHtml = true
        };

        foreach (var recipient in payload.Recipients)
            mail.To.Add(recipient);

        if (payload.RecipientsCc != null)
        {
            foreach (var recipientCc in payload.RecipientsCc)
                mail.CC.Add(recipientCc);
        }

        return mail;
    }

    private SmtpClient GetSmtpClient()
    {
        return new(smtpServer)
        {
            Port = smtpPort,
            EnableSsl = true,
            Credentials = new NetworkCredential(smtpUser, smtpPassword)
        };
    }

    private static bool TrySendEmail(MailMessage mail, SmtpClient smtpClient, out string status)
    {
        try
        {
            smtpClient.Send(mail);
            status = "Email sent successfully";
            return true;
        }
        catch (Exception ex)
        {
            status = ex.Message;
            return false;
        }
    }

    public bool SendEmail(EmailPayload payload, out string status)
    {
        if (payload.Recipients.Length == 0)
            throw new ArgumentNullException(nameof(payload), "At least one recipient is required");

        var mail = GetMailMessage(payload);
        var smtpClient = GetSmtpClient();

        return TrySendEmail(mail, smtpClient, out status);
    }
}