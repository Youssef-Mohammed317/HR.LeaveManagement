using HR.LeaveManagement.Application.Model;

namespace HR.LeaveManagement.Application.Contracts.Email;

public interface IEmailSender
{
    Task<bool> SendEmail(EmailMessage email);
}
