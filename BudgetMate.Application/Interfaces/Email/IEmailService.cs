using System;

namespace BudgetMate.Application.Interfaces.Email;

public interface IEmailService
{
    Task SendEmail(string reciever);
}
