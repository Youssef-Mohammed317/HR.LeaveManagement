using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Model.Email;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace HR.LeaveManagement.Infrastructure.EmailService;

public class BrevoEmailSender : IEmailSender
{
    private readonly BrevoEmailSettings emailSettings;
    private readonly HttpClient httpClient;
    private readonly ILogger<BrevoEmailSender> logger;

    public BrevoEmailSender(IOptions<BrevoEmailSettings> options, HttpClient httpClient, ILogger<BrevoEmailSender> logger)
    {
        this.emailSettings = options.Value;
        this.httpClient = httpClient;
        this.logger = logger;
        this.httpClient.BaseAddress = new Uri(this.emailSettings.BaseAddress);
        this.httpClient.DefaultRequestHeaders.Add(
            "api-key",
            this.emailSettings.ApiKey
        );
    }
    public async Task<bool> SendEmailAsync(EmailMessage email)
    {
        try
        {
            var body = new
            {
                sender = new
                {
                    email = emailSettings.FromEmail,
                    name = emailSettings.FromName
                },
                 to = new[]
                  {
                    new
                    {
                        email = email.To
                    }
                },
                subject = email.Subject,
                htmlContent = email.Body
            };

            var content = new StringContent(
                   JsonSerializer.Serialize(body),
                   Encoding.UTF8,
                   "application/json"
               );
            var response = await httpClient.PostAsync("", content);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send login email.");
            return false;
        }
    }
}
