using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Presistance;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

        var leaveRequest = await leaveRequestRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(Domain.LeaveRequest), request.Id);

        if (leaveRequest.EmployeeId != userId)
            throw new ForbiddenAccessException();

        if (leaveRequest.Status != LeaveRequestStatus.Pending)
            throw new BadRequestException("Only pending requests can be updated.");

        var currentYear = DateTime.UtcNow.Year;

        var allocation = await leaveAllocationRepository.GetFirstAsync(
         filter: q => q.LeaveTypeId == request.LeaveTypeId && q.EmployeeId == userId && q.Period == currentYear)
         ?? throw new BadRequestException("You do not have any allocations for this leave type.");



        var approvedDays = await leaveRequestRepository.SumAsync(
            selector: s => EF.Functions.DateDiffDay(s.StartDate, s.EndDate) + 1,
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


        leaveRequest.StartDate = request.StartDate;
        leaveRequest.EndDate = request.EndDate;
        leaveRequest.LeaveTypeId = request.LeaveTypeId;
        leaveRequest.RequestComments = request.RequestComments;


        await leaveRequestRepository.UpdateAsync(leaveRequest);


    }
}
