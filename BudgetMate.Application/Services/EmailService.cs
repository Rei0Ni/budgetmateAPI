using System;
using BudgetMate.Application.Interfaces.Email;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BudgetMate.Application.Services;

public class EmailService : IEmailService
{
    public async Task SendEmail(string reciever)
    {
        var apiKey = Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY");
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress("test@example.com", "Example User");
        var subject = "Sending with SendGrid is Fun";
        var to = new EmailAddress(reciever);
        var plainTextContent = "";
        var htmlContent = "";
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        var response = await client.SendEmailAsync(msg);
    }
}
