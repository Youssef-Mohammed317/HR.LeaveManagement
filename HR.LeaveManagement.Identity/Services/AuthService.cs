using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Model.Email;
using HR.LeaveManagement.Application.Model.Identity;
using HR.LeaveManagement.Domain.Identity;
using HR.LeaveManagement.Domain.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HR.LeaveManagement.Identity.Services;

public class AuthService(IOptions<JwtSettings> options,
    IEmailSender emailSender,
    ILogger<AuthService> logger,
    UserManager<ApplicationUser> userManager,

    SignInManager<ApplicationUser> signInManager) : IAuthService
{
    private readonly JwtSettings jwtSettings = options.Value;
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email)
            ?? throw new BadRequestException("Invalid credentials.");

        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded)
        {
            throw new BadRequestException($"Credentials for '{request.Email} aren't valid'.");
        }

        var token = await GenerateToken(user);
        _ = Task.Run(async () =>
        {
            try
            {
                await emailSender.SendEmailAsync(new EmailMessage
                {
                    To = user.Email!,
                    Subject = "Login Notification",
                    Body = $@"
                    Hello {user.UserName},

                    You have successfully logged in at {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC.

                    If this wasn't you, please contact support immediately.
                "
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send login email.");
            }
        });

        return new LoginResponse
        {
            Id = user.Id,
            Token = token,
            Email = user.Email!,
            UserName = user.UserName!
        };
    }



    public async Task<RegistrationResponse> RegisterAsync(RegistrationRequest request)
    {

        var checkUserEmail = await userManager.FindByEmailAsync(request.Email);

        if (checkUserEmail != null)
        {
            throw new BadRequestException($"This email: '{request.Email}' already exists");
        }
        var checkUserName = await userManager.FindByNameAsync(request.UserName);
        if (checkUserName != null)
        {
            throw new BadRequestException($"This username: '{request.Email}' already exists");
        }
        var user = new ApplicationUser
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName,

        };

        var result = await userManager.CreateAsync(user);

        if (!result.Succeeded)
        {
            var errors = string.Join(Environment.NewLine,
                result.Errors.Select(e => $"• {e.Description}"));

            throw new BadRequestException(errors);
        }


        var roleResult = await userManager.AddToRoleAsync(user, Roles.Employee);

        if (!roleResult.Succeeded)
        {
            await userManager.DeleteAsync(user);

            var errors = string.Join(Environment.NewLine,
                roleResult.Errors.Select(e => $"• {e.Description}"));

            throw new BadRequestException(errors);
        }
        _ = Task.Run(async () =>
        {
            try
            {
                await emailSender.SendEmailAsync(new EmailMessage
                {
                    To = user.Email!,
                    Body = $@"
                    <h2>Welcome to HR Leave Management System</h2>
                    <p>Hello <strong>{user.UserName}</strong>,</p>
                    <p>Your account has been successfully created.</p>
                    <p>You can now log in and manage your leave requests.</p>
                    <p>Registration Date: {DateTime.UtcNow:yyyy-MM-dd}</p>
                    <br/>
                    <p>Best regards,<br/>HR Team</p>
                "
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send welcome email.");
            }
        });
        return new RegistrationResponse(user.Id);
    }


    private async Task<string> GenerateToken(ApplicationUser user)
    {
        var userClaims = await userManager.GetClaimsAsync(user);
        var roles = await userManager.GetRolesAsync(user);

        var roleClaims = roles.Select(q => new Claim(ClaimTypes.Role, q)).ToList();

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier,user.Id),
            new(ClaimTypes.Email,user.Email!),
            new(JwtRegisteredClaimNames.Sub,user.UserName!),
            new(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
        }.Union(userClaims)
        .Union(roleClaims);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));

        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var token = new JwtSecurityToken(
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience,
            claims: claims,
            signingCredentials: cred,
            expires: DateTime.UtcNow.AddMinutes(jwtSettings.DurationInMinutes));

        var handler = new JwtSecurityTokenHandler();

        return handler.WriteToken(token);
    }
}
