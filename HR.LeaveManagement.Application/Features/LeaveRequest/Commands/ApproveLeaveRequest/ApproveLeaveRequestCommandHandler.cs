using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Presistance;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Model.Email;
using HR.LeaveManagement.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.ApproveLeaveRequest;

public class ApproveLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository,
    ILeaveAllocationRepository leaveAllocationRepository,
    IUserService userService) : IRequestHandler<ApproveLeaveRequestCommand>
{
    public async Task Handle(ApproveLeaveRequestCommand request, CancellationToken cancellationToken)
    {

        if (!userService.IsAdmin)
            throw new ForbiddenAccessException();

        var leaveRequest = await leaveRequestRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(LeaveRequest), request.Id);


        if (leaveRequest.Status != LeaveRequestStatus.Pending)
            throw new BadRequestException("Only pending requests can be approved.");


        var daysRequested = (leaveRequest.EndDate - leaveRequest.StartDate).Days + 1;

        var allocation = await leaveAllocationRepository.GetFirstAsync(
            q => q.EmployeeId == leaveRequest.EmployeeId &&
                 q.LeaveTypeId == leaveRequest.LeaveTypeId &&
                 q.Period == leaveRequest.StartDate.Year)
            ?? throw new BadRequestException("No allocation found for this employee.");

        var approvedDays = await leaveRequestRepository.SumAsync(
                   s => EF.Functions.DateDiffDay(s.StartDate, s.EndDate) + 1,
                   q => q.EmployeeId == leaveRequest.EmployeeId &&
                        q.LeaveTypeId == leaveRequest.LeaveTypeId &&
                        q.Status == LeaveRequestStatus.Approved);

        var remainingDays = allocation.NumberOfDays - approvedDays;

        if (daysRequested > remainingDays)
            throw new BadRequestException("Employee does not have enough remaining days.");

        leaveRequest.Status = LeaveRequestStatus.Approved;

        await leaveRequestRepository.UpdateAsync(leaveRequest);

    }
}