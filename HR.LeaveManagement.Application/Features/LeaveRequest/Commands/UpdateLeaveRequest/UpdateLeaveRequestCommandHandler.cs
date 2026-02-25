using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Presistance;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Model.Email;
using HR.LeaveManagement.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest;

public class UpdateLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository,
    ILeaveAllocationRepository leaveAllocationRepository,
    IEmailSender emailSender,
    ILogger<UpdateLeaveRequestCommand> logger,
    IUserService userService) : IRequestHandler<UpdateLeaveRequestCommand>
{
    public async Task Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        var userId = userService?.UserId
            ?? throw new ForbiddenAccessException();

        var leaveRequestEntity = await leaveRequestRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(Domain.LeaveRequest), request.Id);

        if (leaveRequestEntity.EmployeeId != userId)
            throw new ForbiddenAccessException();

        if (leaveRequestEntity.Status != LeaveRequestStatus.Pending)
            throw new BadRequestException("Only pending requests can be updated.");

        var currentYear = DateTime.UtcNow.Year;

        var allocation = await leaveAllocationRepository.GetFirstAsync(
         filter: q => q.LeaveTypeId == request.LeaveTypeId && q.EmployeeId == userId && q.Period == currentYear)
         ?? throw new BadRequestException("You do not have any allocations for this leave type.");



        var approvedDays = await leaveRequestRepository.SumAsync(
            selector: s => (s.EndDate - s.StartDate).Days + 1,
            filter: q => q.LeaveTypeId == request.LeaveTypeId &&
                  q.EmployeeId == userId &&
                  q.Status == LeaveRequestStatus.Approved &&
                  q.StartDate.Year == allocation.Period &&
                  q.Id != request.Id);


        var daysRequested = (int)(request.EndDate - request.StartDate).TotalDays + 1;


        var remainingDays = allocation.NumberOfDays - approvedDays;

        if (daysRequested > remainingDays)
        {
            throw new BadRequestException("You do not have enough remaining days for this request.");
        }


        leaveRequestEntity.StartDate = request.StartDate;
        leaveRequestEntity.EndDate = request.EndDate;
        leaveRequestEntity.LeaveTypeId = request.LeaveTypeId;
        leaveRequestEntity.RequestComments = request.RequestComments;

        await leaveRequestRepository.UpdateAsync(leaveRequestEntity);

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
            logger.LogWarning(ex, "Failed to send update leave request email");
        }


    }
}
