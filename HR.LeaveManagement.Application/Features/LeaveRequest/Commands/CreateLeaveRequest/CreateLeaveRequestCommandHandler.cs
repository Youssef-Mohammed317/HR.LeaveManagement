using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Presistance;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Model.Email;
using HR.LeaveManagement.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CreateLeaveRequest;

public class CreateLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository,
    ILeaveAllocationRepository leaveAllocationRepository,
    IEmailSender emailSender,
    ILogger<CreateLeaveRequestCommand> logger,
    IUserService userService) : IRequestHandler<CreateLeaveRequestCommand, int>
{
    public async Task<int> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var userId = userService?.UserId
            ?? throw new ForbiddenAccessException();

        var currentYear = DateTime.UtcNow.Year;

        var allocation = await leaveAllocationRepository.GetFirstAsync(
            filter: q => q.LeaveTypeId == request.LeaveTypeId && q.EmployeeId == userId && q.Period == currentYear)
            ?? throw new BadRequestException("You do not have any allocations for this leave type.");

        var daysRequested = (int)(request.EndDate - request.StartDate).TotalDays + 1;

        var approvedDays = await leaveRequestRepository.SumAsync(
            selector: s => (s.EndDate - s.StartDate).Days + 1,
            filter: q => q.LeaveTypeId == request.LeaveTypeId &&
            q.EmployeeId == userId &&
            q.Status == LeaveRequestStatus.Approved &&
              q.StartDate.Year == allocation.Period);

        var remainingDays = allocation.NumberOfDays - approvedDays;

        if (daysRequested > remainingDays)
        {
            throw new BadRequestException("You do not have enough remaining days for this request.");
        }

        var leaveRequest = new Domain.LeaveRequest
        {
            EmployeeId = userId,
            LeaveTypeId = request.LeaveTypeId,
            RequestComments = request.RequestComments,
            Status = LeaveRequestStatus.Pending,
            DateRequested = DateTime.UtcNow,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            
        };
        await leaveRequestRepository.CreateAsync(leaveRequest);

        try
        {
            var email = new EmailMessage
            {
                To = userService.Email!,
                Body = $"Your leave request for {request.StartDate:D} to {request.EndDate:D} " +
                    $"has been submitted successfully.",
                Subject = "Leave Request Submitted"
            };

            await emailSender.SendEmailAsync(email);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to send create leave request email");
        }

        return leaveRequest.Id;

    }
}