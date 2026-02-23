using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Model;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace HR.LeaveManagement.Infrastructure.EmailService;

public class BrevoEmailSender : IEmailSender
{
    private readonly BrevoEmailSettings emailSettings;
    private readonly HttpClient httpClient;

    public BrevoEmailSender(IOptions<BrevoEmailSettings> options, HttpClient httpClient)
    {
        this.emailSettings = options.Value;
        this.httpClient = httpClient;
        this.httpClient.BaseAddress = new Uri(this.emailSettings.BaseAddress);
        this.httpClient.DefaultRequestHeaders.Add(
            "api-key",
            this.emailSettings.ApiKey
        );
    }
    public async Task<bool> SendEmail(EmailMessage email)
    {
        var body = new
        {
            htmlContent = email.Body,
            sender = new
            {
                email = email.To,
            },
            subject = email.Subject,
            to = new[]
               {
                new
                {
                    email = emailSettings.FromAddress,
                    name = emailSettings.FromName
                }
            }
        };

        var content = new StringContent(
               JsonSerializer.Serialize(body),
               Encoding.UTF8,
               "application/json"
           );
        var response = await httpClient.PostAsync("", content);

        return response.IsSuccessStatusCode;
    }
}
