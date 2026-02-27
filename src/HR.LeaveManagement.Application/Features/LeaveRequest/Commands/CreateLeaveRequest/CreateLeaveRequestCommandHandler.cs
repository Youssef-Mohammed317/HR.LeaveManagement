using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Presistance;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CreateLeaveRequest;

public class CreateLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository,
    ILeaveAllocationRepository leaveAllocationRepository,
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
            selector: s => EF.Functions.DateDiffDay(s.StartDate, s.EndDate) + 1,
            filter: q => q.LeaveTypeId == request.LeaveTypeId &&
            q.EmployeeId == userId &&
            q.Status == LeaveRequestStatus.Approved &&
              q.StartDate.Year == allocation.Period);

        var remainingDays = allocation.NumberOfDays - approvedDays;

        if (daysRequested > remainingDays)
        {
            throw new BadRequestException($"You do not have enough remaining days for this request. you have ({(int)remainingDays}) days remaining");
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

        return leaveRequest.Id;

    }
}