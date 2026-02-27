using HR.LeaveManagement.Application.Model.Email;

namespace HR.LeaveManagement.Application.Contracts.Email;

public interface IEmailSender
{
    Task<bool> SendEmailAsync(EmailMessage email);
}
