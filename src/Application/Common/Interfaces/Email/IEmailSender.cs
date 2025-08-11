﻿namespace Application.Common.Interfaces.Email;

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string body);
}

